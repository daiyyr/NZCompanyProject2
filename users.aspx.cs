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
using System.Text;
using System.Data.Odbc;
using Sapp.Common;
using Sapp.JQuery;

namespace telco
{
    public partial class users : System.Web.UI.Page, IPostBackEventHandler
    {
        string constr = AdFunction.conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);
            if (!Page.IsPostBack)
            {

            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {

            string[] args = eventArgument.Split('|');
            string user_id = args[1];
            if (args[0] == "ImageButtonDelete")
            {
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    string sql = "DELETE FROM users WHERE user_id=" + user_id;
                    mydb.NonQuery(sql);
                    Response.BufferOutput = true;
                    Response.Redirect("~/users.aspx");
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

                #region Format Form
                ButtonAdd.Visible = false;
                ButtonUpdate.Visible = true;
                LiteralTitle.Text = "Edit ID:";
                //user_id = UltraWebGrid1.DisplayLayout.ActiveRow.Cells[0].Text;
                LiteralID.Text = user_id;
                #endregion
                #region Retrieve
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    string sql = "SELECT * FROM users WHERE user_id=" + user_id;
                    OdbcDataReader dr = mydb.Reader(sql);
                    if (dr.Read())
                    {
                        if (dr["user_login"] != DBNull.Value) TextBoxLogin.Text = dr["user_login"].ToString();
                        if (dr["user_name"] != DBNull.Value) TextBoxName.Text = dr["user_name"].ToString();
                        if (dr["user_password"] != DBNull.Value) TextBoxPassword.Text = dr["user_password"].ToString();
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
            else if (args[0] == "ImageButtonCall")
            {

            }
            else if (args[0] == "ImageButtonCopy")
            {

                ButtonAdd.Visible = true;
                ButtonUpdate.Visible = false;
                LiteralTitle.Text = "Add New User:";
                LiteralID.Text = "";
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    string sql = "SELECT * FROM users WHERE user_id=" + user_id;
                    OdbcDataReader dr = mydb.Reader(sql);
                    if (dr.Read())
                    {
                        if (dr["user_login"] != DBNull.Value) TextBoxLogin.Text = dr["user_login"].ToString();
                        if (dr["user_name"] != DBNull.Value) TextBoxName.Text = dr["user_name"].ToString();
                        if (dr["user_password"] != DBNull.Value) TextBoxPassword.Text = dr["user_password"].ToString();
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
        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                string sql = "SELECT user_id AS ID, user_login AS Login, user_name AS Name FROM users";
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

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            #region Format Variables
            string user_login = MyFuncs.FormatStr(TextBoxLogin.Text);
            string user_name = MyFuncs.FormatStr(TextBoxName.Text);
            string user_password = MyFuncs.FormatStr(TextBoxPassword.Text);
            #endregion
            #region Add
            MySqlOdbc mydb = null;
            try
            {
                mydb = new MySqlOdbc(constr);
                string sql = "INSERT INTO users VALUES(null, '" + user_login + "', '" + user_name + "', '" + user_password + "')";
                mydb.NonQuery(sql);
                Response.BufferOutput = true;
                Response.Redirect("~/users.aspx");
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

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            #region Format Variables
            string user_id = LiteralID.Text;
            string user_login = MyFuncs.FormatStr(TextBoxLogin.Text);
            string user_name = MyFuncs.FormatStr(TextBoxName.Text);
            string user_password = MyFuncs.FormatStr(TextBoxPassword.Text);
            #endregion
            #region Update
            MySqlOdbc mydb = null;
            try
            {
                mydb = new MySqlOdbc(constr);
                string sql = "UPDATE users SET user_login='" + user_login + "', user_name='" + user_name + "', user_password='" + user_password + "'"
                    + " WHERE user_id=" + user_id;
                mydb.NonQuery(sql);
                Response.BufferOutput = true;
                Response.Redirect("~/users.aspx");
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