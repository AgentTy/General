using System;
using System.Text;

namespace General.Debugging 
{
	/// <summary>
	/// Summary description for ErrorReporter.
	/// </summary>
	public class ErrorReporter 
    {
      
		#region GetErrorReport
		public static StringBuilder GetErrorReport(Exception e, string strLineBreak) {
			
			StringBuilder sb = new StringBuilder();
			sb.Append("TIME: " + DateTimeOffset.UtcNow.ToString() + strLineBreak);

#if NET472 || NET471 || NET47 || NET462 || NET461 || NET46 || NET452 || NET451 || NET45 || NET40 || NET35 || NET20
            HttpContext _context = HttpContext.Current;
            if (_context != null)
				sb.Append("URL: " + General.Web.WebTools.GetRequestedUrl() + strLineBreak);
#endif

            sb.Append("SOURCE: " + e.Source + strLineBreak);
            sb.Append("MESSAGE: " + SanitizeString(e.Message) + strLineBreak + strLineBreak);

            sb.Append("BASE EXCEPTION: " + e.GetBaseException() + strLineBreak);
            if (e.InnerException != null && e.InnerException.ToString() != e.GetBaseException().ToString())
                sb.Append("INNER EXCEPTION: " + SanitizeString(e.InnerException.ToString()) + strLineBreak);

            //Pretty sure this is a waste of CPU
            if (!String.IsNullOrWhiteSpace(e.StackTrace) && !sb.ToString().Contains(e.StackTrace))
                sb.Append("STACK TRACE: " + SanitizeString(e.StackTrace) + strLineBreak);

			sb.Append("" + strLineBreak);			
			sb.Append("" + strLineBreak);

#if NET472 || NET471 || NET47 || NET462 || NET461 || NET46 || NET452 || NET451 || NET45 || NET40 || NET35 || NET20
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
#endif

			return sb;
		}
    
#endregion

        #region SanitizeString
        private static System.Text.RegularExpressions.Regex rgxEncapsulatedPassword = new System.Text.RegularExpressions.Regex(@"PASSWORD=[""'].*[""']|PWD=[""'].*[""']|PASS=[""'].*[""']", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
        private static System.Text.RegularExpressions.Regex rgxSQLPassword = new System.Text.RegularExpressions.Regex(@"PASSWORD=[^;\n\r]*|PWD=[^;\n\r]*", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
        private static System.Text.RegularExpressions.Regex rgxFinalPassword = new System.Text.RegularExpressions.Regex(@"PASSWORD[^;\n\r]*|PWD[^;\n\r]*|PASS[^;\n\r]*", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
        private static System.Text.RegularExpressions.Regex rgxFlexToken = new System.Text.RegularExpressions.Regex(@"""flexToken[^;\n\r]*", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
        public static string SanitizeString(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
                return "";

            string sClean = s;
            sClean = rgxEncapsulatedPassword.Replace(sClean, "REDACTED");
            sClean = rgxSQLPassword.Replace(sClean, "REDACTED;");
            sClean = rgxFinalPassword.Replace(sClean, "REDACTED");
            sClean = rgxFlexToken.Replace(sClean, "REDACTED");
            return sClean;
        }
        #endregion

    }
}
