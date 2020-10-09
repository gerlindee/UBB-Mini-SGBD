using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPObserver;

namespace ServerApp
{
    class Session : IObserver
    {
        private readonly int sessionID;

        public int getSessionID()
        {
            return sessionID;
        }

        public Session(int ID, TcpClient clientSocket)
        {
            sessionID = ID;
            tcpClient = clientSocket;
        }

        public void DisplayClientRequest(string query)
        {
            Console.WriteLine("Client " + sessionID + ": " + query);
        }

        public void HandleClientRequest(string command, string attributes)
        {

        }
    }
}
