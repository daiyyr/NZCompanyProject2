using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Data.Odbc;
using System.Xml;


public class Invoice
{
    private int? invoice_id;
    private int? invoice_number;
    private string invoice_date;
    private string invoice_due;
    private string invoice_term;
    private int? client_id;
    private decimal? total;
    private decimal? gst;
    private decimal? gsttotal;
    private bool paid;
    private string paydate;

    private ArrayList invoice_lines;

    public Invoice()
    {
        invoice_id = NextInvoiceID();
        invoice_number = NextInvoiceNum();
        invoice_lines = new ArrayList();
    }

    public Invoice(int invoice_id)
    {
        this.invoice_id = invoice_id;
    }

    public Invoice(string date, string due, string term, int client_id)
    {
        invoice_id = NextInvoiceID();
        invoice_number = NextInvoiceNum();
        invoice_date = date;
        invoice_due = due;
        invoice_term = term;
        this.client_id = client_id;
        invoice_lines = new ArrayList();
    }

    public Invoice(string number, string date, string due, string term, int client_id)
        : this(date, due, term, client_id)
    {
        invoice_number = Convert.ToInt32(number);
    }

    public void AddLine(string product_code, string description, int qty, decimal price, string associate_type, int associate_id)
    {
        ArrayList invoice_line = new ArrayList();
        invoice_line.Add(null);
        invoice_line.Add(invoice_id);
        invoice_line.Add(product_code);
        invoice_line.Add(description);
        invoice_line.Add(qty);
        invoice_line.Add(price);
        invoice_line.Add(associate_type);
        invoice_line.Add(associate_id);
        invoice_lines.Add(invoice_line);
    }

