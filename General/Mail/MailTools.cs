using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using General;
using General.Model;
using General.StringExtensions;
using General.Configuration;

namespace General.Mail {
	/// <summary>
	/// Mail Tools
	/// </summary>
	public class MailTools 
	{
        #region Settings
        protected static string MailServerDebug { get { return GlobalConfiguration.GlobalSettings["MailServerDebug"] ?? GlobalConfiguration.GlobalSettings["mail_server_debug"]; } }
        protected static string MailServerDebug_Port { get { return GlobalConfiguration.GlobalSettings["MailServerDebug_Port"] ?? GlobalConfiguration.GlobalSettings["mail_server_port_debug"]; } }
        protected static string MailServerDebug_UserName { get { return GlobalConfiguration.GlobalSettings["MailServerDebug_UserName"] ?? GlobalConfiguration.GlobalSettings["mail_server_username_debug"]; } }
        protected static string MailServerDebug_Password { get { return GlobalConfiguration.GlobalSettings["MailServerDebug_Password"] ?? GlobalConfiguration.GlobalSettings["mail_server_password_debug"]; } }

        protected static string MailServerBulkEmail { get { return GlobalConfiguration.GlobalSettings["MailServerBulkEmail"] ?? GlobalConfiguration.GlobalSettings["mail_server_bulk_email"]; } }
        protected static string MailServerBulkEmail_Port { get { return GlobalConfiguration.GlobalSettings["MailServerBulkEmail_Port"] ?? GlobalConfiguration.GlobalSettings["mail_server_port_bulk_email"]; } }
        protected static string MailServerBulkEmail_UserName { get { return GlobalConfiguration.GlobalSettings["MailServerBulkEmail_UserName"] ?? GlobalConfiguration.GlobalSettings["mail_server_username_bulk_email"]; } }
        protected static string MailServerBulkEmail_Password { get { return GlobalConfiguration.GlobalSettings["MailServerBulkEmail_Password"] ?? GlobalConfiguration.GlobalSettings["mail_server_password_bulk_email"]; } }

        protected static string MailServerDev { get { return GlobalConfiguration.GlobalSettings["MailServerDev"] ?? GlobalConfiguration.GlobalSettings["mail_server_dev"]; } }
        protected static string MailServerDev_Port { get { return GlobalConfiguration.GlobalSettings["MailServerDev_Port"] ?? GlobalConfiguration.GlobalSettings["mail_server_port_dev"]; } }
        protected static string MailServerDev_UserName { get { return GlobalConfiguration.GlobalSettings["MailServerDev_UserName"] ?? GlobalConfiguration.GlobalSettings["mail_server_username_dev"]; } }
        protected static string MailServerDev_Password { get { return GlobalConfiguration.GlobalSettings["MailServerDev_Password"] ?? GlobalConfiguration.GlobalSettings["mail_server_password_dev"]; } }

        protected static string MailServerStage { get { return GlobalConfiguration.GlobalSettings["MailServerStage"] ?? GlobalConfiguration.GlobalSettings["mail_server_stage"]; } }
        protected static string MailServerStage_Port { get { return GlobalConfiguration.GlobalSettings["MailServerStage_Port"] ?? GlobalConfiguration.GlobalSettings["mail_server_port_stage"]; } }
        protected static string MailServerStage_UserName { get { return GlobalConfiguration.GlobalSettings["MailServerStage_UserName"] ?? GlobalConfiguration.GlobalSettings["mail_server_username_stage"]; } }
        protected static string MailServerStage_Password { get { return GlobalConfiguration.GlobalSettings["MailServerStage_Password"] ?? GlobalConfiguration.GlobalSettings["mail_server_password_stage"]; } }

        protected static string MailServerLive { get { return GlobalConfiguration.GlobalSettings["MailServerLive"] ?? GlobalConfiguration.GlobalSettings["mail_server_live"]; } }
        protected static string MailServerLive_Port { get { return GlobalConfiguration.GlobalSettings["MailServerLive_Port"] ?? GlobalConfiguration.GlobalSettings["mail_server_port_live"]; } }
        protected static string MailServerLive_UserName { get { return GlobalConfiguration.GlobalSettings["MailServerLive_UserName"] ?? GlobalConfiguration.GlobalSettings["mail_server_username_live"]; } }
        protected static string MailServerLive_Password { get { return GlobalConfiguration.GlobalSettings["MailServerLive_Password"] ?? GlobalConfiguration.GlobalSettings["mail_server_password_live"]; } }

        #endregion

