using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    /**/
    /// <summary>
    /// Searchd设置
    /// </summary>
    [Serializable]
    public class SearchSet
    {
        #region Flags
        /**/
        /// <summary>
        /// IP地址的标志
        /// </summary>
        public const string AddressFlag = "ADDRESS";
        /**/
        /// <summary>
        /// 端口的标志
        /// </summary>
        public const string PortFlag = "PORT";
        /**/
        /// <summary>
        /// 结果信息的标志
        /// </summary>
        public const string LogPathFlag = "LOG";
        /**/
        /// <summary>
        /// 查询信息路径的标志
        /// </summary>
        public const string QueryLogPathFlag = "QUERY_LOG";
        /**/
        /// <summary>
        /// 超时时间的标志
        /// </summary>
        public const string TimeOutFlag = "READ_TIMEOUT";
        /**/
        /// <summary>
        /// 最大匹配记录数的标志
        /// </summary>
        public const string MaxChildrenFlag = "MAX_CHILDREN";
        /**/
        /// <summary>
        /// 最大匹配记录数的标志
        /// </summary>
        public const string MaxMatchesFlag = "MAX_MATCHES";
        public const string MaxTransFlag = "MAX_TRANS";
        #endregion
        #region Property
        /**/
        /// <summary>
        /// 存储IP地址
        /// </summary>
        private string address = "192.168.0.1";
        /**/
        /// <summary>
        /// 设定或返回IP地址
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        /**/
        /// <summary>
        /// 存储端口
        /// </summary>
        private int port = 3312;
        /**/
        /// <summary>
        /// 设定或返回端口
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        /**/
        /// <summary>
        /// 存储结果信息
        /// </summary>
        private string logpath = "";
        /**/
        /// <summary>
        /// 设定或返回结果信息
        /// </summary>
        public string LogPath
        {
            get { return logpath; }
            set { logpath = value; }
        }
        /**/
        /// <summary>
        /// 存储查询信息路径
        /// </summary>
        private string querylogpath;
        /**/
        /// <summary>
        /// 设定或返回查询信息路径
        /// </summary>
        public string QueryLogPath
        {
            get { return querylogpath; }
            set { querylogpath = value; }
        }
        /**/
        /// <summary>
        /// 存储超时时间
        /// </summary>
        private int timeout = 5;
        /**/
        /// <summary>
        /// 设定或返回超时时间，秒
        /// </summary>
        public int TimeOut
        {
            get { return timeout; }
            set { timeout = value; }
        }
        /**/
        /// <summary>
        /// 存储最大匹配记录数
        /// </summary>
        private int maxchildren = 0;
        /**/
        /// <summary>
        /// 设定或返回最大查询客户数
        /// </summary>
        public int MaxChildren
        {
            get { return maxchildren; }
            set { maxchildren = value; }
        }
        /**/
        /// <summary>
        /// 存储最大匹配记录数
        /// </summary>
        private int maxmatches = 1000;
        /**/
        /// <summary>
        /// 设定或返回最大匹配记录数
        /// </summary>
        public int MaxMatches
        {
            get { return maxmatches; }
            set { maxmatches = value; }
        }
        private int maxtrans = 100;
        public int MaxTrans
        {
            get { return maxtrans; }
            set { maxtrans = value; }
        }
        private float minscore = 0.5f;
        public float MinScore
        {
            get { return minscore; }
            set { minscore = value; }
        }
        #endregion
        #region Override
        /**/
        /// <summary>
        /// 获取SearchSet内容
        /// </summary>
        /// <returns>返回类型</returns>
        //public const string MaxTrans
        public override string ToString()
        {
            string ret = string.Format("Searchd:Address({0}),Port({1}),Log({2}),Query Log({3}),Read Timeout({4}),Max Children({5}),Max Matches({6})",
                                     address, port, logpath,
                                     querylogpath, timeout, maxchildren,maxmatches);
            return base.ToString() + "\t" + ret;
        }
        #endregion
        #region Static Func
        /**/
        /// <summary>
        /// 获取字符串列表中的搜索引擎设置
        /// </summary>
        /// <param name="srcList">字符串列表</param>
        /// <returns>返回类型</returns>
        public static SearchSet GetSearchSet(List<string> srcList)
        {
            SearchSet set = new SearchSet();
            bool findSearchd = false, searchdStart = false;
            foreach (string s in srcList)
            {
                if (SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Config.SearchdFlag))
                {
                    findSearchd = true;
                    continue;
                }
                if (findSearchd && SupportClass.String.FormatStr(s).StartsWith(Config.Prefix))
                {
                    searchdStart = true;
                    continue;
                }
                if (findSearchd && searchdStart && SupportClass.String.FormatStr(s).StartsWith(Config.Suffix))
                {
                    findSearchd = false;
                    searchdStart = false;
                    break;
                }
                if (SupportClass.String.FormatStr(s).StartsWith(Config.Ignore))
                    continue;
                if (findSearchd && searchdStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), SearchSet.AddressFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        set.Address=split[1];
                    continue;
                }
                if (findSearchd && searchdStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), SearchSet.LogPathFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        set.LogPath = split[1];
                    continue;
                }
                if (findSearchd && searchdStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), SearchSet.MaxChildrenFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        set.MaxChildren= int.Parse(split[1]);
                    continue;
                }
                if (findSearchd && searchdStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), SearchSet.MaxMatchesFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        set.MaxMatches = int.Parse(split[1]);
                    continue;
                }
                if (findSearchd && searchdStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), SearchSet.PortFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        set.Port = int.Parse(split[1]);
                    continue;
                }
                if (findSearchd && searchdStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), SearchSet.QueryLogPathFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        set.QueryLogPath = split[1];
                    continue;
                }
                if (findSearchd && searchdStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), SearchSet.TimeOutFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        set.TimeOut = int.Parse(split[1]);
                    continue;
                }
                if (findSearchd && searchdStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), SearchSet.MaxTransFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        set.MaxTrans= int.Parse(split[1]);
                    continue;
                }
            }
            return set;
        }
        public static void WriteToFile(ref System.IO.StreamWriter sw, SearchSet searchSet)
        {
            sw.WriteLine("#############################################");
            sw.WriteLine("#searchd settings");
            sw.WriteLine("#############################################");
            sw.WriteLine(Config.SearchdFlag.ToLower());
            sw.WriteLine(Config.Prefix);
            sw.WriteLine("\t#搜索主机");
            sw.WriteLine("\t" + SearchSet.AddressFlag.ToLower() + "=" + searchSet.Address);
            sw.WriteLine();
            sw.WriteLine("\t#监听端口");
            sw.WriteLine("\t" + SearchSet.PortFlag.ToLower() + "=" + searchSet.Port);
            sw.WriteLine();
            sw.WriteLine("\t#搜索输出");
            sw.WriteLine("\t" + SearchSet.LogPathFlag.ToLower() + "=" + searchSet.LogPath);
            sw.WriteLine();
            sw.WriteLine("\t#查询输出");
            sw.WriteLine("\t" + SearchSet.QueryLogPathFlag.ToLower() + "=" + searchSet.QueryLogPath);
            sw.WriteLine();
            sw.WriteLine("\t#搜索超时");
            sw.WriteLine("\t" + SearchSet.TimeOutFlag.ToLower() + "=" + searchSet.TimeOut.ToString());
            sw.WriteLine();
            sw.WriteLine("\t#允许连接数目");
            sw.WriteLine("\t" + SearchSet.MaxChildrenFlag.ToLower() + "=" + searchSet.MaxChildren.ToString());
            sw.WriteLine();
            sw.WriteLine("\t#最大匹配记录数");
            sw.WriteLine("\t" + SearchSet.MaxMatchesFlag.ToLower() + "=" + searchSet.MaxMatches.ToString());
            sw.WriteLine();
            sw.WriteLine("\t#异步最大传送");
            sw.WriteLine("\t" + SearchSet.MaxTransFlag.ToLower() + "=" + searchSet.MaxTrans.ToString());
            sw.WriteLine(Config.Suffix);
        }
        #endregion
    }
}
