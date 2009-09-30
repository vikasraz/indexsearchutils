using System;
using Lucene.Net.Documents;

namespace ISUtils.Common
{
    [Serializable]
    public class SearchField:FieldBase,IComparable
    {
        #region 属性
        private string value = "";
        private string result = "";
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public string Result
        {
            get { return result; }
            set { result = value; }
        }
        #endregion
        #region 构造函数
        public SearchField()
        { 
        }
        public SearchField(string fieldname, string caption, string value, string result, float boost, bool isTitle, bool visible, int order)
            : base(fieldname, caption, boost, isTitle, visible, order)
        {
            this.value = value;
            this.result = result;
        }
        public SearchField(string fieldname,string caption, string value,string result, float boost, bool isTitle)
            :this(fieldname,caption,value,result,boost,isTitle,true,0)
        {
        }
        public SearchField(string fieldname, string caption, string value,string result, float boost)
            : this(fieldname, caption, value, result, boost, false, false, 0)
        {
        }
        public SearchField(string fieldname, string caption, string value, float boost,bool isTitle)
            : this(fieldname, caption, value, value, boost, isTitle, true, 0)
        {
        }
        public SearchField(string fieldname, string caption, string value, string result)
            : this(fieldname, caption, value, result, 1.0f, false, false, 0)
        {

        }
        public SearchField(string fieldname, string value,string result)
            : this(fieldname, fieldname, value, result, 1.0f, false, false, 0)
        {

        }
        public SearchField(string fieldname, string value)
            : this(fieldname, fieldname, value, value, 1.0f, false, false, 0)
        {
 
        }
        public SearchField(string fieldname, string caption, string value, float boost)
            : this(fieldname, caption, value, value, boost, false, false, 0)
        {
        }
        public SearchField(string fieldname, string value, float boost)
            : this(fieldname, fieldname, value, value, boost, false, false, 0)
        {
        }
        public SearchField(Field field)
            : this(field.Name(), field.Name(), field.StringValue(), field.StringValue(), field.GetBoost(), false, false, 0)
        { 
        }
        public SearchField(Field field, bool isTitle)
            : this(field.Name(), field.Name(), field.StringValue(), field.StringValue(), field.GetBoost(), isTitle, true, 0)
        { 
        }
        public SearchField(Field field, string caption, bool isTitle)
            : this(field.Name(), caption, field.StringValue(), field.StringValue(), field.GetBoost(), isTitle, true, 0)
        { 
        }
        public SearchField(Field field, IndexField properties)
            : this(field.Name(), properties.Caption, field.StringValue(), field.StringValue(), properties.Boost, properties.IsTitle, true, properties.Order)
        { 
        }
        #endregion
        #region 全局方法
        public static implicit operator SearchField(Field field)
        {
            return new SearchField(field);
        }
        #endregion
        #region IComparable
        private static ReverserInfo.Direction direct=ReverserInfo.Direction.ASC;
        public static ReverserInfo.Direction Direction
        {
            get { return direct; }
            set { direct = value; }
        }
        public int CompareTo(object obj)
        {
            if(!(obj is SearchField))
                throw new InvalidCastException("This object is not of type SearchField");
            SearchField sf = (SearchField)obj;
            if (Direction == ReverserInfo.Direction.ASC)
            {
                return this.order - sf.order;
            }
            else
            {
                return sf.order - this.order;
            }
        }
        #endregion
    }
}
