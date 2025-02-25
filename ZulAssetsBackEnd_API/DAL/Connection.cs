
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace ZulAssetsBackEnd_API.DAL
{
    public class Connection
    {
        string constr = null;
        public Connection()
        {
            constr = ConfigSettings.conStr1;
        }
        public Connection(int i)
        {
            constr = ConfigSettings.ConfigSettings_id(i);
        }
        public SqlConnection GetDataBaseConnection()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection();
                sqlcon.ConnectionString = constr;
                sqlcon.Open();
                return sqlcon;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
