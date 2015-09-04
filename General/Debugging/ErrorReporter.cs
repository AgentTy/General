using System;
using System.Text;
using System.Web;

namespace General.Debugging 
{
	/// <summary>
	/// Summary description for ErrorReporter.
	/// </summary>
	public class ErrorReporter 
    {
      
		#region GetErrorReport
		public static StringBuilder GetErrorReport(Exception e, string strLineBreak) {
			HttpContext _context = HttpContext.Current;
			StringBuilder sb = new StringBuilder();
			sb.Append(DateTime.Now.ToString() + strLineBreak);

			if(_context != null)
				sb.Append("URL: " + General.Web.WebTools.GetRequestedUrl() + strLineBreak);

            sb.Append("BASE EXCEPTION: " + e.GetBaseException() + strLineBreak);
			sb.Append("SOURCE: " +e.Source + strLineBreak);
			sb.Append("MESSAGE: " + SanitizeString(e.Message) + strLineBreak);
            if (e.InnerException != null && e.InnerException.ToString() != e.GetBaseException().ToString())
                sb.Append("INNER ERROR: " + SanitizeString(e.InnerException.ToString()) + strLineBreak);
			//sb.Append("TARGET SITE NAME: " + e.TargetSite.Name.ToString() + strLineBreak);
            if (!StringFunctions.IsNullOrWhiteSpace(e.StackTrace))
                sb.Append("STACK TRACE: " + SanitizeString(e.StackTrace) + strLineBreak);
			sb.Append("" + strLineBreak);			
			sb.Append("" + strLineBreak);

			#region DATABASE CALLS
            /*
			sb.Append("DATABASE CALLS:" + strLineBreak);
			if (_context != null) {
				string strDataHistory = DAO.SQLCache.GetTransactionHistory(strLineBreak);
				sb.Append(strDataHistory + strLineBreak);
			} else {
				sb.Append("Unavailable" + strLineBreak);
			}
            */
			#endregion

			if (_context != null) {
                try
                {
                    sb.Append(GetContextReport(strLineBreak));
                }
                catch (Exception ex)
                {
                    sb.Append("An error occurred while trying to generate the HTTPContext portion of this report." + strLineBreak);
                    sb.Append("Message: " + ex.Message + strLineBreak);
                    sb.Append("Stack: " + ex.StackTrace + strLineBreak);
                    sb.Append("" + strLineBreak);
                    sb.Append("" + strLineBreak);
                }
			}

			return sb;
		}
    
		#endregion

		#region GetContextReport
		public static string GetContextReport() {
			return GetContextReport("<br>\r\n");
		}

		public static string GetContextReport(string strLineBreak) {
			HttpContext _context = HttpContext.Current;
			StringBuilder sb = new StringBuilder();

			#region HTTP REQUEST
            try
            {
                if (_context.Request != null)
                {
                    try
                    {
                        sb.Append("REQUEST DUMP:" + strLineBreak);
                        foreach (string strKey in _context.Request.Form.Keys)
                        {
                            if (strKey != "__VIEWSTATE")
                                sb.Append(strKey + " = " + _context.Request.Form[strKey].ToString() + strLineBreak);
                        }
                        sb.Append("" + strLineBreak);
                    }
                    catch (Exception ex)
                    {
                        sb.Append("Couldn't dump REQUEST." + strLineBreak + ex.ToString());
                    }

                    #region HTTP COOKIES
                    sb.Append("COOKIES DUMP (EXCLUDING BINARY OBJECTS):" + strLineBreak);
                    if (_context.Request.Cookies != null)
                    {
                        try
                        {
                            foreach (string strKey in _context.Request.Cookies.Keys)
                            {
                                if (strKey != string.Empty && !strKey.ToLower().EndsWith("object"))
                                    sb.Append(strKey + " = " + _context.Request.Cookies[strKey].Value + strLineBreak);
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.Append("Couldn't dump COOKIE." + strLineBreak + ex.ToString());
                        }
                    }
                    sb.Append(strLineBreak);
                    #endregion

                    #region SERVER VARIABLES
                    sb.Append("SERVER VARIABLES:" + strLineBreak);
                    if (_context.Request.ServerVariables != null)
                    {
                        try
                        {
                            foreach (string strKey in _context.Request.ServerVariables.Keys)
                            {
                                switch (strKey.ToLower())
                                {
                                    case "cookie":
                                    case "http_cookie":
                                    case "all_raw":
                                    case "all_http":
                                        //DO NOTHING
                                        break;
                                    default:
                                        sb.Append(strKey + " = " + _context.Request.ServerVariables[strKey].ToString() + strLineBreak);
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.Append("Couldn't dump SERVER VARIABLES." + strLineBreak + ex.ToString());
                        }
                    }
                    sb.Append(strLineBreak);
                    #endregion
                }
            }
            catch(Exception ex)
            { sb.Append("REQUEST DUMP: " + ex.Message + strLineBreak); }
			#endregion

			#region ASP.NET SESSION
            try
            {
                if (_context.Session != null)
                {
                    try
                    {
                        sb.Append("SESSION DUMP:" + strLineBreak);
                        foreach (string strKey in _context.Session.Keys)
                        {
                            if (_context.Session[strKey] != null)
                                sb.Append(strKey + " = " + _context.Session[strKey].ToString() + strLineBreak);
                            else
                                sb.Append(strKey + " = " + "NULL" + strLineBreak);
                        }
                        sb.Append("" + strLineBreak);
                    }
                    catch (Exception ex)
                    {
                        sb.Append("Couldn't dump SESSION." + strLineBreak + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            { sb.Append("SESSION DUMP: " + ex.Message + strLineBreak); }
			#endregion

            return SanitizeString(sb.ToString());
		}
		#endregion

        #region SanitizeString
        private static System.Text.RegularExpressions.Regex rgxEncapsulatedPassword = new System.Text.RegularExpressions.Regex(@"PASSWORD=[""'].*[""']|PWD=[""'].*[""']|PASS=[""'].*[""']", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
        private static System.Text.RegularExpressions.Regex rgxSQLPassword = new System.Text.RegularExpressions.Regex(@"PASSWORD=[^;\n\r]*|PWD=[^;\n\r]*", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
        private static System.Text.RegularExpressions.Regex rgxFinalPassword = new System.Text.RegularExpressions.Regex(@"PASSWORD[^;\n\r]*|PWD[^;\n\r]*|PASS[^;\n\r]*", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
        public static string SanitizeString(string s)
        {
            if (StringFunctions.IsNullOrWhiteSpace(s))
                return "";

            string sClean = s;
            sClean = rgxEncapsulatedPassword.Replace(sClean, "REDACTED");
            sClean = rgxSQLPassword.Replace(sClean, "REDACTED;");
            sClean = rgxFinalPassword.Replace(sClean, "REDACTED");
            return sClean;
        }
        #endregion

    }
}
