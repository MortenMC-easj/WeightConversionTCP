using System;

namespace WeightConversion.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            client.Start();

            Console.ReadLine();
        }
    }
}
