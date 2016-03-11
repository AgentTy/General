using System;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using System.Web;

namespace General.Web {
	/// <summary>
	/// Utilities related to the Web
	/// </summary>
	public class WebTools {

		#region Constructor
		private WebTools() {} // All methods are static
		#endregion

        #region GetClientIPAddress
        /// Gets the IP address of the request.
        /// <remarks>
        /// This method is more useful than built in because in some cases it may show real user IP address even under proxy.
        /// <summary>
        /// Gets the IP address of the request.
        /// <remarks>
        /// This method is more useful than built in because in some cases it may show real user IP address even under proxy.
        /// The <see cref="System.Net.IPAddress.None" /> value will be returned if getting is failed.
        /// </remarks>
        /// </summary>
        /// <param name="request">The HTTP request object.</param>
        /// <returns></returns>
        public static IPAddress GetClientIPAddress(HttpRequest request)
        {
            string ipString;
            if (string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                ipString = request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                ipString = request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
            }

            IPAddress result;
            if (!IPAddress.TryParse(ipString, out result))
            {
                result = IPAddress.None;
            }
            return result;
        }
        #endregion

        #region GetReferrer
        /// <summary>
		/// Retrieves referring URL including protocol and query string from Request.ServerVariables
		/// </summary>
		public static string GetReferrer() 
		{
			try 
			{
                if (HttpContext.Current != null && HttpContext.Current.Request.UrlReferrer != null)
                    return HttpContext.Current.Request.UrlReferrer.ToString();
                else
                    return null;
			} 
			catch 
			{
				return null;
			}
		}
		#endregion

		#region GetRequestedUrl
		/// <summary>
		/// Retrieves requested URL including protocol and query string from Request.ServerVariables
		/// </summary>
		public static string GetRequestedUrl() 
		{
			try {
                if (HttpContext.Current == null)
                    return null;
                if (HttpContext.Current.Request == null)
                    return null;

				string strProtocol = "http://";
				string strRequestedUrl;
            
                if(!StringFunctions.IsNullOrWhiteSpace(HttpContext.Current.Request.ServerVariables["HTTPS"]))
				    if(HttpContext.Current.Request.ServerVariables["HTTPS"] != "off")
					    strProtocol = "https://";

				strRequestedUrl = strProtocol + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.ServerVariables["URL"];
			
				if(HttpContext.Current.Request.ServerVariables["QUERY_STRING"] != String.Empty)
					strRequestedUrl += "?" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"];

				return strRequestedUrl;
			} catch {
				return null;
			}
		}
		#endregion

		#region GetUrl

		#region Overloads
		/// <summary>
		/// Call a url and return it as a string
		/// </summary>
		public static string GetUrl(string Url) 
		{
			return(GetUrl(Url,"get",""));
		}

        /// <summary>
		/// Call a url and return it as a string
		/// </summary>
        public static string GetUrl(string Url, out HttpStatusCode ResponseStatusCode, out string ResponseStatusDescription) 
		{
			return(GetUrl(Url,"get","", null, out ResponseStatusCode, out ResponseStatusDescription));
		}

        
		/// <summary>
		/// Call a url and return it as a string
		/// </summary>
		public static string GetUrl(string Url, NetworkCredential Authentication) 
		{
			return(GetUrl(Url,"get","",Authentication));
		}

		/// <summary>
		/// Call a url and return it as a string
		/// </summary>
		public static string GetUrl(string Url, string Method, string PostData) 
		{
			return(GetUrl(Url,Method,PostData,null));
		}
		#endregion

		/// <summary>
		/// Call a url and return it as a string
		/// </summary>
		public static string GetUrl(string Url, string Method, string PostData, NetworkCredential Authentication) 
		{
            HttpStatusCode ResponseStatusCode;
            string ResponseStatusDescription;

            return GetUrl(Url, Method, PostData, Authentication, out ResponseStatusCode, out ResponseStatusDescription);
		}

        /// <summary>
        /// Call a url and return it as a string
        /// </summary>
        public static string GetUrl(string Url, string Method, string PostData, NetworkCredential Authentication, out HttpStatusCode ResponseStatusCode, out string ResponseStatusDescription)
        {
            string str_response;
            StreamReader sw_in = new StreamReader(GetUrlStream(Url, Method, PostData, Authentication, out ResponseStatusCode, out ResponseStatusDescription));
            str_response = sw_in.ReadToEnd();
            sw_in.Close();
            return (str_response);

        }
		#endregion
		
