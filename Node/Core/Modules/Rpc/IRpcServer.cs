using System.Threading.Tasks;
using ADL.Protocol.Rpc.Node;
using Grpc.Core;

namespace ADL.Node.Core.Modules.Rpc
{
    public interface IRpcServer
    {
        Task<PongResponse> Ping(PingRequest request, ServerCallContext context);
        Task<VersionResponse> Version(VersionRequest request, ServerCallContext context);
    }
}