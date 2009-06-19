using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using ISUtils.Analysis.Chinese;

namespace Test.Analysis
{
    [TestFixture]
    public class AllAnalysisTest
    {
        [Test]
        public void TestMethod(string[] fields, string testWords)
        {
            List<Analyzer> analysis = new List<Analyzer>();
            analysis.Add(new KeywordAnalyzer());
            //analysis.Add(new SimpleAnalyzer());
            analysis.Add(new StandardAnalyzer());
            //analysis.Add(new StopAnalyzer());
            analysis.Add(new WhitespaceAnalyzer());
            //analysis.Add(new EsayAnalyzer());
            //analysis.Add(new DoubleAnalyzer());
            //string path=@"F:\lwh\TestLucene\TestLucene\seglib\";
            //ChineseSegAnalysis ca = new ChineseSegAnalysis(path + "BaseDict.txt", path + "FamilyName.txt", path + "Number.txt", path + "CustomDict.txt", path+"Other.txt");
            //ca.FilterFilePath = path + "Filter.txt";
            //analysis.Add(ca.GetAnalyzer());
            for (int i = 0; i < analysis.Count; i++)
            {
                Console.WriteLine(analysis[i].ToString() + "结果：");
                Console.WriteLine("-----------------------------");
                TestFactory.QueryTestFunc(analysis[i], fields, testWords);
                Console.WriteLine("-----------------------------");
            }
        }
        [Test]
        public void TestMethod(string field,string testWords)
        {
            List<Analyzer> analysis = new List<Analyzer>();
            analysis.Add(new KeywordAnalyzer());
            //analysis.Add(new SimpleAnalyzer());
            analysis.Add(new StandardAnalyzer());
            //analysis.Add(new StopAnalyzer());
            analysis.Add(new WhitespaceAnalyzer());
            //analysis.Add(new EsayAnalyzer());
            //analysis.Add(new DoubleAnalyzer());
            //string path=@"F:\lwh\TestLucene\TestLucene\seglib\";
            //ChineseSegAnalysis ca = new ChineseSegAnalysis(path + "BaseDict.txt", path + "FamilyName.txt", path + "Number.txt", path + "CustomDict.txt", path+"Other.txt");
            //ca.FilterFilePath = path + "Filter.txt";
            //analysis.Add(ca.GetAnalyzer());
            for (int i = 0; i < analysis.Count; i++)
            {
                Console.WriteLine(analysis[i].ToString() + "结果：");
                Console.WriteLine("-----------------------------");
                TestFactory.QueryTestFunc(analysis[i],field,testWords);
                Console.WriteLine("-----------------------------");
            }
        }
        [Test]
        public void TestMethod(string testWords)
        {
            List<Analyzer> analysis = new List<Analyzer> ();
            analysis.Add(new KeywordAnalyzer());
            analysis.Add(new SimpleAnalyzer());
            analysis.Add(new StandardAnalyzer());
            analysis.Add(new StopAnalyzer());
            analysis.Add(new WhitespaceAnalyzer());
            analysis.Add(new EsayAnalyzer());
            analysis.Add(new DoubleAnalyzer());
            //string path=@"F:\lwh\TestLucene\TestLucene\seglib\";
            //ChineseSegAnalysis ca = new ChineseSegAnalysis(path + "BaseDict.txt", path + "FamilyName.txt", path + "Number.txt", path + "CustomDict.txt", path+"Other.txt");
            //ca.FilterFilePath = path + "Filter.txt";
            //analysis.Add(ca.GetAnalyzer());
            for ( int i =0 ; i < analysis.Count; i++ )
            {
                Console.WriteLine(analysis[i].ToString()+"结果：");
                Console.WriteLine("-----------------------------");
                if (string.IsNullOrEmpty(testWords))
                    TestFactory.TestFunc(analysis[i]);
                else
                    TestFactory.TestFunc(analysis[i], testWords);
                Console.WriteLine("-----------------------------");
            }
        }
        [Test]
        public void TestMethod()
        {
            List<Analyzer> analysis = new List<Analyzer>();
            analysis.Add(new KeywordAnalyzer());
            analysis.Add(new SimpleAnalyzer());
            analysis.Add(new StandardAnalyzer());
            analysis.Add(new StopAnalyzer());
            analysis.Add(new WhitespaceAnalyzer());
            analysis.Add(new EsayAnalyzer());
            analysis.Add(new DoubleAnalyzer());
            //string path=@"F:\lwh\TestLucene\TestLucene\seglib\";
            //ChineseSegAnalysis ca = new ChineseSegAnalysis(path + "BaseDict.txt", path + "FamilyName.txt", path + "Number.txt", path + "CustomDict.txt", path+"Other.txt");
            //ca.FilterFilePath = path + "Filter.txt";
            //analysis.Add(ca.GetAnalyzer());
            for (int i = 0; i < analysis.Count; i++)
            {
                Console.WriteLine(analysis[i].ToString() + "结果：");
                Console.WriteLine("-----------------------------");
                TestFactory.TestFunc(analysis[i]);
                Console.WriteLine("-----------------------------");
            }
        }
    }
}
