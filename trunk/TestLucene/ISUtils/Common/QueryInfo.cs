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
