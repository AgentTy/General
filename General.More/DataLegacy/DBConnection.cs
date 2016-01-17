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

		/// <summary>
		/// Gets the configured connection string
		/// </summary>
		public static string GetConnectionString(string strConnectionName)
		{
            string strConnection = General.Data.DBConnection.GetConnectionString(strConnectionName);
            if (!StringFunctions.IsNullOrWhiteSpace(strConnection))
                return strConnection;

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
                /*
                try //LOOK FOR HTTP HOST RELATIVE CONNECTION STRING
                {
                    string strSource;
                    if (!StringFunctions.IsNullOrWhiteSpace(GlobalConfiguration.GlobalSettings["spoof_host"]) && iAm == General.Environment.EnvironmentContext.Dev)
                        strSource = GlobalConfiguration.GlobalSettings["spoof_host"].ToString().ToLower();
                    else
                    {
                        System.Web.HttpRequest Request = System.Web.HttpContext.Current.Request;
                        strSource = Request.ServerVariables["HTTP_HOST"].ToLower();
                    }
                    strSource = strSource.Replace("http://", "");
                    strSource = strSource.Replace("https://", "");

                    strConnection = GlobalConfiguration.GlobalSettings["str_connection" + strSuffix + "_" + strSource].ToString();
                }
                catch //LOOK FOR SERVER RELATIVE CONNECTION STRING
                {
                    try
                    {
                        strConnection = GlobalConfiguration.GlobalSettings["str_connection" + strSuffix].ToString();
                    }
                    catch
                    {
                        throw new NullReferenceException("Unable to find connection string");
                    }
                }
                */
            }
			return strConnection;
		}
        #endregion

        #region GetConnectionString_NoHost

        public static string GetConnectionString_NoHost()
        {
            return GetConnectionString_NoHost("");
        }

        public static string GetConnectionString_NoHost(int intClientID)
        {
            return GetConnectionString_NoHost(intClientID.ToString());
        }

        /// <summary>
        /// Gets the configured connection string
        /// </summary>
        public static string GetConnectionString_NoHost(string strConnectionName)
        {
            string strConnection = String.Empty;

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
                        strConnection = GlobalConfiguration.GlobalSettingsExplicit["str_connection" + strSuffix].ToString();
                    else
                    {
                        try
                        {
                            //Try getting named/client specific connection string
                            strConnection = GlobalConfiguration.GlobalSettingsExplicit["str_connection_" + strConnectionName + strSuffix].ToString();
                        }
                        catch
                        {
                            strConnection = String.Empty;
                        }
                        if (StringFunctions.IsNullOrWhiteSpace(strConnection)) //Use default connection
                            strConnection = GlobalConfiguration.GlobalSettingsExplicit["str_connection" + strSuffix].ToString();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        string strComment = "(no host)";
                        General.Debugging.Report.SendError("Connection String Error: " + strConnectionName + strSuffix + strComment, ex);
                    }
                    catch { }
                    throw new NullReferenceException("Unable to find connection string: " + strConnectionName + strSuffix, ex);
                }
            }
            return strConnection;
        }
        #endregion

        #region GetAllConnections/Strings
        public static System.Configuration.ConnectionStringSettingsCollection GetAllConnectionStrings()
        {
            System.Configuration.ConnectionStringSettingsCollection objConns = new System.Configuration.ConnectionStringSettingsCollection();

            #region Get Suffix From MachineName
            string strSuffix = String.Empty;

            if (PickConnectionByDevLiveStage)
                strSuffix = "_" + General.Environment.Current.WhereAmI().ToString().ToLower();
            #endregion

            if (GlobalConfiguration.GetFromAppConfig)
            {
                foreach (System.Configuration.ConnectionStringSettings objConnString in System.Configuration.ConfigurationManager.ConnectionStrings)
                {
                    if (PickConnectionByDevLiveStage)
                    {
                        if (objConnString.Name.EndsWith(strSuffix))
                            objConns.Add(objConnString);
                    }
                    else
                    {
                        objConns.Add(objConnString);
                    }
                }

            }

            return objConns;
        }


        //This is very BAD way to manage connections
        public static System.Collections.Generic.List<SqlConnection> GetAllConnections()
        {
            System.Collections.Generic.List<SqlConnection> objConns = new System.Collections.Generic.List<SqlConnection>();

            #region Get Suffix From MachineName
            string strSuffix = String.Empty;

            if (PickConnectionByDevLiveStage)
                strSuffix = "_" + General.Environment.Current.WhereAmI().ToString().ToLower();
            #endregion

            if (GlobalConfiguration.GetFromAppConfig)
            {
                foreach (System.Configuration.ConnectionStringSettings objConnString in System.Configuration.ConfigurationManager.ConnectionStrings)
                {
                    if (PickConnectionByDevLiveStage)
                    {
                        if (objConnString.Name.EndsWith(strSuffix))
                            objConns.Add(new SqlConnection(objConnString.ConnectionString));
                    }
                    else
                    {
                        objConns.Add(new SqlConnection(objConnString.ConnectionString));
                    }
                }
                
            }

            if (objConns.Count == 0)
            {
                //throw new Exception("This method is not yet supported in General.config");
                //strConnection = GlobalConfiguration.GlobalSettings["str_connection" + strSuffix].ToString();
            }

            return objConns;
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
