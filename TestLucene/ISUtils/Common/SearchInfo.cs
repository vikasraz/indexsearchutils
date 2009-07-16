using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ISUtils.Common
{
    [Serializable]
    public class SearchInfo : IXmlSerializable
    {
        #region "常量"
        public const int DEFPAGESIZE=10;
        public const int MINPAGESIZE = 0;
        #endregion
        #region "属性"
        private QueryInfo queryInfo=new QueryInfo();
        public QueryInfo Query
        {
            get { return queryInfo; }
            set { queryInfo = value; }
        }
        private int pageSize = DEFPAGESIZE;
        public int PageSize
        {
            get 
            { 
                return pageSize; 
            }
            set 
            { 
                pageSize = value;
                if (pageSize < MINPAGESIZE)
                    pageSize = MINPAGESIZE;
            }
        }
        private int pageNum = 1;
        public int PageNum
        {
            get 
            { 
                return pageNum; 
            }
            set 
            { 
                pageNum = value;
                if (pageNum <= 0)
                    pageNum = 1;
            }
        }
        private bool highLight = false;
        public bool HighLight
        {
            get { return highLight; }
            set { highLight = value; }
        }
        private string filter = "";
        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }
        #endregion
        #region "重写"
        public override string ToString()
        {
            return queryInfo.ToString()+"\tPageSize="+pageSize.ToString()+"\tPageNum="+pageNum.ToString()+"\tHighLight="+highLight.ToString()+"\tFilter="+filter;
        }
        #endregion
        #region "IXmlSerializable"
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }
        public void ReadXml(XmlReader reader)
        {
            string startElementName = reader.Name;
            string currentElementName,currentNodeName,currentItemName;
            string table,field,from,to;
            bool interval;
            List<string> itemList=new List<string>();
            this.queryInfo.FilterList.Clear();
            this.queryInfo.ExcludeList.Clear();
            this.queryInfo.RangeList.Clear();
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
                    case "PageNum":
                        this.pageNum = int.Parse(reader.ReadElementString());
                        break;

                    case "PageSize":
                        this.pageSize = int.Parse(reader.ReadElementString());
                        break;
                    case "HighLight":
                        this.highLight = bool.Parse(reader.ReadElementString());
                        break;
                    case "Filter":
                        this.filter = reader.ReadElementString();
                        break;
                    case "QueryInfo":
                        this.queryInfo.IndexNames = SupportClass.FileUtil.GetXmlAttribute(reader,"IndexNames",typeof(string));
                        this.queryInfo.QueryAts = SupportClass.FileUtil.GetXmlAttribute(reader, "QueryAts", typeof(string));
                        this.queryInfo.WordsAllContains = SupportClass.FileUtil.GetXmlAttribute(reader, "WordsAllContains", typeof(string));
                        this.queryInfo.ExactPhraseContain = SupportClass.FileUtil.GetXmlAttribute(reader, "ExactPhraseContain", typeof(string));
                        this.queryInfo.OneOfWordsAtLeastContain = SupportClass.FileUtil.GetXmlAttribute(reader, "OneOfWordsAtLeastContain", typeof(string));
                        this.queryInfo.WordNotInclude = SupportClass.FileUtil.GetXmlAttribute(reader, "WordNotInclude", typeof(string));
                        do
                        {
                            currentNodeName = reader.Name;
                            if (currentNodeName == currentElementName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                            {
                                break;
                            }
                            switch (currentNodeName)
                            {
                                case "FilterCondition":
                                    table = SupportClass.FileUtil.GetXmlAttribute(reader, "Table", typeof(string));
                                    field = SupportClass.FileUtil.GetXmlAttribute(reader, "Field", typeof(string));
                                    itemList.Clear();
                                    do 
                                    {
                                        currentItemName=reader.Name;
                                        if (currentItemName == currentNodeName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                                        {
                                            break;
                                        }
                                        switch (currentItemName)
                                        {
                                            case "Item":
                                                itemList.Add(reader.ReadElementString());
                                                reader.Read();
                                                break;
                                            default :
                                                reader.Read();
                                                break;
                                        }
                                    } while (true);
                                    this.queryInfo.FilterList.Add(new FilterCondition(table, field, itemList.ToArray()));
                                    reader.Read();
                                    break;
                                case "ExcludeCondition":
                                    table = SupportClass.FileUtil.GetXmlAttribute(reader, "Table", typeof(string));
                                    field = SupportClass.FileUtil.GetXmlAttribute(reader, "Field", typeof(string));
                                    itemList.Clear();
                                    do
                                    {
                                        currentItemName = reader.Name;
                                        if (currentItemName == currentNodeName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                                        {
                                            break;
                                        }
                                        switch (currentItemName)
                                        {
                                            case "Item":
                                                itemList.Add(reader.ReadElementString());
                                                reader.Read();
                                                break;
                                            default:
                                                reader.Read();
                                                break;
                                        }
                                    } while (true);
                                    this.queryInfo.ExcludeList.Add(new ExcludeCondition(table, field, itemList.ToArray()));
                                    reader.Read();
                                    break;
                                case "RangeCondition":
                                    table = SupportClass.FileUtil.GetXmlAttribute(reader, "Table", typeof(string));
                                    field = SupportClass.FileUtil.GetXmlAttribute(reader, "Field", typeof(string));
                                    from = SupportClass.FileUtil.GetXmlAttribute(reader, "From", typeof(string));
                                    to = SupportClass.FileUtil.GetXmlAttribute(reader, "To", typeof(string));
                                    interval = bool.Parse(SupportClass.FileUtil.GetXmlAttribute(reader, "Interval", typeof(bool)));
                                    RangeCondition rc = new RangeCondition(table, field, from, to);
                                    rc.IntervalType = interval;
                                    this.queryInfo.RangeList.Add(rc);
                                    reader.Read();
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        } while (true);
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
            writer.WriteElementString("PageNum", pageNum.ToString());
            writer.WriteElementString("PageSize", pageSize.ToString());
            writer.WriteElementString("HighLight", highLight.ToString());
            writer.WriteElementString("Filter", filter);
            writer.WriteStartElement("QueryInfo");
            writer.WriteAttributeString("IndexNames", queryInfo.IndexNames);
            writer.WriteAttributeString("QueryAts", queryInfo.QueryAts);
            writer.WriteAttributeString("WordsAllContains", queryInfo.WordsAllContains);
            writer.WriteAttributeString("ExactPhraseContain", queryInfo.ExactPhraseContain);
            writer.WriteAttributeString("OneOfWordsAtLeastContain", queryInfo.OneOfWordsAtLeastContain);
            writer.WriteAttributeString("WordNotInclude", queryInfo.WordNotInclude);
            foreach (FilterCondition fc in queryInfo.FilterList)
            {
                writer.WriteStartElement("FilterCondition");
                writer.WriteAttributeString("Table", fc.Table);
                writer.WriteAttributeString("Field", fc.Field);
                writer.WriteStartElement("Values");
                foreach (string value in fc.Values) 
                {
                    writer.WriteElementString("Item", value);
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            foreach (ExcludeCondition ec in queryInfo.ExcludeList)
            {
                writer.WriteStartElement("ExcludeCondition");
                writer.WriteAttributeString("Table", ec.Table);
                writer.WriteAttributeString("Field", ec.Field);
                writer.WriteStartElement("Values");
                foreach (string value in ec.Values) 
                {
                    writer.WriteElementString("Item", value);
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            foreach (RangeCondition rc in queryInfo.RangeList)
            {
                writer.WriteStartElement("RangeCondition");
                writer.WriteAttributeString("Table", rc.Table);
                writer.WriteAttributeString("Field", rc.Field);
                writer.WriteAttributeString("From", rc.RangeFrom);
                writer.WriteAttributeString("To", rc.RangeTo);
                writer.WriteAttributeString("Interval", rc.IntervalType.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        #endregion
    }
}
