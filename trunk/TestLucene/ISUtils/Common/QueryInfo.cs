using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

namespace ISUtils.Common
{
    [Serializable]
    public class QueryInfo
    {
        #region "精确查询"
        [Serializable]
        public sealed class RangeConditon
        {
            //true for open interval,else closed interval
            private bool interval = false;
            public bool IntervalType
            {
                get { return interval; }
                set { interval = value; }
            }
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
            private string from = "";
            private string to = "";
            public string RangeFrom
            {
                get { return from; }
                set { from = value; }
            }
            public string RangeTo
            {
                get { return to; }
                set { to = value; }
            }
            public RangeConditon()
            { 
            }
            public RangeConditon(string tablename, string fieldname, string from, string to)
            {
                table = tablename;
                field = fieldname;
                this.from = from;
                this.to = to;
            }
            public RangeConditon(string tablename, string fieldname, DateTime from, DateTime to)
            {
                table = tablename;
                field = fieldname;
                this.from = SupportClass.Time.GetLuceneDate(from);
                this.to = SupportClass.Time.GetLuceneDate(to);
            }
            public RangeConditon(string srcRC)
            {
                //table.field:[from TO to]
                //table.field:{from TO to}
                int pos = srcRC.IndexOf(':');
                string[] strArray = SupportClass.String.Split(srcRC.Substring(0,pos), ".");
            }
        }
        [Serializable]
        public sealed class SqlQuery
        {
            #region "Properties"
            private TableField[] resultFields;
            public TableField[] ResultFields
            {
                get { return resultFields; }
                set { resultFields = value; }
            }
            private string[] tables;
            public string[] SearchTables
            {
                get { return tables; }
                set { tables = value; }
            }
            private List<FilterCondition> filterConList=new List<FilterCondition>();
            public List<FilterCondition> FilterList
            {
                get { return filterConList; }
                set { filterConList = value; }
            }
            #endregion
            #region "Functions"
            public void SetResultFields(params string[] fieldArray)
            {
                List<TableField> tfList=new List<TableField>();
                foreach(string result in fieldArray)
                {
                    tfList.Add(new TableField(result));
                }
                resultFields = tfList.ToArray();
            }
            public void SetResultFileds(List<string> fieldList)
            {
                List<TableField> tfList = new List<TableField>();
                foreach (string result in fieldList)
                {
                    tfList.Add(new TableField(result));
                }
                resultFields = tfList.ToArray();
            }
            public void SetSearchTables(params string[] tableArray)
            {
                tables = tableArray;
            }
            public void SetSearchTables(List<string> tableList)
            {
                tables = tableList.ToArray();
            }
            public void SetFilters(params FilterCondition[] filters)
            {
                if (filterConList == null)
                    filterConList = new List<FilterCondition>();
                filterConList.Clear();
                filterConList.AddRange(filters);
            }
            public void AddFilter(FilterCondition fc)
            {
                if (filterConList == null)
                    filterConList = new List<FilterCondition>();
                filterConList.Add(fc);
            }
            public void AddFilters(params FilterCondition[] fcArray)
            {
                if (filterConList == null)
                    filterConList = new List<FilterCondition>();
                filterConList.AddRange(fcArray);
            }
            public void AddFilters(List<FilterCondition> fcList)
            {
                if (filterConList == null)
                    filterConList = new List<FilterCondition>();
                filterConList.AddRange(fcList);
            }
            #endregion
            #region "override"
            public override string ToString()
            {
                StringBuilder  ret=new StringBuilder();
                ret.Append( "select ");
                foreach (TableField tf in resultFields)
                {
                    ret .Append( tf.ToString() + ",");
                }
                if (resultFields.Length > 0)
                    ret.Remove(ret.Length-1,1);
                ret.Append(" from ");
                foreach (string table in tables)
                {
                    ret.Append(table + ",");
                }
                if (tables.Length >0)
                    ret.Remove(ret.Length-1,1);
                ret.Append(" where ");
                foreach (FilterCondition fc in filterConList)
                {
                    ret.Append(fc.ToString()+" and ");
                }
                if (filterConList.Count >0)
                    ret.Remove(ret.Length-5,1);
                return ret.ToString();
            }
            #endregion
        }
        #endregion
        private bool isFuzzySearch = true;
        public bool IsFuzzySearch
        {
            get { return isFuzzySearch; }
            set { isFuzzySearch = value; }
        }
        private SqlQuery sqlQuery=new SqlQuery();
        private FuzzyQuery fuzzyQuery = new FuzzyQuery();
        public SqlQuery SQuery
        {
            get
            {
                if (!isFuzzySearch)
                    return sqlQuery;
                else
                    return null;
            }
            set
            {
                if (!isFuzzySearch)
                    sqlQuery = value;
            }
        }
        public FuzzyQuery FQuery
        {
            get
            {
                if (isFuzzySearch)
                    return fuzzyQuery;
                else
                    return null;
            }
            set
            {
                if (isFuzzySearch)
                    fuzzyQuery = value;
            }
        }
        public override string ToString()
        {
            if (isFuzzySearch)
                return fuzzyQuery.ToString();
            else
                return sqlQuery.ToString();
        }
    }
}
