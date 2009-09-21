using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Documents;
using ISUtils.Common;
using ISUtils.Database.Writer;
using ISUtils.Async;
using ISUtils.Database;
using ISUtils.File;

namespace ISUtils.Utils
{
    public static class IndexUtil
    {
        #region static private vars
        private static Dictionary<IndexSet, Source> indexDict=new Dictionary<IndexSet,Source>();
        private static FileIndexSet fileSet = new FileIndexSet();
        private static IndexerSet indexerSet=new IndexerSet();
        private static DictionarySet dictSet=new DictionarySet();
        private static Analyzer analyzer = new StandardAnalyzer();
        private static bool initSettings = false;
        #endregion
        #region settings
        public static void SetIndexSettings(string configFileName,bool isXmlFile)
        {
            if (initSettings) return;
            initSettings = true;
            try
            {
                List<Source> sourceList;
                List<IndexSet> indexList;
                if (isXmlFile)
                {
                    Config config = (Config)SupportClass.FileUtil.GetObjectFromXmlFile(configFileName, typeof(Config));
                    sourceList = config.SourceList;
                    indexList = config.IndexList;
                    indexerSet = config.IndexerSet;
                    dictSet = config.DictionarySet;
                    fileSet = config.FileIndexSet;
                }
                else
                {
                    List<string> srcList = SupportClass.FileUtil.GetFileText(configFileName);
                    sourceList = Source.GetSourceList(srcList);
                    indexList = IndexSet.GetIndexList(srcList);
                    indexerSet = IndexerSet.GetIndexer(srcList);
                    dictSet = DictionarySet.GetDictionarySet(srcList);
                }
                if (indexDict == null)
                    indexDict = new Dictionary<IndexSet, Source>();
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
                        if(indexDict.ContainsKey(set)==false)
                            indexDict.Add(set, source);
                        break;
                    }
                }
            }
        }
        public static void SetIndexSettings(List<Source> sourceList, List<IndexSet> indexList, FileIndexSet fileIndexSet, DictionarySet dictSet, IndexerSet indexerSet)
        {
            if (initSettings) return;
            initSettings = true;
            IndexUtil.fileSet = fileIndexSet;
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
                        if (indexDict.ContainsKey(set) == false)
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
        public static void SetIndexSettings(Dictionary<IndexSet, Source> dict, FileIndexSet fileIndexSet, DictionarySet dictSet, IndexerSet indexerSet)
        {
            if (initSettings) return;
            initSettings = true;
            if (dict != null)
                indexDict = dict;
            else
                indexDict = new Dictionary<IndexSet, Source>();
            IndexUtil.dictSet = dictSet;
            IndexUtil.indexerSet = indexerSet;
            IndexUtil.fileSet = fileIndexSet;
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
        #endregion
        #region Index
        #region No Ram,No boost
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
        #endregion
        #region No Ram,Boost
        public static void BoostIndex(IndexTypeEnum type)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (indexSet.Type == IndexTypeEnum.Ordinary)
                    {
                        SupportClass.FileUtil.DeleteFolder(indexSet.Path);
                    }
                    if (indexSet.Type == type)
                    {
                        IWriter.WriteBoostIndex(analyzer, indexerSet, indexSet, indexDict[indexSet], type == IndexTypeEnum.Ordinary);
                    }
                }
            }
        }
        public static void BoostIndex(DataBaseLibrary.SearchUpdateManage dblSum, IndexTypeEnum type)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                //DataBaseLibrary.SearchUpdateManage dblSum = new DataBaseLibrary.SearchUpdateManage();
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (indexSet.Type == IndexTypeEnum.Ordinary)
                    {
                        SupportClass.FileUtil.DeleteFolder(indexSet.Path);
                    }
                    if (indexSet.Type == type)
                    {
                        IWriter.WriteBoostIndex(analyzer, indexerSet, indexSet, indexDict[indexSet], type == IndexTypeEnum.Ordinary);
                    }
                    if (indexSet.Type == IndexTypeEnum.Increment)
                    {
                        string view = indexSet.IndexName;
                        if (view.StartsWith(Config.IndexPrefix))
                            view = view.Substring(Config.IndexPrefix.Length);
                        try
                        {
                            dblSum.Clear(view);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
        }
        public static void BoostIndexWithEvent(IndexSet indexSet, IndexTypeEnum type, IndexCompletedEventHandler OnIndexCompleted, IndexProgressChangedEventHandler OnProgressChanged)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0 && indexDict.ContainsKey(indexSet))
            {
                //DataBaseLibrary.SearchUpdateManage dblSum = new DataBaseLibrary.SearchUpdateManage();
                SupportClass.FileUtil.DeleteFolder(indexSet.Path);
                IWriter.WriteBoostIndexWithEvent(analyzer, indexerSet, indexSet, indexDict[indexSet], type == IndexTypeEnum.Ordinary, OnIndexCompleted, OnProgressChanged);
            }
        }
        public static void BoostIndexWithEvent(DataBaseLibrary.SearchUpdateManage dblSum, IndexTypeEnum type, IndexCompletedEventHandler OnIndexCompleted, IndexProgressChangedEventHandler OnProgressChanged)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                //DataBaseLibrary.SearchUpdateManage dblSum = new DataBaseLibrary.SearchUpdateManage();
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (indexSet.Type == IndexTypeEnum.Ordinary)
                    {
                        SupportClass.FileUtil.DeleteFolder(indexSet.Path);
                    }
                    if (indexSet.Type == type)
                    {
                        IWriter.WriteBoostIndexWithEvent(analyzer, indexerSet, indexSet, indexDict[indexSet], type == IndexTypeEnum.Ordinary, OnIndexCompleted, OnProgressChanged);
                    }
                    if (indexSet.Type == IndexTypeEnum.Increment)
                    {
                        string view = indexSet.IndexName;
                        if (view.StartsWith(Config.IndexPrefix))
                            view = view.Substring(Config.IndexPrefix.Length);
                        try
                        {
                            dblSum.Clear(view);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
        }
        public static void BoostIndex(DataBaseLibrary.SearchUpdateManage dblSum,bool create)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                //DataBaseLibrary.SearchUpdateManage dblSum=new DataBaseLibrary.SearchUpdateManage();
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (create)
                    {
                        SupportClass.FileUtil.DeleteFolder(indexSet.Path);
                    }
                    IWriter.WriteBoostIndex(analyzer, indexerSet, indexSet, indexDict[indexSet], create);
                    if (!create)
                    {
                        string view = indexSet.IndexName;
                        if (view.StartsWith(Config.IndexPrefix))
                            view = view.Substring(Config.IndexPrefix.Length);
                        try
                        {
                            dblSum.Clear(view);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
        }
        public static void BoostIndexWithEvent(IndexTypeEnum type, IndexCompletedEventHandler OnIndexCompleted, IndexProgressChangedEventHandler OnProgressChanged)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (indexSet.Type == IndexTypeEnum.Ordinary)
                    {
                        SupportClass.FileUtil.DeleteFolder(indexSet.Path);
                    }
                    if (indexSet.Type == type)
                    {
                        IWriter.WriteBoostIndexWithEvent(analyzer, indexerSet, indexSet, indexDict[indexSet], type == IndexTypeEnum.Ordinary, OnIndexCompleted, OnProgressChanged);
                    }
                }
            }
        }
        public static void BoostIndex(bool create)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (create)
                    {
                        SupportClass.FileUtil.DeleteFolder(indexSet.Path);
                    }
                    IWriter.WriteBoostIndex(analyzer, indexerSet, indexSet, indexDict[indexSet], create);
                }
            }
        }
        #endregion
        #region Use Ram,No Boost
        public static void IndexEx(IndexTypeEnum type)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (indexSet.Type == type)
                    {
                        RamIWriter.WriteIndex(analyzer, indexerSet, indexSet, indexDict[indexSet], type == IndexTypeEnum.Ordinary);
                    }
                }
            }
        }
        public static void IndexWithEventEx(IndexTypeEnum type, IndexCompletedEventHandler OnIndexCompleted, IndexProgressChangedEventHandler OnProgressChanged)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    if (indexSet.Type == type)
                    {
                        RamIWriter.WriteIndexWithEvent(analyzer, indexerSet, indexSet, indexDict[indexSet], type == IndexTypeEnum.Ordinary, OnIndexCompleted, OnProgressChanged);
                    }
                }
            }
        }
        public static void IndexEx(bool create)
        {
            if (!initSettings)
                throw new ApplicationException("Index Settings not init!");
            if (indexDict.Count > 0)
            {
                foreach (IndexSet indexSet in indexDict.Keys)
                {
                    RamIWriter.WriteIndex(analyzer, indexerSet, indexSet, indexDict[indexSet], create);
                }
            }
        }
        #endregion
        #region File Index
        public static bool IndexFile(bool create)
        {
            if (string.IsNullOrEmpty(fileSet.Path))
                return true;
            try
            {
                if (create)
                {
                    SupportClass.FileUtil.DeleteFolder(fileSet.Path);
                }
                IndexWriter writer = new IndexWriter(fileSet.Path, analyzer, create);
                writer.SetMaxFieldLength(indexerSet.MaxFieldLength);
                writer.SetRAMBufferSizeMB(indexerSet.RamBufferSize);
                writer.SetMergeFactor(indexerSet.MergeFactor);
                writer.SetMaxBufferedDocs(indexerSet.MaxBufferedDocs);
                foreach (string dir in fileSet.BaseDirs)
                {
                    FileIndexer.IndexDir(writer, dir);
                }
                writer.Optimize();
                writer.Close();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static bool IndexFile(bool create, IndexCompletedEventHandler OnIndexCompleted, IndexProgressChangedEventHandler OnProgressChanged)
        {
            try
            {
                if (create)
                {
                    SupportClass.FileUtil.DeleteFolder(fileSet.Path);
                }
                IndexWriter writer = new IndexWriter(fileSet.Path, analyzer, create);
                writer.SetMaxFieldLength(indexerSet.MaxFieldLength);
                writer.SetRAMBufferSizeMB(indexerSet.RamBufferSize);
                writer.SetMergeFactor(indexerSet.MergeFactor);
                writer.SetMaxBufferedDocs(indexerSet.MaxBufferedDocs);
                int i=0;
                foreach (string dir in fileSet.BaseDirs)
                {
                    i++;
                    FileIndexer.IndexDir(writer, dir,OnProgressChanged);
                    OnProgressChanged("IndexUtil", new IndexProgressChangedEventArgs(fileSet.BaseDirs.Count, i));
                }
                writer.Optimize();
                writer.Close();
                OnIndexCompleted("IndexUtil", new IndexCompletedEventArgs("File"));
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #endregion
    }
}
