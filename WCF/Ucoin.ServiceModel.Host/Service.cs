using Ucoin.ServiceModel.Server;

namespace Ucoin.Service.Host
{
    public class Service : EnterpriseService
    {
        private static void Main(string[] args)
        {
            var service = new Service();
            service.Start(args);
        }
    }
}
