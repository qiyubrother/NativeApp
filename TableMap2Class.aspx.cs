using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;

public partial class TableMap2Class : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            connectionString.Value =
                "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.33.1.237)(PORT=1521)))(CONNECT_DATA=(SID=imcserver)(SERVER=DEDICATED)));User Id=IMCPLUS;Password=IMCPLUS;";
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
    protected void doWork_OnServerClick(object sender, EventArgs e)
    {
        using (var conn = new OracleConnection(connectionString.Value.ToString()))
        {
            try
            {
                conn.Open();
                var tblName = this.tableName.Value.ToUpper();
                var dtCols = new DataTable();
                var ada = new OracleDataAdapter(string.Format("select column_id,  column_name,  data_type, data_length, nullable from user_tab_cols where Table_Name='{0}'", tblName), conn);
                ada.Fill(dtCols);

                var clsBuilder = new StringBuilder();
                clsBuilder.Append(string.Format("public class C{0}\n", this.tableName.Value.Substring(0, 1).ToUpper() + this.tableName.Value.Substring(1).ToLower())); // 首字母大写
                clsBuilder.Append("{\n");
                foreach (DataRow col in dtCols.Rows)
                {
                    var c = col["column_name"].ToString();
                    var colName = c.Substring(0, 1).ToUpper() + c.Substring(1).ToLower();
                    switch (col["data_type"].ToString())
                    {
                        case "NUMBER":
                            clsBuilder.Append(string.Format("    public decimal {0};\n", colName));
                            break;
                        case "VARCHAR2":
                        case "NVARCHAR2":
                        case "VARCHAR":
                        case "CHAR":
                            clsBuilder.Append(string.Format("    public string {0};\n", colName));
                            break;
                        case "DATE":
                            clsBuilder.Append(string.Format("    public DateTime {0};\n", colName));
                            break;
                        default:
                            throw new Exception("Unknow data type");
                    }
                }
                clsBuilder.Append("}\n");

                sqlResult.InnerHtml = "<code>" + clsBuilder + "</code>";

            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script>alert('{0}');</script>", ex.Message));

            }
        }
    }

    protected void clearAll_OnServerClick(object sender, EventArgs e)
    {
        tableName.Value = string.Empty;
        sqlResult.InnerHtml = string.Empty;
    }
}