		#region GetUrlStream

		#region Overloads
		/// <summary>
		/// Call a url and return it as a string
		/// </summary>
		public static Stream GetUrlStream(string Url) 
		{
			return(GetUrlStream(Url,"get",""));
		}

        /// <summary>
        /// Call a url and return it as a string
        /// </summary>
        public static Stream GetUrlStream(string Url, out HttpStatusCode ResponseStatusCode, out string ResponseStatusDescription)
        {
            return (GetUrlStream(Url, "get", "", null, out ResponseStatusCode, out ResponseStatusDescription));
        }

		/// <summary>
		/// Call a url and return it as a string
		/// </summary>
		public static Stream GetUrlStream(string Url, NetworkCredential Authentication) 
		{
            HttpStatusCode ResponseStatusCode;
            string ResponseStatusDescription;

			return(GetUrlStream(Url,"get","",Authentication, out ResponseStatusCode, out ResponseStatusDescription));
		}

		/// <summary>
		/// Call a url and return it as a string
		/// </summary>
		public static Stream GetUrlStream(string Url, string Method, string PostData) 
		{
            HttpStatusCode ResponseStatusCode;
            string ResponseStatusDescription;

            return (GetUrlStream(Url, Method, PostData, null, out ResponseStatusCode, out ResponseStatusDescription));
		}
		#endregion


		/// <summary>
		/// Call a url and return it as a string
		/// </summary>
		public static Stream GetUrlStream(string Url, string Method, string PostData, NetworkCredential Authentication, out HttpStatusCode ResponseStatusCode, out string ResponseStatusDescription) 
		{
			string str_send;
			// Create the request back
			HttpWebRequest hwr_request;
			hwr_request = (HttpWebRequest) WebRequest.Create(Url);
			//hwr_request.Timeout = 10;

			// Set values for the request back
			if(Method.ToLower().Trim() == "post")
			{
				hwr_request.Method = "POST";
				hwr_request.ContentType = "application/x-www-form-urlencoded";
				str_send = PostData;
				hwr_request.ContentLength = str_send.Length;
				// Write the request
				StreamWriter sw_out = new StreamWriter(hwr_request.GetRequestStream(), Encoding.ASCII);
				sw_out.Write(str_send);
				sw_out.Close();
			}
			else
			{
				hwr_request.Method = "GET";
				hwr_request.ContentType = "text/html";
			}

			//Attach Credentials
			if(Authentication != null)
			{
				hwr_request.Credentials = Authentication;
			}
			
            var objResponse = (HttpWebResponse) hwr_request.GetResponse();
            ResponseStatusCode = objResponse.StatusCode;
            ResponseStatusDescription = objResponse.StatusDescription;
			return(objResponse.GetResponseStream());
		}

		#endregion

		#region RemoveUrlParameter
		/// <summary>
		/// Removes the specified parameter from the url passed in.
		/// </summary>
		/// <param name="strUrl">string - A valid URL</param>
		/// <param name="strRemove">string - The parameter name to be removed</param>
		/// <returns>string</returns>
		public static string RemoveUrlParameter(string strUrl, string strRemove) 
		{
			#region Preprocessing
			// Make sure we're comparing apples to apples.
			strUrl = strUrl.ToLower();
			strRemove = strRemove.ToLower();
			
			// If the url doesn't have a query string, just return what was
			// passed in.
			if (strUrl.IndexOf("?") < 0) return strUrl;
			
			// If the parameter to remove doesn't exist in the url, just
			// return the url.
			if (strUrl.IndexOf(strRemove + "=") < 0) return strUrl;
			#endregion
			
			// Separate the script and the query string.
			string[] astrUrl = strUrl.Split('?');
			string strScript = astrUrl[0];
			
			string strQuery;
			try { strQuery = astrUrl[1]; } catch { strQuery = string.Empty; }
			
			if (strQuery != "") {
				ArrayList alGoodParams = new ArrayList();
				
				// Separate the query string into parameters
				string[] astrParams = strQuery.Split('&');
				
				foreach(string strParam in astrParams) {
					// Separate the parameter from the value
					string[] astrParamValue = strParam.Split('=');
					
					// If the parameter doesn't match the one we're trying to remove
					// add THE ENTIRE PARAMETER to the list of good parameters.
					if (astrParamValue[0] != strRemove) alGoodParams.Add(strParam);
				}
				
				// Put Humpty Dumpty back together again.
				StringBuilder sbQuery = new StringBuilder();
				foreach(string strParam in alGoodParams) {
					sbQuery.Append("&" + strParam);
				}
				if (sbQuery.Length > 2) sbQuery.Remove(0, 1);
				
				// Only return if we have something left in the query string.
				if (sbQuery.Length > 0)
					return strScript + "?" +sbQuery.ToString();
			}
			
			return strScript;
		}
		#endregion

