<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchsetting.aspx.cs" Inherits="searchsetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>搜索设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <table style="width: 813px; height: 213px">
            <tr>
                <td style="width: 1066px; height: 37px; text-align: center">
                    结果设置</td>
                <td colspan="3" style="height: 37px; text-align: left;">
                    默认设置（10项）最有效且最快<br />
                    <asp:DropDownList ID="dropListPageSize" runat="server" Width="220px">
                        <asp:ListItem>10项结果</asp:ListItem>
                        <asp:ListItem>20项结果</asp:ListItem>
                        <asp:ListItem>30项结果</asp:ListItem>
                        <asp:ListItem>50项结果</asp:ListItem>
                        <asp:ListItem>100项结果</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td rowspan="2" style="width: 1066px; text-align: center">
                    区域设置
                </td>
                <td colspan="3" style="height: 1px; text-align: left;">
                    <asp:RadioButtonList ID="radioListArea" runat="server">
                        <asp:ListItem Selected="True">所有区域(建议)</asp:ListItem>
                        <asp:ListItem>自定义区域</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="1" style="width: 54px; height: 23px;">
                    &nbsp;</td>
                <td colspan="2" style="text-align: left; width: 651px; height: 23px;">
                    <asp:CheckBoxList ID="checkListArea" runat="server" Height="1px" RepeatDirection="Horizontal"
                        Width="390px">
                        <asp:ListItem>华民镇</asp:ListItem>
                        <asp:ListItem>新立街</asp:ListItem>
                        <asp:ListItem>么六桥</asp:ListItem>
                    </asp:CheckBoxList></td>
            </tr>
            <tr>
                <td rowspan="2" style="width: 1066px; text-align: center">
                    内容设置</td>
                <td colspan="3" style="height: 48px; text-align: left;">
                    <asp:RadioButtonList ID="radioListDetails" runat="server">
                        <asp:ListItem Selected="True">所有内容(建议)</asp:ListItem>
                        <asp:ListItem>自定义内容</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td colspan="1" style="width: 54px;">
                </td>
                <td colspan="2" style="width: 651px; text-align: left;">
                    <asp:CheckBoxList ID="checkListDetails" runat="server" RepeatDirection="Horizontal" Height="1px" Width="599px" RepeatColumns="6">
                        <asp:ListItem>土地利用规划</asp:ListItem>
                        <asp:ListItem>地籍权属</asp:ListItem>
                        <asp:ListItem>耕地保护</asp:ListItem>
                        <asp:ListItem>土地利用</asp:ListItem>
                        <asp:ListItem>执法监察</asp:ListItem>
                        <asp:ListItem>整理开发复垦</asp:ListItem>
                        <asp:ListItem Value="法律法规 ">法律法规  </asp:ListItem>
                        <asp:ListItem>综合办公</asp:ListItem>
                    </asp:CheckBoxList></td>
            </tr>
            <tr>
                <td colspan="4" rowspan="1" style="text-align: center">
                    <asp:Button ID="btnOk" runat="server" Text="确定" Width="71px" />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" Width="65px" />
                    <asp:Button ID="btnReset" runat="server" Text="重置" Width="69px" /></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
