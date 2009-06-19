using System;
using System.Data;
using System.Configuration;
using System.Data.OleDb;

namespace ISUtils.Database.Link
{
    public class OleDbLinker : DBLinker, DataBaseLinker
    {
        /**/
        /// <summary>
        /// 数据库连接
        /// </summary>
        private OleDbConnection m_Conn = null;
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="args">数据库连接字符串列表</param>
        public OleDbLinker(params string[] args)
        {
            Connect(args);
        }
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ConnectString">数据库连接字符串</param>
        public OleDbLinker(string ConnectString)
        {
            Connect(ConnectString);
        }
        /**/
        /// <summary>
        /// 与数据库建立连接
        /// </summary>
        /// <param name="connectStrs">数据库连接参数列表</param>
        /// <returns>返回类型</returns>
        public override bool Connect(params string[] connectArgs)
        {
            bool isConnect = false;

            if (m_Conn == null || m_Conn.State != ConnectionState.Open)
            {
                m_Conn = new OleDbConnection(SupportClass.String.GetConnectStr(connectArgs));

                m_Conn.Open();
                isConnect = true;
            }

            return isConnect;
        }
        /**/
        /// <summary>
        /// 与数据库建立连接
        /// </summary>
        /// <param name="connectString">数据库连接字符串</param>
        /// <returns>返回类型</returns>
        public override bool Connect(string connectString)
        {
            bool isConnect = false;

            if (m_Conn == null || m_Conn.State != ConnectionState.Open)
            {
                m_Conn = new OleDbConnection(connectString);

                m_Conn.Open();
                isConnect = true;
            }

            return isConnect;
        }
        /**/
        /// <summary>
        /// 执行数据库查询语句
        /// </summary>
        /// <param name="strSQL">结构化数据库查询语句</param>
        /// <returns>返回类型</returns>
        public override DataTable ExecuteSQL(string strSQL)
        {
            DataTable dt = new DataTable();
            OleDbCommand command = null;
            OleDbDataAdapter da = null;

            try
            {
                command = new OleDbCommand(strSQL, m_Conn);
                da = new OleDbDataAdapter();
                da.SelectCommand = command;

                da.Fill(dt);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                command.Dispose();
                da.Dispose();
            }

            return dt;
        }
        /**/
        /// <summary>
        /// 执行数据库查询语句
        /// </summary>
        /// <param name="sql">结构化数据库查询语句</param>
        public override void ExecSQL(string sql)
        {
            OleDbCommand command = null;
            try
            {
                command = new OleDbCommand(sql, m_Conn);
                command.ExecuteNonQuery();
            }
            finally
            {
                command.Dispose();
            }
        }
        /**/
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public override void Close()
        {
            if (m_Conn != null && m_Conn.State == ConnectionState.Open)
            {
                try
                {
                    this.m_Conn.Close();
                }
                catch { }
            }
        }
        /**/
        /// <summary>
        /// 析构函数
        /// </summary>
        ~OleDbLinker()
        {
            this.Close();
            GC.Collect();
        }
    }
}
