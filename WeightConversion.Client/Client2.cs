using System;
using System.IO;
using System.Net.Sockets;

namespace WeightConversion.Client
{
    public class Client2
    {
        private string serverEndpoint;
        private int serverPort;

        public Client2(string serverEndpoint, int serverPort)
        {
            this.serverEndpoint = serverEndpoint;
            this.serverPort = serverPort;
        }

        public string Convert(string conversionType, int valueToConvert)
        {   
            using (TcpClient socket = new TcpClient("127.0.0.1", 500))
            {
                using (NetworkStream ns = socket.GetStream())
                {
                    using (StreamReader sr = new StreamReader(ns))
                    {
                        using (StreamWriter sw = new StreamWriter(ns))
                        {
                            // FORMAT: "TOGRAM,500"
                            var payload = $"{conversionType},{valueToConvert}";
                            sw.WriteLine(payload);
                            sw.Flush();

                            var result = sr.ReadLine();
                            return result;
                        }
                    }
                }
            }
        }
    }
}