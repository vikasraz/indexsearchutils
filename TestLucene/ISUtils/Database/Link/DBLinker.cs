using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;

namespace ISUtils.Database.Link
{
    public abstract class DBLinker : DataBaseLinker
    {
        /**/
        /// <summary>
        /// 与数据库建立连接
        /// </summary>
        /// <param name="connectString">数据库连接字符串</param>
        /// <returns>返回类型</returns>
        public abstract bool Connect(string connectString);
        /**/
        /// <summary>
        /// 与数据库建立连接
        /// </summary>
        /// <param name="connectStrs">数据库连接参数列表</param>
        /// <returns>返回类型</returns>
        public abstract bool Connect(params string[] connectStrs);
        /**/
        /// <summary>
        /// 执行数据库查询语句
        /// </summary>
        /// <param name="strSQL">结构化数据库查询语句</param>
        /// <returns>返回类型</returns>
        public abstract DataTable ExecuteSQL(string strSQL);
        /**/
        /// <summary>
        /// 执行数据库查询语句
        /// </summary>
        /// <param name="sql">结构化数据库查询语句</param>
        public abstract void ExecSQL(string sql);
        /**/
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public abstract void Close();
    }
}
