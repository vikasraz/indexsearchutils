using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ISUtils.Common;

namespace ISUtils.Network
{
    public class SocketFactory
    {
        private Thread serverListenThread;
        public void StartServer(int port)
        {
            TcpListener listener = new TcpListener(port);
            listener.Start();
            Server server = new Server(listener);
            serverListenThread = new Thread(new ThreadStart(server.Start));
            serverListenThread.Start();
        }
        public Connection StartClient(IPAddress ipAddress, int port)
        {
            return Client.StartClient(ipAddress, port);
        }
        public static void SendMessage(FormatedResult formatedResult, Connection connection)
        {
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                formater.Serialize(connection.NetworkStream, formatedResult);
            }
            catch (SerializationException e)
            {
                throw e;
            }
            connection.NetworkStream.Flush();
        }
    }
}
