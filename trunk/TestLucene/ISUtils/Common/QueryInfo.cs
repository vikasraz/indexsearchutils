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
        private bool isFuzzySearch=true;
        public bool IsFuzzySearch
        {
            get { return isFuzzySearch; }
            set { isFuzzySearch = value; }
        }
        private string[] fields;
        public string[] SearchFields
        {
            get { return fields; }
            set { fields = value; }
        }
        public void SetSearchFields(params string[] fieldArray)
        {
            fields = fieldArray;
        }
        public void SetSearchFileds(List<string> fieldList)
        {
            fields = fieldList.ToArray();
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
