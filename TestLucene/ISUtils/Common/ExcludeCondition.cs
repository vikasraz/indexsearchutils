using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public sealed class ExcludeCondition:TableField
    {
        private string[] values;
        public string[] Values
        {
            get { return values; }
            set { values = value; }
        }
        public ExcludeCondition(TableField tf, params string[] valueList)
            :base(tf.Table,tf.Field)
        {
            if (valueList.Length<=0)
                throw new ArgumentNullException("valueList","Do not input any value for ExcludeCondition");
            values = valueList;
        }
        public ExcludeCondition(string tablename, string fieldname, params string[] valuelist)
            :base(tablename,fieldname)
        {
            if (valuelist.Length <= 0)
                throw new ArgumentNullException("valuelist", "Do not input any value for ExcludeCondition");
            values = valuelist;
        }
        public ExcludeCondition(string srcEC)
        {
            if (string.IsNullOrEmpty(srcEC))
                throw new ArgumentException("srcEC has bad format!", "srcEC");
            //srcFC format: table.field in "value1,value2,value3,....,valuen"
            int posNot = srcEC.ToLower().IndexOf("not");
            if (posNot <= 0)
                throw new ArgumentException("srcEC has bad format!", "srcEC");
            string tf = SupportClass.String.LeftOf(srcEC, posNot);
            SupportClass.QueryParser.TableFieldOf(tf, out table,out field);
            string leave;
            leave = srcEC.Substring(posNot + 3);
            values = SupportClass.String.Split(leave, " \t,，\"“”");
        }
        public override string ToString()
        {
            string ret=base.ToString() ;
            ret += " not (";
            foreach (string s in values)
            {
                ret += s + ",";
            }
            if (values.Length > 0)
            {
                ret = ret.Substring(0, ret.Length - 1);
            }
            ret += ")";
            return ret;
        }
    }
}
