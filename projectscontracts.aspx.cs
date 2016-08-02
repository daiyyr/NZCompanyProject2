using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.Odbc;

namespace telco
{
    public partial class projectscontracts : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string project_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            XGridView.AddHead(UltraWebGrid1);
            XGridView.AddHead(UltraWebGrid2);
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (!Page.IsPostBack && project_id != "")
            {
                MySqlOdbc mydb = null;
                string client_id = "";
                try
                {
                    #region Load Web Page
                    mydb = new MySqlOdbc(constr);
                    string sql = "SELECT project_client_id FROM projects WHERE project_id=" + project_id;
                    OdbcDataReader dr = mydb.Reader(sql);
                    if (dr.Read())
                    {
                        if (dr["project_client_id"] != DBNull.Value) client_id = dr["project_client_id"].ToString();
                    }
                    if (client_id != "")
                    {
                        sql = "SELECT contract_id AS ID, contract_code AS Code, contract_name AS Name, contract_setup_fee AS `Setup Fee`, contract_charge AS `Monthly Fee` "
                            + "FROM contracts WHERE contract_client_id=" + client_id + " AND contract_ended=0 AND contract_id NOT IN "
                            + "(SELECT projects_contracts_contract_id FROM projects_contracts WHERE projects_contracts_project_id=" + project_id + ")";
                        XGridView.BindData(UltraWebGrid1, mydb.ReturnTable(sql, "t1"));

                        sql = "SELECT contract_id AS ID, contract_code AS Code, contract_name AS Name, contract_setup_fee AS `Setup Fee`, contract_charge AS `Monthly Fee` "
                            + "FROM contracts WHERE contract_client_id=" + client_id + " AND contract_ended=0 AND contract_id IN "
                            + "(SELECT projects_contracts_contract_id FROM projects_contracts WHERE projects_contracts_project_id=" + project_id + ")";
                        DataTable dt = mydb.ReturnTable(sql, "t1");
                        dt.Columns.Add("TID");
                        if (dt.Rows.Count > 1)
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string contract_id = dt.Rows[i]["ID"].ToString();
                                sql = "SELECT * FROM projects_contracts WHERE projects_contracts_contract_id=" + contract_id + " AND projects_contracts_project_id=" + project_id;
                                OdbcDataReader dr2 = mydb.Reader(sql);
                                if (dr2.Read())
                                {
                                    string billed = dr2["projects_contracts_billed"].ToString();
                                    string invoice_id = dr2["projects_contracts_invoice_id"].ToString();
                                    if (billed == "1")
                                    {
                                        dt.Rows[i]["TID"] = invoice_id;
                                    }

                                }
                            }

                        XGridView.BindData(UltraWebGrid2, dt);
                        foreach (GridViewRow gr in UltraWebGrid2.Rows)
                        {
                            Button ib = (Button)gr.Cells[0].FindControl("ButtonBill");
                            string tid = ib.Attributes["UID"].ToString();
                            if (tid.Equals(""))
                                ib.Visible = false;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
                finally
                {
                    if (mydb != null) mydb.Close();
                }

            }
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            string contract_id = ((Button)sender).Attributes["UID"].ToString();
            MySqlOdbc mydb = null;
            try
            {
                mydb = new MySqlOdbc(constr);
                string sql = "INSERT INTO projects_contracts VALUES(null, " + project_id + ", " + contract_id + ", 0, null)";
                mydb.NonQuery(sql);
                UpdateProjectContractStatus(Convert.ToInt32(project_id));
                Response.BufferOutput = true;
                Response.Redirect("~/projectscontracts.aspx?projectid=" + Server.UrlEncode(project_id), false);
            }
            catch (Exception ex)
            {
                Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                LabelAlertBoard.Text = ex.ToString();
            }
            finally
            {
                if (mydb != null) mydb.Close();
            }
        }


        protected void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            string contract_id = ((Button)sender).Attributes["UID"].ToString();

            MySqlOdbc mydb = null;
            try
            {
                mydb = new MySqlOdbc(constr);
                string sql = "DELETE FROM projects_contracts WHERE projects_contracts_project_id=" + project_id
                    + " AND projects_contracts_contract_id=" + contract_id + " AND projects_contracts_billed=0";
                mydb.NonQuery(sql);
                UpdateProjectContractStatus(Convert.ToInt32(project_id));
                Response.BufferOutput = true;
                Response.Redirect("~/projectscontracts.aspx?projectid=" + Server.UrlEncode(project_id), false);
            }
            catch (Exception ex)
            {
                Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                LabelAlertBoard.Text = ex.ToString();
            }
            finally
            {
                if (mydb != null) mydb.Close();
            }

        }
        protected void ButtonBill_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            string contract_id = ((Button)sender).Attributes["UID"].ToString();

            Response.Redirect("invoicedetails.aspx?invoiceid=" + contract_id, false);

        }

        private void UpdateProjectContractStatus(int project_id)
        {
            MySqlOdbc mydb = null;
            try
            {
                mydb = new MySqlOdbc(constr);
                string sql = "SELECT * FROM projects_contracts WHERE projects_contracts_project_id=" + project_id;
                OdbcDataReader dr = mydb.Reader(sql);
                if (dr.Read())
                {
                    sql = "UPDATE projects SET project_contract_included=1 WHERE project_id=" + project_id;
                    mydb.NonQuery(sql);

                }
                else
                {
                    sql = "UPDATE projects SET project_contract_included=0 WHERE project_id=" + project_id;
                    mydb.NonQuery(sql);
                }
            }
            catch (Exception ex)
            {
                Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                LabelAlertBoard.Text = ex.ToString();
            }
            finally
            {
                if (mydb != null) mydb.Close();
            }
        }

        protected void ImageButtonGoBack_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            Response.BufferOutput = true;
            Response.Redirect("~/projectdetails.aspx?projectid=" + Server.UrlEncode(project_id));
        }
    }
}