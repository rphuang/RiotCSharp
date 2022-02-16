using RiotService;

namespace RiotServiceWin
{

    class Program
    {
        /// <summary>
        /// Command line for self hosting the web service
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var listeningOn = args.Length == 0 ? "http://*:8000/" : args[0];

            var appHost = new AppHost()
                .Init()
                .Start(listeningOn);

            Console.WriteLine("AppHost Created at {0}, listening on {1}",
                DateTime.Now, listeningOn);

            Console.ReadKey();
        }
    }
}
