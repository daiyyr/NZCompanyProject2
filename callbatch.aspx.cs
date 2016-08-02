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

    public partial class callbatch : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string project_id = "";
        int threadid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (!Page.IsPostBack)
            {

                #region Load File Info
                DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/uploads"));
                FileInfo[] files = di.GetFiles();
                LiteralFileList.Text = "";
                foreach (FileInfo file in files)
                {
                    LiteralFileList.Text += file.Name;
                    LiteralFileList.Text += "<br>";
                }
                #endregion

                MySqlOdbc mydb = new MySqlOdbc(AdFunction.conn);
                try
                {

                    #region Load WebCombo
                    string sql = "";
                    string sql2 = "SELECT account_id AS ID, account_code AS Code, account_number AS Number FROM accounts";
                    WebComboAccount.DataSource = mydb.ReturnTable(sql2, "account");
                    WebComboAccount.DataValueField = "ID";
                    WebComboAccount.DataTextField = "Code";
                    WebComboAccount.DataBind();

                    string sql3 = "SELECT supplier_id AS ID, supplier_name AS Name FROM suppliers";
                    WebComboSupplier.DataSource = mydb.ReturnTable(sql3, "supplier");
                    WebComboSupplier.DataValueField = "ID";
                    WebComboSupplier.DataTextField = "Name";
                    WebComboSupplier.DataBind();

                    #endregion
                    #region Set Default Option
                    if (project_id != "")
                    {
                        sql = "SELECT account_id FROM accounts WHERE account_client_id IN (SELECT project_client_id FROM projects WHERE project_id=" + project_id + ")";
                        OdbcDataReader dr2 = mydb.Reader(sql);
                        if (dr2.Read())
                        {
                            if (dr2["account_id"] != DBNull.Value)
                            {
                                int account_id = Convert.ToInt32(dr2["account_id"]);
                                WebComboAccount.SelectedValue = account_id.ToString();
                            }
                        }
                    }
                    WebComboSupplier.SelectedIndex = 0;
                    #endregion
                    #region If Background Threading
                    int threadcount = 0;
                    sql = "SELECT COUNT(*) FROM threads WHERE thread_name='Batch Import'";
                    OdbcDataReader dr3 = mydb.Reader(sql);
                    if (dr3.Read())
                        threadcount = Convert.ToInt32(dr3[0]);
                    //if (threadcount != 0)
                    //this.RegisterStartupScript("ButtonImport_Click", "<script>ButtonImport_Click();</script>");
                    #endregion
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

        protected void ButtonUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    string dirpath = Server.MapPath("~/uploads");
                    FileUpload1.SaveAs(dirpath + "\\" + FileUpload1.FileName);

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
        }

        protected void ButtonImport_Click(object sender, EventArgs e)
        {
            try
            {
                //GetProgress();
                //this.RegisterStartupScript("ButtonImport_Click", "<script>ButtonImport_Click();</script>");
                #region Define Variables
                string supplier_id = "null";
                string account_id = "null";
                if (WebComboSupplier.SelectedIndex > -1) supplier_id = WebComboSupplier.SelectedValue;
                if (WebComboAccount.SelectedIndex > -1) account_id = WebComboAccount.SelectedValue;
                if (supplier_id == "null") throw new Exception("Incorrect supplier id!");
                if (account_id == "null") throw new Exception("Incorrect account id!");

                if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
                else throw new Exception("Incorrect project id!");

                string uploadpath = Server.MapPath("~/uploads");
                string logpath = HttpContext.Current.Server.MapPath("~/logs/log.txt");
                #endregion
                ImportThread import = new ImportThread(supplier_id, account_id, project_id, uploadpath, logpath, constr);
                //Thread thread = new Thread(new ThreadStart(import.ButtonImport_Click_Threading));

                //threadid = thread.ManagedThreadId;
                import.ButtonImport_Click_Threading();
                #region Insert Thread Info
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    string sql = "INSERT INTO threads VALUES(" + threadid + ", 'Batch Import', " + MyFuncs.FormatDateStr(DateTime.Now)
                        + ", " + MyFuncs.FormatTimeStr(DateTime.Now) + ", 0)";
                    mydb.NonQuery(sql);
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
                //thread.Start();
            }
            catch (Exception ex)
            {
                Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                LabelAlertBoard.Text = ex.ToString();
            }
        }

        protected void ImageButtonRefresh_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (project_id != "")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/callbatch.aspx?projectid=" + Server.UrlEncode(project_id));
            }
        }



        class ImportThread
        {
            string supplier_id;
            string account_id;
            string project_id;
            string uploadpath;
            string logpath;
            string constr;
            int threadid;

            public ImportThread(string supplier_id, string account_id, string project_id, string uploadpath, string logpath, string constr)
            {
                this.supplier_id = supplier_id;
                this.account_id = account_id;
                this.project_id = project_id;
                this.uploadpath = uploadpath;
                this.logpath = logpath;
                this.constr = constr;
                this.threadid = Thread.CurrentThread.ManagedThreadId;
            }

            public void ButtonImport_Click_Threading()
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(uploadpath);
                    FileInfo[] files = di.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        ImportData(file.FullName);
                        file.Delete();
                    }
                }
                catch (Exception ex)
                {
                    MyLog.InsertLog(ex.ToString(), logpath);
                    throw ex;
                }
                finally
                {
                    #region Remove Thread Info
                    MySqlOdbc mydb = null;
                    try
                    {
                        mydb = new MySqlOdbc(constr);
                        string sql = "DELETE FROM threads WHERE thread_name='Batch Import'";
                        mydb.NonQuery(sql);
                    }
                    catch (Exception ex)
                    {
                        MyLog.InsertLog(ex.ToString(), logpath);
                        throw ex;
                    }
                    finally
                    {
                        if (mydb != null) mydb.Close();
                    }
                    #endregion
                }
            }

            protected void ImportData(string path)
            {
                int count = 0;
                string account_number = "";
                MySqlOdbc mydb = null;
                StreamReader reader = null;
                string sql = "";
                int rowIndex = 1;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    //account num?
                    //string sql = "SELECT account_number FROM accounts WHERE (account_supplier_id=" + supplier_id
                    //    + ") AND (account_client_id IN (SELECT project_client_id FROM projects WHERE project_id="
                    //    + project_id + "))";
                    sql = "SELECT account_number FROM accounts where account_id =" + account_id;
                    OdbcDataReader dr = mydb.Reader(sql);
                    if (dr.Read()) account_number = dr[0].ToString();
                    sql = "START TRANSACTION";
                    mydb.NonQuery(sql);

                    if (supplier_id == "1")
                    {
                        #region If supplier is 2talk
                        reader = File.OpenText(path);
                        string row = reader.ReadLine();
                        while (row != null)
                        {
                            rowIndex++;
                            if (count > 0)
                            {
                                #region Remove comma in the field
                                bool quoted = false;
                                for (int i = 0; i < row.Length; i++)
                                {
                                    if (row[i] == '"')
                                    {
                                        if (quoted) quoted = false;
                                        else quoted = true;
                                    }
                                    if (row[i] == ',' && quoted)
                                    {
                                        row = row.Remove(i, 1);
                                        i--;
                                    }
                                }
                                #endregion

                                string[] fields = row.Split(',');
                                if (row[0] == '"')
                                {
                                    for (int i = 0; i < fields.Length; i++)
                                    {
                                        if (i != 11)
                                        {
                                            if (fields[i].Length > 2) fields[i] = fields[i].Substring(1, fields[i].Length - 2);
                                            else fields[i] = "";
                                        }
                                    }
                                }

                                //for (int i = 0; i < fields.Length; i++)
                                //{
                                //    fields[i] = fields[i].Replace("\"", "");
                                //}

                                #region Format Date
                                string ID = fields[0];
                                string BillingGroup = fields[1];
                                string from_number = fields[2];
                                string to_number = fields[3];
                                string description = MyFuncs.FormatStr(fields[4]);                                                                
                                string status = fields[5];
                                string terminated = fields[6];

                                string date = MyFuncs.FormatDateStr(fields[7]);
                                string time = MyFuncs.FormatTimeStr(fields[8]);
                                string datetime = MyFuncs.FormatDateTimeStr(fields[9]);
                                //string date ="'"+ fields[7]+"'";
                                //string time = "'" + fields[8] + "'";
                                //string datetime = "'" + fields[9] + "'";
                                string length = "0";
                                if (fields[10] != "") length = fields[10];
                                string cost = "0";
                                if (fields[12] != "") cost = fields[12];
                                if (cost.Equals("."))
                                    cost = "0";
                                string Smartcode = fields[13];
                                string SmartcodeDescription = fields[14];
                                string type = fields[15];
                                string subtype = fields[16];
                                string number_id = "null";
                                #endregion
                                sql = "INSERT INTO call_batch VALUES(null, " + project_id + ", " + account_id + ", " + number_id
                                    + ", '" + from_number + "', '" + to_number + "', '" + description + "', '" + status
                                    + "', '" + terminated + "', " + date + ", " + time + ", " + datetime + ", " + length
                                    + ", " + cost + ", '" + type + "', '" + subtype + "', 0)";
                                mydb.NonQuery(sql);
                            }
                            row = reader.ReadLine();
                            count++;
                        }
                        #endregion
                    }
                    else if (supplier_id == "2")
                    {
                        #region If supplier is Orcon
                        if (account_number == "") throw new Exception("Incorrect account number!");
                        reader = File.OpenText(path);
                        string row = reader.ReadLine();
                        while (row != null)
                        {
                            if (count > 0)
                            {
                                #region Split by comma
                                string[] fields = row.Split(',');
                                #endregion
                                #region Format Date
                                string Account = fields[0];
                                string Username = fields[1];
                                string Number = fields[2];
                                string Date = MyFuncs.FormatDateStr(fields[3]);
                                string Time = MyFuncs.FormatTimeStr(fields[3]);
                                string DateTime = MyFuncs.FormatDateTimeStr(fields[3]);
                                string Tariff = fields[4];
                                string From = fields[5];
                                string To = fields[6];
                                string Duration = fields[7];
                                string DurationCharged = fields[8];
                                string Discount = fields[9];
                                string Charge = fields[10];
                                string BillDate = fields[11];
                                #endregion
                                #region Insert call record when account number match
                                if (account_number == Account)
                                {
                                    sql = "INSERT INTO call_batch VALUES(null, " + project_id + ", " + account_id + ", null"
                                        + ", '" + From + "', '" + To + "', '" + Tariff + "', null, null, " + Date + ", " + Time
                                        + ", " + DateTime + ", " + DurationCharged + ", " + Charge + ", 'ORCON', 'Calling', 0)";
                                    mydb.NonQuery(sql);
                                }
                                #endregion
                            }
                            row = reader.ReadLine();
                            count++;
                        }
                        #endregion
                    }
                    sql = "COMMIT";
                    mydb.NonQuery(sql);
                }
                catch (Exception ex)
                {

                    MyLog.InsertLog(ex.ToString()+"Rows:"+rowIndex+"|"+sql, logpath);
                    throw ex;
                }
                finally
                {
                    if (mydb != null) mydb.Close();
                    if (reader != null) reader.Close();
                }
            }
        }

        class SubmitThread
        {
            string project_id;
            string supplier_id;
            string logpath;
            string constr;
            int threadid;

            public SubmitThread(string project_id, string supplier_id, string logpath, string constr)
            {
                this.project_id = project_id;
                this.supplier_id = supplier_id;
                this.logpath = logpath;
                this.constr = constr;
                this.threadid = Thread.CurrentThread.ManagedThreadId;
            }

            private int NextBatchID()
            {
                MySqlOdbc mydb = null;
                int next_id = 1;
                try
                {
                    string sql = "SELECT MAX(call_record_batch_id) FROM call_records";
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

            public void Submit()
            {
                try
                {
                    string call_record_supplier_id = supplier_id;
                    MySqlOdbc mydb = null;
                    try
                    {
                        int batch_id = NextBatchID();
                        string sql = "SELECT * FROM call_batch WHERE call_batch_validated=1";
                        mydb = new MySqlOdbc(constr);
                        OdbcDataReader dr = mydb.Reader(sql);
                        sql = "START TRANSACTION";
                        mydb.NonQuery(sql);
                        while (dr.Read())
                        {
                            #region Format Variables
                            string call_record_account_id = MyFuncs.FormatInt(dr["call_batch_account_id"]);
                            string call_record_number_id = MyFuncs.FormatInt(dr["call_batch_number_id"]);
                            string call_record_from_number = MyFuncs.FormatStr(dr["call_batch_from_number"]);
                            string call_record_to_number = MyFuncs.FormatStr(dr["call_batch_to_number"]);
                            string call_record_description = MyFuncs.FormatStr(dr["call_batch_description"]);
                            string call_record_status = MyFuncs.FormatStr(dr["call_batch_status"]);
                            string call_record_terminated = MyFuncs.FormatStr(dr["call_batch_terminated"]);
                            string call_record_date = MyFuncs.FormatDateStr(dr["call_batch_date"]);
                            string call_record_time = MyFuncs.FormatTimeStr(dr["call_batch_time"]);
                            string call_record_datetime = MyFuncs.FormatDateTimeStr(dr["call_batch_datetime"]);
                            string call_record_length = MyFuncs.FormatInt(dr["call_batch_length"]);
                            string call_record_cost = MyFuncs.FormatDecimal(dr["call_batch_cost"]);
                            string call_record_charge = "null";
                            string call_record_type = MyFuncs.FormatStr(dr["call_batch_type"]);
                            string call_record_subtype = MyFuncs.FormatStr(dr["call_batch_subtype"]);
                            #endregion
                            #region Submit Single Transaction
                            sql = "INSERT INTO call_records VALUES(null, " + project_id + "," + call_record_supplier_id + ", "
                                + call_record_account_id + ", " + call_record_number_id + ", '"
                                + call_record_from_number + "', '" + call_record_to_number + "', '"
                                + call_record_description + "', '" + call_record_status + "', '"
                                + call_record_terminated + "', " + call_record_date + ", " + call_record_time + ", "
                                + call_record_datetime + ", " + call_record_length + ", " + call_record_cost + ", "
                                + call_record_charge + ", '" + call_record_type + "', '" + call_record_subtype
                                + "', 0, " + batch_id + ")";
                            mydb.NonQuery(sql);
                            #endregion
                        }
                        sql = "TRUNCATE call_batch";
                        mydb.NonQuery(sql);
                        sql = "COMMIT";
                        mydb.NonQuery(sql);
                        #region Update Projects
                        sql = "UPDATE projects SET project_call_records_imported=1 WHERE project_id=" + project_id;
                        mydb.NonQuery(sql);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        MyLog.InsertLog(ex.ToString(), logpath);
                    }
                    finally
                    {
                        if (mydb != null) mydb.Close();
                        #region Remove Thread Info
                        MySqlOdbc mydb2 = null;
                        try
                        {
                            mydb2 = new MySqlOdbc(constr);
                            string sql = "DELETE FROM threads WHERE thread_name='Batch Submit'";
                            mydb2.NonQuery(sql);
                        }
                        catch (Exception ex)
                        {
                            MyLog.InsertLog(ex.ToString(), logpath);
                        }
                        finally
                        {
                            if (mydb2 != null) mydb2.Close();
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    MyLog.InsertLog(ex.ToString(), logpath);
                }
            }
        }
        protected void ImageButtonValidate_Click(object sender, ImageClickEventArgs e)
        {
            if (!Page.IsValid) return;
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            string logpath = HttpContext.Current.Server.MapPath("~/logs/log.txt");
            ValidationThread validation = new ValidationThread(logpath, constr);
            validation.Validation();

            //if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            //if (Request.QueryString["supplier"] != null) supplier_id = Server.UrlDecode(Request.QueryString["supplier"]);
            SubmitThread submit = new SubmitThread(project_id, WebComboSupplier.SelectedValue, logpath, constr);
            Thread thread = new Thread(new ThreadStart(submit.Submit));
            threadid = thread.ManagedThreadId;
            #region Insert Thread Info
            MySqlOdbc mydb = null;
            try
            {
                mydb = new MySqlOdbc(constr);
                string sql = "INSERT INTO threads VALUES(" + threadid + ", 'Batch Submit', " + MyFuncs.FormatDateStr(DateTime.Now)
                    + ", " + MyFuncs.FormatTimeStr(DateTime.Now) + ", 0)";
                mydb.NonQuery(sql);
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
            thread.Start();
            Response.Redirect("callrecords.aspx?projectid=" + Request.QueryString["projectid"]);

            //if ((WebComboSupplier.SelectedIndex > -1) && (project_id != ""))
            //{
            //    string supplierid = WebComboSupplier.SelectedValue;
            //    Response.BufferOutput = true;
            //    Response.Redirect("~/callvalidation.aspx?supplier=" + Server.UrlEncode(supplierid) + "&projectid=" + Server.UrlEncode(project_id));
            //}
        }
        class ValidationThread
        {
            string logpath;
            string constr;
            int threadid;

            public ValidationThread(string logpath, string constr)
            {
                this.logpath = logpath;
                this.constr = constr;
                this.threadid = Thread.CurrentThread.ManagedThreadId;
            }

            public void Validation()
            {
                #region Validation
                MySqlOdbc mydb = null;
                try
                {
                    string sql = "SELECT * FROM call_batch WHERE call_batch_validated=0";
                    //string sql3 = "UPDATE call_batch SET call_batch_validated=1 WHERE call_batch_account_id is not null and call_batch_date is not null and call_batch_length is not null and call_batch_cost is not null and call_batch_type is not and call_batch_subtype is not null";
                    mydb = new MySqlOdbc(constr);
                    OdbcDataReader dr = mydb.Reader(sql);
                    while (dr.Read())
                    {
                        string call_batch_id = dr["call_batch_id"].ToString();
                        bool valid = true;
                        if (dr["call_batch_account_id"] == DBNull.Value) valid = false;
                        //if (dr["call_batch_number_id"] == DBNull.Text) valid = false;
                        if (dr["call_batch_date"] == DBNull.Value) valid = false;
                        if (dr["call_batch_length"] == DBNull.Value) valid = false;
                        if (dr["call_batch_cost"] == DBNull.Value) valid = false;
                        if (dr["call_batch_type"] == DBNull.Value) valid = false;
                        if (dr["call_batch_subtype"] == DBNull.Value) valid = false;
                        if (valid)
                        {
                            string sql2 = "UPDATE call_batch SET call_batch_validated=1 WHERE call_batch_id=" + call_batch_id;
                            mydb.NonQuery(sql2);
                        }
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    MyLog.InsertLog(ex.ToString(), logpath);
                }
                finally
                {
                    if (mydb != null) mydb.Close();
                    #region Remove Thread Info
                    MySqlOdbc mydb2 = null;
                    try
                    {
                        mydb2 = new MySqlOdbc(constr);
                        string sql = "DELETE FROM threads WHERE thread_name='Batch Validation'";
                        mydb2.NonQuery(sql);
                    }
                    catch (Exception ex)
                    {
                        MyLog.InsertLog(ex.ToString(), logpath);
                    }
                    finally
                    {
                        if (mydb2 != null) mydb2.Close();
                    }
                    #endregion
                }
                #endregion
            }
        }

        protected void ImageButtonDeleteAll_Click(object sender, ImageClickEventArgs e)
        {
            MySqlOdbc mydb = null;
            try
            {
                string sql = "TRUNCATE call_batch";
                mydb = new MySqlOdbc(constr);
                mydb.NonQuery(sql);
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

        protected void ImageButtonClose_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (project_id != "")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/projectdetails.aspx?projectid=" + Server.UrlEncode(project_id));
            }
        }

        protected void CustomValidatorSupplier_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboSupplier.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorAccount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboAccount.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }


        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                string sql = "SELECT call_batch_id AS `ID`, call_batch_project_id AS `Project ID`, call_batch_account_id AS `Account`"
     + ", call_batch_number_id AS `Number`, call_batch_from_number AS `From`, call_batch_to_number AS `To`"
     + ", call_batch_description AS `Description`, call_batch_status AS `Status`, call_batch_terminated AS `Terminated`"
     + ", call_batch_date  AS `Date`, call_batch_time AS `Time`, call_batch_datetime AS `DateTime`, call_batch_length AS `Length`"
     + ", call_batch_cost AS `Cost`, call_batch_type AS `Type`, call_batch_subtype AS `SubType`, call_batch_validated AS `Valid`";

                //DataTable dt = new MySqlOdbc(AdFunction.conn).ReturnTable(sql, "temp");
                string sqlselectstr = "select `ID`, `Project ID`, `Account`, `Number`, `From`, `To`, `Description`, `Status`,`Terminated`,`Date`,`Time`,`DateTime`, `Length`, `Cost`, `Type`, `SubType`, `Valid` from(  " + sql;
                string sqlfromstr = "FROM `call_batch`";
                JQGrid jqgridObj = new JQGrid(postdata, AdFunction.conn, sqlfromstr, sqlselectstr);
                string jsonStr = jqgridObj.GetJSONStr();
                return jsonStr;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            string path = Server.MapPath("~/uploads");
            string[] files = Directory.GetFiles(path);
            foreach(string f in files)
            {
                FileInfo fi = new FileInfo(f);
                fi.Delete();
            }
        }


    }

}