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

namespace telco
{
    public partial class contractdetails : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string client_id = "";
        string contract_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
                if (Request.QueryString["contractid"] != null) contract_id = Server.UrlDecode(Request.QueryString["contractid"]);
                if (contract_id != "" && client_id != "")
                {
                    #region Javascript Setup
                    Control[] wc = { ButtonDelete };
                    ButtonDelete.Attributes.Add("onclick", "return confirm_delete();");
                    RenderJSArrayWithCliendIds(wc);
                    #endregion
                    #region Load Page
                    MySqlOdbc mydb = null;
                    try
                    {
                        mydb = new MySqlOdbc(constr);
                        string sql = "SELECT * FROM (((contracts LEFT JOIN clients ON contract_client_id=client_id) "
                            + "LEFT JOIN types ON contract_type_id=type_id) LEFT JOIN freqs ON contract_freq_id=freq_id) "
                            + "WHERE contract_id=" + contract_id;

                        OdbcDataReader dr = mydb.Reader(sql);
                        if (dr.Read())
                        {
                            if (dr["client_name"] != DBNull.Value) LabelClientName.Text = dr["client_name"].ToString();
                            LabelID.Text = contract_id;
                            if (dr["contract_code"] != DBNull.Value) LabelCode.Text = dr["contract_code"].ToString();
                            if (dr["contract_name"] != DBNull.Value) LabelName.Text = dr["contract_name"].ToString();
                            if (dr["type_name"] != DBNull.Value) LabelType.Text = dr["type_name"].ToString();
                            if (dr["freq_name"] != DBNull.Value) LabelFreq.Text = dr["freq_name"].ToString();
                            if (dr["contract_number"] != DBNull.Value) LabelNumber.Text = dr["contract_number"].ToString();
                            if (dr["contract_lc"] != DBNull.Value) LabelLC.Text = dr["contract_lc"].ToString();
                            if (dr["contract_nc"] != DBNull.Value) LabelNC.Text = dr["contract_nc"].ToString();
                            if (dr["contract_ic"] != DBNull.Value) LabelIC.Text = dr["contract_ic"].ToString();
                            if (dr["contract_mc"] != DBNull.Value) LabelMC.Text = dr["contract_mc"].ToString();
                            if (dr["contract_dc"] != DBNull.Value) LabelDC.Text = dr["contract_dc"].ToString();
                            if (dr["contract_setup_fee"] != DBNull.Value) LabelSetupFee.Text = dr["contract_setup_fee"].ToString();
                            if (dr["contract_charge"] != DBNull.Value) LabelCharge.Text = dr["contract_charge"].ToString();
                            if (dr["contract_start"] != DBNull.Value) LabelStart.Text = dr["contract_start"].ToString();
                            if (dr["contract_end"] != DBNull.Value) LabelEnd.Text = dr["contract_end"].ToString();
                            if (dr["contract_pending"] != DBNull.Value)
                            {
                                if (dr["contract_pending"].ToString() == "1") CheckBoxPending.Checked = true;
                                else CheckBoxPending.Checked = false;
                            }
                            else CheckBoxPending.Checked = false;
                            if (dr["contract_locked"] != DBNull.Value)
                            {
                                if (dr["contract_locked"].ToString() == "1") CheckBoxLocked.Checked = true;
                                else CheckBoxLocked.Checked = false;
                            }
                            else CheckBoxLocked.Checked = false;
                            if (dr["contract_ended"] != DBNull.Value)
                            {
                                if (dr["contract_ended"].ToString() == "1") CheckBoxEnded.Checked = true;
                                else CheckBoxEnded.Checked = false;
                            }
                            else CheckBoxEnded.Checked = false;
                            if (dr["contract_auto_renew"] != DBNull.Value)
                            {
                                if (dr["contract_auto_renew"].ToString() == "1") CheckBoxAutoRenew.Checked = true;
                                else CheckBoxAutoRenew.Checked = false;
                            }
                            //if (dr["plan_included_data"] != DBNull.Value) LabelIncludeData.Text = dr["plan_included_data"].ToString();
                            //if (dr["plan_exceed_rate"] != DBNull.Value) LabelExceedRate.Text = dr["plan_exceed_rate"].ToString();
                            //if (dr["plan_exceed_meter"] != DBNull.Value) LabelExceedRateMeter.Text = dr["plan_exceed_meter"].ToString();
                            else CheckBoxAutoRenew.Checked = false;
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

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            MySqlOdbc mydb = null;
            if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
            if (Request.QueryString["contractid"] != null) contract_id = Server.UrlDecode(Request.QueryString["contractid"]);
            try
            {
                string sql = "DELETE FROM contracts WHERE contract_id=" + contract_id;
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
                mydb.Close();
            }
        }

        protected void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
            if (Request.QueryString["contractid"] != null) contract_id = Server.UrlDecode(Request.QueryString["contractid"]);
            Response.BufferOutput = true;
            Response.Redirect("~/contractedit.aspx?contractid=" + Server.UrlEncode(contract_id) + "&clientid="
                + Server.UrlEncode(client_id) + "&mode=edit");
        }

        protected void ImageButtonGoBack_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
            if (Request.QueryString["contractid"] != null) contract_id = Server.UrlDecode(Request.QueryString["contractid"]);
            Response.BufferOutput = true;
            Response.Redirect("~/clientcontracts.aspx?clientid=" + Server.UrlEncode(client_id));
        }
    }
}