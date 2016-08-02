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


    public class Contract
    {
        #region Variables Declaration
        private string constr = AdFunction.conn;
        private int? contract_id;
        private int? contract_client_id;
        private string contract_code;
        private string contract_name;
        private int? contract_type_id;
        private int? contract_freq_id;
        private int? contract_number;
        private int? contract_lc;
        private int? contract_nc;
        private int? contract_ic;
        private int? contract_mc;
        private int? contract_dc;
        private decimal? contract_setup_fee;
        private decimal? contract_charge;
        private DateTime contract_start;
        private DateTime contract_end;
        private bool contract_pending;
        private bool contract_locked;
        private bool contract_ended;
        private bool contract_auto_renew;
        #endregion
        #region Constructor
        public Contract()
        {
            #region Initialise Variables
            contract_id = null;
            contract_client_id = null;
            contract_code = "";
            contract_name = "";
            contract_type_id = null;
            contract_freq_id = null;
            contract_number = null;
            contract_lc = null;
            contract_nc = null;
            contract_ic = null;
            contract_mc = null;
            contract_dc = null;
            contract_setup_fee = null;
            contract_charge = null;
            contract_start = DateTime.MinValue;
            contract_end = DateTime.MinValue;
            contract_pending = false;
            contract_locked = false;
            contract_ended = false;
            contract_auto_renew = false;
            #endregion
        }
        public Contract(int contract_id)
        {
            #region Initialise Variables
            this.contract_id = contract_id;
            contract_client_id = null;
            contract_code = "";
            contract_name = "";
            contract_type_id = null;
            contract_freq_id = null;
            contract_number = null;
            contract_lc = null;
            contract_nc = null;
            contract_ic = null;
            contract_mc = null;
            contract_dc = null;
            contract_setup_fee = null;
            contract_charge = null;
            contract_start = DateTime.MinValue;
            contract_end = DateTime.MinValue;
            contract_pending = false;
            contract_locked = false;
            contract_ended = false;
            contract_auto_renew = false;
            #endregion
            MySqlOdbc mydb = null;
            try
            {
                mydb = new MySqlOdbc(constr);
                string sql = "SELECT * FROM contracts WHERE contract_id=" + contract_id;
                OdbcDataReader dr = mydb.Reader(sql);
                if (dr.Read())
                {
                    if (dr["contract_client_id"] != DBNull.Value) contract_client_id = Convert.ToInt32(dr["contract_client_id"]);
                    if (dr["contract_code"] != DBNull.Value) contract_client_id = Convert.ToInt32(dr["contract_code"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mydb != null) mydb.Close();
            }
        }
        #endregion
        #region Set
        public void SetContractClientID(int contract_client_id)
        {
            this.contract_client_id = contract_client_id;
        }
        public void SetContractCode(string contract_code)
        {
            this.contract_code = contract_code;
        }
        public void SetContractName(string contract_name)
        {
            this.contract_name = contract_name;
        }
        public void SetContractTypeID(int contract_type_id)
        {
            this.contract_type_id = contract_type_id;
        }
        public void SetContractFreqID(int contract_freq_id)
        {
            this.contract_freq_id = contract_freq_id;
        }
        
        #endregion
    }
