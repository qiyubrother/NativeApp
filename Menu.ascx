<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Menu" %>
    <nav class="navbar navbar-default equinav" role="navigation">
      <div class="navbar-header">
        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
          <span class="icon-bar"></span>
          <span class="icon-bar"></span>
          <span class="icon-bar"></span>
        </button>
        <span class="navbar-brand">Native App</span>
      </div><!-- /.navbar-header -->
      <div class="collapse navbar-collapse">    
        <ul class="nav navbar-nav">
        	<!--<li><a href="/">Home</a></li> -->
            <li><a href="#" class="dropdown-toggle" data-toggle="dropdown">Oracle<b class="caret"></b></a>
                <ul class="dropdown-menu depth_0">
                    <li><a href="/SqlMap2Class.aspx">[Oracle] SQL map to class</a></li>
                    <li><a href="/TableMap2Class.aspx">[Oracle] Table map to Class</a></li>
                    <li><a href="/Table2SQL.aspx">[Oracle] Table to SQL</a></li>
                    <li><a href="/OracleTable2ADODotNetIUD.aspx">[Oracle] Classic SQL For ADO.Net</a></li>
                </ul>
            </li>
            <li><a href="#" class="dropdown-toggle" data-toggle="dropdown">SQLServer<b class="caret"></b></a>
                <ul class="dropdown-menu depth_0">
                    <li><a href="/DataTable2Json.aspx">[SQLServer] dataTable to JSON</a></li>
                </ul>
            </li>
            <li><a href="#" class="dropdown-toggle" data-toggle="dropdown">DBase<b class="caret"></b></a>
                <ul class="dropdown-menu depth_0">
                    <!--<li class="divider"></li>-->
                    <li><a href="/DBF2JSON.aspx">[DBF] DBF to JSON</a></li>
                    <li><a href="/DBFViewer.aspx">[DBF] DBF Viewer</a></li>
                </ul>
            </li>
        </ul>
      </div><!-- /.navbar-collapse -->
  	</nav>