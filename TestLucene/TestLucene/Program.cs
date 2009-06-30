using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;

namespace TestLucene
{
    using Lucene.Net.Index;
    using Lucene.Net.Store;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    //using for search index
    using Lucene.Net.Search;
    using Lucene.Net.QueryParsers;
    //using Test;
    //using Test.Analysis;
    using ISUtils.Analysis.Chinese;
    using ISUtils.Common;
    using ISUtils.Utils;
    using ISUtils.Database.Indexer;
    using ISUtils.Searcher;
    using ISUtils.Indexer;
    using ISUtils.CSegment;

    class Program
    {
        const int TESTNUM = 10000;
        const string TESTTEXT = "新浪科技讯 2月23日上午消息，已成为热门关键词的丁磊养猪事件似乎并未吸引风险投资的兴趣。虽然投资者会用“有趣”一词来形容这样一则新闻，但更多的是保持谨慎观望态度。\n\n　　早在去年初，风险投资已经开始将嗅觉转向传统领域，新农业是关注重点之一，然而具体到养猪项目，则看的多投的少。即使加上足够吸引眼球的网易CEO丁磊，也并未能吸引投资者对“开源养猪”模式的兴趣。\n\n　　丁磊的计划是：养10000头猪来探索一套科学养猪模式供大家分享，他的优势在于，其养猪计划的背后挂靠一个老牌互联网公司，他希望用互联网直播等方式来公开猪肉信息、探索一个盈利的高效养猪模式。";
        //const string TESTTEXT = "2月23日上午消息";//，已成为热门关键词的丁磊养猪事件似乎并未吸引风险投资的兴趣。虽然投资者会用“有趣”一词来形容这样一则新闻，但更多的是保持谨慎观望态度。\n\n　　早在去年初，风险投资已经开始将嗅觉转向传统领域，新农业是关注重点之一，然而具体到养猪项目，则看的多投的少。即使加上足够吸引眼球的网易CEO丁磊，也并未能吸引投资者对“开源养猪”模式的兴趣。\n\n　　丁磊的计划是：养10000头猪来探索一套科学养猪模式供大家分享，他的优势在于，其养猪计划的背后挂靠一个老牌互联网公司，他希望用互联网直播等方式来公开猪肉信息、探索一个盈利的高效养猪模式。";
        //const string TESTTEXT = "养10000头猪来探索一套科学养猪模式供大家分享";
        void test()
        { 
            string src = "我们都是中国人";
            string[] tokens = new string[] { "我", "我们", "都", "是" ,"中国", "中国人","人" };
            ISUtils.SupportClass.Offset[] offsets;
            offsets = ISUtils.SupportClass.String.GetOffsets(src, tokens);
            for (int i = 0; i < offsets.Length; i++ )
            {
                Console.WriteLine(string.Format("{0}\t{1}\t{2}", tokens[i],offsets[i].Start, offsets[i].End));
            }
        }
        static void testDB()
        {
            string path = @"F:\lwh\trunk\TestLucene\TestLucene\config.conf";
            IndexMaker oper = new IndexMaker(path);
            DateTime start = DateTime.Now;
            Console.WriteLine("Start " + start.ToLocalTime().ToString());
            oper.ExecuteIndexer(new TimeSpan(0, 2, 0), IndexTypeEnum.Increment);
            TimeSpan span = DateTime.Now - start;
            Console.WriteLine(string.Format("Spend {0} ", span.ToString()));
        }
        static void testSearch()
        {
            string path = @"d:\Indexer\config.conf";
            SearchMaker searcher = new SearchMaker(path);
            QueryInfo info = new QueryInfo();
            info.IsFuzzySearch = false;
            info.SQuery.IndexNames = "IN_IndexView_Monitoring_RSSPI,IN_IndexView_Monitoring_PM";
            info.SQuery.FilterList.Add(new FilterCondition("","JSDW", "东丽"));
            DateTime start = DateTime.Now;
            Query query;
            List<Document> results = searcher.ExecuteFastSearch(info,out query);
            TimeSpan span = DateTime.Now - start;
            Console.WriteLine(string.Format("Spend {0} ", span.ToString()));
            //ISUtils.SupportClass.Result.Output(result);
            foreach (Document doc in results)
            {
                Field[] fields = new Field[doc.GetFields().Count];
                doc.GetFields().CopyTo(fields, 0);
                foreach (Field field in fields)
                {
                    string key = field.Name();
                    string value = field.StringValue();
                    Console.WriteLine(key + ":\t" + value);
                }
                Console.WriteLine("--------------------------------");
            }
        }
        static void TeseCSegment()
        {
            string path = @"d:\Indexer\seglib\";
            Segment.SetPaths(path + "BaseDict.txt", path + "FamilyName.txt", path + "Number.txt", path + "CustomDict.txt", path + "Other.txt");
            string text = "ASP.Net MVC框架配置与分析 而今，微软推出了新的MVC开发框架，也就是Microsoft ASP.NET 3.5 Extensions";
            Segment.SetDefaults(new ISUtils.CSegment.DictionaryLoader.TextDictionaryLoader(), new ForwardMatchSegment());
            //Segment.OutputDictionary();
            DateTime now = DateTime.Now;
            Console.WriteLine(Segment.SegmentString(text));
            TimeSpan span = DateTime.Now - now;
            Console.WriteLine(span.TotalMilliseconds);
            //Segment.SetDefaults(new ISUtils.CSegment.DictionaryLoader.TextDictionaryLoader(), new ForwardMatchSegment());
            //Segment.OutputDictionary();
            //now = DateTime.Now;
            //Console.WriteLine(Segment.SegmentString(text));
            //span = DateTime.Now - now;
            //Console.WriteLine(span.TotalMilliseconds);
        }
        static void TestForQuery()
        {
            string path = @"d:\Indexer\seglib\";
            string searchWords = "中国人民解放军 93688";
            Segment.SetPaths(path + "BaseDict.txt", path + "FamilyName.txt", path + "Number.txt", path + "CustomDict.txt", path + "Other.txt");
            Segment.SetDefaults(new ISUtils.CSegment.DictionaryLoader.TextDictionaryLoader(), new ForwardMatchSegment());
            string result = Segment.SegmentString(searchWords," ");
            Console.WriteLine(result);
            //AllAnalysisTest aat = new AllAnalysisTest();
            //aat.TestMethod(new string[] { "LZDDH", "SYQLX" }, result);
            //Analyzer analyzer = new SimpleAnalyzer();
            //QueryParser queryParser = new QueryParser("LZDDH", analyzer);
            //Query query = queryParser.Parse(result);
            //Console.WriteLine(query.ToString());
            //WriteIndex(new ChineseAnalyzer(), "indexdir");
            WriteIndex(new WhitespaceAnalyzer(), "indexdir");
            //SearchIndex(new ChineseAnalyzer(), "IndexDir", "发布");
            //SearchIndex(new KeywordAnalyzer(), "IndexDir", "发布");
            //SearchIndex(new StandardAnalyzer(), "IndexDir", "发布");
            //SearchIndex(new WhitespaceAnalyzer(), "IndexDir", "发布");
            SearchIndex(new WhitespaceAnalyzer(), "IndexDir", Segment.SegmentString("发布", " "));
            //SearchIndex(new StandardAnalyzer(), "IndexDir", Segment.SegmentString("发布", " "));
            //SearchIndex(new WhitespaceAnalyzer(), "IndexDir", Segment.SegmentString("发布", " "));
        }
        static void TestForChineseSegment()
        {
            List<string> testList = new List<string>();
            testList.Add("MVC框架配置与分析");
            testList.Add("发布");
            testList.Add("SQL Server 2008 的新特性");
            testList.Add("SQL Server 2008 的发布");
            testList.Add("ASP.Net MVC框架配置与分析");
            testList.Add("而今，微软推出了新的MVC开发框架，也就是Microsoft ASP.NET 3.5 Extensions");
            testList.Add("中国人民解放军");
            testList.Add("新浪科技讯 2月23日上午消息");
            testList.Add("已成为热门关键词的丁磊养猪事件似乎并未吸引风险投资的兴趣");
            testList.Add("虽然投资者会用“有趣”一词来形容这样一则新闻");
            testList.Add("但更多的是保持谨慎观望态度");
            testList.Add("早在去年初");
            testList.Add("风险投资已经开始将嗅觉转向传统领域");
            testList.Add("新农业是关注重点之一");
            testList.Add("然而具体到养猪项目");
            testList.Add("则看的多投的少");
            testList.Add("即使加上足够吸引眼球的网易CEO丁磊");
            testList.Add("也并未能吸引投资者对“开源养猪”模式的兴趣");
            testList.Add("丁磊的计划是");
            testList.Add("养10000头猪来探索一套科学养猪模式供大家分享");
            testList.Add("他的优势在于");
            testList.Add("其养猪计划的背后挂靠一个老牌互联网公司");
            testList.Add("他希望用互联网直播等方式来公开猪肉信息、探索一个盈利的高效养猪模式。");
            string path = @"d:\Indexer\seglib\";
            Segment.SetPaths(path + "BaseDict.txt", path + "FamilyName.txt", path + "Number.txt", path + "CustomDict.txt", path + "Other.txt");
            Segment.SetDefaults(new ISUtils.CSegment.DictionaryLoader.TextDictionaryLoader(), new ForwardMatchSegment());
            foreach (string testWords in testList)
            {
                //Console.WriteLine(Segment.SegmentString(testWords," "));
                List<string> results = Segment.SegmentStringEx(testWords);
                Console.WriteLine(testWords);
                foreach (string result in results)
                    Console.Write(result+"\t"); 
                Console.WriteLine();
            }
        }
        static void Main()
        {
            //char ch = '中';
            //Console.WriteLine(ch);
            ////testDB();
            testSearch();
            //TestChineseSegment();
            //TeseCSegment();
            //RangeQuery query = new RangeQuery(new Term("date",DateTime.Parse("2006-01-01").ToShortDateString()), new Term("date", "20060130"), true);
            //Console.WriteLine(query.ToString());
            //Console.WriteLine(string.Format("{0}%2d%2d",DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day));
            //DateTime dt = DateTime.Now;
            //String[] format = {"d","D","f","F","g","G","m","r","s","t", "T","u", "U","y","dddd, MMMM dd yyyy","ddd, MMM d ","yy","dddd, MMMM dd","M/yy","dd-MM-yy","yyyyMMdd","yyyyMMddHHmmss","HHmmss"};
            //String date;
            //for (int i = 0; i < format.Length; i++)
            //{
            //    date = dt.ToString(format[i]);
            //    Console.WriteLine(String.Concat(format[i], " :" , date));
            //}
            //TestChineseSegmentIndexerSpeed();
            //AllAnalysisTest aat = new AllAnalysisTest();
            //aat.TestMethod("");
            //TestForQuery();
            //TestChineseSearch();
            //Mainf();
            //TestForChineseSegment();
            //TestRam();
            //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
            Console.ReadKey();
        }
        static void TestRam()
        {
            FSDirectory fsDir = FSDirectory.GetDirectory("testdir", true);
            RAMDirectory ramDir = new RAMDirectory();
            IndexWriter fsWriter = new IndexWriter(fsDir, new StandardAnalyzer(), true);
            IndexWriter ramWriter = new IndexWriter(ramDir, new StandardAnalyzer(), true);
            AddDocument(ramWriter, "SQL Server 2008 的发布", "SQL Server 2008 的新特性");
            AddDocument(ramWriter, "ASP.Net MVC框架配置与分析", "而今，微软推出了新的MVC开发框架，也就是Microsoft ASP.NET 3.5 Extensions");
            ramWriter.Flush();
            fsWriter.AddIndexes(new Directory[] { ramDir });
            fsWriter.Optimize();
            fsWriter.Close();
        }
        static void TestChineseSearch()
        {
            string path = @"d:\Indexer\config.conf";
            try
            {
                List<Source> sourceList;
                List<IndexSet> indexList;
                DictionarySet dictSet;
                SearchSet searchd;
                Parser parser = new Parser(path);
                searchd = parser.GetSearchd();
                sourceList = parser.GetSourceList();
                indexList = parser.GetIndexList();
                dictSet = parser.GetDictionarySet();
                QueryInfo info = new QueryInfo();
                info.FQuery.IndexNames = "in_main1,in_main2";
                info.FQuery.SearchWords = "中国人民解放军 93688";
                Console.WriteLine("SetSearchSettings");
                ISUtils.Utils.SearchUtil.SetSearchSettings(sourceList, indexList, dictSet, searchd);
                Console.WriteLine("SetQueryInfo");
                ISUtils.Utils.SearchUtil.SetQueryInfo(info);
                Console.WriteLine("UseDefaultChineseAnalyzer");
                //ISUtils.Utils.SearchUtil.UseDefaultChineseAnalyzer(true);
                //List<QueryResult.SearchInfo> qrsiList;
                Console.WriteLine("Search");
                //IndexSearcher searcher = new IndexSearcher(@"E:\TEMP\in_MAIN1\");
                ////TDZL,QSXZ,SYQLX,SYQR,LZDDH
                //QueryParser queryParser = new QueryParser("LZDDH", new ChineseAnalyzer());
                //Query query = queryParser.Parse("中国人民解放军 93688");
                ////输出我们要查看的表达式
                //Console.WriteLine(query.ToString());
                DateTime start = DateTime.Now;
                //Hits hits = searcher.Search(query);
                Hits hits = ISUtils.Utils.SearchUtil.Search();
                TimeSpan tm = DateTime.Now - start;
                for (int i = 0; i < hits.Length(); i++)
                {
                    Document doc = hits.Doc(i);
                    Console.WriteLine(doc.ToString());
                    //Console.WriteLine(string.Format("title:{0} \nhistoryName:{1}", doc.Get("id"), doc.Get("historyName")));
                }
                Console.WriteLine(hits.Length());
                Console.WriteLine("搜索测试完成，花费时间：" + tm.TotalMilliseconds.ToString() + "毫秒");
                //List<Hits> hitsList = ISUtils.Utils.SearchUtil.Search(out qrsiList);
                //QueryResult result = new QueryResult();
                //Console.WriteLine("AddResult");
                //result.AddResult(qrsiList, hitsList, searchd.MaxMatches);
                //Console.WriteLine("Output");
                //foreach (QueryResult.SearchInfo si in result.docs.Keys)
                //{
                //    Console.WriteLine("index :" + si.IndexName);
                //    foreach (QueryResult.ExDocument ed in result.docs[si])
                //    {
                //        Console.Write("Score=" + ed.score.ToString());
                //        foreach (string s in si.Fields)
                //        {
                //            Console.Write("\t" + ed.doc.Get(s));
                //        }
                //        Console.WriteLine();
                //        //Response.Write(ISUtils.SupportClass.Document.ToString(ed.doc)+ "<br>");
                //        //Console.WriteLine(string.Format("title:{0} \nhistoryName:{1}", doc.Get("id"), doc.Get("historyName")));
                //    }
                //}
                Console.WriteLine("Finish!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace.ToString());
            }
        }
        static void TestChineseSegmentIndexerSpeed()
        {
            string path = @"d:\Indexer\config.conf";
            //string dict = @"d:\Indexer\seglib\";
            //Segment.SetPaths(dict + "BaseDict.txt", dict + "FamilyName.txt", dict + "Number.txt",dict+"Filter.txt", dict + "CustomDict.txt", dict + "Other.txt");
            //Segment.SetDefaults(new ISUtils.CSegment.DictionaryLoader.TextDictionaryLoader(), new ForwardMatchSegment());
            ISUtils.Utils.IndexUtil.SetIndexSettings(path);
            //ISUtils.Utils.IndexUtil.UseDefaultChineseAnalyzer(true);
            //ISUtils.Utils.IndexUtil.SetAnalyzer(new ChineseAnalyzer());
            Console.WriteLine("Begin indexing....."+DateTime.Now.ToShortTimeString());
            DateTime start = DateTime.Now;
            ISUtils.Utils.IndexUtil.Index(IndexTypeEnum.Ordinary);
            TimeSpan span = DateTime.Now - start;
            Console.WriteLine(span.TotalMilliseconds.ToString());
        }
        static void Mainc()
        { 
            ArrayList list = new ArrayList(5);
            list.Add("B");
            list.Add("G");
            list.Add("J");
            list.Add("S");
            list.Add("M");

            string[] array1 = new string[list.Count];
            list.CopyTo(array1, 0);


            object[] array2 = list.ToArray();
            string[] array3 = (string[])list.ToArray(typeof(String));
            
            foreach (string s in array1)
            {
                Console.WriteLine(s);
            }
            foreach (string s in array2)
            {
                Console.WriteLine(s);
            }
            foreach (string s in array3)
            {
                Console.WriteLine(s);
            }
            Console.ReadKey();
        }       
        //static void MainT()
        //{
        //    Console.WriteLine("FILENAME:\t" + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        //    Console.WriteLine("CurDir:\t" + System.Environment.CurrentDirectory);
        //    Console.WriteLine("GetCurDir:\t" + System.IO.Directory.GetCurrentDirectory());
        //    Console.WriteLine("BaseDir:\t" + System.AppDomain.CurrentDomain.BaseDirectory);
        //    Console.WriteLine("AppBase:\t" + System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase);

        //    try
        //    {
        //        Parser parser = new Parser("config.conf");
        //        List<Source> sourceList = parser.GetSourceList();
        //        List<IndexSet> indexList = parser.GetIndexList();
        //        IndexerSet indexer = parser.GetIndexer();
        //        DictionarySet dictSet = parser.GetDictionarySet();
        //        foreach (Source source in sourceList)
        //            Console.WriteLine(source.ToString());
        //        foreach (IndexSet index in indexList)
        //            Console.WriteLine(index.ToString());
        //        Console.WriteLine(indexer.ToString());
        //        Console.WriteLine(string.Format("compare={0}", indexList[0].SourceName.CompareTo(sourceList[0].SourceName)));
        //        ChineseSegAnalysis csa = new ChineseSegAnalysis(dictSet.BasePath, dictSet.NamePath, dictSet.NumberPath, dictSet.CustomPaths);
        //        csa.FilterFilePath = dictSet.FilterPath;
        //        Analyzer analyzer = csa.GetAnalyzer();
        //        string connect = sourceList[0].GetConnString();
        //        Console.WriteLine(connect);
        //        DateTime start;
        //        DBCreateIndexer dbcIndexer = new DBCreateIndexer(analyzer, sourceList[0].DBType, connect, indexList[0].Path);
        //        start = DateTime.Now;
        //        dbcIndexer.WriteResults(sourceList[0].Query,int.MaxValue,512, indexer.MergeFactor, indexer.MaxBufferedDocs);
        //        TimeSpan span=DateTime.Now-start;
        //        Console.WriteLine(string.Format("用时{0}毫秒", span.TotalMilliseconds));
        //        Console.WriteLine(string.Format("S={0}", (int)span.TotalSeconds));
        //        // Insert code to set properties and fields of the object. 
        //        XmlSerializer mySerializer = new XmlSerializer(typeof(Parser)); 
        //        // To write to a file, create a StreamWriter object. // To write to a file, create a StreamWriter object. 
        //        StreamWriter myWriter = new  StreamWriter( "myFileName.xml" );
        //        mySerializer.Serialize(myWriter, parser); 
        //        myWriter.Close(); 

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //    Console.ReadKey();
        //}
        static void Mainf()
        {
            //string filepath = @"D:\xampp\apache\conf\httpd.conf";
            //FileReader.GetFileText(filepath);
            //Analyzer analyzer = new StandardAnalyzer();
            //WriteIndex(analyzer, "IndexDir");
            //SearchIndex(analyzer, "IndexDir", "发布");
            //SimpleAnalyzerTest sat = new SimpleAnalyzerTest();
            //sat.ReusabeleTokenStreamTest();
            //AllAnalysisTest aat = new AllAnalysisTest();
            //aat.TestMethod();
            //StandardAnalyzerCaseTest sact = new StandardAnalyzerCaseTest();
            //sact.SearcherTest();
            //DoubleTokenizerTest dtt = new DoubleTokenizerTest();
            //dtt.NextTest();
            //string path = @"d:\Indexer\seglib\";
            //ChineseSegAnalysis ca = new ChineseSegAnalysis(path + "BaseDict.txt", path + "FamilyName.txt", path + "Number.txt", path + "CustomDict.txt", path + "Other.txt");
            //Analyzer analyzer = ca.GetAnalyzer();

            //ca.FilterFilePath = path + "Filter.txt";

            //DateTime now = DateTime.Now;
            //WriteIndex(analyzer, "IndexDir");
            //TimeSpan tm = DateTime.Now - now;
            //Console.WriteLine("写索引完成，花费时间：" + tm.TotalMilliseconds.ToString() + "毫秒");
            //SearchIndex(analyzer, "IndexDir", "开发");
            //Console.WriteLine("测试完成！\n数据库索引测试开始！");
            ////string connect = "Data Source=192.168.1.254;Initial Catalog=donglidnn;User ID=dnn5;Password=19370707japan";
            ////DBCreateIndexer dbcIndexer = new DBCreateIndexer(analyzer, DataBaseType.SQL_SERVER, connect, @"E:\TEMP\DB\");
            ////now = DateTime.Now;
            ////dbcIndexer.WriteResults("select * from dbo.businessHistory");
            ////tm = DateTime.Now - now;
            ////Console.WriteLine("写索引完成，花费时间：" + tm.TotalMilliseconds.ToString() + "毫秒");
            //IndexSearcher searcher = new IndexSearcher(@"E:\TEMP\DB\");
            //QueryParser parser = new QueryParser("historyName", analyzer);
            //Query query = parser.Parse("东丽");
            ////输出我们要查看的表达式
            //Console.WriteLine(query.ToString());
            //now = DateTime.Now;
            //Hits hits = searcher.Search(query);
            
            //tm = DateTime.Now - now;
            //for (int i = 0; i < hits.Length(); i++)
            //{
            //    Document doc = hits.Doc(i);
            //    Console.WriteLine(doc.ToString());
            //    //Console.WriteLine(string.Format("title:{0} \nhistoryName:{1}", doc.Get("id"), doc.Get("historyName")));
            //}
            //Console.WriteLine("搜索测试完成，花费时间：" + tm.TotalMilliseconds.ToString() + "毫秒");
            Console.ReadKey();
        }
        static void WriteIndex(Analyzer analyzer,string indexdir)
        {
            IndexWriter writer = new IndexWriter(indexdir, analyzer, true);
            AddDocument(writer, "SQL Server 2008 的发布", "SQL Server 2008 的新特性");
            AddDocument(writer, "ASP.Net MVC框架配置与分析", "而今，微软推出了新的MVC开发框架，也就是Microsoft ASP.NET 3.5 Extensions");
            writer.Optimize();
            writer.Close();
        }
        static void SearchIndex(Analyzer analyzer,string indexdir,string squery)
        {
            IndexSearcher searcher = new IndexSearcher(indexdir);
            MultiFieldQueryParser parser = new MultiFieldQueryParser(new string[] { "title", "content" }, analyzer);
            Query query = parser.Parse(squery);
            Hits hits = searcher.Search(query);
            Console.WriteLine(query.ToString());
            Console.WriteLine("hits.length=" + hits.Length().ToString());
            for (int i = 0; i < hits.Length(); i++)
            {
                Document doc = hits.Doc(i);
                Console.WriteLine(string.Format("title:{0} \ncontent:{1}", doc.Get("title"), doc.Get("content")));
            }
            searcher.Close();
        }
        static void AddDocument(IndexWriter writer, string title, string content)
        {
            Document doc = new Document();
            //doc.Add(new Field("title", Segment.SegmentString(title, " "), Field.Store.COMPRESS, Field.Index.TOKENIZED));
            //doc.Add(new Field("content", Segment.SegmentString(content, " "), Field.Store.COMPRESS, Field.Index.TOKENIZED));
            doc.Add(new Field("title", title, Field.Store.COMPRESS, Field.Index.TOKENIZED));
            doc.Add(new Field("content", content, Field.Store.COMPRESS, Field.Index.TOKENIZED));
            //doc.GetField("tag").SetBoost(boost);
            Console.WriteLine(doc.ToString());
            writer.AddDocument(doc);
        }
    }
}
