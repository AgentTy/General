using System;
using System.Text;

namespace General.Debugging
{
	/// <summary>
	/// Summary description for Time.
	/// </summary>
	public class Time
	{
		private static string strGlobalClockKey = "General.Debugging.Time.GlobalClock";
        private static string strCustomClockKey = "General.Debugging.Time.CustomClock";
		private static string strDataClockKey = "General.Debugging.Time.DataClock";

		#region GlobalClock

		#region Start
		public static void StartGlobalClock()
		{
			if(System.Web.HttpContext.Current != null)
			{
				System.Web.HttpContext.Current.Items.Add(strGlobalClockKey, DateTime.Now);
                _intDataRequestCount = 0;
			}
		}
		#endregion

		#region Get
		public static TimeSpan GetGlobalTime()
		{
			TimeSpan processingTime = new TimeSpan(0);
			if(System.Web.HttpContext.Current != null)
			{
				if(System.Web.HttpContext.Current.Items.Contains(strGlobalClockKey))
					processingTime = DateTime.Now - (DateTime) System.Web.HttpContext.Current.Items[strGlobalClockKey];
				else
					throw new Exception("You must start the global clock at Application_BeginRequest to get the global processing time.");
			}
			return processingTime;
		}
		#endregion

		#endregion

        #region CustomClock

        #region Start/Stop
        public static void StartCustomClock()
		{
			if(System.Web.HttpContext.Current != null)
			{
				System.Web.HttpContext.Current.Items["temp_" + strCustomClockKey] = DateTime.Now;
                _intDataRequestCount++;
			}
		}

		public static TimeSpan StopCustomClock()
		{
            if (System.Web.HttpContext.Current != null)
            {
                DateTime dateStart = (DateTime)System.Web.HttpContext.Current.Items["temp_" + strCustomClockKey];
                DateTime dateEnd = DateTime.Now;
                TimeSpan timeAccumulated = new TimeSpan(0);
                if (System.Web.HttpContext.Current.Items[strCustomClockKey] != null)
                    timeAccumulated = (TimeSpan)System.Web.HttpContext.Current.Items[strCustomClockKey];

                TimeSpan newTime = dateEnd - dateStart;
                timeAccumulated += newTime;
                //Environment.Current.Trace("Data Clock: " + newTime.TotalMilliseconds + " ms");

                System.Web.HttpContext.Current.Items[strCustomClockKey] = timeAccumulated;
                return newTime;
            }
            return TimeSpan.MinValue;
		}
		#endregion

		#region Get
		public static TimeSpan GetCustomTime()
		{
			TimeSpan customTime = new TimeSpan(0);
			if(System.Web.HttpContext.Current != null)
			{
                if(System.Web.HttpContext.Current.Items[strCustomClockKey] != null)
                    customTime = (TimeSpan)System.Web.HttpContext.Current.Items[strCustomClockKey];
			}
            return customTime;
		}
        #endregion

        #endregion

		#region DataClock

		#region Start/Stop
        private static int _intDataRequestCount = 0;
		public static void StartDataClock()
		{
			if(System.Web.HttpContext.Current != null)
			{
				System.Web.HttpContext.Current.Items["temp_" + strDataClockKey] = DateTime.Now;
                _intDataRequestCount++;
			}
		}

		public static TimeSpan StopDataClock()
		{
            if (System.Web.HttpContext.Current != null)
            {
                DateTime dateStart = (DateTime)System.Web.HttpContext.Current.Items["temp_" + strDataClockKey];
                DateTime dateEnd = DateTime.Now;
                TimeSpan timeAccumulated = new TimeSpan(0);
                if (System.Web.HttpContext.Current.Items[strDataClockKey] != null)
                    timeAccumulated = (TimeSpan)System.Web.HttpContext.Current.Items[strDataClockKey];

                TimeSpan newTime = dateEnd - dateStart;
                timeAccumulated += newTime;
                //Environment.Current.Trace("Data Clock: " + newTime.TotalMilliseconds + " ms");

                System.Web.HttpContext.Current.Items[strDataClockKey] = timeAccumulated;
                return newTime;
            }
            return TimeSpan.MinValue;
		}
		#endregion

		#region Get
		public static TimeSpan GetDataTime()
		{
			TimeSpan processingTime = new TimeSpan(0);
			if(System.Web.HttpContext.Current != null)
			{
                if(System.Web.HttpContext.Current.Items[strDataClockKey] != null)
                    processingTime = (TimeSpan)System.Web.HttpContext.Current.Items[strDataClockKey];
			}
			return processingTime;
		}
		#endregion

		#endregion

