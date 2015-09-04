using System;
using General;
using System.Collections;
using System.Data.SqlClient;

namespace General.DAO
{
	/// <summary>
	/// Summary description for SQLCache.
	/// </summary>
	public class SQLCache
	{

		#region Constructors
		/// <summary>
		/// Summary description for SQLCache.
		/// </summary>
		public SQLCache()
		{

		}

		#endregion

		#region GetFromCache
		/// <summary>
		/// Retrieves an object from the SQLCache
		/// </summary>
		public static object GetFromCache(string Key, ref SQLOptions o)
		{
			object obj = null;
			if(System.Web.HttpContext.Current == null)
				throw new Exception("Caching is not available outside of web context");

			if(o.DoMemoryCache)
				obj = SQLMemoryCache.GetFromMemoryCache(Key, ref o);

			if(obj != null)
			{
				//General.Debug.Trace("Got from memory cache");
				o.WriteToLog("Retrieved from memory cache");
			}

			if(obj == null && o.DoFileCache)
			{
				try
				{
					obj = SQLFileCache.GetFromFileCache(Key, ref o);
					if(obj != null && o.DoMemoryCache)
						SQLMemoryCache.AddToMemoryCache(Key,obj,ref o);
				}
				catch(Exception ex)
				{
					obj = null;
					o.WriteToLog("Error trying to load SQL FileCache: " + ex.Message);
					General.Debugging.Report.SendError("Error trying to load SQL FileCache",ex);
				}
				if(obj != null)
				{
					General.Debug.Trace("Got from file cache");
					o.WriteToLog("Retrieved from file cache");
				}
			}
				
			return obj;
		}
		#endregion

		#region AddToCache
		/// <summary>
		/// Adds an object to the SQLCache
		/// </summary>
		public static void AddToCache(string Key, object Obj, ref SQLOptions o)
		{
			if(System.Web.HttpContext.Current == null)
				throw new Exception("Caching is not available outside of web context");

			//if(log == null)
			//log = new TextLog();

			if(o.DoFileCache)
			{
				try
				{
					SQLFileCache.AddToFileCache(Key,Obj,ref o);
				}
				catch(Exception ex)
				{
					o.WriteToLog("Error trying to save to SQL FileCache: " + ex.Message);
					General.Debugging.Report.SendError("Error trying to save to SQL FileCache",ex);
				}
			}

			if(o.DoMemoryCache)
				SQLMemoryCache.AddToMemoryCache(Key,Obj,ref o);	
		}
		#endregion

		#region GetTransactionHistory
		public static string GetTransactionHistory(string strLineBreak)
		{
			string strResult = "";
			if(System.Web.HttpContext.Current != null)
			{
				ArrayList objHistory = (ArrayList) System.Web.HttpContext.Current.Items["SqlHelperTransactionHistory"];
				if(objHistory != null)
				{
					foreach (string strQuery in objHistory)
					{
						strResult += strQuery + "\r";	
						strResult += strLineBreak;
					}
				}
			}
			return strResult;
		}
		#endregion

		#region InsertTransactionHistory
		public static void InsertTransactionHistory(SqlCommand cmd)
		{
			if(System.Web.HttpContext.Current != null)
			{
				ArrayList objHistory = (ArrayList) System.Web.HttpContext.Current.Items["SqlHelperTransactionHistory"];
				if(objHistory == null)
					objHistory = new ArrayList(25);
				
				objHistory.Insert(0,SqlHelper.GetQueryString(cmd));

				if(objHistory.Count > 25)
				{
					objHistory.RemoveAt(objHistory.Count - 1);
				}
				
				System.Web.HttpContext.Current.Items["SqlHelperTransactionHistory"] = objHistory;
			}
		}
		#endregion

	}
}
