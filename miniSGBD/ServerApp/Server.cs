using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPObserver;

namespace ServerApp
{
    class Server : Observable
    {
        public int LatestSessionID = 0;

        public Server()
        {
            Console.WriteLine("Server session started....");
        }
        public void StartServer()
        {
            var ServerSocket = new TcpListener(IPAddress.Parse(TCPConfigs.IP), TCPConfigs.Port);
            ServerSocket.Start();

            while (true)
            {
                var SessionSocket = ServerSocket.AcceptTcpClient();
                Task.Factory.StartNew(() =>
                {
                    LatestSessionID++;
                    var Session = new Session(LatestSessionID, SessionSocket);
                    Console.WriteLine("Client connected with ID: " + LatestSessionID);
                    AddObserver(Session);

                    while (true)
                    {
                        try
                        {
                            var Stream = SessionSocket.GetStream();
                            var ReceivedContent = new byte[TCPConfigs.MessageLength];
                            Stream.Read(ReceivedContent, 0, SessionSocket.ReceiveBufferSize);
                            var ClientQuery = Encoding.ASCII.GetString(ReceivedContent).Split(TCPConfigs.Delimiter)[0].Trim();

                            Console.WriteLine(ClientQuery);
                            // TODO: aici o sa facem handling la query
                            // TODO: dupa ce se face handling la query tot de aici ar trebui sa trimitem un raspuns inapoi la client

                        }
                        catch (Exception)
                        {
                            break; // TODO: exception handling calumea :))) 
                        }
                    }
                    Console.WriteLine("Client disconnected with ID: " + Session.getSessionID());
                    LatestSessionID--;
                    RemoveObserver(Session);
                    SessionSocket.Close();

                });

            }
            ServerSocket.Stop();
        }
    }
}
