using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;
using System.IO;

namespace Test.Analysis
{
    public class EsayTokenizer : Tokenizer
    {
        private TextReader reader;
        public EsayTokenizer(TextReader reader)
        {
            this.reader = reader;
        }
        public override Token Next()
        {
            ////千万不能调用父类的方法，要不又是死递归
            //return base.Next();
            return null;
        }
    }
}
