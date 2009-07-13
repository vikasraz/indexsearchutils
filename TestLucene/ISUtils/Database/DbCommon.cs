using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data;
using ISUtils.Common;
using ISUtils.Database.Link;
using ISUtils.Async;

namespace ISUtils.Database
{
    public delegate void IndexCompletedEventHandler(object sender, IndexCompletedEventArgs e);
    public delegate void IndexProgressChangedEventHandler(object sender, IndexProgressChangedEventArgs e);
    public delegate void WriteRowCompletedEventHandler(object sender, WriteRowCompletedEventArgs e);
    public delegate void WriteTableCompletedEventHandler(object sender, WriteTableCompletedEventArgs e);
    public delegate void WriteDbProgressChangedEventHandler(object sender, WriteDbProgressChangedEventArgs e);
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
                List<FieldProperties> fpList = new List<FieldProperties>();
                if (fields.IndexOf(')') > 0)
                {
                    string[] split = SupportClass.String.Split(fields, ")");
                    foreach (string token in split)
                        fpList.Add(new FieldProperties(token));
                }
                else
                {
                    string[] split = SupportClass.String.Split(fields, ",");
                    foreach (string token in split)
                        fpList.Add(new FieldProperties(token));
                }
                bool find;
                foreach (FieldProperties fp in fpList)
                {
                    find = false;
                    foreach(DataColumn column in dt.Columns)
                    {
                        if(column.ColumnName.ToLower().CompareTo(fp.Name.ToLower())==0)
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
        public static void GetStructures(string configFilePath, out Dictionary<string, List<IndexSet>> tableIndexDict, out Dictionary<string, List<FieldInfo>> tableFieldDict, out Dictionary<IndexSet, List<string>> indexTableDict)
        {
            List<string> srcList = SupportClass.File.GetFileText(configFilePath);
            List<Source> sourceList = Source.GetSourceList(srcList);
            List<IndexSet> indexList = IndexSet.GetIndexList(srcList);
            Dictionary<IndexSet, Source>  indexDict = new Dictionary<IndexSet, Source>();
            foreach (IndexSet set in indexList)
            {
                foreach (Source source in sourceList)
                {
                    if (source.SourceName == set.SourceName)
                    {
                        if(!indexDict.ContainsKey(set))
                            indexDict.Add(set, source);
                        break;
                    }
                }
            }
            tableIndexDict = new Dictionary<string, List<IndexSet>>();
            tableFieldDict = new Dictionary<string, List<FieldInfo>>();
            indexTableDict = new Dictionary<IndexSet, List<string>>();
            Dictionary<string, int> tbDict = new Dictionary<string, int>();
            foreach (IndexSet set in indexDict.Keys)
            {
                Dictionary<string, List<FieldInfo>> tfDict = GetQueryTableFields(indexDict[set].DBType, indexDict[set].GetConnString(), indexDict[set].Query);
                foreach (string table in tfDict.Keys)
                {
                    if (tableFieldDict.ContainsKey(table) == false)
                    {
                        tableFieldDict.Add(table, tfDict[table]);
                    }
                    if (tbDict.ContainsKey(table) == false)
                    {
                        tbDict.Add(table, table.Length);
                    }
                }
                List<string> tbList = new List<string>();
                tbList.AddRange(tfDict.Keys);
                if(indexTableDict.ContainsKey(set)==false)
                    indexTableDict.Add(set,tbList);
            }
            foreach (string table in tbDict.Keys)
            {
                List<IndexSet> inList = new List<IndexSet>();
                foreach (IndexSet set in indexTableDict.Keys)
                {
                    if (indexTableDict[set].Contains(table))
                    {
                        inList.Add(set);
                    }
                }
                if(tableIndexDict.ContainsKey(table)==false)
                    tableIndexDict.Add(table, inList);
            }
        }
        public static void GetStructures(List<Source> sourceList, List<IndexSet> indexList, out Dictionary<string, List<IndexSet>> tableIndexDict, out Dictionary<string, List<FieldInfo>> tableFieldDict, out Dictionary<IndexSet, List<string>> indexTableDict)
        {
            Dictionary<IndexSet, Source> indexDict = new Dictionary<IndexSet, Source>();
            foreach (IndexSet set in indexList)
            {
                foreach (Source source in sourceList)
                {
                    if (source.SourceName == set.SourceName)
                    {
                        if(indexDict.ContainsKey(set)==false)
                            indexDict.Add(set, source);
                        break;
                    }
                }
            }
            tableIndexDict = new Dictionary<string, List<IndexSet>>();
            tableFieldDict = new Dictionary<string, List<FieldInfo>>();
            indexTableDict = new Dictionary<IndexSet, List<string>>();
            Dictionary<string, int> tbDict = new Dictionary<string, int>();
            foreach (IndexSet set in indexDict.Keys)
            {
                Dictionary<string, List<FieldInfo>> tfDict = GetQueryTableFields(indexDict[set].DBType, indexDict[set].GetConnString(), indexDict[set].Query);
                foreach (string table in tfDict.Keys)
                {
                    if (tableFieldDict.ContainsKey(table) == false)
                    {
                        tableFieldDict.Add(table, tfDict[table]);
                    }
                    if (tbDict.ContainsKey(table) == false)
                    {
                        tbDict.Add(table, table.Length);
                    }
                }
                List<string> tbList = new List<string>();
                tbList.AddRange(tfDict.Keys);
                if(indexTableDict.ContainsKey(set)==false)
                    indexTableDict.Add(set, tbList);
            }
            foreach (string table in tbDict.Keys)
            {
                List<IndexSet> inList = new List<IndexSet>();
                foreach (IndexSet set in indexTableDict.Keys)
                {
                    if (indexTableDict[set].Contains(table))
                    {
                        inList.Add(set);
                    }
                }
                if(tableIndexDict.ContainsKey(table)==false)
                    tableIndexDict.Add(table, inList);
            }
        }
        public static void GetStructures(Dictionary<IndexSet, Source> indexDict, out Dictionary<string, List<IndexSet>> tableIndexDict, out Dictionary<string, List<FieldInfo>> tableFieldDict, out Dictionary<IndexSet, List<string>> indexTableDict)
        {
            tableIndexDict = new Dictionary<string, List<IndexSet>>();
            tableFieldDict = new Dictionary<string, List<FieldInfo>>();
            indexTableDict = new Dictionary<IndexSet, List<string>>();
            Dictionary<string, int> tbDict = new Dictionary<string, int>();
            foreach (IndexSet set in indexDict.Keys)
            {
                Dictionary<string, List<FieldInfo>> tfDict = GetQueryTableFields(indexDict[set].DBType, indexDict[set].GetConnString(), indexDict[set].Query);
                foreach (string table in tfDict.Keys)
                {
                    if (tableFieldDict.ContainsKey(table) == false)
                    {
                        tableFieldDict.Add(table, tfDict[table]);
                    }
                    if (tbDict.ContainsKey(table) == false)
                    {
                        tbDict.Add(table, table.Length);
                    }
                }
                List<string> tbList = new List<string>();
                tbList.AddRange(tfDict.Keys);
                if(indexTableDict.ContainsKey(set)==false)
                    indexTableDict.Add(set, tbList);
            }
            foreach (string table in tbDict.Keys)
            {
                List<IndexSet> inList = new List<IndexSet>();
                foreach (IndexSet set in indexTableDict.Keys)
                {
                    if (indexTableDict[set].Contains(table))
                    {
                        inList.Add(set);
                    }
                }
                if(tableIndexDict.ContainsKey(table)==false)
                    tableIndexDict.Add(table, inList);
            }
        }
        public static Dictionary<string, List<FieldInfo>> GetQueryTableFields(DBTypeEnum dbType, string connStr, string query)
        {
            Dictionary<string, List<FieldInfo>> tableFieldDict = new Dictionary<string, List<FieldInfo>>();
            DBLinker linker;
            try
            {
                switch (dbType)
                {
                    case DBTypeEnum.ODBC:
                        linker = new OdbcLinker(connStr);
                        break;
                    case DBTypeEnum.OLE_DB:
                        linker = new OleDbLinker(connStr);
                        break;
                    case DBTypeEnum.Oracle:
                        linker = new OracleLinker(connStr);
                        break;
                    case DBTypeEnum.SQL_Server:
                        linker = new SqlServerLinker(connStr);
                        break;
                    default:
                        linker = new SqlServerLinker(connStr);
                        break;
                }
                List<string> tableList = SupportClass.QueryParser.TablesInQuery(query);
                foreach (string table in tableList)
                {
                    DataTable dt = linker.ExecuteSQL(SupportClass.QueryParser.TopOneOfTable(table));
                    List<FieldInfo> fieldList = new List<FieldInfo>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        fieldList.Add(new FieldInfo(column.ColumnName,column.DataType,column.Unique));
                    }
                    if (tableFieldDict.ContainsKey(table) == false)
                        tableFieldDict.Add(table, fieldList);
                    dt.Clear();
                }
                linker.Close();
            }
            catch (Exception e)
            {
#if DEBUG
                System.Console.WriteLine(e.StackTrace.ToString());
#endif
            }
            return tableFieldDict;
        }
        public static Dictionary<IndexSet,Source> GetExcelSettings(string path)
        {
            if (SupportClass.File.IsFileExists(path) == false)
                throw new ArgumentException("path is not valid.", "path");
            DataTable table = ExcelLinker.GetDataTableFromFile(path);
            Dictionary<IndexSet, Source> dict = new Dictionary<IndexSet, Source>();
            string tableName = "",currentTableName;
            Source source=null;
            IndexSet indexSet=null;
            List<FieldProperties> fpList=new List<FieldProperties>();
            bool change=false;
            foreach (DataRow row in table.Rows)
            {
                FieldProperties fp=new FieldProperties();
                fp.Visible = true;
                fp.Order = 0;
                #region DataColumn
                foreach (DataColumn column in table.Columns)
                {
#if DEBUG
                    System.Console.Write(row[column] + "\t");
#endif
                    if (column.ColumnName.Equals("Table"))
                    {
                        currentTableName = row[column].ToString();
                        if (currentTableName != tableName)
                        {
                            if (source != null && indexSet != null)
                            {
                                source.Fields=fpList;
                                if(dict.ContainsKey(indexSet)==false)
                                    dict.Add(indexSet, source);
                            }
                            tableName = currentTableName;
                            fpList = new List<FieldProperties>();
                            source = new Source();
                            indexSet = new IndexSet();
                            source.SourceName = row[column].ToString();
                            source.Query = "select * from " + row[column].ToString();
                            indexSet.IndexName = row[column].ToString();
                            indexSet.SourceName = row[column].ToString();
                            change =true;
                        }
                        else
                        {
                            change =false;
                        }
                    }
                    else if (column.ColumnName.Equals("TableCaption"))
                    {
                        if (change && string.IsNullOrEmpty(row[column].ToString()) == false)
                            indexSet.Caption = row[column].ToString();
                    }
                    else if (column.ColumnName.Equals("DbType"))
                    {
                        if (change && string.IsNullOrEmpty(row[column].ToString()) == false)
                            source.DBType = ISUtils.Common.DbType.GetDbType(row[column].ToString());
                    }
                    else if (column.ColumnName.Equals("HostName"))
                    {
                        if (change && string.IsNullOrEmpty(row[column].ToString()) == false)
                            source.HostName = row[column].ToString();
                    }
                    else if (column.ColumnName.Equals("DataBase"))
                    {
                        if (change && string.IsNullOrEmpty(row[column].ToString()) == false)
                            source.DataBase = row[column].ToString();
                    }
                    else if (column.ColumnName.Equals("UserName"))
                    {
                        if (change && string.IsNullOrEmpty(row[column].ToString()) == false)
                            source.UserName = row[column].ToString();
                    }
                    else if (column.ColumnName.Equals("Password"))
                    {
                        if (change && string.IsNullOrEmpty(row[column].ToString()) == false)
                            source.Password = row[column].ToString();
                    }
                    else if (column.ColumnName.Equals("Field"))
                    {
                        if (string.IsNullOrEmpty(row[column].ToString()) == false)
                            fp.Name = row[column].ToString();
                    }
                    else if (column.ColumnName.Equals("Caption"))
                    {
                        if (string.IsNullOrEmpty(row[column].ToString()) == false)
                            fp.Caption = row[column].ToString();
                    }
                    else if (column.ColumnName.Equals("IsTitle"))
                    {
                        if (string.IsNullOrEmpty(row[column].ToString()) == false)
                        {
                            if (row[column].Equals("标题"))
                            {
                                fp.IsTitle=true;
                            }
                            else if (row[column].Equals("内容"))
                            {
                                fp.IsTitle =false;
                            }
                            else 
                            {
                                try
                                {
                                    fp.IsTitle =bool.Parse(row[column].ToString());
                                }
                                catch(Exception e)
                                {
                                    fp.IsTitle =false;
                                }
                            }
                        }
                    }
                    else if (column.ColumnName.Equals("Boost"))
                    {
                        if (string.IsNullOrEmpty(row[column].ToString()) == false)
                        {
                            try
                            {
                                fp.Boost = float.Parse(row[column].ToString());
                            }
                            catch (Exception e)
                            {
                                fp.Boost = 1.0f;
                            }
                        }
                    }
                    else
                    { 
                    }
                }
                #endregion
                fpList.Add(fp);
#if DEBUG
                System.Console.WriteLine();
#endif
            }
            if (source != null && indexSet != null)
            {
                source.Fields = fpList;
                if (dict.ContainsKey(indexSet) == false)
                    dict.Add(indexSet, source);
            }
            return dict;
        }
    }
}
