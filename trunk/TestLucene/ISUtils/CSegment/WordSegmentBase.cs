using System;
using System.Collections.Generic;
using System.Text;
using ISUtils.CSegment.SegmentDictionary;
using ISUtils.CSegment.DictionaryLoader;
using ISUtils.CSegment.Utility;

namespace ISUtils.CSegment
{
    /// <summary>
    /// 中文分词算法。
    /// </summary>
    public abstract class WordSegmentBase : IWordSegment
    {
        #region IWordSegment 成员

        private string _separator;
        private Dictionary<string,int> _numberSegments;
        private Dictionary<string,int> _nameSegments;
        private List<string> _filterList;
        /// <summary>
        /// 分词结果字符串分隔符。
        /// </summary>
        public string Separator
        {
            get
            {
                if (string.IsNullOrEmpty(_separator))
                {
                    _separator = "/";
                }
                return _separator;
            }
            set
            {
                _separator = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract ISegmentDictionary SegmentDictionary
        {
            get;
        }

     
        /// <summary>
        /// 数字分词链表
        /// </summary>
        protected Dictionary<string, int> NumberSegments
        {
            get
            {
                return _numberSegments;
            }
        }


        /// <summary>
        /// 姓名分词链表
        /// </summary>
        protected Dictionary<string, int> NameSegments
        {
            get
            {
                return _nameSegments;
            }
        }

        protected List<string> FilterList
        {
            get
            {
                return _filterList;
            }
        }
        private Dictionary<string, int> ParseList(List<string> list)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (string s in list)
            {
                if (!dict.ContainsKey(s))
                    dict.Add(s, s.Length);
            }
            return dict;
        }
        /// <summary>
        /// 分词。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Segment(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            string[] sentences = StrUtility.SplitSentences(text,_filterList,false);
            StringBuilder result = new StringBuilder();
//#if DEBUG
//            foreach (string s in sentences)
//            {
//                System.Console.WriteLine(s);
//            }
//#endif
            foreach (string sentence in sentences)
            {
                
                SegmentSentence(sentence.Trim(), ref result);
            }

            return result.ToString();
        }
        public string Segment(string text, out List<int> startList)
        {
            if (string.IsNullOrEmpty(text))
            {
                startList = null;
                return "";
            }
            string sentence = StrUtility.Filter(text, _filterList, false);
            StringBuilder result = new StringBuilder();
            SegmentSentence(sentence, ref result, out startList);
            return result.ToString();
        }
        /// <summary>
        /// 分词。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<string> SegmentEx(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new List<string>();
            }

            string[] sentences = StrUtility.SplitSentences(text, _filterList, false);
            List<string> resultList = new List<string>();
            foreach (string sentence in sentences)
            {
                resultList.AddRange(SegmentSentence(sentence.Trim()));
            }

            return resultList;
        }

        public List<string> SegmentEx(string text, out List<int> startList)
        {
            startList = null;
            if (string.IsNullOrEmpty(text))
            {
                return new List<string>();
            }

            string sentence = StrUtility.Filter(text, _filterList, false);
            List<string> resultList = new List<string>();
            resultList.AddRange(SegmentSentence(sentence,out startList));
            return resultList;
        }
        /// <summary>
        /// 对单个句子分词。
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="result"></param>
        protected abstract void SegmentSentence(string sentence, ref StringBuilder result);
        /// <summary>
        /// 对单个句子分词。
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="result"></param>
        protected abstract void SegmentSentence(string sentence, ref StringBuilder result,out List<int> startList);
        /// <summary>
        /// 对单个句子分词。
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="result"></param>
        protected abstract List<string> SegmentSentence(string sentence);
        /// <summary>
        /// 对单个句子分词。
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="result"></param>
        protected abstract List<string> SegmentSentence(string sentence,out List<int> startList);

        #endregion

        #region IWordSegment 成员


        public bool LoadDictionary(IDictionaryLoader dictionaryLoader, string dictionaryPath)
        {
            if (dictionaryLoader == null)
            {
                return false;
            }

            return this.SegmentDictionary.Parse(dictionaryLoader.Load(dictionaryPath));
        }

        public bool AppendDictionary(IDictionaryLoader dictionaryLoader, string dictionaryPath)
        {
            if (dictionaryLoader == null)
            {
                return false;
            }

            //将分词链表存储到字典中
            return this.SegmentDictionary.Append(dictionaryLoader.Load(dictionaryPath));
        }

        public bool LoadNameDictionary(IDictionaryLoader dictionaryLoader, string dictionaryPath)
        {
            if (dictionaryLoader == null)
            {
                return false;
            }
            

            this._nameSegments = ParseList( dictionaryLoader.Load(dictionaryPath));
            return this._nameSegments != null && this._nameSegments.Count > 0;
        }

        public bool LoadNumberDictionary(IDictionaryLoader dictionaryLoader, string dictionaryPath)
        {
            if (dictionaryLoader == null)
            {
                return false;
            }

            this._numberSegments = ParseList(dictionaryLoader.Load(dictionaryPath));
            return this._numberSegments != null && this._numberSegments.Count > 0;
        }

        public bool LoadFilterDictionary(IDictionaryLoader dictionaryLoader, string dictionaryPath)
        {
            if (dictionaryLoader == null)
            {
                return false;
            }

            this._filterList = dictionaryLoader.Load(dictionaryPath);
            return this._filterList != null && this._filterList.Count > 0;
        }
        #endregion
    }
}
