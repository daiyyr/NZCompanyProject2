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
    public partial class suppliers : System.Web.UI.Page, IPostBackEventHandler
    {
        string constr = AdFunction.conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);
            if (!Page.IsPostBack)
            {


                #region Javascript Setup
                #endregion
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
            string supplier_id = args[1];
            if (args[0] == "ImageButtonDelete")
            {
                MySqlOdbc mydb = null;
                try
                {
                    string sql = "DELETE FROM suppliers WHERE supplier_id=" + supplier_id;
                    mydb = new MySqlOdbc(constr);
                    mydb.NonQuery(sql);
                    Response.BufferOutput = true;
                    Response.Redirect("~/suppliers.aspx");
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
                    string sql = "SELECT * FROM suppliers WHERE supplier_id=" + supplier_id;
                    mydb = new MySqlOdbc(constr);
                    OdbcDataReader dr = mydb.Reader(sql);
                    if (dr.Read())
                    {
                        TextBoxName.Text = dr["supplier_name"].ToString();
                        TextBoxAccount.Text = dr["supplier_account"].ToString();
                        TextBoxContact.Text = dr["supplier_contact"].ToString();
                        TextBoxPhone.Text = dr["supplier_phone"].ToString();
                        TextBoxFax.Text = dr["supplier_fax"].ToString();
                        TextBoxEmail.Text = dr["supplier_email"].ToString();
                        TextBoxWeb.Text = dr["supplier_web"].ToString();
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

        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                MySqlOdbc mydb = new MySqlOdbc(AdFunction.conn);
                string sql = "SELECT supplier_id AS ID, supplier_name AS Name FROM suppliers";
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
            #region Format Variables
            string supplier_name = TextBoxName.Text;
            string supplier_account = TextBoxAccount.Text;
            string supplier_contact = TextBoxContact.Text;
            string supplier_phone = TextBoxPhone.Text;
            string supplier_fax = TextBoxFax.Text;
            string supplier_email = TextBoxEmail.Text;
            string supplier_web = TextBoxWeb.Text;
            #endregion
            #region Add
            MySqlOdbc mydb = null;
            try
            {
                string sql = "INSERT INTO suppliers VALUES (null, '" + supplier_name + "', '" + supplier_account
                    + "', '" + supplier_contact + "', '" + supplier_phone + "', '" + supplier_fax
                    + "', '" + supplier_email + "', '" + supplier_web + "')";
                mydb = new MySqlOdbc(constr);
                mydb.NonQuery(sql);
                Response.BufferOutput = true;
                Response.Redirect("~/suppliers.aspx");
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