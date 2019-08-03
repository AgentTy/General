using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace General.Data
{
    public class SqlConn
    {
        public SqlConn(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; private set; }
    }

    public class SqlHelper
    {

        #region ExecuteNonQuery

        #region Base
        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
        /// using the SqlCommand.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(cmd, true);
        /// </remarks>
        /// <param name="cmd">A valid SqlCommand</param>
        /// <param name="mustCloseConnection">Should the connection be closed upon completion</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        private static int ExecuteNonQuery(SqlCommand cmd, bool mustCloseConnection)
        {
            int retval;
            if (cmd.Connection == null) throw new ArgumentNullException("connection");
            if (cmd == null) throw new ArgumentNullException("cmd");

            // Finally, execute the command
            try
            {
                retval = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if ((int)ex.Class <= 16)
                {
                    string strErr = "An SQL Server error occurred while trying to execute the command (" + cmd.CommandText + ").\n";
                    strErr += GetQueryString(cmd);
                    throw new Exception(strErr, ex);
                }
                else
                    throw;
            }

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                cmd.Connection.Close();

            return retval;
        }
        #endregion Base

        #region Overrides
        /// <summary>
        /// Executes a SqlCommand and attaches a new connection if one hasn't been attached.
        /// </summary>
        /// <param name="cmd">SQLCommand - A valid command object with or without a Connection</param>
        /// <param name="conn">SqlConn - A connection context object that will be used if a SQLConnection isnt already attached</param>
        /// <param name="ConnectionStringName">ConnectionStringName - A valid connection string defined in the App.config file</param>
        /// <returns>int - Represents the number of rows effected</returns>
        public static int ExecuteNonQuery(SqlCommand cmd, SqlConn conn, String ConnectionStringName = null)
        {
            if (conn == null || String.IsNullOrEmpty(conn.ConnectionString))
                if (!String.IsNullOrWhiteSpace(ConnectionStringName))
                    conn = new SqlConn(DBConnection.GetConnectionString(ConnectionStringName));

            if (cmd.Connection == null)
            {
                cmd.Connection = DBConnection.GetOpenConnection(conn.ConnectionString);
            }
            return ExecuteNonQuery(cmd, true);
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
        /// using the SqlCommand.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(cmd, conn, true);
        /// </remarks>
        /// <param name="cmd">A valid SqlCommand</param>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="mustCloseConnection">Should the connection be closed upon completion</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlCommand cmd, SqlConnection connection, bool mustCloseConnection = false)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (cmd == null) throw new ArgumentNullException("cmd");
            cmd.Connection = connection;
            return (ExecuteNonQuery(cmd, mustCloseConnection));
        }


        #endregion Overrides

        #endregion ExecuteNonQuery

        #region ExecuteScalar

        #region Base
        /// <summary>
        /// Execute a SqlCommand (returning a single value) against the specified SqlConnection 
        /// using the SqlCommand.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteScalar(cmd, true);
        /// </remarks>
        /// <param name="cmd">A valid SqlCommand</param>
        /// <param name="mustCloseConnection">Should the connection be closed upon completion</param>
        /// <returns>An object representing the first column of the first row of data</returns>
        private static object ExecuteScalar(SqlCommand cmd, bool mustCloseConnection)
        {
            object retval;
            if (cmd.Connection == null) throw new ArgumentNullException("connection");
            if (cmd == null) throw new ArgumentNullException("cmd");

            // Finally, execute the command
            try
            {
                retval = cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                if ((int)ex.Class <= 16)
                {
                    string strErr = "An SQL Server error occurred while trying to execute the command (" + cmd.CommandText + ").\n";
                    strErr += GetQueryString(cmd);
                    throw new Exception(strErr, ex);
                }
                else
                    throw;
            }

            // Detach the SqlParameters from the command object, so they can be used again
            //cmd.Parameters.Clear(); //I commented this out May 2015 so I could get output parameter values here
            if (mustCloseConnection)
                cmd.Connection.Close();

            return retval;
        }
        #endregion Base

        #region Overrides
        /// <summary>
        /// Executes a SqlCommand and attaches a new connection if one hasn't been attached.
        /// </summary>
        /// <param name="cmd">SQLCommand - A valid command object with or without a Connection</param>
        /// <param name="conn">SqlConn - A connection context object that will be used if a SQLConnection isnt already attached</param>
        /// <param name="ConnectionStringName">ConnectionStringName - A valid connection string defined in the App.config file</param>
        /// <returns>An object representing the first column of the first row of data</returns>
        public static object ExecuteScalar(SqlCommand cmd, SqlConn conn, String ConnectionStringName = null)
        {
            if (conn == null || String.IsNullOrEmpty(conn.ConnectionString))
                if (!String.IsNullOrWhiteSpace(ConnectionStringName))
                    conn = new SqlConn(DBConnection.GetConnectionString(ConnectionStringName));

            if (cmd.Connection == null)
            {
                cmd.Connection = DBConnection.GetOpenConnection(conn.ConnectionString);
            }
            return ExecuteScalar(cmd, true);
        }

        /// <summary>
        /// Execute a SqlCommand (returning a single value) against the specified SqlConnection 
        /// using the SqlCommand.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteScalar(conn, cmd, true);
        /// </remarks>
        /// <param name="cmd">A valid SqlCommand</param>
        /// <param name="connection">A valid SqlConnection</param>
        /// <param name="mustCloseConnection">Should the connection be closed upon completion</param>
        /// <returns>An object representing the first column of the first row of data</returns>
        public static object ExecuteScalar(SqlCommand cmd, SqlConnection connection, bool mustCloseConnection = true)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (cmd == null) throw new ArgumentNullException("cmd");
            cmd.Connection = connection;
            return (ExecuteScalar(cmd, mustCloseConnection));
        }


        #endregion Overrides

        #endregion ExecuteScalar

        #region ExecuteList
        public static List<string> ExecuteList(SqlCommand cmd, SqlConn conn)
        {

            #region Manage Connection
            if (cmd == null) throw new ArgumentNullException("cmd");
            if (cmd.Connection == null)
            {
                cmd.Connection = DBConnection.GetOpenConnection(conn.ConnectionString);
            }
            else if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            #endregion

            // Create the DataAdapter & DataSet
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {

                #region Execute
                DataSet ds = new DataSet();
                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(ds);
                #endregion

                #region Manage Connection
                cmd.Connection.Close();
                #endregion

                #region Populate List
                List<string> objList = new List<string>();
                foreach (DataRow row in ds.Tables[0].Rows)
                    objList.Add(row[0].ToString());
                #endregion

                return objList;
            }
        }
        #endregion ExecuteList

        #region ExecuteDataset
        /// <summary>
        /// Executes a SqlCommand and attaches a new connection if one hasn't been attached.
        /// </summary>
        /// <param name="cmd">SQLCommand - A valid command object with or without a Connection</param>
        /// <param name="conn">SqlConn - A connection context object that will be used if a SQLConnection isnt already attached</param>
        /// <param name="ConnectionStringName">ConnectionStringName - A valid connection string defined in the App.config file</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataset(SqlCommand cmd, SqlConn conn, String ConnectionStringName = null)
        {
            if (cmd == null) throw new ArgumentNullException("cmd");

            // Create the DataAdapter & DataSet
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                if (conn == null || String.IsNullOrEmpty(conn.ConnectionString))
                    if (!String.IsNullOrWhiteSpace(ConnectionStringName))
                        conn = new SqlConn(DBConnection.GetConnectionString(ConnectionStringName));

                // Fill the DataSet
                try
                {
                    if (cmd.Connection == null)
                        cmd.Connection = DBConnection.GetOpenConnection(conn.ConnectionString);
                    da.Fill(ds);
                    cmd.Connection.Close();
                }
                catch (SqlException ex)
                {
                    if ((int)ex.Class <= 16)
                    {
                        string strErr = "A SQL Server error occurred while trying to execute the command (" + cmd.CommandText + ").\n";
                        strErr += ex.Message + "\n";
                        strErr += GetQueryString(cmd);
                        throw new Exception(strErr, ex);
                    }
                    else if (ex.Message.ToLower().Contains("a transport-level error has occurred"))
                    {
                        throw new Exception(ex.Message + " : " + ConnectionStringName, ex);
                    }
                    else
                        throw new Exception(ex.Message + " : " + ConnectionStringName, ex);
                }
                catch (System.Exception ex)
                {
                    string strErr = "An error occured while communicating with SQL Server (" + cmd.CommandText + ").\n";
                    strErr += ex.Message + "\n";
                    strErr += GetQueryString(cmd);
                    throw new Exception(strErr, ex);
                }

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                // Return the dataset
                return ds;
            }
        }
        #endregion

        #region ExecuteSqlBatchScript

        public static async Task<bool> ExecuteSqlBatchScript(string connectionString, string sqlScript)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                return await General.Data.SqlHelper.ExecuteSqlBatchScript(conn, sqlScript);
            }
        }

        public static async Task<bool> ExecuteSqlBatchScript(SqlConnection conn, string sqlScript)
        {
            bool hasErrors = false;
            // split script on GO command
            IEnumerable<string> commandStrings = Regex.Split(sqlScript, @"^\s*GO\s*$",
                                     RegexOptions.Multiline | RegexOptions.IgnoreCase);

            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
            foreach (string commandString in commandStrings)
            {
                if (commandString.Trim() != "")
                {
                    try
                    {
                        using (var command = new SqlCommand(commandString, conn))
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("SQLScript", commandString);
                        throw ex;
                    }
                }
            }

            return !hasErrors;
        }
        #endregion

        #region GetQueryString
        /// <summary>
        /// This method will return a string containing the command text and its parameters.
        /// </summary>
        /// <param name="cmd">A valid SqlCommand object</param>
        public static string GetQueryString(SqlCommand cmd)
        {
            string strParamList = cmd.CommandText + " ";
            foreach (SqlParameter arg in cmd.Parameters)
            {
                if (arg.Value != null)
                {
                    if (StringFunctions.IsNumeric(arg.Value.ToString()))
                        strParamList += arg.ParameterName + "=" + arg.Value + ",\n";
                    else
                        strParamList += arg.ParameterName + "='" + arg.Value + "',\n";
                }
            }
            strParamList = StringFunctions.Shave(strParamList, 2);
            return (strParamList);
        }
        #endregion

    }
}
