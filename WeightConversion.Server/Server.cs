using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WeightConversion.Server
{

    public class Server
    {
        private string serverEndpoint;
        private int serverPort;

        public Server(string serverEndpoint, int serverPort)
        {
            this.serverEndpoint = serverEndpoint;
            this.serverPort = serverPort;
        }

        public TcpListener Listener { get; set; }

        ///// <summary>
        /////     Configuration class for the server
        ///// </summary>
        //public static class Config
        //{
        //    public static int Port = 500; 
        //    public static string Endpoint = "127.0.0.1";
        //}


        public void Start()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(serverEndpoint);

                Console.WriteLine("Starting TCP listener...");

                Listener = new TcpListener(ipAddress, serverPort);

                Listener.Start();


                while (true)
                {
                    Console.WriteLine("Server is listening on " + Listener.LocalEndpoint);
                    Console.WriteLine("Waiting for a connection...");
                    Console.WriteLine("###########################################");

                    // incoming client connected
                    TcpClient client = Listener.AcceptTcpClient();


                    var sw = new Stopwatch();
                    sw.MeasureTimeElapsed( () =>
                    {
                        // 1. Log that we received a request 
                        Console.WriteLine("Connection accepted.");
                        Console.WriteLine("Reading data...");

                        #region --- 2. Read Payload: --- 
                        // Read the actual payload (posted by the connecting client)
                        // get the incoming data through a network stream
                        NetworkStream stream = client.GetStream();
                        byte[] buffer = new byte[client.ReceiveBufferSize];

                        // read incoming stream
                        int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);

                        Console.WriteLine("Recieved data: ");
                        Console.WriteLine("----------------------------------");

                        // convert the command data received into a string
                        string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead).TrimEnd(Environment.NewLine.ToCharArray());
                        Console.WriteLine("Received Command : " + dataReceived);
                        #endregion


                        #region --- 3. Parse & Evaluate payload: --- 
                        // IMPORTANT: We expect payload to be in the format: "TOGRAM,100"
                        var data = dataReceived.Split(',', StringSplitOptions.RemoveEmptyEntries);

                        var errors = new List<string>();
                        double? clientValue = null;
                        string clientConversionType = null;

                        ConversionType? conversionType = null;


                        #region --- Parse ConversionType: ---
                        try
                        {
                            // try casting the clients input to the data types we expect it to be:
                            clientConversionType = data[0].ToUpperInvariant();
                            conversionType = Enum.Parse<ConversionType>(clientConversionType);
                        }
                        catch (Exception e)
                        {
                            errors.Add($"Invalid ConversionType ({clientConversionType}). Supported types are: {ConversionType.TOGRAM}, {ConversionType.TOOUNCE}!");                            
                        }
                        #endregion

                        #region --- Parse Value: ---
                        try
                        {
                            clientValue = double.Parse(data[1], NumberStyles.Number);
                        }
                        catch (Exception e)
                        {
                            errors.Add($"Invalid Value ({clientValue}). Value must be a valid number!");
                        }
                        #endregion

                        #endregion

                        if (errors.Any())
                        {
                            errors.ForEach(error => WriteToStream(error, stream));
                            WriteToStream("FOO", stream);
                        }
                        else
                        {
                            if (conversionType.HasValue && clientValue.HasValue)
                            {
                                //4.process the request
                                var result = WeightConverter.Convert(conversionType.Value, clientValue.Value);
                                var msg = $"{result.Value} {result.Unit}";

                                //5. return result to client
                                WriteToStream(msg, stream);
                            }
                        }

                        
                        client.Close();

                    });

                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.StackTrace);
                Console.ReadLine();
            }
        }

        private static void WriteToStream(string msg, NetworkStream stream)
        {
            // output the message to the server itself
            Console.WriteLine(msg);
            Console.WriteLine();

            // ...and then send the response to the client:
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);            
            stream.Write(bytes, 0, bytes.Length);
        }


        public void DoClient(TcpClient socket)
        {
            using (StreamReader sr = new StreamReader(socket.GetStream()))
            {
                using (StreamWriter sw = new StreamWriter(socket.GetStream())) 
                {
                    string incomingStr = sr.ReadLine();
                    Console.WriteLine($"String in = {incomingStr}");

                    sw.WriteLine(incomingStr);
                    sw.Flush();

                } // Using slut = 
            }  
            socket?.Close();
        }

        public void Stop()
        {
            Listener?.Stop();
        }
    }
}
