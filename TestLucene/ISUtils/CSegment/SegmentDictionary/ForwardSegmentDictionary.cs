using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.CSegment.SegmentDictionary
{
    internal class ForwardSegmentDictionary : SegmentDictionaryBase
    {
       
        /// <summary>
        /// 将分词链表解析到分词字典中。
        /// </summary>
        /// <param name="dictionaryPath"></param>
        /// <param name="loader"></param>
        /// <returns></returns>
        public override bool Parse(List<string> segmentList)
        {
            if (segmentList == null || segmentList.Count <= 0)
            {
                return false;
            }
            base.Clear();
            foreach (string segment in segmentList)
            {
                //分词的第一个字符
                string firstChar = segment.Substring(0, 1);
                //分词的前2个字符
                string startTwoChars;
                if (segment.Length >= 2)
                    startTwoChars = segment.Substring(0, 2);
                else
                    startTwoChars = firstChar;
                base.AddSegment(segment, firstChar, startTwoChars);
            }
            return true;
        }

        /// <summary>
        /// 将分词链表追加到分词字典中。
        /// </summary>
        /// <param name="dictionaryPath"></param>
        /// <param name="loader"></param>
        /// <returns></returns>
        public override bool Append(List<string> segmentList)
        {
            if (segmentList == null || segmentList.Count <= 0)
            {
                return false;
            }

            foreach (string segment in segmentList)
            {
                //分词的第一个字符
                string firstChar = segment.Substring(0, 1);
                //分词的前2个字符
                string startTwoChars;
                if (segment.Length >= 2)
                    startTwoChars = segment.Substring(0, 2);
                else
                    startTwoChars = firstChar;
                base.AddSegment(segment, firstChar, startTwoChars);
            }
            return true;
        }
    }
}
