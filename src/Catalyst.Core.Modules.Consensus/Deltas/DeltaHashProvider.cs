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
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Catalyst.Abstractions.Consensus.Deltas;
using Catalyst.Abstractions.Sync.Interfaces;
using Catalyst.Core.Abstractions.Sync;
using Catalyst.Core.Lib.Extensions;
using Catalyst.Core.Modules.Sync.Modal;
using Google.Protobuf.WellKnownTypes;
using Lib.P2P;
using Nito.Comparers;
using Serilog;

namespace Catalyst.Core.Modules.Consensus.Deltas
{
    /// <inheritdoc />
    public class DeltaHashProvider : IDeltaHashProvider
    {
        private readonly IDeltaCache _deltaCache;
        private readonly ILogger _logger;

        private readonly ReplaySubject<Cid> _deltaHashUpdatesSubject;
        private readonly SortedList<Timestamp, Cid> _hashesByTimeDescending;
        private readonly int _capacity;
        private readonly IDeltaHeightWatcher _deltaHeightWatcher;
        private readonly SyncState _syncState;

        public IObservable<Cid> DeltaHashUpdates => _deltaHashUpdatesSubject.AsObservable();

        public DeltaHashProvider(IDeltaCache deltaCache,
            //IDeltaHeightWatcher deltaHeightWatcher,
            SyncState syncState,
            ILogger logger,
            int capacity = 10_000)
        {
            _deltaCache = deltaCache;
            _logger = logger;
            _deltaHashUpdatesSubject = new ReplaySubject<Cid>(0);
            var comparer = ComparerBuilder.For<Timestamp>().OrderBy(u => u, descending: true);
            _capacity = capacity;
            _hashesByTimeDescending = new SortedList<Timestamp, Cid>(comparer)
            {
                Capacity = _capacity
            };

            _hashesByTimeDescending.Add(Timestamp.FromDateTime(new DateTime(2020, 1, 1, 0, 0, 0).ToUniversalTime()),
                _deltaCache.GenesisHash);

            //_deltaHeightWatcher = deltaHeightWatcher;
            _syncState = syncState;
        }

        /// <inheritdoc />
        public bool TryUpdateLatestHash(Cid previousHash, Cid newHash)
        {
            var newAddress = newHash;
            var previousAddress = previousHash;
            _logger.Debug("New hash {hash} received for previous hash {previousHash}",
                newAddress, previousAddress);
            var foundNewDelta = _deltaCache.TryGetOrAddConfirmedDelta(newAddress, out var newDelta);
            var foundPreviousDelta = _deltaCache.TryGetOrAddConfirmedDelta(previousAddress, out var previousDelta);

            if (!foundNewDelta
             || !foundPreviousDelta
             || newDelta.PreviousDeltaDfsHash != previousHash.ToArray().ToByteString()
             || previousDelta.TimeStamp >= newDelta.TimeStamp)
            {
                _logger.Warning("Failed to update latest hash from {previousHash} to {newHash}",
                    previousAddress, newAddress);
                return false;
            }

            _logger.Debug("Successfully to updated latest hash from {previousHash} to {newHash}",
                previousAddress, newAddress);


            //if (!_syncState.IsSynchronized)
            //{
            //    _deltaHeightWatcher.LatestDeltaHash = newDelta.
            //}

            lock (_hashesByTimeDescending)
            {
                if (_hashesByTimeDescending.ContainsValue(newHash))
                {
                    return false;
                }

                _hashesByTimeDescending.Add(newDelta.TimeStamp, newHash);
                if (_hashesByTimeDescending.Count > _capacity)
                {
                    _hashesByTimeDescending.RemoveAt(_capacity);
                }
            }

            _deltaHashUpdatesSubject.OnNext(newAddress);

            return true;
        }

        /// <inheritdoc />
        public Cid GetLatestDeltaHash(DateTime? asOf = null)
        {
            _logger.Verbose("Trying to retrieve latest delta as of {asOf}", asOf);
            if (!asOf.HasValue)
            {
                return _hashesByTimeDescending.FirstOrDefault().Value;
            }

            var timestamp = Timestamp.FromDateTime(asOf.Value);
            var hash = _hashesByTimeDescending
               .SkipWhile(p => p.Key > timestamp)
               .FirstOrDefault();

            //todo: do we want to start walking down
            //the history of hashes and get them from IPFS
            //if they are not found here?
            //https://github.com/catalyst-network/Catalyst.Node/issues/615
            return hash.Value;
        }
    }
}
