using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General
{

    public class Bugger: Debug
    {

    }

    public class Debug
    {

        #region Write
        public static void Write(string strMessage)
        {
            Trace(strMessage);
            JQueryDebugWrite(strMessage);
        }
        #endregion

        #region JQueryDebugWrite
        //This must be implimented in every page via MasterPage or Global.asax
        public static string DebugJavascriptQueue = "";
        public static void JQueryDebugWrite(string strMessage)
        {
            DebugJavascriptQueue += "$(document).ready(function() {if($.log){$.log('" + StringFunctions.JSEncode(strMessage) + "');}});";
        }
        #endregion

        #region Trace
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

        /// <summary>
        /// Attempts to write a trace line
        /// </summary>
        public static void Trace(System.Data.SqlClient.SqlCommand objCommand)
        {
            if (TraceEnabled)
                if (System.Web.HttpContext.Current != null)
                    System.Web.HttpContext.Current.Trace.Write(General.DAO.SqlHelper.GetQueryString(objCommand));
        }

        #endregion

    }
}
