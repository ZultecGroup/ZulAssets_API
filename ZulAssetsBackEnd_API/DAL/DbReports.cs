﻿using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using ZulAssetsBackEnd_API.BAL;

namespace ZulAssetsBackEnd_API.DAL
{
    public class DbReports
    {

        #region Declaration

        Connection con;
        DataTable dt;
        DataSet ds;
        SqlDataAdapter da;
        SqlCommand cmd;
        string query = string.Empty;

        #endregion

        #region With Parameters

        public DataTable DTWithParam(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                cmd = new SqlCommand();

                con = new Connection();
                using (sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataTable dt2 = new DataTable();
                DataRow dtRow = dt2.NewRow();
                dt2.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt2.Rows.Add(dtRow);
                return dt2;
            }
            finally
            {
                if (sqlcon != null)
                {
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }
        }

        public DataTable DTWithParamSecondDB(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                cmd = new SqlCommand();

                con = new Connection(connect);

                using (sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataTable dt2 = new DataTable();
                DataRow dtRow = dt2.NewRow();
                dt2.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt2.Rows.Add(dtRow);
                return dt2;
            }
            finally
            {
                if (sqlcon != null)
                {
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }
        }

        #endregion

        #region Without Parameters

        public DataTable DTWithOutParam(string storedProcedure, int connect)
        {
            try
            {

                cmd = new SqlCommand();

                con = new Connection();
                using (SqlConnection sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    cmd.CommandTimeout = cmdTimeout;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataTable dt2 = new DataTable();
                DataRow dtRow = dt2.NewRow();
                dt2.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt2.Rows.Add(dtRow);
                return dt2;
            }
        }

        #endregion

        #region DataSet With Parameters

        public DataSet DSWithParam(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            try
            {
                cmd = new SqlCommand();

                con = new Connection();
                using (SqlConnection sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    var configurationBuilder = new ConfigurationBuilder();
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    cmd.CommandTimeout = cmdTimeout;
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dtRow = dt.NewRow();
                dt.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt.Rows.Add(dtRow);
                ds.Tables.Add(dt);
                return ds;
            }
        }

        public DataSet DSWithParamSecondDB(string storedProcedure, SqlParameter[] parameters, int connect)
        {
            SqlConnection sqlcon = new SqlConnection();
            try
            {
                cmd = new SqlCommand();

                con = new Connection(connect);

                using (sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i]);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dtRow = dt.NewRow();
                dt.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt.Rows.Add(dtRow);
                ds.Tables.Add(dt);
                return ds;
            }
            finally
            {
                if (sqlcon != null)
                {
                    sqlcon.Close();
                    sqlcon.Dispose();
                }
            }
        }

        #endregion

        #region DataSet WithOut Parameters

        public DataSet DSWithOutParam(string storedProcedure, int connect)
        {
            try
            {
                cmd = new SqlCommand();

                con = new Connection();
                using (SqlConnection sqlcon = con.GetDataBaseConnection())
                {
                    query = storedProcedure;
                    cmd = new SqlCommand(query, sqlcon);
                    var configurationBuilder = new ConfigurationBuilder();
                    int cmdTimeout = Convert.ToInt32(ConfigSettings.ConfigSettings_id(4));
                    cmd.CommandTimeout = cmdTimeout;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataRow dtRow = dt.NewRow();
                dt.Columns.Add("ErrorMessage");
                dtRow["ErrorMessage"] = errorMessage;
                dt.Rows.Add(dtRow);
                ds.Tables.Add(dt);
                return ds;
            }
        }

        #endregion

    }
}