        public static bool SendWithBCC = false;
        public static bool SendWithBulkMailServer = false;

        #region Constructor
        /// <summary>
		/// Mail Tools
		/// </summary>
		public MailTools() { }
        #endregion

        #region Overloads with Attachments
        public static bool SendEmail(string FromEmail, string ToEmail, string Subject, string Body, bool IsHtml, System.Collections.ArrayList Attachments)
        {
            if(!StringFunctions.IsNullOrWhiteSpace(ToEmail))
                if (StringFunctions.Contains(ToEmail, ";"))
                    ToEmail = ToEmail.Replace(";", ",");

            System.Collections.ArrayList aryToEmail = new System.Collections.ArrayList();
            foreach(string s in ToEmail.Split(','))
                aryToEmail.Add(new EmailAddress(s));

            return SendEmail(FromEmail, aryToEmail, Subject, Body, IsHtml, Attachments);
        }

        public static bool SendEmail(EmailAddress FromEmail, System.Collections.ArrayList ToEmail, string Subject, string Body, bool IsHtml, string[] Attachments)
        {
            System.Collections.ArrayList aryAttachments = null;
            if (Attachments != null)
            {
                aryAttachments = new System.Collections.ArrayList();
                foreach (string s in Attachments)
                    aryAttachments.Add(s);
            }
            return SendEmail(FromEmail, ToEmail, Subject, Body, IsHtml, aryAttachments);
        }

        public static bool SendEmail(EmailAddress FromEmail, EmailAddress ToEmail, string Subject, string Body, bool IsHtml, string[] Attachments)
        {
            System.Collections.ArrayList aryAttachments = null;
            if(Attachments != null)
            {
                aryAttachments = new System.Collections.ArrayList();
                foreach (string s in Attachments)
                    aryAttachments.Add(s);
            }
            return SendEmail(FromEmail, ToEmail, Subject, Body, IsHtml, aryAttachments);
        }

        public static bool SendEmail(EmailAddress FromEmail, EmailAddress ToEmail, string Subject, string Body, bool IsHtml, System.Collections.ArrayList Attachments)
        {
            System.Collections.ArrayList aryToEmail = new System.Collections.ArrayList();
            aryToEmail.Add(ToEmail);
            return SendEmail(FromEmail, aryToEmail, Subject, Body, IsHtml, Attachments);
        }
        #endregion

        #region Overloads without Attachments

        /// <summary>
        /// Send an email
        /// </summary>
        public static bool SendEmail(string FromEmail, string ToEmail, string Subject, string Body, bool IsHtml)
        {
            return SendEmail(FromEmail, ToEmail, Subject, Body, IsHtml, (System.Collections.ArrayList)null);
        }

        /// <summary>
        /// Send an email
        /// </summary>
        public static bool SendEmail(EmailAddress FromEmail, EmailAddress ToEmail, string Subject, string Body)
        {
            return (SendEmail(FromEmail.ToString(), ToEmail.ToString(), Subject, Body, false));
        }

        /// <summary>
        /// Send an email
        /// </summary>
        public static bool SendEmail(string FromEmail, string ToEmail, string Subject, string Body)
        {
            return (SendEmail(FromEmail, ToEmail, Subject, Body, false));
        }

        /// <summary>
        /// Send an email
        /// </summary>
        public static bool SendEmail(EmailAddress FromEmail, EmailAddress ToEmail, string Subject, string Body, bool IsHtml)
        {
            return (SendEmail(FromEmail.ToString(), ToEmail.ToString(), Subject, Body, IsHtml));
        }
        #endregion

