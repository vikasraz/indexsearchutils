<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchsetting.aspx.cs" Inherits="searchsetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>搜索设置</title>
    <meta http-equiv="x-ua-compatible" content="ie=7" />
     <link href="<%= ConfigurationManager.AppSettings["CSS_QueryPage"] %>" rel="stylesheet" type="text/css" />
    <base target="_self"></base>
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
                    <asp:CheckBoxList ID="checkListArea" runat="server" RepeatColumns="5"  RepeatDirection="Horizontal" Height="1px" Width="499px" >
                        <asp:ListItem>万新街</asp:ListItem>
                        <asp:ListItem>幺六桥</asp:ListItem>
                        <asp:ListItem>军粮城镇</asp:ListItem>
                        <asp:ListItem>新立街</asp:ListItem>
                        <asp:ListItem>无瑕街</asp:ListItem>
                        <asp:ListItem>华明镇</asp:ListItem>
                        <asp:ListItem>金钟街道</asp:ListItem>
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
                    <asp:CheckBoxList ID="checkListDetails" runat="server" RepeatDirection="Horizontal" Height="1px" Width="499px" RepeatColumns="3" >
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
