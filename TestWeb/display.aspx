<%@ Page Language="C#" AutoEventWireup="true" CodeFile="display.aspx.cs" Inherits="display" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script type="text/javascript" src="Silverlight.js"></script>

    <script type="text/javascript">
    <!--
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
        var time;
        function displayResult(result) {
            if (!result)
            {
                result=displayResult.args[0];
            } 
            var a=decodeURIComponent(result);
            var xml=a.replace(/\+/g," ");
            document.getElementById("hiddenXml").value=xml;
            time = setTimeout(getControl,500);
        }
       
      function getControl()
      {
           try{
               document.getElementById("SilverlightControl").content.map.SetMap(document.getElementById("hiddenXml").value);
               clearTimeout(time);
           }
           catch(err){
              alert(document.getElementById("hiddenXml").value);
              txt="此页面存在一个错误。\n";
              txt+="错误信息: " + err.message+"\n";             
              txt+="错误描述: " + err.description + "\n";
              txt+="点击OK继续。\n";
              alert(txt);
          }
                      
      }  
       function SC_OnFocus(){
           alert( document.getElementById("hiddenXml").value);
           document.getElementById("SilverlightControl").content.map.SetMap(document.getElementById("hiddenXml").value);
       } 
       function form_onactivate(){
           alert("form_onactivate") ;
       }  
        //--> 
    </script>
</head>
<body>
    <form id="form1" runat="server" onactivate="form_onactivate">
    <div>
        <input id="hiddenXml" type="hidden" />
        <object id="SilverlightControl" data="data:application/x-silverlight-2," type="application/x-silverlight-2"
            width="100%" height="100%" onload="SC_OnFocus">
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
    </div>
    </form>
</body>
</html>
