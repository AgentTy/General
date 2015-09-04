using System;
using General;
using System.Web.Caching;
using System.Data;
using System.Data.SqlClient;
using General.Utilities.Text;

namespace General.DAO
{
	/// <summary>
	/// SQLMemoryCache
	/// </summary>
	public class SQLMemoryCache
	{
		private static int _intCacheCount = 0;

		#region Constructors

		/// <summary>
		/// SQL Memory Cache
		/// </summary>
		public SQLMemoryCache()
		{

		}

		#endregion

		#region AddToMemoryCache

		/// <summary>
		/// Add to SQL Memory Cache
		/// </summary>
		public static void AddToMemoryCache(string Key, object Obj, ref SQLOptions o, DateTime ExpirationOverride, bool FileDependency)
		{
			DateTime dateTempExpire = o.Expiration;
			bool boolTempFileCache = o.DoFileCache;
			o.Expiration = ExpirationOverride;
			o.DoFileCache = FileDependency;
			AddToMemoryCache(Key,Obj,ref o);
			o.DoFileCache = boolTempFileCache;
			o.Expiration = dateTempExpire;
		}

		/// <summary>
		/// Add to SQL Memory Cache
		/// </summary>
		public static void AddToMemoryCache(string Key, object Obj, ref SQLOptions o)
		{
			if(System.Web.HttpContext.Current == null)
				throw new Exception("Caching is not available outside of web context");
			System.Web.Caching.Cache GlobalCache = System.Web.HttpContext.Current.Cache;

			o.WriteToLog("adding object to memory cache... " + Key + "...Expire=" + o.Expiration.ToString() + "...FileDependency=" + o.DoFileCache);
			System.Web.Caching.CacheDependency dep;
			if(o.DoFileCache)
				dep = new CacheDependency(SQLFileCache.GetCacheFilePath(Key));
			else
				dep = null;
			CacheItemRemovedCallback OnCacheRemove = new CacheItemRemovedCallback(OnCacheRemoveCallback);
			GlobalCache.Add("SQLHelper:" + Key, Obj, dep, o.Expiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, OnCacheRemove);
			_intCacheCount++;
		}

		#endregion

		#region GetFromMemoryCache
		/// <summary>
		/// Get from SQL Memory Cache
		/// </summary>
		public static object GetFromMemoryCache(string Key, ref SQLOptions o)
		{
			o.WriteToLog("searching memory cache..." + Key);
			System.Web.Caching.Cache GlobalCache = System.Web.HttpContext.Current.Cache;
			return(GlobalCache.Get("SQLHelper:" + Key));
		}
		#endregion

		//COMMENTED OUT
		#region AddToCache Overloads
/*
		private static void AddToCache(string Key, object Obj) 
		{
			int intTimeoutMinutes;
			try 
			{
				intTimeoutMinutes = Convert.ToInt32(GlobalConfiguration.GlobalSettings["sqlhelper_data_cache_timeout"]);
			} 
			catch 
			{
				intTimeoutMinutes = 240;
			}
			    
			AddToCache(Key, Obj, DateTime.Now.AddMinutes(intTimeoutMinutes));
		}
		*/
		#endregion
		//END COMMENT

		#region Misc
		/// <summary>
		/// OnCacheRemoveCallback
		/// </summary>
		public static void OnCacheRemoveCallback(string Key, object Obj, CacheItemRemovedReason R)
		{
			//THIS METHOD RUNS WHEN A CACHED DATASET EXPIRES
            if (System.Web.HttpContext.Current != null)
            {
                _intCacheCount--;
                SQLFileCache.DeleteCacheFile(Key);
            }
		}

		/// <summary>
		/// Returns the number of DataSet objects in the SQL Cache
		/// </summary>
		public static int CacheCount
		{
			get{return _intCacheCount;}
		}

		/// <summary>
		/// Returns a summary of the DataSet objects in the SQL Cache
		/// </summary>
		public static string CacheSummary
		{
			get
			{
				if(System.Web.HttpContext.Current == null)
					throw new Exception("Caching is not available outside of web context");

				System.Web.Caching.Cache GlobalCache = System.Web.HttpContext.Current.Cache;
				string strResult = "";
				foreach(System.Collections.DictionaryEntry o in GlobalCache)
				{
					if(StringFunctions.StartsWith(o.Key.ToString(),"SQLHelper:"))
						strResult += o.Key + "<br>\n";
				}
				return strResult;
			}
		}

		/// <summary>
		/// Clears all DataSet objects from the SQL Cache
		/// </summary>
		public static void ClearCache()
		{
			if(System.Web.HttpContext.Current == null)
				throw new Exception("Caching is not available outside of web context");

			System.Web.Caching.Cache GlobalCache = System.Web.HttpContext.Current.Cache;
			foreach(System.Collections.DictionaryEntry o in GlobalCache)
			{
				if(StringFunctions.StartsWith(o.Key.ToString(),"SQLHelper:"))
					GlobalCache.Remove(o.Key.ToString());
			}
		}

		/// <summary>
		/// Clears the specified DataSet object from the SQL Cache
		/// </summary>
		public static void ClearCache(SqlCommand cmd)
		{
			if(System.Web.HttpContext.Current == null)
				throw new Exception("Caching is not available outside of web context");

			System.Web.Caching.Cache GlobalCache = System.Web.HttpContext.Current.Cache;
			foreach(System.Collections.DictionaryEntry o in GlobalCache)
			{
				if(o.Key.ToString() == "SQLHelper:" + SqlHelper.GetQueryHashCode(cmd))
					GlobalCache.Remove(o.Key.ToString());
			}
		}
		#endregion

	}
}
