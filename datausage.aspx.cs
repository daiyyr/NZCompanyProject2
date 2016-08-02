using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using Sapp.JQuery;
using Sapp.Common;

namespace telco
{

    public partial class datausage : System.Web.UI.Page, IPostBackEventHandler
    {
        string constr = AdFunction.conn;

        protected void Page_Load(object sender, EventArgs e)
        {
            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);
        }

        protected void ButtonUpload_Click(object sender, EventArgs e)
        {

        }
        public void RaisePostBackEvent(string eventArgument)
        {

            string[] args = eventArgument.Split('|');
            string project_id = args[1];
            if (args[0] == "ImageButtonDelete")
            {
                MySqlOdbc mydb = new MySqlOdbc(constr);
                try
                {

                    DataTable tempDT = mydb.ReturnTable("select * from data_usage where ID=" + project_id, "tr1");
                    string batch = tempDT.Rows[0]["Batch"].ToString();
                    DataTable checkDT = mydb.ReturnTable("select * from data_usage_detail where InvID is not null and  Batch='" + batch + "'", "check");
                    if (checkDT.Rows.Count == 0)
                    {

                        string sql = "delete from data_usage_detail where Batch ='" + batch + "'";
                        mydb.NonQuery(sql);
                        sql = "delete from data_usage where Batch ='" + batch + "'";
                        mydb.NonQuery(sql);
                    }
                    else
                    { throw new Exception("Can not be delete please delete the invoice frist"); }
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
            else if (args[0] == "ImageButtonDetails")
            {


            }
            else if (args[0] == "ImageButtonEdit")
            {

            }
            else if (args[0] == "ImageButtonCall")
            {

            }
            else if (args[0] == "ImageButtonCopy")
            {
            }
        }





        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                string sql = "select * from data_usage";

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

        public string Qute(string s)
        {
            return "'" + s + "'";
        }
        protected void ButtonImport_Click(object sender, EventArgs e)
        {

            string dirpath = Server.MapPath("~/uploads/DataUsage");
            DataTable dt = CsvDT.CsvToDataTable(dirpath, "\\DataUsage.csv");
            MySqlOdbc mydb = new MySqlOdbc(constr);

            foreach (DataRow dr in dt.Rows)
            {
                string v = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    v += Qute(dr[dc.ColumnName].ToString().Replace("'", "")) + ",";
                }
                v = v.Substring(0, v.Length - 1);
                string batch = DateTime.Parse(StartDateT.Text).ToString("yyyyMMdd");
                string sql = "insert into data_usage values(null,'" + batch + "','" + DateTime.Parse(StartDateT.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(EndDateT.Text).ToString("yyyy-MM-dd") + "',null," + v + ")";
                mydb.ExecuteScalar(sql);
                string idsql = "SELECT LAST_INSERT_ID()";
                string id = mydb.ExecuteScalar(idsql).ToString();
                DataTable temp = mydb.ReturnTable("select * from data_usage where ID=" + id, "tt");
                decimal tot_upload = 0;
                decimal.TryParse(temp.Rows[0]["tot_upload"].ToString(), out tot_upload);
                decimal tot_download = 0;
                decimal.TryParse(temp.Rows[0]["tot_download"].ToString(), out tot_download);
                decimal tot_offpeakupload = 0;
                decimal.TryParse(temp.Rows[0]["tot_offpeakupload"].ToString(), out tot_offpeakupload);
                decimal tot_offpeakdownload = 0;
                decimal.TryParse(temp.Rows[0]["tot_offpeakdownload"].ToString(), out tot_offpeakdownload);
                decimal tot_usage = 0;
                decimal.TryParse(temp.Rows[0]["tot_usage"].ToString(), out tot_usage);

                int days = int.Parse(DaysDL.SelectedValue);

                tot_upload = tot_upload / days;
                tot_download = tot_download / days;
                tot_offpeakupload = tot_offpeakupload / days;
                tot_offpeakdownload = tot_offpeakdownload / days;
                tot_usage = tot_usage / days;
                for (int i = 0; i < days; i++)
                {
                    sql = "insert into data_usage_detail values(null,null,'" + batch + "','" + DateTime.Parse(StartDateT.Text).AddDays(i).ToString("yyyy-MM-dd") + "'," + id + ",null,'" + tot_upload.ToString("f2") + "','" + tot_download.ToString("f2") + "','" + tot_offpeakupload.ToString("f2") + "','" + tot_offpeakdownload.ToString("f2") + "','" + tot_usage.ToString("f2") + "')";
                    mydb.ExecuteScalar(sql);
                }
            }
        }


        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            string dirpath = Server.MapPath("~/uploads/DataUsage/DataUsage.csv");
            File.Delete(dirpath);
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            string dirpath = Server.MapPath("~/uploads/DataUsage");
            if (FileUpload1.HasFile)
            {
                try
                {

                    if (!Directory.Exists(dirpath))
                        Directory.CreateDirectory(dirpath);
                    FileUpload1.SaveAs(dirpath + "\\DataUsage.csv");

                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
            }
            else
            {
                Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                LabelAlertBoard.Text = "You have not specified a file.";
            }
            //data_usage_report_in_Feb_2015_2015-02-02
            string name = FileUpload1.FileName.Substring(30, 10);
            StartDateT.Text = DateTime.Parse(name).AddDays(-int.Parse(DaysDL.SelectedValue)).ToString("dd/MM/yyyy");
            EndDateT.Text = DateTime.Parse(name).AddDays(-1).ToString("dd/MM/yyyy");

        }





    }

}