using System;
using System.Net;
using System.Net.Sockets;

namespace ISUtils.Network
{
    public class Connection
    {
        private NetworkStream networkStream;
        private string connectionName;
        public NetworkStream NetworkStream
        {
            get { return networkStream; }
            set { networkStream = value; }
        }
        public string ConnectionName
        {
            get { return connectionName; }
            set { connectionName = value; }
        }
        public Connection(NetworkStream ns, string name)
        {
            networkStream = ns;
            connectionName = name;
        }
        public Connection(NetworkStream ns)
            : this(ns, string.Empty)
        { 
        }
    }
}
