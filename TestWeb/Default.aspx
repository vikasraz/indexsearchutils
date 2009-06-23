<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>搜索测试</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <asp:Panel ID="Panel1" runat="server" Height="692px" Width="751px">
            &nbsp;<asp:Label ID="Label1" runat="server" Text="服务器地址："></asp:Label>
            <asp:TextBox ID="txtIP" runat="server" Height="15px" Width="151px">192.168.1.102</asp:TextBox><br />
            <asp:Label ID="Label2" runat="server" Text="使用索引："></asp:Label>
            <asp:TextBox ID="txtIndexName" runat="server">in_main1,in_main2</asp:TextBox><br />
        <asp:TextBox ID="txtSearch" runat="server" Height="23px" Width="636px"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="搜索" Height="31px" OnClick="btnSearch_Click" Width="103px" /><br />
       <%=this.result%>
        </asp:Panel>
        &nbsp;</div>
    </form>
</body>
</html>
