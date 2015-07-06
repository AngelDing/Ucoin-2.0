using Microsoft.Owin.Hosting;
using System;
using Thinktecture.IdentityServer.Core.Logging;

namespace Ucoin.Identity.ServerHost
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Identity Server SelfHost";
            LogProvider.SetCurrentLogProvider(new DiagnosticsTraceLogProvider());

            const string url = "https://localhost:44333/core";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("\n\nServer listening at {0}. Press enter to stop", url);
                Console.ReadLine();
            }
        }
    }
}
