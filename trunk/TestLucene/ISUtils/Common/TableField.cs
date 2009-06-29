using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public sealed class TableField
    {
        private string table = "";
        private string field = "";
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
        public TableField(string tablename, string fieldname)
        {
            table = tablename;
            field = fieldname;
        }
        public TableField(string srcTF)
        {
            if (string.IsNullOrEmpty(srcTF.Trim()))
                throw new ArgumentException("srcTF has bad format!", "srcTF");
            string[] strArray = SupportClass.String.Split(srcTF, " .\t");
            if (strArray.Length < 2)
            {
                table = "";
                field = strArray[0];
            }
            else
            {
                table = strArray[0];
                field = strArray[1];
            }
        }
        public override string ToString()
        {
            if (string.IsNullOrEmpty(table))
                return field;
            return table + "." + field;
        }
    }
}
