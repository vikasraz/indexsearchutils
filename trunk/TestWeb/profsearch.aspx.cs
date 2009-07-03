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

public partial class profsearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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

    }
}
