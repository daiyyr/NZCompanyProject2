using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Web;
using System.Data.Odbc;
public class AdFunction
{
    public static string conn = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;


    #region QueryString

    public static string GetQueryString(string QSname)
    {
        string result = "";
        if (HttpContext.Current.Request.QueryString[QSname] != null)
            result = HttpContext.Current.Request.QueryString[QSname].ToString();
        return result;
    }

    #endregion

    #region Session

    public static string GetSessionString(string name)
    {
        string result = "";
        if (HttpContext.Current.Session[name] != null)
            result = HttpContext.Current.Session[name].ToString();
        return result;
    }
    #endregion




    #region decimal
    public static decimal Rounded(string v, int places = 2)
    {
        try
        {
            decimal d = 0;
            decimal.TryParse(v, out d);
            string temp = d.ToString("f" + places.ToString());
            d = decimal.Parse(temp);
            return d;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static decimal Rounded(decimal v, int places = 2)
    {
        return decimal.Parse(v.ToString("f" + places.ToString()));
    }
    #endregion

    public static string GetSelector(string sql)
    {
        MySqlOdbc o = new MySqlOdbc(AdFunction.conn);
        DataTable dt = o.ReturnTable(sql, "temp");
        string selectsql = "SELECT ";
        foreach (DataColumn dc in dt.Columns)
        {
            selectsql += dc.ColumnName + ",";
        }
        selectsql = selectsql.Remove(selectsql.Length - 1, 1);
        selectsql += " FROM (SELECT *";
        return selectsql;
    }

    public static string GetSelector(DataTable dt)
    {
        MySqlOdbc o = new MySqlOdbc(AdFunction.conn);
        string selectsql = "SELECT ";
        foreach (DataColumn dc in dt.Columns)
        {
            selectsql += "`" + dc.ColumnName + "`" + ",";
        }
        selectsql = selectsql.Remove(selectsql.Length - 1, 1);
        selectsql += " FROM (SELECT *";
        return selectsql;
    }

    public static string GetNextNumber(string PREFIX_name, string PILOT_name, string DIGIT_name, string table, string column)
    {

        try
        {
            MySqlOdbc mydb = new MySqlOdbc(AdFunction.conn);
            string INVOICEPREFIX = "";
            string INVOICEPILOT = "";
            string INVOICEDIGIT = "";
            DataTable prefix = mydb.ReturnTable("select * from system where code = '" + PREFIX_name + "'", "1");
            DataTable pilot = mydb.ReturnTable("select * from system where code = '" + PILOT_name + "'", "1");
            DataTable digitdt = mydb.ReturnTable("select * from system where code = '" + DIGIT_name + "'", "1");

            INVOICEPREFIX = prefix.Rows[0]["value"].ToString();

            INVOICEPILOT = pilot.Rows[0]["value"].ToString();

            INVOICEDIGIT = digitdt.Rows[0]["value"].ToString(); ;
            int next_number = Convert.ToInt32(INVOICEPILOT) + 1;
            int digit = Convert.ToInt32(INVOICEDIGIT);
            string inv_number = next_number.ToString();
            if (inv_number.Length < digit)
            {
                for (int i = 0; i < (digit - inv_number.Length); i++)
                {
                    INVOICEPREFIX += "0";
                }
            }
            inv_number = INVOICEPREFIX + inv_number;
            mydb.ExecuteScalar("update system set value='" + next_number.ToString() + "' where id=" + pilot.Rows[0][0].ToString());
            if (CheckExist(table, column, inv_number))
                return GetNextNumber(PREFIX_name, PILOT_name, DIGIT_name, table, column);
            else
                return inv_number;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public static bool CheckExist(string tablename, string column, string value)
    {

        try
        {
            MySqlOdbc mydb = new MySqlOdbc(AdFunction.conn);
            bool ret = false;
            string sql = "SELECT * FROM `" + tablename + "` WHERE `" + column + "`='" + value + "'";
            DataTable dt = mydb.ReturnTable(sql, "temp");
            if (dt.Rows.Count > 0)
                ret = true;
            return ret;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

}

