<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchsetting.aspx.cs" Inherits="searchsetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>搜索设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <table style="width: 813px; height: 663px">
            <tr>
                <td style="width: 200px; height: 25px; text-align: center">
                    结果设置</td>
                <td colspan="3" style="height: 25px">
                    默认设置（10项）最有效且最快<br />
                    <select id="Select1" style="width: 225px; height: 30px">
                        <option selected="selected"></option>
                    </select>
                </td>
            </tr>
            <tr>
                <td rowspan="2" style="width: 200px; text-align: center">
                    区域设置</td>
                <td colspan="3" style="height: 4px">
                    <asp:RadioButtonList ID="radioListArea" runat="server">
                        <asp:ListItem Selected="True">所有区域(建议)</asp:ListItem>
                        <asp:ListItem>自定义区域</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td colspan="1" style="width: 85px">
                    &nbsp;</td>
                <td colspan="2">
                    <asp:CheckBoxList ID="checkListArea" runat="server" Height="1px" RepeatDirection="Horizontal"
                        Width="390px">
                        <asp:ListItem>华民镇</asp:ListItem>
                        <asp:ListItem>新立街</asp:ListItem>
                        <asp:ListItem>么六桥</asp:ListItem>
                    </asp:CheckBoxList></td>
            </tr>
            <tr>
                <td rowspan="2" style="width: 200px; text-align: center">
                    内容设置</td>
                <td colspan="3" style="height: 48px">
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                        <asp:ListItem Selected="True">所有内容(建议)</asp:ListItem>
                        <asp:ListItem>自定义内容</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td colspan="1" style="width: 85px">
                </td>
                <td colspan="2">
                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem>土地利用规划</asp:ListItem>
                        <asp:ListItem>地籍权属</asp:ListItem>
                        <asp:ListItem>耕地保护</asp:ListItem>
                        <asp:ListItem>土地利用</asp:ListItem>
                        <asp:ListItem>执法监察</asp:ListItem>
                        <asp:ListItem>整理开发复垦</asp:ListItem>
                        <asp:ListItem>法律法规</asp:ListItem>
                        <asp:ListItem>综合办公</asp:ListItem>
                    </asp:CheckBoxList></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
