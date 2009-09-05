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
            string searchWords = GetCookie("SearchWords");
            if (string.IsNullOrEmpty(searchWords))
                return;
            txtSearch.Text = searchWords;
            RunSearch();
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
        bool allCon = true;
        bool allArea = true;
        if (userCookie != null)
        {
            try
            {
                pageSize = int.Parse(Decode(userCookie.Values["PageSize"]));
                area = Decode(userCookie.Values["Area"]);
                content = Decode(userCookie.Values["Content"]);
                allCon = bool.Parse(Decode(userCookie.Values["AllContent"]));
                allArea = bool.Parse(Decode(userCookie.Values["AllArea"]));
            }
            catch (Exception )
            {
            }
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
            if(!allCon)
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
            Number.InnerText = GetStatisticString(sr.Statistics, txtSearch.Text.Trim(),filter, pageSize, sr.PageNum);
            #region Title and Content
            DataBaseLibrary.GraphicsManagementHandle gmh = new DataBaseLibrary.GraphicsManagementHandle();            
            foreach (SearchRecord record in sr.Records)
            {
                if (record.Caption.Equals("文件", StringComparison.CurrentCultureIgnoreCase))
                {
                    string type, fvalue;
                    GetFileTypeValue(record["路径"].Value, out type, out fvalue);
                    buffer.Append("<span class=\"LargeTitle\" >" + type + "<a href=\"" + GetFileUrl(record["路径"].Value) + "\" target=\"_blank\" >" + GetColorString(record["文件名"].Result) + "&nbsp;得分：" + record.Score.ToString() + "</a></span><br>");
                    buffer.Append("<span class=\"SmallTitle\" >" + fvalue + "</span><br>");
                    if(!string.IsNullOrEmpty(record["内容"].Value))
                        buffer.Append("<span class=\"SmallTitle\" >" + GetColorString(record["内容"].Result) + "</span><br>");
                    buffer.Append("<br>");
                }
                else
                {
                    string title, detail, xmlRecord;
                    xmlRecord = GetXmlRecord(xmlSerializer, record);
                    record.GetWebInfo(out title, out detail, true,false);
                    detail = detail + "......";
                    //标题，点击调用序列化
                    if(!string.IsNullOrEmpty(title))
                        buffer.Append("<a href=\"#\" class=\"LargeTitle\" onclick=\"OpenMessage('" + GetRedirectUrl(record) + "')\"><span class=\"LargeTitle\" onmouseover=\"this.className='MouseDown'\" onmouseout=\"this.className='LargeTitle'\">" + record.Caption + "：" + title.Replace("</B><B>", "").Replace("<B>", "<font color=\"Red\">").Replace("</B>", "</font>") + "&nbsp;得分：" + record.Score.ToString() + "</span></a><br />");
                    else
                        buffer.Append("<a href=\"#\" class=\"LargeTitle\" onclick=\"OpenMessage('" + GetRedirectUrl(record) + "')\"><span class=\"LargeTitle\" onmouseover=\"this.className='MouseDown'\" onmouseout=\"this.className='LargeTitle'\">" + record.Caption + "&nbsp;得分：" + record.Score.ToString() + "</span></a><br />");
                    buffer.Append("<span class=\"SmallTitle\" style=\"line-height:20px\">" + detail.Replace("</B><B>", "").Replace("<B>", "<font color=\"Red\">").Replace("</B>", "</font>") + "</span><br />");
                    buffer.Append("<img src=\"action_import.gif\" width=\"16px\" height=\"16px\" />&nbsp;<a href=\"#\" onclick=\"TransferString('" + Encode(xmlRecord) + "')\" class=\"SmallTitle\" >搜索关系</a>");

                    //查看图形                     

                    string ID = GetPGLValue(record);
                    if (!string.IsNullOrEmpty(ID))
                    {
                        bool IsImg = gmh.GetProjectGraphicsLabel(record.Caption, ID);
                        if (IsImg)
                        {
                            if (record.Caption == "遥感卫片监测调查")
                            {
                                string TBH = record["JCTBH"].Value;
                                buffer.Append("&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"icon_solutions_16px.gif\" width=\"16px\" height=\"16px\" />&nbsp;<a href=\"#\" onclick=\"OpenMessage('" + ConfigurationManager.AppSettings["MapPath_WP"] + "&TBH=" + TBH + "')\" class=\"SmallTitle\" >查看图形</a><br /><br />");
                            }
                            else
                            {
                                buffer.Append("&nbsp;&nbsp;&nbsp;&nbsp;<img src=\"icon_solutions_16px.gif\" width=\"16px\" height=\"16px\" />&nbsp;<a href=\"#\" onclick=\"OpenMessage('" + ConfigurationManager.AppSettings["MapPath"] + "?BusinessName=" + Server.UrlEncode(record.Caption) + "&ProjectID=" + ID + "')\" class=\"SmallTitle\" >查看图形</a><br /><br />");
                            }
                        }
                        else
                        {
                            buffer.Append("<br /><br />");
                        }
                    }
                    else
                    {
                        buffer.Append("<br /><br />");
                    }
                }
            }
            tdResult.InnerHtml = buffer.ToString();
            #endregion
            #region Statistics
            statis.Append("<table class=\"TableStyle\" width=\"100%\"><tr height=\"35px\"><td class=\"TableText\" colspan=\"2\" style=\"font-size:9pt; font-weight:bold\">分类统计信息</td></tr>");
            string url;
            foreach (string key in sr.Statistics.Keys)
            {
                if(sr.Statistics[key]>0)
                {
                    url=GetUrl(szWordsAllContains,szExactPhraseContain,szOneOfWordsAtLeastContain,szWordNotInclude,key,1);
                    string displayKey = key;
                    if (key.Equals("文件", StringComparison.CurrentCultureIgnoreCase))
                        displayKey = "电子文档";
                    statis.Append("<tr height=\"35px\"><td class=\"TableValue\" style=\"font-size:9pt;text-align:center;border-right:none\" width=\"30px\"><img src=\"icon_search_16px.gif\" width=\"16px\" height=\"16px\" /></td><td class=\"TableValue\" style=\"font-size:9pt;text-align:left;border-left:none\"><a href=\"" + url + "\" >" + displayKey + "</a>&nbsp;&nbsp;(" + sr.Statistics[key].ToString() + ")</td></tr>");
                }
            }
            url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, "", 1);
            statis.Append(GetStatisticDisplay(sr.Statistics,url));
            statis.Append("</table>");
            if(sr.Records.Count > 0)
                tdStatis.InnerHtml = statis.ToString();
            #endregion
            #region Page
            StringBuilder pageBuilder = new StringBuilder();
            if (sr.PageNum > 1)
            {
                url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, sr.PageNum - 1);
                pageBuilder.Append("<a class=\"SmallTitle\" href=\"" + url + "\" >上一页</a>&nbsp;");
            }
            if (sr.TotalPages<=10)
            {
                for (int i = 1; i <= sr.TotalPages; i++)
                {
                    if (i != sr.PageNum)
                    {
                        url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, i);
                        pageBuilder.Append("<a class=\"SmallTitle\" href=\"" + url + "\" >" + i.ToString() + "</a>&nbsp;");
                    }
                    else
                    {
                        pageBuilder.Append(sr.PageNum.ToString() + "&nbsp;");
                    }
                }
            }
            else
            {
                int startPage= sr.PageNum>=6 ? sr.PageNum-5 :1 ;
                int endPage=sr.PageNum+4>sr.TotalPages ? sr.TotalPages : sr.PageNum+4;
                if(endPage==sr.TotalPages)
                    startPage=endPage-9;
                if (startPage == 1)
                    endPage = startPage + 9;
                for (int i = startPage; i <= endPage; i++)
                {
                    if (i != sr.PageNum)
                    {
                        url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, i);
                        pageBuilder.Append("<a class=\"SmallTitle\"  href=\"" + url + "\" >" + i.ToString() + "</a>&nbsp;");
                    }
                    else
                    {
                        pageBuilder.Append(sr.PageNum.ToString() + "&nbsp;");
                    }
                }
            }
            if (sr.PageNum < sr.TotalPages)
            {
                url = GetUrl(szWordsAllContains, szExactPhraseContain, szOneOfWordsAtLeastContain, szWordNotInclude, filter, sr.PageNum + 1);
                pageBuilder.Append("<a class=\"SmallTitle\"  href=\"" + url + "\" >下一页</a>");
            }
            tdPageSet.InnerHtml = pageBuilder.ToString();
            #endregion
        }
        catch (SocketException sep)
        {
            Response.Write("搜索服务没有运行！");
            Response.Write(sep.StackTrace.ToString());
            return;
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
    protected string GetPGLValue(SearchRecord record)
    {
        string value = ConfigurationManager.AppSettings[record.Caption];
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        int start = value.IndexOf('[');
        int end = value.IndexOf(']');
        if (start <= 0 || end <= 0)
            return string.Empty;
        string item = value.Substring(start + 1, end - start - 1);
        item = item.Substring(item.IndexOf(":")+1);
        SearchField sf = record[item];
        if (sf == null)
            return string.Empty;
        else
            return sf.Value;
    }
    protected void GetFileTypeValue(string file, out string type, out string value)
    {
        if (file.EndsWith(".")) 
            file = file.Substring(0, file.Length - 1);
        string suffix = file.Substring(file.LastIndexOf(".") + 1).ToUpper();
        type = "【" + suffix + "】";
        value = ConfigurationManager.AppSettings[suffix];
        if (string.IsNullOrEmpty(value))
            value = "文件格式：未知";
        else
            value = "文件格式：" + value;
    }
    protected string GetStatisticDisplay(Dictionary<string, int> statis,string url)
    {
        int total = 0;
        StringBuilder result = new StringBuilder("<tr height=\"35px\"><td class=\"TableValue\" style=\"font-size:9pt;text-align:center;border-right:none\" width=\"30px\"><img src=\"icon_search_16px.gif\" width=\"16px\" height=\"16px\" /></td><td class=\"TableValue\" style=\"font-size:9pt;text-align:left;border-left:none\"><a href=\"");
        foreach (string key in statis.Keys)
        {
            total += statis[key];
        }
        result.Append(url);
        result.Append("\" >全部分类</a>&nbsp;&nbsp;(");
        result.Append(total);
        result.Append(")</td></tr>");
        return result.ToString();
    }
    protected string GetStatisticString(Dictionary<string, int> statis, string searchWords, string filter, int pageSize, int pageNum)
    {
        int total = 0;
        StringBuilder result = new StringBuilder();
        if (string.IsNullOrEmpty(filter) || statis.ContainsKey(filter) == false)
        {
            foreach (string key in statis.Keys)
            {
                total += statis[key];
            }
        }
        else
        {
            if (filter.Equals("文件"))
                result.Append("在“电子文档”中");
            else
                result.Append("在“" + filter + "”中");
            total = statis[filter];
        }
        result.Append("搜索“" + searchWords + "”获得大约");
        result.Append(total);
        if (total > 0)
        {
            result.Append("条查询结果，以下是第 ");
            if (pageNum == 0) pageNum = 1;
            result.Append((pageNum - 1) * pageSize + 1);
            if ((pageNum - 1) * pageSize + 1 != total)
            {
                result.Append("-");
                if (pageNum * pageSize > total)
                    result.Append(total);
                else
                    result.Append(pageSize * pageNum);
            }
            result.Append(" 条。");
        }
        else
        {
            result.Append("条查询结果。");
        }
        return result.ToString();
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
        txtWords.Value = result.ToString().Trim();
    }
    protected string GetUrl(string wordsAllContains,string exactPhraseContain,string oneOfWordsAtLeastContain,string wordNotInclude,string filter,int pagenum)
    {
        StringBuilder url = new StringBuilder("search.aspx?");
        url.Append("Word="+Encode(wordsAllContains));
        url.Append("&Exact="+Encode(exactPhraseContain));
        url.Append("&One="+Encode(oneOfWordsAtLeastContain));
        url.Append("&Not=" +Encode(wordNotInclude));
        url.Append("&Page=" + pagenum.ToString());
        if (!string.IsNullOrEmpty(filter))
        {
            url.Append("&Filter=" + Encode(filter));
        }
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
        if (string.IsNullOrEmpty(value))
            return "#";
        int start = value.IndexOf('{');
        int end = value.IndexOf('}');
        if (start <= 0 || end <=0)
            return "#";
        string href=value.Substring(start+1,end-start-1);
        if (href.IndexOf("?") > 0)
        {
            url.Append(href + "&");
        }
        else
        {
            url.Append(href + "?");
        }
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
    protected string GetFileUrl(string filepath)
    {
        try
        {
            string value = ConfigurationManager.AppSettings["电子文档"];
            if (string.IsNullOrEmpty(value))
                return "#";
            
            string[] array = value.Split("{}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (array == null)
                return "#";
            if (array.Length != 2)
                return "#";
            string suffix = filepath.Substring(array[1].Length).Replace('\\', '/');
            string prefix = array[0];
            if ( (prefix.EndsWith("/") && !prefix.EndsWith("://")) && suffix.StartsWith("/"))
            {
                return prefix + suffix.Substring(1);
            }
            return prefix + suffix;
        }
        catch (Exception e)
        {
            Response.Write(e.StackTrace.ToString() + "<br>");
            return "#";
        }
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
    public static string GetColorString(string szSrc)
    {
        szSrc = szSrc.Replace("</B><B>", "");
        szSrc = szSrc.Replace("<B>", "<font color=\"Red\">");
        szSrc = szSrc.Replace("</B>", "</font>");
        return szSrc;
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
    #region Cookie
    protected void SetCookie(string name, string value)
    {
        HttpCookie oldCookie = Request.Cookies[name];
        HttpCookie userCookie = new HttpCookie(name);
        if (oldCookie == null)
        {
            userCookie.Value = oldCookie.Value;
            foreach (string key in oldCookie.Values.Keys)
            {
                userCookie.Values[key] = oldCookie.Values[key];
            }
        }
        userCookie.Value = Server.UrlEncode(value);
        userCookie.Expires = DateTime.Now.AddDays(1);
        Response.AppendCookie(userCookie);
    }
    protected string GetCookie(string name)
    {
        string result = "";
        HttpCookie userCookie = Request.Cookies[name];
        if (userCookie != null && userCookie.Value.Equals("undefined", StringComparison.CurrentCultureIgnoreCase) == false)
        {
            result = Server.UrlDecode(userCookie.Value);
        }
        return result;
    }
    #endregion
}
