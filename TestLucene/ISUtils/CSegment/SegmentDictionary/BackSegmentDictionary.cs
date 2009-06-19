using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.CSegment.SegmentDictionary
{
    internal class BackSegmentDictionary : SegmentDictionaryBase
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
                //分词的最后1个字符
                string lastChar = segment.Substring(segment.Length-1);
                //分词的最后2个字符
                string endTwoChars;
                if (segment.Length >= 2)
                    endTwoChars = segment.Substring(segment.Length - 2);
                else
                    endTwoChars = lastChar;
                base.AddSegment(segment, lastChar, endTwoChars);
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
                //分词的最后一个字符
                string lastChar = segment.Substring(segment.Length - 1);
                //分词的最后2个字符
                string endTwoChars;
                if (segment.Length >= 2)
                    endTwoChars = segment.Substring(segment.Length - 2);
                else
                    endTwoChars = lastChar;
                base.AddSegment(segment, lastChar, endTwoChars);
            }
            return true;
        }
    }
}
