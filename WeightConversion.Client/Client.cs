using System;
using System.IO;
using System.Net.Sockets;

namespace WeightConversion.Client
{
    public class Client
    {
        public void Start()
        {   
            using (TcpClient socket = new TcpClient("127.0.0.1", 500))
            {
                using (NetworkStream ns = socket.GetStream())
                {
                    using (StreamReader sr = new StreamReader(ns))
                    {
                        using (StreamWriter sw = new StreamWriter(ns))
                        {
                            //var payload = $"({WeightConverter.ConvertToGram(1)} Gram)  ({WeightConverter.ConvertToOunce(300)} Ounce)";
                            var payload = "togram,500";
                            sw.WriteLine(payload);

                            sw.Flush();

                            string line = sr.ReadLine();

                            Console.WriteLine(line);
                        }
                    }
                }
            }
        }
    }
}