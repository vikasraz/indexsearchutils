using System;
using System.Net;
using System.Net.Sockets;

namespace ISUtils.Network
{
    public class Client
    {
        public const int CONNECTTIMEOUT = 10;
        private Connection connection;
        public Connection Connection
        {
            get {return connection;}
            set {connection =value;}
        }
        public Client()
        { 
        }
        public static Connection StartClient(IPAddress ipAddress, int port)
        {
            TcpClient client = new TcpClient();
            client.SendTimeout = CONNECTTIMEOUT;
            client.ReceiveTimeout = CONNECTTIMEOUT;
            client.Connect(ipAddress, port);
            NetworkStream stream = client.GetStream();
            return new Connection(stream);
        }
    }
}
