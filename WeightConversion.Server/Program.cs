using System;

namespace WeightConversion.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 500);
            server.Start();

            Console.WriteLine("Shutting down server...");
            //Console.ReadLine();   
        }
    }
}
