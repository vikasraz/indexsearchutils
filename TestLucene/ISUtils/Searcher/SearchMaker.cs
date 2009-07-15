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
        public Message ExecuteSearch(ref NetworkStream ns,string path)
        {
            Message msg = new Message();
            QueryInfo info = null;
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch");
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Deserialize NetworkStream");
                info = (QueryInfo)formater.Deserialize(ns);
                SupportClass.FileUtil.WriteToLog(path, "Query Info:" + info.ToString());

            }
            catch (SerializationException se)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to deserialize. Reason: " + se.Message);
#if DEBUG
                Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + se.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
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
            SupportClass.FileUtil.WriteToLog(path, "Before QueryResutl Add Result.");
            result.AddResult(qrsiList, hitsList, searchd.MaxMatches);
            SupportClass.FileUtil.WriteToLog(path, "After QueryResutl Add Result.");
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Serialize Result");
                formater.Serialize(ns, result);
                SupportClass.FileUtil.WriteToLog(path, "Finish Serialize Result");
            }
            catch (SerializationException e)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to serialize. Reason: " + e.Message);
#if DEBUG
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + e.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
                return msg;
            }
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch Success");
            msg.Success = true;
            msg.Result = "ExecuteSearch Success.";
            ns.Close();
            return msg;
        }
        public Message ExecuteSearch(ref NetworkStream ns, string path,bool highlight)
        {
            Message msg = new Message();
            QueryInfo info = null;
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch");
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Deserialize NetworkStream");
                info = (QueryInfo)formater.Deserialize(ns);
                SupportClass.FileUtil.WriteToLog(path, "Query Info:" + info.ToString());

            }
            catch (SerializationException se)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to deserialize. Reason: " + se.Message);
#if DEBUG
                Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + se.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
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
            SupportClass.FileUtil.WriteToLog(path, "Hits " + hits.Length().ToString());
            Highlighter highlighter = new Highlighter(new QueryScorer(query));
            highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
            Analyzer analyzer = new StandardAnalyzer();
            FormatedResult fResult = new FormatedResult();
            SupportClass.FileUtil.WriteToLog(path, "Before FormatedResutl Add Element.");
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
            SupportClass.FileUtil.WriteToLog(path, "After FormatedResutl Add Result.");
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Serialize fResult");
                formater.Serialize(ns, fResult);
                SupportClass.FileUtil.WriteToLog(path, "Finish Serialize fResult");
            }
            catch (SerializationException e)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to serialize. Reason: " + e.Message);
#if DEBUG
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + e.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
                return msg;
            }
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch Success");
            msg.Success = true;
            msg.Result = "ExecuteSearch Success.";
            ns.Close();
            return msg;
        }
        public Message ExecuteSearchEx(ref NetworkStream ns, string path)
        {
            Message msg = new Message();
            QueryInfo info = null;
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch");
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Deserialize NetworkStream");
                info = (QueryInfo)formater.Deserialize(ns);
                SupportClass.FileUtil.WriteToLog(path, "Query Info:" + info.ToString());

            }
            catch (SerializationException se)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to deserialize. Reason: " + se.Message);
#if DEBUG
                Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + se.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
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
            SupportClass.FileUtil.WriteToLog(path, "Hits "+hits.Length().ToString());
            QueryResult result = new QueryResult();
            SupportClass.FileUtil.WriteToLog(path, "Before QueryResutl Add Result.");
            result.AddResult(hits, searchd.MaxMatches);
            SupportClass.FileUtil.WriteToLog(path, "After QueryResutl Add Result.");
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Serialize Result");
                formater.Serialize(ns, result);
                SupportClass.FileUtil.WriteToLog(path, "Finish Serialize Result");
            }
            catch (SerializationException e)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to serialize. Reason: " + e.Message);
