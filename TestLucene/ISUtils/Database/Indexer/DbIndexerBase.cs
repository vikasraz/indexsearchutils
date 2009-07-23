using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using ISUtils.Async;

namespace ISUtils.Database.Indexer
{
    public abstract class DbIndexerBase : DataBaseIndexer
    {
        #region EventHandler
        public event IndexCompletedEventHandler OnIndexCompleted;
        public event IndexProgressChangedEventHandler OnProgressChanged;
        #endregion
        #region Property
        protected bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
        }
        protected string primaryKey = "";
        public string PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }
        }
        protected string dbName = "";
        public string DbName
        {
            get { return dbName; }
            set { dbName = value; }
        }
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
        #endregion
        #region Event Func
        protected virtual void OnIndexCompletedEvent(object sender, IndexCompletedEventArgs e)
        {
            if (OnIndexCompleted != null)
                OnIndexCompleted(sender, e);
        }
        protected virtual void OnProgressChangedEvent(object sender, IndexProgressChangedEventArgs e)
        {
            if (OnProgressChanged != null)
                OnProgressChanged(sender, e);
        }
        #endregion
        #region Index Func
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
        public abstract void WriteResultsWithEvent(string strSQL, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs);
        /**/
        /// <summary>
        /// 将数据库查询结果写入索引
        /// </summary>
        /// <param name="strSQL">数据库查询语句</param>
        /// <param name="mergeFactor">合并因子 (mergeFactor)</param>
        /// <param name="maxBufferedDocs">文档内存最大存储数</param>
        public abstract void WriteResults(string strSQL, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs,Dictionary<string,float> fieldBoostDict);
        /**/
        /// <summary>
        /// 将数据库查询结果写入索引
        /// </summary>
        /// <param name="strSQL">数据库查询语句</param>
        /// <param name="mergeFactor">合并因子 (mergeFactor)</param>
        /// <param name="maxBufferedDocs">文档内存最大存储数</param>
        public abstract void WriteResultsWithEvent(string strSQL, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs, Dictionary<string, float> fieldBoostDict);
        #endregion
    }
}
