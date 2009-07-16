<%@ Page Language="C#" AutoEventWireup="true" CodeFile="viewsearch.aspx.cs" Inherits="minisearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>搜索</title>
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <link href="<%= ConfigurationManager.AppSettings["CSS_QueryPage"] %>" rel="stylesheet" type="text/css" />
    <link href="<%= ConfigurationManager.AppSettings["CSS_ResourceStyle"] %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript" src="cookie.js"></script>
    <script type="text/javascript">
        function TransferString(str)
        {
            var style="channelmode=1,alwaysRaised=1,depended=0,location=1,menubar=1,resizable=1,titlebar=1,toolbar=1,status=1";
            var childWin=window.open("display.aspx",'',style);
            childWin.attachEvent("onload", function(){childWin.displayResult(str)});
        }
        function Init()
        { 
            document.getElementById("txtSearch").focus();
        }
        
        function OpenMessage(url)
        {
            window.open(url,"newwindow","height="+window.screen.height+", width="+window.screen.width+", top=0, left=0, toolbar=no, menubar=no, scrollbars=yes, resizable=yes,location=no, status=no"); 
        }
    </script>
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
</head>
<body style="text-align: center" onload="Init();" bgcolor="white">
    <form id="form1" runat="server">
    <div id="divTable" style="width: 600px; height: 100%;" >
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing ="0">
           <tr>
               <td width="3%" height="100%" ></td>
               <td width="94%" height="100%">
                   <table width="100%" height="100%" border="0" cellpadding="0" cellspacing ="0">
                       <tr height="35px" >
                          <td width="30px"></td>
                          <td  width="300px">
                             <asp:TextBox ID="txtSearch" Width="98%" runat="server"  OnTextChanged="txtSearch_TextChanged" ></asp:TextBox>
                          </td>
                          <td width="50px" style="text-align:right">
                              <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro" />
                          </td>
                       </tr>
                       <tr height="20px" >
                          <td id="Number" runat="server" style="text-align:right; background-color:#F3F9FF; height:12px; border:none; font-family:宋体; font-size:9pt; color:#014F8A" colspan="4" >
                          </td>
                       </tr>
                       <tr>
                          <td id="tdResult" runat="server" colspan="3" style="text-align: left" >
                          </td>
                       </tr>
                       <tr>
                           <td id="tdPageSet" runat="server" colspan="3">
                           </td>
                       </tr>
                   </table>
               </td>
               <td width="3%" height="100%">
               </td>
           </tr>
        </table>
    </div>
    <asp:Label ID="setting" runat="server"></asp:Label>
    </form>
</body>
</html>
