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
using System.IO;
using System.Text;
using ISUtils.Common;
using ISUtils.Searcher;
using System.Xml;
using System.Xml.Serialization;

public partial class searchresult : System.Web.UI.Page
{
    public string searchResult = "";
    public string searchInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        TcpClient client;
        NetworkStream ns;
        BinaryFormatter formater;
        string hostname = ConfigurationManager.AppSettings["HostName"];
        int port = int.Parse(ConfigurationManager.AppSettings["PortNum"]);
        HttpCookie userCookie = Request.Cookies[ConfigurationManager.AppSettings["CookieName"]];
        int pageSize = 10;
        string area = "";
        string content = "";
        if (userCookie != null)
        {
            pageSize = int.Parse(userCookie.Values["PageSize"]);
            area = userCookie.Values["Area"];
            content = userCookie.Values["Content"];
        }
        string szWordsAllContains="";
        string szExactPhraseContain="";
        string szOneOfWordsAtLeastContain="";
        string szWordNotInclude="";
        if (Request.QueryString["WordsAllContains"]!=null)
            szWordsAllContains=Server.UrlDecode(Request.QueryString["WordsAllContains"]);
        searchInfo = szWordsAllContains;
        if (Request.QueryString["ExactPhraseContain"]!=null)
            szExactPhraseContain=Server.UrlDecode(Request.QueryString["ExactPhraseContain"]);
        if (Request.QueryString["OneOfWordsAtLeastContain"]!=null)
            szOneOfWordsAtLeastContain= Server.UrlDecode(Request.QueryString["OneOfWordsAtLeastContain"]);
        if (Request.QueryString["WordNotInclude"]!=null)
            szWordNotInclude= Server.UrlDecode(Request.QueryString["WordNotInclude"]);
        try
        {
            client = new TcpClient(hostname, port);
            ns = client.GetStream();
            formater = new BinaryFormatter();
            SearchInfo sinfo = new SearchInfo();
            QueryInfo info = new QueryInfo();
            if (!string.IsNullOrEmpty(szWordsAllContains))
               info.WordsAllContains=szWordsAllContains;
            if (!string.IsNullOrEmpty(szExactPhraseContain))
               info.ExactPhraseContain = szExactPhraseContain;
            if (!string.IsNullOrEmpty(szOneOfWordsAtLeastContain))
               info.OneOfWordsAtLeastContain = szOneOfWordsAtLeastContain;
            if (!string.IsNullOrEmpty(szWordNotInclude))
               info.WordNotInclude = szWordsAllContains;
            info.IndexNames = area;
            sinfo.PageSize = pageSize;
            sinfo.PageNum = 1;
            sinfo.Query = info;
            sinfo.HighLight = true;
            formater.Serialize(ns, sinfo);
            //searchInfo = sinfo.ToString();
            SearchResult sr = (SearchResult)formater.Deserialize(ns);
            StringBuilder buffer = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SearchRecord));
            foreach (SearchRecord record in sr.Records)
            {
                StringBuilder builder = new StringBuilder();
                StringWriter writer = new StringWriter(builder);
                xmlSerializer.Serialize(writer, record);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(builder.ToString());
                writer.Close();
                //Response.Write(doc.DocumentElement.OuterXml);
                string title, detail;
                record.GetWebInfo(out title, out detail);
                //if (string.IsNullOrEmpty(record.Name))
                //    buffer.Append("<a href=\"#\" onclick=\"searchFunc('" + info.SearchWords + "','IndexView_Monitoring_LI')\">" + title + "</a><br>");
                //else
                buffer.Append("<a href=\"#\" onclick=\"TransferXmlDoc('" + Server.UrlEncode(doc.DocumentElement.OuterXml) + "')\">" + title + "</a><br>");
                buffer.Append(detail+ "<br><br>");
                //foreach (SearchField field in record.Fields)
                //{
                //    buffer.Append(field.Name + "\t" + field.Value + "<br>");
                //}
                //buffer.Append("--------------------------------------------------<br>");
            }
            searchResult = buffer.ToString();
        }
        catch (Exception se)
        {
            Response.Write(se.StackTrace.ToString());
            return;
        }
    }
    protected string szXmlDoc = "";
    public string XmlString
    {
        get { return szXmlDoc; }
    }
    protected void RedirectWithXml(string szXml)
    {
        Application["xmldoc"] =szXml;
        Response.Redirect("~/display.aspx");
    }
}
