<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DBFViewer.aspx.cs" Inherits="DBFViewer" %>

<%@ Register Src="~/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>DBF Viewer</title>
	<script type="text/javascript" src="Scripts/jquery-1.9.1.min.js" ></script>
	<script type="text/javascript" src="Scripts/bootstrap.min.js" ></script>
	<link rel="stylesheet" href="Content/bootstrap.min.css" />
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
    <form id="form1" runat="server">
    <div>
	    <div class="form-group center-block" style="width: 1200px; text-align: center; overflow: auto; border: 0px solid red;">
		    <h3>
			    DBF Viewer
		    </h3>
            <asp:FileUpload ID="FileUpload1" runat="server" style="width: 100%" />
            <br />
            <asp:Button ID="read" runat="server" Text="View" CssClass="btn btn-default" OnClick="read_Click" />
            <br /> <br />
            <asp:Table ID="tbl" runat="server" cssClass="table table-hover table-bordered"></asp:Table>
	    </div>
    </div>
    </form>
</body>
</html>
