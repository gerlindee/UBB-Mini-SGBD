using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ServerApp
{
    class Server : Observable
    {
        private int latestSessionID = 0;

        public void StartServer()
        {
            var serverListener = new TcpListener(IPAddress.Parse(TCPConfigs.IP), TCPConfigs.Port);
            serverListener.Start();

            Console.WriteLine("Server session started....");

            while (true)
            {
                var tcpClient = serverListener.AcceptTcpClient();
                Task.Factory.StartNew(() =>
                {
                    latestSessionID++;
                    var clientSession = new Session(latestSessionID, tcpClient);
                    Console.WriteLine("Client connected with ID: " + latestSessionID);
                    Subscribe(clientSession);

                    while (true)
                    {
                        try
                        {
                            var clientRequest = clientSession.Read();
                            clientSession.DisplayClientRequest(clientRequest);

                        }
                        catch (Exception)
                        {
                            break; // TODO: exception handling calumea :))) 
                        }
                    }
                    Console.WriteLine("Client disconnected with ID: " + clientSession.getSessionID());
                    latestSessionID--;
                    Unsubscribe(clientSession);
                    tcpClient.Close();

                });

            }
            serverListener.Stop();
        }
    }
}
