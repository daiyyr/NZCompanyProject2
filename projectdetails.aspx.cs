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
using System.Text;
using Sapp.Common;
using Sapp.JQuery;
namespace telco
{
    public partial class projectdetails : System.Web.UI.Page, IPostBackEventHandler
    {
        string constr = AdFunction.conn;
        string project_id = "";


        public void RaisePostBackEvent(string eventArgument)
        {

            string[] args = eventArgument.Split('|');
            string id = args[1];
            if (args[0] == "ImageButtonDelete1")
            {
                string material_id = id;
                if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
                MySqlOdbc mydb = null;
                try
                {



                    mydb = new MySqlOdbc(constr);


                    string ssql = "SELECT * FROM materials WHERE material_id=" + material_id;
                    OdbcDataReader dr2 = mydb.Reader(ssql);
                    if (dr2.Read())
                    {
                        string billed = dr2["material_billed"].ToString();
                        string invoice_id = dr2["material_invoice_id"].ToString();
                        if (billed == "1")
                        {
                            throw new Exception("Billed Material Can Not Delete");
                        }
                    }


                    string sql = "DELETE FROM materials WHERE material_id=" + material_id;



                    mydb.NonQuery(sql);
                    #region Redirect
                    Response.BufferOutput = true;
                    Response.Redirect("~/projectdetails.aspx?projectid=" + Server.UrlEncode(project_id));
                    #endregion

                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
                finally
                {
                    mydb.Close();
                }
            }
            else if (args[0] == "ImageButtonDelete2")
            {
                string task_id = id;
                if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
                MySqlOdbc mydb = mydb = new MySqlOdbc(constr);
                try
                {
                    string ssql = "SELECT * FROM tasks WHERE task_id=" + task_id;
                    OdbcDataReader dr2 = mydb.Reader(ssql);
                    if (dr2.Read())
                    {
                        string billed = dr2["task_billed"].ToString();
                        string invoice_id = dr2["task_invoice_id"].ToString();
                        if (billed == "1")
                        {
                            throw new Exception("Billed Tasks Can Not Delete");
                        }
                    }

                    string sql = "DELETE FROM tasks WHERE task_id=" + task_id;
                    mydb.NonQuery(sql);
                    #region Redirect
                    Response.BufferOutput = true;
                    Response.Redirect("~/projectdetails.aspx?projectid=" + Server.UrlEncode(project_id));
                    #endregion

                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
                finally
                {
                    mydb.Close();
                }
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            box.Visible = false;
            Control[] wc = { jqGridTable2, jqGridTable1 };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);

            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            Session["projectid"] = project_id;
            if (!Page.IsPostBack)
            {
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    #region Load Form
                    string sql = "SELECT p1.project_title AS title, c1.client_name AS clientname, "
                        + "pc1.pro_category_name AS categoryname, ps1.pro_status_name AS statusname, "
                        + "p1.project_priority AS priority, c2.client_name AS billclientname, "
                        + "p2.project_title AS parent, p1.project_description AS description, "
                        + "p3.person_name AS personname, p1.project_start AS start, p1.project_est_end AS end, "
                        + "p1.project_deadline AS deadline, p1.project_created AS created, "
                        + "p1.project_contract_included AS contractincluded, "
                        + "p1.project_call_records_imported AS recordsimported "
                        + "FROM ((((((projects AS p1 LEFT JOIN clients AS c1 ON p1.project_client_id=c1.client_id) "
                        + "LEFT JOIN pro_categories AS pc1 ON p1.project_category_id=pc1.pro_category_id) "
                        + "LEFT JOIN pro_statuses AS ps1 ON p1.project_status_id=ps1.pro_status_id) "
                        + "LEFT JOIN clients AS c2 ON p1.project_bill_client_id=c2.client_id) "
                        + "LEFT JOIN projects AS p2 ON p1.project_parent_id=p2.project_id) "
                        + "LEFT JOIN persons AS p3 ON p1.project_manager_id=p3.person_id) WHERE p1.project_id=" + project_id;
                    OdbcDataReader dr = mydb.Reader(sql);
                    if (dr.Read())
                    {
                        if (dr["title"] != DBNull.Value) LiteralProjectTitle.Text = dr["title"].ToString();
                        if (dr["clientname"] != DBNull.Value) LabelClientName.Text = dr["clientname"].ToString();
                        if (dr["categoryname"] != DBNull.Value) LabelCategory.Text = dr["categoryname"].ToString();
                        if (dr["statusname"] != DBNull.Value) LabelStatus.Text = dr["statusname"].ToString();
                        if (dr["priority"] != DBNull.Value) LabelPriority.Text = dr["priority"].ToString();
                        if (dr["billclientname"] != DBNull.Value) LabelBillClientName.Text = dr["billclientname"].ToString();
                        if (dr["parent"] != DBNull.Value) LabelParent.Text = dr["parent"].ToString();
                        if (dr["description"] != DBNull.Value) TextBoxDescription.Text = dr["description"].ToString();
                        if (dr["personname"] != DBNull.Value) LabelManager.Text = dr["personname"].ToString();
                        if (dr["start"] != DBNull.Value) LabelStart.Text = MyFuncs.FormatDateTxt(dr["start"]);
                        if (dr["end"] != DBNull.Value) LabelEnd.Text = MyFuncs.FormatDateTxt(dr["end"]);
                        if (dr["deadline"] != DBNull.Value) LabelDeadline.Text = MyFuncs.FormatDateTxt(dr["deadline"]);
                        if (dr["contractincluded"] != DBNull.Value)
                        {
                            int included = Convert.ToInt32(dr["contractincluded"]);
                            if (included == 0)
                            {
                                ImageButtonContracts.ImageUrl = "~/images/contracts_disabled.gif";
                                ImageButtonContracts.Enabled = false;
                                ButtonAddContract.Enabled = true;
                            }
                            else if (included == 1)
                            {
                                ImageButtonContracts.ImageUrl = "~/images/contracts.gif";
                                ImageButtonContracts.Enabled = true;
                                ButtonAddContract.Enabled = true;
                            }
                        }
                        if (dr["recordsimported"] != DBNull.Value)
                        {
                            int included = Convert.ToInt32(dr["recordsimported"]);
                            if (included == 0)
                            {
                                ImageButtonRecords.ImageUrl = "~/images/records_disabled.gif";
                                ImageButtonRecords.Enabled = false;
                                ButtonImportCallRecords.Enabled = true;
                            }
                            else if (included == 1)
                            {
                                ImageButtonRecords.ImageUrl = "~/images/Records.gif";
                                ImageButtonRecords.Enabled = true;
                                ButtonImportCallRecords.Enabled = false;
                            }
                        }
                    }
                    //#endregion
                    //#region Load WebGrid1


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
                    #endregion
            }
        }

        #region Javascript Func
        private void RenderJSArrayWithCliendIds(params Control[] wc)
        {
            if (wc.Length > 0)
            {
                StringBuilder arrClientIDValue = new StringBuilder();
                StringBuilder arrServerIDValue = new StringBuilder();

                // Get a ClientScriptManager reference from the Page class.
                ClientScriptManager cs = Page.ClientScript;

                // Now loop through the controls and build the client and server id's
                for (int i = 0; i < wc.Length; i++)
                {
                    arrClientIDValue.Append("\"" +
                                     wc[i].ClientID + "\",");
                    arrServerIDValue.Append("\"" +
                                     wc[i].ID + "\",");
                }

                // Register the array of client and server id to the client
                cs.RegisterArrayDeclaration("MyClientID",
                   arrClientIDValue.ToString().Remove(arrClientIDValue.ToString().Length - 1, 1));
                cs.RegisterArrayDeclaration("MyServerID",
                   arrServerIDValue.ToString().Remove(arrServerIDValue.ToString().Length - 1, 1));
            }
        }
        #endregion

        protected void ImageButtonEdit_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (project_id != "")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/projectedit.aspx?mode=edit&projectid=" + Server.UrlEncode(project_id));
            }
        }

