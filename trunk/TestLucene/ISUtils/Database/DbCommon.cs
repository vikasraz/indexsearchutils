using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data;
using ISUtils.Common;
using ISUtils.Database.Link;

namespace ISUtils.Database
{
    public static class DbCommon
    {
        public static bool TestDbLink(DBTypeEnum dbType,string hostName,string db,string user,string pwd)
        {
            string connStr = "", format="";
            DbConnection conn;
            try
            {
                switch (dbType)
                {
                    case DBTypeEnum.ODBC:
                        format = "Server={0};Database={1};Uid={2};Pwd={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        conn = new OdbcConnection(connStr);
                        break;
                    case DBTypeEnum.OLE_DB:
                        format = "Server={0};Database={1};Uid={2};Pwd={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        conn = new OleDbConnection(connStr);
                        break;
                    case DBTypeEnum.Oracle:
                        format = "host={0};Data source={1};User id={2};Password={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        conn = new OracleConnection(connStr);
                        break;
                    case DBTypeEnum.SQL_Server:
                        format = "Data Source={0};Initial Catalog= {1};User Id={2};Password={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        conn = new SqlConnection(connStr);
                        break;
                    default:
                        format = "Data Source={0};Initial Catalog= {1};User Id={2};Password={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        conn = new SqlConnection(connStr);
                        break;
                }
                conn.Open();
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Close();
                    return false;
                }
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                System.Console.WriteLine(e.StackTrace.ToString());
#endif
                return false;
            }
        }
        public static bool TestQuery(DBTypeEnum dbType, string hostName, string db, string user, string pwd, string query)
        {
            string connStr = "", format = "";
            DBLinker linker;
            try
            {
                switch (dbType)
                {
                    case DBTypeEnum.ODBC:
                        format = "Server={0};Database={1};Uid={2};Pwd={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        linker =new OdbcLinker(connStr);
                        break;
                    case DBTypeEnum.OLE_DB:
                        format = "Server={0};Database={1};Uid={2};Pwd={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        linker = new OleDbLinker(connStr);
                        break;
                    case DBTypeEnum.Oracle:
                        format = "host={0};Data source={1};User id={2};Password={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        linker = new OracleLinker(connStr);
                        break;
                    case DBTypeEnum.SQL_Server:
                        format = "Data Source={0};Initial Catalog= {1};User Id={2};Password={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        linker = new SqlServerLinker(connStr);
                        break;
                    default:
                        format = "Data Source={0};Initial Catalog= {1};User Id={2};Password={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        linker = new SqlServerLinker(connStr);
                        break;
                }
                DataTable dt = linker.ExecuteSQL(query);
                int count = dt.Rows.Count;
                dt.Clear();
                linker.Close();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
#if DEBUG
                System.Console.WriteLine(e.StackTrace.ToString());
#endif
                return false;
            }
        }
        public static bool TestFields(DBTypeEnum dbType, string hostName, string db, string user, string pwd, string query,string fields)
        {
            string connStr = "", format = "";
            DBLinker linker;
            try
            {
                switch (dbType)
                {
                    case DBTypeEnum.ODBC:
                        format = "Server={0};Database={1};Uid={2};Pwd={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        linker = new OdbcLinker(connStr);
                        break;
                    case DBTypeEnum.OLE_DB:
                        format = "Server={0};Database={1};Uid={2};Pwd={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        linker = new OleDbLinker(connStr);
                        break;
                    case DBTypeEnum.Oracle:
                        format = "host={0};Data source={1};User id={2};Password={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        linker = new OracleLinker(connStr);
                        break;
                    case DBTypeEnum.SQL_Server:
                        format = "Data Source={0};Initial Catalog= {1};User Id={2};Password={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        linker = new SqlServerLinker(connStr);
                        break;
                    default:
                        format = "Data Source={0};Initial Catalog= {1};User Id={2};Password={3};";
                        connStr = string.Format(format, hostName, db, user, pwd);
                        linker = new SqlServerLinker(connStr);
                        break;
                }
                DataTable dt = linker.ExecuteSQL(query);
                int count = dt.Rows.Count;
                if (count < 0)
                {
                    dt.Clear();
                    linker.Close();
                    return false;
                }
                string[] fieldArray = SupportClass.String.Split(fields,"\t, ");
                bool find;
                foreach (string field in fieldArray)
                {
                    find = false;
                    foreach(DataColumn column in dt.Columns)
                    {
                        if(column.ColumnName.ToLower().CompareTo(field.ToLower())==0)
                        {
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        dt.Clear();
                        linker.Close();
                        return false;
                    }
                }
                dt.Clear();
                linker.Close();
                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                System.Console.WriteLine(e.StackTrace.ToString());
#endif
                return false;
            }
        }
    }
}
