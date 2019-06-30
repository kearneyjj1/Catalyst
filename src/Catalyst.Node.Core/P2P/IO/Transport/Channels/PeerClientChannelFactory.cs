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

using System.Collections.Generic;
using System.Net;
using System.Reactive.Linq;
using System.Security.Cryptography.X509Certificates;
using Catalyst.Common.Interfaces.IO.EventLoop;
using Catalyst.Common.Interfaces.IO.Messaging;
using Catalyst.Common.Interfaces.IO.Messaging.Dto;
using Catalyst.Common.Interfaces.IO.Transport.Channels;
using Catalyst.Common.Interfaces.Modules.KeySigner;
using Catalyst.Common.IO.Handlers;
using Catalyst.Common.IO.Transport.Channels;
using Catalyst.Protocol.Common;
using DotNetty.Transport.Channels;
using Serilog;

namespace Catalyst.Node.Core.P2P.IO.Transport.Channels
{
    public class PeerClientChannelFactory : UdpClientChannelFactory
    {
        private readonly IKeySigner _keySigner;
        private readonly IMessageCorrelationManager _correlationManager;
        private readonly ILogger _logger;

        protected override List<IChannelHandler> Handlers =>
            new List<IChannelHandler>
            {
                new CombinedChannelDuplexHandler<IChannelHandler, IChannelHandler>(
                    new DatagramProtobufDecoder(_logger), new DatagramProtobufEncoder(_logger)
                ),
                new CombinedChannelDuplexHandler<IChannelHandler, IChannelHandler>(
                    new ProtocolMessageVerifyHandler(_keySigner, _logger), new ProtocolMessageSignHandler(_keySigner, _logger)
                ),
                new CombinedChannelDuplexHandler<IChannelHandler, IChannelHandler>(
                    new CorrelationHandler(_correlationManager, _logger), new CorrelationHandler(_correlationManager, _logger)
                )
            };
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keySigner"></param>
        /// <param name="correlationManager"></param>
        /// <param name="logger"></param>
        public PeerClientChannelFactory(IKeySigner keySigner,
            IMessageCorrelationManager correlationManager,
            ILogger logger)
        {
            _keySigner = keySigner;
            _correlationManager = correlationManager;
            _logger = logger;
        }
        
        /// <param name="handlerEventLoopGroupFactory"></param>
        /// <param name="targetAddress">Ignored</param>
        /// <param name="targetPort">Ignored</param>
        /// <param name="certificate">Local TLS certificate</param>
        public override IObservableChannel BuildChannel(IEventLoopGroupFactory handlerEventLoopGroupFactory,
            IPAddress targetAddress,
            int targetPort,
            X509Certificate2 certificate = null)
        {
            var channel = BootStrapChannel(handlerEventLoopGroupFactory, targetAddress, targetPort);

            return new ObservableChannel(Observable.Never<IObserverDto<ProtocolMessage>>(), channel);
        }
    }
}
