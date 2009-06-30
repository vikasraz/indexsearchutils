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
        #region"属性"
        private string indexnames = "";
        public string IndexNames
        {
            get { return indexnames; }
            set { indexnames = value; }
        }
        #region "模糊搜索"
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
        #endregion
        #region "精确搜索"
        private List<FilterCondition> filterConList = new List<FilterCondition>();
        public List<FilterCondition> FilterList
        {
            get { return filterConList; }
            set { filterConList = value; }
        }
        private List<ExcludeCondition> excludeList = new List<ExcludeCondition>();
        public List<ExcludeCondition> ExcludeList
        {
            get { return excludeList; }
            set { excludeList = value; }
        }
        private List<RangeCondition> rangeConList = new List<RangeCondition>();
        public List<RangeCondition> RangeList
        {
            get { return rangeConList; }
            set { rangeConList = value; }
        }
        #endregion
        #endregion
        #region "方法"
        #region "模糊搜索"
        #endregion
        #region "精确搜索"
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
        public void SetExcludes(params ExcludeCondition[] excludes)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.Clear();
            excludeList.AddRange(excludes);
        }
        public void AddExclude(ExcludeCondition ec)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.Add(ec);
        }
        public void AddExcludes(params ExcludeCondition[] ecArray)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.AddRange(ecArray);
        }
        public void AddExcludes(List<ExcludeCondition> ecList)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.AddRange(ecList);
        }
        public void SetRanges(params RangeCondition[] ranges)
        {
            if (rangeConList == null)
                rangeConList = new List<RangeCondition>();
            rangeConList.Clear();
            rangeConList.AddRange(ranges);
        }
        public void AddRange(RangeCondition rc)
        {
            if (rangeConList == null)
                rangeConList = new List<RangeCondition>();
            rangeConList.Add(rc);
        }
        public void AddRanges(params RangeCondition[] rangeArray)
        {
            if (rangeConList == null)
                rangeConList = new List<RangeCondition>();
            rangeConList.AddRange(rangeArray);
        }
        public void AddRanges(List<RangeCondition> rcList)
        {
            if (rangeConList == null)
                rangeConList = new List<RangeCondition>();
            rangeConList.AddRange(rcList);
        }
        #endregion
        #endregion
        #region "继承方法"
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(indexnames);
            ret.Append("\t"+queryAts);
            ret.Append("\t" + wordsAllContains);
            ret.Append("\t" + exactPhraseContain);
            ret.Append("\t" + oneOfWordsAtLeastContain );
            ret.Append("\t" + wordNotInclude);
            ret.Append(":select "+queryAts);
            ret.Append(" from "+indexnames+" where ");
            foreach (FilterCondition fc in filterConList)
            {
                ret.Append(fc.ToString() + " and ");
            }
            if (filterConList.Count > 0)
                ret.Append(" and ");
            foreach (RangeCondition rc in rangeConList)
            {
                ret.Append(rc.ToString() + " and ");
            }
            if (rangeConList.Count > 0)
                ret.Remove(ret.Length - 5, 1);
            if (excludeList.Count > 0)
                ret.Append(" and ");
            foreach (ExcludeCondition ec in excludeList)
            {
                ret.Append(ec.ToString() + " and ");
            }
            if (excludeList.Count > 0)
                ret.Remove(ret.Length - 5, 1);
            return ret.ToString();
        }
        #endregion
    }
}
