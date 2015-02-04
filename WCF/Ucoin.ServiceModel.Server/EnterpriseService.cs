using System;
using System.Linq;
using System.ServiceProcess;
using System.Diagnostics;
//using Ucoin.ServiceModel.Runtime;
using Common.Logging;
using Ucoin.ServiceModel.Server.Runtime;

namespace Ucoin.ServiceModel.Server
{
    public class EnterpriseService : ServiceBase
    {
        private readonly ILog log = LogManager.GetLogger(typeof(EnterpriseService));

        private IEventListener Listener
        {
            get { return null; }
        }

        private void DoStart(string[] args)
        {
            if (Environment.UserInteractive)
            {
                StartByUserInteractive(args);
            }
            else
            {
                var ServicesToRun = new ServiceBase[] {this};
                ServiceBase.Run(ServicesToRun);
            }
        }

        private void StartByUserInteractive(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var ls = new ConsoleTraceListener();
            if (args.Any(c => c == "-l"))
            {
                Trace.Listeners.Add(ls);
            }
            OnStart(args);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("all services are ready...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("press X to exit.");
            Console.ForegroundColor = ConsoleColor.Green;
            var key = Console.ReadKey();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                if (key.Key == ConsoleKey.X)
                {
                    break;
                }
            }
            Stop();
        }

        protected void Start(string[] args)
        {
            try
            {
                DoStart(args);
            }
            catch (Exception ex)
            {
                if (Environment.UserInteractive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error:\r\n=====================================");
                    Console.WriteLine(ex.Message);

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("press any key to exit.");
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            log.Info("service starting");

            new ServiceConsole().StartServices(
                Listener,
                delegate(Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("start error:" + ex.Message);
                    log.Error("service started has error:" + ex.Message, ex);
                    Console.ForegroundColor = ConsoleColor.Green;
                });

            log.Info("service started");
        }

        protected override void OnStop()
        {
            base.OnStop();
            log.Info("service stoped");
        }

        protected override void OnPause()
        {
            base.OnPause();
            log.Info("service paused");
        }
    }
}
