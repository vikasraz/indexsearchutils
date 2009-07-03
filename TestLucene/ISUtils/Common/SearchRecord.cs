using System;
using System.Collections.Generic;
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
            : this(set.IndexName, set.IndexName, set.IndexName, fields)
        { 
        }
        #endregion
        #region 方法
        public void Add(SearchField field)
        {
            if (fieldList == null)
                fieldList = new List<SearchField>();
            fieldList.Add(field);
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
