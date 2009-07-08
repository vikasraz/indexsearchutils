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
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class searchsetting : System.Web.UI.Page
{
    private int pageSize;
    private bool allArea, allContent;
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
    public bool AllArea
    {
        get
        {
            return ViewState["AllArea"] == null ? true : (bool)ViewState["AllArea"];
        }
        set
        {
            ViewState.Add("AllArea", value);
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
    public bool AllContent
    {
        get
        {
            return ViewState["AllContent"] == null ? true : (bool)ViewState["AllContent"];
        }
        set
        {
            ViewState.Add("AllContent", value);
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

        if (Request.UrlReferrer==null || Request.UrlReferrer.ToString().Contains(Request.Url.ToString()) == false)
        {
            GetUserSettings(out pageSize, out allArea, out area, out allContent, out content);
            SetGuiControlSettings(pageSize, allArea, area, allContent, content);
            PageSize = pageSize;
            AllArea = allArea;
            Area = area;
            AllContent = allContent;
            Content = content;
        }
        else
        {
            GetGuiControlSettings(out pageSize, out allArea, out area, out allContent, out content);
            PageSize = pageSize;
            AllArea = allArea;
            Area = area;
            AllContent = allContent;
            Content = content;
        }
        //OutputSettings();
    }
    protected void SetUserSettings(int pageSize, bool allArea, string area, bool allContent, string content)
    {
        HttpCookie oldCookie = Request.Cookies[ConfigurationManager.AppSettings["CookieName"]];
        HttpCookie userCookie = new HttpCookie(ConfigurationManager.AppSettings["CookieName"]);
        if (oldCookie != null)
        {
            foreach (string key in oldCookie.Values.Keys)
            {
                userCookie.Values[key] = oldCookie.Values[key];
            }
        }
        userCookie.Values["PageSize"] = pageSize.ToString();
        userCookie.Values["AllArea"]=allArea.ToString();
        userCookie.Values["Area"]=area;
        userCookie.Values["AllContent"]=allContent.ToString();
        userCookie.Values["Content"]=content;
        userCookie.Expires = DateTime.Now.AddDays(7);
        Response.Cookies.Add(userCookie);
    }
    protected void GetUserSettings(out int pageSize,out bool allArea,out string area,out bool allContent,out string content)
    {
        HttpCookie userCookie = Request.Cookies[ConfigurationManager.AppSettings["CookieName"]];
        pageSize = 10;
        allArea = true;
        allContent = true;
        area = "";
        content = "";
        if (userCookie!=null)
        {
            pageSize = int.Parse(userCookie.Values["PageSize"]);
            allArea = bool.Parse(userCookie.Values["AllArea"]);
            area = userCookie.Values["Area"];
            allContent = bool.Parse(userCookie.Values["AllContent"]);
            content = userCookie.Values["Content"];
        }
    }
    protected void OutputSettings()
    {
        Response.Write("PageSize:\t" + pageSize.ToString() + "<br>");
        Response.Write("AllArea:\t" + allArea.ToString() + "<br>");
        Response.Write("Area:\t" + area + "<br>");
        Response.Write("AllContent:\t" + allContent.ToString() + "<br>");
        Response.Write("Content:\t" + content + "<br>");
    }
    public static string GetCheckBoxListValue(CheckBoxList cbList,bool isAppSetting)
    {
        StringBuilder buffer = new StringBuilder();
        foreach (ListItem item in cbList.Items)
        {
            if (item.Selected)
            {
                if (isAppSetting)
                    buffer.Append(ConfigurationManager.AppSettings[item.Text.Trim()] + ",");
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
        for (int i = 0; i < cbList.Items.Count; i++ )
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
    protected void GetGuiControlSettings(out int pageSize, out bool allArea, out string area, out bool allContent, out string content)
    {
        pageSize = int.Parse(dropListPageSize.SelectedValue);
        allArea = radioListArea.SelectedIndex ==0;
        area = GetCheckBoxListValue(checkListArea, false);
        allContent = radioListDetails.SelectedIndex ==0;
        content = GetCheckBoxListValue(checkListDetails, true);
    }
    protected void SetGuiControlSettings(int pageSize, bool allArea, string area, bool allContent, string content)
    {
        dropListPageSize.SelectedValue = pageSize.ToString();
        radioListArea.SelectedIndex = allArea ? 0 : 1;
        radioListDetails.SelectedIndex = allContent ? 0 : 1;
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
    protected void btnOk_Click(object sender, EventArgs e)
    {
        GetGuiControlSettings(out pageSize,out allArea,out area,out allContent,out content);
        SetUserSettings(pageSize, allArea, area, allContent, content);
        //OutputSettings();
        Response.Redirect("~/commonsearch.aspx");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/commonsearch.aspx");
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        GetUserSettings(out pageSize, out allArea, out area, out allContent, out content);
        SetGuiControlSettings(pageSize, allArea, area, allContent, content);
    }
}
