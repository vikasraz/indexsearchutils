using System;
using System.Collections.Generic;
using ISUtils.Common;
using ISUtils.Database.Writer;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using ISUtils.Async;
using ISUtils.Database;

namespace ISUtils.Utils
{
    public static class IndexUtil
    {
        private static Dictionary<IndexSet, Source> indexDict=new Dictionary<IndexSet,Source>();
        private static IndexerSet indexerSet=new IndexerSet();
        private static DictionarySet dictSet=new DictionarySet();
        private static Analyzer analyzer = new StandardAnalyzer();
        private static bool initSettings = false;

        public static void SetIndexSettings(string configFileName)
        {
            if (initSettings) return;
            initSettings = true;
            try
            {
                List<string> srcList = SupportClass.File.GetFileText(configFileName);
                List<Source> sourceList = Source.GetSourceList(srcList);
                List<IndexSet> indexList = IndexSet.GetIndexList(srcList);
                indexerSet = IndexerSet.GetIndexer(srcList);
                dictSet = DictionarySet.GetDictionarySet(srcList);
                if (indexDict == null)
                    indexDict = new Dictionary<IndexSet, Source>();
                foreach (IndexSet set in indexList)
                {
                    foreach (Source source in sourceList)
                    {
                        if (source.SourceName == set.SourceName)
                        {
                            indexDict.Add(set, source);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
#if DEBUG
                System.Console.WriteLine(e.StackTrace.ToString());
#endif
                throw e;
            }
        }
        public static void SetIndexSettings(List<Source> sourceList, List<IndexSet> indexList, DictionarySet dictSet, IndexerSet indexerSet)
        {
            if (initSettings) return;
            initSettings = true;
            IndexUtil.indexerSet = indexerSet;
            IndexUtil.dictSet = dictSet;
            if (indexDict == null)
                indexDict = new Dictionary<IndexSet, Source>();
            foreach (IndexSet set in indexList)
            {
                foreach (Source source in sourceList)
                {
                    if (source.SourceName == set.SourceName)
                    {
                        indexDict.Add(set, source);
                        break;
                    }
                }
            }
        }
        public static void SetIndexSettings(Dictionary<IndexSet, Source> dict, DictionarySet dictSet, IndexerSet indexerSet)
        {
            if (initSettings) return;
            initSettings = true;
            if (dict != null)
                indexDict = dict;
            else
                indexDict = new Dictionary<IndexSet, Source>();
            IndexUtil.dictSet = dictSet;
            IndexUtil.indexerSet = indexerSet;
        }
        public static void SetAnalyzer(Analyzer analyzer)
        {
            IndexUtil.analyzer = analyzer;
        }
        public static void UseDefaultChineseAnalyzer(bool useChineseAnalyzer)
        {
            if (useChineseAnalyzer)
            {
                ISUtils.CSegment.Segment.SetPaths(dictSet.BasePath, dictSet.NamePath, dictSet.NumberPath,dictSet.FilterPath, dictSet.CustomPaths);
                ISUtils.CSegment.Segment.SetDefaults(new ISUtils.CSegment.DictionaryLoader.TextDictionaryLoader(),new ISUtils.CSegment.ForwardMatchSegment());
                analyzer = new ISUtils.Analysis.Chinese.ChineseAnalyzer();
            }
            else
                analyzer = new StandardAnalyzer();
        }
        public static void Index(IndexTypeEnum type)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (indexSet.Type == type)
                    {
                        IWriter.WriteIndex(analyzer,indexerSet, indexSet, indexDict[indexSet], type == IndexTypeEnum.Ordinary);
                    }
                }
            }
        }
        public static void IndexWithEvent(IndexTypeEnum type,IndexCompletedEventHandler OnIndexCompleted,IndexProgressChangedEventHandler OnProgressChanged)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (indexSet.Type == type)
                    {
                        IWriter.WriteIndexWithEvent(analyzer, indexerSet, indexSet, indexDict[indexSet], type == IndexTypeEnum.Ordinary,OnIndexCompleted,OnProgressChanged);
                    }
                }
            }
        }
        public static void Index(bool create)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    IWriter.WriteIndex(analyzer, indexerSet, indexSet, indexDict[indexSet], create);
                }
            }
        }
        public static void Index(IndexTypeEnum type,ref System.Windows.Forms.ToolStripProgressBar progressBar)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (indexSet.Type == type)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        IWriter.WriteIndex(analyzer, indexerSet, indexSet, indexDict[indexSet], type == IndexTypeEnum.Ordinary,ref progressBar);
                        System.Windows.Forms.Application.DoEvents();
                    }
                }
                System.Windows.Forms.Application.DoEvents();
            }
        }
        public static void Index(IndexTypeEnum type, ref System.Windows.Forms.ProgressBar progressBar)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (indexSet.Type == type)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        IWriter.WriteIndex(analyzer, indexerSet, indexSet, indexDict[indexSet], type == IndexTypeEnum.Ordinary, ref progressBar);
                        System.Windows.Forms.Application.DoEvents();
                    }
                }
                System.Windows.Forms.Application.DoEvents();
            }
        }
    }
}
