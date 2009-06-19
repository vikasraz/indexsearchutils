using System;
using System.Collections.Generic;
using System.Text;
using ISUtils.CSegment.SegmentDictionary;
using ISUtils.CSegment.DictionaryLoader;

namespace ISUtils.CSegment
{
    /// <summary>
    /// 中文分词算法。
    /// </summary>
    public interface IWordSegment
    {
        /// <summary>
        /// 分词结果字符串分隔符。
        /// </summary>
        string Separator
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        ISegmentDictionary SegmentDictionary
        {
            get;
        }

        /// <summary>
        /// 加载中文字库。
        /// </summary>
        /// <param name="dictionaryLoader"></param>
        /// <param name="dictionaryPath"></param>
        /// <returns></returns>
        bool LoadDictionary(IDictionaryLoader dictionaryLoader, string dictionaryPath);

        /// <summary>
        /// 追加中文字库。
        /// </summary>
        /// <param name="dictionaryLoader"></param>
        /// <param name="dictionaryPath"></param>
        /// <returns></returns>
        bool AppendDictionary(IDictionaryLoader dictionaryLoader, string dictionaryPath);

        /// <summary>
        /// 加载中文姓名字库。
        /// </summary>
        /// <param name="dictionaryLoader"></param>
        /// <param name="dictionaryPath"></param>
        /// <returns></returns>
        bool LoadNameDictionary(IDictionaryLoader dictionaryLoader, string dictionaryPath);

        /// <summary>
        /// 加载中文数字字库。
        /// </summary>
        /// <param name="dictionaryLoader"></param>
        /// <param name="dictionaryPath"></param>
        /// <returns></returns>
        bool LoadNumberDictionary(IDictionaryLoader dictionaryLoader, string dictionaryPath);

        /// <summary>
        /// 加载中文数字字库。
        /// </summary>
        /// <param name="dictionaryLoader"></param>
        /// <param name="dictionaryPath"></param>
        /// <returns></returns>
        bool LoadFilterDictionary(IDictionaryLoader dictionaryLoader, string dictionaryPath);
        /// <summary>
        /// 分词。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string Segment(string text);
        /// <summary>
        /// 分词。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        List<string> SegmentEx(string text);
    }
}
