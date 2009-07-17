using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using ISUtils;
using ISUtils.CSegment;

namespace ISUtils.Analysis.Chinese
{
    public class ChineseTokenizer : Tokenizer
    {
        /**/
        /// <summary>
        /// 保持传入的流
        /// </summary>
        private System.IO.TextReader reader;
        /**/
        /// <summary>
        /// 控制分词器只打开一次
        /// </summary>
        private bool done = true;
        /**/
        /// <summary>
        /// 保存分词结果
        /// </summary>
        private List<string> resultList;
        /**/
        /// <summary>
        /// 保存分词结果
        /// </summary>
        private List<int> startList;
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reader">TextReader（文本读取器）</param>
        public ChineseTokenizer(System.IO.TextReader reader):base(reader)
        {
            if (CSegment.Segment.IsInit() == false)
                throw new ApplicationException("Segment has not init!");
            this.reader = reader;
        }
        /**/
        /// <summary>
        /// 当前读取到分词的记录数
        /// </summary>
        private int ptr = 0;
        /**/
        /// <summary>
        /// 重写Next方法
        /// </summary>
        /// <param name="result">Token</param>
        /// <returns></returns>
        public override Token Next(Token result)
        {
            if (done )   //第一次可以运行，运行后将被设置为false,在一个实例中只会运行一次
            {
                done = false;
                ptr = 0;
                string text = reader.ReadToEnd();
                if (string.IsNullOrEmpty(text))
                    return null;
#if DEBUG
                System.Console.WriteLine(text);
#endif
                List<int> posList;
                resultList = Segment.SegmentStringEx(text,out posList);
                startList = posList;
                //返回第一个分词结果，并且把指针移向下一位
                Token token = new Token(resultList[ptr], startList[ptr], startList[ptr]+resultList[ptr].Length - 1);
                ptr++;
                return token;
            }
            else
            {
                //在分词结果范围内取词
                if (ptr < resultList.Count)
                {
                    if (resultList[ptr].Length == 0)
                    {
                        done = true;
                        ptr = 0;
                        return null;
                    }
                    Token token = new Token(resultList[ptr], startList[ptr],startList[ptr]+ resultList[ptr].Length - 1);
                    ptr++;
                    //done = true;
                    return token;
                }
                //超出则返回结束符号
                done = true;
                ptr = 0;
                return null;
            }
        }
    }
}
