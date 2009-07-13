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
                        <asp:ListItem Value="10">10项结果</asp:ListItem>
                        <asp:ListItem Value="20">20项结果</asp:ListItem>
                        <asp:ListItem Value="30">30项结果</asp:ListItem>
                        <asp:ListItem Value="50">50项结果</asp:ListItem>
                        <asp:ListItem Value="100">100项结果</asp:ListItem>
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
        </table>
    
    </div>
    </form>
</body>
</html>
