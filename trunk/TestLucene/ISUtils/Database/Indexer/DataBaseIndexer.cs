using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;

namespace ISUtils.Database.Indexer
{
    public interface DataBaseIndexer
    {
        /**/
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        string ConnectString { get;}
        /**/
        /// <summary>
        /// 字符串分析器
        /// </summary>
        Analyzer Analyzer { get;}
        /**/
        /// <summary>
        /// 索引存储路径
        /// </summary>
        string Directory { get;}
        /**/
        /// <summary>
        /// 将数据库查询结果写入索引
        /// </summary>
        /// <param name="strSQL">数据库查询语句</param>
        void WriteResults(string strSQL);
    }
}
