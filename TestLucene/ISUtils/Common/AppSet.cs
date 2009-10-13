using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ISUtils.Common
{
    /*
<Settings>
	<CommonSetting>
    <IndexPath>D:\IndexData</IndexPath>
  </CommonSetting>
  <FileIndex>
  	<Directories>
  		<Directory >
  			<Path>W:\津国土房东丽</Path>
  			<VRoot>files</VRoot>
  		</Directory>
  	</Directories>
  </FileIndex>
  <Dictionary>
    <BasePath>seglib\BaseDict.txt</BasePath>
    <NamePath>seglib\FamilyName.txt</NamePath>
    <NumberPath>seglib\Number.txt</NumberPath>
    <FilterPath>seglib\Filter.txt</FilterPath>
    <CustomPaths>
      <Path>seglib\CustomDict.txt</Path>
      <Path>seglib\Other.txt</Path>
    </CustomPaths>
  </Dictionary>
  <Indexer>
    <MainIndexSpan>1</MainIndexSpan>
    <RamBufferSize>512</RamBufferSize>
    <MaxBufferedDocs>10000</MaxBufferedDocs>
    <MergeFactor>15000</MergeFactor>   
  </Indexer>
  <Searchd>
    <Address>192.168.1.102</Address>
    <Port>3312</Port>
  </Searchd>
  <DbMointor>
  	<DbSuffix>_SSB</DbSuffix>
  	<TvSuffix>_log</TvSuffix>
  </DbMonitor>
</Settings>
    */
    [Serializable]
    public class AppSet:IXmlSerializable
    {
        #region 内部类
        [Serializable]
        public sealed class CommonSet
        {
            #region 属性
            /**/
            /// <summary>
            /// 存储密码
            /// </summary>
            private string indexPath = "";
            /**/
            /// <summary>
            /// 设定或返回密码
            /// </summary>
            public string IndexPath
            {
                get { return indexPath; }
                set { indexPath = value; }
            }
            #endregion
        }
        [Serializable]
        public sealed class IndexerSet
        {
            #region 属性
            /**/
            /// <summary>
            /// 存储主索引重建间隔
            /// </summary>
            private int maintmspan = 1;//1 day
            /**/
            /// <summary>
            /// 设定或返回主索引重建间隔
            /// </summary>
            public int MainIndexReCreateTimeSpan
            {
                get { return maintmspan; }
                set { maintmspan = value; }
            }
            /**/
            /// <summary>
            /// 存储主索引重建时间
            /// </summary>
            private double rambuffersize = 512;
            /**/
            /// <summary>
            /// 设定或返回主索引重建时间
            /// </summary>
            public double RamBufferSize
            {
                get { return rambuffersize; }
                set { rambuffersize = value; }
            }
            /**/
            /// <summary>
            /// 存储合并因子
            /// </summary>
            private int mergeFactor = 10000;
            /**/
            /// <summary>
            /// 设定或返回合并因子
            /// </summary>
            public int MergeFactor
            {
                get { return mergeFactor; }
                set { mergeFactor = value; }
            }
            /**/
            /// <summary>
            /// 存储文档内存最大存储数
            /// </summary>
            private int maxBufferedDocs = 10000;
            /**/
            /// <summary>
            /// 设定或返回文档内存最大存储数
            /// </summary>
            public int MaxBufferedDocs
            {
                get { return maxBufferedDocs; }
                set { maxBufferedDocs = value; }
            }
            #endregion
        }
        [Serializable]
        public sealed class SearchSet
        {
            #region 属性
            /**/
            /// <summary>
            /// 存储IP地址
            /// </summary>
            private string address = "192.168.0.1";
            /**/
            /// <summary>
            /// 设定或返回IP地址
            /// </summary>
            public string Address
            {
                get { return address; }
                set { address = value; }
            }
            /**/
            /// <summary>
            /// 存储端口
            /// </summary>
            private int port = 3312;
            /**/
            /// <summary>
            /// 设定或返回端口
            /// </summary>
            public int Port
            {
                get { return port; }
                set { port = value; }
            }
            #endregion
        }
        #endregion
        #region 属性
        private CommonSet commonSet = new CommonSet();
        public CommonSet CommonSettings
        {
            get { return commonSet; }
            set { commonSet = value; }
        }
        private FileIndexSet fileIndexSet = new FileIndexSet();
        public FileIndexSet FileIndexSet
        {
            get { return fileIndexSet; }
            set { fileIndexSet = value; }
        }
        private DictionarySet dictionarySet = new DictionarySet();
        public DictionarySet DictionarySet
        {
            get { return dictionarySet; }
            set { dictionarySet = value; }
        }
        private IndexerSet indexerSet = new IndexerSet();
        public IndexerSet IndexerSettings
        {
            get { return indexerSet; }
            set { indexerSet = value; }
        }
        private SearchSet searchSet = new SearchSet();
        public SearchSet SearchSettings
        {
            get { return searchSet; }
            set { searchSet = value; }
        }
        private DbMonitorSet dbmSet = new DbMonitorSet();
        public DbMonitorSet DbMonitorSet
        {
            get { return dbmSet; }
            set { dbmSet = value; }
        }
        #endregion
        #region IXmlSerializable 成员
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }
        public void ReadXml(XmlReader reader)
        {
                                //case "MainIndexSpan":
                                //    indexerSet.MainIndexReCreateTimeSpan = int.Parse(reader.ReadElementString());
                                //    break;
        }
        public void WriteXml(XmlWriter writer)
        {
            #region CommonSet
            writer.WriteStartElement("CommonSetting");
            writer.WriteElementString("IndexPath", commonSet.IndexPath);
            writer.WriteEndElement();
            #endregion
            #region Indexer
            writer.WriteStartElement("Indexer");
            writer.WriteElementString("MainIndexSpan", indexerSet.MainIndexReCreateTimeSpan.ToString());
            writer.WriteElementString("MainIndexTime", indexerSet.MainIndexReCreateTime.ToString("HH:mm:ss"));
            writer.WriteElementString("MainIndexSpan", indexerSet.MainIndexReCreateTimeSpan.ToString());
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
