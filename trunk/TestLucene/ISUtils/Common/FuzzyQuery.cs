using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
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
}
