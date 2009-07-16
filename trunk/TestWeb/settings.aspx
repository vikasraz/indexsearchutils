<%@ Page Language="C#" AutoEventWireup="true" CodeFile="settings.aspx.cs" Inherits="settings" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>设置</title>
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <link href="<%= ConfigurationManager.AppSettings["CSS_QueryPage"] %>" rel="stylesheet" type="text/css" />
    <link href="<%= ConfigurationManager.AppSettings["CSS_ResourceStyle"] %>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .LargeTitle
        {
            font-size:13pt;
            font-family:宋体;            
        }
        .SmallTitle
        {
            font-size:10pt;
            font-family:宋体;            
        }    
        .MouseDown
        {
            color:Red;
        }
    </style>
    <base target="_self"></base>
</head>
<body bgcolor="white">
    <form id="form1" runat="server">
    <div style="text-align: center">
        <div style="width: 100px; height: 100px; text-align: left">
           <table width="100%" height="100%" border="0" cellpadding="0" cellspacing ="0">
               <tr>
                   <td>
                        <asp:CheckBoxList ID="checkListDetails" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" Width="500px">
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
               <tr>
                   <td style=" text-align:center;">
                       <asp:Button ID="btnAllSel" runat="server" Text="全  选"  CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro" OnClick="btnAllSel_Click"  />
                       <asp:Button ID="btnUnSel" runat="server" Text="全不选"  CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro" OnClick="btnUnSel_Click"  />
                       <asp:Button ID="btnOK" runat="server" Text="确  定"  CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro" OnClick="btnOK_Click" />
                   </td>
               </tr>
           </table> 
       </div>
    </div>
    </form>
</body>
</html>
