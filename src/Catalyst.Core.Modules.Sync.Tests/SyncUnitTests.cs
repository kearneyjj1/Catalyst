#region LICENSE

/**
* Copyright (c) 2019 Catalyst Network
*
* This file is part of Catalyst.Node <https://github.com/catalyst-network/Catalyst.Node>
*
* Catalyst.Node is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 2 of the License, or
* (at your option) any later version.
*
* Catalyst.Node is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with Catalyst.Node. If not, see <https://www.gnu.org/licenses/>.
*/

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Catalyst.Abstractions.Cli;
using Catalyst.Abstractions.Consensus.Deltas;
using Catalyst.Abstractions.Hashing;
using Catalyst.Abstractions.Ledger;
using Catalyst.Abstractions.P2P;
using Catalyst.Abstractions.P2P.IO;
using Catalyst.Abstractions.P2P.IO.Messaging.Dto;
using Catalyst.Core.Lib.Cli;
using Catalyst.Core.Lib.DAO;
using Catalyst.Core.Lib.DAO.Ledger;
using Catalyst.Core.Lib.Extensions;
using Catalyst.Core.Lib.IO.Messaging.Correlation;
using Catalyst.Core.Lib.IO.Messaging.Dto;
using Catalyst.Core.Lib.P2P.IO.Messaging.Dto;
using Catalyst.Core.Lib.P2P.Repository;
using Catalyst.Core.Modules.Dfs.Extensions;
using Catalyst.Core.Modules.Hashing;
using Catalyst.Protocol.Deltas;
using Catalyst.Protocol.IPPN;
using Catalyst.Protocol.Peer;
using Catalyst.TestUtils;
using FluentAssertions;
using Google.Protobuf;
using Lib.P2P;
using MultiFormats.Registry;
using Newtonsoft.Json;
using NSubstitute;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository;
using Xunit;
using Peer = Catalyst.Core.Lib.P2P.Models.Peer;

namespace Catalyst.Core.Modules.Sync.Tests
{
    public class SyncUnitTests
    {
        private readonly IHashProvider _hashProvider;
        private readonly IPeerSettings _peerSettings;
        private readonly IDeltaDfsReader _deltaDfsReader;
        private readonly ILedger _ledger;
        private readonly IPeerClient _peerClient;
        private IDeltaIndexService _deltaIndexService;
        private readonly IPeerRepository _peerRepository;

        private readonly IPeerClientObservable _deltaHeightResponseObserver;
        private readonly ReplaySubject<IPeerClientMessageDto> _deltaHeightReplaySubject;

        private readonly IPeerClientObservable _deltaHistoryResponseObserver;
        private readonly ReplaySubject<IPeerClientMessageDto> _deltaHistoryReplaySubject;

        private readonly IMapperProvider _mapperProvider;
        private readonly IUserOutput _userOutput;

        private readonly IPeerSyncManager _peerSyncManager;
        private readonly IDeltaHeightWatcher _deltaHeightWatcher;

        public SyncUnitTests()
        {
            _hashProvider = new HashProvider(HashingAlgorithm.GetAlgorithmMetadata("blake2b-256"));

            _peerSettings = Substitute.For<IPeerSettings>();
            _peerSettings.PeerId.Returns(PeerIdHelper.GetPeerId());

            _deltaDfsReader = Substitute.For<IDeltaDfsReader>();
            _deltaDfsReader.TryReadDeltaFromDfs(Arg.Any<Cid>(), out Arg.Any<Delta>()).Returns(x => true);

            _ledger = Substitute.For<ILedger>();

            _deltaIndexService = new DeltaIndexService(new InMemoryRepository<DeltaIndexDao, string>());

            _peerClient = Substitute.For<IPeerClient>();
            ModifyPeerClient<LatestDeltaHashRequest>((request, senderPeerIdentifier) =>
            {
                var deltaHeightResponse = new LatestDeltaHashResponse
                {
                    Result = new DeltaIndex
                    {
                        Cid = _hashProvider.ComputeUtf8MultiHash("1000").ToCid().ToArray().ToByteString(),
                        Height = 1000
                    }
                };

                var peerClientMessageDto = new PeerClientMessageDto(deltaHeightResponse,
                    senderPeerIdentifier,
                    CorrelationId.GenerateCorrelationId());
                _deltaHeightReplaySubject.OnNext(peerClientMessageDto);
            });

            _peerRepository = Substitute.For<IPeerRepository>();
            var peers = new List<Peer>
            {
                new Peer {PeerId = PeerIdHelper.GetPeerId()}
            };
            _peerRepository.GetActivePeers(Arg.Any<int>()).Returns(peers);

            _deltaHeightResponseObserver = Substitute.For<IPeerClientObservable>();
            _deltaHeightReplaySubject = new ReplaySubject<IPeerClientMessageDto>(1);
            _deltaHeightResponseObserver.MessageStream.Returns(_deltaHeightReplaySubject);

            _deltaHistoryResponseObserver = Substitute.For<IPeerClientObservable>();
            _deltaHistoryReplaySubject = new ReplaySubject<IPeerClientMessageDto>(1);
            _deltaHistoryResponseObserver.MessageStream.Returns(_deltaHistoryReplaySubject);

            _mapperProvider = new TestMapperProvider();

            _userOutput = new ConsoleUserOutput();

            _peerSyncManager = new PeerSyncManager(_peerSettings, _peerClient, _peerRepository);
            _deltaHeightWatcher =
                new DeltaHeightWatcher(_peerSyncManager, _deltaHeightResponseObserver, _mapperProvider);
        }

