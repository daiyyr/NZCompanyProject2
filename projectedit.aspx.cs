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
using System.Data.Odbc;
using System.Text;

namespace telco
{
    public partial class projectedit : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string project_id = "";
        string mode = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (Request.QueryString["mode"] != null) mode = Server.UrlDecode(Request.QueryString["mode"]);
            if (!Page.IsPostBack)
            {
                #region Javascript Setup
                Control[] wc = { WebComboClient, WebComboBillClient };
                RenderJSArrayWithCliendIds(wc);
                #endregion
                if (mode == "add")  
                {
                    #region Load Page
                    LiteralMode.Text = "Add Project";
                    LabelID.Text = "";

                    TextBoxTitle.Text = "";
                    TextBoxDescription.Text = "";
                    MySqlOdbc mydb = null;
                    try
                    {
                        mydb = new MySqlOdbc(constr);
                        string sql = "SELECT client_id AS ID, client_code AS Code, client_name AS Name FROM clients order by name";
                        WebComboClient.DataSource = mydb.Reader(sql);
                        WebComboClient.DataValueField = "ID";
                        WebComboClient.DataTextField = "Name";
                        WebComboClient.DataBind();

                        WebComboBillClient.DataSource = mydb.Reader(sql);
                        WebComboBillClient.DataValueField = "ID";
                        WebComboBillClient.DataTextField = "Name";
                        WebComboBillClient.DataBind();
                        WebComboBillClient.Items.Insert(0, new ListItem("Null", "Null"));
                        sql = "SELECT pro_category_id AS ID, pro_category_name AS Name FROM pro_categories order by name";
                        WebComboCategory.DataSource = mydb.Reader(sql);
                        WebComboCategory.DataValueField = "ID";
                        WebComboCategory.DataTextField = "Name";
                        WebComboCategory.DataBind();

                        sql = "SELECT person_id AS ID, person_name AS Name FROM persons order by name";
                        WebComboManager.DataSource = mydb.Reader(sql);
                        WebComboManager.DataValueField = "ID";
                        WebComboManager.DataTextField = "Name";
                        WebComboManager.DataBind();

                        sql = "SELECT project_id AS ID, project_title AS Name FROM projects order by name";
                        OdbcDataReader dr = mydb.Reader(sql);
                        if (dr.HasRows)
                        {
                            WebComboParent.DataSource = dr;
                            WebComboParent.DataValueField = "ID";
                            WebComboParent.DataTextField = "Name";
                            WebComboParent.DataBind();
                        }

                        DataTable dt = new DataTable();
                        dt.Columns.Add("ID", Type.GetType("System.String"));
                        for (int i = 0; i < 10; i++)
                        {
                            DataRow nr = dt.NewRow();
                            nr[0] = i.ToString();
                            dt.Rows.Add(nr);
                        }
                        WebComboPriority.DataSource = dt;
                        WebComboPriority.DataValueField = "ID";
                        WebComboPriority.DataBind();

                        WebComboPriority.SelectedValue = "5";

                        sql = "SELECT pro_status_id AS ID, pro_status_name AS Name FROM pro_statuses order by name";
                        WebComboStatus.DataSource = mydb.Reader(sql);
                        WebComboStatus.DataValueField = "ID";
                        WebComboStatus.DataTextField = "Name";
                        WebComboStatus.DataBind();

                        WebComboStatus.SelectedValue = ("Open");
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
                else if (mode == "edit" && project_id != "")
                {
                    #region Load Page
                    LiteralMode.Text = "Edit Project";
                    LabelID.Text = project_id;

                    TextBoxTitle.Text = "";
                    TextBoxDescription.Text = "";
                    MySqlOdbc mydb = null;
                    try
                    {
                        mydb = new MySqlOdbc(constr);
                        string sql = "SELECT client_id AS ID, client_code AS Code, client_name AS Name FROM clients order by name";
                        WebComboClient.DataSource = mydb.Reader(sql);
                        WebComboClient.DataValueField = "ID";
                        WebComboClient.DataTextField = "Name";
                        WebComboClient.DataBind();

                        WebComboBillClient.DataSource = mydb.Reader(sql);
                        WebComboBillClient.DataValueField = "ID";
                        WebComboBillClient.DataTextField = "Name";
                        WebComboBillClient.DataBind();

                        WebComboBillClient.Items.Insert(0, new ListItem("Null", "Null"));

                        sql = "SELECT pro_category_id AS ID, pro_category_name AS Name FROM pro_categories order by name";
                        WebComboCategory.DataSource = mydb.Reader(sql);
                        WebComboCategory.DataValueField = "ID";
                        WebComboCategory.DataTextField = "Name";
                        WebComboCategory.DataBind();

                        sql = "SELECT person_id AS ID, person_name AS Name FROM persons order by name";
                        WebComboManager.DataSource = mydb.Reader(sql);
                        WebComboManager.DataValueField = "ID";
                        WebComboManager.DataTextField = "Name";
                        WebComboManager.DataBind();

                        sql = "SELECT project_id AS ID, project_title AS Name FROM projects order by name";
                        OdbcDataReader dr = mydb.Reader(sql);
                        if (dr.HasRows)
                        {
                            WebComboParent.DataSource = dr;
                            WebComboParent.DataValueField = "ID";
                            WebComboParent.DataTextField = "Name";
                            WebComboParent.DataBind();
                        }

                        DataTable dt = new DataTable();
                        dt.Columns.Add("ID", Type.GetType("System.String"));
                        for (int i = 0; i < 10; i++)
                        {
                            DataRow nr = dt.NewRow();
                            nr[0] = i.ToString();
                            dt.Rows.Add(nr);
                        }
                        WebComboPriority.DataSource = dt;
                        WebComboPriority.DataValueField = "ID";
                        WebComboPriority.DataBind();

                        sql = "SELECT pro_status_id AS ID, pro_status_name AS Name FROM pro_statuses order by name";
                        WebComboStatus.DataSource = mydb.Reader(sql);
                        WebComboStatus.DataValueField = "ID";
                        WebComboStatus.DataTextField = "Name";
                        WebComboStatus.DataBind();

                        sql = "SELECT * FROM projects WHERE project_id=" + project_id;
                        OdbcDataReader dr2 = mydb.Reader(sql);
                        if (dr2.Read())
                        {
                            if (dr2["project_title"] != DBNull.Value) TextBoxTitle.Text = dr2["project_title"].ToString();
                            if (dr2["project_client_id"] != DBNull.Value) WebComboClient.SelectedValue = dr2["project_client_id"].ToString();
                            if (dr2["project_category_id"] != DBNull.Value) WebComboCategory.SelectedValue = dr2["project_category_id"].ToString();
                            if (dr2["project_status_id"] != DBNull.Value) WebComboStatus.SelectedValue = dr2["project_status_id"].ToString();
                            if (dr2["project_priority"] != DBNull.Value) WebComboPriority.SelectedValue = dr2["project_priority"].ToString().ToString();
                            if (dr2["project_bill_client_id"] != DBNull.Value) WebComboBillClient.SelectedValue = dr2["project_bill_client_id"].ToString();
                            if (dr2["project_parent_id"] != DBNull.Value) WebComboParent.SelectedValue = dr2["project_parent_id"].ToString();
                            if (dr2["project_description"] != DBNull.Value) TextBoxDescription.Text = dr2["project_description"].ToString();
                            if (dr2["project_manager_id"] != DBNull.Value) WebComboManager.SelectedValue = dr2["project_manager_id"].ToString();
                            if (dr2["project_start"] != DBNull.Value) WebDateChooserStart.Text = dr2["project_start"].ToString();
                            if (dr2["project_est_end"] != DBNull.Value) WebDateChooserEnd.Text = dr2["project_est_end"].ToString();
                            if (dr2["project_deadline"] != DBNull.Value) WebDateChooserDeadline.Text = dr2["project_deadline"].ToString();
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

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (Request.QueryString["mode"] != null) mode = Server.UrlDecode(Request.QueryString["mode"]);
            #region Format Variables
            string project_title = MyFuncs.FormatStr(TextBoxTitle.Text);
            string project_client_id = "null";
            if (WebComboClient.SelectedIndex > -1) project_client_id = WebComboClient.SelectedValue;
            string project_category_id = "null";
            if (WebComboCategory.SelectedIndex > -1) project_category_id = WebComboCategory.SelectedValue;
            string project_status_id = "null";
            if (WebComboStatus.SelectedIndex > -1) project_status_id = WebComboStatus.SelectedValue;
            string project_priority = "null";
            if (WebComboPriority.SelectedIndex > -1) project_priority = WebComboPriority.SelectedValue;
            string project_bill_client_id = "null";
            if (WebComboBillClient.SelectedIndex > -1) project_bill_client_id = WebComboBillClient.SelectedValue;
            string project_parent_id = "null";
            if (WebComboParent.SelectedIndex > -1) project_parent_id = WebComboParent.SelectedValue;
            string project_description = MyFuncs.FormatStr(TextBoxDescription.Text);
            string project_manager_id = "null";
            if (WebComboManager.SelectedIndex > -1) project_manager_id = WebComboManager.SelectedValue;
            string project_start = MyFuncs.FormatDateStr(WebDateChooserStart.Text);
            string project_est_end = MyFuncs.FormatDateStr(WebDateChooserEnd.Text);
            string project_deadline = MyFuncs.FormatDateStr(WebDateChooserDeadline.Text);

            #endregion
            if (mode == "add")
            {
                #region Submit
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    string sql = "INSERT INTO projects VALUES(null, '" + project_title + "', " + project_client_id
                        + ", " + project_category_id + ", " + project_status_id + ", " + project_priority
                        + ", " + project_bill_client_id + ", " + project_parent_id + ", '" + project_description
                        + "', " + project_manager_id + ", " + project_start + ", " + project_est_end
                        + ", " + project_deadline + ", " + MyFuncs.FormatDateStr(DateTime.Today) + ", 0, 0)";
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
                #region Redirect
                Response.BufferOutput = true;
                Response.Redirect("~/projectlist2.aspx");
                #endregion
            }
            else if (mode == "edit" && project_id != "")
            {
                #region Submit
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    string sql = "UPDATE projects SET project_title='" + project_title + "', project_client_id=" + project_client_id
                        + ", project_category_id=" + project_category_id + ", project_status_id=" + project_status_id
                        + ", project_priority=" + project_priority + ", project_bill_client_id=" + project_bill_client_id
                        + ", project_parent_id=" + project_parent_id + ", project_description='" + project_description
                        + "', project_manager_id=" + project_manager_id + ", project_start=" + project_start
                        + ", project_est_end=" + project_est_end + ", project_deadline=" + project_deadline
                        + " WHERE project_id=" + project_id;
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
                #region Redirect
                Response.BufferOutput = true;
                Response.Redirect("~/projectlist2.aspx");
                #endregion
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            #region Redirect
            Response.BufferOutput = true;
            Response.Redirect("~/projectlist2.aspx");
            #endregion
        }

        protected void CustomValidatorClient_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboClient.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboCategory.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorStatus_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboStatus.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }
    }
}