using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ISUtils.Network
{
    public class Server
    {
        private ConnectionCollection connections;
        private TcpListener listener;
        private Thread listenningThread;
        
        public ConnectionCollection Connections
        {
            get { return connections; }
            set { connections = value; }
        }
        public Server(TcpListener listener)
        {
            connections = new ConnectionCollection();
            this.listener = listener;
        }
        public void Start()
        {
            while (true)
            {
                if (listener.Pending())
                {
                    TcpClient client = listener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    this.connections.Add(new Connection(stream));
                }
            }
        }
        public void Listenning()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(100);
                foreach (Connection connection in this.connections)
                {
                    if (connection.NetworkStream.CanRead && connection.NetworkStream.DataAvailable)
                    {
 
                    }
                }
            }
        }
        public void StartListen()
        {
            listenningThread = new Thread(new ThreadStart(Listenning));
            listenningThread.Start();
        }
    }
}
