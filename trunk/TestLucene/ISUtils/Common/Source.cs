using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public class Source
    {
        #region 标志设置
        /**/
        /// <summary>
        /// 数据库类型的标志
        /// </summary>
        public const string DBTypeFlag = "DBTYPE";
        /**/
        /// <summary>
        /// 数据库路径的标志
        /// </summary>
        public const string HostNameFlag = "HOSTNAME";
        /**/
        /// <summary>
        /// 数据库名的标志
        /// </summary>
        public const string DataBaseFlag = "DATABASE";
        /**/
        /// <summary>
        /// 查询语句的标志
        /// </summary>
        public const string QueryFlag = "QUERY";
        /**/
        /// <summary>
        /// 索引字段的标志
        /// </summary>
        public const string FieldsFlag = "FIELDS";
        /**/
        /// <summary>
        /// 索引权重的标志
        /// </summary>
        public const string BoostsFlag = "BOOSTS";
        /**/
        /// <summary>
        /// 用户名的标志
        /// </summary>
        public const string UserNameFlag = "USERNAME";
        /**/
        /// <summary>
        /// 密码的标志
        /// </summary>
        public const string PasswordFlag = "PASSWORD";
        /**/
        /// <summary>
        ///主键的标志
        /// </summary>
        public const string PrimaryKeyFlag = "PRIMARYKEY";
        #endregion
        #region 属性
        /**/
        /// <summary>
        /// 存储Source的名称
        /// </summary>
        private string sourcename = "";
        /**/
        /// <summary>
        /// Gets and Sets SourceName
        /// </summary>
        public string SourceName
        {
            get { return sourcename; }
            set { sourcename = value; }
        }
        /**/
        /// <summary>
        /// 存储数据库类型
        /// </summary>
        private DBTypeEnum dbtype = DBTypeEnum.SQL_Server;
        /**/
        /// <summary>
        /// 设定或返回数据库类型
        /// </summary>
        public DBTypeEnum DBType
        {
            get { return dbtype;}
            set { dbtype = value; }
        }
        /**/
        /// <summary>
        /// 存储数据库路径
        /// </summary>
        private string hostname = "";
        /**/
        /// <summary>
        /// 设定或返回数据库路径
        /// </summary>
        public string HostName
        {
            get { return hostname; }
            set { hostname = value; }
        }
        /**/
        /// <summary>
        /// 存储数据库名称
        /// </summary>
        private string database = "";
        /**/
        /// <summary>
        /// 设定或返回数据库名称
        /// </summary>
        public string DataBase
        {
            get { return database; }
            set { database = value; }
        }
        /**/
        /// <summary>
        /// 存储数据库查询语句
        /// </summary>
        private string query = "";
        /**/
        /// <summary>
        /// 设定或返回数据库查询语句
        /// </summary>
        public string Query
        {
            get { return query; }
            set { query = value; }
        }
        /**/
        /// <summary>
        /// 存储索引字段名称
        /// </summary>
        private FieldProperties[] fields;
        /**/
        /// <summary>
        /// 设定或返回索引字段名称
        /// </summary>
        public FieldProperties[] Fields
        {
            get { return fields; }
            set 
            { 
                fields = (FieldProperties[])value.Clone();
                if (fields != null)
                { 
                    if (fieldDict ==null)
                        fieldDict = new Dictionary<string, FieldProperties>();
                    if (boostDict == null)
                        boostDict = new Dictionary<string, float>();
                    foreach (FieldProperties fb in fields)
                    {
                        fieldDict.Add(fb.Field, fb);
                        boostDict.Add(fb.Field, fb.Boost);
                    }
                }
            }
        }
        private Dictionary<string, FieldProperties> fieldDict = new Dictionary<string, FieldProperties>();
        public string[] StringFields
        {
            get
            {
                string[] szFields = new string[fieldDict.Count];
                fieldDict.Keys.CopyTo(szFields, 0);
                return szFields;
            }
        }
        public Dictionary<string, FieldProperties> FieldDict
        {
            get
            {
                return fieldDict;
            }
        }
        private Dictionary<string, float> boostDict = new Dictionary<string, float>();
        public Dictionary<string, float> FieldBoostDict
        {
            get { return boostDict; }
        }
        /**/
        /// <summary>
        /// 存储用户名
        /// </summary>
        private string username = "";
        /**/
        /// <summary>
        /// 设定或返回用户名
        /// </summary>
        public string UserName
        {
            get { return username ; }
            set { username = value; }
        }
        /**/
        /// <summary>
        /// 存储密码
        /// </summary>
        private string password = "";
        /**/
        /// <summary>
        /// 设定或返回密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        /**/
        /// <summary>
        /// 存储主键
        /// </summary>
        private string primaryKey = "";
        /**/
        /// <summary>
        /// 设定或返回主键
        /// </summary>
        public string PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }
        }
        #endregion
        #region 方法
        public string GetFields()
        {
            if (fields == null)
                return "";
            string ret=" ";
            foreach (FieldProperties fb in fields)
            {
                ret += "," + fb.ToString();
            }
            return ret.Substring(ret.IndexOf(',')+1).Trim();
        }
        /**/
        /// <summary>
        /// 获取Source的连接字符串
        /// </summary>
        /// <returns>返回类型</returns>
        public string GetConnString()
        {
            return GetConnectString(this);
        }
        public bool AllContains(params string[] fieldArray)
        {
            bool ret = true;
            foreach (string field in fieldArray)
            {
                ret = ret && fieldDict.ContainsKey(field);
            }
            return ret;
        }
        #endregion
        #region 重写
        /**/
        /// <summary>
        /// 获取Source的内容
        /// </summary>
        /// <returns>返回类型</returns>
        public override string ToString()
        {
            string ret = string.Format("Souce[{0}]:DBType({1}),Host({2}),DB({3}),User({4}),Pwd({5}),Query({6}),PrimayKey({7}),Fields(",
                                     sourcename, DbType.GetDbTypeStr(dbtype), hostname, database,
                                     username, password, query, primaryKey);
            if (fields != null && fields.Length > 0)
            {
                foreach (FieldProperties fb in fields)
                    ret += fb.ToString() + ",";
                ret = ret.Substring(0, ret.Length - 1);
            }
            ret += ")";
            return base.ToString() + "\t" + ret;
        }
        #endregion
        #region 公共方法
        /**/
        /// <summary>
        /// 获取Source的连接字符串
        /// </summary>
        /// <param name="source">Source名称</param>
        /// <returns>返回类型</returns>
        public static string GetConnectString(Source source)
        {
            string connectstr;
            switch (source.DBType)
            { 
                case DBTypeEnum.SQL_Server:
                    connectstr = string.Format("Data Source={0};Initial Catalog= {1};User Id={2};Password={3};", source.HostName, source.DataBase, source.UserName, source.Password);
                    break;
                case DBTypeEnum.Oracle:
                    connectstr = string.Format("host={0};Data source={1};User id={2};Password={3};", source.HostName, source.DataBase, source.UserName, source.Password);
                    break;
                case DBTypeEnum.OLE_DB:
                case DBTypeEnum.ODBC:
                default:
                    connectstr = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", source.HostName, source.DataBase, source.UserName, source.Password);
                    break;
            }
            return connectstr;
        }
        /**/
        /// <summary>
        /// 获取字符串列表中的Source列表
        /// </summary>
        /// <param name="srcList">字符串列表</param>
        /// <returns>返回类型</returns>
        public static List<Source> GetSourceList(List<string> srcList)
        {
            List<Source> sourceList = new List<Source>();
            bool findSource = false,srcStart=false ;
            Source src = new Source();
            foreach (string s in srcList)
            {
                if (SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Config.SourceFlag))
                {
                    findSource = true;
                    src = new Source();
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, " \t");
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split )
                        Console.WriteLine(a);
