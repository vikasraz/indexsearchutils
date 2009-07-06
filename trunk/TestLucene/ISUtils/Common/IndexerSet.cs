using System;
using System.Collections.Generic;
using System.Text;

namespace ISUtils.Common
{
    [Serializable]
    public class IndexerSet
    {
        #region Flags
        /**/
        /// <summary>
        /// 字段最大长度
        /// </summary>
        public const string MaxFieldLengthFlag = "MAX_FIELD_LENGTH";
        /**/
        /// <summary>
        /// 内存最大使用量
        /// </summary>
        public const string RamBufferSizeFlag = "RAM_BUFFER_SIZE";
        /**/
        /// <summary>
        /// 主索引重建时间的标志
        /// </summary>
        public const string MainIndexReCreateTimeFlag = "MAIN_CREATE_TIME";
        /**/
        /// <summary>
        /// 主索引重建间隔的标志
        /// </summary>
        public const string MainIndexReCreateSpanFlag = "MAIN_TIME_SPAN";
        /**/
        /// <summary>
        /// 增量索引重建间隔的标志
        /// </summary>
        public const string IncrIndexReCreateSpanFlag = "INCR_TIME_SPAN";
        /**/
        /// <summary>
        /// 合并因子的标志
        /// </summary>
        public const string IndexerMergeFactorFlag = "MERGE_FACTOR";
        /**/
        /// <summary>
        ///文档内存最大存储数的标志
        /// </summary>
        public const string IndexerMaxBufferedDocsFlag = "MAX_BUFFERED_DOCS";
        #endregion
        #region Property
        /**/
        /// <summary>
        /// 存储主索引重建时间
        /// </summary>
        private int maxfieldlength = int.MaxValue;
        /**/
        /// <summary>
        /// 设定或返回主索引重建时间
        /// </summary>
        public int MaxFieldLength
        {
            get { return maxfieldlength; }
            set { maxfieldlength = value; }
        }
        /**/
        /// <summary>
        /// 存储主索引重建时间
        /// </summary>
        private double rambuffersize = 512;
        /**/
        /// <summary>
        /// 设定或返回主索引重建时间
        /// </summary>
        public double RamBufferSize
        {
            get { return rambuffersize; }
            set { rambuffersize = value; }
        }
        /**/
        /// <summary>
        /// 存储主索引重建时间
        /// </summary>
        private DateTime maincreate = DateTime.Parse("23:30:00");
        /**/
        /// <summary>
        /// 设定或返回主索引重建时间
        /// </summary>
        public DateTime MainIndexReCreateTime
        {
            get { return maincreate; }
            set { maincreate = value; }
        }
        /**/
        /// <summary>
        /// 存储主索引重建间隔
        /// </summary>
        private int maintmspan = 1;//1 day
        /**/
        /// <summary>
        /// 设定或返回主索引重建间隔
        /// </summary>
        public int MainIndexReCreateTimeSpan
        {
            get { return maintmspan; }
            set { maintmspan = value; }
        }
        /**/
        /// <summary>
        /// 存储增量索引重建间隔
        /// </summary>
        private int incrtmspan = 120;//120 seconds
        /**/
        /// <summary>
        /// 设定或返回增量索引重建间隔
        /// </summary>
        public int IncrIndexReCreateTimeSpan
        {
            get { return incrtmspan; }
            set { incrtmspan = value; }
        }
        /**/
        /// <summary>
        /// 存储合并因子
        /// </summary>
        private int mergeFactor = 10000;
        /**/
        /// <summary>
        /// 设定或返回合并因子
        /// </summary>
        public int MergeFactor
        {
            get { return mergeFactor; }
            set { mergeFactor = value; }
        }
        /**/
        /// <summary>
        /// 存储文档内存最大存储数
        /// </summary>
        private int maxBufferedDocs = 10000;
        /**/
        /// <summary>
        /// 设定或返回文档内存最大存储数
        /// </summary>
        public int MaxBufferedDocs
        {
            get { return maxBufferedDocs; }
            set { maxBufferedDocs = value; }
        }
        #endregion
        #region Override
        /**/
        /// <summary>
        /// 获取Indexer内容
        /// </summary>
        /// <returns>返回类型</returns>
        public override string ToString()
        {
            string ret = string.Format("Indexer:MainReCreateTime({0}),MainReCreateTimeSpan({1}),IncrReCreateTimeSpan({2}),MergerFactor({3}),MaxBufferedDocs({4})",
                                     maincreate.ToShortTimeString(), maintmspan.ToString(), incrtmspan.ToString(),
                                     mergeFactor, maxBufferedDocs);
            return base.ToString()+"\t"+ret;
        }
        #endregion
        #region Static Func
        /**/
        /// <summary>
        /// 获取字符串列表中的索引器
        /// </summary>
        /// <param name="srcList">字符串列表</param>
        /// <returns>返回类型</returns>
        public static IndexerSet GetIndexer(List<string> srcList)
        {
            IndexerSet indexer = new IndexerSet();
            bool findIndexer = false, indexerStart = false;
            foreach (string s in srcList)
            {
                if (SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), Config.IndexerFlag))
                {
                    findIndexer = true;
                    continue;
                }
                if (findIndexer && SupportClass.String.FormatStr(s).StartsWith(Config.Prefix))
                {
                    indexerStart = true;
                    continue;
                }
                if (findIndexer && indexerStart && SupportClass.String.FormatStr(s).StartsWith(Config.Suffix))
                {
                    findIndexer = false;
                    indexerStart = false;
                    break;
                }
                if (SupportClass.String.FormatStr(s).StartsWith(Config.Ignore))
                    continue;
                if (findIndexer && indexerStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), IndexerSet.IncrIndexReCreateSpanFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        indexer.IncrIndexReCreateTimeSpan = int.Parse(split[1]);
                    continue;
                }
                if (findIndexer && indexerStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), IndexerSet.IndexerMaxBufferedDocsFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        indexer.MaxBufferedDocs = int.Parse(split[1]);
                    continue;
                }
                if (findIndexer && indexerStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), IndexerSet.IndexerMergeFactorFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        indexer.MergeFactor = int.Parse(split[1]);
                    continue;
                }
                if (findIndexer && indexerStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), IndexerSet.MainIndexReCreateSpanFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        indexer.MainIndexReCreateTimeSpan = int.Parse(split[1]);
                    continue;
                }
                if (findIndexer && indexerStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), IndexerSet.MainIndexReCreateTimeFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        indexer.MainIndexReCreateTime = DateTime.Parse(split[1]);
                    continue;
                }
                if (findIndexer && indexerStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), IndexerSet.MaxFieldLengthFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        indexer.MaxFieldLength = int.Parse(split[1]);
                    continue;
                }
                if (findIndexer && indexerStart && SupportClass.String.StartsWithNoCase(SupportClass.String.FormatStr(s), IndexerSet.RamBufferSizeFlag))
                {
                    string format = SupportClass.String.FormatStr(s);
                    string[] split = SupportClass.String.Split(format, Config.Devider);
#if DEBUG
                    Console.WriteLine(format);
                    foreach (string a in split)
                        Console.WriteLine(a);
#endif
                    if (split.Length >= 2)
                        indexer.RamBufferSize = double.Parse(split[1]);
                    continue;
                }
            }
            return indexer;
        }
        public static void WriteToFile(ref System.IO.StreamWriter sw, IndexerSet indexerSet)
        {
            sw.WriteLine("#############################################");
            sw.WriteLine("#indexer settings");
            sw.WriteLine("#############################################");
            sw.WriteLine(Config.IndexerFlag.ToLower() );
            sw.WriteLine(Config.Prefix);
            sw.WriteLine("\t#主索引创建时间");
            sw.WriteLine("\t" + IndexerSet.MainIndexReCreateTimeFlag.ToLower() + "=" + indexerSet.MainIndexReCreateTime.ToShortTimeString());
            sw.WriteLine();
            sw.WriteLine("\t#主索引创建间隔");
            sw.WriteLine("\t" + IndexerSet.MainIndexReCreateSpanFlag.ToLower() + "=" + indexerSet.MainIndexReCreateTimeSpan.ToString());
            sw.WriteLine();
            sw.WriteLine("\t#增量索引创建间隔");
            sw.WriteLine("\t" + IndexerSet.IncrIndexReCreateSpanFlag.ToLower() + "=" + indexerSet.IncrIndexReCreateTimeSpan.ToString());
            sw.WriteLine();
            sw.WriteLine("\t#字段最大长度");
            sw.WriteLine("\t" + IndexerSet.MaxFieldLengthFlag.ToLower() + "=" + indexerSet.MaxFieldLength.ToString());
            sw.WriteLine();
            sw.WriteLine("\t#内存最大使用量");
            sw.WriteLine("\t" + IndexerSet.RamBufferSizeFlag.ToLower() + "=" + indexerSet.RamBufferSize.ToString());
            sw.WriteLine();
            sw.WriteLine("\t#合并因子");
            sw.WriteLine("\t" + IndexerSet.IndexerMergeFactorFlag.ToLower() + "=" + indexerSet.MergeFactor.ToString());
            sw.WriteLine();
            sw.WriteLine("\t#内存文档最大存储数");
            sw.WriteLine("\t" + IndexerSet.IndexerMaxBufferedDocsFlag.ToLower() + "=" + indexerSet.MaxBufferedDocs.ToString());
            sw.WriteLine(Config.Suffix);
        }
        #endregion
    }
}
