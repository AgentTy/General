using System;
using General;
using General.Configuration;
using System.Data.SqlClient;

namespace General.DAO
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

        public static string GetConnectionString(int intClientID)
        {
            return GetConnectionString(intClientID.ToString());
        }

        //public const string DefaultConnectionStringName = "DefaultConnection";
		/// <summary>
		/// Gets the configured connection string
		/// </summary>
		public static string GetConnectionString(string strConnectionName)
		{
            return General.Data.DBConnection.GetConnectionString(strConnectionName);

            /*
            //Interrupt this method and go to the new connection string manager
            string strConnection = General.Data.DBConnection.GetConnectionString(strConnectionName);
            if (!StringFunctions.IsNullOrWhiteSpace(strConnection))
                return strConnection;

            //Legacy Code
            #region Get Suffix From MachineName
            string strSuffix = String.Empty;

            if (PickConnectionByDevLiveStage)
                strSuffix = "_" + General.Environment.Current.WhereAmI().ToString().ToLower();
            #endregion

            if (GlobalConfiguration.GetFromAppConfig)
            {
                if (string.IsNullOrEmpty(strConnectionName))
                    if (!string.IsNullOrEmpty(GlobalConfiguration.DefaultConnectionStringName))
                        strConnectionName = GlobalConfiguration.DefaultConnectionStringName;
                    else
                        strConnectionName = "str_connection";
                    
                if (System.Configuration.ConfigurationManager.ConnectionStrings[strConnectionName + strSuffix] != null)
                {
                    System.Configuration.ConnectionStringSettings objConnString = System.Configuration.ConfigurationManager.ConnectionStrings[strConnectionName + strSuffix];
                    strConnection = objConnString.ConnectionString;
                }
            }

            if (StringFunctions.IsNullOrWhiteSpace(strConnection))
            {
                try
                {
                    if (StringFunctions.IsNullOrWhiteSpace(strConnectionName)) //Use default connection
                        strConnection = GlobalConfiguration.GlobalSettings["str_connection" + strSuffix].ToString();
                    else
                    {
                        try
                        {
                            //Try getting named/client specific connection string
                            strConnection = GlobalConfiguration.GlobalSettings["str_connection_" + strConnectionName + strSuffix].ToString();
                        }
                        catch
                        {
                            strConnection = String.Empty;
                        }
                        if (StringFunctions.IsNullOrWhiteSpace(strConnection)) //Use default connection
                            strConnection = GlobalConfiguration.GlobalSettings["str_connection" + strSuffix].ToString();
                    }
                }
                catch(Exception ex)
                {
                    try
                    {
                        string strComment = "";
                        try
                        {
                            strComment = " (" + General.Configuration.GlobalConfiguration.GetCurrentHost() + ")";
                        }
                        catch { }
                        General.Debugging.Report.SendError("Connection String Error: " + strConnectionName + strSuffix + strComment, ex);
                    }
                    catch { }
                    throw new NullReferenceException("Unable to find connection string: " + strConnectionName + strSuffix, ex);
                }
              
            }*/
            //return strConnection;
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
        /// Gets an open connection object from the configured connection string
        /// </summary>
        public static SqlConnection GetOpenConnection(int intClientID)
        {
            return GetOpenConnection(GetConnectionString(intClientID));
        }

        /// <summary>
        /// Gets an open connection object from the configured connection string
        /// </summary>
        public static SqlConnection GetDynamicDataConnection()
        {
            return GetOpenConnection(GetConnectionString("DynamicData"));
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
