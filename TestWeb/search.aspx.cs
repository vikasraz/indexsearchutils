using System;
using System.Data;
using System.Configuration;
using System.Collections;
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
using System.IO;
using System.Text;
using ISUtils.Common;
using ISUtils.Searcher;
using System.Xml;
using System.Xml.Serialization;

public partial class searchresult : System.Web.UI.Page
{
    public string searchFilter = "";
    #region Event
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            return;
        }
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
        {
            return;
        }
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
            #region Title and Content
            foreach (SearchRecord record in sr.Records)
            {                
                string title, detail,xmlRecord;
                xmlRecord = GetXmlRecord(xmlSerializer,record);
                record.GetWebInfo(out title, out detail,true);
                if (!string.IsNullOrEmpty(title))
                {
                    buffer.Append("<a href=\"#\" onclick=\"TransferString('" + Encode(xmlRecord) + "')\" >" + title + "</a>");
                }
                else
                {
                    buffer.Append("<a href=\"#\" onclick=\"TransferString('" + Encode(xmlRecord) + "')\" >" + record.Caption + "</a>");
                }
                buffer.Append("&nbsp;<a href=\"" + GetRedirectUrl(record) + "\" target=\"_blank\"  >详细信息</a><br>");
                buffer.Append(detail + "<br><br>");
            }
            tdResult.InnerHtml = buffer.ToString();
            #endregion
            #region Statistics
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
            #endregion
            #region Page
            StringBuilder pageBuilder = new StringBuilder();
            if (sr.PageNum > 1)
            {
                url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, sr.PageNum - 1);
                pageBuilder.Append("<a href=\""+url+"\" >上一页</a>&nbsp;");
            }
            if (sr.TotalPages<=10)
            {
                for (int i = 1; i <= sr.TotalPages; i++)
                {
                    if (i != sr.PageNum)
                    {
                        url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, i);
                        pageBuilder.Append("<a href=\"" + url + "\" >" + i.ToString() + "</a>&nbsp;");
                    }
                    else
                    {
                        pageBuilder.Append(sr.PageNum.ToString() + "&nbsp;");
                    }
                }
            }
            else
            {
                if (sr.PageNum + 9 > sr.TotalPages)
                {
                    for (int i = sr.TotalPages - 9; i <= sr.TotalPages; i++)
                    {
                        if (i != sr.PageNum)
                        {
                            url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, i);
                            pageBuilder.Append("<a href=\"" + url + "\" >" + i.ToString() + "</a>&nbsp;");
                        }
                        else
                        {
                            pageBuilder.Append(sr.PageNum.ToString() + "&nbsp;");
                        }
                    }
                }
                else
                { 
                    pageBuilder.Append(sr.PageNum.ToString() + "&nbsp;");
                    for (int i = 1; i < 10; i++)
                    {
                        url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, sr.PageNum + i);
                        pageBuilder.Append("<a href=\"" + url + "\" >" + (sr.PageNum + i).ToString() + "</a>&nbsp;");
                    }
                }
            }
            if (sr.PageNum < sr.TotalPages)
            {
                url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, sr.PageNum + 1);
                pageBuilder.Append("<a href=\"" + url + "\" >下一页</a>");
            }
            tdPageSet.InnerHtml = pageBuilder.ToString();
            #endregion
        }
        catch (Exception se)
        {
            Response.Write(se.StackTrace.ToString());
            return;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        RunSearch();
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        RunSearch();
    }
    #endregion
    #region Function
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
        txtWords.Value = result.ToString().Trim();
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
    protected string GetXmlRecord(XmlSerializer xmlSerializer ,SearchRecord record)
    {
        StringBuilder builder = new StringBuilder();
        StringWriter writer = new StringWriter(builder);
        xmlSerializer.Serialize(writer, record);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(builder.ToString());
        writer.Close();
        return doc.DocumentElement.OuterXml;
    }
    protected string GetRedirectUrl(SearchRecord record)
    {
        string value = ConfigurationManager.AppSettings[record.Caption];
        StringBuilder url = new StringBuilder();
        int start = value.IndexOf('{');
        int end = value.IndexOf('}');
        if (start <= 0 || end <=0)
            return "#";
        string href=value.Substring(start+1,end-start-1);
        url.Append(href + "?");
        string rest = value.Substring(end + 1);
        string[] paramArray = rest.Split("[]".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        Dictionary<string, string> paramDict = new Dictionary<string, string>();
        foreach (string param in paramArray)
        {
            string[] keyValue = param.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if(keyValue.Length!=2)
                continue;
            paramDict.Add(keyValue[1], keyValue[0]);
        }
        bool find=false;
        foreach (SearchField field in record.Fields)
        {
            if (paramDict.ContainsKey(field.Name))
            {
                if (!find)
                {
                    url.Append(paramDict[field.Name] + "=" + field.Value);
                    find = true;
                }
                else
                {
                    url.Append("&" + paramDict[field.Name] + "=" + field.Value);
                }
            }
        }
        return url.ToString();
    }
    #endregion
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
            if (string.IsNullOrEmpty(value))
                continue;
            if (value.IndexOf('{') >= 0)
            {
                value = value.Substring(0, value.IndexOf('{'));
            }
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
