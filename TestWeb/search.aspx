<%@ Page Language="C#" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="searchresult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>资源搜索</title>
    
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <link href="<%= ConfigurationManager.AppSettings["CSS_QueryPage"] %>" rel="stylesheet" type="text/css" />
    <link href="<%= ConfigurationManager.AppSettings["CSS_ResourceStyle"] %>" rel="stylesheet" type="text/css" />
    
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
<body onload="Init();" bgcolor="white">
    <form id="form1" runat="server">
     <div style="text-align:center;width:100%" id="divTalbe">
     <table border="0" width ="100%" cellpadding="0" cellspacing ="0">
        <tr>
            <td width="3%">&nbsp;</td>
            <td width="94%">         
                <table border="0" width="800px" cellpadding="0" cellspacing ="0">
                    <tr>
                        <td colspan="4">
                             <span class="LabelStyle">国 土 资 源 综 合 搜 索</span>
                             <br /><br />
                        </td>                       
                    </tr>
                    <tr height="35px">
                        <td width="80px" style="text-align:right">
                        </td>
                        <td style="width: 600px">
                            <asp:TextBox ID="txtSearch" Width="98%" runat="server" AutoPostBack="true"  OnTextChanged="txtSearch_TextChanged" >东丽</asp:TextBox>
                        </td>                       
                        <td width="60px" style="text-align:right">
                            <a id="HyperLink1" href="profsearch.aspx" style="display:inline-block;width:50px;font-size:9pt">高级搜索</a>
                        </td>
                        <td width="60px" style="text-align:right">
                            <a id="HyperLink2" href="searchsetting.aspx" style="display:inline-block;width:50px;font-size:9pt">搜索设置</a>                            
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" colspan="4">
                            <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" CssClass="BtnStyle" onmouseover="this.className='BtnOverStyle'" onmouseout="this.className='BtnStyle'" BackColor="Gainsboro" />
                        </td>
                    </tr>
                </table>   
                 
                
                <br />
                <table class="TableStyle" border="1" cellpadding="0" cellspacing="0" width="100%" style="border-bottom:none; border-left:none; border-right:none">
                    <tr>
                        <td id="Number" runat="server" style="text-align:right; background-color:#F3F9FF; height:30px; border:none; font-family:宋体; font-size:13pt; color:#014F8A"></td>
                    </tr>
                </table>
                <br />
                
                
                <table border="0" cellpadding ="5" cellspacing ="5" width="100%">
                    <tr>
                       <td width="70%" id="tdResult" runat="server" style="text-align:left; vertical-align:top; line-height:25px"></td> 
                       <td width="8%"></td> 
                       <td width="22%" id="tdStatis" runat="server" style="text-align:left; vertical-align:top"></td> 
                    </tr>
                    <tr>
                        <td colspan="3" id="tdPageSet" runat="server" ></td>
                    </tr>
                </table> 
                
            </td>
            <td width="3%">&nbsp;</td>
        </tr>     
     </table>
    </div>
    <input id="txtWords" type="hidden" runat="server" style="width: 6px" />
    </form>
</body>
</html>
