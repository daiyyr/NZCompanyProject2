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
using Sapp.Common;
using Sapp.JQuery;

namespace telco
{
    public partial class products : System.Web.UI.Page
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
            string product_id = args[1];
            if (args[0] == "ImageButtonDelete")
            {
                MySqlOdbc mydb = null;
                try
                {
                    mydb = new MySqlOdbc(constr);
                    string sql = "DELETE FROM products WHERE product_id=" + product_id;
                    mydb.NonQuery(sql);
                    Response.BufferOutput = true;
                    Response.Redirect("~/products.aspx");
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
            MySqlOdbc mydb = null;
            try
            {
                mydb = new MySqlOdbc(constr);
                string sql = "INSERT INTO products VALUES(null, '" + MyFuncs.FormatStr(TextBoxCode.Text) + "', '" + MyFuncs.FormatStr(TextBoxName.Text) + "', " + Convert.ToDecimal(WebCurrencyEditPrice.Text) + ")";
                mydb.NonQuery(sql);
                Response.BufferOutput = true;
                Response.Redirect("~/products.aspx");
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




        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                MySqlOdbc mydb = new MySqlOdbc(AdFunction.conn);
                DataTable dt = mydb.ReturnTable("SELECT product_id AS ID, product_code AS Code, product_name AS Name, product_price AS Price FROM products", "temp");
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

    }
}