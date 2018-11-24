<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DBF2JSON.aspx.cs" Inherits="DBF2JSON" %>

<%@ Register Src="~/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<script type="text/javascript" src="Scripts/jquery-1.9.1.min.js" ></script>
	<script type="text/javascript" src="Scripts/bootstrap.min.js" ></script>
	<link rel="stylesheet" href="Content/bootstrap.min.css" />
    <title>DBF to JSON</title>
<style>
    .min-height {
        min-height: 400px;
    }
</style>
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
    <form id="form1" runat="server">
    <div>
	    <div class="form-group center-block" style="width: 1000px; text-align: center;">
		    <h3>
			    DBF file to json
		    </h3>
            <asp:FileUpload ID="FileUpload1" runat="server" style="width:100%"/>
            <br />
            <asp:Button ID="getJsonData" runat="server" Text="Get Json Data" CssClass="btn btn-default" OnClick="getJsonData_Click" />
            <br /> <br />
            <asp:TextBox ID="jsonDataContainer" runat="server" TextMode="MultiLine" cssClass="form-control min-height"> </asp:TextBox>
	    </div>
    </div>
    </form>
</body>
</html>