        private DeltaHistoryResponse GenerateSampleData(int height, int range)
        {
            var deltaHeightResponse = new DeltaHistoryResponse();
            var deltaIndexList = new List<DeltaIndex>();
            for (var i = height; i <= height + range; i++)
            {
                deltaIndexList.Add(new DeltaIndex
                {
                    Cid = ByteString.CopyFrom(_hashProvider.ComputeUtf8MultiHash(i.ToString()).ToCid().ToArray()),
                    Height = (uint) i
                });
            }

            deltaHeightResponse.Result.Add(deltaIndexList);
            return deltaHeightResponse;
        }

        private void ModifyPeerClient<TRequest>(Action<TRequest, PeerId> callback) where TRequest : IMessage<TRequest>
        {
            _peerClient.When(x =>
                x.SendMessage(
                    Arg.Is<MessageDto>(y => y.Content.TypeUrl.EndsWith(typeof(TRequest).Name)))).Do(
                z =>
                {
                    var messageDto = (MessageDto) z[0];
                    callback.Invoke(messageDto.Content.FromProtocolMessage<TRequest>(), messageDto.SenderPeerIdentifier);
                });
        }

        [Fact]
        public async Task Can_Restore_DeltaIndex()
        {
            const int sampleCurrentDeltaHeight = 100;
            _deltaIndexService = Substitute.For<IDeltaIndexService>();
            _deltaIndexService.Height().Returns(sampleCurrentDeltaHeight);

            var sync = new Sync(_peerSyncManager, Substitute.For<IDeltaHeightWatcher>(), _ledger, _deltaDfsReader,
                _deltaIndexService,
                _deltaHistoryResponseObserver, _mapperProvider, _userOutput);

            await sync.StartAsync();

            sync.CurrentDeltaIndex.Should().Be(sampleCurrentDeltaHeight);
        }

        [Fact]
        public async Task Sync_Can_Request_DeltaIndexRange_From_Peer()
        {
            var firstResponseReceived = false;

            ModifyPeerClient<DeltaHistoryRequest>((request, senderPeerIdentifier) =>
            {
                if (firstResponseReceived)
                {
                    return;
                }

                var peerClientMessageDto = new PeerClientMessageDto(GenerateSampleData(0, 10),
                    senderPeerIdentifier,
                    CorrelationId.GenerateCorrelationId());
                _deltaHistoryReplaySubject.OnNext(peerClientMessageDto);
                firstResponseReceived = true;
            });

            var sync = new Sync(_peerSyncManager, _deltaHeightWatcher, _ledger, _deltaDfsReader, _deltaIndexService,
                _deltaHistoryResponseObserver, _mapperProvider, _userOutput);

            await sync.StartAsync();

            sync.CurrentDeltaIndex.Should().Be(10);
        }

        [Fact]
        public async Task Sync_Can_Add_DeltaIndexRange_To_Memory()
        {
            var firstResponseReceived = false;
            var deltaHeightResponse = GenerateSampleData(0, 10);

            ModifyPeerClient<DeltaHistoryRequest>((request, senderPeerIdentifier) =>
            {
                if (firstResponseReceived)
                {
                    return;
                }

                var peerClientMessageDto = new PeerClientMessageDto(deltaHeightResponse,
                    senderPeerIdentifier,
                    CorrelationId.GenerateCorrelationId());
                _deltaHistoryReplaySubject.OnNext(peerClientMessageDto);
                firstResponseReceived = true;
            });

            var sync = new Sync(_peerSyncManager, _deltaHeightWatcher, _ledger, _deltaDfsReader, _deltaIndexService,
                _deltaHistoryResponseObserver, _mapperProvider, _userOutput);

            await sync.StartAsync();

            _peerRepository.GetAll().Should().AllBeEquivalentTo(deltaHeightResponse);
        }

        [Fact]
        public async Task Sync_Can_Update_CurrentDeltaIndex_From_Requested_DeltaIndexRange()
        {
            var firstResponseReceived = false;

            ModifyPeerClient<DeltaHistoryRequest>((request, senderPeerIdentifier) =>
            {
                if (firstResponseReceived)
                {
                    return;
                }

                var peerClientMessageDto = new PeerClientMessageDto(GenerateSampleData(0, 10),
                    senderPeerIdentifier,
                    CorrelationId.GenerateCorrelationId());
                _deltaHistoryReplaySubject.OnNext(peerClientMessageDto);
                firstResponseReceived = true;
            });

            var sync = new Sync(_peerSyncManager, _deltaHeightWatcher, _ledger, _deltaDfsReader, _deltaIndexService,
                _deltaHistoryResponseObserver, _mapperProvider, _userOutput);

            await sync.StartAsync();

            sync.CurrentDeltaIndex.Should().Be(10);
        }

        //[Fact]
        //public void Sync_Can_Update_LatestDeltaHeight_From_Requested_DeltaIndexRange() { }

        [Fact]
        public async Task Sync_Can_Complete()
        {
            var sync = new Sync(_peerSyncManager, _deltaHeightWatcher, _ledger, _deltaDfsReader, _deltaIndexService,
                _deltaHistoryResponseObserver, _mapperProvider, _userOutput);

            ModifyPeerClient<DeltaHistoryRequest>((request, senderPeerIdentifier) =>
            {
                if (sync.IsSynchronized)
                {
                    return;
                }

                var peerClientMessageDto = new PeerClientMessageDto(GenerateSampleData((int) request.Height, (int) request.Range),
                    senderPeerIdentifier,
                    CorrelationId.GenerateCorrelationId());
                _deltaHistoryReplaySubject.OnNext(peerClientMessageDto);
            });

            await sync.StartAsync();

            sync.CurrentDeltaIndex.Should().Be(10);
        }
    }
}
