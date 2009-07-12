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

public partial class commonsearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //txtSearch.Attributes["onkeydown"] = "if(event.keycode==13){document.all.imgBtnSearch.click();return false;}";
    }
    protected void imgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        //StringBuilder url = new StringBuilder("~/search.aspx?");
        //url.Append("wd=" + txtSearch.Value);
        //Response.Redirect(url.ToString());
    }
    //public string RedirectUrl()
    //{
    //    StringBuilder url = new StringBuilder("search.aspx?");
    //    url.Append("wd=" + Server.UrlEncode(txtSearch.Value));
    //    return url.ToString();
    //}
}
