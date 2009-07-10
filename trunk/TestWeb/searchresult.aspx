<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchresult.aspx.cs" Inherits="searchresult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>搜索结果</title>
    <script type ="text/javascript">
    function TransferString(str){
        var childWin=window.open("display.aspx");
//       do{;} while(!childWin.document.head);
//       childWin.displayResult(str); 
        childWin.attachEvent("onload", function(){childWin.displayResult(str)});
//       childWin.attachEvent("onload", function(){      dtBegin = new Date();
//         do{;}while((new Date())-dtBegin < 1000);childWin.displayResult(str)});
        //childWin.displayResult(str); 
        //child.displayResult(xmlDoc);    
        //child.testJs();
//       childWin.attachEvent("onload", function(){childWin.addLoadEvent(function(){childWin.displayResult(str)})});
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
               
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
