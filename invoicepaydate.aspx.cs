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

namespace telco
{
    public partial class invoicepaydate : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string invoice_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region Load Page
                if (Request.QueryString["invoiceid"] != null) invoice_id = Server.UrlDecode(Request.QueryString["invoiceid"]);

                if (invoice_id != "")
                {
                    MySqlOdbc mydb = null;
                    try
                    {
                        string sql = "SELECT * FROM invoices WHERE invoice_id=" + invoice_id;
                        mydb = new MySqlOdbc(constr);
                        OdbcDataReader dr = mydb.Reader(sql);
                        if (dr.Read())
                        {
                            if (dr["invoice_number"] != DBNull.Value) LiteralNumber.Text = dr["invoice_number"].ToString();
                            if (dr["invoice_date"] != DBNull.Value) LiteralDate.Text = MyFuncs.FormatDateTxt(dr["invoice_date"]);
                            if (dr["invoice_term"] != DBNull.Value) LiteralTerm.Text = dr["invoice_term"].ToString();
                            if (dr["invoice_due"] != DBNull.Value) LiteralDue.Text = MyFuncs.FormatDateTxt(dr["invoice_due"]);

                            if (dr["invoice_gsttotal"] != DBNull.Value) AmountL.Text = dr["invoice_gsttotal"].ToString();
                            if (dr["invoice_paidamount"] != DBNull.Value) PaidAmountT.Text = dr["invoice_paidamount"].ToString();
                            else
                                PaidAmountT.Text = AmountL.Text;

                            WebDateChooserPayDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

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
                #endregion
            }
        }


        protected void ImageButtonGoBack_Click(object sender, ImageClickEventArgs e)
        {
            #region GoBack
            if (Request.QueryString["invoiceid"] != null) invoice_id = Server.UrlDecode(Request.QueryString["invoiceid"]);
            if (invoice_id != "")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/invoicedetails.aspx?invoiceid=" + Server.UrlEncode(invoice_id));
            }
            #endregion
        }

        protected void ImageButtonSubmit_Click(object sender, ImageClickEventArgs e)
        {
            #region Submit

            if (Request.QueryString["invoiceid"] != null) invoice_id = Server.UrlDecode(Request.QueryString["invoiceid"]);
            if (invoice_id != "")
            {
                MySqlOdbc mydb = null;
                try
                {
                    string invoice_paydate = DateTime.Parse(WebDateChooserPayDate.Text).ToString("yyyy-MM-dd");
                    if (invoice_paydate == "null") throw new Exception("Pay Date is invalid!");
                    decimal amount = decimal.Parse(AmountL.Text);
                    decimal paida = decimal.Parse(PaidAmountT.Text);
                    if (paida > amount)
                        throw new Exception("Paid is invalid!");
                    else
                    {
                        string paid = "null";
                        if (!PaidAmountT.Text.Equals(""))
                            paid = PaidAmountT.Text;
                        mydb = new MySqlOdbc(constr);
                        string sql = "UPDATE invoices SET invoice_paydate='" + invoice_paydate + "', invoice_paid=1, invoice_paidamount=" + paid
                            + " WHERE invoice_id=" + invoice_id;
                        mydb.NonQuery(sql);

                        Response.Redirect("~/invoicedetails.aspx?invoiceid=" + Server.UrlEncode(invoice_id), false);
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
            #endregion
        }
    }
}