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
                            var clientStream = tcpClient.GetStream();
                            var requestReceived = new byte[TCPConfigs.MessageLength];
                            clientStream.Read(requestReceived, 0, tcpClient.ReceiveBufferSize);
                            var clientQuery = Encoding.ASCII.GetString(requestReceived).Split(TCPConfigs.Delimiter)[0].Split(';');

                            clientSession.DisplayClientRequest(clientQuery[0].Trim(), clientQuery[1].Trim());
                            clientSession.HandleClientRequest(clientQuery[0].Trim(), clientQuery[1].Trim());

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
