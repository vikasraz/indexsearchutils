<%@ Page Language="C#" AutoEventWireup="true" CodeFile="profsearch.aspx.cs" Inherits="profsearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>高级搜索</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <table style="width: 715px; height: 85px">
            <tr>
                <td colspan="5">
                    高级搜索</td>
            </tr>
            <tr>
                <td rowspan="4" style="width: 1425px">
                    常规设置</td>
                <td style="width: 2432px; height: 26px; text-align: left">
                    包含完整的词</td>
                <td style="width: 144px; height: 26px">
                    <asp:TextBox ID="txtWordsAllContains" runat="server"></asp:TextBox></td>
                <td style="width: 740px; height: 26px">
                </td>
                <td style="width: 357px; height: 26px">
                </td>
            </tr>
            <tr>
                <td style="width: 2432px; text-align: left">
                    包含完整的字句</td>
                <td style="width: 144px">
                    <asp:TextBox ID="txtExactPhraseContain" runat="server"></asp:TextBox></td>
                <td style="width: 740px; text-align: right">
                    <asp:DropDownList ID="dropListPageSize" runat="server" Width="129px">
                        <asp:ListItem>10项结果</asp:ListItem>
                        <asp:ListItem>20项结果</asp:ListItem>
                        <asp:ListItem>30项结果</asp:ListItem>
                        <asp:ListItem>50项结果</asp:ListItem>
                        <asp:ListItem>100项结果</asp:ListItem>
                    </asp:DropDownList></td>
                <td style="width: 357px; text-align: left">
                    <asp:Button ID="btnSearch" runat="server" Text="搜索" Width="59px" OnClick="btnSearch_Click" /></td>
            </tr>
            <tr>
                <td style="width: 2432px; text-align: left">
                    包含至少一个词</td>
                <td style="width: 144px">
                    <asp:TextBox ID="txtOneOfWordsAtLeastContain" runat="server"></asp:TextBox></td>
                <td style="width: 740px">
                </td>
                <td style="width: 357px">
                </td>
            </tr>
            <tr>
                <td style="width: 2432px; text-align: left">
                    不包含字词</td>
                <td style="width: 144px">
                    <asp:TextBox ID="txtWordNotInclude" runat="server"></asp:TextBox></td>
                <td style="width: 740px">
                </td>
                <td style="width: 357px">
                </td>
            </tr>
            <tr>
                <td rowspan="2" style="width: 1425px">
                    区域设置</td>
                <td colspan="4" style="text-align: left">
                    <asp:Button ID="btnAllSelArea" runat="server" Text="全选" Width="59px" OnClick="btnAllSelArea_Click" />
                    <asp:Button ID="btnAllUnSelArea" runat="server" Text="全不选" OnClick="btnAllUnSelArea_Click" /></td>
            </tr>
            <tr>
                <td colspan="4" style="height: 23px; text-align: left">
                    <asp:CheckBoxList ID="checkListArea" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">华民镇</asp:ListItem>
                        <asp:ListItem Selected="True">新立街</asp:ListItem>
                        <asp:ListItem Selected="True">么六桥</asp:ListItem>
                    </asp:CheckBoxList></td>
            </tr>
            <tr>
                <td rowspan="2" style="width: 1425px">
                    内容设置</td>
                <td colspan="4" style="text-align: left">
                    <asp:Button ID="btnAllSelDetails" runat="server" Text="全选" Width="57px" OnClick="btnAllSelDetails_Click" />
                    <asp:Button ID="btnAllUnSelDetails" runat="server" Text="全不选" OnClick="btnAllUnSelDetails_Click" /></td>
            </tr>
            <tr>
                <td colspan="4" style="height: 21px; text-align: left">
                    <asp:CheckBoxList ID="checkListDetails" runat="server" RepeatColumns="6" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">土地利用规划</asp:ListItem>
                        <asp:ListItem Selected="True">地籍权属</asp:ListItem>
                        <asp:ListItem Selected="True">耕地保护</asp:ListItem>
                        <asp:ListItem Selected="True">土地利用</asp:ListItem>
                        <asp:ListItem Selected="True">执法监察</asp:ListItem>
                        <asp:ListItem Selected="True">整理开发复垦</asp:ListItem>
                        <asp:ListItem Selected="True">法律法规</asp:ListItem>
                        <asp:ListItem Selected="True">综合办公</asp:ListItem>
                    </asp:CheckBoxList></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
