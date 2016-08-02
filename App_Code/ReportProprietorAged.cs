using System;
using SystemAlias = System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using Microsoft.Reporting.WebForms;
using Sapp.Common;
using Sapp.Data;
using System.Globalization;

//18/07/2013

public class ReportProprietorAged
{

    #region Variables
    private DataTable generalDT = new DataTable();
    public DataTable Creditor = new DataTable();
    public DataTable SumDT = new DataTable();
    private Odbc o;
    #endregion



    #region Functions
    public void SetReportInfo(DateTime start, DateTime end)
    {
        try
        {
            o = new Odbc(AdFunction.conn);
            Creditor.Columns.Add("Name");
            Creditor.Columns.Add("Unit");
            Creditor.Columns.Add("Code");
            Creditor.Columns.Add("A3Mon", SystemAlias.Type.GetType("System.Decimal"));
            Creditor.Columns.Add("A2Mon", SystemAlias.Type.GetType("System.Decimal"));
            Creditor.Columns.Add("A1Mon", SystemAlias.Type.GetType("System.Decimal"));
            Creditor.Columns.Add("Current", SystemAlias.Type.GetType("System.Decimal"));
            Creditor.Columns.Add("Total", SystemAlias.Type.GetType("System.Decimal"));
            SumDT.Columns.Add("T3Mon", SystemAlias.Type.GetType("System.Decimal"));
            SumDT.Columns.Add("T2Mon", SystemAlias.Type.GetType("System.Decimal"));
            SumDT.Columns.Add("T1Mon", SystemAlias.Type.GetType("System.Decimal"));
            SumDT.Columns.Add("TCurrent", SystemAlias.Type.GetType("System.Decimal"));
            SumDT.Columns.Add("Total", SystemAlias.Type.GetType("System.Decimal"));
            SumDT.Rows.Add(SumDT.NewRow());


            generalDT.Columns.Add("company_name");
            generalDT.Columns.Add("bodycorp_code");
            generalDT.Columns.Add("bodycorp_name");
            generalDT.Columns.Add("start_date");
            generalDT.Columns.Add("end_date");

            DataRow nr = generalDT.NewRow();
            //nr["company_name"] = system.SystemValue;
            //nr["bodycorp_code"] = bodycorp.BodycorpCode;
            //nr["bodycorp_name"] = bodycorp.BodycorpName;

            nr["start_date"] = start.ToString("dd MMM yy");
            nr["end_date"] = end.ToString("dd MMM yy");
            generalDT.Rows.Add(nr);


            DataTable unitmaster = o.ReturnTable("SELECT * FROM clients order by client_name", "yt1");

            foreach (DataRow dr in unitmaster.Rows)
            {
                DataTable temp = o.ReturnTable("SELECT clients.client_name, clients.client_code, invoices.* FROM            clients, invoices WHERE        clients.client_id = invoices.invoice_client_id and invoice_client_id=" + dr["client_id"].ToString(), "t1");

                DataTable dt3 = ReportDT.FilterDT(temp, "invoice_date <= #" + end.AddDays(-90).ToString("yyyy-MM-dd") + "#");
                DataTable dt2 = ReportDT.FilterDT(temp, "invoice_date > #" + end.AddDays(-90).ToString("yyyy-MM-dd") + "# and invoice_date <=#" + end.AddDays(-60).ToString("yyyy-MM-dd") + "#");
                DataTable dt1 = ReportDT.FilterDT(temp, "invoice_date > #" + end.AddDays(-60).ToString("yyyy-MM-dd") + "# and invoice_date <= #" + end.AddDays(-30).ToString("yyyy-MM-dd") + "#");
                DataTable dt = ReportDT.FilterDT(temp, "(invoice_date >#" + end.AddDays(-30).ToString("yyyy-MM-dd") + "# and invoice_date <= #" + end.ToString("yyyy-MM-dd") + "#) or invoice_date is null");



                decimal i3 = ReportDT.SumTotal(dt3, "invoice_gsttotal");
                decimal r3 = ReportDT.SumTotal(ReportDT.FilterDT(dt3, "invoice_paid=1 and invoice_paidamount is null"), "invoice_gsttotal");
                r3 += ReportDT.SumTotal(ReportDT.FilterDT(dt3, "invoice_paidamount is not null"), "invoice_paidamount");
                decimal i2 = ReportDT.SumTotal(dt2, "invoice_gsttotal");
                decimal r2 = ReportDT.SumTotal(ReportDT.FilterDT(dt2, "invoice_paid=1 and invoice_paidamount is null"), "invoice_gsttotal");
                r2 += ReportDT.SumTotal(ReportDT.FilterDT(dt2, "invoice_paidamount is not null"), "invoice_paidamount");
                decimal i1 = ReportDT.SumTotal(dt1, "invoice_gsttotal");
                decimal r1 = ReportDT.SumTotal(ReportDT.FilterDT(dt1, "invoice_paid=1 and invoice_paidamount is null"), "invoice_gsttotal");
                r1 += ReportDT.SumTotal(ReportDT.FilterDT(dt1, "invoice_paidamount is not null"), "invoice_paidamount");
                decimal i = ReportDT.SumTotal(dt, "invoice_gsttotal");
                decimal r = ReportDT.SumTotal(ReportDT.FilterDT(dt, "invoice_paid=1 and invoice_paidamount is null"), "invoice_gsttotal");
                r += ReportDT.SumTotal(ReportDT.FilterDT(dt, "invoice_paidamount is not null"), "invoice_paidamount");

                //decimal ap3 = ReportDT.SumTotal(dt3, "AP");
                //decimal ap2 = ReportDT.SumTotal(dt2, "AP");
                //decimal ap1 = ReportDT.SumTotal(dt1, "AP");
                //decimal ap = ReportDT.SumTotal(dt, "AP");

                DataRow newRow = Creditor.NewRow();
                newRow["Name"] = dr["client_name"].ToString();
                newRow["Code"] = dr["client_code"].ToString();
                newRow["A3Mon"] = i3 - r3;
                newRow["A2Mon"] = i2 - r2;
                newRow["A1Mon"] = i1 - r1;
                //newRow["Unit"] = dr["unit_master_code"].ToString();
                newRow["Current"] = i - r;
                decimal total = i3 - r3 + i2 - r2 + i1 - r1 + i - r;

                //newRow["A3Mon"] = ap3;
                //newRow["A2Mon"] = ap2;
                //newRow["A1Mon"] = ap1;
                //newRow["Unit"] = dr["unit_master_code"].ToString();
                //newRow["Current"] = ap;
                //decimal total = ap3 + ap2 + ap1 + ap;

                newRow["Total"] = total;
                //decimal checktotal = ReportDT.SumTotal(temp, "Balance");
                //if (total != 0)
                Creditor.Rows.Add(newRow);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void Print(ReportViewer rv)
    {
        try
        {
            #region Set DataSource

            ReportDataSource rpDataSource = new ReportDataSource("general", generalDT);
            ReportDataSource rpDataSource2 = new ReportDataSource("Aged", ReportDT.DeleteNullRow(Creditor, "Total"));
            ReportDataSource rpDataSource4 = new ReportDataSource("SumDT", SumDT);
            #endregion
            rv.LocalReport.DataSources.Add(rpDataSource);
            rv.LocalReport.DataSources.Add(rpDataSource2);
            rv.LocalReport.DataSources.Add(rpDataSource4);
            rv.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
}

