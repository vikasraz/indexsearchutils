using System;
using System.Collections.Generic;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using ISUtils.Common;
using ISUtils.Analysis.Chinese;
using ISUtils.Database.Writer;
using ISUtils.Database.Indexer;
using ISUtils.Async;

namespace ISUtils.Database
{
    public class RamIWriter
    {
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
                    DBRamCreateIndexer dbcIndexer = new DBRamCreateIndexer(analyzer, source.DBType, connect, index.Path,index.Caption);
                    start = DateTime.Now;
                    dbcIndexer.WriteResults(source.Query,indexer.MaxFieldLength,indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs);
                    return DateTime.Now - start;
                }
                else
                {
                    DBRamIncremIndexer dbiIndexer = new DBRamIncremIndexer(analyzer, source.DBType, connect, index.Path,index.Caption);
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
                    DBRamCreateIndexer dbcIndexer = new DBRamCreateIndexer(analyzer, source.DBType, connect, index.Path,index.Caption);
                    dbcIndexer.OnIndexCompleted += OnIndexCompleted;
                    dbcIndexer.OnProgressChanged += OnProgressChanged;
                    start = DateTime.Now;
                    dbcIndexer.WriteResultsWithEvent(source.Query, indexer.MaxFieldLength, indexer.RamBufferSize, indexer.MergeFactor, indexer.MaxBufferedDocs);
                    return DateTime.Now - start;
                }
                else
                {
                    DBRamIncremIndexer dbiIndexer = new DBRamIncremIndexer(analyzer, source.DBType, connect, index.Path,index.Caption);
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
    }
}

