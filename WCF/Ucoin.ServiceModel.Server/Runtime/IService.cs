using System.ServiceModel.Description;

namespace Ucoin.ServiceModel.Server.Runtime
{
    public interface IService
    {
        ServiceInfo Info { get; }

        RunState RunState { get; }

        ServiceEndpoint[] ServiceEndpointes { get; }

        void Run();
    }
}
