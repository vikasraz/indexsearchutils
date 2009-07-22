using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Highlight;
using ISUtils.Common;

namespace ISUtils.Utils
{
    public static class SearchUtil
    {
        #region "私有全局变量"
        private static Dictionary<IndexSet, Source> indexDict = new Dictionary<IndexSet, Source>();
        private static Dictionary<IndexSet, List<string>> indexFieldsDict = new Dictionary<IndexSet, List<string>>();
        private static FileIndexSet fileSet = new FileIndexSet();
        private static SearchSet searchSet = new  SearchSet();
        private static DictionarySet dictSet = new DictionarySet();
        private static Analyzer analyzer = new StandardAnalyzer();
        private static string wordsAllContains="";
        private static string exactPhraseContain="";
        private static string oneOfWordsAtLeastContain="";
        private static string wordNotInclude="";
        private static List<string> queryAtList = new List<string>();
        private static List<IndexSet> searchIndexList = new List<IndexSet>();
        private static List<FilterCondition> filterList = new List<FilterCondition>();
        private static List<ExcludeCondition> excludeList = new List<ExcludeCondition>();
        private static List<RangeCondition> rangeList = new List<RangeCondition>();
        private static bool initSettings=false;
        private static int maxMatches = 1000;
        #endregion
        #region "搜索基本设置"
        public static void SetSearchSettings(string configFileName,bool isXmlFile)
        {
            if (initSettings) return;
            try
            {
                List<Source> sourceList ;
                List<IndexSet> indexList;
                if (isXmlFile)
                {
                    Config config = (Config)SupportClass.FileUtil.GetObjectFromXmlFile(configFileName, typeof(Config));
                    sourceList = config.SourceList;
                    indexList = config.IndexList;
                    searchSet = config.SearchSet;
                    dictSet = config.DictionarySet;
                    fileSet = config.FileIndexSet;
                }
                else
                {
                    List<string> srcList = SupportClass.FileUtil.GetFileText(configFileName);
                    sourceList = Source.GetSourceList(srcList);
                    indexList = IndexSet.GetIndexList(srcList);
                    searchSet = SearchSet.GetSearchSet(srcList);
                    dictSet = DictionarySet.GetDictionarySet(srcList);
                }
                searchIndexList.AddRange(indexList);
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
                            if(indexDict.ContainsKey(set)==false)
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
            searchIndexList.AddRange(indexList);
            if (indexDict == null)
                indexDict = new Dictionary<IndexSet, Source>();
            foreach (IndexSet set in indexList)
            {
                foreach (Source source in sourceList)
                {
                    if (source.SourceName == set.SourceName)
                    {
                        if(indexDict.ContainsKey(set)==false)
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
        public static void SetSearchSettings(List<Source> sourceList, List<IndexSet> indexList,FileIndexSet fileIndexSet, DictionarySet dictSet, SearchSet searchSet)
        {
            if (initSettings) return;
            SearchUtil.searchSet = searchSet;
            SearchUtil.dictSet = dictSet;
            SearchUtil.fileSet = fileIndexSet;
            ISUtils.CSegment.Segment.SetPaths(dictSet.BasePath, dictSet.NamePath, dictSet.NumberPath, dictSet.FilterPath, dictSet.CustomPaths);
            ISUtils.CSegment.Segment.SetDefaults(new ISUtils.CSegment.DictionaryLoader.TextDictionaryLoader(), new ISUtils.CSegment.ForwardMatchSegment());
            searchIndexList.AddRange(indexList);
            if (indexDict == null)
                indexDict = new Dictionary<IndexSet, Source>();
            foreach (IndexSet set in indexList)
            {
                foreach (Source source in sourceList)
                {
                    if (source.SourceName == set.SourceName)
                    {
                        if (indexDict.ContainsKey(set) == false)
                            indexDict.Add(set, source);
#if DEBUG
                        System.Console.WriteLine("SearchUtil.indexDict.Add:");
                        System.Console.WriteLine("\t" + set.ToString());
                        System.Console.WriteLine("\t" + source.ToString());
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
            searchIndexList.AddRange(indexDict.Keys);
            SearchUtil.dictSet = dictSet;
            SearchUtil.searchSet = searchSet;
            ISUtils.CSegment.Segment.SetPaths(dictSet.BasePath, dictSet.NamePath, dictSet.NumberPath, dictSet.FilterPath, dictSet.CustomPaths);
            ISUtils.CSegment.Segment.SetDefaults(new ISUtils.CSegment.DictionaryLoader.TextDictionaryLoader(), new ISUtils.CSegment.ForwardMatchSegment());
            initSettings = true;
        }
        public static void SetSearchSettings(Dictionary<IndexSet, Source> dict, FileIndexSet fileIndexSet, DictionarySet dictSet, SearchSet searchSet)
        {
            if (initSettings) return;
            if (dict != null)
                indexDict = dict;
            else
                indexDict = new Dictionary<IndexSet, Source>();
            searchIndexList.AddRange(indexDict.Keys);
            SearchUtil.dictSet = dictSet;
            SearchUtil.searchSet = searchSet;
            SearchUtil.fileSet = fileIndexSet;
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
        #endregion
        #region "搜索扩展设置"
        /**/
        /// <summary>
        /// 设置搜索索引
        /// </summary>
        /// <param name="indexNames">索引名称列表</param>
        public static void SetSearchIndexes(params string[] indexNames)
        {
            if (searchIndexList == null)
                searchIndexList = new  List<IndexSet>();
            searchIndexList.Clear();
            if (indexNames.Length <=0)
            {
                searchIndexList.AddRange(indexDict.Keys);
                return;
            }
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
            if (string.IsNullOrEmpty(indexNames))
            {
                searchIndexList.AddRange(indexDict.Keys);
                return;
            }
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
            if (indexFieldsDict == null)
                indexFieldsDict = new Dictionary<IndexSet, List<string>>();
            indexFieldsDict.Clear();
            if (queryAtList.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    List<string> fieldList = new List<string>();
                    foreach (FieldProperties field in indexDict[indexSet].Fields)
                    {
                        if (queryAtList.Contains(field.Name))
                        {
                            fieldList.Add(field.Name);
                        }
                    }
                    if(indexFieldsDict.ContainsKey(indexSet)==false)
                        indexFieldsDict.Add(indexSet, fieldList);
                }
            }
            else
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    List<string> fieldList = new List<string>();
                    foreach (FieldProperties field in indexDict[indexSet].Fields)
                    {
                        fieldList.Add(field.Name);
                    }
                    if (indexFieldsDict.ContainsKey(indexSet) == false)
                        indexFieldsDict.Add(indexSet, fieldList);
                }
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
        public static void SetFilters(params FilterCondition[] filters)
        {
            if (filterList == null)
                filterList = new List<FilterCondition>();
            filterList.Clear();
            filterList.AddRange(filters);
        }
        public static void SetFilters(List<FilterCondition> filters)
        {
            if (filterList == null)
                filterList = new List<FilterCondition>();
            filterList.Clear();
            filterList.AddRange(filters);
        }
        public static void AddFilter(FilterCondition fc)
        {
            if (filterList == null)
                filterList = new List<FilterCondition>();
            filterList.Add(fc);
        }
        public static void AddFilters(params FilterCondition[] fcArray)
        {
            if (filterList == null)
                filterList = new List<FilterCondition>();
            filterList.AddRange(fcArray);
        }
        public static void AddFilters(List<FilterCondition> fcList)
        {
            if (filterList == null)
                filterList = new List<FilterCondition>();
            filterList.AddRange(fcList);
        }
        public static void SetExcludes(params ExcludeCondition[] excludes)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.Clear();
            excludeList.AddRange(excludes);
        }
        public static void SetExcludes(List<ExcludeCondition> excludes)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.Clear();
            excludeList.AddRange(excludes);
        }
        public static void AddExclude(ExcludeCondition ec)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.Add(ec);
        }
        public static void AddExcludes(params ExcludeCondition[] ecArray)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.AddRange(ecArray);
        }
        public static void AddExcludes(List<ExcludeCondition> ecList)
        {
            if (excludeList == null)
                excludeList = new List<ExcludeCondition>();
            excludeList.AddRange(ecList);
        }
        public static void SetRanges(List<RangeCondition> ranges)
        {
            if (rangeList == null)
                rangeList = new List<RangeCondition>();
            rangeList.Clear();
            rangeList.AddRange(ranges);
        }
        public static void SetRanges(params RangeCondition[] ranges)
        {
            if (rangeList == null)
                rangeList = new List<RangeCondition>();
            rangeList.Clear();
            rangeList.AddRange(ranges);
        }
        public static void AddRange(RangeCondition rc)
        {
            if (rangeList == null)
                rangeList = new List<RangeCondition>();
            rangeList.Add(rc);
        }
        public static void AddRanges(params RangeCondition[] rangeArray)
        {
            if (rangeList == null)
                rangeList = new List<RangeCondition>();
            rangeList.AddRange(rangeArray);
        }
        public static void AddRanges(List<RangeCondition> rcList)
        {
            if (rangeList == null)
                rangeList = new List<RangeCondition>();
            rangeList.AddRange(rcList);
        }       
        #endregion
        #region "搜索设置接口"
        public static void SetQueryInfo(QueryInfo info)
        {
            SetSearchIndexes(info.IndexNames);
            SetSearchWords(info.WordsAllContains, info.ExactPhraseContain, info.OneOfWordsAtLeastContain, info.WordNotInclude);
            SetSearchLimit(info.QueryAts);
            SetFilters(info.FilterList);
            SetExcludes(info.ExcludeList);
            SetRanges(info.RangeList);
        }
        #endregion
        #region "私有公共方法"
        private static Query GetFileQuery()
        {
            string[] fields = new string[] { "Name","Content"};
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
                foreach (string word in wordList)
                {
                    Query query = parser.Parse(word);
                    queryRet.Add(query, BooleanClause.Occur.MUST);
                }
            }
            foreach (string words in oneWordContainArray)
            {
                List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                foreach (string word in wordList)
                {
                    Query query = parser.Parse(word);
                    queryRet.Add(query, BooleanClause.Occur.SHOULD);
                }
            }
            foreach (string words in wordNoIncludeArray)
            {
                List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                foreach (string word in wordList)
                {
                    Query query = parser.Parse(word);
                    queryRet.Add(query, BooleanClause.Occur.MUST_NOT);
                }
            }
            return queryRet;
        }
        private static Query GetFuzzyQuery(IndexSet indexSet)
        {
            string[] fields;
            if (indexFieldsDict.Count > 0 && indexFieldsDict.ContainsKey(indexSet))
                fields = indexFieldsDict[indexSet].ToArray();
            else
                fields = indexDict[indexSet].StringFields.ToArray();
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
            foreach (string words in oneWordContainArray)
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
        private static Query GetFuzzyQuery(IndexSet indexSet, out QueryResult.SearchInfo info)
        {
            string[] fields;
            info = new QueryResult.SearchInfo();
            info.IndexName = indexSet.IndexName;
            if (indexFieldsDict.Count > 0 && indexFieldsDict.ContainsKey(indexSet))
                fields = indexFieldsDict[indexSet].ToArray();
            else
                fields = indexDict[indexSet].StringFields.ToArray();
            info.Fields.AddRange(fields);
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
            foreach (string words in oneWordContainArray)
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
        private static Query GetExactQuery()
        {
            BooleanQuery queryRet = new BooleanQuery();
            foreach (FilterCondition fc in filterList)
            {
                //SupportClass.FileUtil.WriteLog("fc :" + fc.ToString());
                QueryParser parser = new QueryParser(fc.GetString(), analyzer);
                foreach (string value in fc.Values)
                {
                    //SupportClass.FileUtil.WriteLog("fc loop,value of fc.values:" + value);
                    string[] wordArray = SupportClass.String.Split(value);
                    foreach (string words in wordArray)
                    {
                        //SupportClass.FileUtil.WriteLog("word loop,word of value split:" + words);
                        List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                        foreach (string word in wordList)
                        {
                            Query query = parser.Parse(word);
                            queryRet.Add(query, BooleanClause.Occur.MUST);
                            //SupportClass.FileUtil.WriteLog(queryRet.ToString());
                        }
                    }
                }
            }
            foreach (ExcludeCondition ec in excludeList)
            {
                //SupportClass.FileUtil.WriteLog("ec :" + ec.ToString());
                QueryParser parser = new QueryParser(ec.GetString(), analyzer);
                foreach (string value in ec.Values)
                {
                    //SupportClass.FileUtil.WriteLog("ec loop,value of ec.values:" + value);
                    string[] wordArray = SupportClass.String.Split(value);
                    foreach (string words in wordArray)
                    {
                        //SupportClass.FileUtil.WriteLog("word loop,word of value split:" + words);
                        List<string> wordList = ISUtils.CSegment.Segment.SegmentStringEx(words);
                        foreach (string word in wordList)
                        {
                            Query query = parser.Parse(word);
                            queryRet.Add(query, BooleanClause.Occur.MUST_NOT);
                            //SupportClass.FileUtil.WriteLog(queryRet.ToString());
                        }
                    }
                }
            }
            foreach (RangeCondition rc in rangeList)
            {
                SupportClass.FileUtil.WriteLog("rc:" + rc.ToString());
                RangeQuery query = new RangeQuery(new Term(rc.GetString(), rc.RangeFrom), new Term(rc.GetString(), rc.RangeTo), rc.IntervalType);
                queryRet.Add(query, BooleanClause.Occur.MUST);
                SupportClass.FileUtil.WriteLog(queryRet.ToString());
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
                    queryRet.Add(GetFuzzyQuery(indexSet), BooleanClause.Occur.SHOULD);
                }
            }
            else
            {
                foreach (IndexSet indexSet in indexFieldsDict.Keys)
                {
                    queryRet.Add(GetFuzzyQuery(indexSet), BooleanClause.Occur.SHOULD);
                }
            }
            queryRet.Add(GetExactQuery(), BooleanClause.Occur.SHOULD);
            return queryRet;
        }
        public static Query GetQuery(IndexSet indexSet)
        {
            BooleanQuery queryRet = new BooleanQuery();
            queryRet.Add(GetFuzzyQuery(indexSet), BooleanClause.Occur.MUST);
            queryRet.Add(GetExactQuery(), BooleanClause.Occur.MUST);
            return queryRet;
        }
        #endregion
        #region "模糊搜索接口"
        public static List<Hits> FuzzySearch()
        {
            List<Hits> hitsList = new List<Hits>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        Query query = GetFuzzyQuery(indexSet);
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
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        Query query = GetFuzzyQuery(indexSet);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        Hits hits = searcher.Search(query);
                        hitsList.Add(hits);
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return hitsList;
        }
        public static List<Hits> FuzzySearch(out List<QueryResult.SearchInfo> siList)
        {
            List<Hits> hitsList = new List<Hits>();
            siList = new List<QueryResult.SearchInfo>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        QueryResult.SearchInfo si;
                        Query query = GetFuzzyQuery(indexSet, out si);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                        Hits hits = searcher.Search(query);
                        hitsList.Add(hits);
                        siList.Add(si);
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        QueryResult.SearchInfo si;
                        Query query = GetFuzzyQuery(indexSet, out si);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                        Hits hits = searcher.Search(query);
                        hitsList.Add(hits);
                        siList.Add(si);
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return hitsList;
        }
        public static Hits FuzzySearchEx()
        {
            Hits hits = null;
            try
            {
                List<IndexReader> readerList = new List<IndexReader>();
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        readerList.Add(IndexReader.Open(indexSet.Path));
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        readerList.Add(IndexReader.Open(indexSet.Path));
                    }
                }
                MultiReader multiReader = new MultiReader(readerList.ToArray());
                IndexSearcher searcher = new IndexSearcher(multiReader);
                Query query = GetQuery();
#if DEBUG
                System.Console.WriteLine(query.ToString());
#endif
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                hits = searcher.Search(query);
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return hits;
        }
        public static Hits FuzzySearchEx(out Query query)
        {
            Hits hits = null;
            query = null;
            try
            {
                List<IndexReader> readerList = new List<IndexReader>();
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        readerList.Add(IndexReader.Open(indexSet.Path));
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        readerList.Add(IndexReader.Open(indexSet.Path));
                    }
                }
                MultiReader multiReader = new MultiReader(readerList.ToArray());
                IndexSearcher searcher = new IndexSearcher(multiReader);
                query = GetQuery();
#if DEBUG
                System.Console.WriteLine(query.ToString());
#endif
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                hits = searcher.Search(query);
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return hits;
        }
        public static List<SearchRecord> FuzzyFastSearch()
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            List<SearchField> sfList=new List<SearchField>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        Query query = GetFuzzyQuery(indexSet);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields=new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields,0);
                            sfList.Clear();
                            foreach(Field field in fields)
                            {
                                sfList.Add(new SearchField(field,indexDict[indexSet].FieldDict[field.Name()]));
                            }
                            recordList.Add(new SearchRecord(indexSet,sfList,indexDict[indexSet].PrimaryKey,score));
                        }
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        Query query = GetFuzzyQuery(indexSet);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            sfList.Clear();
                            foreach (Field field in fields)
                            {
                                sfList.Add(new SearchField(field, indexDict[indexSet].FieldDict[field.Name()]));
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        public static List<SearchRecord> FuzzyFastSearch(out List<QueryResult.SearchInfo> siList)
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            List<SearchField> sfList = new List<SearchField>();
            siList = new List<QueryResult.SearchInfo>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        QueryResult.SearchInfo si;
                        Query query = GetFuzzyQuery(indexSet, out si);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            sfList.Clear();
                            foreach (Field field in fields)
                            {
                                sfList.Add(new SearchField(field, indexDict[indexSet].FieldDict[field.Name()]));
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                        }
                        siList.Add(si);
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        QueryResult.SearchInfo si;
                        Query query = GetFuzzyQuery(indexSet, out si);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            sfList.Clear();
                            foreach (Field field in fields)
                            {
                                sfList.Add(new SearchField(field, indexDict[indexSet].FieldDict[field.Name()]));
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                        }
                        siList.Add(si);
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        public static List<SearchRecord> FuzzyFastSearchEx()
        {
            List<SearchRecord> docList = new List<SearchRecord>();
            try
            {
                List<IndexReader> readerList = new List<IndexReader>();
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        readerList.Add(IndexReader.Open(indexSet.Path));
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        readerList.Add(IndexReader.Open(indexSet.Path));
                    }
                }
                MultiReader multiReader = new MultiReader(readerList.ToArray());
                IndexSearcher searcher = new IndexSearcher(multiReader);
                Query query = GetQuery();
#if DEBUG
                System.Console.WriteLine(query.ToString());
#endif
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                for (int i = 0; i < scoreDocs.Length; i++)
                {
                    Document doc = searcher.Doc(scoreDocs[i].doc);
                    float score = scoreDocs[i].score;
                    if (score < searchSet.MinScore)
                        continue;
                    docList.Add(doc);
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return docList;
        }
        public static List<SearchRecord> FuzzyFastSearchEx(out Query query)
        {
            List<SearchRecord> docList = new List<SearchRecord>();
            query = null;
            try
            {
                List<IndexReader> readerList = new List<IndexReader>();
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        readerList.Add(IndexReader.Open(indexSet.Path));
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        readerList.Add(IndexReader.Open(indexSet.Path));
                    }
                }
                MultiReader multiReader = new MultiReader(readerList.ToArray());
                IndexSearcher searcher = new IndexSearcher(multiReader);
                query = GetQuery();
#if DEBUG
                System.Console.WriteLine(query.ToString());
#endif
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                for (int i = 0; i < scoreDocs.Length; i++)
                {
                    Document doc = searcher.Doc(scoreDocs[i].doc);
                    float score = scoreDocs[i].score;
                    if (score < searchSet.MinScore)
                        continue;
                    docList.Add(doc);
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return docList;
        }
        public static List<SearchRecord> FuzzyFastFieldSearch()
        {
            List<SearchRecord> docList = new List<SearchRecord>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        Query query = GetFuzzyQuery(indexSet);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        SpecialFieldSelector sfs = new SpecialFieldSelector(indexDict[indexSet].PrimaryKey);
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Document doc = searcher.Doc(scoreDocs[i].doc, sfs);
                            docList.Add(doc);
                        }
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        Query query = GetFuzzyQuery(indexSet);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        SpecialFieldSelector sfs = new SpecialFieldSelector(indexDict[indexSet].PrimaryKey);
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc,sfs);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            docList.Add(doc);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return docList;
        }
        public static List<SearchRecord> FuzzyFastFieldSearch(out List<QueryResult.SearchInfo> siList)
        {
            List<SearchRecord> docList = new List<SearchRecord>();
            siList = new List<QueryResult.SearchInfo>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        QueryResult.SearchInfo si;
                        Query query = GetFuzzyQuery(indexSet, out si);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        SpecialFieldSelector sfs = new SpecialFieldSelector(indexDict[indexSet].PrimaryKey);
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc,sfs);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            docList.Add(doc);
                        }
                        siList.Add(si);
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        QueryResult.SearchInfo si;
                        Query query = GetFuzzyQuery(indexSet, out si);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        SpecialFieldSelector sfs = new SpecialFieldSelector(indexDict[indexSet].PrimaryKey);
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc,sfs);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            docList.Add(doc);
                        }
                        siList.Add(si);
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return docList;
        }
        public static List<SearchRecord> FuzzyFastFieldSearch(out Query mquery)
        {
            List<SearchRecord> docList = new List<SearchRecord>();
            mquery = null;
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        Query query = GetFuzzyQuery(indexSet);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        SpecialFieldSelector sfs = new SpecialFieldSelector(indexDict[indexSet].PrimaryKey);
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc, sfs);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            docList.Add(doc);
                        }
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        Query query = GetFuzzyQuery(indexSet);
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        SpecialFieldSelector sfs = new SpecialFieldSelector(indexDict[indexSet].PrimaryKey);
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc, sfs);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            docList.Add(doc);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            mquery = GetQuery();
            return docList;
        }
        #endregion
        #region "精确搜索接口"
        public static Hits ExactSearch()
        {
            Hits hits = null;
            try
            {
                List<IndexReader> readerList = new List<IndexReader>();
                foreach (IndexSet indexSet in searchIndexList)
                {
                    if (indexSet.Type == IndexTypeEnum.Increment)
                        continue;
                    readerList.Add(IndexReader.Open(indexSet.Path));
                }
                MultiReader multiReader = new MultiReader(readerList.ToArray());
                IndexSearcher searcher = new IndexSearcher(multiReader);
                Query query = GetQuery();
#if DEBUG
                System.Console.WriteLine(query.ToString());
#endif
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                hits = searcher.Search(query);
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return hits;
        }
        public static Hits ExactSearch(out Query query)
        {
            Hits hits = null;
            query = null;
            try
            {
                List<IndexReader> readerList = new List<IndexReader>();
                foreach (IndexSet indexSet in searchIndexList)
                {
                    if (indexSet.Type == IndexTypeEnum.Increment)
                        continue;
                    readerList.Add(IndexReader.Open(indexSet.Path));
                }
                MultiReader multiReader = new MultiReader(readerList.ToArray());
                IndexSearcher searcher = new IndexSearcher(multiReader);
                query = GetQuery();
#if DEBUG
                System.Console.WriteLine(query.ToString());
#endif
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, query.ToString());
                hits = searcher.Search(query);
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return hits;
        }
        public static List<SearchRecord> ExactFastSearch()
        {
            List<SearchRecord> docList = new List<SearchRecord>();
            try
            {
                List<IndexReader> readerList = new List<IndexReader>();
                foreach (IndexSet indexSet in searchIndexList)
                {
                    if (indexSet.Type == IndexTypeEnum.Increment)
                        continue;
                    readerList.Add(IndexReader.Open(indexSet.Path));
                }
                MultiReader multiReader = new MultiReader(readerList.ToArray());
                IndexSearcher searcher = new IndexSearcher(multiReader);
                Query query = GetQuery();
#if DEBUG
                System.Console.WriteLine(query.ToString());
#endif
                TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                for (int i = 0; i < scoreDocs.Length; i++)
                {
                    Document doc = searcher.Doc(scoreDocs[i].doc);
                    float score = scoreDocs[i].score;
                    if (score < searchSet.MinScore)
                        continue;
                    docList.Add(doc);
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return docList;
        }
        public static List<SearchRecord> ExactFastSearch(out Query query)
        {
            List<SearchRecord> docList = new List<SearchRecord>();
            query = null;
            try
            {
                List<IndexReader> readerList = new List<IndexReader>();
                foreach (IndexSet indexSet in searchIndexList)
                {
                    if (indexSet.Type == IndexTypeEnum.Increment)
                        continue;
                    readerList.Add(IndexReader.Open(indexSet.Path));
                }
                MultiReader multiReader = new MultiReader(readerList.ToArray());
                IndexSearcher searcher = new IndexSearcher(multiReader);
                query = GetQuery();
#if DEBUG
                System.Console.WriteLine(query.ToString());
#endif
                TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                for (int i = 0; i < scoreDocs.Length; i++)
                {
                    Document doc = searcher.Doc(scoreDocs[i].doc);
                    float score = scoreDocs[i].score;
                    if (score < searchSet.MinScore)
                        continue;
                    docList.Add(doc);
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return docList;
        }
        #endregion
        #region "通用搜索接口"
        public static Hits Search()
        {
            return ExactSearch();
        }
        public static Hits Search(out Query query)
        {
            return ExactSearch(out query);
        }
        public static List<SearchRecord> FastSearch()
        {
            return ExactFastSearch();
        }
        public static List<SearchRecord> FastSearch(out Query query)
        {
            return ExactFastSearch(out query);
        }
        public static List<SearchRecord> SearchEx(out Query query)
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            query = GetQuery();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                if(fpDict.ContainsKey(field.Name()))
                                    sfList.Add(new SearchField(field, fpDict[field.Name()]));
                                else
                                    sfList.Add(new SearchField(field));
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                        }
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                if (fpDict.ContainsKey(field.Name()))
                                    sfList.Add(new SearchField(field, fpDict[field.Name()]));
                                else
                                    sfList.Add(new SearchField(field));
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        public static List<SearchRecord> SearchEx()
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query query = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                if (fpDict.ContainsKey(field.Name()))
                                    sfList.Add(new SearchField(field, fpDict[field.Name()]));
                                //else
                                //    sfList.Add(new SearchField(field,false));
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                        }
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query query = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                if (fpDict.ContainsKey(field.Name()))
                                    sfList.Add(new SearchField(field, fpDict[field.Name()]));
                                //else
                                //    sfList.Add(new SearchField(field, false));
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        public static List<SearchRecord> SearchEx(out Query query,out Dictionary<string,List<int>> statistics)
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            query = GetQuery();
            statistics = new Dictionary<string,List<int>>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query theQuery = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        TopDocs topDocs = searcher.Search(theQuery.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        List<int> posList = new List<int>();
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                if (fpDict.ContainsKey(field.Name()))
                                    sfList.Add(new SearchField(field, fpDict[field.Name()]));
                                else
                                    sfList.Add(new SearchField(field));
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                            posList.Add(recordList.Count - 1);
                        }
                        try
                        {
                            statistics.Add(indexSet.Caption, posList);
                        }
                        catch (Exception)
                        {
                            int i = 2;
                            while (statistics.ContainsKey(indexSet.Caption + i.ToString()))
                                i++;
                            statistics.Add(indexSet.Caption + i.ToString(), posList);
                        }
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query theQuery = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(theQuery.ToString());
#endif
                        TopDocs topDocs = searcher.Search(theQuery.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        List<int> posList = new List<int>();
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                if (fpDict.ContainsKey(field.Name()))
                                    sfList.Add(new SearchField(field, fpDict[field.Name()]));
                                else
                                    sfList.Add(new SearchField(field));
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                            posList.Add(recordList.Count - 1);
                        }
                        try
                        {
                            statistics.Add(indexSet.Caption, posList);
                        }
                        catch (Exception)
                        {
                            int i = 2;
                            while (statistics.ContainsKey(indexSet.Caption + i.ToString()))
                                i++;
                            statistics.Add(indexSet.Caption + i.ToString(), posList);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        public static List<SearchRecord> SearchEx(out Dictionary<string,List<int>> statistics)
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            statistics = new Dictionary<string,List<int>>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query query = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        List<int> posList=new List<int>();
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                if (fpDict.ContainsKey(field.Name()))
                                    sfList.Add(new SearchField(field, fpDict[field.Name()]));
                                //else
                                //    sfList.Add(new SearchField(field,false));
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                            posList.Add(recordList.Count - 1);
                        }
                        try
                        {
                            statistics.Add(indexSet.Caption, posList);
                        }
                        catch (Exception)
                        {
                            int i = 2;
                            while (statistics.ContainsKey(indexSet.Caption + i.ToString()))
                                i++;
                            statistics.Add(indexSet.Caption + i.ToString(), posList);
                        }
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query query = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        List<int> posList=new List<int>();
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                if (fpDict.ContainsKey(field.Name()))
                                    sfList.Add(new SearchField(field, fpDict[field.Name()]));
                                //else
                                //    sfList.Add(new SearchField(field, false));
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                            posList.Add(recordList.Count - 1);
                        }
                        try
                        {
                            statistics.Add(indexSet.Caption, posList);
                        }
                        catch (Exception)
                        {
                            int i = 2;
                            while (statistics.ContainsKey(indexSet.Caption + i.ToString()))
                                i++;
                            statistics.Add(indexSet.Caption + i.ToString(), posList);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        public static List<SearchRecord> HighLightSearch(out Query query)
        {
            List<SearchRecord> recordList = new List<SearchRecord>();            
            query = GetQuery();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        Highlighter highlighter = new Highlighter(new QueryScorer(query));
                        highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>(); 
                            foreach (Field field in fields)
                            {
                                string key = field.Name();
                                string value = field.StringValue();
                                TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                                string result = "";
                                result = highlighter.GetBestFragment(tokenStream, value);
                                if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption,value, result, field.GetBoost(), fpDict[key].IsTitle,true,fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key,value, result, field.GetBoost(), false,false,0));
                                }
                                else
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, value, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                        }
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        Highlighter highlighter = new Highlighter(new QueryScorer(query));
                        highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>(); 
                            foreach (Field field in fields)
                            {
                                string key = field.Name();
                                string value = field.StringValue();
                                TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                                string result = "";
                                result = highlighter.GetBestFragment(tokenStream, value);
                                if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, result, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                                else
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, value, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        public static List<SearchRecord> HighLightSearch()
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query query = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        Highlighter highlighter = new Highlighter(new QueryScorer(query));
                        highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                string key = field.Name();
                                string value = field.StringValue();
                                TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                                string result = "";
                                result = highlighter.GetBestFragment(tokenStream, value);
                                if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, result, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                                else
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, value, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                        }
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query query = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        Highlighter highlighter = new Highlighter(new QueryScorer(query));
                        highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                string key = field.Name();
                                string value = field.StringValue();
                                TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                                string result = "";
                                result = highlighter.GetBestFragment(tokenStream, value);
                                if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, result, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                                else
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, value, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        public static List<SearchRecord> HighLightSearch(out Query query, out Dictionary<string,List<int>> statistics)
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            query = GetQuery();
            statistics = new Dictionary<string,List<int>>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query theQuery = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        Highlighter highlighter = new Highlighter(new QueryScorer(theQuery));
                        highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
                        TopDocs topDocs = searcher.Search(theQuery.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        List<int> posList = new List<int>();
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                string key = field.Name();
                                string value = field.StringValue();
                                TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                                string result = "";
                                result = highlighter.GetBestFragment(tokenStream, value);
                                if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, result, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                                else
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, value, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                            posList.Add(recordList.Count - 1);
                        }
                        try
                        {
                            statistics.Add(indexSet.Caption, posList);
                        }
                        catch (Exception)
                        {
                            int i = 2;
                            while (statistics.ContainsKey(indexSet.Caption + i.ToString()))
                                i++;
                            statistics.Add(indexSet.Caption + i.ToString(), posList);
                        }
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query theQuery = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        Highlighter highlighter = new Highlighter(new QueryScorer(theQuery));
                        highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
                        TopDocs topDocs = searcher.Search(theQuery.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        List<int> posList=new List<int>();
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                string key = field.Name();
                                string value = field.StringValue();
                                TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                                string result = "";
                                result = highlighter.GetBestFragment(tokenStream, value);
                                if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, result, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                                else
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, value, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                            posList.Add(recordList.Count - 1);
                        }
                        try
                        {
                            statistics.Add(indexSet.Caption, posList);
                        }
                        catch (Exception)
                        {
                            int i = 2;
                            while (statistics.ContainsKey(indexSet.Caption + i.ToString()))
                                i++;
                            statistics.Add(indexSet.Caption + i.ToString(), posList);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        public static List<SearchRecord> HighLightSearch(out Dictionary<string,List<int>> statistics)
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            statistics = new Dictionary<string,List<int>>();
            try
            {
                if (searchIndexList.Count > 0)
                {
                    foreach (IndexSet indexSet in searchIndexList)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query query = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        Highlighter highlighter = new Highlighter(new QueryScorer(query));
                        highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        List<int> posList=new List<int>();
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                string key = field.Name();
                                string value = field.StringValue();
                                TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                                string result = "";
                                result = highlighter.GetBestFragment(tokenStream, value);
                                if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, result, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                                else
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, value, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                            posList.Add(recordList.Count - 1);
                        }
                        try
                        {
                            statistics.Add(indexSet.Caption, posList);
                        }
                        catch (Exception)
                        {
                            int i = 2;
                            while (statistics.ContainsKey(indexSet.Caption + i.ToString()))
                                i++;
                            statistics.Add(indexSet.Caption + i.ToString(), posList);
                        }
                    }
                }
                else
                {
                    foreach (IndexSet indexSet in indexFieldsDict.Keys)
                    {
                        if (indexSet.Type == IndexTypeEnum.Increment)
                            continue;
                        Query query = GetQuery(indexSet);
                        Source source = indexDict[indexSet];
                        Dictionary<string, FieldProperties> fpDict = source.FieldDict;
                        //IndexSearcher searcher = new IndexSearcher(indexSet.Path);
                        IndexSearcher presearcher = new IndexSearcher(indexSet.Path);
                        ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                        System.Console.WriteLine(query.ToString());
#endif
                        Highlighter highlighter = new Highlighter(new QueryScorer(query));
                        highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
                        TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                        ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                        List<int> posList=new List<int>();
                        for (int i = 0; i < scoreDocs.Length; i++)
                        {
                            float score = scoreDocs[i].score;
                            if (score < searchSet.MinScore)
                                continue;
                            Document doc = searcher.Doc(scoreDocs[i].doc);
                            Field[] fields = new Field[doc.GetFields().Count];
                            doc.GetFields().CopyTo(fields, 0);
                            List<SearchField> sfList = new List<SearchField>();
                            foreach (Field field in fields)
                            {
                                string key = field.Name();
                                string value = field.StringValue();
                                TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StringReader(value));
                                string result = "";
                                result = highlighter.GetBestFragment(tokenStream, value);
                                if (result != null && string.IsNullOrEmpty(result.Trim()) == false)
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, result, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                                else
                                {
                                    if (fpDict.ContainsKey(key))
                                        sfList.Add(new SearchField(key, fpDict[key].Caption, value, value, field.GetBoost(), fpDict[key].IsTitle, true, fpDict[key].Order));
                                    else
                                        sfList.Add(new SearchField(key, key, value, result, field.GetBoost(), false, false, 0));
                                }
                            }
                            recordList.Add(new SearchRecord(indexSet, sfList, indexDict[indexSet].PrimaryKey, score));
                            posList.Add(recordList.Count - 1);
                        }
                        try
                        {
                            statistics.Add(indexSet.Caption, posList);
                        }
                        catch (Exception)
                        {
                            int i = 2;
                            while (statistics.ContainsKey(indexSet.Caption + i.ToString()))
                                i++;
                            statistics.Add(indexSet.Caption + i.ToString(), posList);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        public static List<SearchRecord> SearchFile()
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            try
            {
                Query query = GetFileQuery();
                IndexSearcher presearcher = new IndexSearcher(fileSet.Path);
                ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                System.Console.WriteLine(query.ToString());
#endif
                TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                for (int i = 0; i < scoreDocs.Length; i++)
                {
                    float score = scoreDocs[i].score;
                    if (score < searchSet.MinScore)
                        continue;
                    Document doc = searcher.Doc(scoreDocs[i].doc);
                    string name = doc.Get("Name");
                    string path = doc.Get("Path");
                    string content = doc.Get("Content");
                    SearchField nf = new SearchField("文件名", "文件名", name, name, 1.0f, true, true, 0);
                    SearchField pf = new SearchField("路径", "路径", path, path, 1.0f, false, true, 0);
                    SearchField cf = new SearchField("内容", "内容", content, content, 1.0f, false, true, 0);
                    recordList.Add(new SearchRecord("文件","文件","文件",score,nf,pf,cf));
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        public static List<SearchRecord> HighLightSearchFile()
        {
            List<SearchRecord> recordList = new List<SearchRecord>();
            try
            {
                Query query = GetFileQuery();
                IndexSearcher presearcher = new IndexSearcher(fileSet.Path);
                ParallelMultiSearcher searcher = new ParallelMultiSearcher(new IndexSearcher[] { presearcher });
#if DEBUG
                System.Console.WriteLine(query.ToString());
#endif
                Highlighter highlighter = new Highlighter(new QueryScorer(query));
                highlighter.SetTextFragmenter(new SimpleFragmenter(SupportClass.FRAGMENT_SIZE));
                TopDocs topDocs = searcher.Search(query.Weight(searcher), null, searchSet.MaxMatches);
                ScoreDoc[] scoreDocs = topDocs.scoreDocs;
                for (int i = 0; i < scoreDocs.Length; i++)
                {
                    float score = scoreDocs[i].score;
                    if (score < searchSet.MinScore)
                        continue;
                    Document doc = searcher.Doc(scoreDocs[i].doc);
                    string name = doc.Get("Name");
                    string path = doc.Get("Path");
                    string content = doc.Get("Content");
                    TokenStream nts = analyzer.TokenStream("Name", new System.IO.StringReader(name));
                    TokenStream pts = analyzer.TokenStream("Path", new System.IO.StringReader(path));
                    TokenStream cts = analyzer.TokenStream("Content", new System.IO.StringReader(content));
                    string nr = "",pr="",cr="";
                    nr = highlighter.GetBestFragment(nts, name);
                    pr = highlighter.GetBestFragment(pts, path);
                    cr = highlighter.GetBestFragment(cts, content);
                    SearchField nf;
                    SearchField pf;
                    SearchField cf;
                    if (nr != null && string.IsNullOrEmpty(nr.Trim()) == false)
                    {
                        nf = new SearchField("文件名", "文件名", name, nr, 1.0f, true, true, 0);
                    }
                    else
                    {
                        nf = new SearchField("文件名", "文件名", name, name, 1.0f, true, true, 0);
                    }
                    if (pr != null && string.IsNullOrEmpty(pr.Trim()) == false)
                    {
                        pf = new SearchField("路径", "路径", path, pr, 1.0f, false, true, 0);
                    }
                    else
                    {
                        pf = new SearchField("路径", "路径", path, path, 1.0f, false, true, 0);
                    }
                    if (cr != null && string.IsNullOrEmpty(cr.Trim()) == false)
                    {
                        cf = new SearchField("内容", "内容", content, cr, 1.0f, false, true, 0);
                    }
                    else
                    {
                        cf = new SearchField("内容", "内容", content, content, 1.0f, false, true, 0);
                    }
                    recordList.Add(new SearchRecord("文件", "文件", "文件",score, nf, pf, cf));
                }
            }
            catch (Exception e)
            {
                SupportClass.FileUtil.WriteToLog(SupportClass.LogPath, e.StackTrace.ToString());
            }
            return recordList;
        }
        #endregion
    }
}
