using System;
using General;
using System.Threading;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace General.Debugging
{
	/// <summary>
	/// Summary description for DeathChecker.
	/// </summary>
	public class DeathChecker
	{

		#region Private Variables
		private int intTestInterval;
        private int intTestIntervalFast;
		private int intBackupInterval;
		private int intSiteFailureThreshold;
        private int intSMTPServerFailureThreshold;
        private int intRunningReminderEveryXDays;
        private int intRunningReminderTargetHour;
        private int intRunningReminderTargetMinute;

		private List<string> arySites = new List<string>();
        private DateTime dtSitesLoaded = DateTime.MinValue;
        private DateTime dtPhonedHome = DateTime.MinValue;
        private bool[] arySiteNotifyOnSuccess;
		private bool[] arySiteNotifyOnFailure;
		private int[] arySiteFailureCount;

        private string[] arySMTPServers;
        private bool[] arySMTPNotifyOnSuccess;
        private bool[] arySMTPNotifyOnFailure;
        private int[] arySMTPFailureCount;

		private bool boolStop;
		private DeathCheckerMessageDelegate Listeners;
		#endregion

		#region Constructors

		public DeathChecker()
		{

		}

		#endregion

        #region DeathCheckerMessageDelegate
        public delegate void DeathCheckerMessageDelegate(string strMessage, EventLogEntryType objType);
		#endregion

		#region Control Methods

		public void Start()
		{
			if(boolStop)
				boolStop = false;
			else
				ThreadPool.QueueUserWorkItem(new WaitCallback(PingSites));
		}

		public void Stop()
		{
			boolStop = true;
		}

		public void AddListener(DeathCheckerMessageDelegate objListener)
		{
			Listeners += objListener;
		}
		#endregion

		#region PingSites
        bool blnErrorsFound;
		private void PingSites(Object stateInfo)
		{
			try
            {

                #region Load Settings
                try
                {
                    intTestInterval = int.Parse(General.Configuration.GlobalConfiguration.GlobalSettings["test_interval"]);
                }
                catch { intTestInterval = 60; }
                try
                {
                    intTestIntervalFast = int.Parse(General.Configuration.GlobalConfiguration.GlobalSettings["test_interval_fast"]);
                }
                catch { intTestIntervalFast = intTestInterval/2; }
				intBackupInterval = intTestInterval;
                try
                {
                    intSiteFailureThreshold = int.Parse(General.Configuration.GlobalConfiguration.GlobalSettings["site_failure_threshold"]);
                }
                catch { intSiteFailureThreshold = 1; }
                try
                {
                    intRunningReminderEveryXDays = int.Parse(General.Configuration.GlobalConfiguration.GlobalSettings["RunningReminderEveryXDays"]);
                    intRunningReminderTargetHour = int.Parse(General.Configuration.GlobalConfiguration.GlobalSettings["RunningReminderTargetHour"]);
                    intRunningReminderTargetMinute = int.Parse(General.Configuration.GlobalConfiguration.GlobalSettings["RunningReminderTargetMinute"]);
                }
                catch 
                {
                    intRunningReminderEveryXDays = 31;
                    intRunningReminderTargetHour = 15;
                    intRunningReminderTargetMinute = 0;
                }

                #region Web Sites
                GetSiteList();
                #endregion

                #region SMTP Servers
                intSMTPServerFailureThreshold = int.Parse(General.Configuration.GlobalConfiguration.GlobalSettings["smtp_server_failure_threshold"]);
                if (!String.IsNullOrEmpty(General.Configuration.GlobalConfiguration.GlobalSettings["smtp_servers_to_check"]))
                    arySMTPServers = General.Configuration.GlobalConfiguration.GlobalSettings["smtp_servers_to_check"].Split(',');
                else
                    arySMTPServers = new string[0];
                arySMTPNotifyOnSuccess = new bool[arySMTPServers.Length];
                for (int inc = 0; inc < arySMTPNotifyOnSuccess.Length; inc++)
                    arySMTPNotifyOnSuccess[inc] = false;
                arySMTPNotifyOnFailure = new bool[arySMTPServers.Length];
                for (int inc = 0; inc < arySMTPNotifyOnFailure.Length; inc++)
                    arySMTPNotifyOnFailure[inc] = true;
                arySMTPFailureCount = new int[arySMTPServers.Length];
                for (int inc = 0; inc < arySMTPFailureCount.Length; inc++)
                    arySMTPFailureCount[inc] = 0;
                #endregion

                #endregion

                Listeners("DeathChecker Started",EventLogEntryType.Information);

				while(!boolStop)
                {
                    GetSiteList();

                    blnErrorsFound = false; //Reset

                    #region Check Web Sites
                    for (int inc = 0; inc < arySites.Count; inc++)
					{
						bool boolNotifyOnSuccess = arySiteNotifyOnSuccess[inc];
						bool boolNotifyOnFailure = arySiteNotifyOnFailure[inc];

						try
						{
							Listeners("REQUESTING URL " + arySites[inc],EventLogEntryType.Information);
                            System.Net.HttpStatusCode enuResponseCode;
                            string strResponseDescription;
                            string strContent = General.Web.WebTools.GetUrl(arySites[inc], out enuResponseCode, out strResponseDescription);
                            strContent = strContent.ToLowerInvariant();

                            if (enuResponseCode == System.Net.HttpStatusCode.OK)
                            {
                                if (strContent.Contains("finder.cox.net"))
                                {
                                    SiteTestFail(inc,"Connection Failed", "ISP Redirected");
                                }
                                else if (strContent.Contains("error.aspx"))
                                {
                                    SiteTestFail(inc, "Site Error", "Redirected To Error Page");
                                }
                                else if (strContent.Contains("404.aspx"))
                                {
                                    SiteTestFail(inc, "404 Error", "Redirected To 404 Page");
                                }
                                else
                                {
                                    SiteTestSuccess(inc);
                                }
                            }
                            else
                            {
                                SiteTestFail(inc, "Bad Response", enuResponseCode.ToString() + ": " + strResponseDescription);
                            }
						}
						catch(Exception e)
                        {
                            if (e.Message.Contains("404"))
                                SiteTestFail(inc, "404 Error", "");
                            else if (e.Message.Contains("500"))
                                SiteTestFail(inc, "500 Error", e.Message);
                            else
                                SiteTestFail(inc, "Connection Failed", e.Message);

                            #region Error
                            /*
                            Listeners("FAILURE: " + DateTime.Now.ToString() + "\r\n" + arySites[inc] + "\r\n" + e.Message,EventLogEntryType.Warning);
							arySiteFailureCount[inc]++;
							intTestInterval = intTestIntervalFast;
							if(arySiteNotifyOnFailure[inc] && arySiteFailureCount[inc] >= intSiteFailureThreshold)
							{
								Listeners("NOTIFYING...",EventLogEntryType.Information);
								arySiteNotifyOnFailure[inc] = false;
								arySiteNotifyOnSuccess[inc] = true;
								try
								{
                                    #region Generate Message
                                    string strMessage;
                                    if (e.Message.Contains("404"))
                                        strMessage = "404 Error, Site " + arySites[inc];
                                    else
                                        strMessage = "Connection Failed, Site " + arySites[inc] + " Offline";
                                    #endregion

                                    Listeners(strMessage, EventLogEntryType.FailureAudit);
                                    Notify(strMessage);
								}
								catch(Exception ex)
								{
									Listeners("Mail Failure: " + ex.ToString(),EventLogEntryType.Error);
								}
                            }
                            */
                            #endregion
                        }
					} //END FOR EACH
                    #endregion

                    #region Check SMTP Servers
                    for (int inc = 0; inc < arySMTPServers.Length; inc++)
                    {
                        bool boolNotifyOnSuccess = arySMTPNotifyOnSuccess[inc];
                        bool boolNotifyOnFailure = arySMTPNotifyOnFailure[inc];

                        try
                        {
                            Listeners("TESTING SMTP SERVER " + arySMTPServers[inc], EventLogEntryType.Information);

                            string strServer = arySMTPServers[inc];
                            int intPort = 25;
                            if (strServer.Contains(":"))
                            {
                                intPort = int.Parse(StringFunctions.AllAfter(strServer, ":"));
                                strServer = StringFunctions.AllBefore(strServer, ":");
                            }
                            var result = General.Mail.SMTPTest.TestMailServer(strServer, intPort, false);
                            if (!result.Found)
                            {
                                throw new Exception(result.Response);
                            }
                            Listeners("SUCCESS: " + DateTime.Now.ToString(), EventLogEntryType.Information);
                            arySMTPFailureCount[inc] = 0;
                            if (arySMTPNotifyOnSuccess[inc])
                            {
                                Listeners("NOTIFYING...", EventLogEntryType.Information);
                                arySMTPNotifyOnFailure[inc] = true;
                                arySMTPNotifyOnSuccess[inc] = false;
                                try
                                {
                                    Listeners("Connection Succeeded, SMTP Server " + arySMTPServers[inc] + " Online", EventLogEntryType.SuccessAudit);
                                    Notify("Connection Succeeded, SMTP Server " + arySMTPServers[inc] + " Online");
                                }
                                catch (Exception ex)
                                {
                                    Listeners("Mail Failure: " + ex.ToString(), EventLogEntryType.Error);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            blnErrorsFound = true;
                            Listeners("FAILURE: " + DateTime.Now.ToString() + "\r\n" + arySMTPServers[inc] + "\r\n" + e.Message, EventLogEntryType.Warning);
                            arySMTPFailureCount[inc]++;
                            if (arySMTPNotifyOnFailure[inc] && arySMTPFailureCount[inc] >= intSMTPServerFailureThreshold)
                            {
                                Listeners("NOTIFYING...", EventLogEntryType.Information);
                                arySMTPNotifyOnFailure[inc] = false;
                                arySMTPNotifyOnSuccess[inc] = true;
                                try
                                {
                                    Listeners("Connection Failed, SMTP Server " + arySMTPServers[inc] + " Offline", EventLogEntryType.FailureAudit);
                                    Notify("Connection Failed, SMTP Server " + arySMTPServers[inc] + " Offline");
                                }
                                catch (Exception ex)
                                {
                                    Listeners("Mail Failure: " + ex.ToString(), EventLogEntryType.Error);
                                }
                            }
                        }
                    } //END FOR EACH
                    #endregion

                    #region Periodically Phone Home So You Know I'm Out There
                    if (dtPhonedHome == DateTime.MinValue || (DateTime.Now - dtPhonedHome) > TimeSpan.FromDays(intRunningReminderEveryXDays))
                    {
                        DateTime dtTargetTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, intRunningReminderTargetHour, intRunningReminderTargetMinute, 0); //9:00 PM
                        if (DateTime.Now >= dtTargetTime || (dtTargetTime - DateTime.Now < TimeSpan.FromMinutes(intTestInterval)))
                        {
                            PhoneHome();
                            dtPhonedHome = DateTime.Now;
                        }
                    }
                    #endregion

                    if (blnErrorsFound)
                        intTestInterval = intTestIntervalFast;
                    else
                        intTestInterval = intBackupInterval;

                    Thread.Sleep(new TimeSpan(0,0,intTestInterval,0,0)); //Pause
				} //WEND

				boolStop = false;
				Listeners("DeathChecker Stopped",EventLogEntryType.Information);
			}
			catch(Exception ex)
			{
				Listeners(ex.ToString(),EventLogEntryType.Error);
			}
		}
		#endregion

        #region Site Status Functions

        #region SiteTestSuccess
        private void SiteTestSuccess(int inc)
        {
            Listeners("SUCCESS: " + DateTime.Now.ToString(), EventLogEntryType.Information);
            arySiteFailureCount[inc] = 0;
            if (arySiteNotifyOnSuccess[inc])
            {
                Listeners("NOTIFYING...", EventLogEntryType.Information);
                arySiteNotifyOnFailure[inc] = true;
                arySiteNotifyOnSuccess[inc] = false;
                try
                {
                    Listeners("Connection Succeeded, Site " + arySites[inc] + " Online", EventLogEntryType.SuccessAudit);
                    Notify("Connection Succeeded, Site " + arySites[inc] + " Online");
                }
                catch (Exception ex)
                {
                    Listeners("Mail Failure: " + ex.ToString(), EventLogEntryType.Error);
                }
            }
        }
        #endregion

        #region SiteTestFail
        private void SiteTestFail(int inc, string strFailType, string strFailDetail)
        {
            Listeners(strFailType + ": " + DateTime.Now.ToString() + "\r\n" + arySites[inc] + "\r\n" + strFailDetail, EventLogEntryType.Warning);
            arySiteFailureCount[inc]++;
            blnErrorsFound = true;
            if (arySiteNotifyOnFailure[inc] && arySiteFailureCount[inc] >= intSiteFailureThreshold)
            {
                Listeners("NOTIFYING...", EventLogEntryType.Information);
                arySiteNotifyOnFailure[inc] = false;
                arySiteNotifyOnSuccess[inc] = true;
                try
                {
                    #region Generate Message
                    string strMessage;
                    if(!string.IsNullOrEmpty(strFailDetail))
                        strMessage = strFailType + ", " + arySites[inc] + ", " + strFailDetail;
                    else
                        strMessage = strFailType + ", " + arySites[inc] + "";
                    #endregion

                    Listeners(strMessage, EventLogEntryType.FailureAudit);
                    Notify(strMessage);
                }
                catch (Exception ex)
                {
                    Listeners("Mail Failure: " + ex.ToString(), EventLogEntryType.Error);
                }
            }
        }
        #endregion

        #endregion

        #region PhoneHome
        private void PhoneHome()
        {
            string strMessage = "Death Checker online watching " + arySites.Count + " sites";

            bool blnHeaderDone = false;
            List<string> lstOfflineSites = GetOfflineSiteList();
            foreach (string strSite in lstOfflineSites)
            {
                if (!blnHeaderDone)
                {
                    strMessage += "\r\n" + "Down Sites: " + lstOfflineSites.Count;
                    blnHeaderDone = true;
                }
                strMessage += "\r\n" + strSite;
            }

            Notify(strMessage);
        }
        #endregion

        #region Notify
        private void Notify(string Message)
		{
			string[] aryRecipients = General.Configuration.GlobalConfiguration.GlobalSettings["people_to_notify"].Split(',');
			
			foreach(string strRecipient in aryRecipients)
			{
                try
                {
                    Mail.MailTools.SendEmail(General.Configuration.GlobalConfiguration.GlobalSettings["debug_email_from"], strRecipient, "Site Alert", Message);
                }
                catch(Exception ex)
                {
                    Listeners("Error sending email: " + ex.Message, EventLogEntryType.Error);
                }
			}
		}
		#endregion

        #region GetSiteList
        private List<string> GetSiteList()
        {
            if (arySites == null || arySites.Count == 0 || dtSitesLoaded == DateTime.MinValue || dtSitesLoaded < DateTime.Now.AddHours(-24))
            {
                #region Phone Home If There Are Problems
                if(arySites != null && arySites.Count > 0)
                    if (GetOfflineSiteList().Count > 0)
                        PhoneHome();
                #endregion

                arySites = new List<string>();

                #region Check Config File
                if (!String.IsNullOrEmpty(General.Configuration.GlobalConfiguration.GlobalSettings["sites_to_check"]))
                {
                    string[] aryTempSiteList = General.Configuration.GlobalConfiguration.GlobalSettings["sites_to_check"].Split(',');
                    arySites.AddRange(aryTempSiteList);
                }
                #endregion

                #region Check Database
                string strConnection = String.Empty;
                try
                {
                    strConnection = General.DAO.DBConnection.GetConnectionString();
                }
                catch { }

                if (!string.IsNullOrEmpty(strConnection))
                {
                    try
                    {
                        using (System.Data.SqlClient.SqlConnection objConn = new System.Data.SqlClient.SqlConnection(strConnection))
                        {
                            SqlCommand cmd;
                            cmd = new SqlCommand("[dbo].[pr_DeathChecker_SelectSiteList]");
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 300;
                            cmd.Connection = objConn;

                            DataTable objTable = General.DAO.SqlHelper.ExecuteDataset(cmd).Tables[0];
                            foreach (DataRow objRow in objTable.Rows)
                            {
                                if (objRow.Table.Columns.Contains("TestURL"))
                                    if (objRow["TestURL"] != DBNull.Value)
                                        if (!String.IsNullOrEmpty((string)objRow["TestURL"]))
                                            if (!arySites.Contains((string)objRow["TestURL"]))
                                                arySites.Add((string)objRow["TestURL"]);

                                if (objRow.Table.Columns.Contains("TestURL1"))
                                    if (objRow["TestURL1"] != DBNull.Value)
                                        if (!String.IsNullOrEmpty((string)objRow["TestURL1"]))
                                            if (!arySites.Contains((string)objRow["TestURL1"]))
                                                arySites.Add((string)objRow["TestURL1"]);

                                if (objRow.Table.Columns.Contains("TestURL2"))
                                    if (objRow["TestURL2"] != DBNull.Value)
                                        if (!String.IsNullOrEmpty((string)objRow["TestURL2"]))
                                            if (!arySites.Contains((string)objRow["TestURL2"]))
                                                arySites.Add((string)objRow["TestURL2"]);

                                if (objRow.Table.Columns.Contains("TestURL3"))
                                    if (objRow["TestURL3"] != DBNull.Value)
                                        if (!String.IsNullOrEmpty((string)objRow["TestURL3"]))
                                            if (!arySites.Contains((string)objRow["TestURL3"]))
                                                arySites.Add((string)objRow["TestURL3"]);

                                if (objRow.Table.Columns.Contains("TestURL4"))
                                    if (objRow["TestURL4"] != DBNull.Value)
                                        if (!String.IsNullOrEmpty((string)objRow["TestURL4"]))
                                            if (!arySites.Contains((string)objRow["TestURL4"]))
                                                arySites.Add((string)objRow["TestURL4"]);

                                if (objRow.Table.Columns.Contains("TestURL5"))
                                    if (objRow["TestURL5"] != DBNull.Value)
                                        if (!String.IsNullOrEmpty((string)objRow["TestURL5"]))
                                            if (!arySites.Contains((string)objRow["TestURL5"]))
                                                arySites.Add((string)objRow["TestURL5"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Listeners("DB Check Failure: " + ex.ToString(), EventLogEntryType.Error);
                        Report.SendError("DeathChecker Error", ex);
                    }
                }
                #endregion

                #region Reset Variables
                arySiteNotifyOnSuccess = new bool[arySites.Count + 500];
                for (int inc = 0; inc < arySiteNotifyOnSuccess.Length; inc++)
                    arySiteNotifyOnSuccess[inc] = false;
                arySiteNotifyOnFailure = new bool[arySites.Count + 500];
                for (int inc = 0; inc < arySiteNotifyOnFailure.Length; inc++)
                    arySiteNotifyOnFailure[inc] = true;
                arySiteFailureCount = new int[arySites.Count + 500];
                for (int inc = 0; inc < arySiteFailureCount.Length; inc++)
                    arySiteFailureCount[inc] = 0;
                #endregion

                dtSitesLoaded = DateTime.Now;
            }
            return arySites;
        }
        #endregion

        #region GetOfflineSiteList
        private List<string> GetOfflineSiteList()
        {
            List<string> lstSites = new List<string>();
            for (int i = 0; i < arySiteNotifyOnFailure.Length; i++)
            {
                if (arySiteNotifyOnFailure[i] == false)
                {
                    lstSites.Add(arySites[i]);
                }
            }
            return lstSites;
        }
        #endregion

    }
}
