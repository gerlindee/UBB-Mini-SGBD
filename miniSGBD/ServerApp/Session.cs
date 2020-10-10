using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Utils;

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

        public void DisplayClientRequest(string command, string attributes)
        {
            Console.WriteLine("Client " + sessionID + ": " + command + " with attributes " + attributes);
        }

        public void HandleClientRequest(string command, string attributes)
        {

        }
    }
}
