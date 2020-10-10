using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public abstract class IObserver
    {
        protected TcpClient tcpClient;

        public void WriteToConnection(string Message)
        {
            var Stream = tcpClient.GetStream();
            var SentContent = Encoding.ASCII.GetBytes(Message.Trim() + TCPConfigs.Delimiter);
            Stream.Write(SentContent, 0, SentContent.Length);
            Stream.Flush();
        }

        public string ReadFromConnection()
        {
            var ReceivedContent = new byte[TCPConfigs.MessageLength];
            tcpClient.GetStream().Read(ReceivedContent, 0, tcpClient.ReceiveBufferSize);
            return Encoding.ASCII.GetString(ReceivedContent).Split(TCPConfigs.Delimiter)[0].Trim();
        }

    }

    public abstract class Observable
    {
        private List<IObserver> observers = new List<IObserver>();

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
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
