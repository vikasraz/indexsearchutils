using System;
using Lucene.Net.Documents;

namespace ISUtils.Common
{
    [Serializable]
    public class SearchField
    {
        #region 属性
        private string name = "";
        private string caption = "";
        private string value = "";
        private float boost = 1.0f;
        private bool isTitle = false;
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
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public float Boost
        {
            get { return boost; }
            set { boost = value; }
        }
        public bool IsTitle
        {
            get { return isTitle; }
            set { isTitle = value; }
        }
        #endregion
        #region 构造函数
        public SearchField()
        { 
        }
        public SearchField(string fieldname,string caption, string value, float boost, bool isTitle)
        {
            this.name = fieldname;
            this.caption = caption;
            this.value = value;
            this.boost = boost;
            this.isTitle = isTitle;
        }
        public SearchField(string fieldname,string caption,string value)
            : this(fieldname, caption, value, 1.0f, false)
        {

        }
        public SearchField(string fieldname, string value)
            : this(fieldname,fieldname, value, 1.0f, false)
        {
 
        }
        public SearchField(string fieldname,string caption, string value, float boost)
            : this(fieldname, caption, value, boost, false)
        {
        }
        public SearchField(string fieldname, string value, float boost)
            : this(fieldname,fieldname, value, boost, false)
        {
        }
        public SearchField(Field field)
            : this(field.Name(),field.Name(), field.StringValue(), field.GetBoost(), false)
        { 
        }
        public SearchField(Field field, bool isTitle)
            : this(field.Name(), field.Name(),field.StringValue(), field.GetBoost(), isTitle)
        { 
        }
        public SearchField(Field field, string caption, bool isTitle)
            : this(field.Name(), caption, field.StringValue(), field.GetBoost(), isTitle)
        { 
        }
        public SearchField(Field field, FieldProperties properties)
            : this(field.Name(), properties.Caption, field.StringValue(), field.GetBoost(), properties.TitleOrContent)
        { 
        }
        #endregion
        #region 全局方法
        public static implicit operator SearchField(Field field)
        {
            return new SearchField(field);
        }
        #endregion
    }
}
