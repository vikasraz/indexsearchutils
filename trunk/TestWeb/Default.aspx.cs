using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Lucene.Net.Documents;
using ISUtils.Common;
using ISUtils.Searcher;

public partial class _Default : System.Web.UI.Page 
{
    public string result;
    private const int portNum = 3312;
    //private const string hostName = "192.168.1.102";

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSearch.Text) && !string.IsNullOrEmpty(txtIP.Text))
        {
            
            result = txtSearch.Text+"\n";
            TcpClient client;
            NetworkStream ns;
            BinaryFormatter formater;
            DateTime now = DateTime.Now;
            try
            {
                client = new TcpClient(txtIP.Text, portNum);
                ns = client.GetStream();
                formater = new BinaryFormatter();
            }
            catch (Exception ex)
            {
                Response.Write(ex.StackTrace.ToString() + "<br>");
                return;
            }
            SearchInfo sinfo = new SearchInfo();
            QueryInfo info = new QueryInfo();
            info.IndexNames = txtIndexName.Text;
            info.SearchWords = txtSearch.Text;
            sinfo.Query = info;           
            try
            {
                formater.Serialize(ns, sinfo);
            }
            catch (SerializationException se)
            {
                Response.Write(se.Message + "\n");
            }
            SearchResult sr= null;
            try
            {
                DateTime start = DateTime.Now;
                sr = (SearchResult)formater.Deserialize(ns);
                TimeSpan span = DateTime.Now - start;
                Response.Write("反序列化:"+span.TotalMilliseconds.ToString() + "毫秒<br>");
            }
            catch (SerializationException sep)
            {
                Response.Write(sep.Message + "\n");
            }
            finally
            {
                ns.Close();
            }
            //info.Path = @"E:\TEMP\DB\";

            //string path = @"D:\Indexer\seglib\";
            //ChineseSegAnalysis ca = new ChineseSegAnalysis(path + "BaseDict.txt", path + "FamilyName.txt", path + "Number.txt", path + "CustomDict.txt", path + "Other.txt");
            //Analyzer analyzer = ca.GetAnalyzer();

            //ca.FilterFilePath = path + "Filter.txt";

            //IndexSearcher searcher = new IndexSearcher(@"E:\TEMP\DB\");
            //QueryParser parser = new QueryParser("dlmc", analyzer);
            //Query query = parser.Parse(txtSearch.Text);

            ////输出我们要查看的表达式
            //result +=query.ToString()+"\n";
            //DateTime now = DateTime.Now;
            //Hits hits = searcher.Search(query);

            TimeSpan tm = DateTime.Now - now;
            Response.Write(sr.ToString() + "<br>");
            foreach (SearchRecord record in sr.Records)
            {
                Response.Write("----------------------------------------<br>");
                foreach (SearchField field in record.Fields)
                {
                    Response.Write(field.Name+":\t"+field.Value + "<br>");
                }
            }
            //foreach (QueryResult.SearchInfo si in qr.docs.Keys)
            //{
            //    Response.Write("index :" + si.IndexName + "<br>");
            //    foreach (QueryResult.ExDocument ed in qr.docs[si])
            //    {
            //        foreach (string s in si.Fields)
            //        {
            //            Response.Write("\t" + ed.doc.Get(s));
            //        }
            //        Response.Write("\tscore=" + ed.score.ToString() + "<br>");
            //        //Response.Write(ISUtils.SupportClass.Document.ToString(ed.doc)+ "<br>");
            //        //Console.WriteLine(string.Format("title:{0} \nhistoryName:{1}", doc.Get("id"), doc.Get("historyName")));
            //    }
            //}
            //Lucene.Net.Highlight.Highlighter highlighter = new Lucene.Net.Highlight.Highlighter(new Lucene.Net.Highlight.QueryScorer(highquery));
            //highlighter.SetTextFragmenter(new Lucene.Net.Highlight.SimpleFragmenter(100));

            ////for (int i = start; i < end; i++)
            ////{
            ////    Lucene.Net.Documents.Document doc = hits.Doc(i);
            ////    System.String text = doc.Get("content");
            ////    //添加结尾，保证结尾特殊符号不被过滤
            ////    string title = doc.Get("title") + "+aaaaaaaaa";
            ////    Lucene.Net.Analysis.TokenStream tokenStream = highanalyzer.TokenStream("content", new System.IO.StringReader(text));
            ////    Lucene.Net.Analysis.TokenStream titkeStream = highanalyzer.TokenStream("title", new System.IO.StringReader(title));
            ////    System.String result = highlighter.GetBestFragments(tokenStream, text, 2, "...");
            ////    string tresult = highlighter.GetBestFragments(titkeStream, title, 0, "..");
            ////    //祛除标题结尾标记
            ////    if (tresult.Length > 10)
            ////        tresult = tresult.Remove(tresult.Length - 10, 10);
            ////    if (string.IsNullOrEmpty(tresult))
            ////        tresult = title.Remove(title.Length - 10, 10);
            ////    //未标注内容读取
            ////    if (string.IsNullOrEmpty(result))
            ////    {
            ////        if (text.Length > 100)
            ////            result = text.Substring(0, 100);
            ////        else
            ////            result = text;
            ////    }
            ////    if (result.Length < text.Length)
            ////        result = result + "...";
            ////} 
            //Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer();
            //foreach (QueryResult.ExDocument ed in qr.docList)
            //{
            //    //Response.Write(ed.doc.ToString() + "<br>");
            //    Document doc = ed.doc;
            //    //List<Field> fields = new List<Field>();
            //    //fields.AddRange(doc.GetFields().CopyTo);
            //    Field[] fields = new Field[doc.GetFields().Count];
            //    doc.GetFields().CopyTo(fields, 0);
            //    foreach (Field field in fields)
            //    {
            //        string key = field.Name();
            //        string value = field.StringValue();
            //        Lucene.Net.Analysis.TokenStream tokenStream = analyzer.TokenStream(key, new System.IO.StreamReader(value));
            //        string result = highlighter.GetBestFragment(tokenStream, value);
            //        Response.Write(key + "\t" + result+"\t" );
            //    }
            //    Response.Write("<br>");
            //    //Response.Write(ed.doc.Get("TDZL"));
            //    //Response.Write(ed.doc.Get("SYQR") + "<br>");
            //    //IList<string> fields = (IList<string>)ed.doc.GetFields();
            //    //foreach(string field in fields)
            //    //  Response.Write(ed.doc.Get(field) + "<br>");
            //}
            Response.Write("搜索测试完成，花费时间：" + tm.TotalMilliseconds.ToString() + "毫秒\n");
        }
    }
    protected void btnExactSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtSearch.Text) || string.IsNullOrEmpty(txtIP.Text))
            return;
        if (string.IsNullOrEmpty(txtFieldInclude.Text) && string.IsNullOrEmpty(txtFieldExclude.Text) &&
            string.IsNullOrEmpty(txtRangeFrom.Text))
            return;
        TcpClient client;
        NetworkStream ns;
        BinaryFormatter formater;
        DateTime now = DateTime.Now;
        try
        {
            client = new TcpClient(txtIP.Text, portNum);
            ns = client.GetStream();
            formater = new BinaryFormatter();
        }
        catch (Exception ex)
        {
            Response.Write(ex.StackTrace.ToString() + "<br>");
            return;
        }
        SearchInfo sinfo = new SearchInfo();
        QueryInfo info = new QueryInfo();
        info.IndexNames = txtIndexName.Text;
        if (!string.IsNullOrEmpty(txtFieldInclude.Text ) && !string.IsNullOrEmpty(txtWordsInclude.Text))
            info.FilterList.Add(new FilterCondition("", txtFieldInclude.Text, txtWordsInclude.Text));
        if (!string.IsNullOrEmpty(txtFieldExclude.Text) && !string.IsNullOrEmpty(txtWordsExclude.Text))
            info.ExcludeList.Add(new ExcludeCondition("", txtFieldExclude.Text, txtWordsExclude.Text));
        if (!string.IsNullOrEmpty(txtFieldRange.Text) && !string.IsNullOrEmpty(txtRangeFrom.Text) && !string.IsNullOrEmpty(txtRangeTo.Text))
            info.RangeList.Add(new RangeCondition("", txtFieldRange.Text, txtRangeFrom.Text, txtRangeTo.Text, RangeType.Date));
        sinfo.Query = info;
        try
        {
            formater.Serialize(ns, sinfo);
        }
        catch (SerializationException se)
        {
            Response.Write(se.Message + "\n");
        }
        SearchResult sr = null;
        try
        {
            DateTime start = DateTime.Now;
            sr = (SearchResult)formater.Deserialize(ns);
            TimeSpan span = DateTime.Now - start;
            Response.Write("反序列化:" + span.TotalMilliseconds.ToString() + "毫秒<br>");
        }
        catch (SerializationException sep)
        {
            Response.Write(sep.Message + "\n");
        }
        finally
        {
            ns.Close();
        }
        TimeSpan tm = DateTime.Now - now;
        Response.Write(sr.ToString() + "<br>");
        foreach (SearchRecord record in sr.Records)
        {
            Response.Write("----------------------------------------<br>");
            foreach (SearchField field in record.Fields)
            {
                Response.Write(field.Name + ":\t" + field.Value + "<br>");
            }
        }
        Response.Write("搜索测试完成，花费时间：" + tm.TotalMilliseconds.ToString() + "毫秒\n");
    }
}
