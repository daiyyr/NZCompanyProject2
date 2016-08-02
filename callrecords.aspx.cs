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
using System.Text;
using System.Data.Odbc;
using Sapp.JQuery;
using Sapp.Common;

namespace telco
{
    public partial class callrecords : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string project_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);
            if (!Page.IsPostBack)
            {

                if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
                Session["projectid"] = project_id;

                string sql = "SELECT * FROM call_records WHERE call_record_project_id=" + project_id;
                DataTable dt = new MySqlOdbc(AdFunction.conn).ReturnTable(sql, "tt");

                if (dt.Rows.Count > 0)
                {
                    string billed = dt.Rows[0]["call_record_billed"].ToString();
                    if (billed == "1")
                    {
                        ImageButtonBilled.Enabled = true;
                        ImageButtonBilled.ImageUrl = "~/images/copy_large.gif";
                    }
                    else
                    {
                        ImageButtonBilled.Enabled = false;
                        ImageButtonBilled.ImageUrl = "~/images/records_disabled.gif";
                    }
                }

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




        protected void ImageButtonDeleteAll_Click(object sender, ImageClickEventArgs e)
        {


            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);


            string sql = "SELECT * FROM call_records WHERE call_record_project_id=" + Session["projectid"].ToString();
            DataTable dt = new MySqlOdbc(AdFunction.conn).ReturnTable(sql, "tt");

            if (dt.Rows.Count > 0)
            {
                string billed = dt.Rows[0]["call_record_billed"].ToString();
                if (billed == "1")
                {
                    throw new Exception("Billed Call Records Can Not Delete");
                }

            }

            MySqlOdbc mydb = null;
            try
            {
                if (project_id != "")
                {
                    mydb = new MySqlOdbc(constr);
                    sql = "DELETE FROM call_records WHERE (call_record_project_id=" + project_id + ") AND (call_record_billed=0)";
                    mydb.NonQuery(sql);
                    sql = "UPDATE projects SET project_call_records_imported=0 WHERE project_id=" + project_id;
                    mydb.NonQuery(sql);
                    Response.BufferOutput = true;
                    Response.Redirect("~/projectdetails.aspx?projectid=" + Server.UrlEncode(project_id));
                }
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

        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                string project_id = HttpContext.Current.Session["projectid"].ToString();
                if (project_id != "")
                {
                    MySqlOdbc mydb = null;
                    try
                    {
                        #region Load WebGrid
                        string sql = "SELECT COUNT(*) FROM call_records";
                        mydb = new MySqlOdbc(AdFunction.conn);
                        OdbcDataReader dr = mydb.Reader(sql);
                        if (dr.Read())
                        {
                            int count = 0;
                            if (dr[0] != DBNull.Value) count = Convert.ToInt32(dr[0]);
                            if (count != 0)
                            {
                                sql = "SELECT call_record_id AS ID, call_record_from_number AS `FROM`, call_record_to_number AS `TO`, call_record_description AS Description, call_record_status AS Status,"
                                        + "  call_record_date  AS Date, call_record_time AS Time, call_record_length AS Length";


                                //#region Add Column Billed
                                //DataTable dt = mydb.ReturnTable(sql, "t1");
                                //dt.Columns.Add("Billed");
                                //string invoice_id = "";
                                //sql = "SELECT * FROM call_records WHERE call_record_project_id=" + project_id;
                                //OdbcDataReader dr2 = mydb.Reader(sql);
                                //if (dr2.Read())
                                //{
                                //    string billed = dr2["call_record_billed"].ToString();
                                //    if (billed == "1")
                                //    {
                                //        invoice_id = dr2["call_record_batch_id"].ToString();
                                //        if (invoice_id != "")
                                //        {
                                //            for (int i = 0; i < dt.Rows.Count; i++)
                                //            {

                                //                dt.Rows[i]["Billed"] = invoice_id;
                                //            }
                                //        }
                                //    }
                                //}
                                //#endregion
                                string sqlselectstr = "select * from (" + sql;
                                string sqlfromstr = " FROM call_records WHERE call_record_project_id=" + project_id;
                                JQGrid jqgridObj = new JQGrid(postdata, AdFunction.conn, sqlfromstr, sqlselectstr);
                                string jsonStr = jqgridObj.GetJSONStr();
                                return jsonStr;
                            }

                        }
                        return "";

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        mydb.Close();
                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }

        protected void ImageButtonBilled_Click(object sender, ImageClickEventArgs e)
        {
            string sql = "SELECT * FROM call_records WHERE call_record_project_id=" + Session["projectid"].ToString();
            DataTable dt = new MySqlOdbc(AdFunction.conn).ReturnTable(sql, "tt");

            if (dt.Rows.Count > 0)
            {
                string billed = dt.Rows[0]["call_record_billed"].ToString();
                if (billed == "1")
                {
                    string invoice_id = dt.Rows[0]["call_record_batch_id"].ToString();
                    Response.Redirect("invoicedetails.aspx?invoiceid=" + invoice_id, false);
                }
            }

        }
    }
}