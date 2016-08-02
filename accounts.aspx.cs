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
using Sapp.JQuery;
using Sapp.Common;
namespace telco
{
    public partial class accounts : System.Web.UI.Page, IPostBackEventHandler
    {
        string constr = AdFunction.conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);
            if (!Page.IsPostBack)
            {

                #region Load WebGrid

                #endregion
                #region Load WebCombo
                MySqlOdbc mydb2 = null;
                MySqlOdbc mydb3 = null;
                try
                {
                    string sql2 = "SELECT supplier_id, supplier_name FROM suppliers";
                    mydb2 = new MySqlOdbc(constr);
                    WebComboSupplier.DataSource = mydb2.Reader(sql2);
                    WebComboSupplier.DataValueField = "supplier_id";
                    WebComboSupplier.DataTextField = "supplier_name";
                    WebComboSupplier.DataBind();

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
            try
            {
                string[] args = eventArgument.Split('|');
                string id = args[1];
                if (args[0] == "ImageButtonDelete")
                {
                    string account_id = id;

                    MySqlOdbc mydb = null;
                    try
                    {
                        string sql = "DELETE FROM accounts WHERE account_id=" + account_id;
                        mydb = new MySqlOdbc(constr);
                        mydb.NonQuery(sql);
                        Response.BufferOutput = true;
                        Response.Redirect("~/accounts.aspx");
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

                    Response.BufferOutput = true;
                    Response.Redirect("~/numbers.aspx?accountid=" + Server.UrlEncode(id), false);
                }
                else if (args[0] == "ImageButtonCopy")
                {
                    string account_id = id;

                    MySqlOdbc mydb = null;
                    try
                    {
                        string sql = "SELECT * FROM accounts WHERE account_id=" + account_id;
                        mydb = new MySqlOdbc(constr);
                        OdbcDataReader dr = mydb.Reader(sql);
                        if (dr.Read())
                        {
                            TextBoxCode.Text = dr["account_code"].ToString();
                            TextBoxNumber.Text = dr["account_number"].ToString();
                            WebComboSupplier.SelectedValue = dr["account_supplier_id"].ToString();
                            if (dr["account_client_id"] != DBNull.Value) WebComboClient.SelectedValue = dr["account_client_id"].ToString();
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
            catch (Exception ex)
            {
                HttpContext.Current.Session["ErrorUrl"] = HttpContext.Current.Request.Url.ToString(); HttpContext.Current.Session["Error"] = ex; HttpContext.Current.Response.Redirect("~/error.aspx", false);
            }
        }

        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                DataTable dt = new MySqlOdbc(AdFunction.conn).ReturnTable("SELECT account_id AS ID, account_code AS Code, account_number AS Number, client_name AS Name FROM accounts LEFT JOIN clients ON accounts.account_client_id = clients.client_id", "temp");
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




        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            #region Format Variables
            string account_code = TextBoxCode.Text;
            string account_number = TextBoxNumber.Text;
            string account_supplier_id = "null";
            if (WebComboSupplier.SelectedIndex > -1) account_supplier_id = WebComboSupplier.SelectedValue;
            string account_client_id = "null";
            if (WebComboClient.SelectedIndex > -1) account_client_id = WebComboClient.SelectedValue;
            #endregion
            #region Add
            MySqlOdbc mydb = null;
            try
            {
                string sql = "INSERT INTO accounts VALUES (null, '" + account_code + "', '" + account_number
                    + "', " + account_supplier_id + ", " + account_client_id + ")";
                mydb = new MySqlOdbc(constr);
                mydb.NonQuery(sql);
                Response.BufferOutput = true;
                Response.Redirect("~/accounts.aspx");
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



        protected void CustomValidatorSupplier_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboSupplier.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }

    }
}