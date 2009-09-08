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
    public class DBCreateIWriter : DbWriterBase,DataBaseWriter
    {
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="analyzer">分析器</param>
        /// <param name="directory">索引存储路径</param>
        /// <param name="create">创建索引还是增量索引</param>
        public DBCreateIWriter(Analyzer analyzer,string dbName, string directory, int maxFieldLength, double ramBufferSize, int mergeFactor, int maxBufferedDocs)
            :base(directory,dbName)
        {
            try
            {
                fsWriter = new IndexWriter(directory, analyzer, true);
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
        ~DBCreateIWriter()
        {
            if (fsWriter != null)
                fsWriter.Close();
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
            if (fsWriter != null)
            {
                fsWriter.Flush();
            }
            try
            {
                fsWriter = new IndexWriter(directory, analyzer, true);
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
            this.isBusy = true;
            RowNum = table.Rows.Count;
            Percent = RowNum / SupportClass.PERCENTAGEDIVE+1;
            DataColumnCollection columns = table.Columns;
            foreach (DataColumn column in columns)
            {
                if (fieldDict.ContainsKey(column.ColumnName))
                    continue;
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
        public override void WriteDataTable(DataTable table, Dictionary<string, float> fieldsBoostDict)
        {
            if (fsWriter == null)
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
                Field field=new Field(column.ColumnName, "value", Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                if (fieldsBoostDict.ContainsKey(column.ColumnName))
                    field.SetBoost(fieldsBoostDict[column.ColumnName]);
                if (fieldDict.ContainsKey(column.ColumnName))
                    continue;
                fieldDict.Add(column.ColumnName, field);
                //defValList.Add(column.ColumnName,SupportClass.Formatter.GetValue
#if DEBUG
                System.Console.WriteLine("Caption:\t" + column.Caption + "\tName:\t" + column.ColumnName + "\tType:\t" + column.DataType.ToString());
#endif
            }
#if DEBUG
            DateTime start = DateTime.Now;
            int count = table.Rows.Count;
            System.Console.WriteLine("Table Name:\t" + table.TableName);
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
            if (fsWriter == null)
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
            if (fsWriter == null)
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
                //#if DEBUG
                //                Console.WriteLine("Column: name " + column.ColumnName + "\tvalue " + row[column].ToString());
                //#endif
                if (!fieldDict.ContainsKey(column.ColumnName)) continue;
                fieldDict[column.ColumnName].SetValue(Pretreatment(row[column].ToString()));
                document.RemoveField(column.ColumnName);
                document.Add(fieldDict[column.ColumnName]);
                //doc.Add(new Field(column.ColumnName, row[column].ToString(), Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
            }
            try
            {
                fsWriter.AddDocument(document);
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
            }
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
            foreach (DataRow row in collection)
            {
                //try
                //{
                    WriteDataRow(row);
                //}
                //catch (Exception e)
                //{
                //    continue;
                //}
            }
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
