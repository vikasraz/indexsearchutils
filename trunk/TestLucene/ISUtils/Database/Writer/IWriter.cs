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
        public static TimeSpan WriteIndex(Analyzer analyzer, IndexerSet indexer, IndexSet index, Source source, bool create,ref System.Windows.Forms.ToolStripProgressBar progressBar)
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
    }
}

