using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Text;
using Sapp.Common;
using Sapp.JQuery;
namespace telco
{
    public partial class numbers : System.Web.UI.Page, IPostBackEventHandler
    {
        string constr = AdFunction.conn;
        string account_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);
            if (!Page.IsPostBack)
            {

                if (Request.QueryString["accountid"] != null) account_id = Server.UrlDecode(Request.QueryString["accountid"]);
                Session["accountid"] = account_id;

                #region Load WebCombo
                MySqlOdbc mydb2 = null;
                MySqlOdbc mydb3 = null;
                try
                {
                    string sql2 = "SELECT account_id, account_code FROM accounts";
                    mydb2 = new MySqlOdbc(constr);
                    WebComboAccount.DataSource = mydb2.Reader(sql2);
                    WebComboAccount.DataValueField = "account_id";
                    WebComboAccount.DataTextField = "account_code";
                    WebComboAccount.DataBind();
                    if (account_id != "")
                        WebComboAccount.SelectedValue = account_id;

                    string sql3 = "SELECT client_id, client_name FROM clients";
                    mydb3 = new MySqlOdbc(constr);
                    WebComboClient.DataSource = mydb3.Reader(sql3);
                    WebComboClient.DataValueField = "client_id";
                    WebComboClient.DataTextField = "client_name";
                    WebComboClient.DataBind();
                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
                finally
                {
                    if (mydb2 != null) mydb2.Close();
                    if (mydb3 != null) mydb3.Close();
                }
                #endregion
            }
        }
        public void RaisePostBackEvent(string eventArgument)
        {

            string[] args = eventArgument.Split('|');
            string number_id = args[1];
            if (args[0] == "ImageButtonDelete")
            {
                if (Request.QueryString["accountid"] != null) account_id = Server.UrlDecode(Request.QueryString["accountid"]);

                MySqlOdbc mydb = null;
                try
                {
                    string sql = "DELETE FROM numbers WHERE number_id=" + number_id;
                    mydb = new MySqlOdbc(constr);
                    mydb.NonQuery(sql);
                    Response.BufferOutput = true;
                    if (account_id != "")
                        Response.Redirect("~/numbers.aspx?accountid=" + Server.UrlEncode(account_id));
                    else
                        Response.Redirect("~/numbers.aspx");
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
            else if (args[0] == "ImageButtonDetails")
            {
            }
            else if (args[0] == "ImageButtonEdit")
            {

                ButtonAdd.Visible = false;
                ButtonUpdate.Visible = true;
                LiteralTitle.Text = "Edit ID:";
                LiteralID.Text = number_id;


                MySqlOdbc mydb = null;
                string sql = "SELECT * FROM numbers WHERE number_id=" + number_id;
                mydb = new MySqlOdbc(constr);
                OdbcDataReader dr = mydb.Reader(sql);
                if (dr.Read())
                {
                    TextBoxCode.Text = dr["number_code"].ToString();
                    if (dr["number_account_id"] != DBNull.Value) WebComboAccount.SelectedValue = dr["number_account_id"].ToString();
                    if (dr["number_client_id"] != DBNull.Value) WebComboClient.SelectedValue = dr["number_client_id"].ToString();
                    if (dr["number_pending"] != DBNull.Value)
                        if (dr["number_pending"].ToString() == "1") CheckBoxPending.Checked = true;
                    if (dr["number_free"] != DBNull.Value)
                        if (dr["number_free"].ToString() == "1") CheckBoxFree.Checked = true;
                    TextBoxNote.Text = dr["number_note"].ToString();
                }
            }
            else if (args[0] == "ImageButtonCall")
            {

            }
            else if (args[0] == "ImageButtonCopy")
            {
                #region Format Form
                ButtonAdd.Visible = true;
                ButtonUpdate.Visible = false;
                LiteralTitle.Text = "Add New Number:";
                LiteralID.Text = "";
                #endregion

                MySqlOdbc mydb = null;
                try
                {
                    string sql = "SELECT * FROM numbers WHERE number_id=" + number_id;
                    mydb = new MySqlOdbc(constr);
                    OdbcDataReader dr = mydb.Reader(sql);
                    if (dr.Read())
                    {
                        TextBoxCode.Text = dr["number_code"].ToString();
                        if (dr["number_account_id"] != DBNull.Value) WebComboAccount.SelectedValue = dr["number_account_id"].ToString();
                        if (dr["number_client_id"] != DBNull.Value) WebComboClient.SelectedValue = dr["number_client_id"].ToString();
                        if (dr["number_pending"] != DBNull.Value)
                            if (dr["number_pending"].ToString() == "1") CheckBoxPending.Checked = true;
                        if (dr["number_free"] != DBNull.Value)
                            if (dr["number_free"].ToString() == "1") CheckBoxFree.Checked = true;
                        TextBoxNote.Text = dr["number_note"].ToString();
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


        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            if (Request.QueryString["accountid"] != null) account_id = Server.UrlDecode(Request.QueryString["accountid"]);
            #region Format Variables
            string number_code = TextBoxCode.Text;
            string number_account_id = "null";
            if (WebComboAccount.SelectedIndex > -1) number_account_id = WebComboAccount.SelectedValue.ToString();
            string number_client_id = "null";
            if (WebComboClient.SelectedIndex > -1) number_client_id = WebComboClient.SelectedValue.ToString();
            string number_pending = "0";
            if (CheckBoxPending.Checked) number_pending = "1";
            string number_free = "0";
            if (CheckBoxFree.Checked) number_free = "1";
            string number_note = TextBoxNote.Text;
            #endregion
            #region Add
            MySqlOdbc mydb = null;
            try
            {
                string sql = "INSERT INTO numbers VALUES(null, '" + number_code + "', " + number_account_id
                    + ", " + number_client_id + ", " + number_pending + ", " + number_free + ", '" + number_note + "')";
                mydb = new MySqlOdbc(constr);
                mydb.NonQuery(sql);
                Response.BufferOutput = true;
                if (account_id != "")
                    Response.Redirect("~/numbers.aspx?accountid=" + Server.UrlEncode(account_id));
                else
                    Response.Redirect("~/numbers.aspx");
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



        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["accountid"] != null) account_id = Server.UrlDecode(Request.QueryString["accountid"]);
            #region Format Variables
            string number_id = LiteralID.Text;
            string number_code = TextBoxCode.Text;
            string number_account_id = "null";
            if (WebComboAccount.SelectedIndex > -1) number_account_id = WebComboAccount.SelectedValue.ToString();
            string number_client_id = "null";
            if (WebComboClient.SelectedIndex > -1) number_client_id = WebComboClient.SelectedValue.ToString();
            string number_pending = "0";
            if (CheckBoxPending.Checked) number_pending = "1";
            string number_free = "0";
            if (CheckBoxFree.Checked) number_free = "1";
            string number_note = TextBoxNote.Text;
            #endregion
            #region Update
            MySqlOdbc mydb = null;
            try
            {
                string sql = "UPDATE numbers SET number_code='" + number_code + "', number_account_id=" + number_account_id
                    + ", number_client_id=" + number_client_id + ", number_pending=" + number_pending
                    + ", number_free=" + number_free + ", number_note='" + number_note + "'"
                    + " WHERE number_id=" + number_id;
                mydb = new MySqlOdbc(constr);
                mydb.NonQuery(sql);
                Response.BufferOutput = true;
                if (account_id != "")
                    Response.Redirect("~/numbers.aspx?accountid=" + Server.UrlEncode(account_id));
                else
                    Response.Redirect("~/numbers.aspx");
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


        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                string account_id = "";
                if (HttpContext.Current.Session["account_id"] != null)
                    account_id = HttpContext.Current.Session["account_id"].ToString();
                string sql = "";
                if (account_id != "")
                    sql = "SELECT number_id AS ID, number_code AS Number, account_code AS Account, client_name AS Client FROM"
                        + " numbers JOIN accounts ON numbers.number_account_id = accounts.account_id"
                        + " LEFT JOIN clients ON numbers.number_client_id = clients.client_id"
                        + " WHERE account_id = " + account_id;
                else
                    sql = "SELECT number_id AS ID, number_code AS Number, account_code AS Account, client_name AS Client FROM"
                        + " numbers LEFT JOIN accounts ON numbers.number_account_id = accounts.account_id"
                        + " LEFT JOIN clients ON numbers.number_client_id = clients.client_id";
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



        protected void CustomValidatorAccount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboAccount.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }

    }
}