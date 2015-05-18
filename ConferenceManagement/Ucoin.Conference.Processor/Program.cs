using System;
namespace Ucoin.Conference.Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseSetup.Initialize();

            using (var processor = new ConferenceProcessor(false))
            {
                processor.Start();

                Console.WriteLine("Host started");
                Console.WriteLine("Press enter to finish");
                Console.ReadLine();

                processor.Stop();
            }
        }
    }
}
