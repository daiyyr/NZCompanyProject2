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
    public partial class numberlist : System.Web.UI.Page, IPostBackEventHandler
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
            if (args[0] == "Delete")
            {
                MySqlOdbc mydb = null;
                try
                {
                    string sql = "DELETE FROM rate_cards WHERE rate_cards_id=" + client_id;
                    mydb = new MySqlOdbc(constr);
                    mydb.NonQuery(sql);

                    Response.BufferOutput = true;
                    Response.Redirect("~/numberlist.aspx");

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
        public static void DataGridCommsSave(string rowValue)
        {
            try
            {
                MySqlOdbc o = new MySqlOdbc(AdFunction.conn);
                Hashtable rowObj = (Hashtable)JSON.JsonDecode(rowValue);
                if(rowObj["rate_cards_number"].ToString().Equals(""))
                {
                    rowObj["rate_cards_number"] = "0";
                }
                if(rowObj["rate_cards_id"].ToString().Equals(""))
                {
                    string sql="insert into rate_cards values(null,'"+rowObj["rate_cards_code"].ToString()+"','"+rowObj["rate_cards_description"].ToString()+"','"+rowObj["rate_cards_number"].ToString()+"',"+rowObj["rate_cards_rate"].ToString()+","+rowObj["rate_cards_discount"].ToString()+")";
                    o.ExecuteScalar(sql);
                }
                else
                {
                    string sql = "update rate_cards set rate_cards_code='" + rowObj["rate_cards_code"].ToString() + "', rate_cards_description='" + rowObj["rate_cards_description"].ToString() + "', rate_cards_number='" + rowObj["rate_cards_number"].ToString() + "', rate_cards_rate=" + rowObj["rate_cards_rate"] + ", rate_cards_discount=" + rowObj["rate_cards_discount"] + " where rate_cards_id=" + rowObj["rate_cards_id"].ToString();
                                 o.ExecuteScalar(sql);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [System.Web.Services.WebMethod]
        public static string BindJQGrid(string postdata)
        {
            try
            {
                string sql = "SELECT * from rate_cards";
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