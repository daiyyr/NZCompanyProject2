using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using System.Text;


public static class MyLog
{
    public static void InsertLog(string log)
    {
        StreamWriter sw = null;
        try
        {
            string filename = HttpContext.Current.Server.MapPath("~/logs/log.txt");
            sw = new StreamWriter(filename, true, Encoding.Default);
            log = DateTime.Now.ToString() + ": " + log;
            sw.WriteLine(log);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sw != null) sw.Close();
        }
    }

    public static void InsertLog(string log, string logpath)
    {
        StreamWriter sw = null;
        try
        {
            string filename = logpath;
            sw = new StreamWriter(filename, true, Encoding.Default);
            log = DateTime.Now.ToString() + ": " + log;
            sw.WriteLine(log);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sw != null) sw.Close();
        }
    }
}
