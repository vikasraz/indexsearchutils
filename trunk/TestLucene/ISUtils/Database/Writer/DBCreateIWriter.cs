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
    /// 数据库的新建索引写入器
    /// </summary>
    class DBCreateIWriter : DbWriterBase,DataBaseWriter
    {
        /**/
        /// <summary>
        /// 索引文档
        /// </summary>
        private Document document;
        /**/
        /// <summary>
        /// 索引字段
        /// </summary>
        private Dictionary<string, Field> fieldDict;
        /**/
        /// <summary>
        /// 索引写入器
        /// </summary>
        private IndexWriter writer;
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="analyzer">分析器</param>
        /// <param name="directory">索引存储路径</param>
        /// <param name="create">创建索引还是增量索引</param>
        public DBCreateIWriter(Analyzer analyzer, string directory, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs)
        {
            document = new Document();
            fieldDict = new Dictionary<string, Field>();
            try
            {
                writer = new IndexWriter(directory, analyzer, true);
                writer.SetMaxFieldLength(maxFieldLength);
                writer.SetRAMBufferSizeMB(ramBufferSize);
                writer.SetMergeFactor(mergeFactor);
                writer.SetMaxBufferedDocs(maxBufferedDocs);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /**/
        /// <summary>
        /// 析构函数
        ~DBCreateIWriter()
        {
            if (writer != null)
                writer.Close();
        }
        /**/
        /// <summary>
        /// 设定基本属性值
        /// </summary>
        /// <param name="analyzer">分析器</param>
        /// <param name="directory">索引存储路径</param>
        /// <param name="create">创建索引还是增量索引</param>
        public override void SetBasicProperties(Analyzer analyzer, string directory, bool create)
        {
            if (writer != null)
            {
                writer.Flush();
            }
            try
            {
                writer = new IndexWriter(directory, analyzer, true);
                writer.SetMaxFieldLength(int.MaxValue);
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
            if (writer == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            writer.SetMergeFactor(mergeFactor);
            writer.SetMaxBufferedDocs(maxBufferedDocs);
        }
        /**/
        /// <summary>
        /// 对数据库表进行索引
        /// </summary>
        /// <param name="table">数据库表名</param>
        public override void WriteDataTable(DataTable table)
        {
            if (writer == null)
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
                fieldDict.Add(column.ColumnName, new Field(column.ColumnName, "value", Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
#if DEBUG
                System.Console.WriteLine("Caption:\t" + column.Caption + "\tName:\t" + column.ColumnName + "\tType:\t" + column.DataType.ToString());
#endif
            }
#if DEBUG
            DateTime start=DateTime.Now;
            int count = table.Rows.Count;
            System.Console.WriteLine("Table Name:\t"+table.TableName);
#endif
            WriteDataRowCollectionWithNoEvent(table.Rows);
#if DEBUG
            TimeSpan span=DateTime.Now -start;
            System.Console.WriteLine(string.Format("Speed:{0}ms/line",span.TotalMilliseconds/count));
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
            if (writer == null)
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
                fieldDict.Add(column.ColumnName, new Field(column.ColumnName, "value", Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
#if DEBUG
            DateTime start = DateTime.Now;
            int count = table.Rows.Count;
#endif
            WriteDataRowCollection(table.Rows);
#if DEBUG
            TimeSpan span = DateTime.Now - start;
            System.Console.WriteLine(string.Format("Speed:{0}ms/line", span.TotalMilliseconds / count));
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
        public override void WriteDataTable(DataTable table, ref System.Windows.Forms.ProgressBar progressBar)
        {
            if (writer == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            if (document == null)
                document = new Document();
            DataColumnCollection columns = table.Columns;
            foreach (DataColumn column in columns)
            {
                fieldDict.Add(column.ColumnName, new Field(column.ColumnName, "value", Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
            WriteDataRowCollection(table.Rows,ref progressBar);
        }
        /**/
        /// <summary>
        /// 对数据库表进行索引
        /// </summary>
        /// <param name="table">数据库表名</param>
        public override void WriteDataTable(DataTable table, ref System.Windows.Forms.ToolStripProgressBar progressBar)
        {
            if (writer == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            if (document == null)
                document = new Document();
            DataColumnCollection columns = table.Columns;
            foreach (DataColumn column in columns)
            {
                fieldDict.Add(column.ColumnName, new Field(column.ColumnName, "value", Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
            WriteDataRowCollection(table.Rows, ref progressBar);
        }
        /**/
        /// <summary>
        /// 对数据库一行进行索引
        /// </summary>
        /// <param name="row">数据库中的一行数据</param>
        /// <param name="boost">数据的权重</param>
        public override void WriteDataRow(DataRow row, float boost)
        {
            DataColumnCollection columns = row.Table.Columns;
            foreach (DataColumn column in columns)
            {
                //#if DEBUG
                //                Console.WriteLine("Column: name " + column.ColumnName + "\tvalue " + row[column].ToString());
                //#endif
                fieldDict[column.ColumnName].SetValue(row[column].ToString());
                document.RemoveField(column.ColumnName);
                document.Add(fieldDict[column.ColumnName]);
                //doc.Add(new Field(column.ColumnName, row[column].ToString(), Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
            writer.AddDocument(document);
        }
        /**/
        /// <summary>
        /// 对数据库行进行索引
        /// </summary>
        /// <param name="collection">数据库中行数据</param>
        public override void WriteDataRowCollection(DataRowCollection collection)
        {
            int i = 0;
            foreach (DataRow row in collection)
            {
                WriteDataRow(row, 1.0f);
                i++;
#if DEBUG
                if (i % SupportClass.MAX_ROWS_WRITE == 0)
                    System.Console.WriteLine(i.ToString() + "\t" + DateTime.Now.ToLongTimeString());
#endif
                WriteRowCompletedEventArgs args = new WriteRowCompletedEventArgs(RowNum, i);
                base.OnWriteRowCompletedEvent(this, args);
                if (i % Percent == 0)
                {
                    WriteDbProgressChangedEventArgs pargs = new WriteDbProgressChangedEventArgs(RowNum, i);
                    base.OnProgressChangedEvent(this, pargs);
                }
            }
            writer.Optimize();
            writer.Close();
#if DEBUG
            System.Console.WriteLine("WriteDataRowCollection Success!\t" + DateTime.Now.ToLongTimeString());
#endif
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
                WriteDataRow(row, 1.0f);
            }
            writer.Optimize();
            writer.Close();
        }
        /**/
        /// <summary>
        /// 对数据库行进行索引
        /// </summary>
        /// <param name="collection">数据库中行数据</param>
        public override void WriteDataRowCollection(DataRowCollection collection, ref System.Windows.Forms.ToolStripProgressBar progressBar)
        {
            //if (writer == null)
            //{
            //    throw new Exception("The IndexWriter does not created.");
            //}
            int i = 0;
            progressBar.Minimum  = 0;
            progressBar.Maximum = collection.Count;
            progressBar.Value  = 0;
            foreach (DataRow row in collection)
            {
                WriteDataRow(row, 1.0f);
                i++;
                if (i % SupportClass.MAX_ROWS_WRITE == 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    progressBar.Value = i;
                }
                //    writer.Flush();
            }
            writer.Optimize();
            writer.Close();
        }
        /**/
        /// <summary>
        /// 对数据库行进行索引
        /// </summary>
        /// <param name="collection">数据库中行数据</param>
        public override void WriteDataRowCollection(DataRowCollection collection, ref System.Windows.Forms.ProgressBar progressBar)
        {
            //if (writer == null)
            //{
            //    throw new Exception("The IndexWriter does not created.");
            //}
            int i = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = collection.Count;
            progressBar.Value = 0;
            foreach (DataRow row in collection)
            {
                WriteDataRow(row, 1.0f);
                i++;
                if (i % SupportClass.MAX_ROWS_WRITE == 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    progressBar.Value = i;
                }
                //if (i % SupportClass.MAX_ROWS_WRITE == 0 && i / SupportClass.MAX_ROWS_WRITE >= 1)
                //    writer.Flush();
            }
            writer.Optimize();
            writer.Close();
        }
        /**/
        /// <summary>
        /// 合并索引
        /// </summary>
        /// <param name="directoryPaths">索引存储路径列表</param>
        public override void MergeIndexes(params string[] directoryPaths)
        {
            if (writer == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            List<Directory> dictList = new List<Directory>();
            foreach (string directory in directoryPaths)
            {
                dictList.Add(FSDirectory.GetDirectory(directory, false));
            }
            writer.AddIndexes(dictList.ToArray());
            writer.Optimize();
            writer.Close();
        }
    }
}
