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
                string[] strArray = SupportClass.String.Split(srcTF, " .\t");
                if (strArray.Length != 2)
                    throw new ArgumentException("srcTF has bad format!", "srcTF");
                table = strArray[0];
                field = strArray[1];
            }
            public override string ToString()
            {
                return table +"."+field;
            }
        }
        [Serializable]
        public sealed class FilterCondition
        {
            private string table="";
            private string field="";
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
            public FilterCondition(string tablename, string fieldname, params string[] valuelist)
            {
                table = tablename;
                field = fieldname;
                values = valuelist;
            }
            public FilterCondition(string srcFC)
            {
                //srcFC format: table.field in "value1,value2,value3,....,valuen"
                string[] strArray = SupportClass.String.Split(srcFC, " .\t,，\"“");
                if (strArray.Length < 4)
                    throw new ArgumentException("srcFC has bad format!", "srcFC");
                table = strArray[0];
                field = strArray[1];
                string[] valueArray = new string[strArray.Length - 3];
                for (int i = 3; i < strArray.Length; i++)
                    valueArray[i - 3] = strArray[i];
                values = valueArray;
            }
            public override string ToString()
            {
                string ret = table + "." + field+" in (";
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
        #region "模糊查询"
        [Serializable]
        public sealed class FuzzyQuery
        {
            private string indexnames = "";
            public string IndexNames
            {
                get { return indexnames; }
                set { indexnames = value; }
            }
            private string wordsAllContains = "";
            public string WordsAllContains
            {
                get { return wordsAllContains; }
                set { wordsAllContains = value; }
            }
            private string exactPhraseContain = "";
            public string ExactPhraseContain
            {
                get { return exactPhraseContain; }
                set { exactPhraseContain = value; }
            }
            private string oneOfWordsAtLeastContain = "";
            public string OneOfWordsAtLeastContain
            {
                get { return oneOfWordsAtLeastContain; }
                set { oneOfWordsAtLeastContain = value; }
            }
            private string wordNotInclude = "";
            public string WordNotInclude
            {
                get { return wordNotInclude; }
                set { wordNotInclude = value; }
            }
            private string queryAts = "";
            public string QueryAts
            {
                get { return queryAts; }
                set { queryAts = value; }
            }
            public string SearchWords
            {
                get { return wordsAllContains; }
                set { wordsAllContains = value; }
            }
            public override string ToString()
            {
                return indexnames + "\t" + queryAts + "\t" + wordsAllContains + "\t" + exactPhraseContain + "\t" + oneOfWordsAtLeastContain + "\t" + wordNotInclude;
            }
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
