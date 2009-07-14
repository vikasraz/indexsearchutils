<%@ Page Language="C#" AutoEventWireup="true" CodeFile="profsearch.aspx.cs" Inherits="profsearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>高级搜索</title>
    <meta http-equiv="x-ua-compatible" content="ie=7" />
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
                    <asp:CheckBoxList ID="checkListArea" runat="server" RepeatColumns="5" RepeatDirection="Horizontal" Height="1px" Width ="540px">
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
                <td class="TableText" width="150px" rowspan="2">内容设置</td>
                <td class="TableValue" height="30px" colspan="2" style="text-align:left">
                    <asp:Button ID="btnAllSelDetails" runat="server" Text="全选" OnClick="btnAllSelDetails_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro"/>
                    <asp:Button ID="btnAllUnSelDetails" runat="server" Text="全不选" OnClick="btnAllUnSelDetails_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro"/>
                </td>
            </tr>
            <tr>
                <td class="TableValue" height="30px" colspan="2" style="text-align:left">
                    <asp:CheckBoxList ID="checkListDetails" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" Width ="540px">
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
