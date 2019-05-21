using System;
using General.Configuration;

namespace General.Environment
{
	/// <summary>
	/// Current Environment
	/// </summary>
	public class Current
    {

        #region Settings

        private static string _strHostingEnvironment;
        public static string HostingEnvironment {
            get {
                /*
                //Support for environmental variables in Azure
                string azureValue = System.Environment.GetEnvironmentVariable("HostingEnvironment");
                if (!String.IsNullOrWhiteSpace(azureValue))
                    return azureValue;
                azureValue = System.Environment.GetEnvironmentVariable("hosting_environment");
                if (!String.IsNullOrWhiteSpace(azureValue))
                    return azureValue;
                */
                if (_strHostingEnvironment != null)
                    return _strHostingEnvironment;
                _strHostingEnvironment = GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("HostingEnvironment") ?? GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("hosting_environment") ?? "";
                return _strHostingEnvironment;
            }
        }
        public static string ServerNameList_Dev { get { return GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("ServerNameList_Dev") ?? ""; } }
        public static string ServerNameList_QA { get { return GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("ServerNameList_QA") ?? ""; } }
        public static string ServerNameList_Staging { get { return GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("ServerNameList_Staging") ?? ""; } }
        public static string ServerNameList_CustomEnv { get { return GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("ServerNameList_CustomEnv") ?? ""; } }

        #endregion
        
        #region Constructor
        /// <summary>
		/// Current Environment
		/// </summary>
		public Current()
		{

        }
        #endregion

        #region WhereAmI

        #region Shortcuts
        /// <summary>
		/// Returns false if the current MachineName is in the list of Developement Servers or Stage Servers
		/// </summary>
		public static bool AmILive()
		{
			return(WhereAmI() == EnvironmentContext.Live);
		}

		/// <summary>
		/// Returns true if the current MachineName is in the list of Developement Servers
		/// </summary>
		public static bool AmIDev()
		{
			return(WhereAmI() == EnvironmentContext.Dev);
		}

        /// <summary>
        /// Returns true if the current MachineName is in the list of Developement Servers
        /// </summary>
        public static bool AmIQA()
        {
            return (WhereAmI() == EnvironmentContext.QA);
        }

		/// <summary>
		/// Returns true if the current MachineName is in the list of Stage Servers
		/// </summary>
		public static bool AmIStage()
		{
			return(WhereAmI() == EnvironmentContext.Stage);
		}

        /// <summary>
        /// Returns true if the current MachineName is in the list of Stage Servers
        /// </summary>
        public static bool AmICustomEnv()
        {
            return (WhereAmI() == EnvironmentContext.CustomEnv);
        }
        #endregion

        #region AmIBeta
        /// <summary>
        /// Returns true if the url contains beta.
        /// </summary>
        public static bool AmIBeta()
        {
            if (System.Web.HttpContext.Current == null)
                return false;
            if (StringFunctions.AllAfter(System.Web.HttpContext.Current.Request.Url.OriginalString,"://").StartsWith("beta"))
                return true;
            return System.Web.HttpContext.Current.Request.Url.OriginalString.Contains("beta.");
        }
        #endregion

		/// <summary>
		/// Returns an EnvironmentContext Enumeration based on the current server name compared to the list of Developement, QA and Stage servers.
		/// </summary>
		public static EnvironmentContext WhereAmI()
		{

            if(!String.IsNullOrEmpty(HostingEnvironment))
            {
                EnvironmentContext manualEnv = (EnvironmentContext) Enum.Parse(typeof(EnvironmentContext), HostingEnvironment, true);
                return manualEnv;
            }

            //.Net Core Support
            string coreEnv = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (!String.IsNullOrWhiteSpace(coreEnv))
            {
                switch(coreEnv)
                {
                    case "Development":
                    case "Dev":
                        return EnvironmentContext.Dev;
                    case "QA":
                        return EnvironmentContext.QA;
                    case "Staging":
                    case "Stage":
                        return EnvironmentContext.Stage;
                    default:
                        return EnvironmentContext.Live;
                }
            }

            //.Net Framework Support
            string server_name = System.Environment.MachineName.ToUpperInvariant();

            string dev_list = ServerNameList_Dev.ToUpperInvariant();
            string qa_list = ServerNameList_QA.ToUpperInvariant();
            string stage_list = ServerNameList_Staging.ToUpperInvariant();
            string custom_list = ServerNameList_CustomEnv.ToUpperInvariant();

            if (!String.IsNullOrEmpty(dev_list))
            {
                string[] list = dev_list.Split(',');
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == server_name)
                        return (EnvironmentContext.Dev);
                }
            }

