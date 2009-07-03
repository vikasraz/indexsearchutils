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
        private DictionarySet dictSet;
        private SearchSet searchd;
        #endregion
        #region 属性
        public int Port
        {
            get { return searchd.Port; }
        }
        #endregion
        #region 构造函数
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
        #endregion
        #region 其它方法
        public int GetNetworkPort()
        {
            return searchd.Port;
        }
        #endregion
        #region 搜索接口
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
            List<Hits> hitsList = Utils.SearchUtil.FuzzySearch(out qrsiList);
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
        public Message ExecuteSearch(ref NetworkStream ns, string path,bool highlight)
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
                return msg;
            }
            SupportClass.LogPath = path;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            //由于中文分词结果随中文词库的变化而变化，为了使索引不需要根据中文词库的变化而变化，
            //故采用默认的Analyzer来进行分词，即StandardAnalyzer,
            //而采用中文分词进行预处理后，再进行搜索操作
            //Utils.SearchUtil.UseDefaultChineseAnalyzer(true);
            //List<QueryResult.SearchInfo> qrsiList;
            Query query;
            Hits hits = Utils.SearchUtil.Search(out query);
            SupportClass.File.WriteToLog(path, "Hits " + hits.Length().ToString());
            Highlighter highlighter = new Highlighter(new QueryScorer(query));
            highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
            Analyzer analyzer = new StandardAnalyzer();
            FormatedResult fResult = new FormatedResult();
            SupportClass.File.WriteToLog(path, "Before FormatedResutl Add Element.");
            for (int i=0; i< hits.Length() && i <searchd.MaxMatches; i++)
            {
                //Response.Write(ed.doc.ToString() + "<br>");
                Document doc = hits.Doc(i);
                //List<Field> fields = new List<Field>();
                //fields.AddRange(doc.GetFields().CopyTo);
                Field[] fields = new Field[doc.GetFields().Count];
                doc.GetFields().CopyTo(fields, 0);
                FormatedResult.FormatedDoc fd = new FormatedResult.FormatedDoc();
                foreach (Field field in fields)
                {
                    string key = field.Name();
                    string value = field.StringValue();
                    TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                    string result="";
                    result = highlighter.GetBestFragment(tokenStream, value);
                    if (highlight)
                    {
                        if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                        {
                            fd.AddElement(key, result);
                        }
                        else
                        {
                            fd.AddElement(key, value);
                        }
                    }
                    else
                    {
                        fd.AddElement(key, value);
                    }
                }
                fResult.AddFormatedDoc(fd);
            }
            SupportClass.File.WriteToLog(path, "After FormatedResutl Add Result.");
            try
            {
                SupportClass.File.WriteToLog(path, "Try to Serialize fResult");
                formater.Serialize(ns, fResult);
                SupportClass.File.WriteToLog(path, "Finish Serialize fResult");
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
        public Message ExecuteSearchEx(ref NetworkStream ns, string path)
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
                return msg;
            }
            SupportClass.LogPath = path;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            //由于中文分词结果随中文词库的变化而变化，为了使索引不需要根据中文词库的变化而变化，
            //故采用默认的Analyzer来进行分词，即StandardAnalyzer,
            //而采用中文分词进行预处理后，再进行搜索操作
            //Utils.SearchUtil.UseDefaultChineseAnalyzer(true);
            //List<QueryResult.SearchInfo> qrsiList;
            Hits hits = Utils.SearchUtil.Search();
            SupportClass.File.WriteToLog(path, "Hits "+hits.Length().ToString());
            QueryResult result = new QueryResult();
            SupportClass.File.WriteToLog(path, "Before QueryResutl Add Result.");
            result.AddResult(hits, searchd.MaxMatches);
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
        public Message ExecuteFastSearch(ref NetworkStream ns, string path)
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
                return msg;
            }
            SupportClass.LogPath = path;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            //由于中文分词结果随中文词库的变化而变化，为了使索引不需要根据中文词库的变化而变化，
            //故采用默认的Analyzer来进行分词，即StandardAnalyzer,
            //而采用中文分词进行预处理后，再进行搜索操作
            //Utils.SearchUtil.UseDefaultChineseAnalyzer(true);
            List<QueryResult.SearchInfo> qrsiList;
            List<SearchRecord> docList = Utils.SearchUtil.FuzzyFastSearch(out qrsiList);
            SupportClass.File.WriteToLog(path, "Before QueryResutl Add Result.");
            SearchResult result = new SearchResult(docList);
            //result.AddResult(qrsiList, hitsList, searchd.MaxMatches);
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
        public Message ExecuteFastSearch(ref NetworkStream ns, string path, bool highlight)
        {
            Message msg = new Message();
            QueryInfo info = null;
            SupportClass.File.WriteToLog(path, "In SearchMaker.ExecuteSearch");
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                SupportClass.File.WriteToLog(path, "Try to Deserialize NetworkStream");
                info = (QueryInfo)formater.Deserialize(ns);
                SupportClass.File.WriteToLog(path, "Finish to Deserialize NetworkStream");
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
                return msg;
            }
            SupportClass.WriteLogAccess = true;
            SupportClass.LogPath = path;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            //由于中文分词结果随中文词库的变化而变化，为了使索引不需要根据中文词库的变化而变化，
            //故采用默认的Analyzer来进行分词，即StandardAnalyzer,
            //而采用中文分词进行预处理后，再进行搜索操作
            //Utils.SearchUtil.UseDefaultChineseAnalyzer(true);
            //List<QueryResult.SearchInfo> qrsiList;
            Query query;
            List<SearchRecord> docList = Utils.SearchUtil.FastSearch(out query);
            if (query != null)
            {
                SupportClass.File.WriteToLog(path, query.ToString());
            }
            else
            {
                msg.Success = false;
                msg.Result = "ExecuteSearch Failed.";
                ns.Close();
                return msg;
            }
            SupportClass.File.WriteToLog(path, "Hits " + docList.Count.ToString());
            Highlighter highlighter = new Highlighter(new QueryScorer(query));
            highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
            Analyzer analyzer = new StandardAnalyzer();
            FormatedResult fResult = new FormatedResult();
            SupportClass.File.WriteToLog(path, "Before FormatedResutl Add Element.");
            for (int i = 0; i < docList.Count && i < searchd.MaxMatches; i++)
            {
                //Response.Write(ed.doc.ToString() + "<br>");
                FormatedResult.FormatedDoc fd = new FormatedResult.FormatedDoc();
                foreach (SearchField field in docList[i].Fields)
                {
                    string key = field.Name;
                    string value = field.Value;
                    TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                    string result = "";
                    result = highlighter.GetBestFragment(tokenStream, value);
                    if (highlight)
                    {
                        if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                        {
                            fd.AddElement(key, result);
                        }
                        else
                        {
                            fd.AddElement(key, value);
                        }
                    }
                    else
                    {
                        fd.AddElement(key, value);
                    }
                }
                fResult.AddFormatedDoc(fd);
            }
            SupportClass.File.WriteToLog(path, "After FormatedResutl Add Result.");
            try
            {
                SupportClass.File.WriteToLog(path, "Try to Serialize fResult");
                formater.Serialize(ns, fResult);
                SupportClass.File.WriteToLog(path, "Finish Serialize fResult");
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
                SupportClass.WriteLogAccess = false;
                return msg;
            }
            SupportClass.File.WriteToLog(path, "In SearchMaker.ExecuteSearch Success");
            SupportClass.WriteLogAccess = false;
            msg.Success = true;
            msg.Result = "ExecuteSearch Success.";
            ns.Close();
            return msg;
        }
        public Message ExecuteFastSearchEx(ref NetworkStream ns, string path)
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
                return msg;
            }
            SupportClass.LogPath = path;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            //由于中文分词结果随中文词库的变化而变化，为了使索引不需要根据中文词库的变化而变化，
            //故采用默认的Analyzer来进行分词，即StandardAnalyzer,
            //而采用中文分词进行预处理后，再进行搜索操作
            //Utils.SearchUtil.UseDefaultChineseAnalyzer(true);
            //List<QueryResult.SearchInfo> qrsiList;
            List<SearchRecord> docList= Utils.SearchUtil.FastSearch();
            SupportClass.File.WriteToLog(path, "Hits " + docList.Count.ToString());
            SupportClass.File.WriteToLog(path, "Before QueryResutl Add Result.");
            SearchResult result = new SearchResult(docList);
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
        public Message ExecuteFastFieldSearch(ref NetworkStream ns, string path)
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
                return msg;
            }
            SupportClass.LogPath = path;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            //由于中文分词结果随中文词库的变化而变化，为了使索引不需要根据中文词库的变化而变化，
            //故采用默认的Analyzer来进行分词，即StandardAnalyzer,
            //而采用中文分词进行预处理后，再进行搜索操作
            //Utils.SearchUtil.UseDefaultChineseAnalyzer(true);
            List<QueryResult.SearchInfo> qrsiList;
            List<SearchRecord> docList = Utils.SearchUtil.FuzzyFastFieldSearch(out qrsiList);
            SupportClass.File.WriteToLog(path, "Before QueryResutl Add Result.");
            SearchResult result = new SearchResult(docList);
            //result.AddResult(qrsiList, hitsList, searchd.MaxMatches);
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
        public Message ExecuteFastFieldSearch(ref NetworkStream ns, string path, bool highlight)
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
                return msg;
            }
            SupportClass.LogPath = path;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            //由于中文分词结果随中文词库的变化而变化，为了使索引不需要根据中文词库的变化而变化，
            //故采用默认的Analyzer来进行分词，即StandardAnalyzer,
            //而采用中文分词进行预处理后，再进行搜索操作
            //Utils.SearchUtil.UseDefaultChineseAnalyzer(true);
            //List<QueryResult.SearchInfo> qrsiList;
            Query query;
            List<SearchRecord> docList = Utils.SearchUtil.FuzzyFastFieldSearch(out query);
            SupportClass.File.WriteToLog(path, "Hits " + docList.Count.ToString());
            Highlighter highlighter = new Highlighter(new QueryScorer(query));
            highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
            Analyzer analyzer = new StandardAnalyzer();
            FormatedResult fResult = new FormatedResult();
            SupportClass.File.WriteToLog(path, "Before FormatedResutl Add Element.");
            for (int i = 0; i < docList.Count && i < searchd.MaxMatches; i++)
            {
                FormatedResult.FormatedDoc fd = new FormatedResult.FormatedDoc();
                foreach (SearchField field in docList[i].Fields)
                {
                    string key = field.Name;
                    string value = field.Value;
                    TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                    string result = "";
                    result = highlighter.GetBestFragment(tokenStream, value);
                    if (highlight)
                    {
                        if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                        {
                            fd.AddElement(key, result);
                        }
                        else
                        {
                            fd.AddElement(key, value);
                        }
                    }
                    else
                    {
                        fd.AddElement(key, value);
                    }
                }
                fResult.AddFormatedDoc(fd);
            }
            SupportClass.File.WriteToLog(path, "After FormatedResutl Add Result.");
            try
            {
                SupportClass.File.WriteToLog(path, "Try to Serialize fResult");
                formater.Serialize(ns, fResult);
                SupportClass.File.WriteToLog(path, "Finish Serialize fResult");
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
            List<Hits> hitsList = Utils.SearchUtil.FuzzySearch(out qrsiList);
            QueryResult result = new QueryResult();
            result.AddResult(qrsiList, hitsList, searchd.MaxMatches);
            return result;
        }
        public List<SearchRecord> ExecuteFastSearch(QueryInfo info,out Query query)
        {
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            List<SearchRecord> recordList = Utils.SearchUtil.SearchEx(out query);
            return recordList;
        }
        public List<SearchRecord> ExecuteFastSearch(QueryInfo info, out Query query, bool highlight)
        {
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            List<SearchRecord> recordList = Utils.SearchUtil.SearchEx(out query);
            if (highlight)
            {
                Highlighter highlighter = new Highlighter(new QueryScorer(query));
                highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
                Analyzer analyzer = new StandardAnalyzer();
                for (int i = 0; i < recordList.Count; i++)
                {
                    for (int j = 0; j < recordList[i].Fields.Count; j++)
                    {
                        string key = recordList[i].Fields[j].Name;
                        string value = recordList[i].Fields[j].Value;
                        TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                        string result = "";
                        result = highlighter.GetBestFragment(tokenStream, value);
                        if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                        {
                            recordList[i].Fields[j].Value = result;
                        }
                    }
                }
            }
            return recordList;
        }
        #endregion
    }
}
