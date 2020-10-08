using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPObserver
{
    public abstract class IObserver
    {
        protected TcpClient ClientSocket
        {
            get; set;
        }

        public void WriteToConnection(string Message)
        {
            var Stream = ClientSocket.GetStream();
            var SentContent = Encoding.ASCII.GetBytes(Message.Trim() + TCPConfigs.Delimiter);
            Stream.Write(SentContent, 0, SentContent.Length);
            Stream.Flush();
        }

        public string ReadFromConnection()
        {
            var ReceivedContent = new byte[TCPConfigs.MessageLength];
            ClientSocket.GetStream().Read(ReceivedContent, 0, ClientSocket.ReceiveBufferSize);
            return Encoding.ASCII.GetString(ReceivedContent).Split(TCPConfigs.Delimiter)[0].Trim();
        }

    }

    public abstract class Observable
    {
        private List<IObserver> Observers = new List<IObserver>();

        public void AddObserver(IObserver observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            Observers.Remove(observer);
        }


    }

    public static class TCPConfigs
    {
        public const string IP = "127.0.0.1";
        public const int Port = 8888;
        public const int MessageLength = 64 * 1024;
        public const char Delimiter = '$';
    }
}
