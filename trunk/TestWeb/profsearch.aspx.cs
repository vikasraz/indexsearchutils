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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public partial class profsearch : System.Web.UI.Page
{
    private int pageSize;
    private string area, content;
    #region Property
    public int PageSize
    {
        get
        {
            return ViewState["PageSize"] == null ? 10 : (int)ViewState["PageSize"];
        }
        set
        {
            ViewState.Add("PageSize", value);
        }
    }
    public string Area
    {
        get
        {
            return ViewState["Area"] == null ? "" : (string)ViewState["Area"];
        }
        set
        {
            ViewState.Add("Area", value);
        }
    }
    public string Content
    {
        get
        {
            return ViewState["Content"] == null ? "" : (string)ViewState["Content"];
        }
        set
        {
            ViewState.Add("Content", value);
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        //if (Request.UrlReferrer == null || Request.UrlReferrer.ToString().Contains(Request.Url.ToString()) == false)
        {
            GetUserSettings(out pageSize, out area,  out content);
            SetGuiControlSettings(pageSize, area, content);
            PageSize = pageSize;
            Area = area;
            Content = content;
        }
        else
        {
            GetGuiControlSettings(out pageSize,  out area, out content);
            PageSize = pageSize;
            Area = area;
            Content = content;
        }
    }
    protected void SetUserSettings(int pageSize,  string area,  string content)
    {
        HttpCookie oldCookie=Request.Cookies[ConfigurationManager.AppSettings["CookieName"]];
        HttpCookie userCookie = new HttpCookie(ConfigurationManager.AppSettings["CookieName"]);
        if (oldCookie != null)
        {
            foreach (string key in oldCookie.Values.Keys)
            {
                userCookie.Values[key] = oldCookie.Values[key];
            }
        }
        userCookie.Values["PageSize"] = Encode(pageSize.ToString());
        userCookie.Values["Area"] = Encode(area);
        userCookie.Values["Content"] = Encode(content);
        userCookie.Expires = DateTime.Now.AddDays(7);
        Response.Cookies.Add(userCookie);
    }
    protected void GetUserSettings(out int pageSize, out string area, out string content)
    {
        HttpCookie userCookie = Request.Cookies[ConfigurationManager.AppSettings["CookieName"]];
        pageSize = 10;
        area = "";
        content = "";
        if (userCookie != null)
        {
            if (userCookie.Values["PageSize"] != null)
                pageSize = int.Parse(Decode(userCookie.Values["PageSize"]));
            if (userCookie.Values["Area"] != null)
                area = Decode(userCookie.Values["Area"]);
            if (userCookie.Values["Content"] != null)
                content = Decode(userCookie.Values["Content"]);
        }
    }
    protected void OutputSettings()
    {
        Response.Write("PageSize:\t" + pageSize.ToString() + "<br>");
        Response.Write("Area:\t" + area + "<br>");
        Response.Write("Content:\t" + content + "<br>");
    }
    public static string GetCheckBoxListValue(CheckBoxList cbList, bool isAppSetting)
    {
        StringBuilder buffer = new StringBuilder();
        foreach (ListItem item in cbList.Items)
        {
            if (item.Selected)
            {
                if (isAppSetting)
                    buffer.Append(ConfigurationManager.AppSettings[item.Text] + ",");
                else
                    buffer.Append(item.Text + ",");
            }
        }
        if (buffer.Length > 0)
            buffer.Remove(buffer.Length - 1, 1);
        return buffer.ToString();
    }
    public static string GetAppSettingKey(string szValue)
    {
        foreach (string key in ConfigurationManager.AppSettings.AllKeys)
        {
            if (ConfigurationManager.AppSettings[key] == szValue)
                return key;
        }
        return string.Empty;
    }
    public static void SetCheckBoxListValue(ref CheckBoxList cbList, List<string> valueList)
    {
        for (int i = 0; i < cbList.Items.Count; i++)
        {
            ListItem item = cbList.Items[i];
            if (valueList.Contains(item.Text))
            {
                item.Selected = true;
            }
            else
            {
                item.Selected = false;
            }
        }
    }
    public static int GetNumberInt(string szSrc)
    {
        int result = 0;
        if (szSrc != null && szSrc != string.Empty)
        {
            // 正则表达式剔除非数字字符（不包含小数点.）
            szSrc = Regex.Replace(szSrc, @"[^\d.\d]", "");
            // 如果是数字，则转换为decimal类型
            if (Regex.IsMatch(szSrc, @"^[+-]?\d*[.]?\d*$"))
            {
                result = int.Parse(szSrc);
            }
        }
        return result;
    }
    protected void GetGuiControlSettings(out int pageSize,  out string area,  out string content)
    {
        pageSize = int.Parse(dropListPageSize.SelectedValue);
        area = GetCheckBoxListValue(checkListArea, false);
        content = GetCheckBoxListValue(checkListDetails, false);
    }
    protected void SetGuiControlSettings(int pageSize,  string area, string content)
    {
        dropListPageSize.SelectedValue = pageSize.ToString();
        List<string> szAreaList = new List<string>();
        szAreaList.AddRange(area.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        SetCheckBoxListValue(ref checkListArea, szAreaList);
        List<string> szContentList = new List<string>();
        szContentList.AddRange(content.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        //foreach (string token in szTokens)
        //{
        //    szContentList.Add(GetAppSettingKey(token));
        //}
        SetCheckBoxListValue(ref checkListDetails, szContentList);
    }
    protected void btnAllSelArea_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in checkListArea.Items)
            item.Selected = true;
    }
    protected void btnAllUnSelArea_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in checkListArea.Items)
            item.Selected = false;
    }
    protected void btnAllSelDetails_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in checkListDetails.Items)
            item.Selected = true;
    }
    protected void btnAllUnSelDetails_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in checkListDetails.Items)
            item.Selected = false;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetGuiControlSettings(out pageSize, out area, out content);
        SetUserSettings(pageSize,  area, content);
        StringBuilder url = new StringBuilder("~/search.aspx?");
        url.Append("Word="+Server.UrlEncode(txtWordsAllContains.Text));
        url.Append("&Exact="+Server.UrlEncode(txtExactPhraseContain.Text ));
        url.Append("&One="+Server.UrlEncode(txtOneOfWordsAtLeastContain.Text));
        url.Append("&Not=" +Server.UrlEncode(txtWordNotInclude.Text));
        Response.Redirect(url.ToString());
    }
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
