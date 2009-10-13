using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using ISUtils.Common;
using ISUtils.Analysis.Chinese;
using ISUtils.Utils;
using ISUtils.Database.Writer;

namespace ISUtils.Indexer
{
    public class IndexMaker
    {
        #region Var
        private List<Source> sourceList;
        private FileIndexSet fileSet;
        private IndexerSet indexer;
        private DictionarySet dictSet;
        #endregion
        #region Contructor
        public IndexMaker(string filename)
        {
            try
            {
                Config parser = new Config(filename,true);
                sourceList = parser.GetSourceList();
                indexList = parser.GetIndexList();
                indexer = parser.GetIndexer();
                dictSet = parser.GetDictionarySet();
                fileSet = parser.FileIndexSet;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(string.Format("IndexMaker,发生异常，文件名:\t{0},\t异常:\t{1}", filename, ex.ToString()));
#endif
                throw;
            }
            Init();
        }
        #endregion
        #region Private Func
        private void Init()
        {
            ordinaryDict = new Dictionary<IndexSet,Source>();
            incremenDict = new Dictionary<IndexSet,Source>();
            foreach (IndexSet index in indexList)
            {
                foreach (Source source in sourceList)
                {
                    if (source.SourceName.ToUpper().CompareTo(index.SourceName.ToUpper()) == 0)
                    {
                        if (index.Type == IndexTypeEnum.Ordinary)
                        {
                            if(ordinaryDict.ContainsKey(index)==false)
                                ordinaryDict.Add(index, source);
                        }
                        else
                        {
                            if(incremenDict.ContainsKey(index)==false)
                                incremenDict.Add(index, source);
                        }
                        break;
                    }
                }
            }
        }
        #endregion
        #region Function
        public bool CanIndex(TimeSpan span, bool isIncreament)
        {
            if (type == IndexTypeEnum.Ordinary)
            {
                if (SupportClass.Time.IsTimeSame(DateTime.Now, indexer.MainIndexReCreateTime) &&
                    SupportClass.Time.GetDays(span) % indexer.MainIndexReCreateTimeSpan == 0)
                {
                    return true;
                }
            }
            else
            {
                if (SupportClass.Time.GetSeconds(span) % indexer.IncrIndexReCreateTimeSpan == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public Message ExecuteBoostIndexer(TimeSpan span, bool isIncreament)
        {
            Message msg = new Message();
            if (CanIndex(span, type) == false)
            {
                msg.Result = "ExecuteIndexer does not run.";
                msg.Success = false;
                return msg;
            }
            try
            {
                if (type == IndexTypeEnum.Ordinary)
                    BoostExecute(ordinaryDict, dictSet, indexer,  true, ref msg);
                else
                    BoostExecute(incremenDict, dictSet, indexer,  false, ref msg);
                msg.Result = "ExecuteIndexer Success.";
                msg.Success = true;
                return msg;
            }
            catch (Exception e)
            {
                Console.WriteLine("Execute Indexer Error.Reason:" + e.Message);

                msg.Result = "Exception:" + e.StackTrace.ToString();
                msg.Success = false;
                msg.ExceptionOccur = true;
                return msg;
            }
        }
        public static void BoostExecute(List<Source> sourceList, DictionarySet dictSet, IndexerSet indexer, bool create, ref Message msg)
        {
            try
            {
                DateTime allStart = DateTime.Now;
                msg.AddInfo("All Start at :" + allStart.ToLocalTime());
                Utils.IndexUtil.SetIndexSettings(dict, dictSet, indexer);
                //由于中文分词结果随中文词库的变化而变化，为了使索引不需要根据中文词库的变化而变化，
                //故采用默认的Analyzer来进行分词，即StandardAnalyzer
                //Utils.IndexUtil.UseDefaultChineseAnalyzer(true);
                Utils.IndexUtil.BoostIndex( create);
                msg.AddInfo("All End at :" + DateTime.Now.ToLocalTime());
                TimeSpan allSpan = DateTime.Now - allStart;
                msg.AddInfo(string.Format("All Spend {0} millionseconds.", allSpan.TotalMilliseconds));
                msg.Success = true;
                msg.Result = "Execute Success.";
            }
            catch (Exception e)
            {
                Console.WriteLine("Execute Indexer Error.Reason:" + e.Message);
                msg.AddInfo("Write Index Error.Reason:" + e.StackTrace.ToString());
                msg.Success = false;
                msg.ExceptionOccur = true;
                throw e;
            }
        }
        public static void Execute(List<Source> sourceList, DictionarySet dictSet, IndexerSet indexer, bool create, ref Message msg)
        {
            try
            {
                DateTime allStart = DateTime.Now;
                msg.AddInfo("All Start at :" + allStart.ToLocalTime());
                Utils.IndexUtil.SetIndexSettings(dict, dictSet, indexer);
                //由于中文分词结果随中文词库的变化而变化，为了使索引不需要根据中文词库的变化而变化，
                //故采用默认的Analyzer来进行分词，即StandardAnalyzer
                //Utils.IndexUtil.UseDefaultChineseAnalyzer(true);
                Utils.IndexUtil.Index(create);
                msg.AddInfo("All End at :"+DateTime.Now.ToLocalTime());
                TimeSpan allSpan=DateTime.Now -allStart;
                msg.AddInfo(string.Format("All Spend {0} millionseconds.",allSpan.TotalMilliseconds));
                msg.Success =true;
                msg.Result="Execute Success.";
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine("Execute Indexer Error.Reason:"+e.Message);
#endif
                msg.AddInfo("Write Index Error.Reason:"+e.StackTrace.ToString());
                msg.Success=false;
                msg.ExceptionOccur = true;
                throw e;
            }
        }
        public bool IndexFile(bool create)
        {
            if(create)
                Utils.IndexUtil.SetIndexSettings(ordinaryDict,fileSet, dictSet, indexer);
            else
                Utils.IndexUtil.SetIndexSettings(incremenDict, fileSet, dictSet, indexer);
            return ISUtils.Utils.IndexUtil.IndexFile(true);
        }
        #endregion
    }
}
