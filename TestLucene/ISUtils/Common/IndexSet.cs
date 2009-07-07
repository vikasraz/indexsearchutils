using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public class IndexSet
    {
        #region  FLAGS
        /**/
        /// <summary>
        /// 索引类型的标志
        /// </summary>
        public const string TypeFlag = "TYPE";
        /**/
        /// <summary>
        /// 索引类型的标志
        /// </summary>
        public const string CaptionFlag = "CAPTION";
        /**/
        /// <summary>
        /// 索引路径的标志
        /// </summary>
        public const string PathFlag = "PATH";
        #endregion
        #region Property
        /**/
        /// <summary>
        /// 存储索引名称
        /// </summary>
        private string indexname = "";
        /**/
        /// <summary>
        /// 设定或返回索引名称
        /// </summary>
        public string IndexName
        {
            get { return indexname;}
            set { indexname =value;}
        }
        private string caption = "";
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }
        /**/
        /// <summary>
        /// 存储索引类型
        /// </summary>
        private IndexTypeEnum type = IndexTypeEnum.Ordinary;
        /**/
        /// <summary>
        /// 设定或返回索引类型
        /// </summary>
        public IndexTypeEnum Type
        {
            get { return type; }
            set { type = value; }
        }
        /**/
        /// <summary>
        /// 存储索引源名称
        /// </summary>
        private string sourcename = "";
        /**/
        /// <summary>
        /// 设定或返回索引源名称
        /// </summary>
        public string SourceName
        {
            get { return sourcename; }
            set { sourcename = value; }
        }
        /**/
        /// <summary>
        /// 存储索引路径
        /// </summary>
        private string path = "";
        /**/
        /// <summary>
        /// 设定或返回索引路径
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        #endregion
        #region Override
        /**/
        /// <summary>
        /// 获取Index内容
        /// </summary>
        /// <returns>返回类型</returns>
        public override string ToString()
        {
            string ret = string.Format("Index[{0}]:Source({1}),Type({2}),Path({3})",indexname, sourcename, IndexType.GetIndexTypeStr(type),path);
            return base.ToString()+"\t"+ret;
        }
        #endregion
        #region Static Func
        /**/
        /// <summary>
        /// 获取字符串列表中的索引列表
        /// </summary>
        /// <param name="srcList">字符串列表</param>
        /// <returns>返回类型</returns>
        public static List<IndexSet> GetIndexList(List<string> srcList)
        {
            List<IndexSet> indexList = new List<IndexSet>();
            bool findIndex = false, indexStart = false;
            IndexSet index = new IndexSet();
            foreach (string s in srcList)
            {
                if (SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s),Config.IndexFlag))
                {
                    findIndex = true;
                    index = new IndexSet();
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, " \t"); 
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length < 2)
                        throw new ApplicationException("No index name in this index");
                    index.IndexName = split[1];
                    continue;
                }
                if (findIndex && SupportClass.String.FormatStr(s).StartsWith(Config.Prefix))
                {
                    indexStart = true;
                    continue;
                }
                if (findIndex && indexStart && SupportClass.String.FormatStr(s).StartsWith(Config.Suffix))
                {
                    findIndex = false;
                    indexStart = false;
                    if (string.IsNullOrEmpty(index.IndexName) == false)
                        indexList.Add(index );
                    continue;
                }
                if (SupportClass.String.FormatStr(s).StartsWith(Config.Ignore))
                    continue;
                if (findIndex && indexStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s),IndexSet.TypeFlag))
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
                            index.Type=IndexType.GetIndexType(split[1]);
                        }
                        catch(Exception e)
                        {
                            index.Type=IndexTypeEnum.Ordinary;
#if DEBUG
                            System.Console.WriteLine(e.StackTrace.ToString());
#endif
                        }
                    else
                        index.Type = IndexTypeEnum.Ordinary;
                    continue;
                }
                if (findIndex && indexStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s),IndexSet.PathFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        index.Path = split[1];
                    else
                        index .Path= "";
                    continue;
                }
                if (findIndex && indexStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Config.SourceFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        index.SourceName = split[1];
                    else
                        index.SourceName = "";
                    continue;
                }
                if (findIndex && indexStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), IndexSet.CaptionFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        index.Caption = split[1];
                    else
                        index.Caption = "";
                    continue;
                }
            }
            return indexList;
        }
        public static void WriteToFile(ref System.IO.StreamWriter sw, IndexSet indexSet)
        {
            sw.WriteLine("#############################################");
            sw.WriteLine(string.Format("#index {0}", indexSet.IndexName));
            sw.WriteLine("#############################################");
            sw.WriteLine(Config.IndexFlag.ToLower() + " " + indexSet.IndexName);
            sw.WriteLine(Config.Prefix);
            sw.WriteLine("\t#索引源名");
            sw.WriteLine("\t" + Config.SourceFlag.ToLower() + "=" + indexSet.SourceName);
            sw.WriteLine();
            sw.WriteLine("\t#索引类型");
            sw.WriteLine("\t#" + IndexSet.TypeFlag.ToLower() + "=" + IndexType.GetIndexTypeStr(IndexTypeEnum.Increment));
            sw.WriteLine("\t#" + IndexSet.TypeFlag.ToLower() + "=" + IndexType.GetIndexTypeStr(IndexTypeEnum.Ordinary));
            sw.WriteLine("\t" + IndexSet.TypeFlag.ToLower() + "=" + IndexType.GetIndexTypeStr(indexSet.Type));
            sw.WriteLine();
            sw.WriteLine("\t#索引存储路径");
            sw.WriteLine("\t" + IndexSet.PathFlag.ToLower() + "=" + indexSet.Path);
            sw.WriteLine(Config.Suffix);
        }
        #endregion
    }
}
