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

namespace telco
{
    public partial class contractedit : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string contract_id = "";
        string client_id = "";
        string mode = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["contractid"] != null) contract_id = Server.UrlDecode(Request.QueryString["contractid"]);
                if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
                if (Request.QueryString["mode"] != null) mode = Server.UrlDecode(Request.QueryString["mode"]);
                if (mode == "add")
                {
                    #region Load Page Add
                    LiteralMode.Text = "Add";
                    WebComboPlan.Enabled = true;
                    MySqlOdbc mydb = null;
                    try
                    {
                        string sql = "SELECT client_id, client_name FROM clients";
                        mydb = new MySqlOdbc(constr);
                        WebComboClientID.DataSource = mydb.Reader(sql);
                        WebComboClientID.DataValueField = "client_id";
                        WebComboClientID.DataTextField = "client_name";
                        WebComboClientID.DataBind();
                        if (client_id != "")
                            WebComboClientID.SelectedValue = client_id;
                        string sql2 = "SELECT * FROM types";
                        WebComboType.DataSource = mydb.Reader(sql2);
                        WebComboType.DataValueField = "type_id";
                        WebComboType.DataTextField = "type_name";
                        WebComboType.DataBind();
                        string sql3 = "SELECT * FROM freqs";
                        WebComboFreq.DataSource = mydb.Reader(sql3);
                        WebComboFreq.DataValueField = "freq_id";
                        WebComboFreq.DataTextField = "freq_name";
                        WebComboFreq.DataBind();
                        string sql4 = "SELECT plan_id, plan_name FROM plans";
                        WebComboPlan.DataSource = mydb.Reader(sql4);
                        WebComboPlan.DataValueField = "plan_id";
                        WebComboPlan.DataTextField = "plan_name";
                        WebComboPlan.DataBind();
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
                else if (mode == "edit" && contract_id != "")
                {
                    #region Load Page Edit
                    LiteralMode.Text = "Edit";
                    WebComboPlan.Enabled = false;
                    MySqlOdbc mydb = null;
                    try
                    {
                        string sql = "SELECT * FROM contracts WHERE contract_id=" + contract_id;
                        mydb = new MySqlOdbc(constr);
                        OdbcDataReader dr = mydb.Reader(sql);
                        #region Load WebCombo
                        string sql4 = "SELECT client_id, client_name FROM clients";
                        WebComboClientID.DataSource = mydb.Reader(sql4);
                        WebComboClientID.DataValueField = "client_id";
                        WebComboClientID.DataTextField = "client_name";
                        WebComboClientID.DataBind();
                        if (client_id != "") WebComboClientID.SelectedValue = client_id;
                        string sql2 = "SELECT * FROM types";
                        WebComboType.DataSource = mydb.Reader(sql2);
                        WebComboType.DataValueField = "type_id";
                        WebComboType.DataTextField = "type_name";
                        WebComboType.DataBind();
                        string sql3 = "SELECT * FROM freqs";
                        WebComboFreq.DataSource = mydb.Reader(sql3);
                        WebComboFreq.DataValueField = "freq_id";
                        WebComboFreq.DataTextField = "freq_name";
                        WebComboFreq.DataBind();
                        #endregion
                        if (dr.Read())
                        {
                            LabelID.Text = contract_id;
                            if (dr["contract_client_id"] != DBNull.Value) WebComboClientID.SelectedValue = dr["contract_client_id"].ToString();
                            if (dr["contract_code"] != DBNull.Value) TextBoxCode.Text = dr["contract_code"].ToString();
                            if (dr["contract_name"] != DBNull.Value) TextBoxName.Text = dr["contract_name"].ToString();
                            if (dr["contract_type_id"] != DBNull.Value) WebComboType.SelectedValue = dr["contract_type_id"].ToString();
                            if (dr["contract_freq_id"] != DBNull.Value) WebComboFreq.SelectedValue = dr["contract_freq_id"].ToString();
                            if (dr["contract_number"] != DBNull.Value) WebNumericEditNumber.Text = dr["contract_number"].ToString();
                            if (dr["contract_lc"] != DBNull.Value) WebNumericEditLC.Text = dr["contract_lc"].ToString();
                            if (dr["contract_nc"] != DBNull.Value) WebNumericEditNC.Text = dr["contract_nc"].ToString();
                            if (dr["contract_ic"] != DBNull.Value) WebNumericEditIC.Text = dr["contract_ic"].ToString();
                            if (dr["contract_mc"] != DBNull.Value) WebNumericEditMC.Text = dr["contract_mc"].ToString();
                            if (dr["contract_dc"] != DBNull.Value) WebNumericEditDC.Text = dr["contract_dc"].ToString();
                            if (dr["contract_setup_fee"] != DBNull.Value) WebCurrencyEditSetupFee.Text = dr["contract_setup_fee"].ToString();
                            if (dr["contract_charge"] != DBNull.Value) WebCurrencyEditCharge.Text = dr["contract_charge"].ToString();
                            if (dr["contract_start"] != DBNull.Value) WebDateTimeEditStart.Text = dr["contract_start"].ToString();
                            if (dr["contract_end"] != DBNull.Value) WebDateTimeEditEnd.Text = dr["contract_end"].ToString();
                            if (dr["contract_pending"] == DBNull.Value) CheckBoxPending.Checked = false;
                            else if (dr["contract_pending"].ToString() == "0") CheckBoxPending.Checked = false;
                            else CheckBoxPending.Checked = true;
                            if (dr["contract_locked"] == DBNull.Value) CheckBoxLocked.Checked = false;
                            else if (dr["contract_locked"].ToString() == "0") CheckBoxLocked.Checked = false;
                            else CheckBoxLocked.Checked = true;
                            if (dr["contract_ended"] == DBNull.Value) CheckBoxEnded.Checked = false;
                            else if (dr["contract_ended"].ToString() == "0") CheckBoxEnded.Checked = false;
                            else CheckBoxEnded.Checked = true;
                            if (dr["contract_auto_renew"] == DBNull.Value) CheckBoxAutoRenew.Checked = false;
                            else if (dr["contract_auto_renew"].ToString() == "0") CheckBoxAutoRenew.Checked = false;
                            else CheckBoxAutoRenew.Checked = true;
                            //if (dr["plan_included_data"] != DBNull.Value) DataT.Text = dr["plan_included_data"].ToString();
                            //if (dr["plan_exceed_rate"] != DBNull.Value) RateT.Text = dr["plan_exceed_rate"].ToString();
                            //if (dr["plan_exceed_meter"] != DBNull.Value) RateMeterT.Text = dr["plan_exceed_meter"].ToString();
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
                    #endregion
                }
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            #region Format Variables
            if (Request.QueryString["contractid"] != null) contract_id = Server.UrlDecode(Request.QueryString["contractid"]);
            if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
            if (Request.QueryString["mode"] != null) mode = Server.UrlDecode(Request.QueryString["mode"]);
            if (contract_id == "") contract_id = "null";
            string contract_client_id = "null";
            contract_client_id = WebComboClientID.SelectedValue.ToString();
            string contract_code = TextBoxCode.Text;
            string contract_name = TextBoxName.Text;
            string contract_type_id = "null";
            contract_type_id = WebComboType.SelectedValue.ToString();
            string contract_freq_id = "null";
            contract_freq_id = WebComboFreq.SelectedValue.ToString();
            string contract_number = WebNumericEditNumber.Text.ToString();
            string contract_lc = WebNumericEditLC.Text.ToString();
            string contract_nc = WebNumericEditNC.Text.ToString();
            string contract_ic = WebNumericEditIC.Text.ToString();
            string contract_mc = WebNumericEditMC.Text.ToString();
            string contract_dc = WebNumericEditDC.Text.ToString();
            string contract_setup_fee = "null";
            contract_setup_fee = WebCurrencyEditSetupFee.Text.ToString();
            string contract_charge = "null";
            contract_charge = WebCurrencyEditCharge.Text.ToString();
            DateTime startDate = new DateTime();
            string contract_start = "";
            if (!WebDateTimeEditStart.Text.Equals(""))
            {
                startDate = Convert.ToDateTime(WebDateTimeEditStart.Text);
                contract_start = startDate.Year + "-" + startDate.Month + "-" + startDate.Day;
            }
            DateTime endDate = new DateTime();
            string contract_end = "";
            if (!WebDateTimeEditEnd.Text.Equals(""))
            {
                endDate = Convert.ToDateTime(WebDateTimeEditEnd.Text);
                contract_end = endDate.Year + "-" + endDate.Month + "-" + endDate.Day;
            }
            string contract_pending = "0";
            if (CheckBoxPending.Checked) contract_pending = "1";
            string contract_locked = "0";
            if (CheckBoxLocked.Checked) contract_locked = "1";
            string contract_ended = "0";
            if (CheckBoxEnded.Checked) contract_ended = "1";
            string contract_auto_renew = "0";
            if (CheckBoxAutoRenew.Checked) contract_auto_renew = "1";
            #endregion
            if (mode == "add")
            {
                #region Add
                MySqlOdbc mydb = null;
                try
                {
                    string sql = "INSERT INTO contracts VALUES (null, " + contract_client_id + ", '" + contract_code
                        + "', '" + contract_name + "', '" + contract_type_id + "', '" + contract_freq_id
                        + "', '" + contract_number + "', '" + contract_lc + "', '" + contract_nc
                        + "', '" + contract_ic + "', '" + contract_mc + "', '" + contract_dc + "','" + contract_setup_fee
                        + "', '" + contract_charge + "', '" + contract_start + "', '" + contract_end + "', '"
                        + contract_pending + "', '" + contract_locked + "', '" + contract_ended + "', '"
                        + contract_auto_renew + "')";
                    mydb = new MySqlOdbc(constr);
                    mydb.NonQuery(sql);
                    if (client_id != "")
                    {
                        Response.BufferOutput = true;
                        Response.Redirect("~/contractedit.aspx?contractid=null&clientid=" + Server.UrlEncode(client_id) + "&mode=add", false);
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
                #endregion
                #region Redirect
                if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
                Response.BufferOutput = true;
                Response.Redirect("~/clientcontracts.aspx?clientid=" + Server.UrlEncode(client_id));
                #endregion
            }
            if (mode == "edit")
            {
                #region Edit
                MySqlOdbc mydb = null;
                try
                {
                    string sql = "UPDATE contracts SET contract_client_id=" + client_id + ", contract_code='" + contract_code
                        + "', contract_name='" + contract_name + "', contract_type_id=" + contract_type_id
                        + ", contract_freq_id=" + contract_freq_id + ", contract_number=" + contract_number
                        + ", contract_lc=" + contract_lc + ", contract_nc=" + contract_nc
                        + ", contract_ic=" + contract_ic + ", contract_mc=" + contract_mc
                        + ", contract_dc=" + contract_dc + ", contract_setup_fee=" + contract_setup_fee
                        + ", contract_charge=" + contract_charge + ", contract_start='" + contract_start
                        + "', contract_end='" + contract_end + "', contract_pending=" + contract_pending
                        + ", contract_locked=" + contract_locked + ", contract_ended=" + contract_ended
                        + ", contract_auto_renew=" + contract_auto_renew
                        + " WHERE contract_id=" + contract_id;
                    mydb = new MySqlOdbc(constr);
                    mydb.NonQuery(sql);
                    Response.BufferOutput = true;
                    Response.Redirect("~/clientcontracts.aspx?clientid=" + Server.UrlEncode(client_id));
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
                #region Redirect
                if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
                Response.BufferOutput = true;
                Response.Redirect("~/clientcontracts.aspx?clientid=" + Server.UrlEncode(client_id));
                #endregion
            }
        }



        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
            Response.BufferOutput = true;
            Response.Redirect("~/clientcontracts.aspx?clientid=" + Server.UrlEncode(client_id));
        }

        protected void CustomValidatorClientID_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboClientID.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorType_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboType.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorFreq_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboFreq.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void WebComboPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlOdbc mydb = null;
            try
            {
                string plan_id = "null";
                if (WebComboPlan.SelectedIndex > -1) plan_id = WebComboPlan.SelectedValue;
                string sql = "SELECT * FROM plans WHERE plan_id=" + plan_id;
                mydb = new MySqlOdbc(constr);
                OdbcDataReader dr = mydb.Reader(sql);
                if (dr.Read())
                {
                    TextBoxCode.Text = dr["plan_code"].ToString();
                    TextBoxName.Text = dr["plan_name"].ToString();
                    WebComboType.SelectedValue = dr["plan_type_id"].ToString();
                    WebComboFreq.SelectedValue = dr["plan_freq_id"].ToString();
                    WebNumericEditNumber.Text = dr["plan_number"].ToString();
                    WebNumericEditLC.Text = dr["plan_lc"].ToString();
                    WebNumericEditNC.Text = dr["plan_nc"].ToString();
                    WebNumericEditIC.Text = dr["plan_ic"].ToString();
                    WebNumericEditMC.Text = dr["plan_mc"].ToString();
                    WebNumericEditDC.Text = dr["plan_dc"].ToString();
                    WebCurrencyEditSetupFee.Text = dr["plan_setup_fee"].ToString();
                    WebCurrencyEditCharge.Text = dr["plan_charge"].ToString();
                    WebDateTimeEditStart.Text = null;
                    WebDateTimeEditEnd.Text = null;
                    CheckBoxLocked.Checked = false;
                    CheckBoxEnded.Checked = false;
                    CheckBoxAutoRenew.Checked = false;
                    DataT.Text = dr["plan_included_data"].ToString();
                    RateT.Text = dr["plan_exceed_rate"].ToString();
                    RateMeterT.Text = dr["plan_exceed_meter"].ToString();
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
}