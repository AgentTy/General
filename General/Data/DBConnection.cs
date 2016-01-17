using System;
using General;
using General.Configuration;
using System.Data.SqlClient;

namespace General.Data
{
	/// <summary>
	/// This class manages connections to a MSSQL database
	/// </summary>
	public class DBConnection
	{

		#region Constructors
		/// <summary>
		/// Summary description for DBConnection.
		/// </summary>
		public DBConnection()
		{

		}

		#endregion

        #region Properties
        private static bool _blnPickConnectionByDevLiveStage = true;
        public static bool PickConnectionByDevLiveStage
        {
            get { return _blnPickConnectionByDevLiveStage; }
            set { _blnPickConnectionByDevLiveStage = value; }
        }
        #endregion

        #region GetConnectionString

        public static string GetConnectionString()
        {
            return GetConnectionString("");
        }

		/// <summary>
		/// Gets the configured connection string
		/// </summary>
        public static string GetConnectionString(string strConnectionName)
		{
            System.Configuration.ConnectionStringSettings objConnString = null;

            #region Get Suffix From MachineName
            string strSuffix = String.Empty;

            if (PickConnectionByDevLiveStage)
                strSuffix = "_" + General.Environment.Current.WhereAmI().ToString().ToLower();
            #endregion

            if (String.IsNullOrEmpty(strConnectionName))
                if (!String.IsNullOrEmpty(General.Configuration.GlobalConfiguration.DefaultConnectionStringName))
                    strConnectionName = General.Configuration.GlobalConfiguration.DefaultConnectionStringName;
                else
                    strConnectionName = "ConnectionString";

            if (System.Configuration.ConfigurationManager.ConnectionStrings[strConnectionName + strSuffix] != null)
            {
                objConnString = System.Configuration.ConfigurationManager.ConnectionStrings[strConnectionName + strSuffix];
            }
            else if (System.Configuration.ConfigurationManager.ConnectionStrings[strConnectionName] != null)
            {
                objConnString = System.Configuration.ConfigurationManager.ConnectionStrings[strConnectionName];
            }
            if (objConnString != null)
                return objConnString.ConnectionString;
            else
                return String.Empty;
		}
        #endregion

        #region GetOpenConnection
        /// <summary>
		/// Gets an open connection object from the configured connection string
		/// </summary>
		public static SqlConnection GetOpenConnection()
		{
			return GetOpenConnection(GetConnectionString());
		}

		/// <summary>
		/// Gets an open connection object from the provided connection string
		/// </summary>
		public static SqlConnection GetOpenConnection(string ConnectionString)
		{
			SqlConnection objConnection = new SqlConnection(ConnectionString);
            objConnection.Open();
            return objConnection;
		}
		#endregion

	}
}
