using Lucene.Net.Analysis;

namespace Test.Analysis
{
    public class EsayAnalyzer : Analyzer
    {
        public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
        {
            return new EsayTokenizer(reader);
        }
    }
}
