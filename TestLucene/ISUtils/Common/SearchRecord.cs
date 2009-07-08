using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Documents;

namespace ISUtils.Common
{
    [Serializable]
    public class SearchRecord
    {
        #region 属性
        private string name = "";
        private string caption = "";
        private string index = "";
        private List<SearchField> fieldList = new List<SearchField>();
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }
        public string IndexName
        {
            get { return index; }
            set { index = value; }
        }
        public List<SearchField> Fields
        {
            get { return fieldList; }
            set { fieldList = value; }
        }
        public SearchField this[int index]
        {
            get 
            {
                if (fieldList==null)
                    return null;
                if (index < 0 || index >= fieldList.Count)
                    return null;
                return fieldList[index];
            }
            set
            {
                if (fieldList == null)
                    return;
                if (index >= 0 || index < fieldList.Count)
                    fieldList[index] = value;
            }
        }
        public SearchField this[string name]
        {
            get
            {
                if (fieldList == null)
                    return null;
                foreach (SearchField sf in fieldList)
                {
                    if (sf.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                        return sf;
                }
                return null;
            }
            set
            {
                if (fieldList == null)
                    return;
                foreach (SearchField sf in fieldList)
                {
                    if (sf.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        fieldList.Remove(sf);
                        fieldList.Add(value);
                    }
                }
            }
        }
        #endregion
        #region 构造函数
        public SearchRecord()
        {
            if (fieldList == null)
                fieldList = new List<SearchField>();
        }
        public SearchRecord(string name, string caption, string index)
        {
            this.name = name;
            this.caption = caption;
            this.index = index;
            if (this.fieldList == null)
                this.fieldList = new List<SearchField>();
        }
        public SearchRecord(string name, string caption, string index, List<SearchField> fields)
        {
            this.name = name;
            this.caption = caption;
            this.index = index;
            this.fieldList = fields;
            if (this.fieldList == null)
                this.fieldList = new List<SearchField>();
        }
        public SearchRecord(string index, List<SearchField> fields)
            : this(string.Empty, string.Empty, index, fields)
        { 
        }
        public SearchRecord(string name, string caption, string index, params SearchField[] fields)
        {
            this.name = name;
            this.caption = caption;
            this.index = index;
            if (this.fieldList == null)
                this.fieldList = new List<SearchField>();
            this.fieldList.AddRange(fields);
        }
        public SearchRecord(string index, params SearchField[] fields)
            : this(string.Empty, string.Empty, index, fields)
        { 
        }
        public SearchRecord(string name, string caption, string index, Document doc)
            :this(name,caption,index)
        {
            Field[] fields = new Field[doc.GetFields().Count];
            doc.GetFields().CopyTo(fields, 0);
            foreach (Field field in fields)
            {
                fieldList.Add(new SearchField(field));
            }
        }
        public SearchRecord(Document doc)
            :this(string.Empty,string.Empty,string.Empty,doc)
        {           
        }
        public SearchRecord(IndexSet set, List<SearchField> fields)
            : this(set.IndexName, set.Caption, set.IndexName, fields)
        { 
        }
        #endregion
        #region Override
        public override string ToString()
        {
            StringBuilder title = new StringBuilder();
            StringBuilder content = new StringBuilder();
            title.Append(name+" "+caption + " ");
            foreach (SearchField sf in fieldList)
            {
                if (sf.IsTitle)
                    title.Append(sf.Name+":"+sf.Value + "\t");
                else
                    content.Append(sf.Name +":"+sf.Value + "\t");
            }
            title.Append("\n");
            content.Append("\n");
            return title.ToString() + content.ToString();
        }
        #endregion
        #region 方法
        public void Add(SearchField field)
        {
            if (fieldList == null)
                fieldList = new List<SearchField>();
            fieldList.Add(field);
        }
        public string ToWebString()
        {
            StringBuilder title = new StringBuilder();
            StringBuilder content=new StringBuilder();
            title.Append(caption + "&nbsp;");
            foreach (SearchField sf in fieldList)
            {
                if (sf.IsTitle)
                    title.Append(sf.Value + "&nbsp;");
                else
                    content.Append(sf.Value + "&nbsp;");
            }
            title.Append("<br>");
            content.Append("<br>");
            return title.ToString() + content.ToString();
        }
        public void GetWebInfo(out string szTitle, out string szContent)
        {
            StringBuilder title = new StringBuilder();
            StringBuilder content = new StringBuilder();
            //title.Append(caption + "&nbsp;");
            foreach (SearchField sf in fieldList)
            {
                if (sf.IsTitle)
                    title.Append(sf.Caption +":"+sf.Value + "&nbsp;");
                else
                    content.Append(sf.Caption+":"+sf.Value + "&nbsp;");
            }
            szTitle = title.ToString();
            szContent = content.ToString();
        }
        #endregion
        #region 全局方法
        public static implicit operator SearchRecord(Document doc)
        {
            return new SearchRecord(doc);
        }
        public static List<SearchRecord> ToList(List<Document> docList)
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            foreach (Document doc in docList)
            {
                recordList.Add(new SearchRecord(doc));
            }
            return recordList;
        }
        public static List<SearchRecord> ToList(params Document[] docs)
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            foreach (Document doc in docs)
            {
                recordList.Add(new SearchRecord(doc));
            }
            return recordList;
        }
        #endregion
    }
}
