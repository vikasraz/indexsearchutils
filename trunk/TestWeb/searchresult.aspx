<%@ Page Language="C#" AutoEventWireup="true" CodeFile="searchresult.aspx.cs" Inherits="searchresult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>搜索结果</title>
<script type="text/javascript" src="Silverlight.js"></script>

    <script type="text/javascript">
        function onSilverlightError(sender, args) {

            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }
            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            var errMsg = "Silverlight 2 应用程序中未处理的错误 " + appSource + "\n";

            errMsg += "代码: " + iErrorCode + "    \n";
            errMsg += "类别: " + errorType + "       \n";
            errMsg += "消息: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "文件: " + args.xamlFile + "     \n";
                errMsg += "行号: " + args.lineNumber + "     \n";
                errMsg += "位置: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "行号: " + args.lineNumber + "     \n";
                    errMsg += "位置: " + args.charPosition + "     \n";
                }
                errMsg += "方法名称: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
        function mousePosition(ev) {
            if (ev.pageX || ev.pageY) {
                return { x: ev.pageX, y: ev.pageY };
            }
            return {
                x: ev.clientX + document.body.scrollLeft - document.body.clientLeft,
                y: ev.clientY + document.body.scrollTop - document.body.clientTop
            };
        }
        
        function testJs() {
            document.getElementById("SilverlightControl").content.map.SetMap('东丽国土');
        }
       function searchFunc(words,index){
            document.getElementById("SilverlightControl").content.map.SetMap(words, index);
       }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
                    结果页
        <table width="100%" height="100%">
            <tr>
                <td id="TD5" runat="server" style="width: 258px; height: 320px; text-align: left;">
               <%=searchResult %>
                </td>
                <td >
   
                </td>
            </tr>
        </table>
    
    </div>
     <div id="silverlightControlHost" style="text-align: left">
        <object id="SilverlightControl" data="data:application/x-silverlight-2," type="application/x-silverlight-2"
            width="100%" height="100%">
            <param name="source" value="ClientBin/SilverlightApplication.xap" />
            <param name="onerror" value="onSilverlightError" />
            <param name="background" value="white" />
            <param name="minRuntimeVersion" value="2.0.31005.0" />
            <param name="autoUpgrade" value="true" />
            <a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;">
                <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="获取 Microsoft Silverlight"
                    style="border-style: none" />
            </a>
        </object>
        <iframe style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe>
    </div>
    </form>
</body>
</html>
