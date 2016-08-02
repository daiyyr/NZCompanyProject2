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
    public partial class clientedit : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string client_id = "";
        string mode = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["clientid"] != null)
                client_id = Server.UrlDecode(Request.QueryString["clientid"]);
            if (Request.QueryString["mode"] != null)
                mode = Server.UrlDecode(Request.QueryString["mode"]);
            if (!Page.IsPostBack)
            {
                if (mode.Equals("edit") && !client_id.Equals(""))
                {
                    #region Load Page
                    LiteralMode.Text = "Edit";
                    MySqlOdbc mydb = null;
                    try
                    {
                        string sql = "SELECT * FROM clients"
                            + " WHERE client_id=" + client_id;

                        mydb = new MySqlOdbc(constr);
                        OdbcDataReader dr = mydb.Reader(sql);
                        if (dr.Read())
                        {
                            LabelID.Text = dr["client_id"].ToString();
                            WebNumericEditCPNID.Text = dr["cpn_id"].ToString();
                            TextBoxCode.Text = dr["client_code"].ToString();
                            TextBoxName.Text = dr["client_name"].ToString();
                            TextBoxAddress.Text = dr["client_address"].ToString();
                            TextBoxAddress2.Text = dr["client_address2"].ToString();
                            TextBoxCity.Text = dr["client_city"].ToString();
                            TextBoxCountry.Text = dr["client_country"].ToString();
                            #region WebComboCategory
                            MySqlOdbc mydb2 = new MySqlOdbc(constr);
                            string sql2 = "SELECT * FROM categories";
                            WebComboCategory.DataSource = mydb2.Reader(sql2);
                            WebComboCategory.DataValueField = "category_id";
                            WebComboCategory.DataTextField = "category_name";
                            WebComboCategory.DataBind();
                            mydb2.Close();
                            if (dr["client_category_id"] != DBNull.Value) WebComboCategory.SelectedValue =dr["client_category_id"].ToString();
                            #endregion
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
                        mydb.Close();
                    }
                    #endregion
                }
                if (mode == "add")
                {
                    #region Load Page
                    LiteralMode.Text = "Add";
                    try
                    {

                        LabelID.Text = "";
                        WebNumericEditCPNID.Text = null;
                        TextBoxCode.Text = "";
                        TextBoxName.Text = "";
                        TextBoxAddress.Text = "";
                        TextBoxAddress2.Text = "";
                        TextBoxCity.Text = "";
                        TextBoxCountry.Text = "";
                        #region WebComboCategory
                        MySqlOdbc mydb2 = new MySqlOdbc(constr);
                        string sql2 = "SELECT * FROM categories";
                        WebComboCategory.DataSource = mydb2.Reader(sql2);
                        WebComboCategory.DataValueField = "category_id";
                        WebComboCategory.DataTextField = "category_name";
                        WebComboCategory.DataBind();
                        mydb2.Close();
                        WebComboCategory.SelectedValue = null;
                        #endregion
                        TextBoxTax.Text = "";
                        TextBoxURL.Text = "";
                        TextBoxPhone.Text = "";
                        TextBoxFax.Text = "";
                        TextBoxEmail.Text = "";
                        TextBoxContact.Text = "";

                    }
                    catch (Exception ex)
                    {
                        Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                        LabelAlertBoard.Text = ex.ToString();
                    }
                    finally
                    {

                    }
                    #endregion
                }
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            #region Format Variables
            if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);
            if (Request.QueryString["mode"] != null) mode = Server.UrlDecode(Request.QueryString["mode"]);
            if (client_id == "") client_id = "null";
            string cpn_id = "null";
            if (WebNumericEditCPNID.Text != null) cpn_id = WebNumericEditCPNID.Text.ToString();
            string client_code = TextBoxCode.Text.Replace("'", "\''");
            string client_name = TextBoxName.Text.Replace("'", "\''");
            string client_address = TextBoxAddress.Text.Replace("'", "\''");
            string client_address2 = TextBoxAddress2.Text.Replace("'", "\''");
            string client_city = TextBoxCity.Text.Replace("'", "\''");
            string client_country = TextBoxCountry.Text.Replace("'", "\''");
            string client_category_id = "null";
            if (WebComboCategory.SelectedIndex > -1) client_category_id = WebComboCategory.SelectedValue;
            string client_tax = TextBoxTax.Text.Replace("'", "\''");
            string client_url = TextBoxURL.Text.Replace("'", "\''");
            string client_phone = TextBoxPhone.Text.Replace("'", "\''");
            string client_fax = TextBoxFax.Text.Replace("'", "\''");
            string client_email = TextBoxEmail.Text.Replace("'", "\''");
            string client_contact = TextBoxContact.Text.Replace("'", "\''");
            #endregion
            if (mode == "edit")
            {
                #region Update
                MySqlOdbc mydb = null;
                try
                {
                    string sql = "UPDATE clients SET cpn_id=" + cpn_id + ", client_code='" + client_code
                        + "', client_name='" + client_name + "', client_address='" + client_address
                        + "', client_address2='" + client_address2 + "', client_city='" + client_city
                        + "', client_country='" + client_country + "', client_category_id=" + client_category_id
                        + ", client_tax='" + client_tax + "', client_url='" + client_url
                        + "', client_phone='" + client_phone + "', client_fax='" + client_fax
                        + "', client_email='" + client_email + "', client_contact='" + client_contact
                        + "' WHERE client_id=" + client_id;
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
                #endregion
            }
            if (mode == "add")
            {
                #region Insert
                MySqlOdbc mydb = null;
                try
                {
                    string sql = "INSERT INTO clients VALUES (null, '" + cpn_id + "', '" + client_code
                        + "', '" + client_name + "', '" + client_address + "', '" + client_address2
                        + "', '" + client_city + "', '" + client_country + "', " + client_category_id
                        + ", '" + client_tax + "', '" + client_url + "', '" + client_phone + "', '" + client_fax
                        + "', '" + client_email + "', '" + client_contact + "')";
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
                #endregion
            }

        }

        protected void CustomValidatorCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboCategory.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["clientid"] != null) client_id = Server.UrlDecode(Request.QueryString["clientid"]);

            Response.BufferOutput = true;
            if (client_id != "")
                Response.Redirect("~/clientdetails.aspx?clientid=" + Server.UrlEncode(client_id));
            else
                Response.Redirect("~/clientslist.axps");
        }
    }
}