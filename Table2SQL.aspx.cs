using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;

public partial class _Default : System.Web.UI.Page
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
                var primaryKeys = primaryKey.Value.ToUpper().Split(new []{",", " "}, StringSplitOptions.RemoveEmptyEntries);
                var ada = new OracleDataAdapter(string.Format("select column_id,  column_name,  data_type, data_length, nullable from user_tab_cols where Table_Name='{0}'", tblName), conn);
                ada.Fill(dtCols);

                var colBuilder = new StringBuilder();
                var valBuilder = new StringBuilder();
                int paramIndex = 0;
                foreach (DataRow col in dtCols.Rows)
                {
                    colBuilder.Append(string.Format("{0},", col["column_name"]));
                    switch (col["data_type"].ToString())
                    {
                        case "NUMBER":
                            {
                                valBuilder.Append("{" + string.Format("{0}", paramIndex) + "},");
                            }
                            break;
                        case "VARCHAR2":
                            {
                                valBuilder.Append("{" + string.Format("{0}", paramIndex) + "},");
                            }
                            break;
                        case "DATE":
                            {
                                valBuilder.Append("{" + string.Format("{0}", paramIndex) + "},");
                            }
                            break;
                        default:
                            throw new Exception("Unknow data type");
                    }
                    paramIndex++;
                }
                if (colBuilder.Length > 0) colBuilder.Length--;
                if (valBuilder.Length > 0) valBuilder.Length--;
                var insertCommandString = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tblName, colBuilder, valBuilder);
                var rstBuilder = new StringBuilder();
                rstBuilder.Append("foreach(DataRow dr in dt.Rows)\r\n");
                rstBuilder.Append("{\r\n");

                rstBuilder.Append("\tif (dr.RowState == DataRowState.Added)\r\n");
                rstBuilder.Append("\t{\r\n");
                rstBuilder.Append("\t\tsqlCommandList.Add(string.Format(\"" + insertCommandString + "\",\r\n");
                foreach (DataRow col in dtCols.Rows)
                {
                    switch (col["data_type"].ToString())
                    {
                        case "NUMBER":
                            {
                                rstBuilder.Append(string.Format("\t\t\tdr[\"{0}\"] == DBNull.Value ? \"null\" : string.Format(\"{1}\", dr[\"{0}\"]),\r\n", col["column_name"], "{0}"));
                            }
                            break;
                        case "VARCHAR2":
                            {
                                rstBuilder.Append(string.Format("\t\t\tstring.Format(\"{0}\", dr[\"{1}\"]),\r\n", "'{0}'", col["column_name"]));
                            }
                            break;
                        case "DATE":
                            {
                                rstBuilder.Append(string.Format("\t\t\tdr[\"{0}\"] == DBNull.Value ? \"null\" : string.Format(\"TO_DATE({1}, {2})\", Convert.ToDateTime(dr[\"{0}\"]).ToString(OracleHelper.LocalDateFormat), OracleHelper.LocalDateFormat),\r\n", col["column_name"], "'{0}'", "'{1}'"));
                            }
                            break;
                        default:
                            throw new Exception("Unknow data type");
                    }
                }
                rstBuilder.Length -= 3;
                rstBuilder.Append("\r\n\t\t\t));\r\n");
                rstBuilder.Append("\t}\r\n");

                rstBuilder.Append("\telse if (dr.RowState == DataRowState.Modified)\r\n");
                rstBuilder.Append("\t{\r\n");


                #region Update
                var setBuilder = new StringBuilder();
                var updateWhereBuilder = new StringBuilder();
                var delWhereBuilder = new StringBuilder();
                paramIndex = 0;
                foreach (DataRow col in dtCols.Rows)
                {
                    setBuilder.Append(string.Format("{0}={1},", col["column_name"], "{" + string.Format("{0}", paramIndex) + "}"));
                    paramIndex++;
                }
                setBuilder.Length--;

                foreach (var item in primaryKeys)
                {
                    updateWhereBuilder.Append(string.Format("{0} {1} and ", item, "{" + string.Format("{0}", paramIndex) + "}"));
                    paramIndex++;
                }
                updateWhereBuilder.Length -= 5;
                var updateCommandString = string.Format("UPDATE {0} set {1} WHERE {2}", tblName, setBuilder, updateWhereBuilder);

                rstBuilder.Append(string.Format("\t\tsqlCommandList.Add(string.Format(\"{0}\",\r\n", updateCommandString));
                foreach (DataRow col in dtCols.Rows)
                {
                    switch (col["data_type"].ToString())
                    {
                        case "NUMBER":
                            {
                                rstBuilder.Append(string.Format("\t\t\tdr[\"{0}\"] == DBNull.Value ? \"null\" : string.Format(\"{1}\", dr[\"{0}\"]),\r\n", col["column_name"], "{0}"));
                            }
                            break;
                        case "VARCHAR2":
                            {
                                rstBuilder.Append(string.Format("\t\t\tdr[\"{0}\"] == DBNull.Value ? \"null\" : string.Format(\"{1}\", dr[\"{0}\"]),\r\n", col["column_name"], "'{0}'"));
                            }
                            break;
                        case "DATE":
                            {
                                rstBuilder.Append(string.Format("\t\t\tdr[\"{0}\"] == DBNull.Value ? \"null\" : string.Format(\"TO_DATE({1}, {2})\", Convert.ToDateTime(dr[\"{0}\"]).ToString(OracleHelper.LocalDateFormat), OracleHelper.LocalDateFormat),\r\n", col["column_name"], "'{0}'", "'{1}'"));
                            }
                            break;
                        default:
                            throw new Exception("Unknow data type");
                    }
                }
                // Update where
                foreach (var item in primaryKeys)
                {
                    bool isFind = false;
                    DataRow drPk = null;
                    foreach (DataRow col in dtCols.Rows)
                    {
                        if (col["column_name"].ToString() == item.ToString())
                        {
                            isFind = true;
                            drPk = col;
                            break;
                        }
                    }
                    if (!isFind) throw new Exception("Invalid PK!");

                    switch (drPk["data_type"].ToString())
                    {
                        case "NUMBER":
                            {
                                rstBuilder.Append(string.Format("\t\t\tdr[\"{0}\", DataRowVersion.Original] == DBNull.Value ? \"is null\" : string.Format(\"{1}\", dr[\"{0}\", DataRowVersion.Original]),\r\n", item, "={0}"));
                            }
                            break;
                        case "VARCHAR2":
                            {
                                rstBuilder.Append(string.Format("\t\t\tdr[\"{0}\", DataRowVersion.Original] == DBNull.Value ? \"is null\" : string.Format(\"{1}\", dr[\"{0}\", DataRowVersion.Original]),\r\n", item, "='{0}'"));
                            }
                            break;
                        case "DATE":
                            {
                                rstBuilder.Append(string.Format("\t\t\tdr[\"{0}\", DataRowVersion.Original] == DBNull.Value ? \"is null\" : string.Format(\"=TO_DATE({1}, {2})\", dr[\"{0}\", DataRowVersion.Original], OracleHelper.LocalDateFormat),\r\n", item, "'{0}'", "'{1}'"));
                            }
                            break;
                        default:
                            throw new Exception("Unknow data type");
                    }
                }
                rstBuilder.Length -= 3;
                rstBuilder.Append("\r\n\t\t\t));\r\n");
                rstBuilder.Append("\t}\r\n");
                #endregion
                #region Delete
                rstBuilder.Append("\telse if (dr.RowState == DataRowState.Deleted)\r\n");
                rstBuilder.Append("\t{\r\n");

                paramIndex = 0;
                foreach (var item in primaryKeys)
                {
                    delWhereBuilder.Append(string.Format("{0} {1} and ", item, "{" + string.Format("{0}", paramIndex) + "}"));
                    paramIndex++;
                }
                delWhereBuilder.Length -= 5;
                var deleteCommandString = string.Format("DELETE from {0} where {1}", tblName, delWhereBuilder);
                rstBuilder.Append(string.Format("\t\tsqlCommandList.Add(string.Format(\"{0}\",\r\n", deleteCommandString));
                foreach (var item in primaryKeys)
                {
                    bool isFind = false;
                    DataRow drPk = null;
                    foreach (DataRow col in dtCols.Rows)
                    {
                        if (col["column_name"].ToString() == item.ToString())
                        {
                            isFind = true;
                            drPk = col;
                            break;
                        }
                    }
                    if (!isFind) throw new Exception("Invalid PK!");

                    switch (drPk["data_type"].ToString())
                    {
                        case "NUMBER":
                            {
                                rstBuilder.Append(string.Format("\t\t\tdr[\"{0}\", DataRowVersion.Original] == DBNull.Value ? \"is null\" : string.Format(\"{1}\", dr[\"{0}\", DataRowVersion.Original]),\r\n", item, "={0}"));
                            }
                            break;
                        case "VARCHAR2":
                            {
                                rstBuilder.Append(string.Format("\t\t\tdr[\"{0}\", DataRowVersion.Original] == DBNull.Value ? \"is null\" : string.Format(\"{1}\", dr[\"{0}\", DataRowVersion.Original]),\r\n", item, "='{0}'"));
                            }
                            break;
                        case "DATE":
                            {
                                rstBuilder.Append(string.Format("\t\t\tdr[\"{0}\", DataRowVersion.Original] == DBNull.Value ? \"is null\" : string.Format(\"=TO_DATE({1}, {2})\", Convert.ToDateTime(dr[\"{0}\", DataRowVersion.Original]).ToString(OracleHelper.LocalDateFormat), OracleHelper.LocalDateFormat),\r\n", item, "'{0}'", "'{1}'"));
                            }
                            break;
                        default:
                            throw new Exception("Unknow data type");
                    }
                }

                rstBuilder.Length -= 3;
                rstBuilder.Append("\r\n\t\t\t));\r\n");
                rstBuilder.Append("\t}\r\n");

                rstBuilder.Append("}\r\n");
                #endregion
                sqlResult.InnerHtml = "<code>" + rstBuilder + "</code>";

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
        primaryKey.Value = string.Empty;
        sqlResult.InnerHtml = string.Empty;
    }
}