        #region SendEmail
        public static bool SendEmail(EmailAddress FromEmail, System.Collections.ArrayList ToEmail, string Subject, string Body, bool IsHtml, System.Collections.ArrayList Attachments) 
		{
			MailMessage objMailMessage = new MailMessage();
            objMailMessage.From = new MailAddress(FromEmail,FromEmail.Name);

            if (Environment.Current.AmIDev())
            {
                objMailMessage.To.Add(new MailAddress(General.Debugging.Report.DebugEmailTo));
                if (ToEmail.Count > 1)
                {
                    string strRecipients = "";
                    foreach (EmailAddress objEmail in ToEmail)
                    {
                        if (!StringFunctions.IsNullOrWhiteSpace(objEmail.ToString()))
                            strRecipients += objEmail.EmailWithName + ",";
                    }
                    strRecipients = StringFunctions.Shave(strRecipients, 1);
                    Body = "Recipients(" + ToEmail.Count + "): " + strRecipients + "\n" + Body;
                }
            }
            else
            {
                foreach (EmailAddress objEmail in ToEmail)
                {
                     if(objEmail != null)
                        if (objEmail.Valid)
                        {
                            if (SendWithBCC)
                            {
                                if (StringFunctions.IsNullOrWhiteSpace(objEmail.Name))
                                    objMailMessage.Bcc.Add(objEmail.Value);
                                else
                                    objMailMessage.Bcc.Add(new MailAddress(objEmail, objEmail.Name));
                            }
                            else
                            {
                                if (StringFunctions.IsNullOrWhiteSpace(objEmail.Name))
                                    objMailMessage.To.Add(objEmail.Value);
                                else
                                    objMailMessage.To.Add(new MailAddress(objEmail, objEmail.Name));
                            }
                        }
                }
            }
            objMailMessage.Subject = Subject;
            objMailMessage.Body = Body;
            objMailMessage.IsBodyHtml = IsHtml;

			if(Attachments != null)
			{
				foreach(string s in Attachments)
				{
					if(File.Exists(s))
					{
                        Attachment objAttachment = new Attachment(s);
						objMailMessage.Attachments.Add(objAttachment);
					}
				}
			}

            System.Net.Mail.SmtpClient objMailServer;
            if (SendWithBulkMailServer && ToEmail.Count > 1 && !StringFunctions.IsNullOrWhiteSpace(MailServerBulkEmail))
                objMailServer = GetMailServer(MailServerTypes.Bulk);
            else if (!Environment.Current.AmIDev() && !StringFunctions.IsNullOrWhiteSpace(MailServerDebug) && ToEmail.Count == 1 && (((EmailAddress)ToEmail[0]) == new EmailAddress(Debugging.Report.ErrorEmailTo) || ((EmailAddress)ToEmail[0]) == new EmailAddress(Debugging.Report.DebugEmailTo)))
                objMailServer = GetMailServer(MailServerTypes.Debug);
            else
                objMailServer = GetMailServer(MailServerTypes.Normal);

			try
			{
                objMailServer.Send(objMailMessage);
			}
			catch(Exception ex)
			{
                foreach (EmailAddress objEmail in ToEmail) //IF YOU REMOVE THIS CONDITION THE SERVER WILL CRASH (INFINITE RECURSION)
                    if (objEmail == new EmailAddress(Debugging.Report.ErrorEmailTo))
                        throw new Exception("Error sending debug email", ex);

                try
                {
                    SendEmail(Debugging.Report.ErrorEmailFrom, Debugging.Report.ErrorEmailTo, "Email Send Error", ex.ToString());
                }
                catch { }

				throw;
			}
			return true;
        }
        #endregion

        #region GetMailServer
        public enum MailServerTypes
        {
            Normal = 1,
            Bulk = 2,
            Debug = 3
        }

