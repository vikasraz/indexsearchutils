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
        [Serializable]
        public sealed class TableField
        {
            private string table = "";
            private string field = "";
            public TableField(string tablename, string fieldname)
            {
                table = tablename;
                field = fieldname;
            }
        }
        [Serializable]
        public sealed class FilterCondition
        {
            private string table="";
            private string field="";
            private string[] values;
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
        }
        private bool isFuzzySearch = true;
        public bool IsFuzzySearch
        {
            get { return isFuzzySearch; }
            set { isFuzzySearch = value; }
        }
        private string[] resultFields;
        public string[] ResultFields
        {
            get { return resultFields; }
            set { resultFields = value; }
        }
        public void SetResultFields(params string[] fieldArray)
        {
            resultFields = fieldArray;
        }
        public void SetResultFileds(List<string> fieldList)
        {
            resultFields = fieldList.ToArray();
        }
        private string[] tables;
        public string[] SearchTables
        {
            get { return tables; }
            set { tables = value; }
        }
        public void SetSearchTables(params string[] tableArray)
        {
            tables = tableArray;
        }
        public void SetSearchTables(List<string> tableList)
        {
            tables = tableList.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
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
            return base.ToString()+"\t"+indexnames+"\t"+queryAts+"\t"+wordsAllContains+"\t"+exactPhraseContain+"\t"+oneOfWordsAtLeastContain+"\t"+wordNotInclude ;
        }
    }
}
