using System;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


    public static class MyFuncs
    {
        public static string FormatDateTimeStr(string datetime)
        {
            
            try
            {
                if (datetime == "") return "null";
                DateTime dt = DateTime.MinValue;
                if (datetime != "") dt = Convert.ToDateTime(datetime);
                if (dt == DateTime.MinValue) return "null";
                else
                {
                    string ret = "'" + dt.Year + "-" + dt.Month + "-" + dt.Day
                        + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second + "'";
                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string FormatDateTimeStr(object datetime)
        {
            try
            {
                if (datetime == null || datetime == DBNull.Value) return "null";
                DateTime dt = DateTime.MinValue;
                if (datetime != null) dt = Convert.ToDateTime(datetime);
                if (dt == DateTime.MinValue) return "null";
                else
                {
                    string ret = "'" + dt.Year + "-" + dt.Month + "-" + dt.Day
                        + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second + "'";
                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string FormatTimeStr(string time)
        {
            try
            {
                if (time == "") return "null";
                TimeSpan ts = new TimeSpan();
                if (time != "") ts = TimeSpan.Parse(time);
                if (ts.Equals(TimeSpan.Zero)) return "null";
                else
                {
                    string ret = "'" + ts.ToString() + "'";
                    return ret;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    DateTime dt = DateTime.MinValue;
                    if (time != "") dt = Convert.ToDateTime(time);
                    if (dt == DateTime.MinValue) return "null";
                    else
                    {
                        string ret = "'" + dt.Hour + ":" + dt.Minute + ":" + dt.Second + "'";
                        return ret;
                    }
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
        }

        public static string FormatTimeStr(object time)
        {
            try
            {
                if (time == null || time == DBNull.Value) return "null";
                TimeSpan ts = new TimeSpan();
                if (time != null) ts = TimeSpan.Parse(time.ToString());
                if (ts.Equals(TimeSpan.Zero)) return "null";
                else
                {
                    string ret = "'" + ts.ToString() + "'";
                    return ret;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    DateTime dt = DateTime.MinValue;
                    if (time != null) dt = Convert.ToDateTime(time);
                    if (dt == DateTime.MinValue) return "null";
                    else
                    {
                        string ret = "'" + dt.Hour + ":" + dt.Minute + ":" + dt.Second + "'";
                        return ret;
                    }
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
        }

        public static string FormatDateStr(string date)
        {
            try
            {
                if (date == "") return "null";
                DateTime dt = DateTime.MinValue;
                if (date != "") dt = Convert.ToDateTime(date.Replace(".",""));
                if (dt == DateTime.MinValue) return "null";
                else
                {
                    string ret = "'" + dt.Year + "-" + dt.Month + "-" + dt.Day + "'";
                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string FormatDateStr(object date)
        {
            try
            {
                if (date == null || date == DBNull.Value) return "null";
                DateTime dt = DateTime.MinValue;
                if (date != null) dt = Convert.ToDateTime(date);
                if (dt == DateTime.MinValue) return "null";
                else
                {
                    string ret = "'" + dt.Year + "-" + dt.Month + "-" + dt.Day + "'";
                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string FormatDateTxt(object date)
        {
            try
            {
                if (date == null || date == DBNull.Value) return "";
                DateTime dt = DateTime.MinValue;
                if (date != null) dt = Convert.ToDateTime(date);
                if (dt == DateTime.MinValue) return "null";
                else
                {
                    string ret = dt.Day + "/" + dt.Month + "/" + dt.Year;
                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string FormatStr(string str)
        {
            return str.Replace("'", "\\'");
        }

        public static string FormatStr(object str)
        {
           
            string ret = str.ToString();
            return ret.Replace("'", "\\'");
        }

        public static string FormatInt(object obj)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                {
                    return "null";
                }
                else
                {
                    int integer = Convert.ToInt32(obj);
                    return integer.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string FormatDecimal(object obj)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                {
                    return "null";
                }
                else
                {
                    decimal dec = Convert.ToDecimal(obj);
                    return dec.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
