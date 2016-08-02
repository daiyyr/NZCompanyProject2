using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
namespace telco
{
    public partial class login : System.Web.UI.Page
    {
        string constr = AdFunction.conn;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            MySqlOdbc mydb = null;
            try
            {
                mydb = new MySqlOdbc(constr);
                string sql = "SELECT `user_id` FROM `users` WHERE `user_login`='" + MyFuncs.FormatStr(TextBoxLogin.Text)
                    + "' AND `user_password`='" + MyFuncs.FormatStr(TextBoxPassword.Text) + "'";
                OdbcDataReader dr = mydb.Reader(sql);
                if (dr.Read())
                    FormsAuthentication.RedirectFromLoginPage(TextBoxLogin.Text, true);
                else
                    LabelAlertBoard.Text = "Invalid Login";
            }
            catch (Exception ex)
            {
                LabelAlertBoard.Text = ex.ToString();
            }
            finally
            {
                if (mydb != null) mydb.Close();
            }
        }
    }
}