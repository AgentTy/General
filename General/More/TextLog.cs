using System;
using General.Configuration;
using General.Model;

namespace General
{
	/// <summary>
	/// Accumulates and emails a log of anything you want
	/// </summary>
	public class TextLog
	{
		private string _log;
		
		/// <summary>
		/// Accumulates and emails a log of anything you want
		/// </summary>
		public TextLog()
		{
			_log = "";
		}

		/// <summary>
		/// Write a line to the log
		/// </summary>
		public void Write(string input)
		{
			_log += DateTime.Now.ToString() + ":   " + input + "\r\n";
		}

		/// <summary>
		/// Email the log
		/// </summary>
		public void Send()
		{
			string subject = "Log dump " + DateTime.Now.ToString();
			General.Mail.MailTools.SendEmail(GlobalConfiguration.GlobalSettings["debug_email_from"],GlobalConfiguration.GlobalSettings["debug_email_to"],subject,_log,false);
		}

		/// <summary>
		/// Email the log
		/// </summary>
		public void Send(string Subject)
		{
			General.Mail.MailTools.SendEmail(GlobalConfiguration.GlobalSettings["debug_email_from"],GlobalConfiguration.GlobalSettings["debug_email_to"],Subject,_log,false);
		}

		/// <summary>
		/// Email the log
		/// </summary>
		public void Send(EmailAddress Recipient, string Subject)
		{
			General.Mail.MailTools.SendEmail(GlobalConfiguration.GlobalSettings["debug_email_from"],Recipient.ToString(),Subject,_log,false);
		}

        /// <summary>
        /// Save the log to file
        /// </summary>
        public void Save(string strFileName)
        {
            System.IO.StreamWriter w = System.IO.File.AppendText(strFileName);
            w.Write(_log);
            w.Close();
        }

		/// <summary>
		/// Clear log
		/// </summary>
		public void Clear()
		{
			_log = "";
		}

		/// <summary>
		/// Read log
		/// </summary>
		public override string ToString()
		{
			return _log;
		}
	}
}
