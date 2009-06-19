using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using System.IO;
namespace Test.Analysis
{
    public class TestFactory
    {
        public static void TestFunc(Analyzer analyzer)
        {
            TokenStream ts = analyzer.ReusableTokenStream("", new StringReader(TestData.TestWords));
            Lucene.Net.Analysis.Token token;
            while ((token = ts.Next()) != null)
            {
                Console.WriteLine(token.TermText());
            }
            ts.Close();
        }
        public static void TestFunc(Analyzer analyzer,string testWords)
        {
            TokenStream ts = analyzer.ReusableTokenStream("", new StringReader(testWords));
            Lucene.Net.Analysis.Token token;
            while ((token = ts.Next()) != null)
            {
                Console.WriteLine(token.TermText());
            }
            ts.Close();
        }
        public static void QueryTestFunc(Analyzer analyzer,string field,string testWords)
        {
            QueryParser queryParser = new QueryParser(field, analyzer);
            Query query = queryParser.Parse(testWords);
            Console.WriteLine(query.ToString());
        }
        public static void QueryTestFunc(Analyzer analyzer, string[] fields, string testWords)
        {
            QueryParser queryParser = new MultiFieldQueryParser(fields, analyzer);
            Query query = queryParser.Parse(testWords);
            Console.WriteLine(query.ToString());
        }
    }
}
