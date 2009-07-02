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
        private List<Document> docList = new List<Document>();
        public List<Document> Docs
        {
            get 
            {
                if (docList == null)
                    docList = new List<Document>();
                return docList;
            }
            set
            {
                docList = value;
            }
        }
        #endregion
        #region "构造函数"
        public SearchResult()
        { 
        }
        public SearchResult(List<Document> docList)
        {
            this.docList = docList;
        }
        #endregion
        #region "添加数据接口"
        public void AddResult(List<Document> docList)
        {
            if (this.docList == null)
                this.docList = new List<Document>();
            this.docList.AddRange(docList);
        }
        public void AddResult(Document[] docs)
        {
            if (this.docList == null)
                this.docList = new List<Document>();
            this.docList.AddRange(docs); 
        }
        public void AddResult(Document doc)
        {
            if (this.docList == null)
                this.docList = new List<Document>();
            this.docList.Add(doc);
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
            string fieldName;
            string fieldValue;
            float fieldBoost;

            this.docList.Clear();
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
                        Document doc = new Document();
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
                                    fieldName=reader.GetAttribute("Name");
                                    fieldValue= reader.GetAttribute("Value");
                                    fieldBoost = float.Parse(reader.GetAttribute("Boost"));
                                    doc.Add(new Field(fieldName,fieldValue, Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                                    doc.GetField(fieldName).SetBoost(fieldBoost);
                                    reader.Read();
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        } while (true);
                        this.docList.Add(doc);
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
            foreach (Document doc in docList)
            {
                writer.WriteStartElement("Doc");
                Field[] fields = new Field[doc.GetFields().Count];
                doc.GetFields().CopyTo(fields, 0);
                foreach (Field field in fields)
                {
                    writer.WriteStartElement("Field");
                    writer.WriteAttributeString("Name",field.Name());
                    writer.WriteAttributeString("Value", field.StringValue());
                    writer.WriteAttributeString("Boost", field.GetBoost().ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
