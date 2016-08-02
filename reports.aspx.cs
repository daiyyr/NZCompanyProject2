using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;

namespace telco
{
    public partial class reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //dyyr modified @ 2016 08 02
            MySqlOdbc mydb = null;
            string constr = AdFunction.conn;
            try
            {
                mydb = new MySqlOdbc(constr);
                string sql = "SELECT `client_id` AS `ID`, `client_name` AS `Text` "+
                             "FROM `clients` where client_id is not null "+
                             "order by client_id";
                OdbcDataReader dr = mydb.Reader(sql);
                while (dr.Read())
                {
                    debtor_client.Items.Add(new ListItem(dr["Text"].ToString(), dr["ID"].ToString()));
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
            //modified end  
        }
        protected void ButtonAccountReceivable_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script language='javascript'>");
            sb.Append("window.open('reportviewer.aspx?report=accountreceivable&start=" + StartT.Text + "&end=" + EndT.Text + "', 'ReportViewer',");
            sb.Append("'top=0, left=0, width='+ screen.availwidth +', height='+ screen.availheight +',scrollbars=yes,location=yes, menubar=yes,toolbar=yes,status,resizable=yes,addressbar=yes');");
            sb.Append("</script>");

            Type t = this.GetType();

            if (!ClientScript.IsClientScriptBlockRegistered(t, "PopupScript"))
                ClientScript.RegisterClientScriptBlock(t, "PopupScript", sb.ToString());
        }

        protected void btnSearchDebtorDetails_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script language='javascript'>");
            sb.Append("window.open('reportviewer.aspx?report=invoiceclient&start=" + StartT.Text + "&end=" + EndT.Text + "&client=" + debtor_client.SelectedValue + "', 'ReportViewer',");
            sb.Append("'top=0, left=0, width='+ screen.availwidth +', height='+ screen.availheight +',scrollbars=yes,location=yes, menubar=yes,toolbar=yes,status,resizable=yes,addressbar=yes');");
            sb.Append("</script>");

            Type t = this.GetType();

            if (!ClientScript.IsClientScriptBlockRegistered(t, "PopupScript"))
                ClientScript.RegisterClientScriptBlock(t, "PopupScript", sb.ToString());
        }
    }
}