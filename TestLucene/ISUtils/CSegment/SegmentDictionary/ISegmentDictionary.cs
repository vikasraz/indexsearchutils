using System;
using System.Collections.Generic;
using System.Text;

namespace Lwh.ChineseSegment.SegmentDictionary
{
    /// <summary>
    /// 分词字典。
    /// </summary>
    public interface ISegmentDictionary
    {
        /// <summary>
        /// 分词关键字。
        /// </summary>
        Dictionary<string, Dictionary<string, int>> SegmentKeys
        {
            get;
        }

        /// <summary>
        /// 分词字典。
        /// </summary>
        Dictionary<string, Dictionary<int, List<string>>> Segments
        {
            get;
        }
        /// <summary>
        /// 分词字典。
        /// </summary>
        Dictionary<string, int> SegmentDict
        {
            get;
        }

        /// <summary>
        /// 分词链表。
        /// </summary>
        List<string> SegmentList
        {
            get;
        }

        int MaxSegmentLength
        {
            get;
        }
        /// <summary>
        /// 将分词链表解析到分词字典中。
        /// </summary>
        /// <param name="segmentList"></param>
        /// <returns></returns>
        bool Parse(List<string> segmentList);

        /// <summary>
        /// 将分词链表追加到分词字典中。
        /// </summary>
        /// <param name="segmentList"></param>
        /// <returns></returns>
        bool Append(List<string> segmentList);
#if DEBUG
        void Output();
#endif
    }
}
