using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using Sapp.Data;

public class ReportDT
{
    public static string CDateTime(string date, string f = "dd MMM yy")
    {
        string r = "";
        if (date != null)
            if (!date.Equals(""))
            {
                DateTime d = DateTime.Parse(date);
                r = d.ToString(f);
            }

        return r;
    }
    public static DataTable DeleteNullRow(DataTable dt, string c)
    {
        try
        {
            dt.DefaultView.RowFilter = c + " is not null";
            return dt.DefaultView.ToTable();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static DataTable BulidGeneralDT(string constr, int bid, DateTime start, DateTime end)
    {
        return BulidGeneralDT(constr, bid.ToString(), start.ToString(), end.ToString());
    }
    public static DataTable BulidGeneralDT(string constr, string bid, string start, string end)
    {
        return null;
        //DataTable generalDT = new DataTable();
        //generalDT.Columns.Add("company_name");
        //generalDT.Columns.Add("bodycorp_code");
        //generalDT.Columns.Add("bodycorp_name");
        //generalDT.Columns.Add("start_date");
        //generalDT.Columns.Add("end_date");
        //Odbc o = new Odbc(constr);
        //Sapp.SMS.System system = new System(constr);
        //system.SetOdbc(o);
        //system.LoadData("COMPANYNAME");
        //Bodycorp bodycorp = new Bodycorp(constr);
        //bodycorp.SetOdbc(o);
        //bodycorp.LoadData(Convert.ToInt32(bid));
        //DataRow nr = generalDT.NewRow();
        //nr["company_name"] = system.SystemValue;
        //nr["bodycorp_code"] = bodycorp.BodycorpCode;
        //nr["bodycorp_name"] = bodycorp.BodycorpName;
        //nr["start_date"] = (DateTime.Parse(start)).ToString("dd MMM yy");
        //nr["end_date"] = (DateTime.Parse(end)).ToString("dd MMM yy");
        //generalDT.Rows.Add(nr);
        //return generalDT;
    }
    public static DataTable JoinTable_Full(DataTable dt, string Column, DataTable dt2, string KColumn)
    {
        try
        {
            DataTable rDT = dt.Copy();
            foreach (DataColumn dc in dt2.Columns)
            {
                string NewColumnName = "";
                if (dt2.TableName.Equals("") || dt2.TableName == null)
                {
                    NewColumnName = "#" + dc.ColumnName;
                }
                else
                {
                    NewColumnName = dt2.TableName + "." + dc.ColumnName;
                }
                if (!rDT.Columns.Contains(NewColumnName))
                    rDT.Columns.Add(NewColumnName);
                foreach (DataRow dr in dt2.Rows)//遍历DT2
                {
                    foreach (DataRow rdr in rDT.Rows)
                    {
                        if (rdr[Column].ToString().Equals(dr[KColumn].ToString()))
                        {
                            rdr[NewColumnName] = dr[dc.ColumnName];
                        }
                    }
                }

            }
            return rDT;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static string GetDataByColumn(DataTable dt, string column, string value, string VColumn)
    {
        try
        {
            string data = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[column].ToString().Equals(value))
                    data = dr[VColumn].ToString();
            }
            return data;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static DataRow GetDataRowByColumn(DataTable dt, string column, string value)
    {
        try
        {
            DataRow data = null;
            if (dt != null)
                if (dt.Columns.Contains(column))
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[column].ToString().Equals(value))
                        {
                            data = dr;
                        }
                    }
            return data;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static DataTable getTable(string constr, string tablename)
    {
        Odbc o = new Odbc(constr);
        string sql = "select * from " + tablename;
        return o.ReturnTable(sql, tablename);
    }

    public static DataTable FilterDT(DataTable dt, string filter)
    {
        return FilterDT(dt, filter, "");
    }
    public static DataTable FilterDT(DataTable dt, string filter, string sort)
    {
        dt.DefaultView.RowFilter = filter;
        dt.DefaultView.Sort = sort;
        return dt.DefaultView.ToTable();
    }


    public static decimal SumTotal(DataTable dt, string Column)
    {
        decimal sum = 0;
        if (dt.Rows.Count > 0)
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!dt.Rows[i][Column].ToString().Equals(""))
                    sum += decimal.Parse(dt.Rows[i][Column].ToString());
            }
        return sum;
    }
    public static string SumIf(DataView dv, string VColumn, string CColumn, string CValue)
    {
        return SumIf(dv.ToTable(), VColumn, CColumn, CValue);
    }
    public static string SumIf(DataTable dt, string VColumn, string CColumn, string CValue)
    {
        double sum = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (!dt.Rows[i][VColumn].ToString().Equals(""))
                if (dt.Rows[i][CColumn].ToString().Equals(CValue))
                    sum += double.Parse(dt.Rows[i][VColumn].ToString());
        }
        return sum.ToString();
    }
    public static string Avg(DataTable dt, string Column)
    {
        double avg = 0;
        int count = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (!dt.Rows[i][Column].ToString().Equals(""))
            {
                avg += double.Parse(dt.Rows[i][Column].ToString());
                count++;
            }
        }
        if (count > 0 && avg != 0)
        {
            avg = avg / count;
        }
        else
            avg = 0;
        return avg.ToString("f2");
    }
    public static DataTable GetGLDT()
    {
        //if (HttpContext.Current.Cache["StaticGLDT"] == null)
        //{
        string sql = "SELECT   * FROM      gl_transactions, chart_master, chart_types, chart_classes, gl_tran_types WHERE   gl_transactions.gl_transaction_chart_id = chart_master.chart_master_id AND                 chart_master.chart_master_type_id = chart_types.chart_type_id AND                 chart_types.chart_type_class_id = chart_classes.chart_class_id AND                 gl_transactions.gl_transaction_type_id = gl_tran_types.gl_tran_type_id and `gl_transaction_type_id`<>3 AND `gl_transaction_type_id`<>4";
        Odbc o = new Odbc(AdFunction.conn);
        DataTable glDT = o.ReturnTable(sql, "GL");
        glDT.Columns.Add("FixDate", typeof(DateTime));
        string cinvsql = "SELECT   * FROM      cinvoices, cinvoice_gls, gl_transactions WHERE   cinvoices.cinvoice_id = cinvoice_gls.cinvoice_gl_cinvoice_id AND cinvoice_gls.cinvoice_gl_gl_id = gl_transactions.gl_transaction_id";
        DataTable cinDT = o.ReturnTable(cinvsql, "cinv");
        string invsql = "SELECT   * FROM      gl_transactions, invoice_gls, invoice_master WHERE   gl_transactions.gl_transaction_id = invoice_gls.invoice_gl_gl_id AND                 invoice_gls.invoice_gl_invoice_id = invoice_master.invoice_master_id";
        DataTable invDT = o.ReturnTable(invsql, "inv");
        foreach (DataRow gldr in glDT.Rows)
        {
            string glid = gldr["gl_transaction_id"].ToString();
            foreach (DataRow cdr in cinDT.Rows)
            {
                string cgid = cdr["gl_transaction_id"].ToString();
                if (cgid.Equals(glid))
                {
                    string cdate = cdr["cinvoice_apply"].ToString();
                    if (!cdate.Equals(""))
                    {
                        gldr["FixDate"] = cdate;
                    }
                }
            }
            foreach (DataRow indr in invDT.Rows)
            {
                string igid = indr["gl_transaction_id"].ToString();
                if (igid.Equals(glid))
                {
                    string idate = indr["invoice_master_due"].ToString();
                    if (!idate.Equals(""))
                    {
                        gldr["FixDate"] = idate;
                    }
                }
            }
            if (gldr["FixDate"].ToString().Equals(""))
            {
                gldr["FixDate"] = gldr["gl_transaction_date"].ToString();
            }
        }
        //HttpContext.Current.Cache["StaticGLDT"] = glDT;
        return glDT;
        //}
        //else
        //{
        //    return (DataTable)HttpContext.Current.Cache["StaticGLDT"];
        //}
    }
    //public static DataTable GetGLDT()
    //{
    //    //if (HttpContext.Current.Cache["StaticGLDT"] == null)
    //    //{
    //    string sql = "SELECT   * FROM      gl_transactions, chart_master, chart_types, chart_classes, gl_tran_types WHERE   gl_transactions.gl_transaction_chart_id = chart_master.chart_master_id AND                 chart_master.chart_master_type_id = chart_types.chart_type_id AND                 chart_types.chart_type_class_id = chart_classes.chart_class_id AND                 gl_transactions.gl_transaction_type_id = gl_tran_types.gl_tran_type_id and `gl_transaction_type_id`<>3 AND `gl_transaction_type_id`<>4";
    //    Odbc o = new Odbc(AdFunction.conn);
    //    DataTable glDT = o.ReturnTable(sql, "GL");
    //    glDT.Columns.Add("FixDate", typeof(DateTime));
    //    string cinvsql = "SELECT   * FROM      cinvoices, cinvoice_gls, gl_transactions WHERE   cinvoices.cinvoice_id = cinvoice_gls.cinvoice_gl_cinvoice_id AND cinvoice_gls.cinvoice_gl_gl_id = gl_transactions.gl_transaction_id";
    //    DataTable cinDT = o.ReturnTable(cinvsql, "cinv");
    //    string invsql = "SELECT   * FROM      gl_transactions, invoice_gls, invoice_master WHERE   gl_transactions.gl_transaction_id = invoice_gls.invoice_gl_gl_id AND                 invoice_gls.invoice_gl_invoice_id = invoice_master.invoice_master_id";
    //    DataTable invDT = o.ReturnTable(invsql, "inv");
    //    foreach (DataRow gldr in glDT.Rows)
    //    {
    //        string glid = gldr["gl_transaction_id"].ToString();
    //        foreach (DataRow cdr in cinDT.Rows)
    //        {
    //            string cgid = cdr["gl_transaction_id"].ToString();
    //            if (cgid.Equals(glid))
    //            {
    //                string cdate = cdr["cinvoice_apply"].ToString();
    //                if (!cdate.Equals(""))
    //                {
    //                    gldr["FixDate"] = cdate;
    //                }
    //            }
    //        }
    //        foreach (DataRow indr in invDT.Rows)
    //        {
    //            string igid = indr["gl_transaction_id"].ToString();
    //            if (igid.Equals(glid))
    //            {
    //                string idate = indr["invoice_master_due"].ToString();
    //                if (!idate.Equals(""))
    //                {
    //                    gldr["FixDate"] = idate;
    //                }
    //            }
    //        }
    //        if (gldr["FixDate"].ToString().Equals(""))
    //        {
    //            gldr["FixDate"] = gldr["gl_transaction_date"].ToString();
    //        }
    //    }
    //    //HttpContext.Current.Cache["StaticGLDT"] = glDT;
    //    return glDT;
    //    //}
    //    //else
    //    //{
    //    //    return (DataTable)HttpContext.Current.Cache["StaticGLDT"];
    //    //}
    ////}
}