#if DEBUG
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + e.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
                return msg;
            }
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch Success");
            msg.Success = true;
            msg.Result = "ExecuteSearch Success.";
            ns.Close();
            return msg;
        }
        public Message ExecuteFastSearch(ref NetworkStream ns, string path)
        {
            Message msg = new Message();
            QueryInfo info = null;
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch");
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Deserialize NetworkStream");
                info = (QueryInfo)formater.Deserialize(ns);
                SupportClass.FileUtil.WriteToLog(path, "Query Info:" + info.ToString());

            }
            catch (SerializationException se)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to deserialize. Reason: " + se.Message);
#if DEBUG
                Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + se.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
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
            SupportClass.FileUtil.WriteToLog(path, "Before QueryResutl Add Result.");
            SearchResult result = new SearchResult(docList);
            //result.AddResult(qrsiList, hitsList, searchd.MaxMatches);
            SupportClass.FileUtil.WriteToLog(path, "After QueryResutl Add Result.");
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Serialize Result");
                formater.Serialize(ns, result);
                SupportClass.FileUtil.WriteToLog(path, "Finish Serialize Result");
            }
            catch (SerializationException e)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to serialize. Reason: " + e.Message);
#if DEBUG
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + e.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
                return msg;
            }
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch Success");
            msg.Success = true;
            msg.Result = "ExecuteSearch Success.";
            ns.Close();
            return msg;
        }
        public Message ExecuteFastSearch(ref NetworkStream ns, string path, bool highlight)
        {
            Message msg = new Message();
            QueryInfo info = null;
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch");
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Deserialize NetworkStream");
                info = (QueryInfo)formater.Deserialize(ns);
                SupportClass.FileUtil.WriteToLog(path, "Finish to Deserialize NetworkStream");
                SupportClass.FileUtil.WriteToLog(path, "Query Info:" + info.ToString());

            }
            catch (SerializationException se)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to deserialize. Reason: " + se.Message);
#if DEBUG
                Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + se.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
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
                SupportClass.FileUtil.WriteToLog(path, query.ToString());
            }
            else
            {
                msg.Success = false;
                msg.Result = "ExecuteSearch Failed.";
                ns.Close();
                return msg;
            }
            SupportClass.FileUtil.WriteToLog(path, "Hits " + docList.Count.ToString());
            Highlighter highlighter = new Highlighter(new QueryScorer(query));
            highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
            Analyzer analyzer = new StandardAnalyzer();
            FormatedResult fResult = new FormatedResult();
            SupportClass.FileUtil.WriteToLog(path, "Before FormatedResutl Add Element.");
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
            SupportClass.FileUtil.WriteToLog(path, "After FormatedResutl Add Result.");
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Serialize fResult");
                formater.Serialize(ns, fResult);
                SupportClass.FileUtil.WriteToLog(path, "Finish Serialize fResult");
            }
            catch (SerializationException e)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to serialize. Reason: " + e.Message);
#if DEBUG
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + e.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
                SupportClass.WriteLogAccess = false;
                return msg;
            }
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch Success");
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
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch");
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Deserialize NetworkStream");
                info = (QueryInfo)formater.Deserialize(ns);
                SupportClass.FileUtil.WriteToLog(path, "Query Info:" + info.ToString());

            }
            catch (SerializationException se)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to deserialize. Reason: " + se.Message);
#if DEBUG
                Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + se.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
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
            SupportClass.FileUtil.WriteToLog(path, "Hits " + docList.Count.ToString());
            SupportClass.FileUtil.WriteToLog(path, "Before QueryResutl Add Result.");
            SearchResult result = new SearchResult(docList);
            SupportClass.FileUtil.WriteToLog(path, "After QueryResutl Add Result.");
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Serialize Result");
                formater.Serialize(ns, result);
                SupportClass.FileUtil.WriteToLog(path, "Finish Serialize Result");
            }
            catch (SerializationException e)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to serialize. Reason: " + e.Message);
