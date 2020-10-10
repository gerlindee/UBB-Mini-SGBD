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

        public void DisplayClientRequest(string request)
        {
            var requestSplit = request.Split(';');
            Console.WriteLine("Client " + sessionID + ": ");
            int attributeIdx = 0;
            while (attributeIdx < requestSplit.Length)
            {
                if (attributeIdx == 0)
                {
                    Console.WriteLine("\t" + "Command: " + requestSplit[attributeIdx]);
                } 
                else
                {
                    Console.WriteLine("\t" + "Attribute: " + requestSplit[attributeIdx]);
                }
                attributeIdx++;
            }
        }

        public string HandleClientRequest(string request)
        {
            var response = QueryManager.DispatchQuery(request);
            Console.WriteLine("Client " + sessionID + ": ");
            Console.WriteLine("\t" + "Execution of command " + request.Split(';')[0] + " finished with status " + response);
            return response;
        }
    }
}
