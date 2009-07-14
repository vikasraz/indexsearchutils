<%@ Page Language="C#" AutoEventWireup="true" CodeFile="profsearch.aspx.cs" Inherits="profsearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>高级搜索</title>
    
    <link href="<%= ConfigurationManager.AppSettings["CSS_QueryPage"] %>" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center; font-size:9pt">        
        <br />        
        <br />  
        <table class="TableStyle" width="700px" cellpadding="0" cellspacing ="0">
            <tr>
                <td class="TableText" colspan="3" style="height: 50px"><span class="LabelStyle">高级搜索</span></td>
            </tr>
            <tr>
                <td class="TableText" width="150px" rowspan="4">常规设置</td>
                <td class="TableValue" width="150px" height="30px">包含完整的词</td>
                <td class="TableValue" width="400px" height="30px"><asp:TextBox ID="txtWordsAllContains" runat="server" Width="380px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="TableValue" width="150px" height="30px">包含完整的字句</td>
                <td class="TableValue" width="400px" height="30px"><asp:TextBox ID="txtExactPhraseContain" runat="server" Width="380px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="TableValue" width="150px" height="30px">包含至少一个词</td>
                <td class="TableValue" width="400px" height="30px"><asp:TextBox ID="txtOneOfWordsAtLeastContain" runat="server" Width="380px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="TableValue" width="150px" height="30px">不包含字词</td>
                <td class="TableValue" width="400px" height="30px"><asp:TextBox ID="txtWordNotInclude" runat="server" Width="380px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="TableText" width="150px" rowspan="2">区域设置</td>
                <td class="TableValue" height="30px" colspan="2" style="text-align:left">
                    <asp:Button ID="btnAllSelArea" runat="server" Text="全选" OnClick="btnAllSelArea_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro"/>
                    <asp:Button ID="btnAllUnSelArea" runat="server" Text="全不选" OnClick="btnAllUnSelArea_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro"/></td>
            </tr>
            <tr>
                <td class="TableValue" height="30px" colspan="2" style="text-align:left">
                    <asp:CheckBoxList ID="checkListArea" runat="server" RepeatDirection="Horizontal"><asp:ListItem Selected="True">华民镇</asp:ListItem>
                        <asp:ListItem Selected="True">新立街</asp:ListItem>
                        <asp:ListItem Selected="True">么六桥</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td class="TableText" width="150px" rowspan="2">内容设置</td>
                <td class="TableValue" height="30px" colspan="2" style="text-align:left">
                    <asp:Button ID="btnAllSelDetails" runat="server" Text="全选" OnClick="btnAllSelDetails_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro"/>
                    <asp:Button ID="btnAllUnSelDetails" runat="server" Text="全不选" OnClick="btnAllUnSelDetails_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro"/>
                </td>
            </tr>
            <tr>
                <td class="TableValue" height="30px" colspan="2" style="text-align:left">
                    <asp:CheckBoxList ID="checkListDetails" runat="server" RepeatColumns="6" RepeatDirection="Horizontal"><asp:ListItem Selected="True">土地利用规划</asp:ListItem>
                        <asp:ListItem Selected="True">地籍权属</asp:ListItem>
                        <asp:ListItem Selected="True">耕地保护</asp:ListItem>
                        <asp:ListItem Selected="True">土地利用</asp:ListItem>
                        <asp:ListItem Selected="True">执法监察</asp:ListItem>
                        <asp:ListItem Selected="True">整理开发复垦</asp:ListItem>
                        <asp:ListItem Selected="True">法律法规</asp:ListItem>
                        <asp:ListItem Selected="True">综合办公</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>
        
        <br />
        
        <table border="0" width="700px">
            <tr>
                <td width="350px" style="text-align:right">
                    <asp:DropDownList ID="dropListPageSize" runat="server" Width="129px">
                        <asp:ListItem Value="10">10项结果</asp:ListItem>
                        <asp:ListItem Value="20">20项结果</asp:ListItem>
                        <asp:ListItem Value="30">30项结果</asp:ListItem>
                        <asp:ListItem Value="50">50项结果</asp:ListItem>
                        <asp:ListItem Value="100">100项结果</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="350px" style="text-align:left">
                    <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro"/>
                </td>
            </tr>
        </table>
        
       
                    
    </div>
    </form>
</body>
</html>