		#region GetTimeReport
		public static string GetTimeReport(string strLineBreak)
		{
			StringBuilder sb = new StringBuilder();

			TimeSpan timeTotal = GetGlobalTime();
			TimeSpan timeData = GetDataTime();
			TimeSpan timeOther = timeTotal - timeData;
            TimeSpan timeCustom = GetCustomTime();

			sb.Append("<script language=\"javascript\">");
            sb.Append("try {$(document).ready(function() {debugWriteTransmissionTime();}); } catch(e) {}");
			sb.Append("</script>");
			sb.Append("<input type=\"hidden\" id=\"debugTransmissionStart\" value=\"" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond + "\">");
			sb.Append("<input type=\"hidden\" id=\"debugTotalServerTime\" value=\"" + timeTotal.Minutes + ":" + timeTotal.Seconds + ":" + timeTotal.Milliseconds + "\">");
			sb.Append("database trans: " + Math.Round(timeData.TotalMilliseconds,0) + " ms" + strLineBreak);
			sb.Append("server processing: " + Math.Round(timeOther.TotalMilliseconds,0) + " ms" + strLineBreak);
            if (timeCustom.TotalMilliseconds > 0)
                sb.Append("custom time: " + Math.Round(timeOther.TotalMilliseconds, 0) + " ms" + strLineBreak);
			sb.Append("transmission: <label id=\"debugTransmissionTime\"></label> ms" + strLineBreak);
			sb.Append("total time: <label id=\"debugTotalTime\"></label> ms" + strLineBreak);
			return sb.ToString();
		}

        public static string GetTimeReportJavascript()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\">");
            sb.Append(GetTimeAjax());
            sb.Append("</script>");
            return sb.ToString();
        }

		public static string GetTimeAjax() {
			TimeSpan timeTotal = GetGlobalTime();
			TimeSpan timeData = GetDataTime();
			TimeSpan timeOther = timeTotal - timeData;
            TimeSpan timeCustom = GetCustomTime();

			StringBuilder sb = new StringBuilder();

			//sb.Append("<script language=\"javascript\">");
            sb.Append("debugShowTimeReport(");

            sb.Append("\"" + Math.Round(timeData.TotalMilliseconds, 0) + "\", "); // Database
            sb.Append("\"" + _intDataRequestCount + "\", "); // Database Requests
            sb.Append("\"" + Math.Round(timeOther.TotalMilliseconds, 0) + "\", "); // Server
			sb.Append("\"" + ParseTimeString(DateTime.Now) + "\", "); // Transmission Start
			sb.Append("\"" + ParseTimeString(timeTotal) + "\", "); // Total
            sb.Append("\"" + ParseTimeString(timeCustom) + "\" "); // Custom
			//sb.Append("\"timestamp\"");

			sb.Append(");");
            //sb.Append("TestFunction();");
			//sb.Append("</script>");
			
			return sb.ToString();
		}

        public static System.Collections.Specialized.NameValueCollection GetTimeHTTPHeaders()
        {
            TimeSpan timeTotal = GetGlobalTime();
            TimeSpan timeData = GetDataTime();
            TimeSpan timeOther = timeTotal - timeData;
            TimeSpan timeCustom = GetCustomTime();

            System.Collections.Specialized.NameValueCollection objHeaders = new System.Collections.Specialized.NameValueCollection();
            objHeaders.Add("Debug-Database-Time",Math.Round(timeData.TotalMilliseconds, 0).ToString());
            objHeaders.Add("Debug-Database-Requests", _intDataRequestCount.ToString());
            objHeaders.Add("Debug-Server-Time", Math.Round(timeOther.TotalMilliseconds, 0).ToString());
            if(timeCustom.TotalMilliseconds > 0)
                objHeaders.Add("Debug-Custom-Time", Math.Round(timeCustom.TotalMilliseconds, 0).ToString());
            objHeaders.Add("Debug-Transmission-Start", ParseTimeString(DateTime.Now));
            objHeaders.Add("Debug-Total-Time", ParseTimeString(timeTotal));
            return objHeaders;
        }

		#region ParseTimeString
		private static string ParseTimeString(TimeSpan time) {
			StringBuilder sb = new StringBuilder();
			
			sb.Append(time.Minutes + ":");
			sb.Append(time.Seconds + ":");
			sb.Append(time.Milliseconds);
			
			return sb.ToString();
		}
		
		private static string ParseTimeString(DateTime date) {
			StringBuilder sb = new StringBuilder();
			
			sb.Append(date.Minute + ":");
			sb.Append(date.Second + ":");
			sb.Append(date.Millisecond);
			
			return sb.ToString();
		}
		#endregion

		#endregion

	}
}
