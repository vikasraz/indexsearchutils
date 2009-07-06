using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;

namespace ISUtils.Analysis.Chinese
{
    public sealed class ChineseAnalyzer : Analyzer
    {
        /**/
        /// <summary>
        /// 分词器
        /// </summary>
        /// 在分析器中共享一个单一的token实例也将缓解GC的压力。
        //private static Tokenizer tokenizer;
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        public ChineseAnalyzer()
        {
            if (CSegment.Segment.IsInit() == false)
                throw new ApplicationException("Segment has not init!");
        }
        /// <summary>Creates a TokenStream which tokenizes all the text in the provided
        /// Reader.  Default implementation forwards to tokenStream(Reader) for 
        /// compatibility with older version.  Override to allow Analyzer to choose 
        /// strategy based on document and/or field.  Must be able to handle null
        /// field name for backward compatibility. 
        /// </summary>
        public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
        {
            //if (tokenizer == null)
            //   tokenizer=new ChineseTokenizer(reader);
           return new ChineseTokenizer(reader);
        }
        public override TokenStream ReusableTokenStream(string fieldName, System.IO.TextReader reader)
        {
            Tokenizer tokenizer = (Tokenizer)GetPreviousTokenStream();
            if (tokenizer == null)
            {
                tokenizer = new ChineseTokenizer(reader);
                SetPreviousTokenStream(tokenizer);
            }
            else
                tokenizer.Reset(reader);
            return tokenizer;
        }
    }
}
