<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>搜索测试</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <asp:Panel ID="Panel1" runat="server" Height="724px" Width="1185px">
            <hr />
            &nbsp;<asp:Label ID="Label1" runat="server" Text="服务器地址：" Height="26px" Width="103px"></asp:Label>
            <asp:TextBox ID="txtIP" runat="server" Height="24px" Width="493px">192.168.1.102</asp:TextBox>&nbsp;<br />
            <asp:Label ID="Label2" runat="server" Text="使用索引：" Height="24px" Width="107px"></asp:Label>
            <asp:TextBox ID="txtIndexName" runat="server" Height="22px" Width="495px">in_main1,in_main2</asp:TextBox>
            <hr />
        <asp:TextBox ID="txtSearch" runat="server" Height="23px" Width="977px"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="模糊搜索" Height="31px" OnClick="btnSearch_Click" Width="103px" /><br />
       <%=this.result%>
            <hr />
            <asp:Label ID="Label3" runat="server" Height="26px" Text="字段：" Width="71px"></asp:Label><asp:TextBox
                ID="txtFieldInclude" runat="server" Height="24px" Width="301px"></asp:TextBox>
            <asp:Label ID="Label6" runat="server" Height="27px" Text="包含词：" Width="86px"></asp:Label><asp:TextBox
                ID="txtWordsInclude" runat="server" Height="21px" Width="335px"></asp:TextBox><br />
            <asp:Label ID="Label4" runat="server" Height="27px" Text="字段：" Width="72px"></asp:Label><asp:TextBox
                ID="txtFieldExclude" runat="server" Height="24px" Width="302px"></asp:TextBox>
            <asp:Label ID="Label7" runat="server" Height="25px" Text="不包含词：" Width="85px"></asp:Label><asp:TextBox
                ID="txtWordsExclude" runat="server" Height="23px" Width="335px"></asp:TextBox><br />
            <asp:Label ID="Label5" runat="server" Height="26px" Text="字段：" Width="70px"></asp:Label><asp:TextBox
                ID="txtFieldRange" runat="server" Height="21px" Width="304px"></asp:TextBox>
            <asp:Label ID="Label8" runat="server" Height="23px" Text="从" Width="27px"></asp:Label>
            <asp:TextBox ID="txtRangeFrom" runat="server" Height="19px" Width="179px"></asp:TextBox>
            <asp:Label ID="Label9" runat="server" Height="21px" Text="到" Width="24px"></asp:Label>
            <asp:TextBox ID="txtRangeTo" runat="server" Height="20px" Width="239px"></asp:TextBox><br />
            <asp:Button ID="btnExactSearch" runat="server" Height="74px" OnClick="btnExactSearch_Click"
                Text="精确搜索" Width="282px" /><br />
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/commonsearch.aspx">国土资源综合搜索</asp:HyperLink></asp:Panel>
       </div>
    </form>
</body>
</html>
