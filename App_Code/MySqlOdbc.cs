using System;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// MySqlOdbc Version 1.0.0 08/16/2010
/// </summary>


    public class MySqlOdbc
    {
        OdbcConnection con;
        OdbcCommand cmdwithparm;
        string constr;
        string error;
        bool opened;

        public MySqlOdbc(string constr)
        {
            this.constr = constr;
            con = new OdbcConnection(constr);
            opened = false;
        }

        public OdbcDataReader Reader(string sqlcommand)
        {
            OdbcCommand cmd;
            OdbcDataReader dr;
            cmd = new OdbcCommand(sqlcommand, con);
            if (!opened)
            {
                con.Open();
                opened = true;
            }
            try
            {
                dr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dr;
        }

        public bool NonQuery(string sqlcommand)
        {
            OdbcCommand cmd = new OdbcCommand(sqlcommand, con);
            if (!opened)
            {
                con.Open();
                opened = true;
            }

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ExecuteScalar(string sqlcommand)
        {
            OdbcCommand cmd = new OdbcCommand(sqlcommand, con);
            if (!opened)
            {
                con.Open();
                opened = true;
            }

            try
            {
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet FillDataSet(string sqlcommand, string tablename)
        {
            try
            {
                OdbcDataAdapter da = new OdbcDataAdapter(sqlcommand, con);
                DataSet ds = new DataSet();
                da.Fill(ds, tablename);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ReturnTable(string sqlcommand, string tablename)
        {
            try
            {
                OdbcDataAdapter da = new OdbcDataAdapter(sqlcommand, con);
                DataSet ds = new DataSet();
                da.Fill(ds, tablename);
                DataTable dt = ds.Tables[tablename];
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Close()
        {
            con.Close();
        }
    }
