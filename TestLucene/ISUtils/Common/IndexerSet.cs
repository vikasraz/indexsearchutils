using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    /*
     *   <Indexer>
     *    <MainIndexTime>23:45</MainIndexTime>
     *    <MainIndexSpan>1</MainIndexSpan>
     *    <IncrIndexSpan>120</IncrIndexSpan>
     *    <RamBufferSize>512</RamBufferSize>
     *    <MaxBufferedDocs>10000</MaxBufferedDocs>
     *    <MergeFactor>15000</MergeFactor>   
     *  </Indexer>
     * */
    [Serializable]
    public class IndexerSet
    {
        #region Flags
        /**/
        /// <summary>
        /// 字段最大长度
        /// </summary>
        public const string MaxFieldLengthFlag = "MAX_FIELD_LENGTH";
        /**/
        /// <summary>
        /// 内存最大使用量
        /// </summary>
        public const string RamBufferSizeFlag = "RAM_BUFFER_SIZE";
        /**/
        /// <summary>
        /// 主索引重建时间的标志
        /// </summary>
        public const string MainIndexReCreateTimeFlag = "MAIN_CREATE_TIME";
        /**/
        /// <summary>
        /// 主索引重建间隔的标志
        /// </summary>
        public const string MainIndexReCreateSpanFlag = "MAIN_TIME_SPAN";
        /**/
        /// <summary>
        /// 增量索引重建间隔的标志
        /// </summary>
        public const string IncrIndexReCreateSpanFlag = "INCR_TIME_SPAN";
        /**/
        /// <summary>
        /// 合并因子的标志
        /// </summary>
        public const string IndexerMergeFactorFlag = "MERGE_FACTOR";
        /**/
        /// <summary>
        ///文档内存最大存储数的标志
        /// </summary>
        public const string IndexerMaxBufferedDocsFlag = "MAX_BUFFERED_DOCS";
        #endregion
        #region Property
        /**/
        /// <summary>
        /// 存储主索引重建时间
        /// </summary>
        private double rambuffersize = 512;
        /**/
        /// <summary>
        /// 设定或返回主索引重建时间
        /// </summary>
        public double RamBufferSize
        {
            get { return rambuffersize; }
            set { rambuffersize = value; }
        }
        /**/
        /// <summary>
        /// 存储主索引重建时间
        /// </summary>
        private DateTime maincreate = DateTime.Parse("23:30:00");
        /**/
        /// <summary>
        /// 设定或返回主索引重建时间
        /// </summary>
        public DateTime MainIndexReCreateTime
        {
            get { return maincreate; }
            set { maincreate = value; }
        }
        /**/
        /// <summary>
        /// 存储主索引重建间隔
        /// </summary>
        private int maintmspan = 1;//1 day
        /**/
        /// <summary>
        /// 设定或返回主索引重建间隔
        /// </summary>
        public int MainIndexReCreateTimeSpan
        {
            get { return maintmspan; }
            set { maintmspan = value; }
        }
        /**/
        /// <summary>
        /// 存储合并因子
        /// </summary>
        private int mergeFactor = 10000;
        /**/
        /// <summary>
        /// 设定或返回合并因子
        /// </summary>
        public int MergeFactor
        {
            get { return mergeFactor; }
            set { mergeFactor = value; }
        }
        /**/
        /// <summary>
        /// 存储文档内存最大存储数
        /// </summary>
        private int maxBufferedDocs = 10000;
        /**/
        /// <summary>
        /// 设定或返回文档内存最大存储数
        /// </summary>
        public int MaxBufferedDocs
        {
            get { return maxBufferedDocs; }
            set { maxBufferedDocs = value; }
        }
        #endregion
    }
}
