using System;
using System.Text;
using General.Configuration;
using System.Web;
using System.IO;
using System.Collections;
using General.Mail;
namespace General
{
	/// <summary>
	/// CUSTOM ERROR CLASS
	/// </summary>
	public class Err : ApplicationException
	{

		/// <summary>
		/// Create a new Err Object
		/// </summary>
		public Err() : base("An error has occurred.")
		{
  
		}

		/// <summary>
		/// Create a new Err Object
		/// </summary>
		public Err(string message) : base(message)
		{

		}

		/// <summary>
		/// Create a new Err Object
		/// </summary>
		public Err(string message, Exception inner) : base(message,inner)
		{

		}

		/// <summary>
		/// Returns the error number
		/// </summary>
//		public int number
//		{
//			get {return number;}
//		} // ***** I removed this because it will cause NEVER ENDING RECURSION if called. JF 12/05/2006
		

		/// <summary>
		/// For Web Applications only, processes ASP.Net Error
		/// </summary>
		public static void ProcessError(Exception e) 
		{
			string str_exception = e.InnerException.GetBaseException().GetType().ToString();
			switch (str_exception.ToLower()) 
			{
				case "system.io.filenotfoundexception": Process404(e); break;
				default: ProcessDefault(e); break;
			}
		}

		/// <summary>
		/// For Web Applications only, processes ASP.Net Error
		/// </summary>
		public static void ProcessError(Err e) 
		{
			string str_exception = e.InnerException.GetBaseException().GetType().ToString();
			switch (str_exception.ToLower()) 
			{
				case "system.io.filenotfoundexception": Process404(e); break;
				default: ProcessDefault(e); break;
			}
		}
		
		/// <summary>
		/// For Web Applications only, processes ASP.Net Error
		/// </summary>
		public static void Process404(Exception e) 
		{
			HttpContext _context = HttpContext.Current;
			_LoadPages(); _LoadParams();
      
			string str_url = _context.Request.Url.ToString();
			if (str_url.LastIndexOf("404;") > 0) str_url = str_url.Substring(str_url.LastIndexOf("404;") + 4);

			string str_queryString = (str_url.IndexOf("?") > 0) ? str_url.Substring(str_url.LastIndexOf("?") + 1) : "";

			Hashtable hash_queryString = new Hashtable();
			if (str_queryString != "") { hash_queryString = ParseQueryString(str_queryString); }

			string str_page = str_url.Substring(str_url.LastIndexOf("/") + 1);
			str_page = (str_page.IndexOf("?") > 0) ? str_page.Substring(0, str_page.LastIndexOf("?")) : str_page;
      
			if (str_page.EndsWith(".asp") && !str_page.EndsWith(".aspx")) 
			{
				str_page = GetURL(str_page, hash_queryString);
				_context.Response.Write("str_page = " +str_page);
				_context.Response.Status = "301 Moved Permanently";
				_context.Response.AddHeader("Location", str_page);
				_context.Server.ClearError();
			} 
			else if (str_page.IndexOf(".") < 0) 
			{
				// Try the page with .aspx on the end if there is no extension found.
				str_page = str_page + ".aspx";
				FileInfo io_exists = new FileInfo(General.Environment.Current.MapPath(str_page));
				if (io_exists.Exists) 
				{
					_context.Server.ClearError();
					_context.Response.Redirect("~/" + str_page);
				} 
				else 
				{
					ProcessDefault(e);
				}
			} 
			else 
			{
				ProcessDefault(e);
			}
		}

		/// <summary>
		/// For Web Applications only, processes ASP.Net Error
		/// </summary>
		public static void ProcessDefault(Exception e) 
		{
			HttpContext _context = HttpContext.Current;
			
			StringBuilder sbBody = new StringBuilder();
			
			sbBody.Append(DateTime.Now.ToString()+ "\n\n");
			sbBody.Append("general site error on " +_context.Request.Url.ToString()+ "\n\n");
			sbBody.Append("BASE EXCEPTION: " +e.GetBaseException()+ "\n\n");
			sbBody.Append("SOURCE: " +e.Source.ToString()+ "\n\n");
			sbBody.Append("MESSAGE: " +e.Message.ToString()+ "\n\n");

			sbBody.Append("SESSION DUMP:\n");
			if (_context.Session.Count > 0) 
			{
				foreach (string str_sessionKey in _context.Session.Keys) 
				{
					sbBody.Append(str_sessionKey+ " = " +_context.Session[str_sessionKey].ToString()+ "\n");
				}
			}
			sbBody.Append("\n\n");

			sbBody.Append("REQUEST DUMP:\n");
			if (_context.Request.Form.Count > 0) 
			{
				foreach (string str_formKey in _context.Request.Form.Keys) 
				{
					if (str_formKey != "__VIEWSTATE")
						sbBody.Append(str_formKey+ " = " +_context.Request.Form[str_formKey].ToString()+ "\n");
				}
			}
			sbBody.Append("\n\n");

			sbBody.Append("INNER ERROR: " +e.InnerException.ToString()+ "\n\n");
			sbBody.Append("TARGET SITE NAME: " +e.TargetSite.Name.ToString()+ "\n\n");
			sbBody.Append("STACK TRACE: " +e.StackTrace.ToString()+ "\n\n");
			sbBody.Append("\n\n");
      
			sbBody.Append("COOKIES DUMP (EXCLUDING BINARY OBJECTS):\n");
			if (_context.Request.Cookies.Count > 0) 
			{
				foreach (string str_cookieKey in _context.Request.Cookies.Keys) 
				{
					switch(str_cookieKey.ToLower())
					{
						case "":
							//DO NOTHING
							break;
						default:
							if(!str_cookieKey.EndsWith("object"))
								sbBody.Append(str_cookieKey+ " = " +_context.Request.Cookies[str_cookieKey].Value.ToString()+ "\n");
							break;
					}
				}
			}
			sbBody.Append("\n\n");

			sbBody.Append("SYSTEM VARIABLES: \n");
			foreach (string str_variableKey in _context.Request.ServerVariables.Keys) 
			{
				switch(str_variableKey.ToLower())
				{
					case "cookie":
					case "http_cookie":
					case "all_raw":
					case "all_http":
						//DO NOTHING
						break;
					default:
						sbBody.Append(str_variableKey+ " = " +_context.Request.ServerVariables[str_variableKey].ToString()+ "\n");
						break;
				}
			}
			sbBody.Append("\n\n");
			
			MailTools.SendEmail(GlobalConfiguration.GlobalSettings["error_email_from"], GlobalConfiguration.GlobalSettings["error_email_to"], ".Net Site Error", sbBody.ToString(), false);
			
			_context.Server.ClearError();
			_context.Response.Redirect("~/errors.aspx?error_description=" + _context.Server.UrlEncode(e.GetBaseException().ToString()));
		}
    
