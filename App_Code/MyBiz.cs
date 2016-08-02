using System;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;

public static class MyBiz
{
    public static decimal GST(decimal exl)
    {

        return Convert.ToDecimal(Convert.ToDouble(exl) * 0.15);
    }

    public static decimal[] GetPlanMinutes(string project_id)
    {
        string constr = AdFunction.conn;
        decimal local_minutes = 0;
        decimal national_minutes = 0;
        decimal international_minutes = 0;
        decimal mobile_minutes = 0;
        decimal discounted_minutes = 0;
        decimal[] ret = new decimal[5];
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i] = 0;
        }
        MySqlOdbc mydb = null;
        try
        {
            #region Retrieve Plan Minutes
            mydb = new MySqlOdbc(constr);
            string sql = "SELECT SUM(contract_lc), SUM(contract_nc), SUM(contract_ic), SUM(contract_mc),"
                + " SUM(contract_dc) FROM contracts WHERE contract_id IN (SELECT projects_contracts_contract_id FROM projects_contracts WHERE (projects_contracts_project_id="
                + project_id + "))";
            OdbcDataReader dr = mydb.Reader(sql);
            if (dr.Read())
            {
                if (dr[0] != DBNull.Value) local_minutes = Convert.ToDecimal(dr[0]);
                if (dr[1] != DBNull.Value) national_minutes = Convert.ToDecimal(dr[1]);
                if (dr[2] != DBNull.Value) international_minutes = Convert.ToDecimal(dr[2]);
                if (dr[3] != DBNull.Value) mobile_minutes = Convert.ToDecimal(dr[3]);
                if (dr[4] != DBNull.Value) discounted_minutes = Convert.ToDecimal(dr[4]);
            }
            #endregion

            ret[0] = local_minutes;
            ret[1] = national_minutes;
            ret[2] = international_minutes;
            ret[3] = mobile_minutes;
            ret[4] = discounted_minutes;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (mydb != null) mydb.Close();
        }
        return ret;
    }

    public static decimal GetMobileFlatRate()
    {
        return (decimal)0.25;
    }

    public static decimal GetLocalFlatRate()
    {
        return (decimal)0.02;
    }

    public static decimal GetNationalFlatRate()
    {
        return (decimal)0.06;
    }

    public static decimal GetServiceRate()
    {
        return 120;
    }

    public static bool CreateProjectInvoice(string project_id, bool bill_contracts, bool bill_callrecords, bool bill_materials, bool bill_manhours, bool bill_datausage)
    {
        //PC(Project Contracts), CR(Call Records), MH(Man Hours), PM(Project Materials)
        try
        {
            string constr = AdFunction.conn;
            string client_id = "";
            if (project_id != "")
            {
                MySqlOdbc mydb = null;
                try
                {
                    #region Retrive Billed Client
                    mydb = new MySqlOdbc(constr);
                    string sql = "SELECT project_client_id, project_bill_client_id FROM projects WHERE project_id=" + project_id;
                    OdbcDataReader dr = mydb.Reader(sql);
                    if (dr.Read())
                    {
                        if (dr["project_bill_client_id"] != DBNull.Value) client_id = dr["project_bill_client_id"].ToString();
                        else if (dr["project_client_id"] != DBNull.Value) client_id = dr["project_client_id"].ToString();
                    }
                    #endregion
                    #region Create Invoice Lines
                    ArrayList invoice_lines = new ArrayList();
                    int id = 1;
                    #endregion
                    if (bill_contracts)
                    {
                        #region Add Contract Elements
                        sql = "SELECT * FROM contracts WHERE contract_id IN (SELECT projects_contracts_contract_id FROM projects_contracts WHERE (projects_contracts_project_id="
                            + project_id + ") AND (projects_contracts_billed=0))";
                        OdbcDataReader dr2 = mydb.Reader(sql);
                        while (dr2.Read())
                        {
                            string contract_id = "";
                            string contract_code = "";
                            string contract_name = "";
                            string contract_charge = "";
                            if (dr2["contract_id"] != DBNull.Value) contract_id = dr2["contract_id"].ToString();
                            if (dr2["contract_code"] != DBNull.Value) contract_code = dr2["contract_code"].ToString();
                            else continue;
                            if (dr2["contract_name"] != DBNull.Value) contract_name = dr2["contract_name"].ToString();
                            else continue;
                            if (dr2["contract_charge"] != DBNull.Value) contract_charge = dr2["contract_charge"].ToString();
                            else continue;
                            ArrayList invoice_line = new ArrayList();
                            invoice_line.Add(id);
                            id++;
                            invoice_line.Add(contract_code);
                            invoice_line.Add(contract_name);
                            invoice_line.Add(1);
                            invoice_line.Add(contract_charge);
                            invoice_line.Add("PC");
                            sql = "SELECT projects_contracts_id FROM projects_contracts WHERE projects_contracts_project_id=" + project_id
                                + " AND projects_contracts_contract_id=" + contract_id;
                            OdbcDataReader dr3 = mydb.Reader(sql);
                            string projects_contracts_id = "";
                            if (dr3.Read())
                            {
                                if (dr3["projects_contracts_id"] != DBNull.Value) projects_contracts_id = dr3["projects_contracts_id"].ToString();
                            }
                            invoice_line.Add(projects_contracts_id);
                            invoice_lines.Add(invoice_line);
                        }
                        #endregion
                    }
                    if (bill_callrecords)
                    {
                        #region Add Call Exceed Elements
                        ArrayList invoice_lines2 = BillCallRecords(project_id, client_id);
                        foreach (ArrayList invoice_line in invoice_lines2)
                        {
                            invoice_line[0] = id;
                            id++;
                            invoice_lines.Add(invoice_line);
                        }
                        #endregion
                    }
                    if (bill_manhours)
                    {
                        #region Add Manhour Elements
                        sql = "SELECT * FROM tasks WHERE (task_project_id=" + project_id + ") AND (task_billed=0)";
                        OdbcDataReader dr2 = mydb.Reader(sql);
                        while (dr2.Read())
                        {
                            string task_id = "";
                            string task_duration = "0";
                            string task_description = "";
                            if (dr2["task_id"] != DBNull.Value) task_id = dr2["task_id"].ToString();
                            if (dr2["task_duration"] != DBNull.Value) task_duration = dr2["task_duration"].ToString();
                            if (dr2["task_description"] != DBNull.Value) task_description = dr2["task_description"].ToString();

                            ArrayList invoice_line = new ArrayList();
                            invoice_line.Add(id);
                            id++;
                            invoice_line.Add("Service");

                            if (task_description == "")
                            {
                                if (Convert.ToDecimal(task_duration) > 1) invoice_line.Add(task_duration.ToString() + "hrs");
                                else invoice_line.Add(task_duration.ToString() + "hr");
                            }
                            else
                            {
                                if (Convert.ToDecimal(task_duration) > 1) invoice_line.Add(task_description + "-" + task_duration.ToString() + "hrs");
                                else invoice_line.Add(task_description + "-" + task_duration.ToString() + "hr");
                            }
                            invoice_line.Add(1);
                            invoice_line.Add((Convert.ToDecimal(Convert.ToDecimal(task_duration) * GetServiceRate())).ToString());
                            invoice_line.Add("MH");
                            invoice_line.Add(task_id);
                            invoice_lines.Add(invoice_line);
                        }
                        #endregion
                    }
                    if (bill_materials)
                    {
                        #region Add Material Elements
                        sql = "SELECT * FROM materials WHERE (material_project_id=" + project_id + ") AND (material_billed=0)";
                        OdbcDataReader dr2 = mydb.Reader(sql);
                        while (dr2.Read())
                        {
                            string material_id = "";
                            string material_code = "";
                            string material_name = "";
                            string material_description = "";
                            string material_price = "";
                            string material_qty = "";
                            if (dr2["material_id"] != DBNull.Value) material_id = dr2["material_id"].ToString();
                            if (dr2["material_code"] != DBNull.Value) material_code = dr2["material_code"].ToString();
                            if (dr2["material_name"] != DBNull.Value) material_name = dr2["material_name"].ToString();
                            if (dr2["material_description"] != DBNull.Value) material_description = dr2["material_description"].ToString();
                            if (dr2["material_price"] != DBNull.Value) material_price = dr2["material_price"].ToString();
                            if (dr2["material_qty"] != DBNull.Value) material_qty = dr2["material_qty"].ToString();


                            ArrayList invoice_line = new ArrayList();
                            invoice_line.Add(id);
                            id++;
                            invoice_line.Add(material_code);
                            if (material_name == "")
                            {
                                if (material_description == "")
                                {
                                    invoice_line.Add("-");
                                }
                                else
                                {
                                    invoice_line.Add(material_description);
                                }
                            }
                            else
                            {
                                if (material_description == "")
                                {
                                    invoice_line.Add(material_name);
                                }
                                else
                                {
                                    invoice_line.Add(material_name + "-" + material_description);
                                }
                            }
                            invoice_line.Add(material_qty);
                            invoice_line.Add(material_price);
                            invoice_line.Add("PM");
                            invoice_line.Add(material_id);
                            invoice_lines.Add(invoice_line);
                        }
                        #endregion
                    }
                    if (bill_datausage)
                    {
                        string dsql = "SELECT * FROM            accounts, clients, projects WHERE        accounts.account_client_id = clients.client_id AND clients.client_id = projects.project_client_id and projects.project_id=" + project_id;
                        DataTable listDT = mydb.ReturnTable(dsql, "g1");
                        if (listDT.Rows.Count > 0)
                        {
                            DateTime startdate = DateTime.Parse(listDT.Rows[0]["project_start"].ToString());
                            DateTime deadline = DateTime.Parse(listDT.Rows[0]["project_deadline"].ToString());
                            decimal usage = 0;
                            decimal planeusage = 0;
                            decimal rate = 1;
                            decimal ratemeter = 1;
                            string psql = "SELECT        * FROM            projects, projects_contracts, contracts WHERE        projects.project_id = projects_contracts.projects_contracts_project_id AND projects_contracts.projects_contracts_contract_id = contracts.contract_id and projects.project_id=" + project_id;
                            DataTable pDT = mydb.ReturnTable(psql, "g1");
                            foreach (DataRow pdr in pDT.Rows)
                            {
                                decimal temp = 0;
                                decimal.TryParse(pdr["plan_included_data"].ToString(), out temp);
                                planeusage += temp;

                                decimal.TryParse(pdr["plan_exceed_rate"].ToString(), out rate);

                                decimal.TryParse(pdr["plan_exceed_meter"].ToString(), out ratemeter);
                            }
                            string idlist = "datausage";
                            foreach (DataRow ldr in listDT.Rows)
                            {
                                string usagesql = "SELECT  * FROM  data_usage_detail, data_usage WHERE        data_usage_detail.Data_Usage_ID = data_usage.id and  InvID is null and DataDate>='" + startdate.ToString("yyyy-MM-dd") + "' and DataDate<='" + deadline.ToString("yyyy-MM-dd") + "' and ub_username like '" + ldr["account_number"].ToString() + "%'";
                                DataTable ulistDT = mydb.ReturnTable(usagesql, "g1");
                                foreach (DataRow udr in ulistDT.Rows)
                                {
                                    idlist += udr["ID"].ToString() + ",";
                                    usage += decimal.Parse(udr["Dtot_usage"].ToString());
                                }

                            }
                            idlist = idlist.Substring(0, idlist.Length - 1);
                            if (planeusage < usage)
                            {
                                decimal temp = usage - planeusage;

                                ArrayList invoice_line = new ArrayList();
                                invoice_line.Add(idlist);
                                invoice_line.Add("Exceed Usage");
                                invoice_line.Add("Exceed Usage " + temp + " GB");
                                //invoice_line.Add((rate / ratemeter).ToString("f2"));
                                invoice_line.Add(temp);
                                decimal t = 0;
                                if (bill_contracts)
                                {
                                    t = Math.Ceiling(temp / ratemeter) * rate;
                                }
                                invoice_line.Add(t);
                                invoice_line.Add("");
                                invoice_line.Add("0");
                                invoice_lines.Add(invoice_line);
                            }
                        }
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
                    invoice_client_id.InnerText = client_id;
                    invoice.AppendChild(invoice_client_id);
                    doc.Save(filename);
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
                    return true;
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
            MyLog.InsertLog(ex.ToString());
        }
        return false;
    }

    public static ArrayList BillCallRecords(string project_id, string client_id)
    {
        try
        {
            decimal[] planMins = GetPlanMinutes(project_id);
            decimal local_min = planMins[0];
            decimal national_min = planMins[1];
            decimal international_min = planMins[2];
            decimal mobile_min = planMins[3];
            decimal discounted_min = planMins[4];
            decimal local_charge = 0;
            decimal national_charge = 0;
            decimal international_charge = 0;
            decimal mobile_charge = 0;
            decimal orcon_charge = 0;
            decimal  international_usage = 0;
            decimal mobile_usage = 0;
            decimal local_usage = 0;
            decimal national_usage = 0;


            #region Create Invoice Lines
            ArrayList invoice_lines = new ArrayList();
            #endregion

            string constr = AdFunction.conn;
            MySqlOdbc mydb = null;
            try
            {

                mydb = new MySqlOdbc(constr);
                #region Through Each Call Record
                string sql = "SELECT * FROM call_records WHERE (call_record_project_id=" + project_id + ") AND (call_record_billed=0)";
                OdbcDataReader dr = mydb.Reader(sql);
                while (dr.Read())
                {
                    string call_record_id = "";
                    string call_record_to_number = "";
                    string call_record_length = "";
                    string call_record_cost = "";
                    string call_record_type = "";
                    string call_record_subtype = "";
                    decimal c = 0;
                    if (dr["call_record_id"] != DBNull.Value) call_record_id = dr["call_record_id"].ToString();
                    if (dr["call_record_to_number"] != DBNull.Value) call_record_to_number = dr["call_record_to_number"].ToString();
                    if (dr["call_record_length"] != DBNull.Value) call_record_length = dr["call_record_length"].ToString();
                    if (dr["call_record_cost"] != DBNull.Value) call_record_cost = dr["call_record_cost"].ToString();
                    if (dr["call_record_type"] != DBNull.Value) call_record_type = dr["call_record_type"].ToString();
                    if (dr["call_record_subtype"] != DBNull.Value) call_record_subtype = dr["call_record_subtype"].ToString();
                    call_record_type = call_record_type.Replace(" ", "");
                    call_record_subtype = call_record_subtype.Replace(" ", "");

                    DataTable rexDT = mydb.ReturnTable("select * from rate_cards","t1");
                    decimal rate = GetRate("D",mydb);
                    string rateCode = "";
                    foreach(DataRow rdr in rexDT.Rows)
                    {
                        string reg = rdr["rate_cards_number"].ToString();
                        Regex r = new Regex(reg);
                        if(r.IsMatch(call_record_to_number))
                        {
                            rate = decimal.Parse(rdr["rate_cards_rate"].ToString());
                            rateCode = rdr["rate_cards_code"].ToString();
                        }
                    }
                    decimal length = Convert.ToDecimal(call_record_length);
                    decimal cmin = length / 60;
                    cmin = Math.Ceiling(cmin);
                    decimal temp = 0;
                    #region OLD
                    if (call_record_type != "")
                    {
                        if (call_record_type == "I")
                        {
                            #region International Calls
                            decimal charge = Convert.ToDecimal(call_record_cost);

                            if (call_record_length != "")
                            {

                                international_usage += cmin;
                                if (IsDiscount(call_record_to_number,mydb))
                                { 
        
                                if (discounted_min-cmin < 0)
                                {
                                    temp = cmin - discounted_min;
                                    c = rate * temp;
                                    international_charge += c;
                                }
                                    else
                                {
                                    discounted_min -= cmin;
                                }
                                }
                                else
                                {
                                    c = rate * cmin;
                                    international_charge += c;
                                }
                            }

                            #endregion
                        }
                        else if (call_record_type == "IS" && call_record_subtype == "IN8")
                        {
                            #region 0800 Inbound Calls National
                            if (call_record_length != "" && call_record_length != "0")
                            {
                                national_usage += cmin;
                                if (national_min - cmin < 0)
                                {

                                    if (IsDiscountCode("NZN", mydb))
                                    {
                                        if(discounted_min-cmin<0)
                                        {
                                            temp = cmin - discounted_min;
                                        }
                                        else
                                        {
                                            discounted_min -= cmin;
                                            cmin = 0;
                                        }
                                    }
                                    else
                                    {
                                        temp = cmin;
                                    }

                                }
                                else
                                {
                                    national_min -= cmin;
                                    temp = 0;
                                }

                                c = GetRate("NZN", mydb) * temp;
                                national_charge += c;
                            }

                            #endregion
                        }
                        else if (call_record_type == "S")
                        {
                            #region National Calls
                            if (call_record_length != "")
                            {

                                national_usage += cmin;
                                if (national_min - cmin < 0)
                                {

                                    if (IsDiscountCode("NZN", mydb))
                                    {
                                        if (discounted_min - cmin < 0)
                                        {
                                            temp = cmin - discounted_min;
                                        }
                                        else
                                        {
                                            discounted_min -= cmin;
                                        }
                                    }
                                    else
                                    {
                                        temp = cmin;
                                    }

                                }
                                else
                                {
                                    national_min -= cmin;
                                    temp = 0;
                                }
                                c = GetRate("NZN", mydb) * temp;
                                national_charge += c;
                            }
                            #endregion
                        }
                        else if (call_record_type == "IM" && call_record_subtype == "INM")
                        {
                            #region 0800 Inbound Mobile Calls
                            if (call_record_length != "" && call_record_length != "0")
                            {
                                mobile_usage += cmin;
   
                                if (mobile_min - cmin < 0)
                                {

                                    if (IsDiscountCode("NZM", mydb))
                                    {
                                        if (discounted_min - cmin < 0)
                                        {
                                            temp = cmin - discounted_min;
                                        }
                                        else
                                        {
                                            discounted_min -= cmin;
                                        }
                                    }
                                    else
                                    {
                                        temp = cmin;
                                    }


                                }
                                else
                                {
                                    mobile_min -= cmin;
                                    temp = 0;
                                }
                                c = GetRate("NZM", mydb) * temp;
                                mobile_charge += c;
                            }

                            #endregion
                        }
                        else if (call_record_type == "M")
                        {
                            #region Mobile Calls
                            if (call_record_length != "")
                            {

                                mobile_usage += cmin;
                                if (mobile_min - cmin < 0)
                                {
                                    if (IsDiscountCode("NZM", mydb))
                                    {
                                        if (discounted_min - cmin < 0)
                                        {
                                            temp = cmin - discounted_min;
                                        }
                                        else
                                        {
                                            discounted_min -= cmin;
                                        }
                                    }
                                    else
                                    {
                                        temp = cmin;
                                    }
    
                                }
                                else
                                {
                                    mobile_min -= cmin;
                                    temp = 0;
                                }
                                c = GetRate("NZM", mydb) * temp;
                                mobile_charge += c;
                            }
                            #endregion
                        }
                        else if (call_record_type == "L" && !call_record_type.Equals("IB"))
                        {
                            #region Local Calls
                            if (call_record_length != "")
                            {
                                local_usage += cmin;
                                if (local_min - cmin < 0)
                                {

                                    if (IsDiscountCode("NZL", mydb))
                                    {
                                        if (discounted_min - cmin < 0)
                                        {
                                            temp = cmin - discounted_min;
                                        }
                                        else
                                        {
                                            discounted_min -= cmin;
                                        }
                                    }
                                    else
                                    {
                                        temp = cmin;
                                    }


                                }
                                else
                                {
                                    local_min -= cmin;
                                    temp = 0;
                                }
                                c = GetRate("NZL", mydb) * temp;
                                local_charge += c;
                            }
                            #endregion
                        }


                        else if (call_record_type == "ORCON")
                        {
                            #region ORCON Calling
                            if (Convert.ToDecimal(call_record_cost) > 0)
                            {
                                orcon_charge += Convert.ToDecimal(call_record_cost);
                            }
                            #endregion
                        }
                    }
                    string usql = "update call_records set call_record_charge=" + c + " where call_record_id=" + call_record_id;
                    mydb.ExecuteScalar(usql);
                    international_charge = decimal.Parse(international_charge.ToString("f2"));
                    local_charge = decimal.Parse(local_charge.ToString("f2"));
                    international_charge = decimal.Parse(international_charge.ToString("f2"));
                    mobile_charge = decimal.Parse(mobile_charge.ToString("f2"));
                    national_charge = decimal.Parse(national_charge.ToString("f2"));
                    #endregion
                }
                #endregion
                #region Retrive Start End Date
                string project_start = "";
                string project_end = "";
                sql = "SELECT project_start, project_deadline FROM projects WHERE project_id=" + project_id;
                OdbcDataReader dr2 = mydb.Reader(sql);
                if (dr2.Read())
                {
                    if (dr2["project_start"] != DBNull.Value) project_start = MyFuncs.FormatDateTxt(dr2["project_start"]);
                    if (dr2["project_deadline"] != DBNull.Value) project_end = MyFuncs.FormatDateTxt(dr2["project_deadline"]);
                }
                #endregion
                #region Local Charges
                    //local_usage = Convert.ToInt32(local_usage / 60);
                      int local_exceed=0;
                      if (IsDiscountCode("NZL", mydb))
                          local_exceed = Convert.ToInt32(local_usage - local_min - discounted_min);
                      else
                          local_exceed = Convert.ToInt32(local_usage - local_min);
                    if (local_charge > 0)
                    {
                        //local_charge = local_exceed * GetLocalFlatRate();
                        ArrayList invoice_line = new ArrayList();
                        invoice_line.Add(0);
                        invoice_line.Add("LCEC");
                        invoice_line.Add("Local Call Exceed Charge - Local Call Usage " + local_usage
                            + " Mins Exceed " + local_exceed + " Mins @$0.02/Min"
                            + " Between " + project_start + " to " + project_end);
                        invoice_line.Add(1);
                        invoice_line.Add(local_charge);
                        invoice_line.Add("CR");
                        invoice_line.Add(project_id);
                        invoice_lines.Add(invoice_line);
                    }
                
                #endregion
                #region National Charges
                //national_usage = Convert.ToInt32(national_usage / 60);

                int national_exceed = 0;
                if (IsDiscountCode("NZL", mydb))
                    national_exceed = Convert.ToInt32(national_usage - national_min - discounted_min);
                else
                    national_exceed = Convert.ToInt32(national_usage - national_min);

                if (national_charge > 0)
                {
                    //national_charge = national_exceed * GetNationalFlatRate();
                    ArrayList invoice_line = new ArrayList();
                    invoice_line.Add(0);
                    invoice_line.Add("NCEC");
                    invoice_line.Add("National Call Exceed Charge - National Call Usage " + national_usage
                        + " Mins Exceed " + national_exceed + " Mins @$"+GetRate("NZN",mydb).ToString()  + "/Min"
                         + " Between " + project_start + " to " + project_end);
                    invoice_line.Add(1);
                    invoice_line.Add(national_charge);
                    invoice_line.Add("CR");
                    invoice_line.Add(project_id);
                    invoice_lines.Add(invoice_line);
                }
                #endregion
                #region Mobile Charges
                if (mobile_charge > mobile_min)
                {

                    int mobile_exceed = 0;
                    if (IsDiscountCode("NZL", mydb))
                        mobile_exceed = Convert.ToInt32(mobile_usage - mobile_min - discounted_min);
                    else
                        mobile_exceed = Convert.ToInt32(mobile_usage - mobile_min);
                    if (mobile_exceed > 0)
                    {
                        //mobile_charge = mobile_exceed * GetMobileFlatRate();
                        ArrayList invoice_line = new ArrayList();
                        invoice_line.Add(0);
                        invoice_line.Add("MCEC");
                        invoice_line.Add("Mobile Call Exceed Charge - Mobile Call Usage " + mobile_usage
                            + " Mins Exceed " + mobile_exceed + " Mins @$" + GetRate("NZM",mydb).ToString() + "/Min"
                             + " Between " + project_start + " to " + project_end);
                        invoice_line.Add(1);
                        invoice_line.Add(mobile_charge);
                        invoice_line.Add("CR");
                        invoice_line.Add(project_id);
                        invoice_lines.Add(invoice_line);
                    }
                }
                #endregion
                #region International Charges
                if (international_charge > 0)
                {
                    //international_usage = Convert.ToInt32(international_usage / 60);
                    ArrayList invoice_line = new ArrayList();
                    invoice_line.Add(0);
                    invoice_line.Add("ICEC");
                    invoice_line.Add("International Call Exceed Charge - International Call Usage "
                        + international_usage + " Mins"
                         + " Between " + project_start + " to " + project_end);
                    invoice_line.Add(1);
                    invoice_line.Add(international_charge);
                    invoice_line.Add("CR");
                    invoice_line.Add(project_id);
                    invoice_lines.Add(invoice_line);
                }
                #endregion
                #region Orcon Charges
                if (orcon_charge > 0)
                {
                    ArrayList invoice_line = new ArrayList();
                    invoice_line.Add(0);
                    invoice_line.Add("Calling");
                    invoice_line.Add("Calling From Phoneline");
                    invoice_line.Add(1);
                    invoice_line.Add(orcon_charge);
                    invoice_line.Add("CR");
                    invoice_line.Add(project_id);
                    invoice_lines.Add(invoice_line);
                }
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

            return invoice_lines;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static decimal GetRate(string code, MySqlOdbc o)
    {
        decimal r = 0.2M;
        DataTable dt = new DataTable();
        dt = o.ReturnTable("select * from rate_cards", "t1");
        dt.DefaultView.RowFilter = "rate_cards_code='" + code + "'";
        dt = dt.DefaultView.ToTable();
        if (dt.Rows.Count > 0)
            r = decimal.Parse(dt.Rows[0]["rate_cards_rate"].ToString());
        return r;

    }
    public static bool IsDiscountCode(string code, MySqlOdbc o)
    {
        bool r = false;
        DataTable dt = new DataTable();
        dt = o.ReturnTable("select * from rate_cards", "t1");
        dt.DefaultView.RowFilter = "rate_cards_code='" + code + "'";
        dt = dt.DefaultView.ToTable();
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["rate_cards_discount"].ToString().Equals("1"))
                return true;
            else
                return false;
        }
        return false;
    }
    public static bool IsDiscount(string phoneNum, MySqlOdbc o)
    {
        bool result = false;
        DataTable rexDT = o.ReturnTable("select * from rate_cards", "t1");
        foreach (DataRow rdr in rexDT.Rows)
        {
            string reg = rdr["rate_cards_number"].ToString();
            Regex r = new Regex(reg);
            if (r.IsMatch(phoneNum))
            {
                string dis = rdr["rate_cards_discount"].ToString();
                if (dis.Equals("1"))
                    result = true;
            }
        }
        return result;
    }
}
