using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class DBF2JSON : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void getJsonData_Click(object sender, EventArgs e)
    {
        if (FileUpload1.FileName != string.Empty)
        {
            var fn = Server.MapPath("Temp/" + FileUpload1.FileName);
            FileUpload1.SaveAs(fn);
            var fi = new FileInfo(fn);

            var dir = Path.GetDirectoryName(fn);
            var file = "[" + Path.GetFileName(fn) + "]";
            var dt = new DataTable("dt");
            var connStr =
                string.Format("Provider = Microsoft.ACE.OLEDB.12.0 ;Data Source ={0};Extended Properties=dBASE IV;", dir);
            using (var conn = new OleDbConnection(connStr))
            {
                conn.Open();
                var ada = new OleDbDataAdapter(string.Format("select * from {0}", file), conn);
                ada.Fill(dt);
            }
            var s = JsonConvert.SerializeObject(dt, Formatting.Indented);
            jsonDataContainer.Text = s;

            File.Delete(fn);
        }
    }

}