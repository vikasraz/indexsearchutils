<%@ Page Language="C#" AutoEventWireup="true" CodeFile="commonsearch.aspx.cs" Inherits="commonsearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>国土资源综合搜索</title>
    <script type="text/javascript">
    <!--
    function ontxtSearchFocus(){
        if (document.getElementById("txtSearch").value=="请输入搜索关键词")
           document.getElementById("txtSearch").value="";
    }
    function ontxtSearchKeyPress(){
        if (event.keyCode==13){
       debugger; 
            var url="searchresult.aspx?WordsAllContains="+escape(document.getElementById("txtSearch").value);
            

           this.location.href = url;
            //alert(url);
            //window.open(url);
            //self.open(url,'_self');
            //window.location.replace(url);
        }
    }   
    //-->
    </script>
</head>
<body>
    <form id="form1" runat="server" method="post">
   
    <div style="text-align: center">
    
        <table style="width: 865px; height: 15px">
            <tr>
                <td colspan="2" style="height: 196px; text-align: center">
                    国土资源综合搜索</td>
            </tr>
            <tr>
                <td colspan="1" rowspan="6" style="width: 855px; text-align: left">
                    &nbsp;<input id="txtSearch"  onfocus="ontxtSearchFocus()" onkeypress="ontxtSearchKeyPress()" style="width: 747px; height: 16px;" type="text" value="请输入搜索关键词" />
                    <asp:ImageButton ID="imgBtnSearch" runat="server" Height="18px" ImageUrl="~/search.jpg" Width="18px" OnClick="imgBtnSearch_Click" AlternateText="搜索" /><br>
                </td>
                <td colspan="1" rowspan="1" style="width: 26px; height: 20px; text-align: left">
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/profsearch.aspx" Height="16px" Width="64px">高级搜索</asp:HyperLink></td>
            </tr>
            <tr>
                <td colspan="1" rowspan="5" style="width: 26px; text-align: left; height: 10px;">
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/searchsetting.aspx" Height="18px" Width="68px">搜索设置</asp:HyperLink>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
