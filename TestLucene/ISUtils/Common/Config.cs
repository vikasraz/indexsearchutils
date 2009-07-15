using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ISUtils.Common
{
    [Serializable]
    public class Config : IXmlSerializable
    {
        #region Const Values
        /**/
        /// <summary>
        /// Config文件中Config项目的结束标志
        /// </summary>
        public const string SourcePrefix = "SRC_";
        /**/
        /// <summary>
        /// Config文件中Config项目的结束标志
        /// </summary>
        public const string IndexPrefix = "IDX_";
        /**/
        /// <summary>
        /// Config文件中Config项目的开始标志
        /// </summary>
        public const string Prefix = "{";
        /**/
        /// <summary>
        /// Config文件中Config项目的结束标志
        /// </summary>
        public const string Suffix = "}";
        /**/
        /// <summary>
        /// Config文件中Config条目的忽略标志
        /// </summary>
        public const string Ignore = "#";
        /**/
        /// <summary>
        /// Config文件中Config条目的分割字符串
        /// </summary>
        public const string Devider = "=\t\n,";
        /**/
        /// <summary>
        /// Config文件中SOURCE项目的标志
        /// </summary>
        public const string SourceFlag = "SOURCE";
        /**/
        /// <summary>
        /// Config文件中INDEX项目的标志
        /// </summary>
        public const string IndexFlag = "INDEX";
        /**/
        /// <summary>
        /// Config文件中INDEXER项目的标志
        /// </summary>
        public const string IndexerFlag = "INDEXER";
        /**/
        /// <summary>
        /// Config文件中Searchd项目的标志
        /// </summary>
        public const string SearchdFlag = "SEARCHD";
        /**/
        /// <summary>
        /// Config文件中Dictionary项目的标志
        /// </summary>
        public const string DictionaryFlag = "DICTIONARY";
        /**/
        /// <summary>
        /// Config文件中FileIndexSet项目的标志
        /// </summary>
        public const string FileIndexSetFlag = "FILEINDEX";
        #endregion
        #region Property
        /**/
        /// <summary>
        /// 存储索引源列表
        /// </summary>
        private List<Source> sourceList=new List<Source>();
        /**/
        /// <summary>
        /// 返回索引源列表
        /// </summary>
        public List<Source> SourceList
        {
            get { return sourceList; }
            set { sourceList = value; }
        }
        /**/
        /// <summary>
        /// 存储索引列表
        /// </summary>
        private List<IndexSet> indexList=new List<IndexSet>();
        /**/
        /// <summary>
        /// 返回索引列表
        /// </summary>
        public List<IndexSet> IndexList
        {
            get { return indexList; }
            set { indexList = value; }
        }
        /**/
        /// <summary>
        /// 存储索引器
        /// </summary>
        private IndexerSet indexer=new IndexerSet();
        /**/
        /// <summary>
        /// 返回索引器
        /// </summary>
        public IndexerSet IndexerSet
        {
            get { return indexer; }
            set { indexer = value; }
        }
        /**/
        /// <summary>
        /// 存储搜索引擎设置
        /// </summary>
        private SearchSet searchd=new SearchSet();
        /**/
        /// <summary>
        /// 返回搜索引擎设置
        /// </summary>
        public SearchSet SearchSet
        {
            get { return searchd; }
            set { searchd = value; }
        }
        /**/
        /// <summary>
        /// 存储词库设置
        /// </summary>
        private DictionarySet dictSet=new DictionarySet();
        /**/
        /// <summary>
        /// 返回词库设置
        /// </summary>
        public DictionarySet DictionarySet
        {
            get { return dictSet; }
            set { dictSet = value; }
        }
        private FileIndexSet fileSet = new FileIndexSet();
        public FileIndexSet FileIndexSet
        {
            get { return fileSet; }
            set { fileSet = value; }
        }
        #endregion
        #region Constructor
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        public Config()
        { 
        }
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        public Config(string configFilePath,bool isXmlFile)
        {
            if (configFilePath == null)
                throw new ArgumentNullException("configFilePath", "configFilePath must no be null!");
            if (SupportClass.File.IsFileExists(configFilePath) == false)
                throw new ArgumentException("configFilePath must be exists!", "configFilePath");
            if (isXmlFile)
            {
                Config config ;
                config = (Config)SupportClass.File.GetObjectFromXmlFile(configFilePath,typeof(Config));
                this.dictSet = config.dictSet;
                this.indexer = config.indexer;
                this.indexList = config.indexList;
                this.searchd = config.searchd;
                this.sourceList = config.sourceList;
                this.fileSet = config.fileSet;
            }
            else
            {
                List<string> src = SupportClass.File.GetFileText(configFilePath);
                sourceList = Source.GetSourceList(src);
                indexList = IndexSet.GetIndexList(src);
                indexer = IndexerSet.GetIndexer(src);
                searchd = SearchSet.GetSearchSet(src);
                dictSet = DictionarySet.GetDictionarySet(src);
            }
        }
        #endregion
        #region IXmlSerializable
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }
        public void WriteXml(XmlWriter writer)
        {
            #region SourceList
            foreach (Source source in sourceList)
            {
                writer.WriteStartElement("Source");
                writer.WriteAttributeString("Name", source.SourceName);
                writer.WriteElementString("DbType", DbType.GetDbTypeStr(source.DBType));
                writer.WriteElementString("HostName", source.HostName);
                writer.WriteElementString("DataBase", source.DataBase);
                writer.WriteElementString("UserName", source.UserName);
                writer.WriteElementString("Password", source.Password);
                writer.WriteElementString("Query", source.Query);
                writer.WriteElementString("PrimaryKey", source.PrimaryKey);
                writer.WriteStartElement("Fields");
                foreach (FieldProperties fp in source.Fields)
                {
                    writer.WriteStartElement("Field");
                    writer.WriteAttributeString("Name", fp.Name);
                    writer.WriteAttributeString("Caption", fp.Caption);
                    writer.WriteAttributeString("Boost", fp.Boost.ToString());
                    writer.WriteAttributeString("IsTitle", fp.IsTitle.ToString());
                    writer.WriteAttributeString("Order", fp.Order.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            #endregion
            #region IndexList
            foreach (IndexSet indexSet in indexList)
            {
                writer.WriteStartElement("Index");
                writer.WriteAttributeString("Name", indexSet.IndexName);
                writer.WriteAttributeString("Caption", indexSet.Caption);
                writer.WriteAttributeString("Type", IndexType.GetIndexTypeStr(indexSet.Type));
                writer.WriteElementString("Source", indexSet.SourceName);
                writer.WriteElementString("Path", indexSet.Path);
                writer.WriteEndElement();
            }
            #endregion
            #region File Index
            writer.WriteStartElement("FileIndex");
            writer.WriteElementString("Path", fileSet.Path);
            writer.WriteStartElement("Directories");
            if (fileSet.BaseDirs != null)
            {
                foreach (string dir in fileSet.BaseDirs)
                {
                    writer.WriteElementString("Directory", dir);
                }
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion
            #region Dictionary
            writer.WriteStartElement("Dictionary");
            writer.WriteElementString("BasePath", dictSet.BasePath);
            writer.WriteElementString("NamePath", dictSet.NamePath);
            writer.WriteElementString("NumberPath", dictSet.NumberPath);
            writer.WriteElementString("FilterPath", dictSet.FilterPath);
            writer.WriteStartElement("CustomPaths");
            if (dictSet.CustomPaths != null)
            {
                foreach (string path in dictSet.CustomPaths)
                {
                    writer.WriteElementString("Path", path);
                }
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            #endregion
            #region Indexer
            writer.WriteStartElement("Indexer");
            writer.WriteElementString("MainIndexTime", indexer.MainIndexReCreateTime.ToShortTimeString());
            writer.WriteElementString("MainIndexSpan", indexer.MainIndexReCreateTimeSpan.ToString());
            writer.WriteElementString("IncrIndexSpan", indexer.IncrIndexReCreateTimeSpan.ToString());
            writer.WriteElementString("RamBufferSize", indexer.RamBufferSize.ToString());
            writer.WriteElementString("MaxBufferedDocs", indexer.MaxBufferedDocs.ToString());
            writer.WriteElementString("MaxFieldLength", indexer.MaxFieldLength.ToString());
            writer.WriteElementString("MergeFactor", indexer.MergeFactor.ToString());
            writer.WriteEndElement();
            #endregion
            #region Searchd
            writer.WriteStartElement("Searchd");
            writer.WriteElementString("Address", searchd.Address);
            writer.WriteElementString("LogPath", searchd.LogPath);
            writer.WriteElementString("MaxChildren", searchd.MaxChildren.ToString());
            writer.WriteElementString("MaxMatches", searchd.MaxMatches.ToString());
            writer.WriteElementString("MaxTrans", searchd.MaxTrans.ToString());
            writer.WriteElementString("Port", searchd.Port.ToString());
            writer.WriteElementString("QueryLog", searchd.QueryLogPath);
            writer.WriteElementString("TimeOut", searchd.TimeOut.ToString());
            writer.WriteEndElement();
            #endregion
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
                    case "Source":
                        #region Read Source
                        Source source = new Source();
                        source.SourceName = SupportClass.File.GetXmlAttribute(reader, "Name", typeof(string));
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
                                    source.DBType = DbType.GetDbType(reader.ReadElementString());
                                    break;
                                case "HostName":
                                    source.HostName = reader.ReadElementString();
                                    break;
                                case "DataBase":
                                    source.DataBase = reader.ReadElementString();
                                    break;
                                case "UserName":
                                    source.UserName = reader.ReadElementString();
                                    break;
                                case "Password":
                                    source.Password = reader.ReadElementString();
                                    break;
                                case "Query":
                                    source.Query = reader.ReadElementString();
                                    break;
                                case "PrimaryKey":
                                    source.PrimaryKey = reader.ReadElementString();
                                    break;
                                case "Fields":
                                    List<FieldProperties> fpList = new List<FieldProperties>();
                                    do
                                    {
                                        currentItemName = reader.Name;
                                        if (currentItemName == currentNodeName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                                        {
                                            break;
                                        }
                                        switch (currentItemName)
                                        {
                                            case "Field":
                                                FieldProperties fp = new FieldProperties();
                                                fp.Name = SupportClass.File.GetXmlAttribute(reader, "Name", typeof(string));
                                                fp.Caption = SupportClass.File.GetXmlAttribute(reader, "Caption", typeof(string));
                                                fp.Boost = float.Parse(SupportClass.File.GetXmlAttribute(reader, "Boost", typeof(float)));
                                                fp.IsTitle = bool.Parse(SupportClass.File.GetXmlAttribute(reader, "IsTitle", typeof(bool)));
                                                fp.Order = int.Parse(SupportClass.File.GetXmlAttribute(reader, "Order", typeof(int)));
                                                fpList.Add(fp);
                                                reader.Read();
                                                break;
                                            default:
                                                reader.Read();
                                                break;
                                        }
                                    } while (true);
                                    source.Fields=fpList;
                                    reader.Read();
                                    break;
                                default :
                                    reader.Read();
                                    break;
                            }
                        } while (true);
                        this.sourceList.Add(source);
                        #endregion
                        reader.Read();
                        break;
                    case "Index":
                        #region Read Index
                        IndexSet indexSet = new IndexSet();
                        indexSet.IndexName = SupportClass.File.GetXmlAttribute(reader, "Name", typeof(string));
                        indexSet.Caption = SupportClass.File.GetXmlAttribute(reader, "Caption", typeof(string));
                        indexSet.Type = IndexType.GetIndexType(SupportClass.File.GetXmlAttribute(reader, "Type", typeof(string)));
                        do
                        {
                            currentNodeName = reader.Name;
                            if (currentNodeName == currentElementName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                            {
                                break;
                            }
                            switch (currentNodeName)
                            {
                                case "Source":
                                    indexSet.SourceName= reader.ReadElementString();
                                    break;
                                case "Path":
                                    indexSet.Path = reader.ReadElementString();
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        } while (true);
                        this.indexList.Add(indexSet);
                        #endregion
                        reader.Read();
                        break;
                    case "FileIndex":
                        #region Read FileIndex
                        do
                        {
                            currentNodeName = reader.Name;
                            if (currentNodeName == currentElementName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                            {
                                break;
                            }
                            switch (currentNodeName)
                            {
                                case "Path":
                                    this.fileSet.Path = reader.ReadElementString();
                                    break;
                                case "Directories":
                                    List<string> dirList = new List<string>();
                                    do
                                    {
                                        currentItemName = reader.Name;
                                        if (currentItemName == currentNodeName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                                        {
                                            break;
                                        }
                                        switch (currentItemName)
                                        {
                                            case "Directory":
                                                dirList.Add(reader.ReadElementString());
                                                break;
                                            default:
                                                reader.Read();
                                                break;
                                        }
                                    } while (true);
                                    this.fileSet.BaseDirs =dirList;
                                    reader.Read();
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        } while (true);
                        #endregion
                        reader.Read();
                        break;
                    case "Dictionary":
                        #region Read Dictionary
                        do
                        {
                            currentNodeName = reader.Name;
                            if (currentNodeName == currentElementName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                            {
                                break;
                            }
                            switch (currentNodeName)
                            {
                                case "BasePath":
                                    this.dictSet.BasePath = reader.ReadElementString();
                                    break;
                                case "NamePath":
                                    this.dictSet.NamePath = reader.ReadElementString();
                                    break;
                                case "NumberPath":
                                    this.dictSet.NumberPath = reader.ReadElementString();
                                    break;
                                case "FilterPath":
                                    this.dictSet.FilterPath = reader.ReadElementString();
                                    break;
                                case "CustomPaths":
                                    List<string> pathList = new List<string>();
                                    do
                                    {
                                        currentItemName = reader.Name;
                                        if (currentItemName == currentNodeName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                                        {
                                            break;
                                        }
                                        switch (currentItemName)
                                        {
                                            case "Path":
                                                pathList.Add(reader.ReadElementString());
                                                break;
                                            default:
                                                reader.Read();
                                                break;
                                        }
                                    } while (true);
                                    this.dictSet.CustomPaths = pathList;
                                    reader.Read();
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        } while (true);
                        #endregion
                        reader.Read();
                        break;
                    case "Indexer":
                        #region Read Indexer
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
                                    this.indexer.MainIndexReCreateTime = DateTime.Parse(reader.ReadElementString());
                                    break;
                                case "MainIndexSpan":
                                    this.indexer.MainIndexReCreateTimeSpan = int.Parse(reader.ReadElementString());
                                    break;
                                case "IncrIndexSpan":
                                    this.indexer.IncrIndexReCreateTimeSpan = int.Parse(reader.ReadElementString());
                                    break;
                                case "RamBufferSize":
                                    this.indexer.RamBufferSize = double.Parse(reader.ReadElementString());
                                    break;
                                case "MaxBufferedDocs":
                                    this.indexer.MaxBufferedDocs = int.Parse(reader.ReadElementString());
                                    break;
                                case "MaxFieldLength":
                                    this.indexer.MaxFieldLength = int.Parse(reader.ReadElementString());
                                    break;
                                case "MergeFactor":
                                    this.indexer.MergeFactor = int.Parse(reader.ReadElementString());
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        } while (true);
                        #endregion
                        reader.Read();
                        break;
                    case "Searchd":
                        #region Read Searchd
                        do
                        {
                            currentNodeName = reader.Name;
                            if (currentNodeName == currentElementName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                            {
                                break;
                            }
                            switch (currentNodeName)
                            {
                                case "Address":
                                    this.searchd.Address = reader.ReadElementString();
                                    break;
                                case "LogPath":
                                    this.searchd.LogPath = reader.ReadElementString();
                                    break;
                                case "MaxChildren":
                                    this.searchd.MaxChildren = int.Parse(reader.ReadElementString());
                                    break;
                                case "MaxMatches":
                                    this.searchd.MaxMatches = int.Parse(reader.ReadElementString());
                                    break;
                                case "MaxTrans":
                                    this.searchd.MaxTrans = int.Parse(reader.ReadElementString());
                                    break;
                                case "Port":
                                    this.searchd.Port = int.Parse(reader.ReadElementString());
                                    break;
                                case "TimeOut":
                                    this.searchd.TimeOut = int.Parse(reader.ReadElementString());
                                    break;
                                case "QueryLog":
                                    this.searchd.QueryLogPath = reader.ReadElementString();
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        } while (true);
                        #endregion
                        reader.Read();
                        break;
                    default:
                        reader.Read();
                        break;

                }
            } while (true);
        }
        #endregion
        #region Function
        public List<Source> GetSourceList()
        {
            return sourceList;
        }
        public List<IndexSet> GetIndexList()
        {
            return indexList;
        }
        public DictionarySet GetDictionarySet()
        {
            return dictSet;
        }
        public IndexerSet GetIndexer()
        {
            return indexer;
        }
        public SearchSet GetSearchd()
        {
            return searchd;
        }
        #endregion
    }
}
