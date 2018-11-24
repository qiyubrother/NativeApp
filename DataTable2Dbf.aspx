<%@ Page Language="C#" %>

<%@ Register Src="~/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>


<!DOCTYPE html>

<script runat="server">
    string title = "Covnert DataTable to DBF file";
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
<pre class="C#">
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFConvert
{
    public class DbfHelper
    {
        public static void DataTable2Dbf(string path, string dbfFileName, DataTable dt)
        {
            var list = new List&lt;string&gt;();

            var dosFileName = dbfFileName.Substring(0, 8);
            if (File.Exists(Path.Combine(path, dosFileName + ".dbf")))
            {
                File.Delete(Path.Combine(path, dosFileName + ".dbf"));
            }

            var createSql = "create table " + dbfFileName + " (";

            foreach (DataColumn dc in dt.Columns)
            {
                string fieldName = dc.ColumnName;

                string type = dc.DataType.ToString();

                switch (type)
                {
                    case "System.String":
                        type = "varchar(100)";
                        break;

                    case "System.Boolean":
                        type = "varchar(10)";
                        break;

                    case "System.Int32":
                        type = "int";
                        break;

                    case "System.Double":
                        type = "Double";
                        break;

                    case "System.DateTime":
                        type = "TimeStamp";
                        break;
                }

                createSql = createSql + "[" + fieldName + "]" + " " + type + ",";

                list.Add(fieldName);
            }

            createSql = createSql.Substring(0, createSql.Length - 1) + ")";

            using (var con = new OleDbConnection(GetConnection(path)))
            {
                con.Open();
                var cmd = new OleDbCommand
                {
                    Connection = con,
                    CommandText = createSql
                };
                cmd.ExecuteNonQuery();

                foreach (DataRow row in dt.Rows)
                {
                    var insertSql = "insert into " + dbfFileName + " values(";

                    for (int i = 0; i < list.Count; i++)
                    {
                        insertSql = insertSql + "'" + ReplaceEscape(row[list[i].ToString()].ToString()) + "',";
                    }

                    insertSql = insertSql.Substring(0, insertSql.Length - 1) + ")";

                    cmd.CommandText = insertSql;

                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        private static string GetConnection(string path)
        {
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=dBASE IV;";
        }

        private static string ReplaceEscape(string str)
        {
            str = str.Replace("'", "''");
            return str;
        }
    }
}
</pre>
<h3>
    Sample code:
</h3>
<pre>
    var dt = new DataTable("dt");
    dt.Columns.Add("Id", typeof(int));
    dt.Columns.Add("Name", typeof(string));
    dt.Rows.Add(1, "Liuzh");
    dt.Rows.Add(10, "Zhaoss");
    dt.AcceptChanges();

    DbfHelper.DataTable2Dbf(@"C:\","LIUZH.DBF", dt);
</pre>
</body>
</html>
