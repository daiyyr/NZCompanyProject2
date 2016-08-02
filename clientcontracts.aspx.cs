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
using Sapp.JQuery;
using Sapp.Common;

namespace telco
{
    public partial class clientcontracts : System.Web.UI.Page, IPostBackEventHandler
    {
        string constr = AdFunction.conn;
        string client_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);
            if (!Page.IsPostBack)
            {

                #region Setup WebCombo Filter
                //DropDownListFilter.Items.Add("Active");
                //DropDownListFilter.Items.Add("Ended");
                //DropDownListFilter.Items.Add("All");
                //DropDownListFilter.SelectedIndex = 0;
                #endregion
                if (Request.QueryString["clientid"] != null) Session["clientid"] = Request.QueryString["clientid"];

            }
        }
        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                string client_id = HttpContext.Current.Session["clientid"].ToString();
                string sql = "SELECT contract_id AS ID, contract_code AS Code, contract_name AS Name, IF(contract_ended=0, 'Active', 'Ended') AS Type"
                                           + " FROM contracts WHERE (contract_client_id=" + client_id + ")";
                DataTable dt = new MySqlOdbc(AdFunction.conn).ReturnTable(sql, "temp");
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
        public void RaisePostBackEvent(string eventArgument)
        {

            string[] args = eventArgument.Split('|');
            string contract_id = args[1];
            if (args[0] == "ImageButtonDelete")
            {

            }
            else if (args[0] == "ImageButtonDetails")
            {
                if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
                if (client_id != "")
                {


                    Response.BufferOutput = true;
                    Response.Redirect("~/contractdetails.aspx?contractid=" + Server.UrlEncode(contract_id) + "&clientid="
                        + Server.UrlEncode(client_id));

                }
            }
            else if (args[0] == "ImageButtonEdit")
            {
                if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
                if (client_id != "")
                {


                    Response.BufferOutput = true;
                    Response.Redirect("~/contractedit.aspx?contractid=" + Server.UrlEncode(contract_id) + "&clientid="
                        + Server.UrlEncode(client_id) + "&mode=edit");

                }
            }
            else if (args[0] == "ImageButtonCall")
            {

            }
            else if (args[0] == "ImageButtonCopy")
            {
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



        protected void ImageButtonAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
            if (client_id != "")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/contractedit.aspx?contractid=null&clientid=" + Server.UrlEncode(client_id) + "&mode=add");
            }
        }



        //protected void DropDownListFilter_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
        //    #region Load Page
        //    MySqlOdbc mydb = null;
        //    try
        //    {
        //        string sql = "";
        //        //if (DropDownListFilter.SelectedItem.Text == "Active")
        //        //{
        //        //    sql = "SELECT contract_id AS ID, contract_code AS Code, contract_name AS Name, IF(contract_ended=0, 'Active', 'Ended') AS Type"
        //        //    + " FROM contracts WHERE (contract_client_id=" + client_id + ") AND ((contract_ended=0) OR (contract_ended=null))";
        //        //}
        //        //else if (DropDownListFilter.SelectedItem.Text == "Ended")
        //        //{
        //        //    sql = "SELECT contract_id AS ID, contract_code AS Code, contract_name AS Name, IF(contract_ended=0, 'Active', 'Ended') AS Type"
        //        //    + " FROM contracts WHERE (contract_client_id=" + client_id + ") AND (contract_ended=1)";
        //        //}
        //        //else
        //        {
        //            sql = "SELECT contract_id AS ID, contract_code AS Code, contract_name AS Name, IF(contract_ended=0, 'Active', 'Ended') AS Type"
        //            + " FROM contracts WHERE (contract_client_id=" + client_id + ")";
        //        }

        //        mydb = new MySqlOdbc(constr);
        //        XGridView.BindData(UltraWebGrid1, mydb.ReturnTable(sql, "t1"));
        //    }
        //    catch (Exception ex)
        //    {
        //        Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
        //        LabelAlertBoard.Text = ex.ToString();
        //    }
        //    finally
        //    {
        //        if (mydb != null) mydb.Close();
        //    }
        //    #endregion
        //}
    }
}