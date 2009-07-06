using System;
using System.Collections.Generic;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using ISUtils.Common;
using ISUtils.Analysis.Chinese;
using ISUtils.Database;
using ISUtils.Database.Indexer;

namespace ISUtils.Database.Writer
{
    public class IWriter
    {
        #region No Boost Index
        public static TimeSpan WriteIndex(Analyzer analyzer,IndexerSet indexer, IndexSet index, Source source,bool create)
        {
            try
            {
                //ChineseSegAnalysis csa = new ChineseSegAnalysis(index.BasePath, index.NamePath, index.NumberPath, index.CustomPaths);
                //csa.FilterFilePath = index.FilterPath;
                //Analyzer analyzer = csa.GetAnalyzer();
                string connect = source.GetConnString();
                DateTime start;
                if (create)
                {
                    DBCreateIndexer dbcIndexer = new DBCreateIndexer(analyzer, source.DBType, connect, index.Path);
                    start = DateTime.Now;
                    dbcIndexer.WriteResults(source.Query,indexer.MaxFieldLength,indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs);
                    return DateTime.Now - start;
                }
                else
                {
                    DBIncremIndexer dbiIndexer = new DBIncremIndexer(analyzer, source.DBType, connect, index.Path);
                    start = DateTime.Now;
                    dbiIndexer.WriteResults(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs);                 
                    return DateTime.Now - start;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static TimeSpan WriteIndexWithEvent(Analyzer analyzer, IndexerSet indexer, IndexSet index, Source source, bool create,
                                                IndexCompletedEventHandler OnIndexCompleted,
                                                IndexProgressChangedEventHandler OnProgressChanged)
        {
            try
            {
                //ChineseSegAnalysis csa = new ChineseSegAnalysis(index.BasePath, index.NamePath, index.NumberPath, index.CustomPaths);
                //csa.FilterFilePath = index.FilterPath;
                //Analyzer analyzer = csa.GetAnalyzer();
                string connect = source.GetConnString();
                DateTime start;
                if (create)
                {
                    DBCreateIndexer dbcIndexer = new DBCreateIndexer(analyzer, source.DBType, connect, index.Path);
                    dbcIndexer.OnIndexCompleted += OnIndexCompleted;
                    dbcIndexer.OnProgressChanged += OnProgressChanged;
                    start = DateTime.Now;
                    dbcIndexer.WriteResultsWithEvent(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs);
                    return DateTime.Now - start;
                }
                else
                {
                    DBIncremIndexer dbiIndexer = new DBIncremIndexer(analyzer, source.DBType, connect, index.Path);
                    dbiIndexer.OnIndexCompleted += OnIndexCompleted;
                    dbiIndexer.OnProgressChanged += OnProgressChanged;
                    start = DateTime.Now;
                    dbiIndexer.WriteResultsWithEvent(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs);
                    return DateTime.Now - start;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #region Boost Index
        public static TimeSpan WriteBoostIndex(Analyzer analyzer, IndexerSet indexer, IndexSet index, Source source, bool create)
        {
            try
            {
                //ChineseSegAnalysis csa = new ChineseSegAnalysis(index.BasePath, index.NamePath, index.NumberPath, index.CustomPaths);
                //csa.FilterFilePath = index.FilterPath;
                //Analyzer analyzer = csa.GetAnalyzer();
                string connect = source.GetConnString();
                DateTime start;
                if (create)
                {
                    DBCreateIndexer dbcIndexer = new DBCreateIndexer(analyzer, source.DBType, connect, index.Path);
                    start = DateTime.Now;
                    dbcIndexer.WriteResults(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs,source.FieldBoostDict);
                    return DateTime.Now - start;
                }
                else
                {
                    DBIncremIndexer dbiIndexer = new DBIncremIndexer(analyzer, source.DBType, connect, index.Path);
                    start = DateTime.Now;
                    dbiIndexer.WriteResults(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs,source.FieldBoostDict);
                    return DateTime.Now - start;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static TimeSpan WriteBoostIndexWithEvent(Analyzer analyzer, IndexerSet indexer, IndexSet index, Source source, bool create,
                                                IndexCompletedEventHandler OnIndexCompleted,
                                                IndexProgressChangedEventHandler OnProgressChanged)
        {
            try
            {
                //ChineseSegAnalysis csa = new ChineseSegAnalysis(index.BasePath, index.NamePath, index.NumberPath, index.CustomPaths);
                //csa.FilterFilePath = index.FilterPath;
                //Analyzer analyzer = csa.GetAnalyzer();
                string connect = source.GetConnString();
                DateTime start;
                if (create)
                {
                    DBCreateIndexer dbcIndexer = new DBCreateIndexer(analyzer, source.DBType, connect, index.Path);
                    dbcIndexer.OnIndexCompleted += OnIndexCompleted;
                    dbcIndexer.OnProgressChanged += OnProgressChanged;
                    start = DateTime.Now;
                    dbcIndexer.WriteResultsWithEvent(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs,source.FieldBoostDict);
                    return DateTime.Now - start;
                }
                else
                {
                    DBIncremIndexer dbiIndexer = new DBIncremIndexer(analyzer, source.DBType, connect, index.Path);
                    dbiIndexer.OnIndexCompleted += OnIndexCompleted;
                    dbiIndexer.OnProgressChanged += OnProgressChanged;
                    start = DateTime.Now;
                    dbiIndexer.WriteResultsWithEvent(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs,source.FieldBoostDict);
                    return DateTime.Now - start;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #region With Windows Contrl
        public static TimeSpan WriteIndex(Analyzer analyzer, IndexerSet indexer, IndexSet index, Source source, bool create, ref System.Windows.Forms.ToolStripProgressBar progressBar)
        {
            try
            {
                //ChineseSegAnalysis csa = new ChineseSegAnalysis(index.BasePath, index.NamePath, index.NumberPath, index.CustomPaths);
                //csa.FilterFilePath = index.FilterPath;
                //Analyzer analyzer = csa.GetAnalyzer();
                string connect = source.GetConnString();
                DateTime start;
                if (create)
                {
                    DBCreateIndexer dbcIndexer = new DBCreateIndexer(analyzer, source.DBType, connect, index.Path);
                    start = DateTime.Now;
                    dbcIndexer.WriteResults(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs,ref progressBar);
                    return DateTime.Now - start;
                }
                else
                {
                    DBIncremIndexer dbiIndexer = new DBIncremIndexer(analyzer, source.DBType, connect, index.Path);
                    start = DateTime.Now;
                    dbiIndexer.WriteResults(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs,ref progressBar);
                    return DateTime.Now - start;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static TimeSpan WriteIndex(Analyzer analyzer, IndexerSet indexer, IndexSet index, Source source, bool create, ref System.Windows.Forms.ProgressBar progressBar)
        {
            try
            {
                //ChineseSegAnalysis csa = new ChineseSegAnalysis(index.BasePath, index.NamePath, index.NumberPath, index.CustomPaths);
                //csa.FilterFilePath = index.FilterPath;
                //Analyzer analyzer = csa.GetAnalyzer();
                string connect = source.GetConnString();
                DateTime start;
                if (create)
                {
                    DBCreateIndexer dbcIndexer = new DBCreateIndexer(analyzer, source.DBType, connect, index.Path);
                    start = DateTime.Now;
                    dbcIndexer.WriteResults(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs, ref progressBar);
                    return DateTime.Now - start;
                }
                else
                {
                    DBIncremIndexer dbiIndexer = new DBIncremIndexer(analyzer, source.DBType, connect, index.Path);
                    start = DateTime.Now;
                    dbiIndexer.WriteResults(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs, ref progressBar);
                    return DateTime.Now - start;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}

