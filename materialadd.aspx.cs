﻿using System;
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
using Sapp.Common;

namespace telco
{
    public partial class materialadd : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string project_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            #region Javascript Setup
            
            ButtonSubmit.Attributes.Add("onclick", "buttonSubmit_Click();");

            
            Control[] wc = { WebComboCode };
            RenderJSArrayWithCliendIds(wc);

            #endregion
            if (!Page.IsPostBack)
            {
                #region Load Session
                Session["MaterialCode"] = "";
                #endregion
                #region Load Form
                TextBoxName.Text = "";
                TextBoxDescription.Text = "";
                WebCurrencyEditPrice.Text = null;
                WebNumericEditQty.Text = null;
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    AjaxControlUtils.SetupComboBox(WebComboCode,
                        "SELECT `product_id` AS `ID`, `product_code` AS `Code` FROM `products`",
                        "ID", "Code", constr, false);
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

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (project_id != "")
            {
                #region Submit
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    #region Format Values
                    string material_code = "";
                    string material_name = "";
                    string material_description = "";
                    string material_price = "null";
                    string material_qty = "null";
                    if (WebComboCode.SelectedIndex > -1)
                        material_code = WebComboCode.SelectedItem.ToString();
                    else
                    {
                        material_code = (string)Session["MaterialCode"];
                    }
                    material_name = MyFuncs.FormatStr(TextBoxName.Text);
                    material_description = MyFuncs.FormatStr(TextBoxDescription.Text);
                    material_price = MyFuncs.FormatDecimal(WebCurrencyEditPrice.Text);
                    material_qty = MyFuncs.FormatDecimal(WebNumericEditQty.Text);
                    #endregion
                    string sql = "INSERT INTO materials VALUES(null, " + project_id + ", '" + material_code
                        + "', '" + material_name + "', '" + material_description + "', " + material_price
                        + ", " + material_qty + ", 0, null)";
                    mydb.NonQuery(sql);
                    #region Redirect
                    Response.BufferOutput = true;
                    Response.Redirect("~/projectdetails.aspx?projectid=" + Server.UrlEncode(project_id));
                    #endregion
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

        protected void WebComboCode_SelectedRowChanged(object sender, EventArgs e)
        {
            #region Load Form
            MySqlOdbc mydb = null;
            try
            {
                mydb = new MySqlOdbc(constr);
                if (WebComboCode.SelectedIndex > -1 && WebComboCode.SelectedValue.Length > 0)
                {
                    string sql = "SELECT * FROM products WHERE product_id=" + WebComboCode.SelectedValue;
                    OdbcDataReader dr = mydb.Reader(sql);
                    if (dr.Read())
                    {
                        TextBoxName.Text = dr["product_name"].ToString(); ;
                        WebCurrencyEditPrice.Text = Convert.ToDecimal(dr["product_price"].ToString()).ToString();
                    }
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

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            #region Redirect
            Response.BufferOutput = true;
            Response.Redirect("~/projectdetails.aspx?projectid=" + Server.UrlEncode(project_id));
            #endregion
        }

        protected void CustomValidatorCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //if (WebComboCode.SelectedIndex > -1) args.IsValid = true;
            //else if ((string)Session["MaterialCode"] != "") args.IsValid = true;
            //else args.IsValid = false;
        }

        protected void CustomValidatorPrice_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebCurrencyEditPrice.Text != null) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorQty_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebNumericEditQty.Text != null) args.IsValid = true;
            else args.IsValid = false;
        }

        [System.Web.Services.WebMethod]
        public static string SetSessionValue(string value)
        {
            try
            {
                HttpContext.Current.Session["MaterialCode"] = value;
                return ("true");
            }
            catch (Exception ex)
            {
                MyLog.InsertLog(ex.ToString());
            }
            return ("false");
        }
    }
}