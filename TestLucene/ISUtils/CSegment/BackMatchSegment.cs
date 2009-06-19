using System;
using System.Collections.Generic;
using System.Text;
using ISUtils.CSegment.SegmentDictionary;
using ISUtils.CSegment.Utility;

namespace ISUtils.CSegment
{
    /// <summary>
    /// 反向最大匹配分词。
    /// </summary>
    public class BackMatchSegment : WordSegmentBase
    {
        private ISegmentDictionary _segmentDictionary = new BackSegmentDictionary();
        /// <summary>
        /// 
        /// </summary>
        public override ISegmentDictionary SegmentDictionary
        {
            get
            {
                if (_segmentDictionary == null)
                {
                    _segmentDictionary = new BackSegmentDictionary();
                }
                return _segmentDictionary;
            }
        }
        /// <summary>
        /// 对单个句子分词。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected override List<string> SegmentSentence(string sentence)
        {
            List<string> resultList = new List<string>();
            if (string.IsNullOrEmpty(sentence))
            {
                return resultList;
            }

            if (this.SegmentDictionary.Segments.Count <= 0)
            {
                return resultList;
            }

            string currentChar;
            string prevChar;
            string currentTwoChars;
            StringBuilder result = new StringBuilder();
            int charCursor = sentence.Length - 1;
            for (charCursor = sentence.Length - 1; charCursor >0; charCursor--)
            {
                currentChar = sentence.Substring(charCursor, 1);

                //非汉字
                if (!Character.IsChinese(currentChar))
                {
                    prevChar = sentence.Substring(charCursor - 1, 1);
                    if (Character.IsLetter(currentChar) && Character.IsChinese(prevChar))
                    {
                        //当前字符是字母，且前一字符是汉字。
                        result.Insert(0,currentChar);
                        resultList.Add(result.ToString());
                        result.Remove(0, result.Length);
                    }
                    else
                    {
                        //当前字符是数字
                        result.Insert(0,currentChar);
                    }
                    continue;
                }
                if (!this.SegmentDictionary.SegmentKeys.ContainsKey(currentChar))
                {
                    resultList.Add(currentChar);
                    continue;
                }
                //汉字
                currentTwoChars = sentence.Substring(charCursor-1, 2);

                //以当前两字符开头的词，在词库中不存在
                if (!this.SegmentDictionary.Segments.ContainsKey(currentTwoChars))
                {
                    //当前字符是姓
                    if (this.NameSegments.ContainsKey(currentTwoChars))
                    {
                        result.Insert(0,currentChar);
                        resultList.Add(result.ToString());
                        result.Remove(0, result.Length);
                    }
                    else if (this.NameSegments.ContainsKey(currentChar))
                    {
                        result.Insert(0,currentTwoChars);
                        resultList.Add(result.ToString());
                        result.Remove(0, result.Length);
                    }
                    else
                    {
                        result.Insert(0,currentChar);
                    }
                    continue;
                }

                //以当前两字符开头的词，在词库中存在,取出所有匹配的词并抽取最大程度的词。
                int len = 2;
                for (int i = 2; i <= this.SegmentDictionary.MaxSegmentLength; i++)
                {
                    string element = sentence.Substring(charCursor-i, i);
                    if (this.SegmentDictionary.SegmentDict.ContainsKey(element))
                    {
                        resultList.Add(element);
                        len = i;
                    }
                }
                charCursor -= len;
            }

            //处理最后一个字符
            if (charCursor >0)
            {
                resultList.Add(sentence.Substring(0,1));
            }
            return resultList;
        }
        /// 对单个句子分词。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="result"></param>
        protected override void SegmentSentence(string text, ref StringBuilder result)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (this.SegmentDictionary.Segments.Count <= 0)
            {
                return;
            }

            string currentChar;
            //string nextChar;
            string currentTwoChars;

            int charCursor = text.Length - 1;
            for (charCursor = text.Length - 1; charCursor >0; charCursor--)
            {
                currentChar = text.Substring(charCursor, 1);

                //非汉字
                if (!Character.IsChinese(currentChar))
                {
                    ProcessNonChinese(text, result, currentChar, charCursor);
                    continue;
                }
                if (!this.SegmentDictionary.SegmentKeys.ContainsKey(currentChar))
                {
                    result.Insert(0,currentChar);
                    result.Insert(0,this.Separator);
                    continue;
                }
                //汉字
                currentTwoChars = text.Substring(charCursor-1, 2);

                //以当前两字符开头的词，在词库中不存在
                if (!this.SegmentDictionary.Segments.ContainsKey(currentTwoChars))
                {
                    //当前字符是姓
                    if (this.NameSegments.ContainsKey(currentChar))
                    {
                        result.Insert(0,currentChar);
                    }
                    else if (this.NameSegments.ContainsKey(currentTwoChars))
                    {
                        result.Insert(0,currentTwoChars);
                    }
                    else
                    {
                        result.Insert(0,currentChar);
                        result.Insert(0,this.Separator);
                    }
                    continue;
                }

                //以当前两字符开头的词，在词库中存在,取出所有匹配的词并抽取最大程度的词。
                ProcessExistingElement(text, result, currentChar, currentTwoChars, ref charCursor);
            }

            //处理最后一个字符
            ProcessLastChar(text, charCursor, result);
        }



