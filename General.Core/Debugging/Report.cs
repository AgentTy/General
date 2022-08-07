using System;
using General;
using General.Configuration;

namespace General.Debugging
{
	/// <summary>
	/// Report
	/// </summary>
	public class Report
    {

        #region Settings
        public static string DebugEmailFrom { get { return GlobalConfiguration.GlobalSettings["DebugEmailFrom"] ?? GlobalConfiguration.GlobalSettings["debug_email_from"]; } }
        public static string DebugEmailTo { get { return GlobalConfiguration.GlobalSettings["DebugEmailTo"] ?? GlobalConfiguration.GlobalSettings["debug_email_to"]; } }

        public static string ErrorEmailFrom { get { return GlobalConfiguration.GlobalSettings["ErrorEmailFrom"] ?? GlobalConfiguration.GlobalSettings["error_email_from"]; } }
        public static string ErrorEmailTo { get { return GlobalConfiguration.GlobalSettings["ErrorEmailTo"] ?? GlobalConfiguration.GlobalSettings["error_email_to"]; } }
        #endregion

        #region Constructors
        /// <summary>
		/// Report
		/// </summary>
		public Report()
		{

		}

		#endregion


	}
}
