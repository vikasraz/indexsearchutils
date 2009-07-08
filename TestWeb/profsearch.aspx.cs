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
    protected void Page_Load(object sender, EventArgs e)
    {
        GetUserSettings(out pageSize,  out area,  out content);
        SetGuiControlSettings(pageSize, area,  content);
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
        userCookie.Values["PageSize"] = pageSize.ToString();
        userCookie.Values["Area"] = area;
        userCookie.Values["Content"] = content;
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
            pageSize = int.Parse(userCookie.Values["PageSize"]);
            area = userCookie.Values["Area"];
            content = userCookie.Values["Content"];
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
        pageSize = GetNumberInt(dropListPageSize.Text);
        area = GetCheckBoxListValue(checkListArea, false);
        content = GetCheckBoxListValue(checkListDetails, true);
    }
    protected void SetGuiControlSettings(int pageSize,  string area, string content)
    {
        dropListPageSize.Text = pageSize.ToString() + "项结果";
        List<string> szAreaList = new List<string>();
        szAreaList.AddRange(area.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        SetCheckBoxListValue(ref checkListArea, szAreaList);
        List<string> szContentList = new List<string>();
        string[] szTokens = area.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        foreach (string token in szTokens)
        {
            szContentList.Add(GetAppSettingKey(token));
        }
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
        StringBuilder url = new StringBuilder("~/searchresult.aspx?");
        url.Append("WordsAllContains="+txtWordsAllContains.Text);
        url.Append("&ExactPhraseContain="+txtExactPhraseContain.Text );
        url.Append("&OneOfWordsAtLeastContain="+txtOneOfWordsAtLeastContain.Text);
        url.Append("&WordNotInclude=" +txtWordNotInclude.Text);
        Response.Redirect(url.ToString());
    }
}
