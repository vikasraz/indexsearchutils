using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Lucene.Net.Documents;
using ISUtils.Common;

namespace ISUtils.Network
{
    public class SocketBase
    {
        #region "事件参数"
        public class SearchEventArgs : EventArgs
        {
            private SearchInfo info;
            public SearchInfo SearchInfo
            {
                get { return info; }
                set { info = value; }
            }
            private Connection connection;
            public Connection Connection
            {
                get { return connection; }
                set { connection = value; }
            }
            public SearchEventArgs(Connection connection, SearchInfo info)
            {
                this.connection = connection;
                this.info = info;
            }
        }
        public class ResultEventArgs : EventArgs
        {
            private Connection connection;
            public Connection Connection
            {
                get { return connection; }
                set { connection = value; }
            }
            private SearchResult result;
            public SearchResult Result
            {
                get { return result; }
                set { result = value; }
            }
            public ResultEventArgs(Connection connection, SearchResult result)
            {
                this.connection=connection;
                this.result =result;
            }
        }
        public class ConnectionEventArgs : EventArgs
        {
            private Connection connection;
            public Connection Connection
            {
                get { return connection; }
                set { connection = value; }
            }
            private Exception _exception;
            public Exception Exception
            {
                get { return _exception; }
                set { _exception = value; }
            }
            public ConnectionEventArgs(Connection connection, Exception e)
            {
                this.connection = connection;
                this._exception = e;
            }
        }
        #endregion
        #region "EventHandler"
        public delegate void SearchEventHandler(object sender, SearchEventArgs e);
        public delegate void ResultEventHandler(object sender, ResultEventArgs e);
        public delegate void ConnectionEventHandler(object sender, ConnectionEventArgs e);
        #endregion
        #region "event"
        protected event SearchEventHandler OnSearchReceivedEvent;
        protected event SearchEventHandler OnSearchSentEvent;
        protected event ResultEventHandler OnResultReceivedEvent;
        protected event ResultEventHandler OnResultSentEvent;
        protected event ConnectionEventHandler OnConnectionClosedEvent;
        protected event ConnectionEventHandler OnConnectedEvent;
        public event SearchEventHandler OnSearchReceived
        {
            add { OnSearchReceivedEvent += value; }
            remove { OnSearchReceivedEvent -= value; }
        }
        public event SearchEventHandler OnSearchSent
        {
            add { OnSearchSentEvent += value; }
            remove { OnSearchSentEvent -= value; }
        }
        public event ResultEventHandler OnResultReceived
        {
            add { OnResultReceivedEvent += value; }
            remove { OnResultReceivedEvent -= value; }
        }
        public event ResultEventHandler OnResultSent
        {
            add { OnResultSentEvent += value; }
            remove { OnResultSentEvent -= value; }
        }
        public event ConnectionEventHandler OnConnectionClosed
        {
            add { OnConnectionClosedEvent += value; }
            remove { OnConnectionClosedEvent -= value; }
        }
        public event ConnectionEventHandler OnConnected
        {
            add { OnConnectedEvent += value; }
            remove { OnConnectedEvent -= value; }
        }
        #endregion
        #region "属性"
        protected Dictionary<QueryInfo, List<Document>> searchResultDict=new Dictionary<QueryInfo,List<Document>>();
        public Dictionary<QueryInfo, List<Document>> SearchResults
        {
            get 
            {
                if (searchResultDict == null)
                    searchResultDict = new Dictionary<QueryInfo, List<Document>>();
                return searchResultDict;
            }
        }
        protected ConnectionCollection connections;
        public ConnectionCollection Connections
        {
            get { return connections; }
            set { connections = value; }
        }
        #endregion
        #region "保护变量"
        protected Thread listenThread;
        protected Thread sendThread;
        #endregion

        public SocketBase()
        {
            this.searchResultDict = new Dictionary<QueryInfo, List<Document>>();
            this.connections = new ConnectionCollection();
        }
        protected void Send(SearchInfo searchInfo, Connection connection)
        {
            lock (this)
            {
                BinaryFormatter formater = new BinaryFormatter();
                try
                {
                    formater.Serialize(connection.NetworkStream, searchInfo);
                }
                catch (SerializationException e)
                {
                    throw e;
                }
                connection.NetworkStream.Flush();
            }
        }
        protected void Send(SearchResult searchResult, Connection connection)
        {
            lock (this)
            {
                BinaryFormatter formater = new BinaryFormatter();
                try
                {
                    formater.Serialize(connection.NetworkStream, searchResult);
                }
                catch (SerializationException e)
                {
                    throw e;
                }
                connection.NetworkStream.Flush();
            }
        }
        protected SearchInfo ReceiveInfo(Connection connection)
        {
            SearchInfo info = null;
            lock(this)
            {
                BinaryFormatter formater = new BinaryFormatter();
                try
                {
                    info = (SearchInfo)formater.Deserialize(connection.NetworkStream);
                }
                catch (SerializationException)
                { 
                }
            }
            return info;
        }

    }
}
