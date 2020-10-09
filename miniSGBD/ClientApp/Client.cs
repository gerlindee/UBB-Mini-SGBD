using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPObserver; 

namespace ClientApp
{
    public class Client:IObserver
    {
        public Client()
        {
            tcpClient = new TcpClient();
        }

        public void Connect()
        {
            tcpClient.Connect(TCPConfigs.IP, TCPConfigs.Port);
        }

        public bool IsDataAvailable()
        {
            return tcpClient.Available > 0;
        }
    }
}
