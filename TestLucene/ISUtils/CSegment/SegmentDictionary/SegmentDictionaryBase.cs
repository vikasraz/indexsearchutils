using System;
using System.Collections.Generic;
using System.Text;

namespace Lwh.ChineseSegment.SegmentDictionary
{
    internal abstract class SegmentDictionaryBase : ISegmentDictionary
    {
        #region ISegmentDictionary 成员

        //private Dictionary<string, List<string>> _segments = new Dictionary<string, List<string>>();
        private List<string> _baseSegmentList = new List<string>();
        //Dictionary<CHAR,Dictionary<CHAR[2],MAXLEN>>
        private Dictionary<string, Dictionary<string,int>> _segmentKeys = new Dictionary<string, Dictionary<string,int>>();
        //Dictionary<CHAR[2],DICTIONARY<INT,List<ALL>>>
        private Dictionary<string, Dictionary<int, List<string>>> _segments = new Dictionary<string, Dictionary<int, List<string>>>();
        //Dictionary<ALL,LEN>
        private Dictionary<string, int> _baseSegmentDict = new Dictionary<string, int>();
        private int maxLength = 0;
        public int MaxSegmentLength
        {
            get { return maxLength; }
        }
        /// <summary>
        /// 分词关键字。
        /// </summary>
        public Dictionary<string, Dictionary<string, int>> SegmentKeys
        {
            get
            {
                if (_segmentKeys == null)
                    _segmentKeys = new Dictionary<string, Dictionary<string, int>>();
                return _segmentKeys;
            }
        }

        /// <summary>
        /// 分词字典。
        /// </summary>
        public Dictionary<string, Dictionary<int, List<string>>> Segments
        {
            get
            {
                if (_segments == null)
                    _segments = new Dictionary<string, Dictionary<int, List<string>>>();
                return _segments;
            }
        }
        /// <summary>
        /// 分词字典。
        /// </summary>
        public Dictionary<string, int> SegmentDict
        {
            get
            {
                if (_baseSegmentDict == null)
                    _baseSegmentDict = new Dictionary<string, int>();
                return _baseSegmentDict;
            }
        }

        /// <summary>
        /// 分词链表。
        /// </summary>
        public List<string> SegmentList
        {
            get
            {
                if (_baseSegmentList == null)
                    _baseSegmentList = new List<string>();
                return _baseSegmentList;
            }
        }
        /// <summary>
        /// 将分词链表解析到分词字典中。
        /// </summary>
        /// <param name="segmentList"></param>
        /// <returns></returns>
        public abstract bool Parse(List<string> segmentList);
       
        /// <summary>
        /// 将分词链表追加到分词字典中。
        /// </summary>
        /// <param name="segmentList"></param>
        /// <returns></returns>
        public abstract bool Append(List<string> segmentList);
        #endregion
        /// <summary>
        /// 从字典中抽取最大长度的词列表。
        /// </summary>
        /// <param name="twoChars">匹配字符串</param>
        /// <returns></returns>
        private List<string> ExstractMaxLenSegment(string twoChars)
        {
            return _segments[twoChars][_segmentKeys[twoChars.Substring(0,1)][twoChars]];
        }
        public bool IsInDictionary(string word)
        {
            return _baseSegmentDict.ContainsKey(word);
        }
        public Dictionary<int, List<string>> FindMatchedSegments(string twoChars)
        {
            return _segments[twoChars];
        }
        /// <summary>
        /// 清空分词字典。
        /// </summary>
        protected void Clear()
        {
            _baseSegmentDict.Clear();
            _baseSegmentList.Clear();
            _segmentKeys.Clear();
            _segments.Clear();
            maxLength = 0;
        }
        /// <summary>
        /// 添加分词。
        /// </summary>
        /// <param name="segment">分词</param>
        protected void AddSegment(string segment, string oneChar, string twoChars)
        {
            //分词的长度
            int length = segment.Length;
            if (_baseSegmentDict.ContainsKey(segment) == false)
            {
                //更新词库中词长
                if (length > maxLength)
                    maxLength = length;
                //将分词加入分词列表
                _baseSegmentList.Add(segment);
                //将分词加入分词字典
                if(!_baseSegmentDict.ContainsKey(segment))
                    _baseSegmentDict.Add(segment, length);
                Dictionary<string, int> keyValueDict = new Dictionary<string, int>();
                int maxLen = 0;
                if (_segmentKeys.ContainsKey(oneChar))
                {
                    keyValueDict = _segmentKeys[oneChar];
                    if (keyValueDict.ContainsKey(twoChars))
                        maxLen = keyValueDict[twoChars];
                    if (maxLen < length)
                        maxLen = length;
                    keyValueDict.Remove(twoChars);
                    keyValueDict.Add(twoChars, maxLen);
                    _segmentKeys.Remove(oneChar);
                    _segmentKeys.Add(oneChar, keyValueDict);
                }
                else
                {
                    _segmentKeys.Remove(oneChar);
                    keyValueDict.Add(twoChars, length);
                    _segmentKeys.Add(oneChar, keyValueDict);
                }
                Dictionary<int, List<string>> segmentValue = new Dictionary<int, List<string>>();
                List<string> slist = new List<string>();
                //Dictionary<CHAR[2],DICTIONARY<INT,List<ALL>>>
                if (_segments.ContainsKey(twoChars))
                {
                    segmentValue = _segments[twoChars];
                    if(segmentValue.ContainsKey(length))
                      slist=segmentValue[length];
                    segmentValue.Remove(length);
                }
                slist.Add(segment);
                segmentValue.Add(length, slist);
                _segments.Remove(twoChars);
                _segments.Add(twoChars, segmentValue);
            }
        }
#if DEBUG
        public void Output()
        {
            System.Console.WriteLine("SegmentList:");
            foreach (string s in _baseSegmentList)
            {
                System.Console.WriteLine(s);
            }
            System.Console.WriteLine("SegmentDict:");
            foreach (string dictKey in _baseSegmentDict.Keys)
            {
                System.Console.WriteLine(string.Format("Key({0}),value({1})",dictKey,_baseSegmentDict[dictKey]));
            }
            System.Console.WriteLine("SegmentKeys:");
            foreach (string firstChar in _segmentKeys.Keys)
            {
                System.Console.WriteLine(firstChar);
                foreach (string twoChars in _segmentKeys[firstChar].Keys)
                {
                    System.Console.WriteLine("\t" + twoChars + "\t" + _segmentKeys[firstChar][twoChars].ToString());
                }
            }
            System.Console.WriteLine("Segments:");
            foreach(string startTwoChars in _segments.Keys)
            {
                System.Console.WriteLine("\t"+startTwoChars+":");
                foreach(int len in _segments[startTwoChars].Keys)
                {
                    System.Console.WriteLine("\t\tlen="+len.ToString());
                    foreach(string value in _segments[startTwoChars][len])
                    {
                        System.Console.WriteLine("\t\t\t"+value);
                    }
                }
            }
        }
#endif
    }
}
