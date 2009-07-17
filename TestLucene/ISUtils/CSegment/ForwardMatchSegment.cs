using System;
using System.Collections.Generic;
using System.Text;
using Lwh.ChineseSegment.SegmentDictionary;
using Lwh.ChineseSegment.Utility;

namespace Lwh.ChineseSegment
{
    /// <summary>
    /// 正向最大匹配分词。
    /// </summary>
    public class ForwardMatchSegment : WordSegmentBase
    {
        private ISegmentDictionary _segmentDictionary = new ForwardSegmentDictionary();

        /// <summary>
        /// 
        /// </summary>
        public override ISegmentDictionary SegmentDictionary
        {
            get
            {
                if (_segmentDictionary == null)
                {
                    _segmentDictionary = new ForwardSegmentDictionary();
                }
                return _segmentDictionary;
            }
        }
        #region 分词并返回分词列表
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
            string nextChar;
            string currentTwoChars;
            string maxLenElement="";
            StringBuilder result = new StringBuilder();
            
//#if DEBUG
//            System.Console.WriteLine(sentence);
//            System.Console.WriteLine(result.ToString());
//#endif
            int charCursor = 0;
            for (charCursor = 0; charCursor < sentence.Length - 1; charCursor++)
            {
                currentChar = sentence.Substring(charCursor, 1);
                nextChar = sentence.Substring(charCursor + 1, 1);
                //非汉字
                if (!Character.IsChinese(currentChar))
                {
                    ProcessNonChinese(sentence, ref resultList, result, currentChar, nextChar);
                    //#if DEBUG
                    //                    System.Console.WriteLine(result.ToString());
                    //#endif
                    continue;
                }
                //下一字符非汉字
                if (!Character.IsChinese(nextChar))
                {
                    result.Append(currentChar);
                    resultList.Add(result.ToString());
                    result.Remove(0, result.Length);
                    continue;
                }
                //汉字
                currentTwoChars = sentence.Substring(charCursor, 2);

                //以当前两字符开头的词，在词库中不存在
                if (!this.SegmentDictionary.Segments.ContainsKey(currentTwoChars))
                {
                    //当前字符是姓
                    if (this.NameSegments.ContainsKey(currentChar))
                    {
                        result.Append(currentChar);
                    }
                    else if (this.NameSegments.ContainsKey(currentTwoChars))
                    {
                        result.Append(currentTwoChars);
                    }
                    else
                    {
                        result.Append(currentChar);
                        resultList.Add(result.ToString());
                        result.Remove(0, result.Length);
                    }
//#if DEBUG
//                    System.Console.WriteLine(result.ToString());
//#endif
                    continue;
                }
                //以当前两字符开头的词，在词库中存在,取出所有匹配的词并抽取最大程度的词。
                ProcessExistingElement(sentence, ref resultList, result, currentChar, currentTwoChars, ref charCursor,ref maxLenElement);
            }