#if DEBUG
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + e.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
                return msg;
            }
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch Success");
            msg.Success = true;
            msg.Result = "ExecuteSearch Success.";
            ns.Close();
            return msg;
        }
        public Message ExecuteFastFieldSearch(ref NetworkStream ns, string path)
        {
            Message msg = new Message();
            QueryInfo info = null;
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch");
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Deserialize NetworkStream");
                info = (QueryInfo)formater.Deserialize(ns);
                SupportClass.FileUtil.WriteToLog(path, "Query Info:" + info.ToString());

            }
            catch (SerializationException se)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to deserialize. Reason: " + se.Message);
#if DEBUG
                Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + se.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
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
            SupportClass.FileUtil.WriteToLog(path, "Before QueryResutl Add Result.");
            SearchResult result = new SearchResult(docList);
            //result.AddResult(qrsiList, hitsList, searchd.MaxMatches);
            SupportClass.FileUtil.WriteToLog(path, "After QueryResutl Add Result.");
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Serialize Result");
                formater.Serialize(ns, result);
                SupportClass.FileUtil.WriteToLog(path, "Finish Serialize Result");
            }
            catch (SerializationException e)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to serialize. Reason: " + e.Message);
#if DEBUG
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + e.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
                return msg;
            }
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch Success");
            msg.Success = true;
            msg.Result = "ExecuteSearch Success.";
            ns.Close();
            return msg;
        }
        public Message ExecuteFastFieldSearch(ref NetworkStream ns, string path, bool highlight)
        {
            Message msg = new Message();
            QueryInfo info = null;
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch");
            BinaryFormatter formater = new BinaryFormatter();
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Deserialize NetworkStream");
                info = (QueryInfo)formater.Deserialize(ns);
                SupportClass.FileUtil.WriteToLog(path, "Query Info:" + info.ToString());

            }
            catch (SerializationException se)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to deserialize. Reason: " + se.Message);
#if DEBUG
                Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + se.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
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
            SupportClass.FileUtil.WriteToLog(path, "Hits " + docList.Count.ToString());
            Highlighter highlighter = new Highlighter(new QueryScorer(query));
            highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
            Analyzer analyzer = new StandardAnalyzer();
            FormatedResult fResult = new FormatedResult();
            SupportClass.FileUtil.WriteToLog(path, "Before FormatedResutl Add Element.");
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
            SupportClass.FileUtil.WriteToLog(path, "After FormatedResutl Add Result.");
            try
            {
                SupportClass.FileUtil.WriteToLog(path, "Try to Serialize fResult");
                formater.Serialize(ns, fResult);
                SupportClass.FileUtil.WriteToLog(path, "Finish Serialize fResult");
            }
            catch (SerializationException e)
            {
                SupportClass.FileUtil.WriteToLog(path, "Failed to serialize. Reason: " + e.Message);
#if DEBUG
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
#endif
                msg.Success = false;
                msg.ExceptionOccur = true;
                msg.Result = "Exception :" + e.Message;
                SupportClass.FileUtil.WriteToLog(path, msg.ToString());
                return msg;
            }
            SupportClass.FileUtil.WriteToLog(path, "In SearchMaker.ExecuteSearch Success");
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
            List<SearchRecord> recordList = Utils.SearchUtil.FastSearch(out query);
            return recordList;
        }
        public List<SearchRecord> ExecuteFastSearch(QueryInfo info, out Query query, bool highlight)
        {
            List<SearchRecord> recordList;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            if (highlight)
            {
                recordList = Utils.SearchUtil.HighLightSearch(out query);
            }
            else
            {
                recordList = Utils.SearchUtil.SearchEx(out query);
            }
            return recordList;
        }
        public List<SearchRecord> ExecuteFastSearch(QueryInfo info,  bool highlight)
        {
            List<SearchRecord> recordList;
            Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
            Utils.SearchUtil.SetQueryInfo(info);
            if (highlight)
            {
                recordList = Utils.SearchUtil.HighLightSearch();
            }
            else
            {
                recordList = Utils.SearchUtil.SearchEx();
            }
            return recordList;
        }
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
            Reverser<SearchRecord> reverser=new Reverser<SearchRecord>("ISUtils.Common.SearchRecord","Score",ReverserInfo.Direction.DESC);
            recordList.Sort(reverser);
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
        #endregion
    }
}