		#region ReplaceUrlParameter
		/// <summary>
		/// This parameter will overwrite the value of a specified parameter with the new
		/// value passed in.
		/// </summary>
		/// <param name="strUrl">string - A valid URL</param>
		/// <param name="strParam">string - The parameter name to be replaced</param>
		/// <param name="strValue">string - The new value for the parameter</param>
		/// <returns>string</returns>
		public static string ReplaceUrlParameter(string strUrl, string strParam, string strValue) 
		{
			strUrl = RemoveUrlParameter(strUrl, strParam);
			if (strValue == string.Empty) return strUrl;
			
			strUrl += (strUrl.IndexOf("?") > 0) ? "&" : "?";
			strUrl += strParam+ "=" +strValue;
			return strUrl;
		}
		#endregion

		#region AddUrlParameter
		/// <summary>
		/// Adds a parameter to the query string. This uses the same exact logic as
		/// ReplaceUrlParameter, but I wanted to provide a better name for usability
		/// purposes. Basically, the parameter is removed if it exists, then added
		/// back on. It's the same logic except the parameter never existed to be
		/// removed. (which is transparent)
		/// </summary>
		/// <param name="strUrl">string - A valid URL</param>
		/// <param name="strParam">string - The parameter name to be added</param>
		/// <param name="strValue">string - The new value for the parameter</param>
		/// <returns>string</returns>
		public static string AddUrlParameter(string strUrl, string strParam, string strValue) 
		{
			return ReplaceUrlParameter(strUrl, strParam, strValue);
		}
		#endregion
		
		#region GetUrlParameter
		/// <summary>
		/// Gets the value of a specified querystring parameter from the specified URL string.
		/// </summary>
		/// <param name="strUrl">string - A valid URL</param>
		/// <param name="strParameter">string - The parameter name to return a value for</param>
		/// <returns>string</returns>
		public static string GetUrlParameter(string strUrl, string strParameter) 
		{
			#region Preprocessing
			// Make sure we're comparing apples to apples.
			strUrl = strUrl.ToLower();
			strParameter = strParameter.ToLower();
			
			// If the url doesn't have a query string, just return what was
			// passed in.
			if (strUrl.IndexOf("?") < 0) return strUrl;
			
			// If the parameter to remove doesn't exist in the url, just
			// return the url.
			if (strUrl.IndexOf(strParameter + "=") < 0) return strUrl;
			#endregion
			
			string strQuery = GetQueryString(strUrl);
			if (strQuery != string.Empty) {
				// Separate the query string into parameters
				string[] astrParams = strQuery.Split('&');
				
				foreach(string strParam in astrParams) {
					// Separate the parameter from the value
					string[] astrParamValue = strParam.Split('=');
					
					// If the parameter matches the one we're trying to find
					// return the parameter value.
					if (astrParamValue[0] == strParameter) return astrParamValue[1];
				}
				
			}
			
			return string.Empty;
		}
		#endregion

		#region GetQueryString
		/// <summary>
		/// Gets only the query string from a valid URL.
		/// </summary>
		/// <param name="strUrl">string - A valid URL</param>
		/// <returns>string</returns>
		public static string GetQueryString(string strUrl) 
		{
			#region Preprocessing
			// If the url doesn't have a query string, just return empty.
			if (strUrl.IndexOf("?") < 0) return string.Empty;
			#endregion
			
			string[] astrUrl = strUrl.Split('?');
			string strQuery;
			try { strQuery = astrUrl[1]; } catch { strQuery = string.Empty; }
			
			return strQuery;
		}
		#endregion

