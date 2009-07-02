<%@ Page Language="C#" AutoEventWireup="true" CodeFile="commonsearch.aspx.cs" Inherits="commonsearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>国土资源综合搜索</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <table style="width: 865px; height: 524px">
            <tr>
                <td colspan="4" style="height: 66px; text-align: center">
                    国土资源综合搜索</td>
            </tr>
            <tr>
                <td colspan="3" rowspan="1" style="width: 653px; height: 184px; text-align: left">
                </td>
                <td colspan="1" rowspan="5" style="width: 46px; text-align: left">
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/profsearch.aspx">高级搜索</asp:HyperLink>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/searchsetting.aspx">搜索设置</asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td colspan="3" rowspan="4" style="width: 653px; text-align: left">
                    <asp:TextBox ID="txtSearch" runat="server" Height="17px" Width="713px">请输入搜索关键词</asp:TextBox>&nbsp;<asp:ImageButton
                        ID="imgBtnSearch" runat="server" Height="17px" ImageUrl="~/App_GlobalResources/search.jpg"
                        Width="18px" />
                    <div id="divSearch" style="width: 718px; height: 151px; text-align: left">
                    </div>
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
            <tr>
                <td colspan="4" style="height: 118px; text-align: left">
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
