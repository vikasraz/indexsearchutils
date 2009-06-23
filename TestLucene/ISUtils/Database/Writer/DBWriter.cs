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
    public class DBWriter:DbWriterBase,DataBaseWriter
    {
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
        public DBWriter(Analyzer analyzer, string directory, bool create)
        {
            if (create)
            {
                writer = new IndexWriter(directory, analyzer, true);
            }
            else
            {
                Directory dict = FSDirectory.GetDirectory(directory, false);
                writer = new IndexWriter(dict, analyzer, false);
            }
        }
        /**/
        /// <summary>
        /// 析构函数
        ~DBWriter()
        {
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
            if (create)
            {
                writer = new IndexWriter(directory, analyzer, true);
            }
            else
            {
                Directory dict = FSDirectory.GetDirectory(directory, false);
                writer = new IndexWriter(dict, analyzer, false);
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
            this.isBusy = true;
            RowNum = table.Rows.Count;
            Percent = RowNum / 100;
            WriteDataRowCollection(table.Rows);
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
            this.isBusy = true;
            RowNum = table.Rows.Count;
            Percent = RowNum / 100;
            WriteDataRowCollection(table.Rows);
            WriteTableCompletedEventArgs args = new WriteTableCompletedEventArgs(table.TableName);
            base.OnWriteTableCompletedEvent(this, args);
            this.isBusy = false;
        }
        /**/
        /// <summary>
        /// 对数据库表进行索引
        /// </summary>
        /// <param name="table">数据库表名</param>
        public override void WriteDataTable(DataTable table, ref System.Windows.Forms.ToolStripProgressBar progressBar)
        {
            WriteDataRowCollection(table.Rows, ref progressBar);
        }
        /**/
        /// <summary>
        /// 对数据库表进行索引
        /// </summary>
        /// <param name="table">数据库表名</param>
        public override void WriteDataTable(DataTable table, ref System.Windows.Forms.ProgressBar progressBar)
        {
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
            Document doc = new Document();
            DataColumnCollection columns = row.Table.Columns;
            foreach (DataColumn column in columns)
            {
                doc.Add(new Field(column.ColumnName, row[column].ToString(), Field.Store.COMPRESS, Field.Index.TOKENIZED,Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
            writer.AddDocument(doc);
        }
        /**/
        /// <summary>
        /// 对数据库行进行索引
        /// </summary>
        /// <param name="collection">数据库中行数据</param>
        public override void WriteDataRowCollection(DataRowCollection collection)
        {
            foreach (DataRow row in collection)
            {
                WriteDataRow(row, 1.0f);
            }
            writer.Close();
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
            writer.Close();
        }
        /**/
        /// <summary>
        /// 对数据库行进行索引
        /// </summary>
        /// <param name="collection">数据库中行数据</param>
        public override void WriteDataRowCollection(DataRowCollection collection, ref System.Windows.Forms.ToolStripProgressBar progressBar)
        {
            progressBar.Minimum  = 0;
            progressBar.Maximum = collection.Count;
            progressBar.Value = 0;
            int i = 0;
            foreach (DataRow row in collection)
            {
                WriteDataRow(row, 1.0f);
                i++;
                if (i % SupportClass.MAX_ROWS_WRITE == 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    progressBar.Value = i;
                }
            }
            writer.Close();
        }
        /**/
        /// <summary>
        /// 对数据库行进行索引
        /// </summary>
        /// <param name="collection">数据库中行数据</param>
        public override void WriteDataRowCollection(DataRowCollection collection, ref System.Windows.Forms.ProgressBar progressBar)
        {
            progressBar.Minimum = 0;
            progressBar.Maximum = collection.Count;
            progressBar.Value = 0;
            int i = 0;
            foreach (DataRow row in collection)
            {
                WriteDataRow(row, 1.0f);
                i++;
                if (i % SupportClass.MAX_ROWS_WRITE == 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    progressBar.Value = i;
                }
            }
            writer.Close();
        }
        /**/
        /// <summary>
        /// 合并索引
        /// </summary>
        /// <param name="directoryPaths">索引存储路径列表</param>
        public override void MergeIndexes(params string[] directoryPaths)
        {
            List<Directory> dictList = new List<Directory>();
            foreach (string directory in directoryPaths)
            {
                dictList.Add(FSDirectory.GetDirectory(directory, false));
            }
            writer.AddIndexes(dictList.ToArray());
            writer.Close();
        }
    }
}