		/// <summary>
		/// Constructs and returns a url string
		/// </summary>
		public static string GetURL(string str_page, Hashtable hashQueryString) {
			// convert the page.
			StringBuilder sbResultPage = new StringBuilder();
			sbResultPage.Append(FindPage(str_page));
			if (sbResultPage.Length == 0) sbResultPage.Append("default.aspx");
      
			// convert the parameters.
			StringBuilder sbParams = new StringBuilder();
			if (hashQueryString.Count > 0) {
				foreach (string strKey in hashQueryString.Keys) {
					string strParamKey = FindParam(strKey);
          
					if (strParamKey != "") 
						sbParams.Append("&" +strParamKey+ "=" +hashQueryString[strKey]);
				}
				StringBuilder sbQueryString = sbParams.Remove(0, 1);
        
				if (sbResultPage.ToString().IndexOf("?") > 0) {
					sbResultPage.Append("&" + sbQueryString.ToString());
				} else {
					sbResultPage.Append("?" + sbQueryString.ToString());
				}
			}
      
			return sbResultPage.ToString();
		}
    
		/// <summary>
		/// Reads a query string and returns a HashTable
		/// </summary>
		public static Hashtable ParseQueryString(string str_queryString) 
		{
			Hashtable hash_queryString = new Hashtable();
      
			if (str_queryString != "") 
			{
				string[] arr_queryString = str_queryString.Split(new Char[] { '&' });
				int int_queryLength = arr_queryString.Length;
				for (int i = 0; i < int_queryLength; i++) 
				{
					string[] arr_queryItem = arr_queryString[i].Split(new char[] { '=' });
					hash_queryString.Add(arr_queryItem[0], arr_queryItem[1]);
				}
			}
			return hash_queryString;
		}
    
		/// <summary>
		/// Returns a corrected page from a list of page redirects
		/// </summary>
		public static string FindPage(string str_page) 
		{
			int int_pagesLength = _obj_pages.Length;
			for (int i = 0; i < int_pagesLength; i++) 
			{
				object[] obj_page = (object[]) _obj_pages[i];
				string str_oldPage = obj_page[0].ToString();
				string str_newPage = obj_page[1].ToString();
        
				if (str_oldPage == str_page) { return str_newPage; }
			}
      
			return "";
		}

		/// <summary>
		/// Returns a corrected param from a list of param overrides
		/// </summary>
		public static string FindParam(string str_param) 
		{
			int int_paramsLength = _obj_params.Length;
			for (int i = 0; i < int_paramsLength; i++) 
			{
				object[] obj_param = (object[]) _obj_params[i];
				string str_oldParam = obj_param[0].ToString();
				string str_newParam = obj_param[1].ToString();
        
				if (str_oldParam == str_param) { return str_newParam; }
			}
      
			return str_param;
		}

    
		/// <summary>
		/// A list of pages to redirect
		/// </summary>
		public static object[] _obj_pages = new object[10];

		/// <summary>
		/// Populate page reference
		/// </summary>
		public static void _LoadPages() 
		{
			int i = 0;
			_obj_pages[i++] = new object[2] { "index.asp", "index.aspx" };
			_obj_pages[i++] = new object[2] { "login.asp", "login.aspx" };
		}
    
		/// <summary>
		/// A list of params to override
		/// </summary>
		public static object[] _obj_params = new object[8];

		/// <summary>
		/// Populate param reference
		/// </summary>
		public static void _LoadParams() 
		{
			//int i = 0;
			//_obj_params[i++] = new object[2] { "brandid", "b" };
		}

	}
}
