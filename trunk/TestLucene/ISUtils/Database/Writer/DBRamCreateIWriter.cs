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
    public class DBRamCreateIWriter : DbWriterBase,DataBaseWriter
    {
        private IndexWriter ramWriter;
        private FSDirectory fsDir;
        private RAMDirectory ramDir=new RAMDirectory();
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="analyzer">分析器</param>
        /// <param name="directory">索引存储路径</param>
        /// <param name="create">创建索引还是增量索引</param>
        public DBRamCreateIWriter(Analyzer analyzer,string dbName, string directory, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs)
            :base(directory,dbName)
        {
            this.analyzer = analyzer;
            try
            {
                fsDir = FSDirectory.GetDirectory(directory, true);
                if (ramDir==null)
                    ramDir =new RAMDirectory();
                fsWriter = new IndexWriter(fsDir, analyzer, true);
                ramWriter = new IndexWriter(ramDir, analyzer, true);
                fsWriter.SetMaxFieldLength(maxFieldLength);
                fsWriter.SetRAMBufferSizeMB(ramBufferSize);
                fsWriter.SetMergeFactor(mergeFactor);
                fsWriter.SetMaxBufferedDocs(maxBufferedDocs);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /**/
        /// <summary>
        /// 析构函数
        ~DBRamCreateIWriter()
        {
            if (fsWriter !=null)
               fsWriter.Close();
            if (ramWriter !=null)
               ramWriter.Close();
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
            this.analyzer = analyzer;
            if (fsWriter != null && ramDir !=null)
            {
                fsWriter.AddIndexes(new Directory[] { ramDir });
                fsWriter.Flush();
                ramWriter.Close();
            }
            try
            {
                fsDir = FSDirectory.GetDirectory(directory, true);
                if (ramDir == null)
                    ramDir = new RAMDirectory();
                fsWriter = new IndexWriter(fsDir, analyzer, true);
                ramWriter = new IndexWriter(ramDir, analyzer, true);
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
            if (fsWriter == null || ramWriter == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
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
            DateTime start=DateTime.Now;
            int count = table.Rows.Count;
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
        public override void WriteDataTable(DataTable table, Dictionary<string, float> fieldsBoostDict)
        {
            if (fsWriter == null || ramWriter == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            if (fieldsBoostDict == null)
                throw new ArgumentNullException("fieldsBoostDict", "fieldsBoostDict is not valid.");
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
            int count = table.Rows.Count;
#endif
            WriteDataRowCollectionWithNoEvent(table.Rows);
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
        public override void WriteDataTableWithEvent(DataTable table, Dictionary<string, float> fieldsBoostDict)
        {
            if (fsWriter == null || ramWriter == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
            if (fieldsBoostDict == null)
                throw new ArgumentNullException("fieldsBoost", "fieldsBoost is not valid.");
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
        public override void WriteDataTableWithEvent(DataTable table)
        {
            if (fsWriter == null || ramWriter == null)
            {
                throw new Exception("The IndexWriter does not created.");
            }
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
                fieldDict[column.ColumnName].SetValue(Pretreatment(row[column].ToString()));
                document.RemoveField(column.ColumnName);
                document.Add(fieldDict[column.ColumnName]);
                //doc.Add(new Field(column.ColumnName, row[column].ToString(), Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
            try
            {
                ramWriter.AddDocument(document);
            }
            catch (Exception e)
            {
                OnException(e);
            }
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
                WriteDataRow(row);
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
                if (i / SupportClass.RAM_FLUSH_NUM >= 1 && i % SupportClass.RAM_FLUSH_NUM == 0)
                {
                    ramWriter.Flush();
                    fsWriter.AddIndexes(new Directory[] { ramDir });
                    ramWriter.Close();
                    ramWriter = new IndexWriter(ramDir, analyzer, true);
                }
            }
            ramWriter.Flush();
            fsWriter.AddIndexes(new Directory[] { ramDir });
            ramWriter.Close();
            ramWriter = new IndexWriter(ramDir, analyzer, true);
            fsWriter.Optimize();
            fsWriter.Close();
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
            int i = 0;
            try
            {
                foreach (DataRow row in collection)
                {
                    WriteDataRow(row);
                    i++;
                    if (i / SupportClass.RAM_FLUSH_NUM >= 1 && i % SupportClass.RAM_FLUSH_NUM == 0)
                    {
                        ramWriter.Flush();
                        fsWriter.AddIndexes(new Directory[] { ramDir });
                        ramWriter.Close();
                        ramWriter = new IndexWriter(ramDir, analyzer, true);
#if DEBUG
                        System.Console.WriteLine(i.ToString() + "\t" + DateTime.Now.ToLongTimeString());
#endif
                    }
                }
                ramWriter.Flush();
                fsWriter.AddIndexes(new Directory[] { ramDir });
                ramWriter.Close();
                ramWriter = new IndexWriter(ramDir, analyzer, true);
                fsWriter.Optimize();
                fsWriter.Close();
            }
            catch (Exception e)
            {
#if DEBUG
                System.Console.WriteLine(e.StackTrace.ToString());
#endif
                throw e;
            }
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
            List<Directory> dictList = new List<Directory>();
            foreach (string directory in directoryPaths)
            {
                dictList.Add(FSDirectory.GetDirectory(directory, false));
            }
            fsWriter.AddIndexes(dictList.ToArray());
            fsWriter.Optimize();
            fsWriter.Close();
        }
    }
}
