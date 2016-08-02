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
    public partial class plans : System.Web.UI.Page, IPostBackEventHandler
    {
        string constr = AdFunction.conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Control[] wc = { jqGridTable };
                JSUtils.RenderJSArrayWithCliendIds(Page, wc);
                #region Javascript Setup
                #endregion
                #region Load Web Page
                MySqlOdbc mydb = null;
                MySqlOdbc mydb2 = null;
                MySqlOdbc mydb3 = null;
                try
                {

                    mydb = new MySqlOdbc(constr);


                    string sql2 = "SELECT * FROM types";
                    mydb2 = new MySqlOdbc(constr);
                    WebComboType.DataSource = mydb2.Reader(sql2);
                    WebComboType.DataValueField = "type_id";
                    WebComboType.DataTextField = "type_name";
                    WebComboType.DataBind();

                    string sql3 = "SELECT * FROM freqs";
                    mydb3 = new MySqlOdbc(constr);
                    WebComboFreq.DataSource = mydb3.Reader(sql3);
                    WebComboFreq.DataValueField = "freq_id";
                    WebComboFreq.DataTextField = "freq_name";
                    WebComboFreq.DataBind();
                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
                finally
                {
                    if (mydb != null) mydb.Close();
                    if (mydb2 != null) mydb2.Close();
                    if (mydb3 != null) mydb3.Close();
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
        public void RaisePostBackEvent(string eventArgument)
        {

            string[] args = eventArgument.Split('|');
            string plan_id = args[1];
            if (args[0] == "ImageButtonDelete")
            {

                MySqlOdbc mydb = null;
                try
                {
                    string sql = "DELETE FROM plans WHERE plan_id=" + plan_id;
                    mydb = new MySqlOdbc(constr);
                    mydb.NonQuery(sql);
                    Response.BufferOutput = true;
                    Response.Redirect("~/plans.aspx");
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

            }
            else if (args[0] == "ImageButtonCall")
            {

            }
            else if (args[0] == "ImageButtonCopy")
            {
                #region Retrieve


                MySqlOdbc mydb = null;
                try
                {
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
                        DataT.Text = dr["plan_included_data"].ToString(); ;
                        RateT.Text = dr["plan_exceed_rate"].ToString(); ;
                        RateMeterT.Text = dr["plan_exceed_meter"].ToString(); ;
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


        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            #region Format Variables
            string plan_code = TextBoxCode.Text;
            string plan_name = TextBoxName.Text;
            string plan_type_id = "null";
            if (WebComboType.SelectedIndex > -1) plan_type_id = WebComboType.SelectedValue.ToString();
            string plan_freq_id = "null";
            if (WebComboFreq.SelectedIndex > -1) plan_freq_id = WebComboFreq.SelectedValue.ToString();
            string plan_number = WebNumericEditNumber.Text.ToString();
            string plan_lc = WebNumericEditLC.Text.ToString();
            string plan_nc = WebNumericEditNC.Text.ToString();
            string plan_ic = WebNumericEditIC.Text.ToString();
            string plan_mc = WebNumericEditMC.Text.ToString();
            string plan_dc = WebNumericEditDC.Text.ToString();
            string plan_setup_fee = "null";
            if (WebCurrencyEditSetupFee.Text != null) plan_setup_fee = WebCurrencyEditSetupFee.Text.ToString();
            string plan_charge = "null";
            if (WebCurrencyEditCharge.Text != null) plan_charge = WebCurrencyEditCharge.Text.ToString();
            #endregion
            #region Add
            MySqlOdbc mydb = null;
            try
            {
                string sql = "INSERT INTO plans VALUES (null, '" + plan_code + "', '" + plan_name
                    + "', " + plan_type_id + ", " + plan_freq_id + ", " + plan_number + ", " + plan_lc
                    + ", " + plan_nc + ", " + plan_ic + ", " + plan_mc + ", " + plan_dc + ", "+DataT.Text+","+RateT.Text+","+RateMeterT.Text+"," + plan_setup_fee
                    + ", " + plan_charge + ")";
                mydb = new MySqlOdbc(constr);
                mydb.NonQuery(sql);
                Response.BufferOutput = true;
                Response.Redirect("~/plans.aspx");
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

        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                string sql = "SELECT plan_id AS ID, plan_code AS Code, plan_name As Name FROM plans";
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
    }
}