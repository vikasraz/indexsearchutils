using System;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;

namespace ISUtils.Database.Link
{
    public class OracleLinker :DBLinker, DataBaseLinker
    {
        /**/
        /// <summary>
        /// 数据库连接
        /// </summary>
        private OracleConnection m_Conn = null;
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="args">数据库连接字符串列表</param>
        public OracleLinker(params string[] strParams)
        {
            Connect(strParams);
        }
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ConnectString">数据库连接字符串</param>
        public OracleLinker(string ConnectString)
        {
            Connect(ConnectString);
        }
        /**/
        /// <summary>
        /// 与数据库建立连接
        /// </summary>
        /// <param name="connectStrs">数据库连接参数列表</param>
        /// <returns>返回类型</returns>
        public override bool Connect(params string[] connectStrs)
        {
            bool isConnect = false;

            if (m_Conn == null || m_Conn.State != ConnectionState.Open)
            {
                m_Conn = new OracleConnection(SupportClass.String.GetConnectStr(connectStrs));

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
                m_Conn = new OracleConnection(connectString);

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
            OracleCommand command = null;
            OracleDataAdapter da = null;

            try
            {
                command = new OracleCommand(strSQL, m_Conn);
                da = new OracleDataAdapter();
                da.SelectCommand = command;

                da.Fill(dt);
            }
            catch (Exception e)
            {
#if DEBUG
                System.Console.WriteLine(e.StackTrace.ToString());
#endif
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
            OracleCommand command = null;
            try
            {
                command = new OracleCommand(sql, m_Conn);
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
        ~OracleLinker()
        {
            this.Close();
            GC.Collect();
        }
    }
}
