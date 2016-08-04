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
using System.IO;
using System.Xml;
using System.Text;
using System.Data.Odbc;
using Sapp.JQuery;
using Sapp.Common;


namespace telco
{
    public partial class invoiceedit : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string mode = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["mode"] != null) mode = Server.UrlDecode(Request.QueryString["mode"]);
                if (mode == "add")
                {
                    #region Load Page
                    MySqlOdbc mydb = null;
                    try
                    {
                        mydb = new MySqlOdbc(constr);
                        #region WebComboCustomer
                        string sql = "SELECT * FROM clients";
                        WebComboCustomer.DataSource = mydb.Reader(sql);
                        WebComboCustomer.DataValueField = "client_id";
                        WebComboCustomer.DataTextField = "client_name";
                        WebComboCustomer.DataBind();
                        #endregion
                        WebNumericEditInvoiceNumber.Text = AdFunction.GetNextNumber("INVOICEPREFIX", "INVOICEPILOT", "INVOICEDIGIT", "invoices", "invoice_number");
                        WebDateChooserInvoiceDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                        ResetXMLContent();
                        LoadWebGrid();
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
                else if (mode == "edit")
                {
                    #region Load Page
                    MySqlOdbc mydb = null;
                    try
                    {
                        mydb = new MySqlOdbc(constr);
                        #region WebComboCustomer
                        string sql = "SELECT * FROM clients";
                        WebComboCustomer.DataSource = mydb.Reader(sql);
                        WebComboCustomer.DataValueField = "client_id";
                        WebComboCustomer.DataTextField = "client_name";
                        WebComboCustomer.DataBind();
                        #endregion
                        WebNumericEditInvoiceNumber.Text = new Invoice().GetInvoiceNum().ToString();
                        WebDateChooserInvoiceDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
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
                    #region Load XML Data
                    try
                    {
                        string filename = Server.MapPath("~/xml/invoice.xml");
                        XmlDocument doc = new XmlDocument();
                        doc.Load(filename);
                        XmlNode invoice = doc.SelectSingleNode("Invoice");
                        if (invoice.HasChildNodes)
                        {
                            XmlNode client_id = invoice.SelectSingleNode("ClientID");
                            WebComboCustomer.SelectedValue = (client_id.InnerText);
                            LoadWebGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                        LabelAlertBoard.Text = ex.ToString();
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

        private void ResetXMLContent()
        {
            try
            {
                string filename = Server.MapPath("~/xml/" + HttpContext.Current.User.Identity.Name + "invoiceedit.xml");
                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
                doc.AppendChild(dec);
                XmlElement root = doc.CreateElement("Lines");
                doc.AppendChild(root);
                doc.Save(filename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LoadWebGrid()
        {
            try
            {
                string filename = HttpContext.Current.Server.MapPath("~/xml/" + HttpContext.Current.User.Identity.Name + "invoiceedit.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                DataTable dt = new DataTable("temp");
                dt.Columns.Add("ID");
                dt.Columns.Add("Code");
                dt.Columns.Add("Description");
                dt.Columns.Add("Qty");
                dt.Columns.Add("Price");
                XmlNode node = doc.SelectSingleNode("Lines");
                if (node.HasChildNodes)
                {
                    DataSet dsWebGrid = new DataSet("WebGrid");
                    dsWebGrid.ReadXml(filename);
                    return dsWebGrid.Tables[0];
                    //XGridView.BindData(UltraWebGrid1, dsWebGrid.Tables[0]);
                    //UltraWebGrid1.DataSource = dsWebGrid;
                    //UltraWebGrid1.DataBind();
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int NextXMLID()
        {
            int id = 1;
            try
            {
                string filename = Server.MapPath("~/xml/" + HttpContext.Current.User.Identity.Name + "invoiceedit.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNode lines = doc.SelectSingleNode("Lines");

                if (lines.HasChildNodes)
                {
                    XmlNodeList linelist = lines.ChildNodes;
                    foreach (XmlNode line in linelist)
                    {
                        XmlNode line_id = line.SelectSingleNode("ID");
                        string innertext = line_id.InnerText;
                        if (innertext != "")
                        {
                            if (id <= Convert.ToInt32(innertext))
                            {
                                id = Convert.ToInt32(innertext) + 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }


        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            #region Validation
            bool isValid = true;
            if (TextBoxProductCode.Text == "") isValid = false;
            if (WebNumericEditQty.Text == null) isValid = false;
            if (WebNumericEditPrice.Text == null) isValid = false;
            if (!isValid) return;
            #endregion
            if (Request.QueryString["mode"] != null) mode = Server.UrlDecode(Request.QueryString["mode"]);
            #region Add
            if (mode == "add")
            {
                try
                {
                    string filename = Server.MapPath("~/xml/" + HttpContext.Current.User.Identity.Name + "invoiceedit.xml");
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filename);
                    XmlNode lines = doc.SelectSingleNode("Lines");
                    XmlNode line = doc.CreateNode(XmlNodeType.Element, "Line", null);
                    XmlNode line_id = doc.CreateNode(XmlNodeType.Element, "ID", null);
                    XmlNode product_code = doc.CreateNode(XmlNodeType.Element, "Code", null);
                    XmlNode description = doc.CreateNode(XmlNodeType.Element, "Description", null);
                    XmlNode qty = doc.CreateNode(XmlNodeType.Element, "Qty", null);
                    XmlNode price = doc.CreateNode(XmlNodeType.Element, "Price", null);
                    XmlNode associate_type = doc.CreateNode(XmlNodeType.Element, "AssociateType", null);
                    XmlNode associate_id = doc.CreateNode(XmlNodeType.Element, "AssociateID", null);
                    line_id.InnerText = NextXMLID().ToString();
                    product_code.InnerText = TextBoxProductCode.Text;
                    description.InnerText = TextBoxDescription.Text;
                    if (WebNumericEditQty.Text != null) qty.InnerText = WebNumericEditQty.Text.ToString();
                    if (WebNumericEditPrice.Text != null) price.InnerText = WebNumericEditPrice.Text.ToString();
                    associate_type.InnerText = "NA";
                    associate_id.InnerText = "0";
                    line.AppendChild(line_id);
                    line.AppendChild(product_code);
                    line.AppendChild(description);
                    line.AppendChild(qty);
                    line.AppendChild(price);
                    line.AppendChild(associate_type);
                    line.AppendChild(associate_id);
                    lines.AppendChild(line);
                    doc.Save(filename);
                    #region Reset Form
                    LiteralID.Text = "";
                    TextBoxProductCode.Text = "";
                    TextBoxDescription.Text = "";
                    WebNumericEditQty.Text = null;
                    WebNumericEditPrice.Text = null;
                    #endregion
                    LoadWebGrid();
                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
            }
            #endregion
            #region Edit
            if (mode == "edit")
            {
                try
                {
                    string filename = Server.MapPath("~/xml/" + HttpContext.Current.User.Identity.Name + "invoiceedit.xml");
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filename);
                    XmlNode lines = doc.SelectSingleNode("Lines");
                    XmlNode line = doc.CreateNode(XmlNodeType.Element, "Line", null);
                    XmlNode line_id = doc.CreateNode(XmlNodeType.Element, "ID", null);
                    XmlNode product_code = doc.CreateNode(XmlNodeType.Element, "Code", null);
                    XmlNode description = doc.CreateNode(XmlNodeType.Element, "Description", null);
                    XmlNode qty = doc.CreateNode(XmlNodeType.Element, "Qty", null);
                    XmlNode price = doc.CreateNode(XmlNodeType.Element, "Price", null);
                    XmlNode associate_type = doc.CreateNode(XmlNodeType.Element, "AssociateType", null);
                    XmlNode associate_id = doc.CreateNode(XmlNodeType.Element, "AssociateID", null);
                    line_id.InnerText = NextXMLID().ToString();
                    product_code.InnerText = TextBoxProductCode.Text;
                    description.InnerText = TextBoxDescription.Text;
                    if (WebNumericEditQty.Text != null) qty.InnerText = WebNumericEditQty.Text.ToString();
                    if (WebNumericEditPrice.Text != null) price.InnerText = WebNumericEditPrice.Text.ToString();
                    associate_type.InnerText = "NA";
                    associate_id.InnerText = "0";
                    line.AppendChild(line_id);
                    line.AppendChild(product_code);
                    line.AppendChild(description);
                    line.AppendChild(qty);
                    line.AppendChild(price);
                    line.AppendChild(associate_type);
                    line.AppendChild(associate_id);
                    lines.AppendChild(line);
                    doc.Save(filename);
                    #region Reset Form
                    LiteralID.Text = "";
                    TextBoxProductCode.Text = "";
                    TextBoxDescription.Text = "";
                    WebNumericEditQty.Text = null;
                    WebNumericEditPrice.Text = null;
                    #endregion
                    LoadWebGrid();
                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
            }
            #endregion
        }

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            #region Validation
            bool isValid = true;
            if (TextBoxProductCode.Text == "") isValid = false;
            if (WebNumericEditQty.Text == null) isValid = false;
            if (WebNumericEditPrice.Text == null) isValid = false;
            if (!isValid) return;
            #endregion
            if (Request.QueryString["mode"] != null) mode = Server.UrlDecode(Request.QueryString["mode"]);
            try
            {
                string filename = Server.MapPath("~/xml/" + HttpContext.Current.User.Identity.Name + "invoiceedit.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                string id = LiteralID.Text;
                XmlNode lines = doc.SelectSingleNode("Lines");
                if (lines.HasChildNodes)
                {
                    XmlNodeList linelist = lines.ChildNodes;
                    foreach (XmlNode line in linelist)
                    {
                        XmlNode line_id = line.SelectSingleNode("ID");
                        string innertext = line_id.InnerText;
                        if (id == innertext)
                        {
                            XmlNode product_code = line.SelectSingleNode("Code");
                            product_code.InnerText = TextBoxProductCode.Text;
                            XmlNode description = line.SelectSingleNode("Description");
                            description.InnerText = TextBoxDescription.Text;
                            XmlNode qty = line.SelectSingleNode("Qty");
                            qty.InnerText = WebNumericEditQty.Text.ToString();
                            XmlNode price = line.SelectSingleNode("Price");
                            price.InnerText = WebNumericEditPrice.Text.ToString();
                            break;
                        }
                    }
                }
                doc.Save(filename);
                #region Reset Form
                LiteralID.Text = "";
                TextBoxProductCode.Text = "";
                TextBoxDescription.Text = "";
                WebNumericEditQty.Text = null;
                WebNumericEditPrice.Text = null;
                #endregion
                #region Format Form
                ButtonAdd.Visible = true;
                ButtonUpdate.Visible = false;
                LiteralTitle.Text = "Add New Line:";
                #endregion
                LoadWebGrid();
            }
            catch (Exception ex)
            {
                Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                LabelAlertBoard.Text = ex.ToString();
            }
        }


        protected void ImageButtonCreateInvoice_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;
                if (Request.QueryString["mode"] != null) mode = Server.UrlDecode(Request.QueryString["mode"]);
                #region Retrieve Invoice Info
                string invoice_number = WebNumericEditInvoiceNumber.Text.ToString();
                string invoice_date = MyFuncs.FormatDateStr(WebDateChooserInvoiceDate.Text);
                string invoice_due = MyFuncs.FormatDateStr(WebDateChooserInvoiceDue.Text);
                string invoice_term = TextBoxTerm.Text;
                int invoice_client_id = Convert.ToInt32(WebComboCustomer.SelectedValue);
                #endregion
                #region Create Invoice
                Invoice invoice = new Invoice(invoice_number, invoice_date, invoice_due, invoice_term, invoice_client_id);
                #endregion
                #region Add Lines
                string filename = Server.MapPath("~/xml/" + HttpContext.Current.User.Identity.Name + "invoiceedit.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNode lines = doc.SelectSingleNode("Lines");
                string datausage_id = "";
                if (lines.HasChildNodes)
                {
                    XmlNodeList linelist = lines.ChildNodes;
                    foreach (XmlNode line in linelist)
                    {
                        datausage_id = line.SelectSingleNode("ID").InnerText;
                        if (datausage_id.Contains("datausage"))
                        {
                            datausage_id = datausage_id.Replace("datausage", "");
                            string[] ids = datausage_id.Split(',');
                            foreach (string t in ids)
                            {
                                MySqlOdbc odbc = new MySqlOdbc(AdFunction.conn);
                                DataTable usage_detailDT = odbc.ReturnTable("select * from data_usage_detail where id=" + t, "tt");
                                string updatesql = "update data_usage set BillDate=" + invoice_date + " where ID=" + usage_detailDT.Rows[0]["Data_Usage_ID"].ToString();

                                odbc.ExecuteScalar(updatesql);

                            }
                        }
                        XmlNode Code = line.SelectSingleNode("Code");
                        XmlNode Description = line.SelectSingleNode("Description");
                        XmlNode Qty = line.SelectSingleNode("Qty");
                        XmlNode Price = line.SelectSingleNode("Price");
                        XmlNode AssociateType = line.SelectSingleNode("AssociateType");
                        XmlNode AssociateID = line.SelectSingleNode("AssociateID");
                        invoice.AddLine(Code.InnerText, Description.InnerText, Convert.ToInt32(Qty.InnerText),
                            Convert.ToDecimal(Price.InnerText), AssociateType.InnerText, Convert.ToInt32(AssociateID.InnerText));
                    }
                }
                #endregion
                #region Update Invoice
                invoice.InsertInvoice();

                //modified by dyyr @ 20160803
                //if (!datausage_id.Equals(""))
                if (false)
                {
                    datausage_id = datausage_id.Replace("datausage", "");
                    string[] ids = datausage_id.Split(',');
                    foreach (string t in ids)
                    {
                        string updatesql = "update data_usage_detail set InvID=" + invoice.GetInvoiceID() + " where id=" + t;
                        MySqlOdbc odbc = new MySqlOdbc(AdFunction.conn);
                        odbc.ExecuteScalar(updatesql);
                    }
                }
                #endregion
                #region Redirect To Invoice Details Page
                Response.BufferOutput = true;
                Response.Redirect("~/invoicedetails.aspx?invoiceid=" + Server.UrlEncode(invoice.GetInvoiceID().ToString()));
                #endregion
            }
            catch (Exception ex)
            {
                Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                LabelAlertBoard.Text = ex.ToString();
            }
        }



        protected void CustomValidatorInvoiceNumber_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebNumericEditInvoiceNumber.Text != null) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorInvoiceDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebDateChooserInvoiceDate.Text != null) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorCustomer_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboCustomer.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }

        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                //string invoiceid = HttpContext.Current.Session["invoiceid"].ToString();

                DataTable dt = LoadWebGrid();
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