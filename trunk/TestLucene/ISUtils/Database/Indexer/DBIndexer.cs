﻿using System;
using System.Collections.Generic;
using System.Data ;
using Lucene.Net.Analysis;
using ISUtils.Common;
using ISUtils.Database.Link;
using ISUtils.Database.Writer;

namespace ISUtils.Database.Indexer
{
    public class DBIndexer : DbIndexerBase, DataBaseIndexer
    {
        /**/
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string _connectString = "";
        public override string ConnectString
        {
            get { return _connectString; }
        }
        /**/
        /// <summary>
        /// 字符串分析器
        /// </summary>
        private Analyzer analyzer;
        public override Analyzer Analyzer 
        {
            get { return analyzer; }
        }
        /**/
        /// <summary>
        /// 索引存储路径
        /// </summary>
        private string _directory = "";
        public override string Directory 
        {
            get { return _directory; }
        }
        /**/
        /// <summary>
        /// 是否创建增量索引
        /// </summary>
        private bool bCreate=true;
        /**/
        /// <summary>
        /// 是否创建增量索引
        /// </summary>
        private DBTypeEnum dbType = DBTypeEnum.SQL_Server;
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mergeFactor">合并因子 (mergeFactor)</param>
        /// <param name="maxBufferedDocs">文档内存最大存储数</param>
        public DBIndexer(Analyzer analyzer,DBTypeEnum type,string connectString,string directory,bool create)
        {
            this.analyzer = analyzer;
            _connectString = connectString;
            _directory = directory;
            bCreate =create;
        }
        /**/
        /// <summary>
        /// 将数据库查询结果写入索引
        /// </summary>
        /// <param name="strSQL">数据库查询语句</param>
        public override void WriteResults(string strSQL)
        {
            DBLinker linker;
           
            switch (dbType)
            {
                case DBTypeEnum.SQL_Server:
                    linker=new SqlServerLinker(_connectString);
                    break;
                case DBTypeEnum.OLE_DB:
                    linker = new OleDbLinker(_connectString);
                    break;
                case DBTypeEnum.ODBC:
                    linker = new OdbcLinker(_connectString);
                    break;
                case DBTypeEnum.Oracle:
                    linker = new OracleLinker(_connectString);
                    break;
                default:
                    linker = new SqlServerLinker(_connectString);
                    break;
            }
            DataTable dt = linker.ExecuteSQL(strSQL);
            DbWriterBase writer = new DBWriter(analyzer, _directory, bCreate);
            writer.WriteDataTable(dt);
            linker.Close();
        }
        /**/
        /// <summary>
        /// 将数据库查询结果写入索引
        /// </summary>
        /// <param name="strSQL">数据库查询语句</param>
        /// <param name="mergeFactor">合并因子 (mergeFactor)</param>
        /// <param name="maxBufferedDocs">文档内存最大存储数</param>
        public override void WriteResults(string strSQL, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs)
        {
            DBLinker linker;

            switch (dbType)
            {
                case DBTypeEnum.SQL_Server:
                    linker = new SqlServerLinker(_connectString);
                    break;
                case DBTypeEnum.OLE_DB:
                    linker = new OleDbLinker(_connectString);
                    break;
                case DBTypeEnum.ODBC:
                    linker = new OdbcLinker(_connectString);
                    break;
                case DBTypeEnum.Oracle:
                    linker = new OracleLinker(_connectString);
                    break;
                default:
                    linker = new SqlServerLinker(_connectString);
                    break;
            }
            DataTable dt = linker.ExecuteSQL(strSQL);
            DbWriterBase writer = new DBWriter(analyzer, _directory, bCreate);
            writer.SetOptimProperties(mergeFactor, maxBufferedDocs);
            writer.WriteDataTable(dt);
            linker.Close();
        }
        /**/
        /// <summary>
        /// 将数据库查询结果写入索引
        /// </summary>
        /// <param name="strSQL">数据库查询语句</param>
        /// <param name="mergeFactor">合并因子 (mergeFactor)</param>
        /// <param name="maxBufferedDocs">文档内存最大存储数</param>
        public override void WriteResults(string strSQL, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs, ref System.Windows.Forms.ProgressBar progressBar)
        {
            DBLinker linker;

            switch (dbType)
            {
                case DBTypeEnum.SQL_Server:
                    linker = new SqlServerLinker(_connectString);
                    break;
                case DBTypeEnum.OLE_DB:
                    linker = new OleDbLinker(_connectString);
                    break;
                case DBTypeEnum.ODBC:
                    linker = new OdbcLinker(_connectString);
                    break;
                case DBTypeEnum.Oracle:
                    linker = new OracleLinker(_connectString);
                    break;
                default:
                    linker = new SqlServerLinker(_connectString);
                    break;
            }
            DataTable dt = linker.ExecuteSQL(strSQL);
            DbWriterBase writer = new DBWriter(analyzer, _directory, bCreate);
            writer.SetOptimProperties(mergeFactor, maxBufferedDocs);
            writer.WriteDataTable(dt,ref progressBar);
            linker.Close();
        }
        /**/
        /// <summary>
        /// 将数据库查询结果写入索引
        /// </summary>
        /// <param name="strSQL">数据库查询语句</param>
        /// <param name="mergeFactor">合并因子 (mergeFactor)</param>
        /// <param name="maxBufferedDocs">文档内存最大存储数</param>
        public override void WriteResults(string strSQL, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs, ref System.Windows.Forms.ToolStripProgressBar progressBar)
        {
            DBLinker linker;

            switch (dbType)
            {
                case DBTypeEnum.SQL_Server:
                    linker = new SqlServerLinker(_connectString);
                    break;
                case DBTypeEnum.OLE_DB:
                    linker = new OleDbLinker(_connectString);
                    break;
                case DBTypeEnum.ODBC:
                    linker = new OdbcLinker(_connectString);
                    break;
                case DBTypeEnum.Oracle:
                    linker = new OracleLinker(_connectString);
                    break;
                default:
                    linker = new SqlServerLinker(_connectString);
                    break;
            }
            DataTable dt = linker.ExecuteSQL(strSQL);
            DbWriterBase writer = new DBWriter(analyzer, _directory, bCreate);
            writer.SetOptimProperties(mergeFactor, maxBufferedDocs);
            writer.WriteDataTable(dt, ref progressBar);
            linker.Close();
        }
    }
}