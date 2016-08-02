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
using Sapp.JQuery;
using Sapp.Common;
namespace telco
{
    public partial class clientslist : System.Web.UI.Page, IPostBackEventHandler
    {
        string constr = AdFunction.conn;


        protected void Page_Load(object sender, EventArgs e)
        {
            Control[] wc = { jqGridTable };
            JSUtils.RenderJSArrayWithCliendIds(Page, wc);


        }


        public void RaisePostBackEvent(string eventArgument)
        {

            string[] args = eventArgument.Split('|');
            string client_id = args[1];
            if (args[0] == "ImageButtonDelete")
            {
                MySqlOdbc mydb = null;
                try
                {
                    string sql = "DELETE FROM clients WHERE client_id=" + client_id;
                    mydb = new MySqlOdbc(constr);
                    mydb.NonQuery(sql);

                    Response.BufferOutput = true;
                    Response.Redirect("~/clientslist.aspx");

                }
                catch (Exception ex)
                {
                    Label LabelAlertBoard = (Label)Master.FindControl("LabelAlertBoard");
                    LabelAlertBoard.Text = ex.ToString();
                }
                finally
                {
                    mydb.Close();
                }
            }
            else if (args[0] == "ImageButtonDetails")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/clientdetails.aspx?clientid=" + Server.UrlEncode(client_id));
            }
            else if (args[0] == "ImageButtonEdit")
            {
                Response.BufferOutput = true;
                Response.Redirect("~/clientedit.aspx?clientid=" + Server.UrlEncode(client_id) + "&mode=edit");
            }
            else if (args[0] == "ImageButtonCall")
            {

            }
            else if (args[0] == "ImageButtonCopy")
            {
            }
        }



        protected void ImageButtonAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.BufferOutput = true;
            Response.Redirect("~/clientedit.aspx?clientid=null&mode=add");
        }




        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                string sql = "SELECT client_id AS ID, client_name AS Name, category_name AS Category"
                        + " FROM clients LEFT JOIN categories ON"
                        + " clients.client_category_id = categories.category_id ORDER BY Name";
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

    }
}