        protected void ButtonAddContract_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (project_id != "")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/projectscontracts.aspx?projectid=" + Server.UrlEncode(project_id));
            }
        }

        protected void ImageButtonContracts_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (project_id != "")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/projectscontracts.aspx?projectid=" + Server.UrlEncode(project_id));
            }
        }

        protected void ButtonImportCallRecords_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            Response.BufferOutput = true;
            Response.Redirect("~/callbatch.aspx?projectid=" + Server.UrlEncode(project_id));
        }

        protected void ImageButtonCreateInvoice_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ImageButtonDelete_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (project_id != "")
            {
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    string sql = "DELETE FROM projects WHERE project_id=" + project_id;
                    mydb.NonQuery(sql);
                    Response.BufferOutput = true;
                    Response.Redirect("~/projectlist2.aspx");
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

        protected void ImageButtonRecords_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            Response.BufferOutput = true;
            Response.Redirect("~/callrecords.aspx?projectid=" + Server.UrlEncode(project_id));
        }



        protected void ButtonAddMaterial_Click(object sender, EventArgs e)
        {
            #region Redirect
            Response.BufferOutput = true;
            Response.Redirect("~/materialadd.aspx?projectid=" + Server.UrlEncode(project_id));
            #endregion
        }

        protected void ButtonAddTask_Click(object sender, EventArgs e)
        {
            #region Redirect
            Response.BufferOutput = true;
            Response.Redirect("~/taskadd.aspx?projectid=" + Server.UrlEncode(project_id));
            #endregion
        }


        protected void ImageButtonClose_Click(object sender, ImageClickEventArgs e)
        {
            #region Redirect
            Response.BufferOutput = true;
            Response.Redirect("~/projectlist2.aspx");
            #endregion
        }

        protected void ButtonCreateInvoice_Click(object sender, EventArgs e)
        {

            bool arg1 = false;
            bool arg2 = false;
            bool arg3 = false;
            bool arg4 = false;
            bool arg5 = false;
            if (CheckBoxPlans.Checked)
            {
                arg1 = true;
            }
            if (CheckBoxCallRecords.Checked)
            {
                arg2 = true;
            }
            if (CheckBoxMaterials.Checked)
            {
                arg3 = true;
            }
            if (CheckBoxManhours.Checked)
            {
                arg4 = true;
            }
            if (CheckBoxDataUsage.Checked)
            {
                arg5 = true;
            }

            MyBiz.CreateProjectInvoice(project_id, arg1, arg2, arg3, arg4,arg5);
            //{
            //    Response.Write("false");
            //}
            //else
            //    Response.Write("true");


            Response.Redirect("invoiceedit.aspx?mode=edit", false);
            //string url = "ajax/createInvoice.aspx?projectid=" + project_id + "&billplans=" + arg1
            //    + "&billcallrecords=" + arg2 + "&billmaterials=" + arg3 + "&billmanhours=" + arg4;

            //Response.Redirect(url, false);

        }




        protected void ImageButtonCreateInvoice_Click1(object sender, ImageClickEventArgs e)
        {
            Panel1.Visible = false;
            box.Visible = true;
        }

        protected void ButtonCreateInvoice0_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
        }

        [System.Web.Services.WebMethod]
        public static string BindJQGrid1(string postdata)
        {
            try
            {
                string project_id = HttpContext.Current.Session["projectid"].ToString();
                string sql = "SELECT material_id AS ID, material_code AS Code, material_name AS Name, material_qty AS Qty, material_price AS Price "
                   + "FROM materials WHERE material_project_id=" + project_id;
                DataTable dt = new MySqlOdbc(AdFunction.conn).ReturnTable(sql, "temp");
                DataColumn dc = dt.Columns.Add("Billed");
                dc.SetOrdinal(1);
                MySqlOdbc mydb = new MySqlOdbc(AdFunction.conn);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string material_id = dt.Rows[i]["ID"].ToString();
                    if (!material_id.Equals(""))
                    {
                        string ssql = "SELECT * FROM materials WHERE material_id=" + material_id;
                        OdbcDataReader dr2 = mydb.Reader(ssql);
                        if (dr2.Read())
                        {
                            string billed = dr2["material_billed"].ToString();
                            string invoice_id = dr2["material_invoice_id"].ToString();
                            if (billed == "1")
                            {
                                dt.Rows[i]["Billed"] = "<a href='invoicedetails.aspx?invoiceid=" + invoice_id + "' >Yes</a>";
                            }
                        }
                    }
                }

                string sqlselectstr = AdFunction.GetSelector(dt);
                string sqlfromstr = "FROM `" + dt.TableName + "`";
                JQGrid jqgridObj = new JQGrid(postdata, AdFunction.conn, sqlfromstr, sqlselectstr, dt);
                string jsonStr = jqgridObj.GetJSONStr();
                return jsonStr;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Services.WebMethod]
        public static string BindJQGrid2(string postdata)
        {
            try
            {
                string project_id = HttpContext.Current.Session["projectid"].ToString();
                string sql = "SELECT task_id AS ID,task_start_date AS Start,task_end_date AS End, task_duration AS Duration, task_description AS Description"
                    + " FROM tasks WHERE task_project_id=" + project_id;
                DataTable dt = new MySqlOdbc(AdFunction.conn).ReturnTable(sql, "temp");
                DataColumn dc = dt.Columns.Add("Billed");
                dc.SetOrdinal(1);
                MySqlOdbc mydb = new MySqlOdbc(AdFunction.conn);


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string task_id = dt.Rows[i]["ID"].ToString();
                    if (!task_id.Equals(""))
                    {
                        string ssql = "SELECT * FROM tasks WHERE task_id=" + task_id;
                        OdbcDataReader dr2 = mydb.Reader(ssql);
                        if (dr2.Read())
                        {
                            string billed = dr2["task_billed"].ToString();
                            string invoice_id = dr2["task_invoice_id"].ToString();
                            if (billed == "1")
                            {
                                dt.Rows[i]["Billed"] = "<a href='invoicedetails.aspx?invoiceid=" + invoice_id + "' >Yes</a>";
                                //ib.Attributes["UID"] = task_id;
                            }
                        }
                    }
                }

                string sqlselectstr = AdFunction.GetSelector(dt);
                string sqlfromstr = "FROM `" + dt.TableName + "`";
                JQGrid jqgridObj = new JQGrid(postdata, AdFunction.conn, sqlfromstr, sqlselectstr, dt);
                string jsonStr = jqgridObj.GetJSONStr();
                return jsonStr;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}