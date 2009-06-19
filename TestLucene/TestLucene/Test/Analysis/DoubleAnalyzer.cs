using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;

namespace Test.Analysis
{
    public class DoubleAnalyzer : Analyzer
    {
        public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
        {
            //throw new Exception("The method or operation is not implemented.");
            return new DoubleTokenizer(reader);
        }
    }
}
