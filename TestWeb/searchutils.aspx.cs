using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using ISUtils.Common;
using ISUtils.Searcher;

public partial class searchutils : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string searchWords = Request.QueryString["SearchWords"];
        string indexNames = Request.QueryString["IndexNames"];
        //Response.Write(searchWords);
        string hostname = ConfigurationManager.AppSettings["HostName"];
        int port = int.Parse(ConfigurationManager.AppSettings["PortNum"]);
        TcpClient client;
        NetworkStream ns;
        BinaryFormatter formater;
        DateTime now = DateTime.Now;
        try
        {
            client = new TcpClient(hostname, port);
            ns = client.GetStream();
            formater = new BinaryFormatter();
            SearchInfo sinfo = new SearchInfo();
            QueryInfo info = new QueryInfo();
            info.IndexNames = "";
            info.SearchWords = searchWords;
            info.IndexNames = indexNames;
            sinfo.Query = info;
            sinfo.PageSize = 10;
            formater.Serialize(ns, sinfo);
            SearchResult sr = (SearchResult)formater.Deserialize(ns);
            //WriteLog(Server.MapPath(".") + "searchutils.log", searchWords, indexNames, sr.Records.Count, sr);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SearchResult));
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            xmlSerializer.Serialize(writer, sr);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            writer.Close();
            ns.Close();
            Response.Write(doc.DocumentElement.OuterXml);
            //FileStream fs = new FileStream(@"result.xml", FileMode.Truncate);
            //xmlSerializer.Serialize(writer, fs);
            //fs.Close();
        }
        catch (Exception se)
        {
            Response.Write(se.StackTrace.ToString());
            return;
        }
    }
    protected void WriteLog(string path,string searchwords,string index,int recNum,SearchResult sr)
    {
        try
        {
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            string str = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff") + "]" ;
            sw.WriteLine(str);
            sw.WriteLine("SearchWords=\t" + searchwords);
            sw.WriteLine("Index=\t" + index);
            sw.WriteLine("Search Record:\t" + recNum.ToString());
            sw.WriteLine("PageNum:\t"+sr.PageNum);
            foreach (SearchRecord record in sr.Records)
            {
                sw.WriteLine("==============================================");
                sw.WriteLine("Caption:\t" + record.Caption);
                foreach (SearchField field in record.Fields)
                {
                    sw.WriteLine(field.Caption + "\t\t" + field.Name + "\t\t" + field.Value);
                }
            }
            sw.WriteLine("#########################################################################");
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        catch (Exception e)
        {
            throw e;
        }

    }
}
