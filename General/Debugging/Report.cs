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

		/// <summary>
		/// Sends a Debug Report
		/// </summary>
		public static void SendDebug(string Subject, string Error)
		{
            bool blnIsHtml = false;
            if (!String.IsNullOrEmpty(Error) && Error.ToLower().Contains("<br"))
                blnIsHtml = true;
            General.Mail.MailTools.SendEmail(DebugEmailFrom, DebugEmailTo, Subject, Error, blnIsHtml);
		}

		/// <summary>
		/// Sends a Debug Report
		/// </summary>
		public static void SendDebug(string Subject,Exception Error)
		{
			SendDebug(Subject,Error.ToString());
		}

		/// <summary>
		/// Sends a Debug Report
		/// </summary>
		public static void SendDebug(Exception Error)
		{
			SendDebug(Error.Message,Error.ToString());
		}

		/// <summary>
		/// Sends a Debug Report
		/// </summary>
		public static void SendDebug(string Error)
		{
			SendDebug("Debug Report",Error);
		}


		/// <summary>
		/// Sends an Error Report
		/// </summary>
		public static void SendError(string Subject,string Error)
		{
            bool blnIsHtml = false;
            if (!String.IsNullOrEmpty(Error) && Error.ToLower().Contains("<br"))
                blnIsHtml = true;
            General.Mail.MailTools.SendEmail(ErrorEmailFrom, ErrorEmailTo, Subject, Error, blnIsHtml);
		}

		/// <summary>
		/// Sends an Error Report
		/// </summary>
		public static void SendError(string Subject,Exception Error)
		{
			SendError(Subject,Error.ToString());
		}

		/// <summary>
		/// Sends an Error Report
		/// </summary>
		public static void SendError(Exception Error)
		{
            string strSubject = Error.Message;
            strSubject = strSubject.Replace('\r', ' ').Replace('\n', ' ');
            SendError(strSubject, Error.ToString());
		}

		/// <summary>
		/// Sends an Error Report
		/// </summary>
		public static void SendError(string Error)
		{
			SendError("Error Report",Error);
		}

	}
}