            //处理最后一个字符
            ProcessLastChar(sentence, charCursor, ref resultList, result,maxLenElement);
            return resultList;
        }
        /// <summary>
        /// 处理非汉字
        /// </summary>
        /// <param name="text"></param>
        /// <param name="result"></param>
        /// <param name="currentChar"></param>
        /// <param name="currentIndex"></param>
        private void ProcessNonChinese(string text, ref List<string> resultList,StringBuilder result, string currentChar, string nextChar)
        {

            if ((Character.IsLetter(currentChar) && !Character.IsLetter(nextChar)) ||
                Character.IsNumber(currentChar) && Character.IsLetter(nextChar))
            {
                //当前字符是字母，且下一字符不是字母。
                //或者当前是数字，且下一字符是字母
                result.Append(currentChar);
                resultList.Add(result.ToString());
                result.Remove(0,result.Length);
            }
            else
            {
                //当前字符是数字
                result.Append(currentChar);
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
        private void ProcessExistingElement(string text,ref List<string> resultList, StringBuilder result,
            string currentChar, string currentTwoChars, ref int currentIndex,ref string maxLenElement)
        {
            int maxlen = this.SegmentDictionary.SegmentKeys[currentChar][currentTwoChars] > (text.Length - currentIndex) ? (text.Length - currentIndex) : this.SegmentDictionary.SegmentKeys[currentChar][currentTwoChars];
            bool exist = false;
            //找到以当前字符开头存在于词典中的最长词
            for (int i = maxlen; i >= 2; i--)
            {
                string element = text.Substring(currentIndex, i);
                if (this.SegmentDictionary.SegmentDict.ContainsKey(element))
                {
                    maxLenElement = element;
                    exist = true;
                    break;
                }
            }
            if (!exist)//没有找到，不存在以当前字符为首的词
            {
                result.Append(currentChar);
                resultList.Add(result.ToString());
                result.Remove(0, result.Length);
                return;
            }
            if (result.Length>0 && currentIndex > 0)
            {
                result.Append(currentTwoChars);
                resultList.Add(result.ToString());
                result.Remove(0, result.Length);
            }            //找到，分解这个最长词
            string strLeave = maxLenElement;
            bool remove = false;
            int minLen = 2;
            while (strLeave.Length >= 2)
            {
                remove = false;
                for (int j = 2; j <= strLeave.Length; j++)
                {
                    string subElement = strLeave.Substring(0, j);
                    if (this.SegmentDictionary.SegmentDict.ContainsKey(subElement))
                    {
                        result.Append(subElement);
                        resultList.Add(result.ToString());
                        result.Remove(0, result.Length);
                        if (!remove)
                        {
                            remove = true;
                            minLen = subElement.Length;
                        }
                    }
                }
                strLeave = strLeave.Substring(minLen);
            }
            currentIndex += maxLenElement.Length - 1;
        }
        /// <summary>
        /// 处理最后一个字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentIndex"></param>
        /// <param name="result"></param>
        private void ProcessLastChar(string text, int currentIndex,ref List<string> resultList, StringBuilder result,string lastMaxLenElement)
        {
            //如果最后一个字符还没有处理
            if (currentIndex < text.Length && !lastMaxLenElement.EndsWith(text.Substring(text.Length - 1)))
            {
                result.Append(text.Substring(text.Length - 1));
                resultList.Add(result.ToString());
                result.Remove(0, result.Length);
            }
        }
        #endregion
        #region 分词并返回分词字符串
        /// <summary>
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
            string nextChar;
            string currentTwoChars;
            string maxLenElement="";
            int charCursor = 0;
            for (charCursor = 0; charCursor < text.Length - 1; charCursor++)
            {
                currentChar = text.Substring(charCursor, 1);
                nextChar = text.Substring(charCursor + 1, 1);
                //非汉字
                if (!Character.IsChinese(currentChar))
                {
                    ProcessNonChinese(text, result, currentChar,nextChar);
                    continue;
                }
                //下一字符非汉字
                if (!Character.IsChinese(nextChar))
                {
                    result.Append(currentChar);
                    result.Append(this.Separator);
                    continue;
                }
                //汉字
                currentTwoChars = text.Substring(charCursor, 2);
                //以当前两字符开头的词，在词库中不存在
                if (!this.SegmentDictionary.Segments.ContainsKey(currentTwoChars))
                {
                    //当前字符是姓
                    if (this.NameSegments.ContainsKey(currentChar))
                    {
                        result.Append(currentChar);
                    }
                    else if (this.NameSegments.ContainsKey(currentTwoChars))
                    {
                        result.Append(currentTwoChars);
                    }
                    else
                    {
                        result.Append(currentChar);
                        result.Append(this.Separator);
                    }
                    continue;
                }                
                //以当前两字符开头的词，在词库中存在,取出所有匹配的词并抽取最大程度的词。
                ProcessExistingElement(text, result, currentChar, currentTwoChars, ref charCursor,ref maxLenElement);
            }

            //处理最后一个字符
            ProcessLastChar(text, charCursor, result,maxLenElement);
        }
        /// <summary>
        /// 处理非汉字
        /// </summary>
        /// <param name="text"></param>
        /// <param name="result"></param>
        /// <param name="currentChar"></param>
        /// <param name="currentIndex"></param>
        private void ProcessNonChinese(string text, StringBuilder result, string currentChar, string nextChar)
        {

            if ((Character.IsLetter(currentChar) && !Character.IsLetter(nextChar))||
                Character.IsNumber(currentChar ) && Character.IsLetter(nextChar))
            {
                //当前字符是字母，且下一字符不是字母。
                //或者当前是数字，且下一字符是字母
                result.Append(currentChar);
                result.Append(this.Separator);
            }
            else
            {
                //当前字符是数字
                result.Append(currentChar);
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
            string currentChar, string currentTwoChars, ref int currentIndex,ref string maxLenElement)
        {
            int maxlen = this.SegmentDictionary.SegmentKeys[currentChar][currentTwoChars] > (text.Length - currentIndex) ? (text.Length - currentIndex) : this.SegmentDictionary.SegmentKeys[currentChar][currentTwoChars];
            bool exist=false;
            //找到以当前字符开头存在于词典中的最长词
            for (int i = maxlen; i >= 2; i--)
            {
                string element = text.Substring(currentIndex, i);
                if (this.SegmentDictionary.SegmentDict.ContainsKey(element))
                {
                    maxLenElement = element;
                    exist = true;
                    break;
                }
            }
            if (!exist)//没有找到，不存在以当前字符为首的词
            {
                result.Append(currentChar);
                result.Append(this.Separator);
                return;
            }
            if (!result.ToString().EndsWith(this.Separator) && currentIndex > 0)
            {
                result.Append(currentTwoChars);
                result.Append(this.Separator);
            }            //找到，分解这个最长词
            string strLeave = maxLenElement;
            bool remove = false;
            int minLen = 2;
            while (strLeave.Length >= 2)
            {
                remove = false;
                for (int j = 2; j <= strLeave.Length; j++)
                {
                    string subElement = strLeave.Substring(0, j);
                    if (this.SegmentDictionary.SegmentDict.ContainsKey(subElement))
                    {
                        result.Append(subElement);
                        result.Append(this.Separator);
                        if (!remove)
                        {
                            remove = true;
                            minLen = subElement.Length;                           
                        }
                    }
                }
                strLeave = strLeave.Substring(minLen);
            }
            currentIndex += maxLenElement.Length - 1;
        }
        /// <summary>
        /// 处理最后一个字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentIndex"></param>
        /// <param name="result"></param>
        private void ProcessLastChar(string text, int currentIndex, StringBuilder result, string lastMaxLenElement)
        {
            //如果最后一个字符还没有处理
            if (currentIndex < text.Length && !lastMaxLenElement.EndsWith(text.Substring(text.Length - 1)))
            {
                result.Append(text.Substring(text.Length - 1));
                result.Append(this.Separator);
            }
        }
        #endregion
        #region 分词返回分词表和位移列表
        /// <summary>
        /// 对单个句子分词。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected override List<string> SegmentSentence(string sentence, out List<int> startList)
        {
            startList = new List<int>();
            if (string.IsNullOrEmpty(sentence))
            {
                return new List<string>();
            }

            if (this.SegmentDictionary.Segments.Count <= 0)
            {
                return new List<string>();
            }

            string currentChar;
            string nextChar;
            string currentTwoChars;
            string maxLenElement = "";
            int charCursor = 0;
            AuxiliaryString auxString = new AuxiliaryString();
            for (charCursor = 0; charCursor < sentence.Length - 1; charCursor++)
            {
                currentChar = sentence.Substring(charCursor, 1);
                nextChar = sentence.Substring(charCursor + 1, 1);
                //非汉字
                if (!Character.IsChinese(currentChar))
                {
                    ProcessNonChinese(sentence, auxString, currentChar, nextChar, charCursor);
                    continue;
                }
                //下一字符非汉字
                if (!Character.IsChinese(nextChar))
                {
                    auxString.Append(currentChar,charCursor);
                    auxString.Append(this.Separator);
                    continue;
                }
                //汉字
                currentTwoChars = sentence.Substring(charCursor, 2);
                //以当前两字符开头的词，在词库中不存在
                if (!this.SegmentDictionary.Segments.ContainsKey(currentTwoChars))
                {
                    //当前字符是姓
                    if (this.NameSegments.ContainsKey(currentChar))
                    {
                        auxString.Append(currentChar,charCursor);
                    }
                    else if (this.NameSegments.ContainsKey(currentTwoChars))
                    {
                        auxString.Append(currentTwoChars,charCursor);
                    }
                    else
                    {
                        auxString.Append(currentChar,charCursor);
                        auxString.Append(this.Separator);
                    }
                    continue;
                }
                //以当前两字符开头的词，在词库中存在,取出所有匹配的词并抽取最大程度的词。
                ProcessExistingElement(sentence, auxString, currentChar, currentTwoChars, ref charCursor, ref maxLenElement);
            }

            //处理最后一个字符
            ProcessLastChar(sentence, charCursor, auxString, maxLenElement);
            startList = auxString.StartList;
            return auxString.Tokens;
        }
        #endregion
        #region 分词返回分词字符串和位移列表
        /// <summary>
        /// 对单个句子分词。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="result"></param>
        protected override void SegmentSentence(string text, ref StringBuilder result, out List<int> startList)
        {
            startList = new List<int>();
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (this.SegmentDictionary.Segments.Count <= 0)
            {
                return;
            }

            string currentChar;
            string nextChar;
            string currentTwoChars;
            string maxLenElement = "";
            int charCursor = 0;
            AuxiliaryString auxString = new AuxiliaryString();
            for (charCursor = 0; charCursor < text.Length - 1; charCursor++)
            {
                currentChar = text.Substring(charCursor, 1);
                nextChar = text.Substring(charCursor + 1, 1);
                //非汉字
                if (!Character.IsChinese(currentChar))
                {
                    ProcessNonChinese(text, auxString, currentChar, nextChar, charCursor);
                    continue;
                }
                //下一字符非汉字
                if (!Character.IsChinese(nextChar))
                {
                    auxString.Append(currentChar,charCursor);
                    auxString.Append(this.Separator);
                    continue;
                }
                //汉字
                currentTwoChars = text.Substring(charCursor, 2);
                //以当前两字符开头的词，在词库中不存在
                if (!this.SegmentDictionary.Segments.ContainsKey(currentTwoChars))
                {
                    //当前字符是姓
                    if (this.NameSegments.ContainsKey(currentChar))
                    {
                        auxString.Append(currentChar,charCursor);
                    }
                    else if (this.NameSegments.ContainsKey(currentTwoChars))
                    {
                        auxString.Append(currentTwoChars,charCursor);
                    }
                    else
                    {
                        auxString.Append(currentChar,charCursor);
                        auxString.Append(this.Separator);
                    }
                    continue;
                }
                //以当前两字符开头的词，在词库中存在,取出所有匹配的词并抽取最大程度的词。
                ProcessExistingElement(text, auxString, currentChar, currentTwoChars, ref charCursor, ref maxLenElement);
            }

            //处理最后一个字符
            ProcessLastChar(text, charCursor, auxString, maxLenElement);
            result = auxString.Builder;
            startList = auxString.StartList;
        }
        /// <summary>
        /// 处理非汉字
        /// </summary>
        /// <param name="text"></param>
        /// <param name="result"></param>
        /// <param name="currentChar"></param>
        /// <param name="currentIndex"></param>
        private void ProcessNonChinese(string text, AuxiliaryString auxString, string currentChar, string nextChar, int currentIndex)
        {
            if (currentChar == " ")
            {
                //当前是空格
            }
            else
            {
                if (nextChar == " ")
                {
                    auxString.Append(currentChar, currentIndex);
                    auxString.Append(this.Separator);
                }
                else
                {
                    if ((Character.IsLetter(currentChar) && !Character.IsLetter(nextChar)) ||
                        Character.IsNumber(currentChar) && Character.IsLetter(nextChar))
                    {
                        //当前字符是字母，且下一字符不是字母。
                        //或者当前是数字，且下一字符是字母
                        auxString.Append(currentChar, currentIndex);
                        auxString.Append(this.Separator);
                    }
                    else
                    {
                        //当前字符是数字
                        auxString.Append(currentChar, currentIndex);
                    }
                }
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
        private void ProcessExistingElement(string text, AuxiliaryString auxString, 
            string currentChar, string currentTwoChars, ref int currentIndex, ref string maxLenElement)
        {
            int maxlen = this.SegmentDictionary.SegmentKeys[currentChar][currentTwoChars] > (text.Length - currentIndex) ? (text.Length - currentIndex) : this.SegmentDictionary.SegmentKeys[currentChar][currentTwoChars];
            bool exist = false;
            //找到以当前字符开头存在于词典中的最长词
            for (int i = maxlen; i >= 2; i--)
            {
                string element = text.Substring(currentIndex, i);
                if (this.SegmentDictionary.SegmentDict.ContainsKey(element))
                {
                    maxLenElement = element;
                    exist = true;
                    break;
                }
            }
            if (!exist)//没有找到，不存在以当前字符为首的词
            {
                auxString.Append(currentChar, currentIndex);
                auxString.Append(this.Separator);
                return;
            }
            if (!auxString.NeedAddPos && currentIndex > 0)
            {
                auxString.Append(currentTwoChars, currentIndex);
                auxString.Append(this.Separator);
            }            //找到，分解这个最长词
            string strLeave = maxLenElement;
            bool remove = false;
            int minLen = 2;
            int startpos = currentIndex;
            while (strLeave.Length >= 2)
            {
                remove = false;
                for (int j = 2; j <= strLeave.Length; j++)
                {
                    string subElement = strLeave.Substring(0, j);
                    if (this.SegmentDictionary.SegmentDict.ContainsKey(subElement))
                    {
                        auxString.Append(subElement,startpos);
                        auxString.Append(this.Separator);
                        if (!remove)
                        {
                            remove = true;
                            minLen = subElement.Length;
                        }
                    }
                }
                strLeave = strLeave.Substring(minLen);
                startpos += minLen;
            }
            currentIndex += maxLenElement.Length - 1;
        }
        /// <summary>
        /// 处理最后一个字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentIndex"></param>
        /// <param name="result"></param>
        private void ProcessLastChar(string text, int currentIndex, AuxiliaryString auxString, string lastMaxLenElement)
        {
            //如果最后一个字符还没有处理
            if (currentIndex < text.Length && !lastMaxLenElement.EndsWith(text.Substring(text.Length - 1)))
            {
                auxString.Append(text.Substring(text.Length - 1),currentIndex);
                auxString.Append(this.Separator);
            }
        }
        #endregion
    }
}
