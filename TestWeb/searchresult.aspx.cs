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
                //lbXml.Items.Add(Server.UrlEncode(doc.DocumentElement.OuterXml));
                buffer.Append("<a href=\"#\" onclick=\"TransferString('" + Server.UrlEncode(doc.DocumentElement.OuterXml) + "')\">" + title + "</a><br>");
                //buffer.Append("<a href=\"#\" onclick=\"TransferString('" + record.ToMinString() + "')\">" + title + "</a><br>");
                //buffer.Append("<a href=\"#\" onclick=\"selectxml('" + Server.UrlEncode(doc.DocumentElement.OuterXml) + "')\">" + title + "</a><br>");
                //string decode= Server.UrlDecode("%3cSearchRecord%3e%3cDoc+Name%3d%22IndexView_Monitoring_HCOV%22+Caption%3d%22%e4%be%9d%e6%b3%95%e8%a1%8c%e6%94%bf%e5%a4%84%e7%bd%9a%22+Index%3d%22IndexView_Monitoring_HCOV%22%3e%3cField+Name%3d%22AJCBR%22+Caption%3d%22%e6%a1%88%e4%bb%b6%e6%89%bf%e5%8a%9e%e4%ba%ba%22+Value%3d%22%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22AJLY%22+Caption%3d%22%e6%a1%88%e4%bb%b6%e6%9d%a5%e6%ba%90%22+Value%3d%22%e5%9c%9f%e5%9c%b0%e5%b7%a1%e6%9f%a5%22+Boost%3d%221%22+IsTitle%3d%22True%22+%2f%3e%3cField+Name%3d%22AY%22+Caption%3d%22%e6%a1%88%e7%94%b1%22+Value%3d%22%e8%bf%9d%e6%b3%95%e5%bb%ba%e8%ae%be%22+Boost%3d%221%22+IsTitle%3d%22True%22+%2f%3e%3cField+Name%3d%22Bxwr_XM%22+Caption%3d%22%e8%a2%ab%e8%af%a2%e9%97%ae%e4%ba%ba%e5%a7%93%e5%90%8d%22+Value%3d%22%e6%9d%8e%e5%9b%9b%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22CFJD%22+Caption%3d%22%e5%a4%84%e7%bd%9a%e5%86%b3%e5%ae%9a%22+Value%3d%22%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22JLR%22+Caption%3d%22%e8%ae%b0%e5%bd%95%e4%ba%ba%22+Value%3d%22%e4%b8%9c%e4%b8%bd%e5%bc%80%e5%8f%91%e5%8c%ba%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22SLBMJY%22+Caption%3d%22%e5%8f%97%e7%90%86%e9%83%a8%e9%97%a8%e5%bb%ba%e8%ae%ae%22+Value%3d%22%e5%90%8c%e6%84%8f%e5%a4%84%e7%90%86%e3%80%82%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22SLRJY%22+Caption%3d%22%e5%8f%97%e7%90%86%e4%ba%ba%e5%bb%ba%e8%ae%ae%22+Value%3d%22%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22SLRQ%22+Caption%3d%22%e5%8f%97%e7%90%86%e6%97%a5%e6%9c%9f%22+Value%3d%222009-6-29+0%3a00%3a00%22+Boost%3d%221%22+IsTitle%3d%22True%22+%2f%3e%3cField+Name%3d%22WFSSXZ%22+Caption%3d%22%e8%bf%9d%e6%b3%95%e4%ba%8b%e5%ae%9e%e5%8f%8a%e6%80%a7%e8%b4%a8%22+Value%3d%22%e6%9c%aa%e5%8f%96%e9%81%93%e5%90%88%e6%b3%95%e6%89%8b%e7%bb%ad%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22Wfdw_DZ%22+Caption%3d%22%e8%bf%9d%e6%b3%95%e5%8d%95%e4%bd%8d%e5%9c%b0%e5%9d%80%22+Value%3d%22%e4%b8%9c%e4%b8%bd%e5%bc%80%e5%8f%91%e5%8c%ba%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22Wfdw_MC%22+Caption%3d%22%e8%bf%9d%e6%b3%95%e5%8d%95%e4%bd%8d%e5%90%8d%e7%a7%b0%22+Value%3d%22%e5%a4%a9%e6%b4%a5%e5%b8%82%e4%b8%87%e7%91%9e%e7%94%b5%e5%99%a8%e6%9c%89%e9%99%90%e8%b4%a3%e4%bb%bb%e5%85%ac%e5%8f%b8%22+Boost%3d%221%22+IsTitle%3d%22True%22+%2f%3e%3cField+Name%3d%22Wfr_XM%22+Caption%3d%22%e8%bf%9d%e6%b3%95%e4%ba%ba%e5%a7%93%e5%90%8d%22+Value%3d%22%e4%b8%9c%e4%b8%bd%e5%bc%80%e5%8f%91%e5%8c%ba%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22XCBMYJ%22+Caption%3d%22%e5%8d%8f%e6%9f%a5%e9%83%a8%e9%97%a8%e6%84%8f%e8%a7%81%22+Value%3d%22%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22XCQQ%22+Caption%3d%22%e5%8d%8f%e6%9f%a5%e8%af%b7%e6%b1%82%22+Value%3d%22ADSF%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22XWJL%22+Caption%3d%22%e8%af%a2%e9%97%ae%e8%ae%b0%e5%bd%95%22+Value%3d%22%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22XWNR%22+Caption%3d%22%e8%af%a2%e9%97%ae%e5%86%85%e5%ae%b9%22+Value%3d%22%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22XWR%22+Caption%3d%22%e8%af%a2%e9%97%ae%e4%ba%ba%22+Value%3d%22%e5%bc%a0%e4%b8%89%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3cField+Name%3d%22ZGLDPS%22+Caption%3d%22%e4%b8%bb%e7%ae%a1%e9%a2%86%e5%af%bc%e6%89%b9%e7%a4%ba%22+Value%3d%22%e5%90%8c%e6%84%8f%22+Boost%3d%221%22+IsTitle%3d%22False%22+%2f%3e%3c%2fDoc%3e%3c%2fSearchRecord%3e");
                buffer.Append(detail + "<br><br>");
                //foreach (SearchField field in record.Fields)
                //{
                //    buffer.Append(field.Name + "\t" + field.Value + "<br>");
                //}
                //buffer.Append("--------------------------------------------------<br>");
            }
            searchResult = buffer.ToString();
            TD5.InnerHtml = searchResult;

        }
        catch (Exception se)
        {
            Response.Write(se.StackTrace.ToString());
            return;
        }
    }
}
