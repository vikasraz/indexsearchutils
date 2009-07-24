using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;
using System.Data;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Documents;
using ISUtils.Async;

namespace ISUtils.Database.Writer
{
    /**/
    /// <summary>
    /// 数据库的增量索引写入器
    /// </summary>
    public class DBIncremIWriter : DbWriterBase, DataBaseWriter
    {
        #region Constructor
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="analyzer">分析器</param>
        /// <param name="directory">索引存储路径</param>
        /// <param name="create">创建索引还是增量索引</param>
        public DBIncremIWriter(Analyzer analyzer, string directory, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs)
            : base(directory)
        {
            Lucene.Net.Store.Directory dict;
            document=new Document();
            fieldDict=new Dictionary<string,Field>();
            try
            {
                dict = FSDirectory.GetDirectory(directory, false);
            }
            catch (System.IO.IOException ioe)
            {
                dict = FSDirectory.GetDirectory(directory, true);
#if DEBUG
                System.Console.WriteLine(ioe.StackTrace.ToString());
#endif
            }
            try
            {
                fsWriter = new IndexWriter(dict, analyzer, false);
                fsWriter.SetMaxFieldLength(maxFieldLength);
                fsWriter.SetRAMBufferSizeMB(ramBufferSize);
                fsWriter.SetMergeFactor(mergeFactor);
                fsWriter.SetMaxBufferedDocs(maxBufferedDocs);
            }
            catch (System.IO.IOException ie)
            {
                fsWriter = new IndexWriter(dict, analyzer, true);
                fsWriter.SetMaxFieldLength(maxFieldLength);
                fsWriter.SetRAMBufferSizeMB(ramBufferSize);
                fsWriter.SetMergeFactor(mergeFactor);
                fsWriter.SetMaxBufferedDocs(maxBufferedDocs);
#if DEBUG
                System.Console.WriteLine(ie.StackTrace.ToString());
#endif
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /**/
        /// <summary>
        /// 析构函数
        ~DBIncremIWriter()
        {
            if (fsWriter !=null)
               fsWriter.Close();
       }
        #endregion
        #region Override
       /**/
        /// <summary>
        /// 设定基本属性值
        /// </summary>
        /// <param name="analyzer">分析器</param>
        /// <param name="directory">索引存储路径</param>
        /// <param name="create">创建索引还是增量索引</param>
        public override void SetBasicProperties(Analyzer analyzer, string directory, bool create)
        {
            if (fsWriter != null)
            {
                fsWriter.Flush();
            }
            Lucene.Net.Store.Directory dict = FSDirectory.GetDirectory(directory, false);
            try
            {
                fsWriter = new IndexWriter(dict, analyzer, false);
                fsWriter.SetMaxFieldLength(int.MaxValue);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /**/
        /// <summary>
        /// 设置优化参数值
        /// </summary>
        /// <param name="mergeFactor">合并因子 (mergeFactor)</param>
        /// <param name="maxBufferedDocs">文档内存最大存储数</param>
        public override void SetOptimProperties(int mergeFactor, int maxBufferedDocs)
        {
            if (fsWriter == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            fsWriter.SetMergeFactor(mergeFactor);
            fsWriter.SetMaxBufferedDocs(maxBufferedDocs);
        }
        /**/
        /// <summary>
        /// 对数据库表进行索引
        /// </summary>
        /// <param name="table">数据库表名</param>
        public override void WriteDataTable(DataTable table)
        {
            if (fsWriter == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            if (document == null)
                document = new Document();
            this.isBusy = true;
            RowNum = table.Rows.Count;
            Percent = RowNum / SupportClass.PERCENTAGEDIVE+1;
            DataColumnCollection columns = table.Columns;
            foreach (DataColumn column in columns)
            {
                if (fieldDict.ContainsKey(column.ColumnName))
                    continue;
                fieldDict.Add(column.ColumnName, new Field(column.ColumnName, "value", Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
#if DEBUG
            DateTime start = DateTime.Now;
#endif
            DeleteIndex(table);
            WriteDataRowCollectionWithNoEvent(table.Rows);
#if DEBUG
            TimeSpan span=DateTime.Now -start;
            System.Console.WriteLine(string.Format("Speed:{0}ms/line",span.TotalMilliseconds/table.Rows.Count));
#endif
            WriteTableCompletedEventArgs args = new WriteTableCompletedEventArgs(table.TableName);
            base.OnWriteTableCompletedEvent(this, args);
            this.isBusy = false;
        }
        /**/
        /// <summary>
        /// 对数据库表进行索引
        /// </summary>
        /// <param name="table">数据库表名</param>
        public override void WriteDataTable(DataTable table, Dictionary<string, float> fieldsBoostDict)
        {
            if (fsWriter == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            if (fieldsBoostDict == null)
                throw new ArgumentNullException("fieldsBoostDict", "fieldsBoostDict is not valid.");
            if (document == null)
                document = new Document();
            this.isBusy = true;
            RowNum = table.Rows.Count;
            Percent = RowNum / SupportClass.PERCENTAGEDIVE + 1;
            DataColumnCollection columns = table.Columns;
            foreach (DataColumn column in columns)
            {
                Field field = new Field(column.ColumnName, "value", Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                if (fieldsBoostDict.ContainsKey(column.ColumnName))
                    field.SetBoost(fieldsBoostDict[column.ColumnName]);
                if (fieldDict.ContainsKey(column.ColumnName))
                    continue;
                fieldDict.Add(column.ColumnName, field);
            }
#if DEBUG
            DateTime start = DateTime.Now;
#endif
            DeleteIndex(table);
            WriteDataRowCollectionWithNoEvent(table.Rows);
#if DEBUG
            TimeSpan span = DateTime.Now - start;
            System.Console.WriteLine(string.Format("Speed:{0}ms/line", span.TotalMilliseconds / table.Rows.Count));
#endif
            WriteTableCompletedEventArgs args = new WriteTableCompletedEventArgs(table.TableName);
            base.OnWriteTableCompletedEvent(this, args);
            this.isBusy = false;
        }
        /**/
        /// <summary>
        /// 对数据库表进行索引
        /// </summary>
        /// <param name="table">数据库表名</param>
        public override void WriteDataTableWithEvent(DataTable table, Dictionary<string, float> fieldsBoostDict)
        {
            if (fsWriter == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            if (fieldsBoostDict == null)
                throw new ArgumentNullException("fieldsBoostDict", "fieldsBoostDict is not valid.");
            if (document == null)
                document = new Document();
            this.isBusy = true;
            RowNum = table.Rows.Count;
            Percent = RowNum / SupportClass.PERCENTAGEDIVE + 1;
            DataColumnCollection columns = table.Columns;
            foreach (DataColumn column in columns)
            {
                if (column.Unique)
                {
                    primaryKey = column.ColumnName;
                }
                Field field = new Field(column.ColumnName, "value", Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                if (fieldsBoostDict.ContainsKey(column.ColumnName))
                    field.SetBoost(fieldsBoostDict[column.ColumnName]);
                if (fieldDict.ContainsKey(column.ColumnName))
                    continue;
                fieldDict.Add(column.ColumnName, field);
            }
#if DEBUG
            DateTime start = DateTime.Now;
#endif
            DeleteIndex(table);
            WriteDataRowCollection(table.Rows);
#if DEBUG
            TimeSpan span = DateTime.Now - start;
            System.Console.WriteLine(string.Format("Speed:{0}ms/line", span.TotalMilliseconds / table.Rows.Count));
#endif
            WriteTableCompletedEventArgs args = new WriteTableCompletedEventArgs(table.TableName);
            base.OnWriteTableCompletedEvent(this, args);
            this.isBusy = false;
        }
        /**/
        /// <summary>
        /// 对数据库表进行索引
        /// </summary>
        /// <param name="table">数据库表名</param>
        public override void WriteDataTableWithEvent(DataTable table)
        {
            if (fsWriter == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            if (document == null)
                document = new Document();
            this.isBusy = true;
            RowNum = table.Rows.Count;
            Percent = RowNum / SupportClass.PERCENTAGEDIVE + 1;
            DataColumnCollection columns = table.Columns;
            foreach (DataColumn column in columns)
            {
                if (fieldDict.ContainsKey(column.ColumnName))
                    continue;
                fieldDict.Add(column.ColumnName, new Field(column.ColumnName, "value", Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
#if DEBUG
            DateTime start = DateTime.Now;
#endif
            DeleteIndex(table);
            WriteDataRowCollection(table.Rows);
#if DEBUG
            TimeSpan span = DateTime.Now - start;
            System.Console.WriteLine(string.Format("Speed:{0}ms/line", span.TotalMilliseconds / table.Rows.Count));
#endif
            WriteTableCompletedEventArgs args = new WriteTableCompletedEventArgs(table.TableName);
            base.OnWriteTableCompletedEvent(this, args);
            this.isBusy = false;
        }
        /**/
        /// <summary>
        /// 对数据库一行进行索引
        /// </summary>
        /// <param name="row">数据库中的一行数据</param>
        /// <param name="boost">数据的权重</param>
        public override void WriteDataRow(DataRow row)
        {
            DataColumnCollection columns = row.Table.Columns;
            foreach (DataColumn column in columns)
            {
                if (!fieldDict.ContainsKey(column.ColumnName)) continue;
                fieldDict[column.ColumnName].SetValue(row[column].ToString());
                document.RemoveField(column.ColumnName);
                document.Add(fieldDict[column.ColumnName]);
            }
            fsWriter.AddDocument(document);
        }
        /**/
        /// <summary>
        /// 对数据库行进行索引
        /// </summary>
        /// <param name="collection">数据库中行数据</param>
        public override void WriteDataRowCollection(DataRowCollection collection)
        {
            int i = 0;
#if DEBUG
            System.Console.WriteLine(string.Format("i={0},time={1}", i, DateTime.Now.ToLongTimeString()));
#endif
            foreach (DataRow row in collection)
            {
                WriteDataRow(row);
                i++;
#if DEBUG
                if (i % SupportClass.MAX_ROWS_WRITE == 0 )
                    System.Console.WriteLine(string.Format("i={0},time={1}",i,DateTime.Now.ToLongTimeString() ));
#endif
                WriteRowCompletedEventArgs args = new WriteRowCompletedEventArgs(RowNum, i);
                base.OnWriteRowCompletedEvent(this, args);
                if (i % Percent == 0)
                {
                    WriteDbProgressChangedEventArgs pargs = new WriteDbProgressChangedEventArgs(RowNum, i);
                    base.OnProgressChangedEvent(this, pargs);
                }
            }
            fsWriter.Flush();
            fsWriter.Optimize();
            fsWriter.Close();
        }
        /**/
        /// <summary>
        /// 对数据库行进行索引
        /// </summary>
        /// <param name="collection">数据库中行数据</param>
        public override void WriteDataRowCollectionWithNoEvent(DataRowCollection collection)
        {
            foreach (DataRow row in collection)
            {
                WriteDataRow(row);
            }
            fsWriter.Flush();
            fsWriter.Optimize();
            fsWriter.Close();
        }
        /**/
        /// <summary>
        /// 合并索引
        /// </summary>
        /// <param name="directoryPaths">索引存储路径列表</param>
        public override void MergeIndexes(params string[] directoryPaths)
        {
            if (fsWriter == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            List<Lucene.Net.Store.Directory> dictList = new List<Lucene.Net.Store.Directory>();
            foreach (string directory in directoryPaths)
            {
                dictList.Add(FSDirectory.GetDirectory(directory, false));
            }
            fsWriter.AddIndexes(dictList.ToArray());
            fsWriter.Flush();
            fsWriter.Optimize();
            fsWriter.Close();
        }
        #endregion
        #region Internal Function
        internal void DeleteIndex(DataTable table)
        {
            if (!string.IsNullOrEmpty(PrimaryKey))
            {
                Directory dir = FSDirectory.GetDirectory(path, false);
                IndexReader reader = IndexReader.Open(dir);
                foreach (DataRow row in table.Rows)
                {
                    Term term = new Term(PrimaryKey, row[PrimaryKey].ToString());
                    reader.DeleteDocuments(term);
                }
                reader.Close();
            }
        }
        #endregion
    }
}