using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;

public partial class OracleTable2ADODotNetIUD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            connectionString.Value = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.33.1.237)(PORT=1521)))(CONNECT_DATA=(SID=imcserver)(SERVER=DEDICATED)));User Id=IMCPLUS;Password=IMCPLUS;";
        }
    }

    protected void testConnection_OnServerClick(object sender, EventArgs e)
    {
        using (var conn = new OracleConnection(connectionString.Value))
        {
            try
            {
                conn.Open();
                Response.Write(string.Format("<script>alert('Connected.')</script>"));
            }
            catch (Exception)
            {
                Response.Write(string.Format("<script>alert('Not connect.');</script>"));
            }
        }
    }

    protected void clearAll_OnServerClick(object sender, EventArgs e)
    {
        tableName.Value = string.Empty;
        sqlResult.InnerHtml = string.Empty;
    }

    protected void doInsert_OnServerClick(object sender, EventArgs e)
    {
        using (var conn = new OracleConnection(connectionString.Value))
        {
            try
            {
                conn.Open();
                var sb = new StringBuilder();
                var ada = new OracleDataAdapter(string.Format("select * from user_tab_columns where table_name = '{0}'", tableName.Value), conn);
                var dtTableDefine = new DataTable();
                var rst = ada.Fill(dtTableDefine);
                if (rst == 0)
                {
                    Response.Write(string.Format("<script>alert('Invalid table name.')</script>"));
                    return;
                }
                var cols = new StringBuilder();
                var vals = new StringBuilder();
                foreach (DataRow dr in dtTableDefine.Rows)
                {
                    var colName = dr["COLUMN_NAME"].ToString();
                    cols.Append(colName).Append(",");
                    vals.Append(":").Append(colName).Append(",");
                }
                if (cols.Length > 1) cols.Length--;
                if (vals.Length > 1) vals.Length--;

                sb.AppendLine(
                    "var connectionString = \"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.33.1.237)(PORT=1521)))(CONNECT_DATA=(SID=imcserver)(SERVER=DEDICATED)));User Id=IMCPLUS;Password=IMCPLUS;\";");
                sb.AppendLine("var conn = new OracleConnection(connectionString);");
                sb.AppendLine("conn.Open();");
                sb.AppendLine(string.Format("var insertCommand = \"INSERT INTO {0}({1}) VALUES({2})\";", tableName.Value, cols, vals));
                sb.AppendLine("var cmd = new OracleCommand(insertCommand, conn);");
                foreach (DataRow dr in dtTableDefine.Rows)
                {
                    var colName = dr["COLUMN_NAME"].ToString();
                    var colDataType = dr["DATA_TYPE"].ToString();

                    var oraType = "OracleDbType.NVarchar2";
                    if (colDataType == "VARCHAR2")
                    {
                        oraType = "OracleDbType.Varchar2";
                    }
                    else if (colDataType == "NUMBER")
                    {
                        oraType = "OracleDbType.Decimal";
                    }
                    else if (colDataType == "DATE")
                    {
                        oraType = "OracleDbType.Date";
                    }
                    else if (colDataType == "DATE")
                    {
                        oraType = "OracleDbType.Date";
                    }
                    sb.AppendLine(string.Format("cmd.Parameters.Add(new OracleParameter(\":{0}\", {1}, ParameterDirection.Input) {2};", colName, oraType, "{Value=null})"));
                }
                sb.AppendLine("var irst = cmd.ExecuteNonQuery();");
                sb.AppendLine("cmd.Dispose();");
                sb.AppendLine("conn.Close();");
                sqlResult.InnerHtml = sb.ToString();

            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('{0}');</script>", ex.Message));
            }
        }
    }

    protected void doSelect_OnServerClick(object sender, EventArgs e)
    {
        using (var conn = new OracleConnection(connectionString.Value))
        {
            try
            {
                conn.Open();
                var sb = new StringBuilder();
                var ada =
                    new OracleDataAdapter(
                        string.Format("select * from user_tab_columns where table_name = '{0}'", tableName.Value), conn);
                var dtTableDefine = new DataTable();
                var rst = ada.Fill(dtTableDefine);
                if (rst == 0)
                {
                    Response.Write(string.Format("<script>alert('Invalid table name.')</script>"));
                    return;
                }
                var cols = new StringBuilder();
                foreach (DataRow dr in dtTableDefine.Rows)
                {
                    var colName = dr["COLUMN_NAME"].ToString();
                    cols.Append(colName).Append(",");
                }
                if (cols.Length > 1) cols.Length--;

                sb.AppendLine(
                    "var connectionString = \"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.33.1.237)(PORT=1521)))(CONNECT_DATA=(SID=imcserver)(SERVER=DEDICATED)));User Id=IMCPLUS;Password=IMCPLUS;\";");
                sb.AppendLine("var dt = new DataTable();");
                sb.AppendLine("using (var conn = new OracleConnection(connectionString)){");
                sb.AppendLine("    conn.Open();");
                sb.AppendLine(string.Format("    var selectCommandText = \"SELECT {0} FROM {1}\";", cols, tableName.Value));
                sb.AppendLine("    var ada = new OracleDataAdapter(selectCommandText, conn);");
                sb.AppendLine("    var srst = ada.Fill(dt);");
                sb.AppendLine("}");
                sqlResult.InnerHtml = sb.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('{0}');</script>", ex.Message));
            }
        }
    }

    private void aaa()
    {
        var connectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.33.1.237)(PORT=1521)))(CONNECT_DATA=(SID=imcserver)(SERVER=DEDICATED)));User Id=IMCPLUS;Password=IMCPLUS;";
        var conn = new OracleConnection(connectionString);
        conn.Open();
        var updateCommand = "UPDATE MMM_BORROW SET BO_NO=:BO_NO,BO_TYPE_ID=:BO_TYPE_ID,COMPANY_ID=:COMPANY_ID,WP_ID=:WP_ID,BO_DATE=:BO_DATE,ORIGINATOR=:ORIGINATOR,DESCRIPTION=:DESCRIPTION,VER=:VER,STATUS=:STATUS,BO_ID=:BO_ID WHERE bo_id=:bo_id";
        var cmd = new OracleCommand(updateCommand, conn);
        cmd.Parameters.Add(new OracleParameter(":BO_NO", OracleDbType.Varchar2, ParameterDirection.Input) { Value = "111" });
        cmd.Parameters.Add(new OracleParameter(":BO_TYPE_ID", OracleDbType.Varchar2, ParameterDirection.Input) { Value = "1" });
        cmd.Parameters.Add(new OracleParameter(":COMPANY_ID", OracleDbType.Varchar2, ParameterDirection.Input) { Value = "4444" });
        cmd.Parameters.Add(new OracleParameter(":WP_ID", OracleDbType.Decimal, ParameterDirection.Input) { Value = 666 });
        cmd.Parameters.Add(new OracleParameter(":BO_DATE", OracleDbType.Date, ParameterDirection.Input) { Value = DateTime.Now });
        cmd.Parameters.Add(new OracleParameter(":ORIGINATOR", OracleDbType.Varchar2, ParameterDirection.Input) { Value = "" });
        cmd.Parameters.Add(new OracleParameter(":DESCRIPTION", OracleDbType.Varchar2, ParameterDirection.Input) { Value = "ggg" });
        cmd.Parameters.Add(new OracleParameter(":VER", OracleDbType.Decimal, ParameterDirection.Input) { Value = 2 });
        cmd.Parameters.Add(new OracleParameter(":STATUS", OracleDbType.Varchar2, ParameterDirection.Input) { Value = "4" });
        cmd.Parameters.Add(new OracleParameter(":BO_ID", OracleDbType.Varchar2, ParameterDirection.Input) { Value = "b250c4c7-0cad-4337-8485-658ea86bb3be" });
        var irst = cmd.ExecuteNonQuery();
        cmd.Dispose();
        conn.Close();

    }
    protected void doUpdate_OnServerClick(object sender, EventArgs e)
    {
        using (var conn = new OracleConnection(connectionString.Value))
        {
            try
            {
                conn.Open();
                var sb = new StringBuilder();
                var ada = new OracleDataAdapter(string.Format("select * from user_tab_columns where table_name = '{0}'", tableName.Value), conn);
                var dtTableDefine = new DataTable();
                var rst = ada.Fill(dtTableDefine);
                if (rst == 0)
                {
                    Response.Write(string.Format("<script>alert('Invalid table name.')</script>"));
                    return;
                }
                var sets = new StringBuilder();
                var conditions = new StringBuilder();
                foreach (DataRow dr in dtTableDefine.Rows)
                {
                    var colName = dr["COLUMN_NAME"].ToString();
                    sets.Append(colName).Append("=:").Append(colName).Append(",");
                }
                foreach (var w in primaryKey.Value.Split(new[] {","}, StringSplitOptions.None))
                {
                    conditions.Append(w).Append("=:").Append(w).Append(","); ;
                }
                if (sets.Length > 1) sets.Length--;
                if (conditions.Length > 1) conditions.Length--;

                sb.AppendLine(
                    "var connectionString = \"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.33.1.237)(PORT=1521)))(CONNECT_DATA=(SID=imcserver)(SERVER=DEDICATED)));User Id=IMCPLUS;Password=IMCPLUS;\";");
                sb.AppendLine("var conn = new OracleConnection(connectionString);");
                sb.AppendLine("conn.Open();");
                sb.AppendLine(string.Format("var updateCommand = \"UPDATE {0} SET {1} WHERE {2}\";", tableName.Value, sets, conditions));
                sb.AppendLine("var cmd = new OracleCommand(updateCommand, conn);");
                foreach (DataRow dr in dtTableDefine.Rows)
                {
                    var colName = dr["COLUMN_NAME"].ToString();
                    var colDataType = dr["DATA_TYPE"].ToString();

                    var oraType = "OracleDbType.NVarchar2";
                    if (colDataType == "VARCHAR2")
                    {
                        oraType = "OracleDbType.Varchar2";
                    }
                    else if (colDataType == "NUMBER")
                    {
                        oraType = "OracleDbType.Decimal";
                    }
                    else if (colDataType == "DATE")
                    {
                        oraType = "OracleDbType.Date";
                    }
                    else if (colDataType == "DATE")
                    {
                        oraType = "OracleDbType.Date";
                    }
                    sb.AppendLine(string.Format("cmd.Parameters.Add(new OracleParameter(\":{0}\", {1}, ParameterDirection.Input) {2};", colName, oraType, "{Value=null})"));
                }
                sb.AppendLine("var irst = cmd.ExecuteNonQuery();");
                sb.AppendLine("cmd.Dispose();");
                sb.AppendLine("conn.Close();");
                sqlResult.InnerHtml = sb.ToString();

            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('{0}');</script>", ex.Message));
            }
        }
    }

    protected void doDelete_OnServerClick(object sender, EventArgs e)
    {
        using (var conn = new OracleConnection(connectionString.Value))
        {
            try
            {
                conn.Open();
                var ada = new OracleDataAdapter(string.Format("select * from user_tab_columns where table_name = '{0}'", tableName.Value), conn);
                var dtTableDefine = new DataTable();
                var rst = ada.Fill(dtTableDefine);
                if (rst == 0)
                {
                    Response.Write(string.Format("<script>alert('Invalid table name.')</script>"));
                    return;
                }
                var sb = new StringBuilder();
                var sets = new StringBuilder();
                var conditions = new StringBuilder();
                foreach (var w in primaryKey.Value.Split(new[] { "," }, StringSplitOptions.None))
                {
                    conditions.Append(w).Append("=:").Append(w).Append(","); ;
                }
                if (sets.Length > 1) sets.Length--;
                if (conditions.Length > 1) conditions.Length--;

                sb.AppendLine(
                    "var connectionString = \"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.33.1.237)(PORT=1521)))(CONNECT_DATA=(SID=imcserver)(SERVER=DEDICATED)));User Id=IMCPLUS;Password=IMCPLUS;\";");
                sb.AppendLine("var conn = new OracleConnection(connectionString);");
                sb.AppendLine("conn.Open();");
                sb.AppendLine(string.Format("var deleteCommand = \"DELETE FROM {0} WHERE {1}\";", tableName.Value, conditions));
                sb.AppendLine("var cmd = new OracleCommand(deleteCommand, conn);");
                foreach (var w in primaryKey.Value.Split(new[] {","}, StringSplitOptions.None))
                {
                    var dr = dtTableDefine.Rows.Cast<DataRow>().FirstOrDefault(r => r["COLUMN_NAME"].ToString() == w);
                    if (dr == null)
                    {
                        Response.Write(string.Format("<script>alert('Invalid primary key.')</script>"));
                        return;
                    }
                    var colName = dr["COLUMN_NAME"].ToString();
                    var colDataType = dr["DATA_TYPE"].ToString();

                    var oraType = "OracleDbType.NVarchar2";
                    if (colDataType == "VARCHAR2")
                    {
                        oraType = "OracleDbType.Varchar2";
                    }
                    else if (colDataType == "NUMBER")
                    {
                        oraType = "OracleDbType.Decimal";
                    }
                    else if (colDataType == "DATE")
                    {
                        oraType = "OracleDbType.Date";
                    }
                    else if (colDataType == "DATE")
                    {
                        oraType = "OracleDbType.Date";
                    }
                    sb.AppendLine(string.Format("cmd.Parameters.Add(new OracleParameter(\":{0}\", {1}, ParameterDirection.Input) {2};", colName, oraType, "{Value=null})"));
                }

                sb.AppendLine("var drst = cmd.ExecuteNonQuery();");
                sb.AppendLine("cmd.Dispose();");
                sb.AppendLine("conn.Close();");
                sqlResult.InnerHtml = sb.ToString();

            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('{0}');</script>", ex.Message));
            }
        }
    }
}