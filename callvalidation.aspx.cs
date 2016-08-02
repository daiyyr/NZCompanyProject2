using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
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
    public partial class callvalidation : System.Web.UI.Page, IPostBackEventHandler
    {
        string constr = AdFunction.conn;
        string supplier_id = "";
        string project_id = "";
        int threadid = 0;

        public void RaisePostBackEvent(string eventArgument)
        {
            try
            {
                string[] args = eventArgument.Split('|');
                string id = args[1];
                if (args[0] == "ImageButtonDelete")
                {

                }
                else if (args[0] == "ImageButtonDetails")
                {
                }
                else if (args[0] == "ImageButtonEdit")
                {
                    #region Format Variables
                    string call_batch_id = LiteralID.Text;
                    string call_batch_account_id = "null";
                    if (WebComboAccount.SelectedIndex > -1) call_batch_account_id = WebComboAccount.SelectedValue;
                    string call_batch_number_id = "null";
                    if (WebComboNumber.SelectedIndex > -1) call_batch_number_id = WebComboNumber.SelectedValue;
                    string call_batch_from_number = TextBoxFrom.Text.Replace("'", "\\'");
                    string call_batch_to_number = TextBoxTo.Text.Replace("'", "\\'");
                    string call_batch_description = TextBoxDescription.Text.Replace("'", "\\'");
                    string call_batch_status = TextBoxStatus.Text.Replace("'", "\\'");
                    string call_batch_terminated = TextBoxTerminated.Text.Replace("'", "\\'");
                    string call_batch_date = MyFuncs.FormatDateStr(WebDateTimeEditDate.Text);
                    string call_batch_time = MyFuncs.FormatTimeStr(WebDateTimeEditTime.Text);
                    string call_batch_datetime = MyFuncs.FormatDateTimeStr(WebDateTimeEditDateTime.Text);
                    string call_batch_length = MyFuncs.FormatInt(WebNumericEditLenghth.Text);
                    string call_batch_cost = MyFuncs.FormatDecimal(WebCurrencyEditCost.Text);
                    string call_batch_type = TextBoxType.Text.Replace("'", "\\'");
                    string call_batch_subtype = TextBoxSubType.Text.Replace("'", "\\'");
                    #endregion
                    #region Update
                    MySqlOdbc mydb = null;
                    try
                    {
                        string sql = "UPDATE call_batch SET call_batch_account_id=" + call_batch_account_id
                            + ", call_batch_number_id=" + call_batch_number_id
                            + ", call_batch_from_number='" + call_batch_from_number + "', call_batch_to_number='" + call_batch_to_number
                            + "', call_batch_description='" + call_batch_description + "', call_batch_status='" + call_batch_status
                            + "', call_batch_terminated='" + call_batch_terminated + "', call_batch_date=" + call_batch_date + ", call_batch_time=" + call_batch_time + ", call_batch_datetime=" + call_batch_datetime + ", call_batch_length=" + call_batch_length + ", call_batch_cost=" + call_batch_cost + ", call_batch_type='" + call_batch_type + "', call_batch_subtype='" + call_batch_subtype + "', call_batch_validated=0"
                            + " WHERE call_batch_id=" + call_batch_id;
                        mydb = new MySqlOdbc(constr);
                        mydb.NonQuery(sql);
                        Response.BufferOutput = true;
                        Response.Redirect("~/callvalidation.aspx");
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
                else if (args[0] == "ImageButtonCall")
                {

                }
                else if (args[0] == "ImageButtonCopy")
                {
                    #region Format Form
                    ButtonAdd.Visible = true;
                    ButtonUpdate.Visible = false;
                    LiteralTitle.Text = "Add New Call Record:";
                    LiteralID.Text = "";
                    #endregion
                    #region Retrieve
                    string batch_id = id;
                    MySqlOdbc mydb = null;
                    try
                    {
                        string sql = "SELECT * FROM call_batch WHERE call_batch_id=" + batch_id;
                        mydb = new MySqlOdbc(constr);
                        OdbcDataReader dr = mydb.Reader(sql);
                        if (dr.Read())
                        {
                            if (dr["call_batch_account_id"] != DBNull.Value)
                                WebComboAccount.SelectedValue = dr["call_batch_account_id"].ToString();
                            if (dr["call_batch_number_id"] != DBNull.Value)
                                WebComboNumber.SelectedValue = dr["call_batch_number_id"].ToString();
                            TextBoxFrom.Text = dr["call_batch_from_number"].ToString();
                            TextBoxTo.Text = dr["call_batch_to_number"].ToString();
                            TextBoxDescription.Text = dr["call_batch_description"].ToString();
                            TextBoxStatus.Text = dr["call_batch_status"].ToString();
                            TextBoxTerminated.Text = dr["call_batch_terminated"].ToString();
                            if (dr["call_batch_date"] != DBNull.Value) WebDateTimeEditDate.Text = dr["call_batch_date"].ToString();
                            if (dr["call_batch_time"] != DBNull.Value) WebDateTimeEditTime.Text = dr["call_batch_time"].ToString();
                            if (dr["call_batch_datetime"] != DBNull.Value) WebDateTimeEditDateTime.Text = dr["call_batch_datetime"].ToString();
                            if (dr["call_batch_length"] != DBNull.Value) WebNumericEditLenghth.Text = dr["call_batch_length"].ToString();
                            if (dr["call_batch_cost"] != DBNull.Value) WebCurrencyEditCost.Text = dr["call_batch_cost"].ToString();
                            TextBoxType.Text = dr["call_batch_type"].ToString();
                            TextBoxSubType.Text = dr["call_batch_subtype"].ToString();
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

                    #endregion
                }
                else if (args[0] == "ImageButtonValidate")
                {
                    if (!id.Equals("-1"))
                    {
                        Validation(int.Parse(id));
                    }
                    else
                    {

                        string logpath = HttpContext.Current.Server.MapPath("~/logs/log.txt");
                        ValidationThread validation = new ValidationThread(logpath, constr);
                        validation.Validation();
                        //ValidationThread validation = new ValidationThread(logpath, constr);
                        //Thread thread = new Thread(new ThreadStart(validation.Validation));
                        //threadid = thread.ManagedThreadId;
                        //#region Insert Thread Info
                        //MySqlOdbc mydb = null;
                        //try
                        //{
                        //    mydb = new MySqlOdbc(constr);
                        //    string sql = "INSERT INTO threads VALUES(" + threadid + ", 'Batch Validation', " + MyFuncs.FormatDateStr(DateTime.Now)
                        //        + ", " + MyFuncs.FormatTimeStr(DateTime.Now) + ", 0)";
                        //    mydb.NonQuery(sql);
                        //}
                        //catch (Exception ex)
                        //{
                        //    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                        //    LabelAlertBoard.Text = ex.ToString();
                        //}
                        //finally
                        //{
                        //    if (mydb != null) mydb.Close();
                        //}
                        //#endregion
                        //thread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);
            //XGridView.AddHead(UltraWebGrid1);
            if (!Page.IsPostBack)
            {
                #region Load WebGrid
                MySqlOdbc mydb = null;
                try
                {

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
                #region Load WebCombo
                MySqlOdbc mydb2 = null;
                MySqlOdbc mydb3 = null;
                try
                {
                    string sql2 = "SELECT account_id, account_code FROM accounts";
                    mydb2 = new MySqlOdbc(constr);
                    WebComboAccount.DataSource = mydb2.Reader(sql2);
                    WebComboAccount.DataValueField = "account_id";
                    WebComboAccount.DataTextField = "account_code";
                    WebComboAccount.DataBind();

                    string sql3 = "SELECT number_id, number_code FROM numbers";
                    mydb3 = new MySqlOdbc(constr);
                    WebComboNumber.DataSource = mydb3.Reader(sql3);
                    WebComboNumber.DataValueField = "number_id";
                    WebComboNumber.DataTextField = "number_code";
                    WebComboNumber.DataBind();
                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
                finally
                {
                    if (mydb2 != null) mydb2.Close();
                    if (mydb3 != null) mydb3.Close();
                }
                #endregion
                #region If Background Threading
                MySqlOdbc mydb4 = null;
                try
                {
                    int threadcount = 0;
                    mydb4 = new MySqlOdbc(constr);
                    string sql = "SELECT COUNT(*) FROM threads WHERE thread_name='Batch Validation'";
                    OdbcDataReader dr3 = mydb4.Reader(sql);
                    if (dr3.Read())
                        threadcount = Convert.ToInt32(dr3[0]);
                    if (threadcount != 0)
                        this.RegisterStartupScript("ImageButtonValidate_Click", "<script>ImageButtonValidate_Click();</script>");
                    else
                    {
                        sql = "SELECT COUNT(*) FROM threads WHERE thread_name='Batch Submit'";
                        OdbcDataReader dr4 = mydb4.Reader(sql);
                        if (dr4.Read())
                            threadcount = Convert.ToInt32(dr4[0]);
                        if (threadcount != 0)
                            this.RegisterStartupScript("ImageButtonSubmit_Click", "<script>ImageButtonSubmit_Click();</script>");
                    }
                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
                finally
                {
                    if (mydb4 != null) mydb4.Close();
                }
                #endregion
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

        protected void ImageButtonDelete_Click(object sender, ImageClickEventArgs e)
        {
            string batch_id = ((ImageButton)sender).Attributes["UID"].ToString();
            MySqlOdbc mydb = null;
            try
            {
                string sql = "DELETE FROM call_batch WHERE call_batch_id=" + batch_id;
                mydb = new MySqlOdbc(constr);
                mydb.NonQuery(sql);
                Response.BufferOutput = true;
                Response.Redirect("~/callvalidation.aspx");
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

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            #region Format Variables
            string call_batch_account_id = "null";
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            else return;
            if (WebComboAccount.SelectedIndex > -1) call_batch_account_id = WebComboAccount.SelectedValue;
            string call_batch_number_id = "null";
            if (WebComboNumber.SelectedIndex > -1) call_batch_number_id = WebComboNumber.SelectedValue;
            string call_batch_from_number = TextBoxFrom.Text.Replace("'", "\\'");
            string call_batch_to_number = TextBoxTo.Text.Replace("'", "\\'");
            string call_batch_description = TextBoxDescription.Text.Replace("'", "\\'");
            string call_batch_status = TextBoxStatus.Text.Replace("'", "\\'");
            string call_batch_terminated = TextBoxTerminated.Text.Replace("'", "\\'");
            string call_batch_date = MyFuncs.FormatDateStr(WebDateTimeEditDate.Text);
            string call_batch_time = MyFuncs.FormatTimeStr(WebDateTimeEditTime.Text);
            string call_batch_datetime = MyFuncs.FormatDateTimeStr(WebDateTimeEditDateTime.Text);
            string call_batch_length = MyFuncs.FormatInt(WebNumericEditLenghth.Text);
            string call_batch_cost = MyFuncs.FormatDecimal(WebCurrencyEditCost.Text);
            string call_batch_type = TextBoxType.Text.Replace("'", "\\'");
            string call_batch_subtype = TextBoxSubType.Text.Replace("'", "\\'");
            #endregion
            #region Add
            MySqlOdbc mydb = null;
            try
            {
                string sql = "INSERT INTO call_batch VALUES(null, " + project_id + "," + call_batch_account_id + ", " + call_batch_number_id
                    + ", '" + call_batch_from_number + "', '" + call_batch_to_number + "', '" + call_batch_description
                    + "', '" + call_batch_status + "', '" + call_batch_terminated + "', " + call_batch_date
                    + ", " + call_batch_time + ", " + call_batch_datetime + ", " + call_batch_length
                    + ", " + call_batch_cost + ", '" + call_batch_type + "', '" + call_batch_subtype + "', 0)";
                mydb = new MySqlOdbc(constr);
                mydb.NonQuery(sql);
                Response.BufferOutput = true;
                Response.Redirect("~/callvalidation.aspx");
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

        protected void ImageButtonCopy_Click(object sender, ImageClickEventArgs e)
        {

        }



        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {

        }

        protected void ImageButtonEdit_Click(object sender, ImageClickEventArgs e)
        {
            string batch_id = ((ImageButton)sender).Attributes["UID"].ToString();

            #region Format Form
            ButtonAdd.Visible = false;
            ButtonUpdate.Visible = true;
            LiteralTitle.Text = "Edit ID:";

            LiteralID.Text = batch_id;
            #endregion
            #region Retrieve
            MySqlOdbc mydb = null;
            try
            {
                string sql = "SELECT * FROM call_batch WHERE call_batch_id=" + batch_id;
                mydb = new MySqlOdbc(constr);
                OdbcDataReader dr = mydb.Reader(sql);
                if (dr.Read())
                {
                    if (dr["call_batch_account_id"] != DBNull.Value)
                        WebComboAccount.SelectedValue = dr["call_batch_account_id"].ToString();
                    if (dr["call_batch_number_id"] != DBNull.Value)
                        WebComboNumber.SelectedValue = dr["call_batch_number_id"].ToString();
                    TextBoxFrom.Text = dr["call_batch_from_number"].ToString();
                    TextBoxTo.Text = dr["call_batch_to_number"].ToString();
                    TextBoxDescription.Text = dr["call_batch_description"].ToString();
                    TextBoxStatus.Text = dr["call_batch_status"].ToString();
                    TextBoxTerminated.Text = dr["call_batch_terminated"].ToString();
                    if (dr["call_batch_date"] != DBNull.Value) WebDateTimeEditDate.Text = dr["call_batch_date"].ToString();
                    if (dr["call_batch_time"] != DBNull.Value) WebDateTimeEditTime.Text = dr["call_batch_time"].ToString();
                    if (dr["call_batch_datetime"] != DBNull.Value) WebDateTimeEditDateTime.Text = dr["call_batch_datetime"].ToString();
                    if (dr["call_batch_length"] != DBNull.Value) WebNumericEditLenghth.Text = dr["call_batch_length"].ToString();
                    if (dr["call_batch_cost"] != DBNull.Value) WebCurrencyEditCost.Text = dr["call_batch_cost"].ToString();
                    TextBoxType.Text = dr["call_batch_type"].ToString();
                    TextBoxSubType.Text = dr["call_batch_subtype"].ToString();
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
            #endregion

        }



        protected void ImageButtonRefresh_Click(object sender, ImageClickEventArgs e)
        {
            if ((Request.QueryString["supplier"] != null) && (Request.QueryString["projectid"] != null))
            {
                supplier_id = Server.UrlDecode(Request.QueryString["supplier"]);
                project_id = Server.UrlDecode(Request.QueryString["projectid"]);
                Response.BufferOutput = true;
                Response.Redirect("~/callvalidation.aspx?supplier=" + Server.UrlEncode(supplier_id) + "&projectid=" + Server.UrlEncode(project_id));
            }
        }

        protected void ImageButtonSubmit_Click(object sender, ImageClickEventArgs e)
        {
            //this.RegisterStartupScript("ImageButtonSubmit_Click", "<script>ImageButtonSubmit_Click();</script>");
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (Request.QueryString["supplier"] != null) supplier_id = Server.UrlDecode(Request.QueryString["supplier"]);
            string logpath = HttpContext.Current.Server.MapPath("~/logs/log.txt");
            SubmitThread submit = new SubmitThread(project_id, supplier_id, logpath, constr);
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

        }

        private void Validation(int id)
        {
            #region Validation
            MySqlOdbc mydb = null;
            try
            {
                string sql = "UPDATE call_batch SET call_batch_validated=1 WHERE call_batch_id=" + id;
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
            #endregion
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



        protected void ImageButtonClose_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (project_id != "")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/projectdetails.aspx?projectid=" + Server.UrlEncode(project_id));
            }
        }

        protected void ImageButtonValidate0_Click(object sender, ImageClickEventArgs e)
        {

            //this.RegisterStartupScript("ImageButtonValidate_Click", "<script>ImageButtonValidate_Click();</script>");
            string logpath = HttpContext.Current.Server.MapPath("~/logs/log.txt");
            ValidationThread validation = new ValidationThread(logpath, constr);
            Thread thread = new Thread(new ThreadStart(validation.Validation));
            threadid = thread.ManagedThreadId;
            #region Insert Thread Info
            MySqlOdbc mydb = null;
            try
            {
                mydb = new MySqlOdbc(constr);
                string sql = "INSERT INTO threads VALUES(" + threadid + ", 'Batch Validation', " + MyFuncs.FormatDateStr(DateTime.Now)
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

        }
        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                string sql = "SELECT call_batch_id AS `ID`, call_batch_project_id AS `Project ID`, call_batch_account_id AS `Account`"
                                             + ", call_batch_number_id AS `Number`, call_batch_from_number AS `From`, call_batch_to_number AS `To`"
                                             + ", call_batch_description AS `Description`, call_batch_status AS `Status`, call_batch_terminated AS `Terminated`"
                                             + ",   call_batch_date AS `Date`, call_batch_time AS `Time`, call_batch_datetime AS `DateTime`, call_batch_length AS `Length`"
                                             + ", call_batch_cost AS `Cost`, call_batch_type AS `Type`, call_batch_subtype AS `SubType`, call_batch_validated AS `Valid`";


                //DataTable dt = new MySqlOdbc(AdFunction.conn).ReturnTable(sql, "temp");
                string sqlselectstr = "select `ID`, `Project ID`, `Account`, `Number`, `From`, `To`, `Description`, `Status`,`Terminated`,`Date`,`Time`,`DateTime`, `Length`, `Cost`, `Type`, `SubType`, `Valid` from(  " + sql;

                string sqlfromstr = "FROM `call_batch` WHERE call_batch_validated=0";
                JQGrid jqgridObj = new JQGrid(postdata, AdFunction.conn, sqlfromstr, sqlselectstr);
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