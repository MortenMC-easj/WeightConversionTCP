using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace WeightConversion.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void CanStartAndStopServer()
        {
            var serverEndpoint = "127.0.0.1";
            var serverPort = 500;
            var server = new WeightConversion.Server.Server(serverEndpoint, serverPort);


            Task.Run(() => server.Start());

            server.Stop();

            Assert.NotNull(server);


        }



        [Fact]
        public void ClientCanConnectToServer()
        {
            // Arrange:
            string expected = "4252,428 Gr.";
            string serverEndpoint = "127.0.0.1";
            int serverPort = 500;


            var server = new WeightConversion.Server.Server(serverEndpoint, serverPort);
            Task.Run(() => server.Start());

            // Act: 

            // server is now running/listening for incomming connections:
            var client = new WeightConversion.Client.Client2(serverEndpoint, serverPort);
            var actual = client.Convert("TOGRAM", 150);


            // Assert: 
            Assert.Equal(expected, actual);
            
            /*
              * for(int i = 0; i < 100; i++) 
              * client.Convert("TOGRAM", 150)
              * 
             */

            server.Stop();

            Assert.NotNull(server);


        }



        [Fact]
        public void MultipleClientCanConnectToServer()
        {
            // Arrange:
            int clientCount = 10000;
            string expected = "4252,428 Gr.";

            var serverEndpoint = "127.0.0.1";
            var serverPort = 500;
            var server = new WeightConversion.Server.Server(serverEndpoint, serverPort);

            Task.Run(() => server.Start());


            for (var i = 0; i < clientCount; i++)
            {
                Console.WriteLine("foo", i);
                // Act: 
                var client = new WeightConversion.Client.Client2(serverEndpoint, serverPort);


                var convertionType = i % 2 == 0 ? "TOGRAM" : "TOOUNCE";
                var actual = client.Convert(convertionType, 150);

                // Assert: 
                Assert.NotEmpty(actual);
            }

            server.Stop();
        }
    }
}
