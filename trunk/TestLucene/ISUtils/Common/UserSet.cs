using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ISUtils.Common
{
    /*
 <Config>
	<CommonSetting>
    <DbType>SqlServer</DbType>
    <HostName>192.168.1.254</HostName>
    <DataBase>DongliBusiness</DataBase>
    <UserName>dnn5</UserName>
    <Password>19370707japan</Password>
    <Source>CodeMaker</Source>
  </CommonSetting>
  <Indexer>
    <MainIndexTime>23:45</MainIndexTime>
  </Indexer>
  <Searchd>
    <MaxChildren>30</MaxChildren>
    <MaxMatches>10000</MaxMatches>
    <MaxTrans>100</MaxTrans>
    <MinScore>0.0</MinScore>
  </Searchd>
</Config>*/
    [Serializable]
    public class UserSet : IXmlSerializable
    {
        #region 内部类
        [Serializable]
        public sealed class CommonSet
        {
            #region 属性
            /**/
            /// <summary>
            /// 存储数据库类型
            /// </summary>
            private DBTypeEnum dbtype = DBTypeEnum.SQL_Server;
            /**/
            /// <summary>
            /// 设定或返回数据库类型
            /// </summary>
            public DBTypeEnum DBType
            {
                get { return dbtype; }
                set { dbtype = value; }
            }
            /**/
            /// <summary>
            /// 存储数据库路径
            /// </summary>
            private string hostname = "";
            /**/
            /// <summary>
            /// 设定或返回数据库路径
            /// </summary>
            public string HostName
            {
                get { return hostname; }
                set { hostname = value; }
            }
            /**/
            /// <summary>
            /// 存储数据库名称
            /// </summary>
            private string database = "";
            /**/
            /// <summary>
            /// 设定或返回数据库名称
            /// </summary>
            public string DataBase
            {
                get { return database; }
                set { database = value; }
            }
            /**/
            /// <summary>
            /// 存储用户名
            /// </summary>
            private string username = "";
            /**/
            /// <summary>
            /// 设定或返回用户名
            /// </summary>
            public string UserName
            {
                get { return username; }
                set { username = value; }
            }
            /**/
            /// <summary>
            /// 存储密码
            /// </summary>
            private string password = "";
            /**/
            /// <summary>
            /// 设定或返回密码
            /// </summary>
            public string Password
            {
                get { return password; }
                set { password = value; }
            }
            /**/
            /// <summary>
            /// 存储密码
            /// </summary>
            private string source = "";
            /**/
            /// <summary>
            /// 设定或返回密码
            /// </summary>
            public string Source
            {
                get { return source; }
                set { source = value; }
            }
            #endregion
        }
        [Serializable]
        public sealed class IndexerSet
        {
            #region 属性
            /**/
            /// <summary>
            /// 存储主索引重建时间
            /// </summary>
            private DateTime maincreate = DateTime.Parse("23:30:00");
            /**/
            /// <summary>
            /// 设定或返回主索引重建时间
            /// </summary>
            public DateTime MainIndexReCreateTime
            {
                get { return maincreate; }
                set { maincreate = value; }
            }
            #endregion
        }
        [Serializable]
        public sealed class SearchSet
        {
            #region 属性
            /**/
            /// <summary>
            /// 存储最大匹配记录数
            /// </summary>
            private int maxchildren = 30;
            /**/
            /// <summary>
            /// 设定或返回最大查询客户数
            /// </summary>
            public int MaxChildren
            {
                get { return maxchildren; }
                set { maxchildren = value; }
            }
            /**/
            /// <summary>
            /// 存储最大匹配记录数
            /// </summary>
            private int maxmatches = 10000;
            /**/
            /// <summary>
            /// 设定或返回最大匹配记录数
            /// </summary>
            public int MaxMatches
            {
                get { return maxmatches; }
                set { maxmatches = value; }
            }
            private int maxtrans = 100;
            public int MaxTrans
            {
                get { return maxtrans; }
                set { maxtrans = value; }
            }
            private float minscore = 0.0f;
            public float MinScore
            {
                get { return minscore; }
                set { minscore = value; }
            }
            #endregion
        }
        #endregion
        #region 属性
        private CommonSet commonSet = new CommonSet();
        public CommonSet CommonSet
        {
            get { return commonSet; }
            set { commonSet = value; }
        }
        private IndexerSet indexerSet = new IndexerSet();
        public IndexerSet IndexerSet
        {
            get { return indexerSet; }
            set { indexerSet = value; }
        }
        private SearchSet searchSet = new SearchSet();
        public SearchSet SearchSet
        {
            get { return searchSet; }
            set { searchSet = value; }
        }
        #endregion
        #region IXmlSerializable 成员
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }
        public void ReadXml(XmlReader reader)
        {
            string startElementName = reader.Name;
            string currentElementName, currentNodeName, currentItemName;
            this.sourceList.Clear();
            this.indexList.Clear();
            do
            {
                currentElementName = reader.Name;
                if (currentElementName == startElementName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                {
                    reader.Read();
                    break;
                }
                switch (currentElementName)
                {
                    case "CommonSetting":
                        #region CommonSet
                        CommonSet commonSet =new CommonSet();
                        do
                        {
                            currentNodeName = reader.Name;
                            if (currentNodeName == currentElementName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                            {
                                break;
                            }
                            switch (currentNodeName)
                            {
                                case "DbType":
                                    commonSet.DBType = DbType.GetDbType(reader.ReadElementString());
                                    break;
                                case "HostName":
                                    commonSet.HostName = reader.ReadElementString();
                                    break;
                                case "DataBase":
                                    commonSet.DataBase = reader.ReadElementString();
                                    break;
                                case "UserName":
                                    commonSet.UserName = reader.ReadElementString();
                                    break;
                                case "Password":
                                    commonSet.Password = reader.ReadElementString();
                                    break;
                                case "Source":
                                    commonSet.Source = reader.ReadElementString();
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        } while (true);
                        this.commonSet=commonSet;
                        #endregion
                        reader.Read();
                        break;
                    case "Indexer":
                        #region Indexer
                        IndexerSet indexerSet = new IndexerSet();
                        do
                        {
                            currentNodeName = reader.Name;
                            if (currentNodeName == currentElementName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                            {
                                break;
                            }
                            switch (currentNodeName)
                            {
                                case "MainIndexTime":
                                    indexerSet.MainIndexReCreateTime = DateTime.Parse(reader.ReadElementString());
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        } while (true);
                        this.indexerSet=indexerSet;
                        #endregion
                        reader.Read();
                        break;
                    case "Searchd":
                        #region Searchd
                        SearchSet searchSet = new SearchSet();  
                        do
                        {
                            currentNodeName = reader.Name;
                            if (currentNodeName == currentElementName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                            {
                                break;
                            }
                            switch (currentNodeName)
                            {
                                case "MaxChildren":
                                    searchSet.MaxChildren = int.Parse(reader.ReadElementString());
                                    break;
                                case "MaxMatches":
                                    searchSet.MaxMatches = int.Parse(reader.ReadElementString());
                                    break;
                                case "MaxTrans":
                                    searchSet.MaxTrans = int.Parse(reader.ReadElementString());
                                    break;
                                case "MinScore":
                                    searchSet.MinScore = float.Parse(reader.ReadElementString());
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        } while (true);
                        this.searchSet = searchSet;
                        #endregion
                        reader.Read();
                        break;
                    default:
                        reader.Read();
                        break;
                }
            } while (true);
        }
        public void WriteXml(XmlWriter writer)
        {
            #region CommonSet
            writer.WriteStartElement("CommonSetting");
            writer.WriteElementString("DbType", DbType.GetDbTypeStr(commonSet.DBType));
            writer.WriteElementString("HostName", commonSet.HostName);
            writer.WriteElementString("DataBase", commonSet.DataBase);
            writer.WriteElementString("UserName", commonSet.UserName);
            writer.WriteElementString("Password", commonSet.Password);
            writer.WriteElementString("Source", commonSet.Source);
            writer.WriteEndElement();
            #endregion
            #region Indexer
            writer.WriteStartElement("Indexer");
            writer.WriteElementString("MainIndexTime", indexerSet.MainIndexReCreateTime.ToString("HH:mm:ss"));
            writer.WriteEndElement();
            #endregion
            #region Search
            writer.WriteStartElement("Searchd");
            writer.WriteElementString("MaxChildren", searchSet.MaxChildren.ToString());
            writer.WriteElementString("MaxMatches", searchSet.MaxMatches.ToString());
            writer.WriteElementString("MaxTrans", searchSet.MaxTrans.ToString());
            writer.WriteElementString("MinScore", searchSet.MinScore.ToString());
            writer.WriteEndElement();
            #endregion
        }
        #endregion
    }
}
