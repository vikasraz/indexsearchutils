using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using System.Data.SqlClient;

namespace TestLucene
{
    //class CreateIndex
    //{
    //    //索引写入器
    //    private IndexWriter writer;
    //    private string _id;
    //    public string id
    //    {
    //        get { return _id; }            
    //    }
    //    /// <summary>
    //    /// 初始化一个索引写入器writer,directory为创建索引的目录，true代表如果不存在索引文件将重新创建索引文件，
    //    /// 如果已经存在索引文件将覆写索引文件，如果为true将代表打开已经存在的索引文件
    //    /// </summary>
    //    /// <param name="directory">传入的要创建索引的目录，注意是字符串值，如果目录不存在，他将会被自动创建</param>
    //    /// <param name="boolCreate">传入bool值，是否全部重建索引</param>
    //    public CreateIndex(string directory, bool boolCreate)
    //    {
    //        writer = new IndexWriter(directory, new ChineseAnalyzer(), boolCreate);
    //        writer.SetUseCompoundFile(true);
    //    }
    //    /// <summary>
    //    /// 读数据库索引
    //    /// </summary>
    //    /// <param name="sql">sql语句</param>
    //    /// <param name="sqlconn">数据库连接字符串</param>
    //    /// <param name="indexColumnName">索引记录的id列名</param>
    //    /// <param name="titleColumnName">索引记录的title列名</param>
    //    public void AddHtmlToDocument(string sql, string sqlconn, string indexColumnName, string titleColumnName)
    //    {

    //        SqlConnection conn = new SqlConnection(sqlconn);
    //        SqlCommand cmd = new SqlCommand(sql, conn);

    //        conn.Open();
    //        SqlDataReader sdr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
    //        StringBuilder html ;
    //        int icount = sdr.FieldCount;
    //        while (sdr.Read())
    //        {
    //            html = new StringBuilder();
    //            Document doc = new Document();
    //            for (int i = 1; i < icount; i++)
    //                html.Append(sdr[1].ToString());

    //            doc.Add(Field.UnStored("text", html.ToString()));
    //            doc.Add(Field.Keyword("id", sdr[indexColumnName].ToString()));
    //            doc.Add(Field.Text("title", sdr[titleColumnName].ToString() + DateTime.Now.ToString()));
    //            _id = sdr[indexColumnName].ToString();
    //            writer.AddDocument(doc);

    //        }
           
    //    }
    //    /// <summary>
    //    /// 把读取的文件中的所有的html标记去掉，把&nbsp;替换成空格
    //    /// </summary>
    //    /// <param name="html"></param>
    //    /// <returns></returns>
    //    private string ParseHtml(string html)
    //    {
    //        string temp = Regex.Replace(html, "<[^>]*>", "");
    //        return temp.Replace("&nbsp;", " ");
    //    }

    //    public void Close()
    //    {
    //        writer.Optimize();
    //        writer.Close();
    //    }

    //}
}
