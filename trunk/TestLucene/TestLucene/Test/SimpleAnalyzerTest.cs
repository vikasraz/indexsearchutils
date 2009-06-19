using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Lucene.Net.Analysis;
using System.IO;
namespace Test
{
    [TestFixture]
    public class SimpleAnalyzerTest
    {
        [Test]
        public void ReusabeleTokenStreamTest()
        {
            string testwords = "我是中国人，I can speak chinese!";
            SimpleAnalyzer sanalyzer = new SimpleAnalyzer();
            TokenStream ts = sanalyzer.ReusableTokenStream("", new StringReader(testwords));
            Token token;
            while ((token = ts.Next()) != null)
            {
                Console.WriteLine(token.TermText());
            }
            ts.Close();
        }
    }
}
