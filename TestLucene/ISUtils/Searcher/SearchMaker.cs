using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Highlight;
using ISUtils.Common;
using ISUtils.Analysis.Chinese;
using ISUtils.Utils;

namespace ISUtils.Searcher
{
    public class SearchMaker
    {
        #region 私有变量
        private List<Source> sourceList;
        private List<IndexSet> indexList;
        private FileIndexSet fileSet;
        private DictionarySet dictSet;
        private SearchSet searchd;
        #endregion
        #region 属性
        public int Port
        {
            get { return searchd.Port; }
        }
        public string Address
        {
            get { return searchd.Address; }
        }
        #endregion
        #region 构造函数
        public SearchMaker(string filename)
        {
            try
            {
                Config parser = new Config(filename,true);
                searchd = parser.GetSearchd();
                sourceList = parser.GetSourceList();
                indexList = parser.GetIndexList();
                dictSet = parser.GetDictionarySet();
                fileSet = parser.FileIndexSet;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(string.Format("Exception for open file {0},{1}", filename, ex.ToString()));
#endif
                throw;
            }
        }
        #endregion
        #region 其它方法
        public int GetNetworkPort()
        {
            return searchd.Port;
        }
        #endregion
        #region 搜索接口
        public List<SearchRecord> ExecuteFastSearch(QueryInfo info, out Query query, out Dictionary<string,List<int>> statistics, bool highlight)
        {
            List<SearchRecord> recordList;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList,fileSet, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            if (highlight)
            {
                recordList = Utils.SearchUtil.HighLightSearch(out query);
                if (string.IsNullOrEmpty(info.IndexNames))
                {
                    recordList.AddRange(Utils.SearchUtil.HighLightSearchFile());
                }
            }
            else
            {
                recordList = Utils.SearchUtil.SearchEx(out query);
                if (string.IsNullOrEmpty(info.IndexNames))
                {
                    recordList.AddRange(Utils.SearchUtil.SearchFile());
                }
            }
            Reverser<SearchRecord> reverser = new Reverser<SearchRecord>("ISUtils.Common.SearchRecord", "Score", ReverserInfo.Direction.DESC);
            recordList.Sort(reverser);
            SearchRecord.Direction = ReverserInfo.Direction.DESC;
            statistics = new Dictionary<string, List<int>>();
            for (int i=0; i<recordList.Count; i++)
            {
                if (statistics.ContainsKey(recordList[i].Caption))
                {
                    statistics[recordList[i].Caption].Add(i);
                }
                else
                {
                    List<int> posList=new List<int>();
                    posList.Add(i);
                    statistics.Add(recordList[i].Caption, posList);
                }

            }
            return recordList;
        }
        public List<SearchRecord> ExecuteFastSearch(QueryInfo info, out Dictionary<string,List<int>> statistics, bool highlight)
        {
            List<SearchRecord> recordList;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            if (highlight)
            {
                recordList = Utils.SearchUtil.HighLightSearch(out statistics);
            }
            else
            {
                recordList = Utils.SearchUtil.SearchEx(out statistics);
            }
            return recordList;
        }
        public List<SearchRecord> ExecutePageSearch(QueryInfo info, out Query query, out Dictionary<string, int> statistics,string filter,int pageSize,int pageNum, bool highlight)
        {
            List<SearchRecord> recordList;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, fileSet, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            List<string> filterList=null;
            bool fileInclude=false;
            if (string.IsNullOrEmpty(info.IndexNames))
            {
                fileInclude=true;
            }
            filterList=new List<string>();
            string[] filters=SupportClass.String.Split(filter,",;");
            filterList.AddRange(filters);
            recordList = Utils.SearchUtil.SearchPage(out query,out statistics,filterList,pageSize,pageNum,fileInclude,highlight);
            return recordList;
        }
        #endregion
    }
}
