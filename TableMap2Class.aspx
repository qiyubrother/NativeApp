<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TableMap2Class.aspx.cs" Inherits="TableMap2Class" %>

<%@ Register Src="~/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="/Scripts/jquery-1.9.1.min.js"></script>
    <script src="/Scripts/bootstrap.min.js"></script>
    <script src="/Scripts/clipboard.min.js"></script>
    <link rel="stylesheet" href="Content/bootstrap.min.css" />

    <link href="/Content/highlight.default.css" rel="stylesheet"/>  
    <script src="/Scripts/highlight.pack.js"></script>
    <link href="/Content/vs.css" rel="stylesheet" />
    <script >hljs.initHighlightingOnLoad();</script>  
    <title>Table Map to Class</title>
<script>
    $(function() {
        var clipboard = new Clipboard('.btnCopy');
    });

    function checkInput() {
        if ($("#connectionString").val() === "") {
            alert("Invalid Connection String.");
            return false;
        }
        if ($("#tableName").val() === "") {
            alert("Invalid table name.");
            return false;
        }

        return true;
    }
</script>
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
    <h1>Table Map to Class</h1>
    <form id="form1" runat="server">
	 	<fieldset>
			<div class="form-group">
					<label for="connectionString">Connection String:</label>
					<input type="text" class="form-control" id="connectionString" runat="server" />
					<label for="tableName">Table Name:</label>
					<input type="text" class="form-control" id="tableName" runat="server" value="" />
                    <div class="text-center">
					    <br/>
                        <a href="#" id="testConnection" class="btn btn-default" runat="server" OnServerClick="testConnection_OnServerClick">Test Connection</a>&nbsp;
                        <a href="#" id="doWork" class="btn btn-success" runat="server" onclick="if (!checkInput()) return false;" OnServerClick="doWork_OnServerClick" >&nbsp;&nbsp;&nbsp;&nbsp;Do&nbsp;&nbsp;&nbsp;&nbsp;</a>&nbsp;
                        <a href="#" id="clearAll" class="btn btn-default" runat="server" OnServerClick="clearAll_OnServerClick">Clear</a><br/><br/>
                    </div>
                    <label for="sqlResult">SQL Result:</label>&nbsp;<a class="btnCopy btn btn-default btn-sm" data-clipboard-target="#sqlResult">Copy code</a>
                    <div style="min-height: 400px">
                        <pre id="sqlResult" runat="server" class="C#"></pre>
                    </div>
			</div>
		</fieldset>
    </form>
</body>
</html>
