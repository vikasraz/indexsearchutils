using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Documents;
using ISUtils.Common;
using ISUtils.File.IFilter;

namespace ISUtils.File
{
    public static class FileIndexer
    {
        #region Internal Function
        internal static FileContent GetFileContent(string filepath)
        {
            FileContent fc = new FileContent();
            fc.Path = filepath;
            try
            {
                TextReader reader = new FilterReader(filepath);
                using (reader)
                {
                    fc.Content = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                if (SupportClass.File.IsTextFile(filepath))
                {
                    fc.Content = SupportClass.File.ReadTextFile(filepath);
                }
            }
            return fc;
        }
        internal static void IndexFile(IndexWriter writer, string filepath)
        {
#if DEBUG
            System.Console.WriteLine(SupportClass.Time.GetDateTime()+"\t"+filepath);
#endif
            FileContent fc = GetFileContent(filepath);
            Document doc = new Document();
            doc.Add(new Field("Name", fc.Name, Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
            doc.Add(new Field("Path", fc.Path, Field.Store.COMPRESS, Field.Index.NO));
            doc.Add(new Field("Content", fc.Content, Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
            writer.AddDocument(doc);
        }
        internal static void IndexDir(IndexWriter writer, string dir)
        {
            List<string> fileList = SupportClass.File.GetDirFiles(dir, string.Empty);
            foreach (string file in fileList)
            {
                try
                {
                    IndexFile(writer, file);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
        #endregion
        #region Static Public Function
        public static bool Index(Analyzer analyzer, string savepath,string dir, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs,bool create)
        {
            try
            {
                IndexWriter writer = new IndexWriter(savepath, analyzer, create);
                writer.SetMaxFieldLength(maxFieldLength);
                writer.SetRAMBufferSizeMB(ramBufferSize);
                writer.SetMergeFactor(mergeFactor);
                writer.SetMaxBufferedDocs(maxBufferedDocs);
                IndexDir(writer, dir);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static bool Index(Analyzer analyzer, FileIndexSet set, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs, bool create)
        {
            try
            {
                IndexWriter writer = new IndexWriter(set.Path, analyzer, create);
                writer.SetMaxFieldLength(maxFieldLength);
                writer.SetRAMBufferSizeMB(ramBufferSize);
                writer.SetMergeFactor(mergeFactor);
                writer.SetMaxBufferedDocs(maxBufferedDocs);
                foreach (string dir in set.BaseDirs)
                {
                    IndexDir(writer, dir);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static bool Index(Analyzer analyzer, FileIndexSet fileIndex,IndexerSet indexer, bool create)
        {
            try
            {
                IndexWriter writer = new IndexWriter(fileIndex.Path, analyzer, create);
                writer.SetMaxFieldLength(indexer.MaxFieldLength);
                writer.SetRAMBufferSizeMB(indexer.RamBufferSize);
                writer.SetMergeFactor(indexer.MergeFactor);
                writer.SetMaxBufferedDocs(indexer.MaxBufferedDocs);
                foreach (string dir in fileIndex.BaseDirs)
                {
                    IndexDir(writer, dir);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion
    }
}