    public void InsertInvoice()
    {
        MySqlOdbc mydb = null;
        string sql = "";
        try
        {
            string constr = AdFunction.conn;
            mydb = new MySqlOdbc(constr);

            sql = "START TRANSACTION";
            mydb.NonQuery(sql);

            sql = "INSERT INTO invoices "
                
                //modified by dyyr @2016 08 02
                //         + " VALUES(" + invoice_id + ", " + invoice_number + ", " + invoice_date + ", " + invoice_due + ", '" + invoice_term + "', " + client_id + ", 0, 0, 0, 0, null)";
                + " VALUES(" + invoice_id + ", " + invoice_number + ", " + invoice_date + ", " + invoice_due + ", '" + invoice_term + "', " + client_id + ", 0, 0, 0, 0, null, null)";

            mydb.NonQuery(sql);

            decimal total = 0;
            foreach (ArrayList line in invoice_lines)
            {
                total += Convert.ToDecimal(line[5]);
                sql = "INSERT INTO invoice_lines VALUES(null, " + line[1].ToString() + ", '" + line[2].ToString()
                    + "', '" + line[3].ToString() + "', " + line[4].ToString() + ", " + line[5].ToString()
                    + ", '" + line[6].ToString() + "', " + line[7].ToString() + ")";
                mydb.NonQuery(sql);
            }

            sql = "UPDATE invoices SET invoice_total=" + total.ToString()
                    + ", invoice_gst=" + Convert.ToString(MyBiz.GST(total))
                    + ", invoice_gsttotal=" + Convert.ToString(total + MyBiz.GST(total))
                    + " WHERE invoice_id=" + invoice_id;
            mydb.NonQuery(sql);
            sql = "COMMIT";
            mydb.NonQuery(sql);
            SetAssociation();
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

    public void DeleteInvoice()
    {
        try
        {
            string constr = AdFunction.conn;
            if (invoice_id != null)
            {
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    string sql = "SELECT * FROM invoice_lines WHERE invoice_line_invoice_id=" + invoice_id;
                    #region Delete Each Invoice Line
                    OdbcDataReader dr = mydb.Reader(sql);
                    bool cr_set = false;
                    while (dr.Read())
                    {
                        string invoice_line_id = "";
                        string associate_type = "";
                        string associate_id = "";
                        if (dr["invoice_line_id"] != DBNull.Value) invoice_line_id = dr["invoice_line_id"].ToString();
                        if (dr["invoice_line_associate_type"] != DBNull.Value) associate_type = dr["invoice_line_associate_type"].ToString();
                        if (dr["invoice_line_associate_id"] != DBNull.Value) associate_id = dr["invoice_line_associate_id"].ToString();
                        if (associate_type == "PC")
                        {
                            sql = "UPDATE projects_contracts SET projects_contracts_billed=0, projects_contracts_invoice_id=null WHERE projects_contracts_id=" + associate_id;
                            mydb.NonQuery(sql);
                        }
                        if ((associate_type == "CR") && !cr_set)
                        {
                            sql = "UPDATE call_records SET call_record_billed=0, call_record_batch_id=null WHERE call_record_project_id=" + associate_id;
                            mydb.NonQuery(sql);
                            cr_set = true;
                        }
                        if (associate_type == "MH")
                        {
                            sql = "UPDATE tasks SET task_billed=0, task_invoice_id=null WHERE task_id=" + associate_id;
                            mydb.NonQuery(sql);
                        }
                        if (associate_type == "PM")
                        {
                            sql = "UPDATE materials SET material_billed=0, material_invoice_id=null WHERE material_id=" + associate_id;
                            mydb.NonQuery(sql);
                        }
                        sql = "DELETE FROM invoice_lines WHERE invoice_line_id=" + invoice_line_id;
                        mydb.NonQuery(sql);
                    }
                    #endregion
                    #region Delete Invoice
                    sql = "DELETE FROM invoices WHERE invoice_id=" + invoice_id;
                    mydb.NonQuery(sql);
                    #endregion
                }
                catch (Exception exx)
                {
                    throw exx;
                }
                finally
                {
                    if (mydb != null) mydb.Close();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private int NextInvoiceID()
    {
        string constr = AdFunction.conn;
        MySqlOdbc mydb = null;
        int next_id = 1;
        try
        {
            string sql = "SELECT MAX(invoice_id) FROM invoices";
            mydb = new MySqlOdbc(constr);
            OdbcDataReader dr = mydb.Reader(sql);
            if (dr.Read())
            {
                if (dr[0] != DBNull.Value) next_id = Convert.ToInt32(dr[0]) + 1;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (mydb != null) mydb.Close();
        }
        return next_id;
    }

    private int NextInvoiceNum()
    {
        string constr = AdFunction.conn;
        MySqlOdbc mydb = null;
        int invoiceNum = 1;
        try
        {
            string sql = "SELECT MAX(invoice_number) FROM invoices";
            mydb = new MySqlOdbc(constr);
            OdbcDataReader dr = mydb.Reader(sql);
            if (dr.Read())
            {
                if (dr[0] != DBNull.Value) invoiceNum = Convert.ToInt32(dr[0]) + 1;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (mydb != null) mydb.Close();
        }
        return invoiceNum;
    }

    public int GetInvoiceID()
    {
        return invoice_id.Value;
    }

    public int GetInvoiceNum()
    {
        return invoice_number.Value;
    }

    private void SetAssociation()
    {
        try
        {
            string constr = AdFunction.conn;
            bool cr_set = false;
            foreach (ArrayList line in invoice_lines)
            {
                string associate_type = line[6].ToString();
                string associate_id = line[7].ToString();
                if (associate_type == "PC")
                {
                    #region Project Contracts
                    MySqlOdbc mydb = null;
                    try
                    {
                        mydb = new MySqlOdbc(constr);
                        string sql = "UPDATE projects_contracts SET projects_contracts_billed=1, projects_contracts_invoice_id="
                            + invoice_id + " WHERE projects_contracts_id=" + associate_id;
                        mydb.NonQuery(sql);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (mydb != null) mydb.Close();
                    }
                    #endregion
                }
                if ((associate_type == "CR") && (!cr_set))
                {
                    #region Call Records
                    MySqlOdbc mydb = null;
                    try
                    {
                        mydb = new MySqlOdbc(constr);
                        string sql = "UPDATE call_records SET call_record_billed=1, call_record_batch_id="
                            + invoice_id + " WHERE call_record_project_id=" + associate_id;
                        mydb.NonQuery(sql);
                        cr_set = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (mydb != null) mydb.Close();
                    }
                    #endregion
                }
                if (associate_type == "MH")
                {
                    #region Man Hours
                    MySqlOdbc mydb = null;
                    try
                    {
                        mydb = new MySqlOdbc(constr);
                        string sql = "UPDATE tasks SET task_billed=1, task_invoice_id="
                            + invoice_id + " WHERE task_id=" + associate_id;
                        mydb.NonQuery(sql);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (mydb != null) mydb.Close();
                    }
                    #endregion
                }
                if (associate_type == "PM")
                {
                    #region Materials
                    MySqlOdbc mydb = null;
                    try
                    {
                        mydb = new MySqlOdbc(constr);
                        string sql = "UPDATE materials SET material_billed=1, material_invoice_id="
                            + invoice_id + " WHERE material_id=" + associate_id;
                        mydb.NonQuery(sql);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (mydb != null) mydb.Close();
                    }
                    #endregion
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void CopyInvoice(string source_id)
    {
        #region Copy Invoice
        MySqlOdbc mydb = null;
        string constr = AdFunction.conn;
        string str_client_id = "";
        try
        {
            string sql = "SELECT * FROM invoices WHERE invoice_id=" + source_id;
            mydb = new MySqlOdbc(constr);
            OdbcDataReader dr = mydb.Reader(sql);
            if (dr.Read())
            {
                str_client_id = MyFuncs.FormatInt(dr["invoice_client_id"]);
            }
            #region Create Invoice XML
            string filename = HttpContext.Current.Server.MapPath("~/xml/invoice.xml");
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("Invoice");
            doc.AppendChild(root);
            XmlNode invoice = doc.SelectSingleNode("Invoice");
            XmlNode invoice_client_id = doc.CreateNode(XmlNodeType.Element, "ClientID", null);
            invoice_client_id.InnerText = str_client_id;
            invoice.AppendChild(invoice_client_id);
            doc.Save(filename);
            #endregion
            #region Create Invoice Lines
            sql = "SELECT * FROM invoice_lines WHERE invoice_line_invoice_id=" + source_id;
            OdbcDataReader dr1 = mydb.Reader(sql);
            invoice_lines = new ArrayList();
            int id = 1;
            while (dr1.Read())
            {
                string product_code = "";
                string description = "";
                string qty = "";
                string price = "";

                if (dr1["invoice_line_product_code"] != DBNull.Value) product_code = dr1["invoice_line_product_code"].ToString();
                if (dr1["invoice_line_description"] != DBNull.Value) description = dr1["invoice_line_description"].ToString();
                if (dr1["invoice_line_qty"] != DBNull.Value) qty = dr1["invoice_line_qty"].ToString();
                if (dr1["invoice_line_price"] != DBNull.Value) price = dr1["invoice_line_price"].ToString();

                ArrayList invoice_line = new ArrayList();
                invoice_line.Add(id);
                id++;
                invoice_line.Add(product_code);
                invoice_line.Add(description);
                invoice_line.Add(qty);
                invoice_line.Add(price);
                invoice_line.Add("");
                invoice_line.Add("");
                invoice_lines.Add(invoice_line);
            }
            #endregion
            #region Create Invoice Lines XML
            string filename2 = HttpContext.Current.Server.MapPath("~/xml/" + HttpContext.Current.User.Identity.Name + "invoiceedit.xml");
            XmlDocument doc2 = new XmlDocument();
            XmlDeclaration dec2 = doc2.CreateXmlDeclaration("1.0", null, null);
            doc2.AppendChild(dec2);
            XmlElement root2 = doc2.CreateElement("Lines");
            doc2.AppendChild(root2);
            XmlNode lines = doc2.SelectSingleNode("Lines");
            #endregion
            #region Update Invoice Lines XML
            foreach (ArrayList invoice_line in invoice_lines)
            {
                XmlNode line = doc2.CreateNode(XmlNodeType.Element, "Line", null);
                XmlNode line_id = doc2.CreateNode(XmlNodeType.Element, "ID", null);
                XmlNode product_code = doc2.CreateNode(XmlNodeType.Element, "Code", null);
                XmlNode description = doc2.CreateNode(XmlNodeType.Element, "Description", null);
                XmlNode qty = doc2.CreateNode(XmlNodeType.Element, "Qty", null);
                XmlNode price = doc2.CreateNode(XmlNodeType.Element, "Price", null);
                XmlNode associate_type = doc2.CreateNode(XmlNodeType.Element, "AssociateType", null);
                XmlNode associate_id = doc2.CreateNode(XmlNodeType.Element, "AssociateID", null);
                line_id.InnerText = invoice_line[0].ToString();
                product_code.InnerText = invoice_line[1].ToString();
                description.InnerText = invoice_line[2].ToString();
                qty.InnerText = invoice_line[3].ToString();
                price.InnerText = invoice_line[4].ToString();
                associate_type.InnerText = invoice_line[5].ToString();
                associate_id.InnerText = invoice_line[6].ToString();
                line.AppendChild(line_id);
                line.AppendChild(product_code);
                line.AppendChild(description);
                line.AppendChild(qty);
                line.AppendChild(price);
                line.AppendChild(associate_type);
                line.AppendChild(associate_id);
                lines.AppendChild(line);
            }
            doc2.Save(filename2);
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
        #endregion
    }
}