#endif
                    if (split.Length < 2)
                        continue;
                    src.SourceName = split[1];
                    continue;
                }
                if (findSource && SupportClass.String.FormatStr(s).StartsWith(Config.Prefix))
                {
                    srcStart = true;
                    continue;
                }
                if (findSource && srcStart && SupportClass.String.FormatStr(s).StartsWith(Config.Suffix))
                {
                    findSource = false;
                    srcStart = false;
                    if (string.IsNullOrEmpty(src.SourceName) == false)
                      sourceList.Add(src);
                    continue;
                }
                if (SupportClass.String.FormatStr(s).StartsWith(Config.Ignore))
                    continue;
                if (findSource && srcStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Source.DBTypeFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        try
                        {
                            src.DBType = DbType.GetDbType(split[1]);
                        }
                        catch(Exception e)
                        {
                            src.DBType = DBTypeEnum.SQL_Server;
#if DEBUG
                            System.Console.WriteLine(e.StackTrace.ToString());
#endif
                        }
                    else
                        src.DBType= DBTypeEnum.SQL_Server;
                    continue;
                }
                if (findSource && srcStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Source.DataBaseFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        src.DataBase= split[1];
                    else
                        src.DataBase="";
                    continue;
                }
                if (findSource && srcStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Source.PrimaryKeyFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        src.PrimaryKey = split[1];
                    else
                        src.PrimaryKey = "";
                    continue;
                }
                if (findSource && srcStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Source.HostNameFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        src.HostName= split[1];
                    else
                        src.HostName= "";
                    continue;
                }
                if (findSource && srcStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Source.PasswordFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        src.Password= split[1];
                    else
                        src.Password= "";
                    continue;
                }
                if (findSource && srcStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Source.QueryFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        src.Query = split[1];
                    else
                        src.Query = "";
                    continue;
                }
                if (findSource && srcStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Source.FieldsFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    int pos = format.IndexOf('=');
                    string rest = format.Substring(pos + 1);
                    if (rest.IndexOf(')') > 0)
                    {
                        List<FieldProperties> fpList = new List<FieldProperties>();
                        string[] split = SupportClass.String.Split(rest, ")");
#if DEBUG
                        Console.WriteLine(format);
                        foreach (string a in split)
                            Console.WriteLine(a);
#endif
                        foreach (string token in split)
                            fpList.Add(new FieldProperties(token));
                        src.Fields = fpList.ToArray();
                    }
                    else
                    {
                        List<FieldProperties> fpList = new List<FieldProperties>();
                        string[] split = SupportClass.String.Split(rest, ",");
#if DEBUG
                        Console.WriteLine(format);
                        foreach (string a in split)
                            Console.WriteLine(a);
#endif
                        foreach (string token in split)
                            fpList.Add(new FieldProperties(token));
                        src.Fields = fpList.ToArray();
                    }
                    continue;
                }
                if (findSource && srcStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Source.UserNameFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        src.UserName= split[1];
                    else
                        src.UserName = "";
                    continue;
                }
            }
            return sourceList;
        }
        public static void WriteToFile(ref System.IO.StreamWriter sw, Source source)
        {
            sw.WriteLine("#############################################");
            sw.WriteLine(string.Format("#source {0}", source.SourceName));
            sw.WriteLine("#############################################");
            sw.WriteLine(Config.SourceFlag.ToLower() + " " + source.SourceName);
            sw.WriteLine(Config.Prefix);
            sw.WriteLine("\t#数据库连接类型");
            sw.WriteLine("\t#" + Source.DBTypeFlag.ToLower() + "=" + DbType.GetDbTypeStr(DBTypeEnum.SQL_Server));
            sw.WriteLine("\t#" + Source.DBTypeFlag.ToLower() + "=" + DbType.GetDbTypeStr(DBTypeEnum.Oracle));
            sw.WriteLine("\t#" + Source.DBTypeFlag.ToLower() + "=" + DbType.GetDbTypeStr(DBTypeEnum.OLE_DB));
            sw.WriteLine("\t#" + Source.DBTypeFlag.ToLower() + "=" + DbType.GetDbTypeStr(DBTypeEnum.ODBC));
            sw.WriteLine("\t" + Source.DBTypeFlag.ToLower() + "=" + DbType.GetDbTypeStr(source.DBType));
            sw.WriteLine();
            sw.WriteLine("\t#服务器名");
            sw.WriteLine("\t" + Source.HostNameFlag.ToLower() + "=" + source.HostName);
            sw.WriteLine();
            sw.WriteLine("\t#数据库名");
            sw.WriteLine("\t" + Source.DataBaseFlag.ToLower() + "=" + source.DataBase);
            sw.WriteLine();
            sw.WriteLine("\t#用户名");
            sw.WriteLine("\t" + Source.UserNameFlag.ToLower() + "=" + source.UserName);
            sw.WriteLine();
            sw.WriteLine("\t#密码");
            sw.WriteLine("\t" + Source.PasswordFlag.ToLower() + "=" + source.Password);
            sw.WriteLine();
            sw.WriteLine("\t#查询");
            sw.WriteLine("\t" + Source.QueryFlag.ToLower() + "=" + source.Query);
            sw.WriteLine();
            sw.WriteLine("\t#字段列表");
            sw.WriteLine("\t" + Source.FieldsFlag.ToLower() + "=" + source.GetFields());
            sw.WriteLine();
            sw.WriteLine("\t#主键");
            sw.WriteLine("\t" + Source.PrimaryKeyFlag.ToLower() + "=" + source.PrimaryKey);
            sw.WriteLine(Config.Suffix);
        }
        #endregion
    }
}
