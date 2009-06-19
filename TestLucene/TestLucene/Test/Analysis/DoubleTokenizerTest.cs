using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using Lucene.Net.Analysis;

namespace Test.Analysis
{
    [TestFixture]
    public class DoubleTokenizerTest
    {
        [Test]
        public void NextTest()
        {
            string testwords = "我是一个中国人，代码yurow001,真是个好名字啊!!!哈哈哈。。。";
            DoubleTokenizer dt = new DoubleTokenizer(new StringReader(testwords));
            Token token;
            while ((token = dt.Next()) != null)
            {
                Console.WriteLine(token.TermText());
            }
            dt.Close();
        }
    }
}
