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
using ISUtils.Common;
using ISUtils.Analysis.Chinese;
using ISUtils.Utils;

namespace ISUtils.Searcher
{
    public class SearchMaker
    {
        private List<Source> sourceList;
        private List<IndexSet> indexList;
        private DictionarySet dictSet;
        private SearchSet searchd;
        public int Port
        {
            get { return searchd.Port; }
        }
        public SearchMaker(string filename)
        {
            try
            {
                Parser parser = new Parser(filename);
                searchd = parser.GetSearchd();
                sourceList = parser.GetSourceList();
                indexList = parser.GetIndexList();
                dictSet = parser.GetDictionarySet();
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(string.Format("Exception for open file {0},{1}", filename, ex.ToString()));
#endif
                throw;
            }
        }
        public int GetNetworkPort()
        {
            return searchd.Port;
        }
        public Message ExecuteSearch(ref NetworkStream ns,string path)
        {
            Message msg = new Message();
            QueryInfo info = null;
            SupportClass.File.WriteToLog(path, "In SearchMaker.ExecuteSearch");
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                SupportClass.File.WriteToLog(path, "Try to Deserialize NetworkStream");
                info = (QueryInfo)formater.Deserialize(ns);
                SupportClass.File.WriteToLog(path, "Query Info:" + info.ToString());

            }
            catch (SerializationException se)
            {
                SupportClass.File.WriteToLog(path, "Failed to deserialize. Reason: " + se.Message);
#if DEBUG
                Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + se.Message;
                SupportClass.File.WriteToLog(path, msg.ToString());
                return msg ;
            }
            SupportClass.LogPath = path;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            //由于中文分词结果随中文词库的变化而变化，为了使索引不需要根据中文词库的变化而变化，
            //故采用默认的Analyzer来进行分词，即StandardAnalyzer,
            //而采用中文分词进行预处理后，再进行搜索操作
            //Utils.SearchUtil.UseDefaultChineseAnalyzer(true);
            List<QueryResult.SearchInfo> qrsiList;
            List<Hits> hitsList = Utils.SearchUtil.Search(out qrsiList);
            QueryResult result = new QueryResult();
            SupportClass.File.WriteToLog(path, "Before QueryResutl Add Result.");
            result.AddResult(qrsiList, hitsList, searchd.MaxMatches);
            SupportClass.File.WriteToLog(path, "After QueryResutl Add Result.");
            try
            {
                SupportClass.File.WriteToLog(path, "Try to Serialize Result");
                formater.Serialize(ns, result);
                SupportClass.File.WriteToLog(path, "Finish Serialize Result");
            }
            catch (SerializationException e)
            {
                SupportClass.File.WriteToLog(path, "Failed to serialize. Reason: " + e.Message);
#if DEBUG
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + e.Message;
                SupportClass.File.WriteToLog(path, msg.ToString());
                return msg;
            }
            SupportClass.File.WriteToLog(path, "In SearchMaker.ExecuteSearch Success");
            msg.Success = true;
            msg.Result = "ExecuteSearch Success.";
            ns.Close();
            return msg;
        }
        public QueryResult ExecuteSearch(QueryInfo query)
        {
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(query);
            Utils.SearchUtil.UseDefaultChineseAnalyzer(true);
            List<QueryResult.SearchInfo> qrsiList;
            List<Hits> hitsList = Utils.SearchUtil.Search(out qrsiList);
            QueryResult result = new QueryResult();
            result.AddResult(qrsiList, hitsList, searchd.MaxMatches);
            return result;
        }
    }
}
