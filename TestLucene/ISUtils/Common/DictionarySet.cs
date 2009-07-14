using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    public class DictionarySet
    {
        #region Public Const Flag
        /**/
        /// <summary>
        /// 基本文件路径的标志
        /// </summary>
        public const string BasePathFlag = "BASEPATH";
        /**/
        /// <summary>
        /// 人名文件路径的标志
        /// </summary>
        public const string NamePathFlag = "NAMEPATH";
        /**/
        /// <summary>
        /// 数字文件路径的标志
        /// </summary>
        public const string NumberPathFlag = "NUMBERPATH";
        /**/
        /// <summary>
        /// 用户自定义文件路径的标志
        /// </summary>
        public const string CustomPathFlag = "CUSTOMPATH";
        /**/
        /// <summary>
        /// 过滤器文件路径的标志
        /// </summary>
        public const string FilterPathFlag = "FILTERPATH";
        #endregion
        #region Property
        /**/
        /// <summary>
        /// 存储基本路径
        /// </summary>
        private string basepath = "";
        /**/
        /// <summary>
        /// 设定或返回基本路径
        /// </summary>
        public string BasePath
        {
            get { return basepath; }
            set { basepath = value; }
        }
        /**/
        /// <summary>
        /// 存储人名文件路径
        /// </summary>
        private string namepath = "";
        /**/
        /// <summary>
        /// 设定或返回人名文件路径
        /// </summary>
        public string NamePath
        {
            get { return namepath; }
            set { namepath = value; }
        }
        /**/
        /// <summary>
        /// 存储数字文件路径
        /// </summary>
        private string numberpath = "";
        /**/
        /// <summary>
        /// 设定或返回数字文件路径
        /// </summary>
        public string NumberPath
        {
            get { return numberpath; }
            set { numberpath = value; }
        }
        /**/
        /// <summary>
        /// 存储过滤器路径
        /// </summary>
        private string filterpath = "";
        /**/
        /// <summary>
        /// 设定或返回过滤器路径
        /// </summary>
        public string FilterPath
        {
            get { return filterpath; }
            set { filterpath = value; }
        }
        /**/
        /// <summary>
        /// 存储用户自定义文件路径
        /// </summary>
        private List<string> custompaths=new List<string>();
        /**/
        /// <summary>
        /// 设定或返回用户自定义路径
        /// </summary>
        public List<string> CustomPaths
        {
            get { return custompaths; }
            set { custompaths = value; }
        }
        #endregion
        #region Override
        /**/
        /// <summary>
        /// 获取Dictionary内容
        /// </summary>
        /// <returns>返回类型</returns>
        public override string ToString()
        {
            string ret = string.Format("Dictionary:Base({0}),Name({1}),Number({2}),Filter({3}),Custom(",basepath, namepath, numberpath, filterpath);
            if (custompaths.Count > 0)
            {
                foreach (string s in custompaths)
                {
                    ret += s + ",";
                }
                ret = ret.Substring(0, ret.Length - 1);
            }
            ret += ")";
            return base.ToString() + "\t" + ret;
        }
        #endregion
        #region Static Public Function
        /**/
        /// <summary>
        /// 获取字符串列表中的索引列表
        /// </summary>
        /// <param name="srcList">字符串列表</param>
        /// <returns>返回类型</returns>
        public static DictionarySet GetDictionarySet(List<string> srcList)
        {
            DictionarySet dictSet = new DictionarySet();
            List<string> pathList = new List<string>();
            bool findDict = false, dictStart = false;
            foreach (string s in srcList)
            {
                if (SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Config.DictionaryFlag))
                {
                    findDict = true;
                    continue;
                }
                if (findDict && SupportClass.String.FormatStr(s).StartsWith(Config.Prefix))
                {
                    dictStart = true;
                    continue;
                }
                if (findDict && dictStart && SupportClass.String.FormatStr(s).StartsWith(Config.Suffix))
                {
                    findDict = false;
                    dictStart = false;
                    break;
                }
                if (SupportClass.String.FormatStr(s).StartsWith(Config.Ignore))
                    continue;
                if (findDict && dictStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), DictionarySet.BasePathFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format,Config.Devider);

#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        dictSet.BasePath = split[1];
                    else
                        dictSet.BasePath = "";
                    continue;
                }
                if (findDict && dictStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), DictionarySet.CustomPathFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        pathList.Add(split[1]);
                    continue;
                }
                if (findDict && dictStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), DictionarySet.FilterPathFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        dictSet.FilterPath = split[1];
                    else
                        dictSet.FilterPath = "";
                    continue;
                }
                if (findDict && dictStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), DictionarySet.NamePathFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        dictSet.NamePath = split[1];
                    else
                        dictSet.NamePath = "";
                    continue;
                }
                if (findDict && dictStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), DictionarySet.NumberPathFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        dictSet.NumberPath = split[1];
                    else
                        dictSet.NumberPath = "";
                    continue;
                }
            }
            dictSet.CustomPaths = pathList;
            return dictSet;
        }
        public static void WriteToFile(ref System.IO.StreamWriter sw, DictionarySet dictSet)
        {
            sw.WriteLine("#############################################");
            sw.WriteLine("#dictionary settings");
            sw.WriteLine("#############################################");
            sw.WriteLine(Config.DictionaryFlag.ToLower());
            sw.WriteLine(Config.Prefix);
            sw.WriteLine("\t#基本词库");
            sw.WriteLine("\t" + DictionarySet.BasePathFlag.ToLower() + "=" + dictSet.BasePath);
            sw.WriteLine();
            sw.WriteLine("\t#姓氏词库");
            sw.WriteLine("\t" + DictionarySet.NamePathFlag.ToLower() + "=" + dictSet.NamePath);
            sw.WriteLine();
            sw.WriteLine("\t#数值词库");
            sw.WriteLine("\t" + DictionarySet.NumberPathFlag.ToLower() + "=" + dictSet.NumberPath);
            sw.WriteLine();
            sw.WriteLine("\t#用户词库");
            foreach(string s in dictSet.CustomPaths)
               sw.WriteLine("\t" + DictionarySet.CustomPathFlag.ToLower() + "=" + s);
            sw.WriteLine();
            sw.WriteLine("\t#过滤词库");
            sw.WriteLine("\t" + DictionarySet.FilterPathFlag.ToLower() + "=" + dictSet.FilterPath);
            sw.WriteLine(Config.Suffix);
        }
        #endregion
    }
}
