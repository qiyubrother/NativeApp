<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTable2Json.aspx.cs" Inherits="_DataTable2Json" %>

<%@ Register Src="~/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<script type="text/javascript" src="Scripts/jquery-1.9.1.min.js" ></script>
	<script type="text/javascript" src="Scripts/bootstrap.min.js" ></script>
	<link rel="stylesheet" href="Content/bootstrap.min.css" />
    <title>DataTable to JSON</title>
    <style>
        .min-height{
            min-height:400px;
        }
    </style>
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
    <form id="form1" runat="server">
    <div>
	    <div class="form-group center-block" style="width: 600px; text-align: center;">
		    <h3>
			    Data talbe to json
		    </h3>
		    <span>MS-SQL Server</span>
            <asp:TextBox ID="connectionString" name="connectionString" runat="server" cssClass="form-control" Text="Data Source=10.33.1.140;Initial Catalog=DocSystem;Integrated Security=False;User Id=worleyparsons;Password=MaisonWP;"></asp:TextBox><br />
            <asp:TextBox ID="tableName" name="tableName" runat="server" Text="Source" cssClass="form-control"></asp:TextBox><br />
            <asp:Button ID="getJsonData" runat="server" Text="Get Json Data" CssClass="btn btn-default" OnClick="getJsonData_Click" />
            <br /> <br />
            <asp:TextBox ID="jsonDataContainer" runat="server" TextMode="MultiLine" cssClass="form-control min-height"> </asp:TextBox>
	    </div>
    </div>

    </form>
</body>
</html>
