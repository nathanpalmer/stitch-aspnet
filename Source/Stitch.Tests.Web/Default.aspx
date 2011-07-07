<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Stitch.Tests.Web.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test</title>
    <script type="text/javascript" src="/test.stitch"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var Test = require("controllers/test");
            var test = new Test();
            test.append();
        })
    </script>
</head>
<body>
    <div id="content"></div>
</body>
</html>
