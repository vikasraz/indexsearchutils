using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Lucene.Net.Documents;

namespace ISUtils.Common
{
    [Serializable]
    public class SearchResult : IXmlSerializable
    {
        #region "属性"
        private int pageNum = 0;
        public int PageNum
        {
            get { return pageNum; }
            set { pageNum = value; }
        }
        private Lucene.Net.Search.Query query;
        public Lucene.Net.Search.Query Query
        {
            get { return query; }
            set { query = value; }
        }
        private int totalPages = 1;
        public int TotalPages
        {
            get { return totalPages; }
            set { totalPages = value; }
        }
        private List<SearchRecord> recordList = new List<SearchRecord>();
        public List<SearchRecord> Records
        {
            get 
            {
                if (recordList == null)
                    recordList = new List<SearchRecord>();
                return recordList;
            }
            set
            {
                recordList = value;
            }
        }
        #endregion
        #region "构造函数"
        public SearchResult()
        { 
        }
        public SearchResult(List<SearchRecord> recordList)
        {
            this.recordList = recordList;
        }
        public SearchResult(List<Document> docList)
        {
            this.recordList = SearchRecord.ToList(docList);
        }
        #endregion
        #region "添加数据接口"
        public void AddResult(List<Document> docList)
        {
            if (this.recordList == null)
                this.recordList = new List<SearchRecord>();
            this.recordList.AddRange(SearchRecord.ToList(docList));
        }
        public void AddResult(Document[] docs)
        {
            if (this.recordList == null)
                this.recordList = new List<SearchRecord>();
            this.recordList.AddRange(SearchRecord.ToList(docs)); 
        }
        public void AddResult(Document doc)
        {
            if (this.recordList == null)
                this.recordList = new List<SearchRecord>();
            this.recordList.Add(new SearchRecord(doc));
        }
        public void AddResult(List<SearchRecord> recordList)
        {
            if (this.recordList == null)
                this.recordList = new List<SearchRecord>();
            this.recordList.AddRange(recordList);
        }
        public void AddResult(SearchRecord[] records)
        {
            if (this.recordList == null)
                this.recordList = new List<SearchRecord>();
            this.recordList.AddRange(records);
        }
        public void AddResult(SearchRecord record)
        {
            if (this.recordList == null)
                this.recordList = new List<SearchRecord>();
            this.recordList.Add(record);
        }
        #endregion
        #region "重写"
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append("TotalPage=" + totalPages.ToString());
            ret.Append("\tPageNum=" + pageNum.ToString());
            if (query !=null)
               ret.Append("\t"+query.ToString());
            //foreach (Document doc in docList)
            //{
            //    ret.Append("\n\tDoc:");
            //    Field[] fields = new Field[doc.GetFields().Count];
            //    doc.GetFields().CopyTo(fields, 0);
            //    foreach (Field field in fields)
            //    {
            //        ret.Append("\n\t\t"+field.Name()+":\t"+field.StringValue()+"\t"+field.GetBoost().ToString());
            //    }
            //}
            return ret.ToString();
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
            string currentElementName;
            string currentNodeName;
            string fieldName,fieldValue,fieldCaption;
            float fieldBoost;
            bool isTitle;

            this.recordList.Clear();
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

                    case "TotalPages":
                        this.totalPages = int.Parse(reader.ReadElementString());
                        break;

                    case "Doc":
                        SearchRecord record = new SearchRecord();
                        record.Name = SupportClass.File.GetXmlAttribute(reader, "Name", typeof(string));
                        record.Caption = SupportClass.File.GetXmlAttribute(reader, "Caption", typeof(string));
                        record.IndexName = SupportClass.File.GetXmlAttribute(reader, "Index", typeof(string));
                        do
                        {
                            currentNodeName = reader.Name;
                            if (currentNodeName == currentElementName && (reader.MoveToContent() == XmlNodeType.EndElement || reader.IsEmptyElement))
                            {
                                break;
                            }
                            switch (currentNodeName)
                            {
                                case "Field":
                                    fieldName = SupportClass.File.GetXmlAttribute(reader, "Name", typeof(string));
                                    fieldValue = SupportClass.File.GetXmlAttribute(reader, "Value", typeof(string));
                                    fieldBoost = float.Parse(SupportClass.File.GetXmlAttribute(reader, "Boost", typeof(float)));
                                    fieldCaption = SupportClass.File.GetXmlAttribute(reader, "Caption", typeof(string));
                                    isTitle = bool.Parse(SupportClass.File.GetXmlAttribute(reader, "IsTitle", typeof(bool)));
                                    record.Add(new SearchField(fieldName,fieldCaption,fieldValue,fieldBoost,isTitle));
                                    reader.Read();
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        } while (true);
                        this.recordList.Add(record);
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
            writer.WriteElementString("TotalPages", totalPages.ToString());
            foreach (SearchRecord record in recordList)
            {
                writer.WriteStartElement("Doc");
                writer.WriteAttributeString("Name", record.Name);
                writer.WriteAttributeString("Caption", record.Caption);
                writer.WriteAttributeString("Index", record.IndexName);
                foreach (SearchField field in record.Fields)
                {
                    if (field.Visible)
                    {
                        writer.WriteStartElement("Field");
                        writer.WriteAttributeString("Name", field.Name);
                        writer.WriteAttributeString("Caption", field.Caption);
                        writer.WriteAttributeString("Value", field.Value);
                        writer.WriteAttributeString("Boost", field.Boost.ToString());
                        writer.WriteAttributeString("IsTitle", field.IsTitle.ToString());
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
