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
        public static string ServerNameList_Dev { get { return GlobalConfiguration.GlobalSettings.GetGlobalOnly("ServerNameList_Dev") ?? GlobalConfiguration.GlobalSettings.GetGlobalOnly("server_name_dev") ?? ""; } }
        public static string ServerNameList_Staging { get { return GlobalConfiguration.GlobalSettings.GetGlobalOnly("ServerNameList_Staging") ?? GlobalConfiguration.GlobalSettings.GetGlobalOnly("server_name_stage") ?? ""; } }
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
		/// Returns true if the current MachineName is in the list of Stage Servers
		/// </summary>
		public static bool AmIStage()
		{
			return(WhereAmI() == EnvironmentContext.Stage);
		}

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
		/// Returns an EnvironmentContext Enumeration based on the current server name compared to the list of Developement and Stage servers.
		/// </summary>
		public static EnvironmentContext WhereAmI()
		{
			string server_name = System.Environment.MachineName.ToUpperInvariant();
            string dev_list = ServerNameList_Dev.ToUpperInvariant();
			if(dev_list == null)
				dev_list = "";
            string stage_list = ServerNameList_Staging.ToUpperInvariant();
			if(stage_list == null)
				stage_list = "";

			string[] list = dev_list.Split(',');	
			for(int i=0; i<list.Length; i++)
			{
				if(list[i] == server_name)
					return(EnvironmentContext.Dev);
			}

			list = stage_list.Split(',');	
			for(int i=0; i<list.Length; i++)
			{
				if(list[i] == server_name)
					return(EnvironmentContext.Stage);
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
		Stage = 2,
		Live = 3
	}
}
