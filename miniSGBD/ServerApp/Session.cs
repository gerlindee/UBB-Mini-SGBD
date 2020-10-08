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
        private int SessionID;

        public int getSessionID()
        {
            return SessionID;
        }

        public Session(int sessionID, TcpClient clientSocket)
        {
            SessionID = sessionID;
            ClientSocket = clientSocket;
        }
    }
}
