using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sapp.JQuery;
using Sapp.Common;
using System.Data;
namespace telco
{
    public partial class projectlist2 : System.Web.UI.Page, IPostBackEventHandler
    {
        string constr = AdFunction.conn;
        protected void Page_Load(object sender, EventArgs e)
        {


            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);
            string search = "";
            //XGridView.AddHead(UltraWebGrid1);
            if (Request.QueryString["search"] != null) search = Server.UrlDecode(Request.QueryString["search"]);
            if (!Page.IsPostBack)
            {
                //TextBoxSearch.Text = search;
                #region Load Web Page
                MySqlOdbc mydb = null;
                try
                {

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
        public void RaisePostBackEvent(string eventArgument)
        {

            string[] args = eventArgument.Split('|');
            string project_id = args[1];
            if (args[0] == "ImageButtonDelete")
            {

            }
            else if (args[0] == "ImageButtonDetails")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/projectdetails.aspx?projectid=" + project_id);
            }
            else if (args[0] == "ImageButtonEdit")
            {

            }
            else if (args[0] == "ImageButtonCall")
            {

            }
            else if (args[0] == "ImageButtonCopy")
            {
            }
        }

        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                MySqlOdbc mydb = new MySqlOdbc(AdFunction.conn);
                string sql = "SELECT project_id AS ID, project_title AS Title, pro_status_name AS Status, project_start AS Start, project_deadline  AS Deadline, pro_category_name AS Category"
                            + " FROM ((projects LEFT JOIN pro_statuses ON project_status_id=pro_status_id) LEFT JOIN pro_categories ON project_category_id=pro_category_id)";
                DataTable dt = mydb.ReturnTable(sql, "temp");
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

        protected void ImageButtonAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.BufferOutput = true;
            Response.Redirect("~/projectedit.aspx?mode=add");
        }


    }
}