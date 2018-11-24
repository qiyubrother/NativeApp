<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OracleTable2ADODotNetIUD.aspx.cs" Inherits="OracleTable2ADODotNetIUD" %>

<%@ Register Src="~/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Table to ADO.Net IUD</title>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/clipboard.min.js"></script>
    <link rel="stylesheet" href="Content/bootstrap.min.css" />

    <link href="Content/highlight.default.css" rel="stylesheet"/>  
    <script src="Scripts/highlight.pack.js"></script>
    <link href="Content/vs.css" rel="stylesheet" />
    <script >hljs.initHighlightingOnLoad();</script>  
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
    function checkInputPK() {
        if ($("#primaryKey").val() === "") {
            alert("Invalid primary key.");
            return false;
        }

        return true;
    }
</script>
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
    <h1>Classic SQL For ADO.Net</h1>
    <form id="form1" runat="server">
    <div>
	 	<fieldset>
			<div class="form-group">
					<label for="connectionString">Connection String:</label>
					<input type="text" class="form-control" id="connectionString" runat="server" />
					<label for="tableName">Table Name:</label>
					<input type="text" class="form-control" id="tableName" runat="server" value="MMM_BORROW" />
					<label for="primaryKey">Primary Key(s):</label>
					<input type="text" class="form-control" id="primaryKey" runat="server" value="" />
                    <div class="help-block">Sapmle:Id, Suppl</div>
                    <div class="text-center">
					    <br/>
                        <a href="#" id="testConnection" class="btn btn-default" runat="server" OnServerClick="testConnection_OnServerClick">Test Connection</a>&nbsp;
                        <a href="#" id="doSelect" class="btn btn-primary" runat="server" onclick="if (!checkInput()) return false;" OnServerClick="doSelect_OnServerClick" >Select statement</a>&nbsp;
                        <a href="#" id="doInsert" class="btn btn-success" runat="server" onclick="if (!checkInput()) return false;" OnServerClick="doInsert_OnServerClick" >Insert statement</a>&nbsp;
                        <a href="#" id="doUpdate" class="btn btn-warning" runat="server" onclick="if (!(checkInput() && checkInputPK())) return false;" OnServerClick="doUpdate_OnServerClick" >Update statement</a>&nbsp;
                        <a href="#" id="doDelete" class="btn btn-info" runat="server" onclick="if (!(checkInput() && checkInputPK())) return false;" OnServerClick="doDelete_OnServerClick" >Delete statement</a>&nbsp;

                        <a href="#" id="clearAll" class="btn btn-default" runat="server" OnServerClick="clearAll_OnServerClick">Clear</a><br/><br/>
                    </div>
                    <label for="sqlResult">Result:</label>&nbsp;<a class="btnCopy btn btn-default btn-sm" data-clipboard-target="#sqlResult">Copy code</a>
                    <div style="min-height: 400px">
                        <pre id="sqlResult" runat="server" class="C#"></pre>
                    </div>
			</div>
		</fieldset>
    </div>
    </form>
</body>
</html>