        /// <summary>
        /// 处理非汉字
        /// </summary>
        /// <param name="text"></param>
        /// <param name="result"></param>
        /// <param name="currentChar"></param>
        /// <param name="currentIndex"></param>
        private void ProcessNonChinese(string text, StringBuilder result, string currentChar, int currentIndex)
        {
            string prevChar;
            prevChar = text.Substring(currentIndex - 1, 1);

            if (Character.IsLetter(currentChar) && Character.IsChinese(prevChar))
            {
                //当前字符是字母，且前一字符是汉字。
                result.Insert(0,currentChar);
                result.Insert(0,this.Separator);
            }
            else
            {
                //当前字符是数字
                result.Insert(0,currentChar);
            }
        }

        /// <summary>
        /// 以当前两字符开头的词，在词库中存在,取出所有匹配的词并抽取最大程度的词。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="result"></param>
        /// <param name="currentTwoChars"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private void ProcessExistingElement(string text, StringBuilder result,
            string currentChar, string currentTwoChars, ref int currentIndex)
        {
            int len = 2;
            for (int i = 2; i <= this.SegmentDictionary.MaxSegmentLength; i++)
            {
                if (currentIndex < i)
                    break;
                string element = text.Substring(currentIndex - i, i);
                if ( this.SegmentDictionary.SegmentDict.ContainsKey(element))
                {
                    result.Insert(0,element);
                    result.Insert(0,this.Separator);
                    len = i;
                }
            }
            currentIndex -= len;
        }

        ///// <summary>
        ///// 继续对最大长度的分词进行分解。
        ///// </summary>
        ///// <param name="result"></param>
        ///// <param name="currentIndex"></param>
        ///// <param name="maxLenElement"></param>
        ///// <returns></returns>
        //private void ProcessMaxLenElement(StringBuilder result, ref int currentIndex, string maxLenElement)
        //{
        //    //长度小于4的，不再分解,直接追加最大长度的分词。
        //    if (maxLenElement.Length < 4)
        //    {
        //        return;
        //    }

        //    for (int i = 1; i < maxLenElement.Length - 1; i++)
        //    {
        //        string currentTwoChars = maxLenElement.Substring(i, 2);
        //        if (!this.SegmentDictionary.Segments.ContainsKey(currentTwoChars))
        //        {
        //            continue;
        //        }

        //        List<string> matchedElements = FindMatchedSegments(maxLenElement.Substring(i),
        //        this.SegmentDictionary.Segments[currentTwoChars]);
        //        string subMaxLenElement = ExstractMaxLenSegment(matchedElements);

        //        foreach (string matchedElement in matchedElements)
        //        {
        //            result.Insert(0,matchedElement);
        //            result.Insert(0,this.Separator);
        //        }

        //        if (subMaxLenElement.Length > 0)
        //        {
        //            result.Insert(0,subMaxLenElement);
        //            result.Insert(0,this.Separator);
        //            i += subMaxLenElement.Length - 1;
        //        }
        //    }

        //    //return AppendMaxLenElement(result, ref currentIndex, maxLenElement);
        //}

        ///// <summary>
        ///// 往结果中追加最大长度的分词。
        ///// </summary>
        ///// <param name="result"></param>
        ///// <param name="currentIndex"></param>
        ///// <param name="maxLenElement"></param>
        ///// <returns></returns>
        //private void AppendMaxLenElement(StringBuilder result, ref int currentIndex, string maxLenElement)
        //{
        //    result.Insert(0,maxLenElement);
        //    result.Insert(0,this.Separator);
        //    currentIndex += maxLenElement.Length - 1;
        //    //return currentIndex;
        //}

        /// <summary>
        /// 处理最后一个字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentIndex"></param>
        /// <param name="result"></param>
        private void ProcessLastChar(string text, int currentIndex, StringBuilder result)
        {
            //如果最后一个字符还没有处理
            if (currentIndex >0)
            {
                result.Insert(0,text.Substring(0,1));
                result.Insert(0,this.Separator);
            }
        }

        ///// <summary>
        ///// 在目标文本中收索出，以当前两格字符开头的词。
        ///// </summary>
        ///// <param name="targetText">目标文本</param>
        ///// <param name="segmentList">以当前两格字符开头的词。</param>
        ///// <returns></returns>
        //private List<string> FindMatchedSegments(string targetText, List<string> segmentList)
        //{
        //    List<string> matchedElements = new List<string>();

        //    foreach (string element in segmentList)
        //    {
        //        if (!targetText.StartsWith(element))
        //        {
        //            continue;
        //        }

        //        if (!matchedElements.Contains(element))
        //        {
        //            matchedElements.Add(element);
        //        }
        //    }

        //    return matchedElements;
        //}

        ///// <summary>
        ///// 从匹配的分词中抽取最大长度的词。
        ///// </summary>
        ///// <param name="targetText"></param>
        ///// <param name="segmentList"></param>
        ///// <param name="maxLenElement"></param>
        ///// <returns></returns>
        //private string ExstractMaxLenSegment(List<string> matchedElements)
        //{
        //    int maxLen = 0;
        //    string maxLenElement = "";

        //    foreach (string element in matchedElements)
        //    {
        //        if (element.Length > maxLen)
        //        {
        //            maxLen = element.Length;
        //            maxLenElement = element;
        //        }
        //    }

        //    matchedElements.Remove(maxLenElement);

        //    return maxLenElement;
        //}
    }
}