            if (!String.IsNullOrEmpty(qa_list))
            {
                string[] list = qa_list.Split(',');
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == server_name)
                        return (EnvironmentContext.QA);
                }
            }

            if (!String.IsNullOrEmpty(stage_list))
            {
                string[] list = stage_list.Split(',');
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == server_name)
                        return (EnvironmentContext.Stage);
                }
            }

            if (!String.IsNullOrEmpty(custom_list))
            {
                string[] list = custom_list.Split(',');
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == server_name)
                        return (EnvironmentContext.CustomEnv);
                }
            }

			return EnvironmentContext.Live;
        }
        #endregion

        #region MapPath
        /// <summary>
		/// Maps a physical path from a relative path
		/// </summary>
		public static string MapPath(string path)
		{
			if(System.Web.HttpContext.Current == null)
				throw(new Exception("Cannot MapPath in this context, try using System.Web.HttpRuntime.AppDomainAppPath"));
			else
				return(System.Web.HttpContext.Current.Server.MapPath(path));
        }
        #endregion

        #region GetAppDirectory
        /// <summary>
		/// Returns the root directory of the current application
		/// </summary>
		public static string GetAppDirectory()
		{
			return System.AppDomain.CurrentDomain.BaseDirectory.Replace("/","\\");
        }
        #endregion

        #region GetBinDirectory
        /// <summary>
		/// Returns the config directory of the current application
		/// </summary>
		public static string GetBinDirectory()
		{
			if(StringFunctions.Contains(System.AppDomain.CurrentDomain.BaseDirectory.ToLower(),"\\bin\\"))
				return System.AppDomain.CurrentDomain.BaseDirectory.Replace("/","\\");
			else
				return System.AppDomain.CurrentDomain.BaseDirectory.Replace("/","\\") + "bin" + "\\";
        }
        #endregion

        #region Debugging
        public static bool TraceEnabled = true;
        /// <summary>
        /// Attempts to write a trace line
        /// </summary>
        public static void Trace(string Message)
        {
            if (TraceEnabled)
                if (System.Web.HttpContext.Current != null)
                    System.Web.HttpContext.Current.Trace.Write(Message);
        }

        /// <summary>
        /// Attempts to write a trace line
        /// </summary>
        public static void Trace(bool Logic)
        {
            if (TraceEnabled)
                if (System.Web.HttpContext.Current != null)
                    System.Web.HttpContext.Current.Trace.Write(Logic.ToString());
        }

        /*
        /// <summary>
        /// Attempts to write a trace line
        /// </summary>
        public static void Trace(System.Data.SqlClient.SqlCommand objCommand)
        {
            if (TraceEnabled)
                if (System.Web.HttpContext.Current != null)
                    System.Web.HttpContext.Current.Trace.Write(General.DAO.SqlHelper.GetQueryString(objCommand));
        }
        */

        /*
        #region JQuery Debug
        public static void JQueryDebugWrite(string strDebugText)
        {
            strDebugText = strDebugText.Replace("'", "\"");
            //if(General.Environment.Current.AmIDev())
            ExecuteJavascript("$(document).ready(function() {if($.log){$.log('" + strDebugText + "');}});");
            //ExecuteJavascript("alert('" + strDebugText + "');");
        }
        #endregion
        */

        #endregion

    }

	public enum EnvironmentContext
	{
		Dev = 1,
        QA = 2,
		Stage = 3,
        CustomEnv = 4,
        Live = 5
	}
}
