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
namespace telco
{
    public partial class taskadd : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        string project_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    string sql = "SELECT person_id AS ID, person_name AS Name FROM persons";
                    OdbcDataReader dr = mydb.Reader(sql);
                    if (dr.HasRows)
                    {
                        WebComboPerson.DataSource = dr;
                        WebComboPerson.DataValueField = "ID";
                        WebComboPerson.DataTextField = "Name";
                        WebComboPerson.DataBind();
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
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            if (Request.QueryString["projectid"] != null) project_id = Server.UrlDecode(Request.QueryString["projectid"]);
            if (project_id != "")
            {
                #region Submit
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    #region Format Variables
                    string task_person_id = "null";
                    if (WebComboPerson.SelectedIndex > -1) task_person_id = WebComboPerson.SelectedValue;
                    string task_start_date = MyFuncs.FormatDateStr(WebDateChooserStartDate.Text);
                    string task_end_date = MyFuncs.FormatDateStr(WebDateChooserEndDate.Text);
                    string task_duration = MyFuncs.FormatDecimal(WebNumericEditDuration.Text);
                    string task_billable = "1";
                    if (CheckBoxBillable.Checked) task_billable = "1";
                    else task_billable = "0";
                    string task_description = TextBoxDescription.Text.Replace("'", "\\'");
                    #endregion
                    string sql = "INSERT INTO tasks VALUES(null, " + project_id + ", " + task_person_id + ", " + task_start_date + ", " + task_end_date
                        + ", " + task_duration + ", " + task_billable + ", '" + task_description + "', 0, null)";
                    mydb.NonQuery(sql);
                    #region Redirect
                    Response.BufferOutput = true;
                    Response.Redirect("~/projectdetails.aspx?projectid=" + Server.UrlEncode(project_id));
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
                #endregion
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            #region Redirect
            Response.BufferOutput = true;
            Response.Redirect("~/projectdetails.aspx?projectid=" + Request.QueryString["projectid"]);
            #endregion
        }

        protected void CustomValidatorPerson_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebComboPerson.SelectedIndex > -1) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorStartDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebDateChooserStartDate.Text != null) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorEndDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebDateChooserEndDate.Text != null) args.IsValid = true;
            else args.IsValid = false;
        }

        protected void CustomValidatorDuration_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (WebNumericEditDuration.Text != null) args.IsValid = true;
            else args.IsValid = false;
        }
    }
}