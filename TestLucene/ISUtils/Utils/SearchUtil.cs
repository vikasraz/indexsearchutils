using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Index;
using ISUtils.Common;

namespace ISUtils.Utils
{
    public static class SearchUtil
    {
        private static Dictionary<IndexSet, Source> indexDict = new Dictionary<IndexSet, Source>();
        private static Dictionary<IndexSet, List<string>> indexFieldsDict = new Dictionary<IndexSet, List<string>>();
        private static SearchSet searchSet = new  SearchSet();
        private static DictionarySet dictSet = new DictionarySet();
        private static Analyzer analyzer = new StandardAnalyzer();
        private static string wordsAllContains="";
        private static string exactPhraseContain="";
        private static string oneOfWordsAtLeastContain="";
        private static string wordNotInclude="";
        private static List<string> queryAtList = new List<string>();
        private static List<IndexSet> searchIndexList = new List<IndexSet>();
        private static bool initSettings=false;
        public static void SetSearchSettings(string configFileName)
        {
            if (initSettings) return;
            try
            {
                List<string> srcList = SupportClass.File.GetFileText(configFileName);
                List<Source> sourceList = Source.GetSourceList(srcList);
                List<IndexSet> indexList = IndexSet.GetIndexList(srcList);
                searchSet = SearchSet.GetSearchSet(srcList);
                dictSet = DictionarySet.GetDictionarySet(srcList);
                ISUtils.CSegment.Segment.SetPaths(dictSet.BasePath, dictSet.NamePath, dictSet.NumberPath, dictSet.FilterPath, dictSet.CustomPaths);
                ISUtils.CSegment.Segment.SetDefaults(new ISUtils.CSegment.DictionaryLoader.TextDictionaryLoader(), new ISUtils.CSegment.ForwardMatchSegment());
                if (indexDict == null)
                    indexDict = new Dictionary<IndexSet, Source>();
                foreach (IndexSet set in indexList)
                {
                    foreach (Source source in sourceList)
                    {
                        if (source.SourceName == set.SourceName)
                        {
                            indexDict.Add(set, source);
                            break;
                        }
                    }
                }
                initSettings = true;
            }
            catch (Exception e)
            {
#if DEBUG
                System.Console.WriteLine(e.StackTrace.ToString());
#endif
                throw e;
            }
        }
        public static void SetSearchSettings(List<Source> sourceList, List<IndexSet> indexList, DictionarySet dictSet, SearchSet searchSet)
        {
            if (initSettings) return;
            SearchUtil.searchSet = searchSet;
            SearchUtil.dictSet = dictSet;
            ISUtils.CSegment.Segment.SetPaths(dictSet.BasePath, dictSet.NamePath, dictSet.NumberPath, dictSet.FilterPath, dictSet.CustomPaths);
            ISUtils.CSegment.Segment.SetDefaults(new ISUtils.CSegment.DictionaryLoader.TextDictionaryLoader(), new ISUtils.CSegment.ForwardMatchSegment());
            if (indexDict == null)
                indexDict = new Dictionary<IndexSet, Source>();
            foreach (IndexSet set in indexList)
            {
                foreach (Source source in sourceList)
                {
                    if (source.SourceName == set.SourceName)
                    {
                        indexDict.Add(set, source);
#if DEBUG
                        System.Console.WriteLine("SearchUtil.indexDict.Add:");
                        System.Console.WriteLine("\t"+set.ToString());
                        System.Console.WriteLine("\t"+source.ToString());
#endif
                        break;
                    }
                }
            }
            initSettings = true;
        }
        public static void SetSearchSettings(Dictionary<IndexSet, Source> dict, DictionarySet dictSet, SearchSet searchSet)
        {
            if (initSettings) return;
            if (dict != null)
                indexDict = dict;
            else
                indexDict = new Dictionary<IndexSet, Source>();
            SearchUtil.dictSet = dictSet;
            SearchUtil.searchSet = searchSet;
            ISUtils.CSegment.Segment.SetPaths(dictSet.BasePath, dictSet.NamePath, dictSet.NumberPath, dictSet.FilterPath, dictSet.CustomPaths);
            ISUtils.CSegment.Segment.SetDefaults(new ISUtils.CSegment.DictionaryLoader.TextDictionaryLoader(), new ISUtils.CSegment.ForwardMatchSegment());
            initSettings = true;
        }
        public static void UseDefaultChineseAnalyzer(bool useChineseAnalyzer)
        {
            if (useChineseAnalyzer)
            {
                analyzer = new ISUtils.Analysis.Chinese.ChineseAnalyzer();
            }
            else
                analyzer = new StandardAnalyzer();
        }
        /**/
        /// <summary>
        /// 设置搜索索引
        /// </summary>
        /// <param name="indexNames">索引名称列表</param>
        public static void SetSearchIndexes(params string[] indexNames)
        {
            if (searchIndexList == null)
                searchIndexList = new  List<IndexSet>();
            foreach (string indexName in indexNames)
            {
                string [] names=SupportClass.String.Split(indexName);
                foreach (string name in names)
                {
                    foreach (IndexSet indexSet in indexDict.Keys)
                    {
                        if (name == indexSet.IndexName)
                        {
                            searchIndexList.Add(indexSet);
                            break;
                        }
                    }
                }
            }
        }
        /**/
        /// <summary>
        /// 设置搜索索引
        /// </summary>
        /// <param name="indexNames">索引名称列表</param>
        public static void SetSearchIndexes(string indexNames)
        {
            if (searchIndexList == null)
                searchIndexList = new  List<IndexSet>();
            searchIndexList.Clear();
            string[] names = SupportClass.String.Split(indexNames);
            foreach (string name in names)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (name == indexSet.IndexName)
                    {
                        searchIndexList.Add(indexSet);
                        break;
                    }
                }
            }
        }
        /**/
        /// <summary>
        /// 设置搜索词
        /// </summary>
        /// <param name="wordsAllContains">包含全部字词</param>
        /// <param name="exactPhraseContain">包含完整字句</param>
        /// <param name="oneOfWordsAtLeastContain">包含至少一个字词</param>
        /// <param name="wordNotInclude">不包括字词</param>
        public static void SetSearchWords(string wordsAllContains, string exactPhraseContain, string oneOfWordsAtLeastContain, string wordNotInclude)
        {
            SearchUtil.wordsAllContains = wordsAllContains;
            SearchUtil.exactPhraseContain = exactPhraseContain;
            SearchUtil.oneOfWordsAtLeastContain = oneOfWordsAtLeastContain;
            SearchUtil.wordNotInclude = wordNotInclude;
        }
        /**/
        /// <summary>
        /// 设置搜索词
        /// </summary>
        /// <param name="words">包含至少一个字词</param>
        public static void SetSearchWords(string words)
        {
            SearchUtil.wordsAllContains = words;
        }
        private static void SetIndexFieldsList()
        {
            if (indexDict == null)
                return;
            if (indexDict.Count <= 0)
                return;
            if (queryAtList == null)
                return;
            if (queryAtList.Count <= 0)
                return;
            if (indexFieldsDict == null)
                indexFieldsDict = new Dictionary<IndexSet, List<string>>();
            indexFieldsDict.Clear();
            foreach (IndexSet indexSet in indexDict.Keys)
            {
                List<string> fieldList = new List<string>();
                foreach (string field in indexDict[indexSet].Fields)
                {
                    if (queryAtList.Contains(field))
                    {
                        fieldList.Add(field);
                    }
                }
                indexFieldsDict.Add(indexSet, fieldList);
            }
        }
        /**/
        /// <summary>
        /// 设置搜索限制
        /// </summary>
        /// <param name="queryAt">查询字词位于</param>
        public static void SetSearchLimit(string queryAt)
        {
            if (queryAtList == null)
                queryAtList = new List<string>();
            queryAtList.Clear();
            string[] fields=SupportClass.String.Split(queryAt);
            foreach (string field in fields)
            {
                if (!queryAtList.Contains(field))
                {
                    queryAtList.Add(field);
                }
            }
            SetIndexFieldsList();
        }
        /**/
        /// <summary>
        /// 设置搜索限制
        /// </summary>
        /// <param name="queryAts">查询字词位于</param>
        public static void SetSearchLimit(params string[] queryAts)
        {
            if (queryAtList == null)
                queryAtList = new List<string>();
            queryAtList.Clear();
            foreach (string queryAt in queryAts)
            {
                string[] fields = SupportClass.String.Split(queryAt);
                foreach (string field in fields)
                {
                    if (!queryAtList.Contains(field))
                    {
                        queryAtList.Add(field);
                    }
                }
            }
            SetIndexFieldsList();
        }
        public static void SetQueryInfo(QueryInfo info)
        {
            if (info.IsFuzzySearch ==false)
                throw new ArgumentException("info has a wrong type.","info");
            SetSearchWords(info.FQuery.WordsAllContains, info.FQuery.ExactPhraseContain, info.FQuery.OneOfWordsAtLeastContain, info.FQuery.WordNotInclude);
            SetSearchIndexes(info.FQuery.IndexNames);
            SetSearchLimit(info.FQuery.QueryAts);
        }
        private static Query GetQuery(IndexSet indexSet)
        {
            string[] fields;
            if (indexFieldsDict.Count > 0 && indexFieldsDict.ContainsKey(indexSet))
                fields = indexFieldsDict[indexSet].ToArray();
            else
                fields = indexDict[indexSet].Fields;
            string[] wordAllContainArray = SupportClass.String.Split(wordsAllContains);
            string[] exactPhraseArray = SupportClass.String.Split(exactPhraseContain);
            string[] oneWordContainArray = SupportClass.String.Split(oneOfWordsAtLeastContain);
            string[] wordNoIncludeArray = SupportClass.String.Split(wordNotInclude);

            MultiFieldQueryParser parser = new MultiFieldQueryParser(fields, analyzer);
            BooleanQuery queryRet = new BooleanQuery();
            foreach (string words in wordAllContainArray)
            {
                List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                foreach (string word in wordList)
                {
                    Query query = parser.Parse(word);
                    queryRet.Add(query, BooleanClause.Occur.MUST);
                }
            }
            foreach (string words in exactPhraseArray)
            {
                List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                foreach(string word in wordList)
                {
                    Query query = parser.Parse(word);
                    queryRet.Add(query, BooleanClause.Occur.MUST);
                }
            }
            foreach (string words in exactPhraseArray)
            {
                List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                foreach(string word in wordList)
                {
                    Query query = parser.Parse(word);
                    queryRet.Add(query, BooleanClause.Occur.SHOULD);
                }
            }
            foreach (string words in wordNoIncludeArray)
            {
                List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                foreach(string word in wordList)
                {
                    Query query = parser.Parse(word);
                    queryRet.Add(query, BooleanClause.Occur.MUST_NOT);
                }
            }
            return queryRet;
        }
        private static Query GetQuery(IndexSet indexSet, out QueryResult.SearchInfo info)
        {
            string[] fields;
            info = new QueryResult.SearchInfo();
            info.IndexName = indexSet.IndexName;
            if (indexFieldsDict.Count > 0 && indexFieldsDict.ContainsKey(indexSet))
                fields = indexFieldsDict[indexSet].ToArray();
            else
                fields = indexDict[indexSet].Fields;
            info.Fields = fields;
            string[] wordAllContainArray = SupportClass.String.Split(wordsAllContains);
            string[] exactPhraseArray = SupportClass.String.Split(exactPhraseContain);
            string[] oneWordContainArray = SupportClass.String.Split(oneOfWordsAtLeastContain);
            string[] wordNoIncludeArray = SupportClass.String.Split(wordNotInclude);

            MultiFieldQueryParser parser = new MultiFieldQueryParser(fields, analyzer);
            BooleanQuery queryRet = new BooleanQuery();
            foreach (string words in wordAllContainArray)
            {
                List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                foreach(string word in wordList)
                {
                    Query query = parser.Parse(word);
                    queryRet.Add(query, BooleanClause.Occur.MUST);
                }
            }
            foreach (string words in exactPhraseArray)
            {
                List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                foreach (string word in wordList)
                {
                    Query query = parser.Parse(word);
                    queryRet.Add(query, BooleanClause.Occur.MUST);
                }
            }
            foreach (string words in exactPhraseArray)
            {
                List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                foreach(string word in wordList)
                {
                    Query query = parser.Parse(word);
                    queryRet.Add(query, BooleanClause.Occur.SHOULD);
                }
            }
            foreach (string words in wordNoIncludeArray)
            {
                List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                foreach(string word in wordList)
                {
                    Query query = parser.Parse(word);
                    queryRet.Add(query, BooleanClause.Occur.MUST_NOT);
                }
            }
            return queryRet;
        }
        private static Query GetQuery()
        {
            BooleanQuery queryRet = new BooleanQuery();
            if (searchIndexList.Count > 0)
            {
                foreach (IndexSet indexSet in searchIndexList)
                {
                    queryRet.Add(GetQuery(indexSet), BooleanClause.Occur.SHOULD);
                }
            }
            else
            {
                foreach (IndexSet indexSet in indexFieldsDict.Keys)
                {
                    queryRet.Add(GetQuery(indexSet), BooleanClause.Occur.SHOULD);
                }
            }
            return queryRet;
        }
        //private static Query GetQuery(out QueryResult.SearchInfo info)
        //{
        //    BooleanQuery queryRet = new BooleanQuery();
        //    info = new QueryResult.SearchInfo();
        //    if (searchIndexList.Count > 0)
        //    {
        //        foreach (IndexSet indexSet in searchIndexList)
        //        {
        //            queryRet.Add(GetQuery(indexSet), BooleanClause.Occur.MUST);
                    
        //        }
        //    }
        //    else
        //    {
        //        foreach (IndexSet indexSet in indexFieldsDict.Keys)
        //        {
        //            queryRet.Add(GetQuery(indexSet), BooleanClause.Occur.MUST);
        //        }
        //    }
        //    return queryRet;
        //}
        public static List<Hits> Search()
        {
            List<Hits> hitsList = new List<Hits>();
            if (searchIndexList.Count > 0)
            {
                foreach (IndexSet indexSet in searchIndexList)
                {
                    IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                    Query query = GetQuery(indexSet);
#if DEBUG
                    System.Console.WriteLine(query.ToString());
#endif
                    Hits hits = searcher.Search(query);
                    hitsList.Add(hits);
                }
            }
            else
            {
                foreach (IndexSet indexSet in indexFieldsDict.Keys)
                {
                    IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                    Query query = GetQuery(indexSet);
#if DEBUG
                    System.Console.WriteLine(query.ToString());
#endif
                    Hits hits = searcher.Search(query);
                    hitsList.Add(hits);
                }
            }
            return hitsList;
        }
        public static Hits SearchEx()
        {
            Hits hits = null;
            List<IndexReader> readerList = new List<IndexReader>();
            if (searchIndexList.Count > 0)
            {
                foreach (IndexSet indexSet in searchIndexList)
                {
                    readerList.Add(IndexReader.Open(indexSet.Path));
                }
            }
            else
            {
                foreach (IndexSet indexSet in indexFieldsDict.Keys)
                {
                    readerList.Add(IndexReader.Open(indexSet.Path));
                }
            }
            MultiReader multiReader = new MultiReader(readerList.ToArray());
            IndexSearcher searcher = new IndexSearcher(multiReader);
            Query query = GetQuery();
#if DEBUG
            System.Console.WriteLine(query.ToString());
#endif
            SupportClass.File.WriteToLog(SupportClass.LogPath, query.ToString());
            hits = searcher.Search(query);
            return hits;
        }
        public static Hits SearchEx(out Query query)
        {
            Hits hits = null;
            query = null;
            List<IndexReader> readerList = new List<IndexReader>();
            if (searchIndexList.Count > 0)
            {
                foreach (IndexSet indexSet in searchIndexList)
                {
                    readerList.Add(IndexReader.Open(indexSet.Path));
                }
            }
            else
            {
                foreach (IndexSet indexSet in indexFieldsDict.Keys)
                {
                    readerList.Add(IndexReader.Open(indexSet.Path));
                }
            }
            MultiReader multiReader = new MultiReader(readerList.ToArray());
            IndexSearcher searcher = new IndexSearcher(multiReader);
            query = GetQuery();
#if DEBUG
            System.Console.WriteLine(query.ToString());
#endif
            SupportClass.File.WriteToLog(SupportClass.LogPath, query.ToString());
            hits = searcher.Search(query);
            return hits;
        }
        public static List<Hits> Search(out List<QueryResult.SearchInfo> siList)
        {
            List<Hits> hitsList = new List<Hits>();
            siList = new List<QueryResult.SearchInfo>();
            if (searchIndexList.Count > 0)
            {
                foreach (IndexSet indexSet in searchIndexList)
                {
                    IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                    QueryResult.SearchInfo si;
                    Query query = GetQuery(indexSet, out si);
#if DEBUG
                    System.Console.WriteLine(query.ToString());
#endif
                    SupportClass.File.WriteToLog(SupportClass.LogPath, query.ToString());
                    Hits hits = searcher.Search(query);
                    hitsList.Add(hits);
                    siList.Add(si);
                }
            }
            else
            {
                foreach (IndexSet indexSet in indexFieldsDict.Keys)
                {
                    IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                    QueryResult.SearchInfo si;
                    Query query = GetQuery(indexSet, out si);
#if DEBUG
                    System.Console.WriteLine(query.ToString());
#endif
                    SupportClass.File.WriteToLog(SupportClass.LogPath, query.ToString());
                    Hits hits = searcher.Search(query);
                    hitsList.Add(hits);
                    siList.Add(si);
                }
            }
            return hitsList;
        }
    }
}
