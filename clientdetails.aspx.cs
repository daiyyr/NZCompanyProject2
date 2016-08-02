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

namespace telco
{

    public partial class clientdetails : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string client_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
                if (client_id != "")
                {
                    #region Javascript Setup
                    Control[] wc = { ButtonDelete };
                    ButtonDelete.Attributes.Add("onclick", "return confirm_delete();");
                    ImageButtonCreateInvoice.Attributes.Add("onclick", "return sm('box', 200, 100);");
                    RenderJSArrayWithCliendIds(wc);
                    #endregion
                    #region Load Page
                    MySqlOdbc mydb = null;
                    try
                    {
                        string sql = "SELECT * FROM clients LEFT JOIN categories ON"
                            + " clients.client_category_id = categories.category_id"
                            + " WHERE client_id=" + client_id;

                        mydb = new MySqlOdbc(constr);
                        OdbcDataReader dr = mydb.Reader(sql);
                        if (dr.Read())
                        {
                            LabelID.Text = dr["client_id"].ToString();
                            TextBoxCPNID.Text = dr["cpn_id"].ToString();
                            TextBoxCode.Text = dr["client_code"].ToString();
                            TextBoxName.Text = dr["client_name"].ToString();
                            TextBoxAddress.Text = dr["client_address"].ToString();
                            TextBoxAddress2.Text = dr["client_address2"].ToString();
                            TextBoxCity.Text = dr["client_city"].ToString();
                            TextBoxCountry.Text = dr["client_country"].ToString();
                            TextBoxCategory.Text = dr["category_name"].ToString();
                            TextBoxTax.Text = dr["client_tax"].ToString();
                            TextBoxURL.Text = dr["client_url"].ToString();
                            TextBoxPhone.Text = dr["client_phone"].ToString();
                            TextBoxFax.Text = dr["client_fax"].ToString();
                            TextBoxEmail.Text = dr["client_email"].ToString();
                            TextBoxContact.Text = dr["client_contact"].ToString();
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
            if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
            if (client_id != "")
            {
                MySqlOdbc mydb = null;
                try
                {
                    string sql = "DELETE FROM clients WHERE client_id=" + client_id;
                    mydb = new MySqlOdbc(constr);
                    mydb.NonQuery(sql);
                    Response.BufferOutput = true;
                    Response.Redirect("~/clientslist.aspx");
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

        protected void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
            Response.BufferOutput = true;
            Response.Redirect("~/clientedit.aspx?clientid=" + Server.UrlEncode(client_id) + "&mode=edit");
        }

        protected void ImageButtonContracts_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
            Response.BufferOutput = true;
            Response.Redirect("~/clientcontracts.aspx?clientid=" + Server.UrlEncode(client_id));
        }

        protected void ButtonCreateInvoice_Click(object sender, EventArgs e)
        {

        }

        private void CreateInvoice(bool billminutes, bool billplan)
        {

        }

        protected void ImageButtonCreateInvoice_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}