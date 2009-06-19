using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using NUnit.Framework;

namespace Test.Analysis
{
    [TestFixture]
    public class StandardAnalyzerCaseTest
    {
        /**//// <summary>
        /// 执行测试的入口
        /// </summary>
        [Test]
        public void SearcherTest()
        {
            Index();
            List<string> list = new List<string>();
            list.Add("中华");
            list.Add("中国");
            list.Add( "人民");
            list.Add( "中国人民");
            list.Add( "人民" );
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine("搜索词：" + list[i]);
                Console.WriteLine("结果：");
                Searcher(list[i]);
                Console.WriteLine("-----------------------------------");
            }
        }

        /**//// <summary>
        /// 搜索
        /// </summary>
        /// <param name="querystring">搜索输入</param>
        private void Searcher(string querystring)
        {
            Analyzer analyzer = new StandardAnalyzer();
            IndexSearcher searcher = new IndexSearcher("IndexDirectory");
            QueryParser parser = new QueryParser("content", analyzer);
            Query query = parser.Parse(querystring);
            Hits hits = searcher.Search(query);
            for (int i = 0; i < hits.Length(); i++)
            {
                Console.WriteLine(hits.Doc(i).Get("content"));
            }
        }

        /**//// <summary>
        /// 索引数据
        /// </summary>
        private void Index()
        {
            Analyzer analyzer = new StandardAnalyzer();
            IndexWriter writer = new IndexWriter("IndexDirectory", analyzer, true);
            AddDocument(writer, "中华人民共和国");
            AddDocument(writer, "中国人民解放军");
            AddDocument(writer, "人民是伟大的，祖国是伟大的。");
            AddDocument(writer, "你站在边上，我站在中央。");
            writer.Optimize();
            writer.Close();
        }
        /**//// <summary>
        /// 为索引准备数据
        /// </summary>
        /// <param name="writer">索引实例</param>
        /// <param name="content">需要索引的数据</param>
        void AddDocument(IndexWriter writer, string content)
        {
            Document document = new Document();
            document.Add(new Field("content", content, Field.Store.YES, Field.Index.TOKENIZED));
            writer.AddDocument(document);
        }
    }
}
