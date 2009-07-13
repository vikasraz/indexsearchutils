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
                    <asp:DropDownList ID="dropListPageSize" runat="server" Width="220px" >
                        <asp:ListItem Value="10">10项结果</asp:ListItem>
                        <asp:ListItem Value="20">20项结果</asp:ListItem>
                        <asp:ListItem Value="30">30项结果</asp:ListItem>
                        <asp:ListItem Value="50">50项结果</asp:ListItem>
                        <asp:ListItem Value="100">100项结果</asp:ListItem>
                    </asp:DropDownList>&nbsp;
                </td>
            </tr>
            <tr>
                <td rowspan="2" style="width: 1066px; text-align: center">
                    区域设置
                </td>
                <td colspan="3" style="height: 1px; text-align: left;">
                    <asp:RadioButtonList ID="radioListArea" runat="server" >
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
                        Width="390px" >
                        <asp:ListItem>华民镇</asp:ListItem>
                        <asp:ListItem>新立街</asp:ListItem>
                        <asp:ListItem>么六桥</asp:ListItem>
                    </asp:CheckBoxList></td>
            </tr>
            <tr>
                <td rowspan="2" style="width: 1066px; text-align: center">
                    内容设置</td>
                <td colspan="3" style="height: 48px; text-align: left;">
                    <asp:RadioButtonList ID="radioListDetails" runat="server" >
                        <asp:ListItem Selected="True">所有内容(建议)</asp:ListItem>
                        <asp:ListItem>自定义内容</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td colspan="1" style="width: 54px;">
                </td>
                <td colspan="2" style="width: 651px; text-align: left;">
                    <asp:CheckBoxList ID="checkListDetails" runat="server" RepeatDirection="Horizontal" Height="1px" Width="599px" RepeatColumns="6" >
                        <asp:ListItem>依法行政处罚</asp:ListItem>
                        <asp:ListItem>土地巡查</asp:ListItem>
                        <asp:ListItem>信访办理</asp:ListItem>
                        <asp:ListItem>卫片核查</asp:ListItem>
                        <asp:ListItem>建设项目用地预审</asp:ListItem>
                        <asp:ListItem>农转用土地征收-项目</asp:ListItem>
                        <asp:ListItem>农转用土地征收-批次</asp:ListItem>
                        <asp:ListItem>核发建设用地批准书</asp:ListItem>
                        <asp:ListItem>国有土地使用权协议出让</asp:ListItem>
                        <asp:ListItem>国有土地使用权公开出让</asp:ListItem>
                        <asp:ListItem>国有土地使用权转让</asp:ListItem>
                        <asp:ListItem>国有土地使用权租赁</asp:ListItem>
                        <asp:ListItem>国有土地使用权变更</asp:ListItem>
                        <asp:ListItem>闲置土地处置</asp:ListItem>
                        <asp:ListItem>地籍调查前置</asp:ListItem>
                        <asp:ListItem>确权登记</asp:ListItem>
                        <asp:ListItem>权属争议调查处理</asp:ListItem>
                        <asp:ListItem>黄土资源</asp:ListItem>
                    </asp:CheckBoxList></td>
            </tr>
            <tr>
                <td colspan="4" rowspan="1" style="text-align: center">
                    <asp:Button ID="btnOk" runat="server" Text="确定" Width="71px" OnClick="btnOk_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" Width="65px" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="重置" Width="69px" OnClick="btnReset_Click" /></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
