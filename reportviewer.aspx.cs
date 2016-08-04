using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Reporting.WebForms;
using System.Data.Odbc;
using Sapp.SMS;
using telco.templates;
using System.Globalization;

namespace telco
{
    public partial class reportviewer : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["report"] != null)
                {
                    string report = Server.UrlDecode(Request.QueryString["report"]);
                    string start = Server.UrlDecode(Request.QueryString["start"]);
                    string end = Server.UrlDecode(Request.QueryString["end"]);
                    string client = Server.UrlDecode(Request.QueryString["client"]);
                    if (report == "invoice") ViewInvoice();
                    if (report == "accountreceivable") ViewAccountReceivable(start, end);
                    if (report == "invoiceclient") ViewInvoiceClient(start, end, client);
                }
            }
        }

        private void ViewInvoice()
        {
            try
            {
                if (Request.QueryString["invoiceid"] != null)
                {
                    string invoice_id = Server.UrlDecode(Request.QueryString["invoiceid"]);
                    MySqlOdbc mydb = null;
                    try
                    {
                        mydb = new MySqlOdbc(constr);
                        #region Define Tables
                        DataTable invoiceTable = new DataTable();
                        invoiceTable.Columns.Add("invoice_number", Type.GetType("System.String"));
                        invoiceTable.Columns.Add("invoice_date", Type.GetType("System.String"));
                        invoiceTable.Columns.Add("invoice_due", Type.GetType("System.String"));
                        invoiceTable.Columns.Add("invoice_term", Type.GetType("System.String"));
                        invoiceTable.Columns.Add("invoice_total", Type.GetType("System.String"));
                        invoiceTable.Columns.Add("invoice_gst", Type.GetType("System.String"));
                        invoiceTable.Columns.Add("invoice_gsttotal", Type.GetType("System.String"));
                        DataTable clientTable = new DataTable();
                        clientTable.Columns.Add("client_name", Type.GetType("System.String"));
                        clientTable.Columns.Add("client_address", Type.GetType("System.String"));
                        clientTable.Columns.Add("client_city", Type.GetType("System.String"));
                        clientTable.Columns.Add("client_country", Type.GetType("System.String"));
                        DataTable linesTable = new DataTable();
                        linesTable.Columns.Add("product_code", Type.GetType("System.String"));
                        linesTable.Columns.Add("description", Type.GetType("System.String"));
                        linesTable.Columns.Add("qty", Type.GetType("System.String"));
                        linesTable.Columns.Add("price", Type.GetType("System.String"));
                        #endregion
                        #region Load invoiceTable
                        string sql = "SELECT * FROM invoices WHERE invoice_id=" + invoice_id;
                        OdbcDataReader dr = mydb.Reader(sql);
                        while (dr.Read())
                        {
                            DataRow nr = invoiceTable.NewRow();
                            if (dr["invoice_number"] != DBNull.Value) nr["invoice_number"] = dr["invoice_number"].ToString();
                            if (dr["invoice_date"] != DBNull.Value) nr["invoice_date"] = MyFuncs.FormatDateTxt(dr["invoice_date"]);
                            if (dr["invoice_due"] != DBNull.Value) nr["invoice_due"] = MyFuncs.FormatDateTxt(dr["invoice_due"]);
                            if (dr["invoice_term"] != DBNull.Value) nr["invoice_term"] = dr["invoice_term"].ToString();
                            if (dr["invoice_total"] != DBNull.Value) nr["invoice_total"] = "$" + dr["invoice_total"].ToString();
                            if (dr["invoice_gst"] != DBNull.Value) nr["invoice_gst"] = "$" + dr["invoice_gst"].ToString();
                            if (dr["invoice_gsttotal"] != DBNull.Value) nr["invoice_gsttotal"] = "$" + dr["invoice_gsttotal"].ToString();
                            invoiceTable.Rows.Add(nr);
                        }
                        #endregion
                        #region Load clientTable
                        sql = "SELECT * FROM clients WHERE client_id IN (SELECT invoice_client_id FROM invoices WHERE invoice_id=" + invoice_id + ")";
                        OdbcDataReader dr2 = mydb.Reader(sql);
                        while (dr2.Read())
                        {
                            DataRow nr = clientTable.NewRow();
                            if (dr2["client_name"] != DBNull.Value) nr["client_name"] = dr2["client_name"].ToString();
                            if (dr2["client_address2"] != DBNull.Value)
                            {
                                if (dr2["client_address2"].ToString() != "") nr["client_address"] = dr2["client_address2"].ToString();
                                else if (dr2["client_address"] != DBNull.Value) nr["client_address"] = dr2["client_address"].ToString();
                            }
                            else if (dr2["client_address"] != DBNull.Value) nr["client_address"] = dr2["client_address"].ToString();
                            if (dr2["client_city"] != DBNull.Value) nr["client_city"] = dr2["client_city"].ToString();
                            if (dr2["client_country"] != DBNull.Value) nr["client_country"] = dr2["client_country"].ToString();
                            clientTable.Rows.Add(nr);
                        }
                        #endregion
                        #region Load linesTable
                        sql = "SELECT * FROM invoice_lines WHERE invoice_line_invoice_id=" + invoice_id;
                        OdbcDataReader dr3 = mydb.Reader(sql);
                        while (dr3.Read())
                        {
                            DataRow nr = linesTable.NewRow();
                            if (dr3["invoice_line_product_code"] != DBNull.Value) nr["product_code"] = dr3["invoice_line_product_code"].ToString();
                            if (dr3["invoice_line_description"] != DBNull.Value) nr["description"] = dr3["invoice_line_description"].ToString();
                            if (dr3["invoice_line_qty"] != DBNull.Value) nr["qty"] = dr3["invoice_line_qty"].ToString();
                            if (dr3["invoice_line_price"] != DBNull.Value) nr["price"] = "$" + dr3["invoice_line_price"].ToString();
                            linesTable.Rows.Add(nr);
                        }
                        #endregion
                        #region Set DataSource
                        ReportDataSource rpDataSource = new ReportDataSource("rpInvoiceDataSet_invoiceTable", invoiceTable);
                        ReportDataSource rpDataSource2 = new ReportDataSource("rpInvoiceDataSet_clientTable", clientTable);
                        ReportDataSource rpDataSource3 = new ReportDataSource("rpInvoiceDataSet_linesTable", linesTable);
                        #endregion
                        #region Load Report Template
                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("templates/Invoice.rdlc");
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rpDataSource);
                        ReportViewer1.LocalReport.DataSources.Add(rpDataSource2);
                        ReportViewer1.LocalReport.DataSources.Add(rpDataSource3);
                        ReportViewer1.LocalReport.Refresh();
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (mydb != null) mydb.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LabelAlertBoard.Text = ex.ToString();
            }
        }
        private void ViewAccountReceivable(string start, string end)
        {

            //ReportProprietorAged pa = new ReportProprietorAged();
            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/templates/ProprietorAged.rdlc");
            //pa.SetReportInfo(DateTime.Parse(start), DateTime.Parse(end));
            //pa.Print(ReportViewer1);
            //try
            //{
            //    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            //    string start_date = start;
            //    string end_date = end;
            //    string type = Request.QueryString["type"].ToString();
            //    if (type.Equals("1"))
            //    {
            //        Page.Title = "Proprietor Aged Due Date Base";

            //        ReportProprietorAged Report = new ReportProprietorAged(constr, Server.MapPath("templates/ProprietorAged.rdlc"), ReportViewer1);
            //        Report.SetReportInfo(Convert.ToInt32(bodycorp_id), Convert.ToDateTime(start_date), Convert.ToDateTime(end_date));
            //        Report.Print();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Session["ErrorUrl"] = HttpContext.Current.Request.Url.ToString(); HttpContext.Current.Session["Error"] = ex; HttpContext.Current.Response.Redirect("~/error.aspx", false);
            //}

        }

        private void ViewInvoiceClient(string start, string end, string client)
        {
            try
            {
                MySqlOdbc mydb = null;
                rpInvoiceClient report = new rpInvoiceClient();
                DateTime startDate = DateTime.ParseExact(start, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                DateTime endDate = DateTime.ParseExact(end, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                string clientSql = (client == "ALL" ? "" : " AND c.client_id=" + client + " ");
                try
                {
                    mydb = new MySqlOdbc(constr);

                    string sql =
                        //"SELECT * FROM " +
                        //"(SELECT i.invoice_client_id, c.client_name, c.client_code, i.invoice_number, i.invoice_date, i.invoice_due, i.invoice_term, " +
                        //"ifnull((select sum(ifnull(inv.invoice_gsttotal, 0)) - sum(ifnull(inv.invoice_paidamount, 0)) from invoices inv where inv.invoice_date < i.invoice_date and inv.invoice_client_id = i.invoice_client_id), 0) ob, " +
                        //"ifnull(i.invoice_gsttotal, 0) invoice_gsttotal,ifnull(i.invoice_paidamount, 0) invoice_paidamount " +
                        //"FROM invoices i " +
                        //"inner join clients c on i.invoice_client_id = c.client_id " +
                        //"where i.invoice_date >= '" + startDate.ToString("yyyy-MM-dd") + "' and i.invoice_date < '" + endDate.ToString("yyyy-MM-dd") + "' " +
                        //") T WHERE ob <> 0 or invoice_gsttotal <> 0 or invoice_paidamount <> 0 order by client_name, invoice_date";
                        "SELECT * FROM " +
                        "(SELECT i.invoice_client_id, c.client_name, c.client_code, i.invoice_number, i.invoice_date, i.invoice_due, (select invoice_line_description from invoice_lines il where il.invoice_line_invoice_id = i.invoice_id ORDER BY invoice_line_invoice_id LIMIT 1) description, " +
                        "ifnull(i.invoice_gsttotal, 0) invoice_gsttotal, ifnull(i.invoice_paidamount, 0) invoice_paidamount, ifnull(i.invoice_gsttotal, 0) - ifnull(i.invoice_paidamount, 0) balance " +
                        "FROM invoices i " +
                        "inner join clients c on i.invoice_client_id = c.client_id " +
                        "where i.invoice_date >= '" + startDate.ToString("yyyy-MM-dd") + "' and i.invoice_date < '" + endDate.ToString("yyyy-MM-dd") + "' " +
                        clientSql +
                        "union all " +
                        "select inv.invoice_client_id, c.client_name,c.client_code,'-' invoice_number,'" + startDate.ToString("yyyy-MM-dd") + "' invoice_date,'-' invoice_due, '[Open Balance]' description, " +
                        "sum(ifnull(inv.invoice_gsttotal, 0)) invoice_gsttotal, sum(ifnull(inv.invoice_paidamount, 0)) invoice_paidamount, sum(ifnull(inv.invoice_gsttotal, 0)) - sum(ifnull(inv.invoice_paidamount, 0)) balance " +
                        "from clients c inner " +
                        "join invoices inv on c.client_id = inv.invoice_client_id " +
                        "where inv.invoice_date < '" + startDate.ToString("yyyy-MM-dd") + "' " +

                        //modified by dyyr @2016 08 02
                        clientSql +

                        "group by c.client_id) T " +
                        "where T.invoice_gsttotal <> 0 or T.invoice_paidamount <> 0 or T.balance <> 0 " +
                        "order by T.client_name, T.invoice_date, T.invoice_number";
                    OdbcDataReader dr = mydb.Reader(sql);
                    while (dr.Read())
                    {
                        var invoiceClientRow = report.InvoiceClient.NewInvoiceClientRow();
                        invoiceClientRow.invoice_gsttotal = 0;
                        invoiceClientRow.invoice_paidamount = 0;
                        if (dr["invoice_number"] != DBNull.Value)
                            invoiceClientRow.invoice_number = dr["invoice_number"].ToString();
                        if (dr["invoice_date"] != DBNull.Value)
                            invoiceClientRow.invoice_date = MyFuncs.FormatDateTxt(dr["invoice_date"]);
                        if (dr["invoice_due"] != DBNull.Value)
                            invoiceClientRow.invoice_due = (dr["invoice_due"].ToString() == "-" ? "-" : MyFuncs.FormatDateTxt(dr["invoice_due"]));
                        if (dr["description"] != DBNull.Value)

                            //modified by dyyr @ 2016 08 02
                            //invoiceClientRow.invoice_desc = dr["description"].ToString() == "[Open Balance]" ? "[Open Balance]" : dr["description"].ToString() + "......";
                              invoiceClientRow.invoice_desc = dr["description"].ToString();


                        if (dr["client_name"] != DBNull.Value)
                            invoiceClientRow.client_name = dr["client_name"].ToString();
                        if (dr["client_code"] != DBNull.Value)
                            invoiceClientRow.client_code = dr["client_code"].ToString();
                        if (dr["invoice_gsttotal"] != DBNull.Value)
                            invoiceClientRow.invoice_gsttotal = Convert.ToDecimal(dr["invoice_gsttotal"]);
                        if (dr["invoice_paidamount"] != DBNull.Value)
                            invoiceClientRow.invoice_paidamount = Convert.ToDecimal(dr["invoice_paidamount"]);
                        if (dr["balance"] != DBNull.Value)
                            invoiceClientRow.invoice_balance = Convert.ToDecimal(dr["balance"]);
                        report.InvoiceClient.AddInvoiceClientRow(invoiceClientRow);
                    }

                    var generalRow = report.General.NewGeneralRow();
                    generalRow.start_date = start;
                    generalRow.end_date = end;
                    report.General.AddGeneralRow(generalRow);

                    //modified by dyyr @2016 08 02
                    DataTable clientDT = new DataTable();
                    clientDT.Columns.Add("client", Type.GetType("System.String"));
                    if (client != "ALL")
                    {
                        DataRow clientRow = clientDT.NewRow();
                        string clientNameSql = "select client_name from clients where client_id="+ client;
                        OdbcDataReader dr2 = mydb.Reader(clientNameSql);
                        if (dr2.Read())
                        {
                            clientRow["client"] = "for " + dr2["client_name"];
                            clientDT.Rows.Add(clientRow);
                        }
                    }
                    
                    
                    //modified end

                    #region Set DataSource
                    ReportDataSource rpDataSource = new ReportDataSource("InvoiceClient", (DataTable)report.InvoiceClient);
                    ReportDataSource rpDataSource2 = new ReportDataSource("General", (DataTable)report.General);
                    ReportDataSource rpDataSource3 = new ReportDataSource("ClientLable", clientDT);
                    
                    #endregion
                    #region Load Report Template
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("templates/InvoiceClient.rdlc");
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rpDataSource);
                    ReportViewer1.LocalReport.DataSources.Add(rpDataSource2);
                    ReportViewer1.LocalReport.DataSources.Add(rpDataSource3);
                    ReportViewer1.LocalReport.Refresh();
                    #endregion
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (mydb != null) mydb.Close();
                }
            }
            catch (Exception ex)
            {
                LabelAlertBoard.Text = ex.ToString();
            }
        }

    }
}