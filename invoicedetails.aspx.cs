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
using Sapp.JQuery;
using Sapp.Common;

namespace telco
{
    public partial class invoicedetails : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string invoice_id = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //XGridView.AddHead(UltraWebGrid1);
            if (!Page.IsPostBack)
            {
                #region Javascript Setup
                Control[] wc = { ImageButtonDelete };
                ImageButtonDelete.Attributes.Add("onclick", "return confirm_delete();");
                RenderJSArrayWithCliendIds(wc);
                #endregion
                #region Load Page
                if (Request.QueryString["invoiceid"] != null) invoice_id = Server.UrlDecode(Request.QueryString["invoiceid"]);
                Session["invoiceid"] = invoice_id;
                if (invoice_id != "")
                {
                    MySqlOdbc mydb = null;
                    try
                    {
                        string sql = "SELECT * FROM invoices LEFT JOIN clients ON invoices.invoice_client_id=clients.client_id WHERE invoice_id=" + invoice_id;
                        mydb = new MySqlOdbc(constr);
                        OdbcDataReader dr = mydb.Reader(sql);
                        if (dr.Read())
                        {
                            if (dr["invoice_number"] != DBNull.Value)
                            {
                                LiteralInvoiceNumber.Text = dr["invoice_number"].ToString();
                                LiteralInvoiceNumber2.Text = dr["invoice_number"].ToString();
                            }
                            if (dr["invoice_date"] != DBNull.Value) LiteralInvoiceDate.Text = MyFuncs.FormatDateTxt(dr["invoice_date"]);
                            if (dr["invoice_due"] != DBNull.Value) LiteralInvoiceDue.Text = MyFuncs.FormatDateTxt(dr["invoice_due"]);
                            if (dr["invoice_term"] != DBNull.Value) LiteralTerm.Text = dr["invoice_term"].ToString();
                            if (dr["client_name"] != DBNull.Value) LiteralCustomerName.Text = dr["client_name"].ToString();
                            if (dr["invoice_total"] != DBNull.Value) LiteralTotal.Text = "$" + dr["invoice_total"].ToString();
                            if (dr["invoice_paidamount"] != DBNull.Value) PaidAmountL.Text = "$" + dr["invoice_paidamount"].ToString();
                            if (dr["invoice_gst"] != DBNull.Value) LiteralGST.Text = "$" + dr["invoice_gst"].ToString();
                            if (dr["invoice_gsttotal"] != DBNull.Value) LiteralGSTTotal.Text = "$" + dr["invoice_gsttotal"].ToString();
                            if (dr["invoice_paydate"] != DBNull.Value) LiteralPaydate.Text = MyFuncs.FormatDateTxt(dr["invoice_paydate"]);

                            //if (dr["invoice_paid"] != DBNull.Value)
                            //{
                            int invoice_paid = Convert.ToInt32(dr["invoice_paid"]);
                            //if (invoice_paid == 0)
                            //{
                            ImageButtonSetPaymentDate.ImageUrl = "~/images/calendar.gif";
                            ImageButtonSetPaymentDate.Enabled = true;
                            //}
                            //else if (invoice_paid == 1)
                            //{
                            //    ImageButtonSetPaymentDate.ImageUrl = "~/images/calendar_disabled.gif";
                            //    ImageButtonSetPaymentDate.Enabled = false;
                            //}
                            //}
                            if (dr["invoice_gsttotal"].ToString().Equals(dr["invoice_paidamount"].ToString()))
                            {
                                ImageButtonSetPaymentDate.ImageUrl = "~/images/calendar_disabled.gif";
                                ImageButtonSetPaymentDate.Enabled = false;
                            }
                        }
                        dr.Close();
                        sql = "SELECT invoice_line_product_code AS Code, invoice_line_description AS Description, invoice_line_qty AS Qty, invoice_line_price AS Price FROM invoice_lines WHERE invoice_line_invoice_id=" + invoice_id;
                        //XGridView.BindData(UltraWebGrid1, mydb.ReturnTable(sql, "t1"));
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


        protected void ImageButtonDelete_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["invoiceid"] != null) invoice_id = Server.UrlDecode(Request.QueryString["invoiceid"]);

            if (invoice_id != "")
            {
                try
                {
                    Invoice Invoice = new Invoice(Convert.ToInt32(invoice_id));
                    Invoice.DeleteInvoice();
                    string updatesql = "update data_usage_detail set InvID= null where InvID=" + invoice_id;
                    MySqlOdbc odbc = new MySqlOdbc(AdFunction.conn);
                    odbc.ExecuteScalar(updatesql);
                    #region Redirect
                    Response.BufferOutput = true;
                    Response.Redirect("~/invoicelist.aspx");
                    #endregion
                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
            }
        }

        protected void ImageButtonExportPDF_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["invoiceid"] != null)
            {
                invoice_id = Server.UrlDecode(Request.QueryString["invoiceid"]);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script language='javascript'>");
                sb.Append("window.open('reportviewer.aspx?report=invoice&invoiceid=" + invoice_id + "', 'ReportViewer',");
                sb.Append("'top=0, left=0, width='+ screen.availwidth +', height='+ screen.availheight +',scrollbars=yes,location=yes, menubar=yes,toolbar=yes,status,resizable=yes,addressbar=yes');");
                sb.Append("</script>");

                Type t = this.GetType();

                if (!ClientScript.IsClientScriptBlockRegistered(t, "PopupScript"))
                    ClientScript.RegisterClientScriptBlock(t, "PopupScript", sb.ToString());
            }
        }

        protected void ImageButtonSetPaymentDate_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["invoiceid"] != null) invoice_id = Server.UrlDecode(Request.QueryString["invoiceid"]);
            if (invoice_id != "")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/invoicepaydate.aspx?invoiceid=" + Server.UrlEncode(invoice_id));
            }
        }

        protected void ImageButtonCopy_Click(object sender, ImageClickEventArgs e)
        {
            #region Copy
            try
            {
                if (Request.QueryString["invoiceid"] != null) invoice_id = Server.UrlDecode(Request.QueryString["invoiceid"]);
                if (invoice_id != "")
                {
                    Invoice invoice = new Invoice();
                    invoice.CopyInvoice(invoice_id);
                    Response.BufferOutput = true;
                    Response.Redirect("invoiceedit.aspx?mode=edit");
                }
            }
            catch (Exception ex)
            {
                Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                LabelAlertBoard.Text = ex.ToString();
            }
            #endregion
        }

        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                string invoiceid = HttpContext.Current.Session["invoiceid"].ToString();
                string sql = sql = "SELECT invoice_line_id AS ID, invoice_line_product_code AS Code, invoice_line_description AS Description, invoice_line_qty AS Qty, invoice_line_price AS Price FROM invoice_lines WHERE invoice_line_invoice_id=" + invoiceid;

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