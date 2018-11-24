using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;

public partial class SqlMap2Class : System.Web.UI.Page
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
                var dtCols = new DataTable();
                var ada = new OracleDataAdapter(sql.Value, conn);
                ada.Fill(dtCols);

                var clsBuilder = new StringBuilder();
                clsBuilder.Append(string.Format("public class {0}\n", className.Value));
                clsBuilder.Append("{\n");
                foreach (DataColumn col in dtCols.Columns)
                {
                    var c = col.ColumnName.Substring(0, 1).ToUpper() + col.ColumnName.Substring(1).ToLower();
                    var typeString = col.DataType.ToString();
                    switch (typeString)
                    {
                        case "System.Decimal":
                            clsBuilder.Append(string.Format("    public decimal? {0};\n", c));
                            break;
                        case "System.String":
                            clsBuilder.Append(string.Format("    public string {0};\n", c));
                            break;
                        case "System.DateTime":
                            clsBuilder.Append(string.Format("    public DateTime? {0};\n", c));
                            break;
                        default:
                            throw new Exception("Unknow data type");
                    }
                }

                // LoadData function
                clsBuilder.Append("\n");
                clsBuilder.Append("    public void LoadData(DataRow dr){\n");
                foreach (DataColumn col in dtCols.Columns)
                {
                    var c = col.ColumnName.Substring(0, 1).ToUpper() + col.ColumnName.Substring(1).ToLower();
                    var typeString = col.DataType.ToString();
                    switch (typeString)
                    {
                        case "System.Decimal":
                            clsBuilder.Append(string.Format("        {0} = dr[\"{1}\"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr[\"{1}\"]);\n", c, col.ColumnName));
                            break;
                        case "System.String":
                            clsBuilder.Append(string.Format("        {0} = dr[\"{1}\"].ToString();\n", c, col.ColumnName));
                            break;
                        case "System.DateTime":
                            clsBuilder.Append(string.Format("        {0} = dr[\"{1}\"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr[\"{1}\"]);\n", c, col.ColumnName));
                            break;
                        default:
                            throw new Exception("Unknow data type");
                    }
                    
                }
                clsBuilder.Append("    }\n");
                // Class End
                clsBuilder.Append("}\n");
                // Class xxxCollection
                clsBuilder.Append("\n");
                clsBuilder.Append(string.Format("public class {0}Collection\n", className.Value));
                clsBuilder.Append("{\n");
                clsBuilder.AppendFormat("    private List < {0} > _lst = new List< {0} >();\n", className.Value);
                clsBuilder.Append("    public void Clear()\n");
                clsBuilder.Append("    {\n");
                clsBuilder.Append("        _lst.Clear();\n");
                clsBuilder.Append("    }\n");
                clsBuilder.Append("    public void LoadData(DataTable dt)\n");
                clsBuilder.Append("    {\n");
                clsBuilder.Append("        foreach (DataRow dr in dt.Rows)\n");
                clsBuilder.Append("        {\n");
                clsBuilder.Append("            var item = new CSpecification();\n");
                clsBuilder.Append("            item.LoadData(dr);\n");
                clsBuilder.Append("            _lst.Add(item);\n");
                clsBuilder.Append("        }\n");
                clsBuilder.Append("    }\n");

                clsBuilder.Append(string.Format("    public IEnumerable< {0} > Items {{ get {{return _lst;}} }}\n", className.Value));
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
        sql.Value = string.Empty;
        className.Value = string.Empty;
        sqlResult.InnerHtml = string.Empty;
    }
}