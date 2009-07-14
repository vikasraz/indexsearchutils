<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchsetting.aspx.cs" Inherits="searchsetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>搜索设置</title>
    
     <link href="<%= ConfigurationManager.AppSettings["CSS_QueryPage"] %>" rel="stylesheet" type="text/css" />
     
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center; font-size:9pt">
        <br />        
        <br />        
        <table class="TableStyle" cellpadding="0" cellspacing ="0" width="650px">
            <tr>
                <td class="TableText" colspan="3" style="height: 50px"><span class="LabelStyle">搜索设置</span></td>
            </tr>
            <tr>
                <td width="150px" style="height: 30px" class="TableText">
                    结果设置
                </td>
                <td width="200px" style="height: 30px" class="TableValue">默认设置（10项）最有效且最快</td>
                <td width="300px" style="height: 30px;text-align:left" class="TableValue" >                   
                    <asp:DropDownList ID="dropListPageSize" runat="server" Width="220px" >
                        <asp:ListItem Value="10">10项结果</asp:ListItem>
                        <asp:ListItem Value="20">20项结果</asp:ListItem>
                        <asp:ListItem Value="30">30项结果</asp:ListItem>
                        <asp:ListItem Value="50">50项结果</asp:ListItem>
                        <asp:ListItem Value="100">100项结果</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="150px" class="TableText" rowspan="2">
                    区域设置
                </td>
                <td colspan="2" style="height: 30px ; text-align:left" class="TableValue">
                    <asp:RadioButtonList ID="radioListArea" runat="server" RepeatDirection="Horizontal" Width="284px" >
                        <asp:ListItem Selected="True">所有区域(建议)</asp:ListItem>
                        <asp:ListItem>自定义区域</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 30px; text-align:left" class="TableValue">
                    <asp:CheckBoxList ID="checkListArea" runat="server" Height="1px" RepeatDirection="Horizontal"
                        Width="390px" >
                        <asp:ListItem>华民镇</asp:ListItem>
                        <asp:ListItem>新立街</asp:ListItem>
                        <asp:ListItem>么六桥</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td width="150px" class="TableText" rowspan="2">
                    内容设置
                </td>
                <td colspan="2" style="height: 30px; text-align:left" class="TableValue" >
                    <asp:RadioButtonList ID="radioListDetails" runat="server" RepeatDirection="Horizontal" Width="284px" >
                        <asp:ListItem Selected="True">所有内容(建议)</asp:ListItem>
                        <asp:ListItem>自定义内容</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 30px; text-align:left" class="TableValue">
                    <asp:CheckBoxList ID="checkListDetails" runat="server" RepeatDirection="Horizontal" Height="1px" Width="499px" RepeatColumns="6" >
                        <asp:ListItem>土地利用规划</asp:ListItem>
                        <asp:ListItem>地籍权属</asp:ListItem>
                        <asp:ListItem>耕地保护</asp:ListItem>
                        <asp:ListItem>土地利用</asp:ListItem>
                        <asp:ListItem>执法监察</asp:ListItem>
                        <asp:ListItem>整理开发复垦</asp:ListItem>
                        <asp:ListItem>法律法规</asp:ListItem>
                        <asp:ListItem>综合办公</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>
    
        <br />        
        
        <table border ="0" width="750px" cellpadding="0" cellspacing="0">
            <tr>
                <td width="175px"></td>
                <td width="100px">
                    <asp:Button ID="btnOk" runat="server" Text="确定" OnClick="btnOk_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro"/>
                </td>
                    <td width="100px">
                <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro"/>
                </td>
                    <td width="100px">
                <asp:Button ID="btnReset" runat="server" Text="重置" OnClick="btnReset_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro"/>
                </td>
                <td width="175px"></td>
            </tr>        
        </table>
    
    </div>
    </form>
</body>
</html>
