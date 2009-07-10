<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchresult.aspx.cs" Inherits="searchresult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>搜索结果</title>
    <script type ="text/javascript">
    function TransferXmlDoc(xmlDoc){
        OpenWin.args=[];
        OpenWin.args.push("Hello,I am here"); 
        window.setTimeout(OpenWin,5000);  
        //child.displayResult(xmlDoc);    
        //child.testJs();
    }
    function OpenWin(argument){
        if(!argument){
           argument=OpenWin.args[0];            
        }        
        var childWin=window.open("display.aspx");
        childWin.displayResult(argument); 
    }    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
                    结果页
        <table width="100%" height="100%">
            <tr>
                <td id="TD5" runat="server" style="text-align: left;">
               <%=searchResult %>
                </td>
            </tr>
        </table>
    
    </div>
        <asp:ListBox ID="lbXml" runat="server" Visible="False"></asp:ListBox>
    </form>
</body>
</html>
