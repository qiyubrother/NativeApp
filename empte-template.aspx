<%@ Page Language="C#" %>
<%@ Register Src="~/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<!DOCTYPE html>
<script runat="server">
    string title = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title><%=title %></title>
    <script src="/Scripts/jquery-1.9.1.min.js"></script>
    <script src="/Scripts/bootstrap.min.js"></script>
    <script src="/Scripts/clipboard.min.js"></script>
    <link rel="stylesheet" href="/Content/bootstrap.min.css" />
    <link href="/Content/highlight.default.css" rel="stylesheet"/>  
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
<h1>
    <%=title %>
</h1>
<pre>

</pre>
</body>
</html>
