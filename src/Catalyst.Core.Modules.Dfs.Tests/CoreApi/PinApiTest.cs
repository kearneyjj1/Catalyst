﻿using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Catalyst.Abstractions.Dfs;
using Catalyst.Abstractions.Dfs.CoreApi;
using Catalyst.Abstractions.Options;
using FluentAssertions;
using Lib.P2P;
using MultiFormats;
using Xunit;
using Xunit.Abstractions;

namespace Catalyst.Core.Modules.Dfs.Tests.CoreApi
{
    public class PinApiTest
    {
        private IDfs ipfs;

        public PinApiTest(ITestOutputHelper output)
        {
            ipfs = new TestFixture(output).Ipfs;      
        }

        [Fact]
        public async Task Add_Remove()
        {
            var result = await ipfs.FileSystem.AddTextAsync("I am pinned");
            var id = result.Id;

            var pins = await ipfs.Pin.AddAsync(id);
            Assert.True(pins.Any(pin => pin == id));
            var all = await ipfs.Pin.ListAsync();
            Assert.True(all.Any(pin => pin == id));

            pins = await ipfs.Pin.RemoveAsync(id);
            Assert.True(pins.Any(pin => pin == id));
            all = await ipfs.Pin.ListAsync();
            Assert.False(all.Any(pin => pin == id));
        }

        [Fact]
        public async Task Remove_Unknown()
        {
            var dag = new DagNode(Encoding.UTF8.GetBytes("some unknown info for net-ipfs-engine-pin-test"));
            await ipfs.Pin.RemoveAsync(dag.Id, true);
        }

        [Fact]
        public async Task Inline_Cid()
        {
            var cid = new Cid
            {
                ContentType = "raw",
                Hash = MultiHash.ComputeHash(new byte[]
                {
                    1, 2, 3
                }, "identity")
            };
            var pins = await ipfs.Pin.AddAsync(cid, recursive: false);
            Assert.Contains(cid, pins.ToArray());
            var all = await ipfs.Pin.ListAsync();
            Assert.Contains(cid, all.ToArray());

            var removals = await ipfs.Pin.RemoveAsync(cid, recursive: false);
            Assert.Contains(cid, removals.ToArray());
            all = await ipfs.Pin.ListAsync();
            Assert.DoesNotContain(cid, all.ToArray());
        }

        [Fact]
        public void Add_Unknown()
        {
            var dag = new DagNode(Encoding.UTF8.GetBytes("some unknown info for net-ipfs-engine-pin-test"));
            ExceptionAssert.Throws<Exception>(() =>
            {
                var cts = new CancellationTokenSource(250);
                var _ = ipfs.Pin.AddAsync(dag.Id, true, cts.Token).Result;
            });
        }

        [Fact]
        public async Task Add_Recursive()
        {
            var options = new AddFileOptions
            {
                ChunkSize = 3,
                Pin = false,
                RawLeaves = true,
                Wrap = true,
            };
            var node = await ipfs.FileSystem.AddTextAsync("hello world", options);
            var cids = await ipfs.Pin.AddAsync(node.Id, true);
            Assert.Equal(6, cids.Count());
        }

        [Fact]
        public async Task Remove_Recursive()
        {
            var options = new AddFileOptions
            {
                ChunkSize = 3,
                Pin = false,
                RawLeaves = true,
                Wrap = true,
            };
            var node = await ipfs.FileSystem.AddTextAsync("hello world", options);
            var cids = await ipfs.Pin.AddAsync(node.Id, true);
            Assert.Equal(6, cids.Count());

            var removedCids = await ipfs.Pin.RemoveAsync(node.Id, true);
            Assert.Equal(cids.ToArray(), removedCids.ToArray());
        }
    }
}