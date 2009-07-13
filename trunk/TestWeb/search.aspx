<%@ Page Language="C#" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="searchresult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>搜索结果</title>
    <script type ="text/javascript">
    <!--
    function TransferString(str){
        var style="channelmode=1,alwaysRaised=1,depended=0,location=1,menubar=1,resizable=1,titlebar=1,toolbar=1,status=1";
        var childWin=window.open("display.aspx",'',style);
        childWin.attachEvent("onload", function(){childWin.displayResult(str)});
    }
    //-->
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: left">
        <table width="100%" height="100%">
            <tr height="24px">
                <td runat="server" style="width: 3%; height: 4px; text-align: left">
                </td>
                <td runat="server" colspan="3" style="width: 96%;height: 24px; text-align: left; vertical-align:top">
                    <div style="width: 100%;height: 24px; text-align: left"><p style="text-align: center">
                        国土资源综合搜索</p><p>
                        <asp:TextBox ID="txtSearch" Width="70%" runat="server" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" ></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
                        <a id="HyperLink1" href="profsearch.aspx" style="display:inline-block;width:70px;">高级搜索</a>
                        <a id="HyperLink2" href="searchsetting.aspx" style="display:inline-block;width:68px;">搜索设置</a>
                        <input id="txtWords" type="hidden" runat="server" style="width: 6px" /></p>
                    </div>
                   <hr />
                </td>
                <td runat="server" style="width: 3%;height: 4px; text-align: left">
                </td>
            </tr>
            <tr>
                <td runat="server" style="width: 3%; text-align: left" rowspan="2">
                </td>
                <td id="tdResult" runat="server" style="text-align: left; width: 70%;">
                </td>
                <td runat="server" rowspan="2" style="vertical-align: top; width: 4%; text-align: left">
                </td>
                <td id="tdStatis" runat="server" style="width: 20%; text-align: left; vertical-align:top" rowspan="2">
                </td>
                <td runat="server" style="width: 3%;text-align: left" rowspan="2">
                </td>
            </tr>
            <tr>
                <td id="tdPageSet" runat="server" style="width: 70%; text-align: center">
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
