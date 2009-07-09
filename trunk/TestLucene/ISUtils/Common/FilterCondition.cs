using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public sealed class FilterCondition:TableField
    {
        private List<string> values=new List<string>();
        public List<string> Values
        {
            get { return values; }
            set { values = value; }
        }
        public FilterCondition(TableField tf, List<string> valueList)
            : base(tf.Table, tf.Field)
        {
            if (valueList==null)
                throw new ArgumentNullException("valueList", "Do not input any value for FilterCondition");
            values=valueList;
        }
        public FilterCondition(TableField tf, params string[] valueList)
            :base(tf.Table,tf.Field)
        {
            if (valueList.Length<=0)
                throw new ArgumentNullException("valueList","Do not input any value for FilterCondition");
            if (values == null)
                values = new List<string>();
            values.Clear();
            values.AddRange(valueList);
        }
        public FilterCondition(string tablename, string fieldname, List<string> valuelist)
            : base(tablename, fieldname)
        {
            if (valuelist==null)
                throw new ArgumentNullException("valuelist", "Do not input any value for FilterCondition");
            values = valuelist;
        }
        public FilterCondition(string tablename, string fieldname, params string[] valuelist)
            :base(tablename,fieldname)
        {
            if (valuelist.Length <= 0)
                throw new ArgumentNullException("valuelist", "Do not input any value for FilterCondition");
            if (values == null)
                values = new List<string>();
            values.Clear();
            values.AddRange(valuelist);
        }
        public FilterCondition(string srcFC)
        {
            if (string.IsNullOrEmpty(srcFC))
                throw new ArgumentException("srcFC has bad format!", "srcFC");
            //srcFC format: table.field in "value1,value2,value3,....,valuen"
            int posIn = srcFC.ToLower().IndexOf("in");
            if (posIn <= 0)
                throw new ArgumentException("srcFC has bad format!", "srcFC");
            string tf = SupportClass.String.LeftOf(srcFC, posIn);
            SupportClass.QueryParser.TableFieldOf(tf, out table, out field);
            string leave;
            leave = srcFC.Substring(posIn + 2);
            if (values == null)
                values = new List<string>();
            values.Clear();
            values.AddRange(SupportClass.String.Split(leave, " \t,，\"“”"));
        }
        public override string ToString()
        {
            string ret=base.ToString() ;
            ret += " in (";
            foreach (string s in values)
            {
                ret += s + ",";
            }
            if (values.Count > 0)
            {
                ret = ret.Substring(0, ret.Length - 1);
            }
            ret += ")";
            return ret;
        }
    }
}
