using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sapp.JQuery;
using Sapp.Common;
using System.Data;
namespace telco
{
    public partial class invoicelist : System.Web.UI.Page, IPostBackEventHandler
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);
            string search = "";
            string type = "";
            if (Request.QueryString["search"] != null) search = Server.UrlDecode(Request.QueryString["search"]);
            Session["search"] = search;
            if (Request.QueryString["type"] != null) type = Server.UrlDecode(Request.QueryString["type"]);
            Session["type"] = type;

            if (!Page.IsPostBack)
            {


                DropDownListType.Items.Add("Unpaid");
                DropDownListType.Items.Add("Paid");
                DropDownListType.Items.Add("All");
                if (type == "") DropDownListType.SelectedIndex = 2;
                else DropDownListType.SelectedIndex = Convert.ToInt32(type);

                MySqlOdbc mydb = null;

                string sql = "";
                mydb = new MySqlOdbc(AdFunction.conn);

                sql = "SELECT client_id AS ID, client_name AS Name FROM clients";
                WebComboClient.DataSource = mydb.Reader(sql);
                WebComboClient.DataValueField = "ID";
                WebComboClient.DataTextField = "Name";
                WebComboClient.DataBind();



                WebComboClient.Items.Insert(0, new ListItem("All", ""));

                HttpContext.Current.Session["DropDownListType"] = DropDownListType;
                Session["WebNumericEditSearch"] = WebNumericEditSearch;
                Session["WebComboClient"] = WebComboClient;
            }
        }
        public void RaisePostBackEvent(string eventArgument)
        {

            string[] args = eventArgument.Split('|');
            string id = args[1];
            if (args[0] == "ImageButtonDelete")
            {

            }
            else if (args[0] == "ImageButtonDetails")
            {

                Response.BufferOutput = true;
                Response.Redirect("~/invoicedetails.aspx?invoiceid=" + Server.UrlEncode(id));
            }
            else if (args[0] == "ImageButtonEdit")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/invoiceedit.aspx?invoiceid=" + Server.UrlEncode(id));
            }
            else if (args[0] == "ImageButtonCall")
            {

            }
            else if (args[0] == "ImageButtonCopy")
            {
            }
        }
        protected void ImageButtonAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/invoiceedit.aspx?mode=add", false);
        }

        protected void ImageButtonSearch_Click(object sender, ImageClickEventArgs e)
        {

            string search = "";
            if (WebNumericEditSearch.Text != "0")
            {
                search += WebNumericEditSearch.Text + ",";
            }
            else search += ",";

            if (WebComboClient.SelectedIndex > -1)
            {
                search += WebComboClient.SelectedValue;
            }

            if (search != ",")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/invoicelist.aspx?search=" + Server.UrlEncode(search)
                    + "&type=" + Server.UrlEncode(DropDownListType.SelectedIndex.ToString()));
            }
            else
            {
                Response.BufferOutput = true;
                Response.Redirect("~/invoicelist.aspx?type=" + Server.UrlEncode(DropDownListType.SelectedIndex.ToString()));
            }

        }

        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {

            string sql = "";
            string search = "";
            if (HttpContext.Current.Session["search"] != null)
                search = HttpContext.Current.Session["search"].ToString();
            TextBox WebNumericEditSearch = (TextBox)HttpContext.Current.Session["WebNumericEditSearch"];
            DropDownList DropDownListType = (DropDownList)HttpContext.Current.Session["DropDownListType"];
            AjaxControlToolkit.ComboBox WebComboClient = (AjaxControlToolkit.ComboBox)HttpContext.Current.Session["WebComboClient"];
            if (search == "" || search == ",")
            {
                sql = "SELECT invoice_id AS ID, invoice_number AS Number, invoice_date AS Date, client_name AS Recipient,"
                    + " invoice_gsttotal AS Amount, invoice_paydate AS 'PaidDate' , invoice_paidamount AS PaidAmount FROM invoices LEFT JOIN clients"
                    + " ON invoices.invoice_client_id=clients.client_id";
                if (DropDownListType.SelectedIndex > -1)
                {
                    if (DropDownListType.SelectedItem.Text == "Unpaid")

                        sql += " WHERE invoices.invoice_paid=0";

                    else if (DropDownListType.SelectedItem.Text == "Paid")

                        sql += " WHERE invoices.invoice_paid=1";

                }
            }
            else
            {
                string[] fields = search.Split(',');
                if (fields.Count() == 2)
                {


                    if (fields[1] == "")
                    {
                        WebNumericEditSearch.Text = fields[0];
                        sql = "SELECT invoice_id AS ID, invoice_number AS `Number`, invoice_date AS `Date`, client_name AS Recipient,"
                            + " invoice_gsttotal AS Amount, invoice_paydate AS `PaidDate` , invoice_paidamount AS PaidAmount FROM invoices LEFT JOIN clients ON invoices.invoice_client_id=clients.client_id"
                            + " WHERE invoices.invoice_number=" + fields[0];
                    }
                    else
                    {
                        if (fields[0] != "")
                        {
                            WebNumericEditSearch.Text = fields[0];
                            WebComboClient.SelectedValue = fields[1];
                            sql = "SELECT invoice_id AS ID, invoice_number AS `Number`, invoice_date AS `Date`, client_name AS `Recipient`,"
                                + " invoice_gsttotal AS Amount, invoice_paydate AS `PaidDate` ,  invoice_paidamount AS PaidAmount FROM invoices LEFT JOIN clients ON invoices.invoice_client_id=clients.client_id"
                                + " WHERE invoices.invoice_number=" + fields[0] + " AND clients.client_id='" + fields[1]+"'";
                        }
                        else
                        {
                            WebNumericEditSearch.Text = null;
                            WebComboClient.SelectedValue = fields[1];
                            sql = "SELECT invoice_id AS ID, invoice_number AS Number, invoice_date AS `Date`, client_name AS Recipient,"
                                + " invoice_gsttotal AS Amount, invoice_paydate AS `PaidDate` , invoice_paidamount AS PaidAmount FROM invoices LEFT JOIN clients ON invoices.invoice_client_id=clients.client_id"
                                + " WHERE clients.client_id='" + fields[1]+"'";
                        }
                    }



                    if (DropDownListType.SelectedIndex > -1)
                    {
                        if (DropDownListType.SelectedItem.Text == "Unpaid")

                            sql += " AND invoices.invoice_paid=0";

                        else if (DropDownListType.SelectedItem.Text == "Paid")

                            sql += " AND invoices.invoice_paid=1";
                    }
                }
            }
            DataTable dt = new MySqlOdbc(AdFunction.conn).ReturnTable(sql, "temp");
            string sqlselectstr = AdFunction.GetSelector(dt);
            string sqlfromstr = "FROM `" + dt.TableName + "`";
            JQGrid jqgridObj = new JQGrid(postdata, AdFunction.conn, sqlfromstr, sqlselectstr, dt);
            string jsonStr = jqgridObj.GetJSONStr();
            return jsonStr;


        }

    }


}