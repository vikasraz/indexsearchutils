using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;
using System.IO;

namespace Test.Analysis
{
    public class DoubleTokenizer : Tokenizer
    {
        /**//// <summary>
        /// 保持传入的流
        /// </summary>
        private TextReader reader;
        /**//// <summary>
        /// 控制分词器只打开一次
        /// </summary>
        private bool done = true;
        /**//// <summary>
        /// 保存分词结果
        /// </summary>
        private List<Token> tokenlist;

        public DoubleTokenizer(TextReader reader)
        {
            this.reader = reader;
        }
        /**//// <summary>
        /// 上一个字的类型
        /// </summary>
        private CharType lastype = CharType.None;
        /**//// <summary>
        /// 当前读取到分词的记录数
        /// </summary>
        private int ptr = 0;
        /**//// <summary>
        /// 重写Next方法
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public override Token Next(Token result)
        {
            if (done)   //第一次可以运行，运行后将被设置为false,在一个实例中只会运行一次
            {
                done = false;
                string text = reader.ReadToEnd();
                //-------------------------------------------------------
                ////使用传入参数作为缓冲区
                //char[] charbuffer = result.TermBuffer();
                //int upto = 0;
                //result.Clear();
                //while (true)
                //{
                //    int length = input.Read(charbuffer, upto, charbuffer.Length - upto);
                //    if (length <= 0)
                //        break;
                //    upto += length;
                //    if (upto == charbuffer.Length)
                //        charbuffer = result.ResizeTermBuffer(1 + charbuffer.Length);
                //}
                //result.SetTermLength(upto);
                ////------------------------------------------------------
                //string text = result.TermText();
                //输入为空，则返回结束符号
                if (string.IsNullOrEmpty(text))
                    return null;
                //初始化分词结果
                tokenlist = new List<Token>();
                //缓冲器，主要用于暂时保存英文数字字符。
                StringBuilder buffer = new StringBuilder();
                Token token;
                for (int i = 0; i < text.Length; i++)
                {
                    char nowchar = text[i];
                    char nextchar = new char();
                    CharType nowtype = GetCharType(nowchar);
                    if (i < text.Length - 1)  //取下一个字符
                        nextchar = text[i + 1];
                    //状态转换
                    if (nowtype != lastype)
                    {
                        lastype = nowtype;
                        if (buffer.Length > 0)
                        {
                            token = new Token(buffer.ToString(), i - buffer.Length, i);
                            tokenlist.Add(token);
                            buffer.Remove(0, buffer.Length);
                        }
                    }

                    switch (nowtype)
                    {
                        case CharType.None:
                        case CharType.Control:
                            goto SingleChar;
                        case CharType.Chinese:
                            break;
                        case CharType.English:
                        case CharType.Number:
                            buffer.Append(nowchar);
                            continue;
                    }
                    //处理连续两个中文字符
                    if (GetCharType(nextchar) == CharType.Chinese)
                    {
                        token = new Token(nowchar.ToString() + nextchar.ToString(), i, i + 2);
                        tokenlist.Add(token);
                        i++;
                        continue;
                    }

                SingleChar:     //处理单个字符
                    token = new Token(nowchar.ToString(), i, i + 1);
                    tokenlist.Add(token);
                    continue;
                }
                //返回第一个分词结果，并且把指针移向下一位
                return tokenlist[ptr++];
            }
            else
            {
                //在分词结果范围内取词
                if (ptr < tokenlist.Count)
                    return tokenlist[ptr++];
                //超出则返回结束符号
                return null;
            }
        }
        /**//// <summary>
        /// 获取Char类型
        /// </summary>
        /// <param name="c">字符</param>
        /// <returns>返回类型</returns>
        public static CharType GetCharType(char c)
        {
            switch (char.GetUnicodeCategory(c))
            {
                //大小写字符判断为英文字符
                case System.Globalization.UnicodeCategory.UppercaseLetter:
                case System.Globalization.UnicodeCategory.LowercaseLetter:
                    return CharType.English;
                //其它字符判断问中文(CJK)
                case System.Globalization.UnicodeCategory.OtherLetter:
                    return CharType.Chinese;
                //十进制数字
                case System.Globalization.UnicodeCategory.DecimalDigitNumber:
                    return CharType.Number;
                //其他都认为是符号
                default:
                    return CharType.Control;
            }
        }
    }
    /**//// <summary>
    /// Char类型枚举，用于分词中类型状态比较
    /// </summary>
    public enum CharType
    {
        None,   //默认值，不可识别类型
        English,    //拉丁字符，用英文标识
        Chinese,    //CJK字符，以中文代表
        Number,     //阿拉伯数字
        Control     //控制符号，指控制符号已经各种标点符号等
    }
}