		#region RemoveQueryString
		/// <summary>
		/// Removes the query string from a valid URL.
		/// </summary>
		/// <param name="strUrl">string - A valid URL</param>
		/// <returns>string</returns>
		public static string RemoveQueryString(string strUrl) 
		{
			#region Preprocessing
			// If the url doesn't have a query string, just return what was passed.
			if (strUrl.IndexOf("?") < 0) return strUrl;
			#endregion
			
			string[] astrUrl = strUrl.Split('?');
			string strQuery;
			try { strQuery = astrUrl[0]; } catch { strQuery = string.Empty; }
			
			return strQuery;
		}
		#endregion
		
		#region GetScriptName
		
		#region GetScriptName Overloads
		public static string GetScriptName(string strUrl) 
		{
			return GetScriptName(strUrl, false);
		}
		#endregion

		/// <summary>
		/// Gets only the name of the file without path information.
		/// </summary>
		/// <param name="strUrl">string - A valid URL</param>
		/// <param name="blnRemoveExtension">bool - Set true to remove the extension</param>
		/// <returns>string</returns>
		public static string GetScriptName(string strUrl, bool blnRemoveExtension) 
		{
			string[] astrUrl;
			
			// Split on the '?' to get rid of the querystring.
			if (strUrl.IndexOf("?") >= 0) {
				astrUrl = strUrl.Split('?');
				strUrl = astrUrl[0];
			}
			if (strUrl == null) return string.Empty;
			
			// Split on the '/' and grab the last index.
			if (strUrl.IndexOf("/") >= 0) {
				astrUrl = strUrl.Split('/');
				strUrl = astrUrl[astrUrl.Length - 1];
			}
			if (strUrl == null) return string.Empty;

            string strExtention = StringFunctions.AllAfterReverse(strUrl, ".");

			//if (blnRemoveExtension) {
				// Split on the '.' to remove the extension and prefixes.
				if (strUrl.IndexOf(".") >= 0) {
					astrUrl = strUrl.Split('.');
                    if(astrUrl.Length >= 2)
                        strUrl = astrUrl[astrUrl.Length - 2];
				}
				if (strUrl == null) return string.Empty;
			//}

                if (!blnRemoveExtension)
                    strUrl = strUrl + "." + strExtention;
			
			return strUrl;
		}
		#endregion
		
		#region GetScriptExtension
		/// <summary>
		/// Gets the extension of the ScriptName for the URL passed in. For example,
		/// if "http://www.domain.com/test.aspx" was passed in, "aspx" would be
		/// returned.
		/// </summary>
		/// <param name="strUrl">string - A valid URL</param>
		/// <returns>string</returns>
		public static string GetScriptExtension(string strUrl) 
		{
			if (strUrl == null || strUrl == string.Empty)
				throw new ArgumentNullException("Must specify a valid URL.");
			
			string strScript = GetScriptName(strUrl);
			if (strScript == string.Empty || strScript.IndexOf('.') <= 0) return string.Empty;
			
			string[] astrScript = strScript.Split('.');
			if (astrScript.Length == 2) return astrScript[1];
			
			return string.Empty;
		}
		#endregion
		
		#region ApplicationVirtualURL
		/// <summary>
		/// Builds a virtual URL based on the application root. You
		/// can pass a URL like "~/images/test.jpg" and it will return
		/// the appropriate full path.
		/// </summary>
		/// <param name="strUrl">string - A valid virtual URL</param>
		/// <returns>string</returns>
		public static string ApplicationVirtualURL(string strUrl) 
		{
			#region Validation
			if (strUrl == string.Empty) return string.Empty;
			
			// If a .Net root path (~/) has been passed, remove the "~".
			if (strUrl.StartsWith("~/")) strUrl = strUrl.Substring(1);
			
			// Make sure the path passed in starts with a forward slash (/).
			if (!strUrl.StartsWith("/")) strUrl = "/" + strUrl;
			#endregion
			
			// Grab the application root.
			string strRoot = GetApplicationRoot();
			
			// Concatenate and return.
			return strRoot + strUrl;
		}
		#endregion

