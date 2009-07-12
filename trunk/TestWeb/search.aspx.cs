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
    public string searchFilter = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack ) return;
        string szWordsAllContains="";
        string szExactPhraseContain="";
        string szOneOfWordsAtLeastContain="";
        string szWordNotInclude="";
        if (Request.QueryString["Word"] != null)
            szWordsAllContains = Decode(Request.QueryString["Word"]);
        if (Request.QueryString["Exact"] != null)
            szExactPhraseContain = Decode(Request.QueryString["Exact"]);
        if (Request.QueryString["One"] != null)
            szOneOfWordsAtLeastContain = Decode(Request.QueryString["One"]);
        if (Request.QueryString["Not"] != null)
            szWordNotInclude = Decode(Request.QueryString["Not"]);
        if (IsNullOrEmpty(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude))
            return;
        int pageNum = 1;
        if (Request.QueryString["Page"] != null)
            pageNum = int.Parse(Request.QueryString["Page"]);
        string filter = "";
        if (Request.QueryString["Filter"] != null)
            filter = Decode(Request.QueryString["Filter"]);
        searchFilter = filter;
        SetSearchWords(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude);
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
            pageSize = int.Parse(Decode(userCookie.Values["PageSize"]));
            area = Decode(userCookie.Values["Area"]);
            content = Decode(userCookie.Values["Content"]);
        }
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
            info.IndexNames = GetIndexNames(content);
            sinfo.PageSize = pageSize;
            sinfo.PageNum = pageNum;
            sinfo.Query = info;
            sinfo.HighLight = true;
            sinfo.Filter = filter;
            formater.Serialize(ns, sinfo);
            //searchInfo = sinfo.ToString();
            SearchResult sr = (SearchResult)formater.Deserialize(ns);
            StringBuilder buffer = new StringBuilder();
            StringBuilder statis = new StringBuilder();
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
                record.GetWebInfo(out title, out detail,true);
                if (!string.IsNullOrEmpty(title))
                {
                    buffer.Append("<a href=\"#\" onclick=\"TransferString('" + Encode(doc.DocumentElement.OuterXml) + "')\" >" + title + "</a><br>");
                }
                else
                {
                    buffer.Append("<a href=\"#\" onclick=\"TransferString('" + Encode(doc.DocumentElement.OuterXml) + "')\" >" + record.Caption + "</a><br>");
                }
                buffer.Append(detail + "<br><br>");
            }
            tdResult.InnerHtml = buffer.ToString();
            statis.Append("<p style=\"text-align: center\">查询结果统计</p><p>");
            string url;
            foreach (string key in sr.Statistics.Keys)
            {
                if(sr.Statistics[key]>0)
                {
                    url=GetUrl(szWordsAllContains,szExactPhraseContain,szOneOfWordsAtLeastContain,szWordNotInclude,key,1);
                    statis.Append("<a href=\""+url+"\" >" + key + "</a>(" + sr.Statistics[key].ToString() + ")<br>");
                }
            }
            statis.Append("</p>");
            if(sr.Records.Count > 0)
                tdStatis.InnerHtml = statis.ToString();
            StringBuilder pageBuilder = new StringBuilder();
            if (sr.PageNum > 1)
            {
                url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, sr.PageNum - 1);
                pageBuilder.Append("<a href=\""+url+"\" >上一页</a>&nbsp;");
            }
            if (sr.PageNum+9 > sr.TotalPages)
            {
                pageBuilder.Append(sr.PageNum.ToString() + "&nbsp;");
                for (int i = sr.PageNum + 1; i <= sr.TotalPages; i++)
                {
                    url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, i);
                    pageBuilder.Append("<a href=\""+url+"\" >" + i.ToString() + "</a>&nbsp;");
                }
            }
            else
            {
                pageBuilder.Append(sr.PageNum.ToString() + "&nbsp;");
                for (int i = 1; i < 10; i++)
                {
                    url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, sr.PageNum + i);
                    pageBuilder.Append("<a href=\"" + url + "\" >" + i.ToString() + "</a>&nbsp;");
                }
                url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, sr.PageNum + 1);
                pageBuilder.Append("<a href=\""+url+"\" >下一页</a>");
            }
            tdPageSet.InnerHtml = pageBuilder.ToString();
        }
        catch (Exception se)
        {
            Response.Write(se.StackTrace.ToString());
            return;
        }
    }
    protected void SetSearchWords(string wordsAllContains,string exactPhraseContain,string oneOfWordsAtLeastContain,string wordNotInclude)
    {
        string[] wordArray = wordNotInclude.Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        StringBuilder result = new StringBuilder();
        result.Append(wordsAllContains + " ");
        result.Append(exactPhraseContain + " ");
        result.Append(oneOfWordsAtLeastContain + " ");
        foreach (string word in wordArray)
        {
            result.Append(" -" + word);
        }
        txtSearch.Text = result.ToString().Trim();
        //txtSearch.Value = result.ToString().Trim();
        txtWords.Value = result.ToString().Trim();
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        //if (txtSearch.Text.EndsWith("\n") || txtSearch.Text.EndsWith("\r"))
        {
            RunSearch();
        }
    }
    protected string GetUrl(string wordsAllContains,string exactPhraseContain,string oneOfWordsAtLeastContain,string wordNotInclude,string filter,int pagenum)
    {
        StringBuilder url = new StringBuilder("search.aspx?");
        url.Append("Word="+Encode(wordsAllContains));
        url.Append("&Exact="+Encode(exactPhraseContain));
        url.Append("&One="+Encode(oneOfWordsAtLeastContain));
        url.Append("&Not=" +Encode(wordNotInclude));
        url.Append("&Page="+pagenum.ToString());
        url.Append("&Filter="+Encode(filter));
        return url.ToString();
    }
    protected void RunSearch()
    {
        if (string.IsNullOrEmpty(txtSearch.Text.Trim())) return;
        string[] wordArray = txtSearch.Text.Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        StringBuilder allContains = new StringBuilder();
        StringBuilder notIncludes = new StringBuilder();
        foreach (string word in wordArray)
        {
            if (word.StartsWith("-"))
            {
                notIncludes.Append(word.Substring(1) + " ");
            }
            else
            {
                allContains.Append(word + " ");
            }
        }
        StringBuilder url = new StringBuilder("~/search.aspx?");
        url.Append("Word=" + Server.UrlEncode(allContains.ToString().Trim()));
        if (!string.IsNullOrEmpty(notIncludes.ToString().Trim()))
            url.Append("&Not=" + Server.UrlEncode(notIncludes.ToString().Trim()));
        Response.Redirect(url.ToString());
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        RunSearch();
    }
    #region String Function
    public static bool IsNullOrEmpty(params string[] array)
    {
        bool result = true;
        foreach (string s in array)
        {
            if (!string.IsNullOrEmpty(s))
                return false;
        }
        return result;
    }
    #endregion
    #region IndexNames
    public string GetIndexNames(string content)
    {
        string[] keys = content.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        StringBuilder result = new StringBuilder();
        foreach (string key in keys)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value) == false)
            {
                result.Append(value + ",");
            }
        }
        if (result.Length > 0)
            result.Remove(result.Length - 1, 1);
        return result.ToString();
    }
    #endregion
    #region Endcode and Decode
    public string Encode(string szSrc)
    {
        return Server.UrlEncode(szSrc);
    }
    public string Decode(string szSrc)
    {
        return Server.UrlDecode(szSrc);
    }
    #endregion
}
