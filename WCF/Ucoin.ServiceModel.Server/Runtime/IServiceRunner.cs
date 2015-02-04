namespace Ucoin.ServiceModel.Server.Runtime
{
    public interface IServiceRunner
    {
        void Load(ServicePackage package);

        void Run( IEventListener listener);
    }
}