		#region GetApplicationRoot
		/// <summary>
		/// Returns the application root if not "/". If the domain root equals
		/// the application root string.Empty is returned.
		/// </summary>
		/// <returns>string</returns>
		public static string GetApplicationRoot() {
			if (HttpContext.Current == null) return string.Empty;
			
			// Grab the application root and make sure it's not just a forward slash (/).
			string strRoot = HttpContext.Current.Request.ApplicationPath;
			if (strRoot == "/") return string.Empty;
			
			return strRoot;
		}
		#endregion

		#region GetApplicationVirtualPath
		/// <summary>
		/// This method will return the virtual path of the URL passed in
		/// relative to the application root. For example if my rawurl is
		/// http://www.sportcompactonly.com/approot/default.aspx and my
		/// application root is "/approot" this method will return "default.aspx".
		/// If, in the same situation, my application root was the domain
		/// root, then this method would return "approot/default.aspx".
		/// </summary>
		/// <param name="strUrl">string - A valid URL</param>
		/// <returns>string</returns>
		private static string GetApplicationVirtualPath(string strUrl) 
		{
			#region Validation
			if (strUrl == string.Empty) return string.Empty;
			if (strUrl.StartsWith("~/")) strUrl = strUrl.Substring(1);
			#endregion
			
			// Grab the application root.
			string strRoot = GetApplicationRoot();
			if (strRoot == string.Empty) {
				// If strRoot is empty it means the application root is
				// the same as the domain root, so we just find the first
				// slash (/) after the double slash (//).
				return GetDomainPath(strUrl);
			} else {
				// Otherwise find the index of the application root and
				// grab anything after it. We add +1 to the root length
				// so that the prefixing slash (/) gets removed.
				if (strUrl != strRoot)
					if (strUrl.IndexOf(strRoot) != -1)
						return strUrl.Substring(strUrl.IndexOf(strRoot) + strRoot.Length + 1);
					else
						return (strUrl.StartsWith("/")) ? strUrl.Substring(1) : strUrl;
				else
					return string.Empty;
			}
		}
		#endregion
		
		#region GetDomainPath
		/// <summary>
		/// Gets the virtual path based on the domain root of a specified URL.
		/// For example, if we have http://www.sco.com/test/test.aspx as our URL
		/// we would get back "test/test.aspx". If we don't have a fully qualified
		/// URL we just check if the URL stars with "/", if so take it off, if not
		/// just return what we got passed in.
		/// </summary>
		/// <param name="strUrl">string - A valid URL</param>
		/// <returns>string</returns>
		private static string GetDomainPath(string strUrl) 
		{
			#region Validation
			if (strUrl == string.Empty) return string.Empty;
				
			// Make sure this is a fully qualified URL. If not
			// we can't process it. Just return what was passed.
			if (strUrl.IndexOf("//") == -1)
				return (strUrl.StartsWith("/")) ? strUrl.Substring(1) : strUrl;
			
			// If there is no slash (/) after the double slash (//),
			// this is an invalid URL.
			if (strUrl.IndexOf("/", strUrl.IndexOf("//") + 2) == -1)
				throw new ArgumentException("Invalid URL passed to GetDomainPath().");
			#endregion
			
			return strUrl.Substring(strUrl.IndexOf("/", strUrl.IndexOf("//") + 2) + 1);
		}
		#endregion

		#region GetAbsoluteURL
		/// <summary>
		/// Gets the absolute path based on the domain root and the type
		/// of URL passed in. Here is the series of checks and results:
		///		1) Check if URL starts with "~/[Root]". If so, remove the "~" and return.
		///		2) Check if the URL starts with "/[Root]". If so, return.
		///		3) Check if the URL starts with "~/". If so, remove the "~", attach the root and return.
		///		4) Check if the URL starts with "/". Attach the root and return.
		///		5) Add "/" to the URL, attach the root and return.
		/// </summary>
		/// <param name="strUrl">string - A valid destination URL</param>
		/// <returns>string</returns>
        private static string GetAbsoluteURL(string strUrl) 
		{
			#region Validation
			if (strUrl == null || strUrl == string.Empty)
				return string.Empty;
			
			if (strUrl.StartsWith("http") || strUrl.StartsWith("javascript:"))
				return strUrl;
			#endregion

			string strRoot = GetApplicationRoot();
			
			if (strRoot != string.Empty) {
				// Check 1)
				if (strUrl.StartsWith("~" + strRoot))
					return strUrl.Substring(1);
				
				// Check 2)
				if (strUrl.StartsWith(strRoot))
					return strUrl;
			}
			
			// Check 3)
			if (strUrl.StartsWith("~/"))
				return strRoot + strUrl.Substring(1);
			
			// Check 4)
			if (strUrl.StartsWith("/"))
				return strRoot + strUrl;
			
			// Default 5)
			return strRoot + "/" + strUrl;
		}
		#endregion
		
