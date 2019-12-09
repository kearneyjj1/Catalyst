﻿using System.Linq;
using System.Threading.Tasks;
using Catalyst.Abstractions.Dfs;
using Catalyst.Core.Modules.Dfs.Tests.Utils;
using MultiFormats;
using Xunit;
using Xunit.Abstractions;

namespace Catalyst.Core.Modules.Dfs.Tests.IntegrationTests.CoreApi
{
    public class BootstapApiTest
    {
        private IDfsService ipfs;
        MultiAddress somewhere = "/ip4/127.0.0.1/tcp/4009/ipfs/QmPv52ekjS75L4JmHpXVeuJ5uX2ecSfSZo88NSyxwA3rAQ";

        public BootstapApiTest(ITestOutputHelper output)
        {
            ipfs = TestDfs.GetTestDfs(output);    
        }
        
        [Fact]
        public async Task Add_Remove()
        {
            var addr = await ipfs.BootstrapApi.AddAsync(somewhere);
            Assert.NotNull(addr);
            Assert.Equal(somewhere, addr);
            var addrs = await ipfs.BootstrapApi.ListAsync();
            Assert.True(addrs.Any(a => a == somewhere));

            addr = await ipfs.BootstrapApi.RemoveAsync(somewhere);
            Assert.NotNull(addr);
            Assert.Equal(somewhere, addr);
            addrs = await ipfs.BootstrapApi.ListAsync();
            Assert.False(addrs.Any(a => a == somewhere));
        }

        [Fact]
        public async Task List()
        {
            var addrs = await ipfs.BootstrapApi.ListAsync();
            Assert.NotNull(addrs);
            Assert.NotEqual(0, addrs.Count());
        }

        [Fact]
        public async Task Remove_All()
        {
            var original = await ipfs.BootstrapApi.ListAsync();
            await ipfs.BootstrapApi.RemoveAllAsync();
            var addrs = await ipfs.BootstrapApi.ListAsync();
            Assert.Equal(0, addrs.Count());
            foreach (var addr in original)
            {
                await ipfs.BootstrapApi.AddAsync(addr);
            }
        }

        [Fact]
        public async Task Add_Defaults()
        {
            var original = await ipfs.BootstrapApi.ListAsync();
            await ipfs.BootstrapApi.RemoveAllAsync();
            try
            {
                await ipfs.BootstrapApi.AddDefaultsAsync();
                var addrs = await ipfs.BootstrapApi.ListAsync();
                Assert.NotEqual(0, addrs.Count());
            }
            finally
            {
                await ipfs.BootstrapApi.RemoveAllAsync();
                foreach (var addr in original)
                {
                    await ipfs.BootstrapApi.AddAsync(addr);
                }
            }
        }

        [Fact]
        public async Task Override_FactoryDefaults()
        {
            var original = ipfs.Options.Discovery.BootstrapPeers;
            try
            {
                ipfs.Options.Discovery.BootstrapPeers = new MultiAddress[0];
                var addrs = await ipfs.BootstrapApi.ListAsync();
                Assert.Equal(0, addrs.Count());

                ipfs.Options.Discovery.BootstrapPeers = new MultiAddress[1]
                    {somewhere};
                addrs = await ipfs.BootstrapApi.ListAsync();
                Assert.Equal(1, addrs.Count());
                Assert.Equal(somewhere, addrs.First());
            }
            finally
            {
                ipfs.Options.Discovery.BootstrapPeers = original;
            }
        }
    }
}