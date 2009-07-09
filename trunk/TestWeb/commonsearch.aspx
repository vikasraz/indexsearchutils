<%@ Page Language="C#" AutoEventWireup="true" CodeFile="commonsearch.aspx.cs" Inherits="commonsearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>国土资源综合搜索</title>
    <script type="text/javascript">
    function ontxtSearchFocus(){
        document.getElementById("txtSearch").value="";
    }
    function ontxtSearchKeyPress(){
        if (event.keyCode==13){
            var url="searchresult.aspx?WordsAllContains="+document.getElementById("txtSearch").value;
            window.open(url,"搜索结果");
        }
    }   
    </script>
</head>
<body>
    <form id="form1" runat="server" method="post">
    <div style="text-align: center">
        <table style="width: 865px; height: 224px">
            <tr>
                <td colspan="4" style="height: 66px; text-align: center">
                    国土资源综合搜索</td>
            </tr>
            <tr>
                <td colspan="3" rowspan="5" style="width: 855px; height: 189px; text-align: left">
                    &nbsp;<input id="txtSearch" runat="server" onfocus="ontxtSearchFocus()" onkeypress="ontxtSearchKeyPress()" style="width: 747px" type="text" value="请输入搜索关键词" />
                    <asp:ImageButton ID="imgBtnSearch" runat="server" Height="18px" ImageUrl="~/App_GlobalResources/search.jpg" Width="18px" OnClick="imgBtnSearch_Click" AlternateText="搜索" /><br>
                    <div id="divSearch" style="width: 764px; height: 167px; text-align: left" atomicselection="true">
                    </div>
                </td>
                <td colspan="1" rowspan="5" style="width: 26px; text-align: left; height: 189px;">
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/profsearch.aspx" Height="16px" Width="64px">高级搜索</asp:HyperLink><br>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/searchsetting.aspx" Height="18px" Width="68px">搜索设置</asp:HyperLink>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
