using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class DBFViewer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void read_Click(object sender, EventArgs e)
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
            tbl.Caption = file;
            var tblHeaderRow = new TableHeaderRow();
            foreach (DataColumn dc in dt.Columns)
            {
                var tblCell = new TableHeaderCell();
                tblCell.Text = dc.ColumnName;
                tblHeaderRow.Cells.Add(tblCell);
            }
            tbl.Rows.Add(tblHeaderRow);
            foreach (DataRow dr in dt.Rows)
            {
                var tblNewRow = new TableRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    var tblCell = new TableCell();
                    tblCell.Text = dr[dc].ToString();
                    tblNewRow.Cells.Add(tblCell);
                }
                tbl.Rows.Add(tblNewRow);
            }

            File.Delete(fn);
        }
    }
}