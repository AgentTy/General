using System;
using General;

using System.Text;
using System.IO;

namespace General
{
	/// <summary>
	/// Summary description for RootFunctions.
	/// </summary>
	public class F:StringFunctions
	{

		#region Constructors

		private F()
		{

		}

		#endregion

		#region StringNull
		/// <summary>
		/// Returns true is a string is null or empty
		/// </summary>
		public static bool StringNull(string strValue)
		{
			if(strValue == null)
				return true;
			if(strValue == String.Empty)
				return true;

			return false;
		}

        public static bool StringNull(object strValue)
        {
            if (strValue == null)
                return true;
            if (strValue.ToString() == String.Empty)
                return true;

            return false;
        }
		#endregion

		#region MakeScript
		public static string MakeScript(string strScript)
		{
			strScript = "<script language=\"javascript\" type=\"text/javascript\">\n" + strScript + "</script>\n";
			return strScript;
		}
		#endregion

        #region ExecuteJavascriptNoCache
        //This must be implimented in every page via MasterPage or Global.asax
        /*
        public static string ExecuteJavascriptQueue = "";
        public static void ExecuteJavascriptNoCache(string strScript)
        {
            ExecuteJavascriptQueue += strScript;
        }
        */
        #endregion

		#region DoDownload
        /*
		public static void DoDownload(string strFileName)
		{
			
			string buffer = "";
			buffer += "<script language=\"javascript\">\n";
			buffer += "function DoDownload()";
			buffer += "{";
			buffer += "document.location = 'download.aspx?file=" + strFileName + "';\n";
			buffer += "}";
			buffer += "</script>";
			System.Web.HttpContext.Current.Response.Write(buffer);
			
			//System.Web.HttpContext.Current.Response.TransmitFile(strFileName);
		}
        */
		#endregion

		#region NewLine
		public static string NewLine
		{
			get {return "\r\n";}
		}
		#endregion

		#region Tab Code
		public static int _intTabCount = 0;
		public static string Tab
		{	
			get { return StringFunctions.RepeatString("	",_intTabCount); }
		}

		public static string TabUp
		{
			get 
			{
				_intTabCount++;
				return Tab; 
			}
		}

		public static string TabUpAfter
		{
			get 
			{
				string strTab = Tab;
				_intTabCount++;
				return strTab; 
			}
		}

		public static string TabDown
		{
			get 
			{ 
				_intTabCount--;
				if(_intTabCount < 0)
					_intTabCount = 0;
				return Tab;
			}
		}

		public static string TabDownAfter
		{
			get 
			{ 
				string strTab = Tab;
				_intTabCount--;
				if(_intTabCount < 0)
					_intTabCount = 0;
				return strTab;
			}
		}

        public static void TabSet(int intTabCount)
        {
            _intTabCount = intTabCount;
        }

		public static void TabReset()
		{
			_intTabCount = 0;
		}
		#endregion

        #region CheckChange

        #region Int
        public static bool CheckChange(int Check, out int Out, int Before)
        {
            if (Before != Check)
            {
                Out = Check;
                return true;
            }
            else
            {
                Out = Before;
                return false;
            }
        }

        private static int intBefore;
        public static bool CheckChange(int Check, out int Out)
        {
            if (intBefore != Check)
            {
                Out = Check;
                intBefore = Check;
                return true;
            }
            else
            {
                Out = intBefore;
                return false;
            }
        }
        #endregion

        #region String
        public static bool CheckChange(string Check, out string Out, string Before)
        {
            if (Before != Check)
            {
                Out = Check;
                return true;
            }
            else
            {
                Out = Before;
                return false;
            }
        }

        private static string strBefore;
        public static bool CheckChange(string Check, out string Out)
        {
            if (strBefore != Check)
            {
                Out = Check;
                strBefore = Check;
                return true;
            }
            else
            {
                Out = strBefore;
                return false;
            }
        }
        #endregion
        
        #endregion

        #region StandardizeName / JSFormat
        /// <summary>
        /// Replaces : and $ with _
        /// </summary>
        /// <returns>string</returns>
        public static string StandardizeName(string strSubject)
        {
            strSubject = strSubject.Replace(":", "_");
            strSubject = strSubject.Replace("$", "_");
            return strSubject;
        }

        /// <summary>
        /// Replaces : with _
        /// </summary>
        public static string JSFormat(string input)
        {
            return (StandardizeName(input));
        }

        #endregion

    }
}
