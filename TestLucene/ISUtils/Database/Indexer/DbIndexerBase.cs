using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace ISUtils.Database.Indexer
{
    public abstract class DbIndexerBase : DataBaseIndexer
    {
        /**/
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public abstract string ConnectString { get; }
        /**/
        /// <summary>
        /// 字符串分析器
        /// </summary>
        public abstract Analyzer Analyzer { get; }
        /**/
        /// <summary>
        /// 索引存储路径
        /// </summary>
        public abstract string Directory { get; }
        /**/
        /// <summary>
        /// 将数据库查询结果写入索引
        /// </summary>
        /// <param name="strSQL">数据库查询语句</param>
        public abstract void WriteResults(string strSQL);
        /**/
        /// <summary>
        /// 将数据库查询结果写入索引
        /// </summary>
        /// <param name="strSQL">数据库查询语句</param>
        /// <param name="mergeFactor">合并因子 (mergeFactor)</param>
        /// <param name="maxBufferedDocs">文档内存最大存储数</param>
        public abstract void WriteResults(string strSQL, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs);
        /**/
        /// <summary>
        /// 将数据库查询结果写入索引
        /// </summary>
        /// <param name="strSQL">数据库查询语句</param>
        /// <param name="mergeFactor">合并因子 (mergeFactor)</param>
        /// <param name="maxBufferedDocs">文档内存最大存储数</param>
        public abstract void WriteResults(string strSQL, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs,ref System.Windows.Forms.ToolStripProgressBar progressBar);
        /**/
        /// <summary>
        /// 将数据库查询结果写入索引
        /// </summary>
        /// <param name="strSQL">数据库查询语句</param>
        /// <param name="mergeFactor">合并因子 (mergeFactor)</param>
        /// <param name="maxBufferedDocs">文档内存最大存储数</param>
        public abstract void WriteResults(string strSQL, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs,ref System.Windows.Forms.ProgressBar progressBar);
    }
}
