<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Parant.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script>
    function openchild()
    {
        var child=window.open("child.aspx");
       child.test('dddd'); 
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <input type=button onclick="openchild()" value="click" />
    </div>
    </form>
</body>
</html>