        public static SmtpClient GetMailServer(MailServerTypes MailServerType)
        {
            string strMailServer = String.Empty;
            int intPort = 0;
            string strUserName = String.Empty;
            string strPassword = String.Empty;

            if (MailServerType == MailServerTypes.Bulk && !StringFunctions.IsNullOrWhiteSpace(MailServerBulkEmail))
            {
                //Use separate mail server for bulk email
                strMailServer = MailServerBulkEmail;
                if (!StringFunctions.IsNullOrWhiteSpace(MailServerBulkEmail_Port))
                    intPort = int.Parse(MailServerBulkEmail_Port);
                if (!StringFunctions.IsNullOrWhiteSpace(MailServerBulkEmail_UserName))
                    strUserName = MailServerBulkEmail_UserName;
                if (!StringFunctions.IsNullOrWhiteSpace(MailServerBulkEmail_Password))
                    strPassword = MailServerBulkEmail_Password;
            }
            else if (MailServerType == MailServerTypes.Debug && !StringFunctions.IsNullOrWhiteSpace(MailServerDebug))
            {
                //Use separate mail server for debugging emails and error reporting
                strMailServer = MailServerDebug;
                if (!StringFunctions.IsNullOrWhiteSpace(MailServerDebug_Port))
                    intPort = int.Parse(MailServerDebug_Port);
                if (!StringFunctions.IsNullOrWhiteSpace(MailServerDebug_UserName))
                    strUserName = MailServerDebug_UserName;
                if (!StringFunctions.IsNullOrWhiteSpace(MailServerDebug_Password))
                    strPassword = MailServerDebug_Password;
            }
            else
            {
                if (Environment.Current.AmILive())
                {
                    strMailServer = MailServerLive;
                    if (!StringFunctions.IsNullOrWhiteSpace(MailServerLive_Port))
                        intPort = int.Parse(MailServerLive_Port);
                    if (!StringFunctions.IsNullOrWhiteSpace(MailServerLive_UserName))
                        strUserName = MailServerLive_UserName;
                    if (!StringFunctions.IsNullOrWhiteSpace(MailServerLive_Password))
                        strPassword = MailServerLive_Password;
                }
                else if (Environment.Current.AmIStage())
                {
                    strMailServer = MailServerStage;
                    if (!StringFunctions.IsNullOrWhiteSpace(MailServerStage_Port))
                        intPort = int.Parse(MailServerStage_Port);
                    if (!StringFunctions.IsNullOrWhiteSpace(MailServerStage_UserName))
                        strUserName = MailServerStage_UserName;
                    if (!StringFunctions.IsNullOrWhiteSpace(MailServerStage_Password))
                        strPassword = MailServerStage_Password;
                }
                else
                {
                    strMailServer = MailServerDev;
                    if (!StringFunctions.IsNullOrWhiteSpace(MailServerDev_Port))
                        intPort = int.Parse(MailServerDev_Port);
                    if (!StringFunctions.IsNullOrWhiteSpace(MailServerDev_UserName))
                        strUserName = MailServerDev_UserName;
                    if (!StringFunctions.IsNullOrWhiteSpace(MailServerDev_Password))
                        strPassword = MailServerDev_Password;
                }
            }

            if (StringFunctions.IsNullOrWhiteSpace(strMailServer))
            {
                strMailServer = MailServerLive;
                if (!StringFunctions.IsNullOrWhiteSpace(MailServerLive_Port))
                    intPort = int.Parse(MailServerLive_Port);
                if (!StringFunctions.IsNullOrWhiteSpace(MailServerLive_UserName))
                    strUserName = MailServerLive_UserName;
                if (!StringFunctions.IsNullOrWhiteSpace(MailServerLive_Password))
                    strPassword = MailServerLive_Password;
            }

            if (StringFunctions.IsNullOrWhiteSpace(strMailServer))
                throw new Exception("Null Mail Server");

            System.Net.Mail.SmtpClient objMailServer = new System.Net.Mail.SmtpClient();
            objMailServer.Host = strMailServer;
            if (intPort > 0)
                objMailServer.Port = intPort;
            //if (objMailServer.Port == 587) //This appears to be a bad assumption to make
            //    objMailServer.EnableSsl = true;

            if (!StringFunctions.IsNullOrWhiteSpace(strUserName))
            {
                NetworkCredential objCredentials = new NetworkCredential(strUserName, strPassword);
                objMailServer.UseDefaultCredentials = false;
                objMailServer.Credentials = objCredentials;
            }

            return objMailServer;
        }
        #endregion

        #region CreateMessageID
        /// <summary>
        /// Creates Rfc 2822 3.6.4 message-id. Syntax: '&lt;' id-left '@' id-right '&gt;'.
        /// Taken from LumiSoft.Net.MIME.MIME_Utils
        /// </summary>
        /// <returns><d8784ef78f8e4415bf27a0e56b80968c@18d3c3397b584335b69ea8c51ba1b8ed></returns>
        public static string CreateMessageID()
        {
            return "<" + Guid.NewGuid().ToString().Replace("-", "").Substring(16) + "@" + Guid.NewGuid().ToString().Replace("-", "").Substring(16) + ">";
        }
        #endregion

        #region GetMIMEBytes
        public static byte[] GetMIMEBytes(Uri SourceUri)
        {
            if (SourceUri.HostNameType == UriHostNameType.Basic)
            { //Local Files
                FileStream fs = File.OpenRead(SourceUri.LocalPath);
                byte[] bytes;
                try
                {
                    bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                }
                finally
                {
                    fs.Close();
                }
                return bytes;
            }
            else
            { //Remote Files over IP
                System.Net.HttpWebRequest objWebRequest;
                objWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(SourceUri.OriginalString);
                //objWebRequest.Timeout = 10;
                objWebRequest.Method = "GET";
                objWebRequest.ContentType = "text/html";

                System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objWebRequest.GetResponse();
                if (objResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    StreamReader sr = new StreamReader(objResponse.GetResponseStream());
                    string strText = sr.ReadToEnd();
                    sr.Close();
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strText);
                    return bytes;
                }
            }
            return null;
        }
        #endregion

    }
}
