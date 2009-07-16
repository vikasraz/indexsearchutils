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
using System.Text;

public partial class settings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetCheckBoxListValue(ref checkListDetails, Server.UrlDecode(Request.QueryString["param"]));
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        //SetCookie("Setting", GetCheckBoxListValue(checkListDetails));
        string str;
        str = "<script language='javascript'>";
        str += "window.returnValue=\"" + GetCheckBoxListValue(checkListDetails) + "\";";
        //str += "alert(window.returnValue);";
        str += "window.opener=null;";
        str += "window.close(this);";
        str += "</script>"; 
        ////Literal1.Visible=true; 
        //Literal1.Text=str;
        Page.RegisterStartupScript("script", str); 
    }
    protected static string GetCheckBoxListValue(CheckBoxList cbList)
    {
        StringBuilder buffer = new StringBuilder();
        foreach (ListItem item in cbList.Items)
        {
            if (item.Selected)
            {
                buffer.Append(item.Text + ",");
            }
        }
        if (buffer.Length > 0)
            buffer.Remove(buffer.Length - 1, 1);
        return buffer.ToString();
    }
    protected void btnAllSel_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in checkListDetails.Items)
            item.Selected = true;
    }
    protected void btnUnSel_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in checkListDetails.Items)
            item.Selected = false;
    }
    protected static void SetCheckBoxListValue(ref CheckBoxList cbList, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            foreach (ListItem item in cbList.Items)
                item.Selected = true;
        }
        else
        {
            string[] array = value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<string> valueList = new List<string>();
            valueList.AddRange(array);
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
    }
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
        if (userCookie != null && userCookie.Value.Equals("undefined",StringComparison.CurrentCultureIgnoreCase)==false)
        {
            result=Server.UrlDecode(userCookie.Value);
        }
        return result;
    }
    #endregion
}
