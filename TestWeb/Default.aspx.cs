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
            QueryInfo info = new QueryInfo();
            info.IndexNames = txtIndexName.Text;
            info.SearchWords = txtSearch.Text;
            DateTime now = DateTime.Now;
            try
            {
                formater.Serialize(ns, info);
            }
            catch (SerializationException se)
            {
                Response.Write(se.Message + "\n");
            }
            QueryResult qr= null;
            try
            {
                DateTime start = DateTime.Now;
                qr = (QueryResult)formater.Deserialize(ns);
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
            Dictionary<QueryResult.SearchInfo, List<QueryResult.ExDocument>>.KeyCollection kc=qr.docs.Keys;
            foreach (QueryResult.SearchInfo si in kc)
            {
                Response.Write("index :" + si.IndexName + "<br>");
                foreach (QueryResult.ExDocument ed in qr.docs[si])
                {
                    foreach (string s in si.Fields)
                    {
                        Response.Write("\t" + ed.doc.Get(s));
                    }
                    Response.Write("\tscore=" + ed.score.ToString() + "<br>");
                    //Response.Write(ISUtils.SupportClass.Document.ToString(ed.doc)+ "<br>");
                    //Console.WriteLine(string.Format("title:{0} \nhistoryName:{1}", doc.Get("id"), doc.Get("historyName")));
                }
            }
            Response.Write("搜索测试完成，花费时间：" + tm.TotalMilliseconds.ToString() + "毫秒\n");
        }
    }
}
