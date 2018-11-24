using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System.IO;

public partial class _DataTable2Json : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void getJsonData_Click(object sender, EventArgs e)
    {
        var conn = new SqlConnection(connectionString.Text);
        conn.Open();
        var ada = new SqlDataAdapter("SELECT * FROM " + tableName.Text, conn);
        var dt = new DataTable();
        ada.Fill(dt);
        var s = JsonConvert.SerializeObject(dt, Formatting.Indented);
        jsonDataContainer.Text = s;
    }
}