		#region GetFullExternalURL
		/// <summary>
		/// Gets the full URL with protocal, domain and path. For example if
		/// strUrl is "~/default.aspx", our domain is www.test.com and we
		/// are not under SSL, http://www.test.com/default.aspx would be
		/// returned.
		/// </summary>
		/// <param name="strUrl">string - a valid path within the current site.</param>
		/// <returns>string - Full URL</returns>
        private static string GetFullExternalURL(string strUrl)
        {
			string strProtocol = (HttpContext.Current.Request.ServerVariables["HTTPS"] == "off") ? "http://" : "https://";
			string strHost = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
			string strAbsolute = GetAbsoluteURL(strUrl);
			
			return strProtocol + strHost + strAbsolute;
		}
		#endregion

		#region IsRobot
		/// <summary>
		/// Tests if the user agent is a robot.
		/// </summary>
		/// <param name="strUserAgent">string - A valid UserAgent</param>
		/// <returns>bool</returns>
		public static bool IsRobot(string strUserAgent) 
		{
			if (strUserAgent == string.Empty) return false;
			
			// Make sure the UserAgent is lower case for comparison.
			strUserAgent = strUserAgent.ToLower();
			
			return (strUserAgent.IndexOf("googlebot") > -1 ||
			    strUserAgent.IndexOf("msnbot") > -1 ||
			    strUserAgent.IndexOf("yahoo! slurp") > -1 ||
			    strUserAgent.IndexOf("scanalert") > -1 ||
			    strUserAgent.IndexOf("exabot") > -1 ||
			    strUserAgent.IndexOf("voyager") > -1 ||
			    strUserAgent.IndexOf("sensis web crawler") > -1 ||
			    strUserAgent.IndexOf("accoonabot") > -1 ||
			    strUserAgent.IndexOf("turnitinbot") > -1 ||
			    strUserAgent.IndexOf("ia_archiver") > -1 ||
			    strUserAgent.IndexOf("converacrawler") > -1 ||
			    strUserAgent.IndexOf("redcarpet") > -1 ||
			    strUserAgent.IndexOf("mj12bot") > -1 ||
			    strUserAgent.IndexOf("noxtrumbot") > -1 ||
			    strUserAgent.IndexOf("yoono") > -1 ||
			    strUserAgent.IndexOf("becomebot") > -1 ||
			    strUserAgent.IndexOf("help:bot") > -1 ||
			    strUserAgent.IndexOf("ask jeeves/teoma") > -1 ||
			    strUserAgent.IndexOf("gigabot") > -1
			);
		}
		#endregion

        #region GetWebSiteURL
        public static string GetWebSiteURL()
        {
            try
            {
                string strProtocol = "http://";
                string strRequestedUrl;

                if (!StringFunctions.IsNullOrWhiteSpace(HttpContext.Current.Request.ServerVariables["HTTPS"]))
                    if (HttpContext.Current.Request.ServerVariables["HTTPS"] != "off")
                        strProtocol = "https://";

                strRequestedUrl = strProtocol + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + GetApplicationRoot();
                return strRequestedUrl;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region GetRequestedPage
        public static string GetRequestedPage()
        {
            string strResult = HttpContext.Current.Request.ServerVariables["URL"];
            if(strResult.Contains("/"))
                strResult = StringFunctions.AllAfterReverse(strResult, "/");
            return strResult;
        }
        #endregion

        #region GetRequestedPageWithQueryString
        public static string GetRequestedPageWithQueryString()
        {
            string strResult = GetRequestedPage();
            if (HttpContext.Current.Request.ServerVariables["QUERY_STRING"] != String.Empty)
                strResult += "?" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
            return strResult;
        }
        #endregion

    }
}
