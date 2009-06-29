using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    public enum FilterConditonType
    {
        In,        //in
        Between,   // between and 
        Equal,     //=
        Large,     //>
        Small,     //<
        UnEqual,   //<>
        Other      //else
    }
    [Serializable]
    public sealed class FilterCondition
    {
        private FilterConditonType type=FilterConditonType.In;
        public FilterConditonType Type
        {
            get { return type; }
            set { type = value; }
        }
        private string table = "";
        private string field = "";
        private string[] values;
        public string Table
        {
            get { return table; }
            set { table = value; }
        }
        public string Field
        {
            get { return field; }
            set { field = value; }
        }
        public string[] Values
        {
            get { return values; }
            set { values = value; }
        }
        public FilterCondition(TableField tf, params string[] valueList)
        {
            table = tf.Table;
            field = tf.Field;
            values = valueList;
        }
        private FilterConditonType TypeOf(int num)
        {
            if (num < 0)
                return FilterConditonType.Other;
            switch (num)
            {
                case 0:
                    return FilterConditonType.Other;
                case 1:
                    return FilterConditonType.Equal;
                case 2:
                    return FilterConditonType.Between;
                default:
                    return FilterConditonType.In;
            }
        }
        public FilterCondition(string tablename, string fieldname, params string[] valuelist)
        {
            table = tablename;
            field = fieldname;
            values = valuelist;
        }
        public FilterCondition(string srcFC)
        {
            if (string.IsNullOrEmpty(srcFC.Trim()))
                throw new ArgumentException("srcTF has bad format!", "srcTF");
            //srcFC format: table.field in "value1,value2,value3,....,valuen"
            int posIn = srcFC.ToLower().IndexOf("in");
            int posEq = srcFC.IndexOf('=');
            int posUEq=srcFC.IndexOf("<>");
            int posLarge=srcFC.IndexOf(">");
            int posSmall=srcFC.IndexOf("<");
            int pos=SupportClass.Numerical.Max(posEq,posIn,posLarge,posSmall,posUEq);
            if (pos <= 0)
                throw new ArgumentException("srcTF has bad format!", "srcTF");
            string tf = SupportClass.String.LeftOf(srcFC, pos);
            SupportClass.QueryParser.TableFieldOf(tf, out table,out field);
            string leave;
            string[] strArray;
            if (posIn > 0)
            {
                leave = srcFC.Substring(posIn + 2);
                values = SupportClass.String.Split(leave, " \t,，\"“”");
                type = FilterConditonType.In;

            }
            if (posEq > 0)
            {
                leave = srcFC.Substring(posEq + 1);
                values = SupportClass.String.Split(leave, " \t,，\"“”");
                type = FilterConditonType.Equal;
            }
            if (posUEq > 0)
            {
                leave = srcFC.Substring(posUEq + 2);
                values = SupportClass.String.Split(leave, " \t,，\"“”");
                type = FilterConditonType.UnEqual;
            }
            if (posLarge > 0)
            {
                leave = srcFC.Substring(posLarge + 1);
                values = SupportClass.String.Split(leave, " \t,，\"“”");
                type = FilterConditonType.Large;
            }
            if (posSmall > 0)
            {
                leave = srcFC.Substring(posSmall + 2);
                values = SupportClass.String.Split(leave, " \t,，\"“”");
                type = FilterConditonType.Small;
            }
        }
        public override string ToString()
        {
            string ret ;
            if (string.IsNullOrEmpty(table))
                ret = field;
            else
               ret= table + "." + field ;
           switch (type)
           {
               case FilterConditonType.Between :
                    ret += string.Format(" between {0} and {1}", values[0], values[1]);
                    break;
               case FilterConditonType.Equal:
                    ret += string.Format(" = {0}", values[0]);
                    break;
               case FilterConditonType.In :
                   ret += " in (";
                    foreach (string s in values)
                    {
                        ret += s + ",";
                    }
                    if (values.Length > 0)
                    {
                        ret = ret.Substring(0, ret.Length - 1);
                    }
                    ret += ")";
                    break;
               case FilterConditonType.Large:
                   ret += string.Format(" > {0}", values[0]);
                   break;
               case FilterConditonType.Small:
                   ret += string.Format(" > {0}", values[0]);
                   break;
               case FilterConditonType.UnEqual :
                   ret += string.Format(" <> {0}", values[0]);
                   break;
               default :
                   foreach (string s in values)
                   {
                       ret += s + ",";
                   }
                   if (values.Length > 0)
                   {
                       ret = ret.Substring(0, ret.Length - 1);
                   }
                   break;
           }
            return ret;
        }
    }
}
