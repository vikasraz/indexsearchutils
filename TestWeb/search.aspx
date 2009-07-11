<%@ Page Language="C#" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="searchresult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>搜索结果</title>
    <script type ="text/javascript">
    function TransferString(str){
        var childWin=window.open("display.aspx");
        childWin.attachEvent("onload", function(){childWin.displayResult(str)});
    }
    function ontxtSearchKeyPress(){
        if (event.keyCode==13){
        debugger; 
            var words=document.getElementById("txtSearch").value.split(" ");
            var allContainArray=new Array();
            var notIncludeArray=new Array();
            for(word in words){
               if(word.substr(0,1)=="-")
                   notIncludeArray.push(word.substring(1));
               else
                   allContainArray.push(word); 
            }
            var allContains="";
            var notIncludes="";
            for(contain in allContainArray){
               allContains+=contain+" ";
            }
            for(exclude in notIncludeArray){
               notIncludes+=exclude+" ";
            }
            var url="search.aspx?WordsAllContains="+escape(allContains)+"&WordNotInclude="+escape(notIncludes);
            this.location.href = url;
            window.navigate(location) ;
        }
    }   
    function onbtnSearchClick(){
       debugger; 
        var words=document.getElementById("txtSearch").value.split(" ");
        var allContainArray=new Array();
        var notIncludeArray=new Array();
        for(word in words){
           if(word.substr(0,1)=="-")
               notIncludeArray.push(word.substring(1));
           else
               allContainArray.push(word); 
        }
        var allContains="";
        var notIncludes="";
        for(contain in allContainArray){
           allContains+=contain+" ";
        }
        for(exclude in notIncludeArray){
           notIncludes+=exclude+" ";
        }
        var url="search.aspx?WordsAllContains="+escape(allContains)+"&WordNotInclude="+escape(notIncludes);
        this.location.href = url;
        window.navigate(location) ;
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: left">
        <table width="100%" height="100%">
            <tr height="24px">
                <td runat="server" style="width: 3%; height: 4px; text-align: left">
                </td>
                <td runat="server" colspan="2" style="width: 96%;height: 24px; text-align: left">
                    <div style="width: 100%;height: 24px; text-align: left"><p style="text-align: center">
                        国土资源综合搜索</p><p>
                        <asp:TextBox ID="txtSearch" Width="70%" runat="server" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" ></asp:TextBox>
                        <%--<input id="txtSearch" style="width: 70%" language="javascript" onkeypress="ontxtSearchKeyPress()" type="text" />--%>
                        <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
                        <%--<input id="btnSearch" type="submit" language="javascript" onclick="return onbtnSearchClick()" value="搜索" />--%>
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
                <td runat="server" style="width: 3%; text-align: left">
                </td>
                <td id="tdResult" runat="server" style="text-align: left; width: 70%;">
                </td>
                <td id="tdStatis" runat="server" style="width: 24%; text-align: left">
                </td>
                <td runat="server" style="width: 3%;text-align: left">
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
