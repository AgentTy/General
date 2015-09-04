using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;

using General.Utilities.Serialization;
using General.Data;

namespace General.Utilities.Data.Caching {
	/// <summary>
	/// Tools that allow the serialization and storage of any type of data.
	/// </summary>
	public class CachingTools {
	  /// <summary>
	  /// This constructor is not used. All methods are static.
	  /// </summary>
		public CachingTools() { }
		
		#region Save
	  /// <summary>
	  /// Saves an object or piece of data to the Caching System.
	  /// </summary>
		public static void Save(object Obj, string ID, CachePersistance Persistance, DateTime ExpirationDate) {
		  switch (Persistance) {
		    // ** SESSION SPECIFIC: This means that an object is stored using the current session ID
		    //    in addition to the ID passed. This is to prevent the data from being accessed once
		    //    the session has expired. If it is a requirement that the data be retrievable after
		    //    the session is expired, the Machine persistance should be use.
		    
		    // Saves to the session by a SESSION SPECIFIC key, then is backed up by a cookie.
		    case CachePersistance.Session:
		      WriteSession(Obj, ID);
		      WriteCookie(Obj, HttpContext.Current.Session.SessionID + "_" + ID, ExpirationDate);
		    break;
		    
		    // Saves to the session only, no cookie back up.
		    case CachePersistance.SessionNoCookie:
		      WriteSession(Obj, ID);
		    break;
		    
		    // Saves the object to the system cache by a SESSION SPECIFIC key.
		    // Then saves the key to the session backed up by a cookie.
		    case CachePersistance.SessionCache:
		      string strCacheId = HttpContext.Current.Session.SessionID + "_" + ID;
			    WriteCache(Obj, strCacheId, ExpirationDate);
			    WriteSession(strCacheId, ID);
			    WriteCookie(strCacheId, ID, ExpirationDate);
			  break;
			  
		    // Saves to the session and is backed up by a cookie. (not session specific)
		    case CachePersistance.Machine:
		      WriteSession(Obj, ID);
		      WriteCookie(Obj, ID, ExpirationDate, DateTime.Now.AddYears(5));
		    break;
		    
		    case CachePersistance.MachineOnly:
		      WriteCookie(Obj, ID, ExpirationDate, DateTime.Now.AddYears(5));
		    break;
		  }


// ** Ty, this is the orginal code for your reference. Just in case I forgot anything...
//			System.Web.HttpContext context = System.Web.HttpContext.Current;
//			if(Persistance >= CachePersistance.Session && Persistance != CachePersistance.MachineOnly) {
//				context.Session[ID] = Obj;	
//			}
//
//			if(Persistance == CachePersistance.Session) {
//					context.Response.Cookies[context.Session.SessionID + "_" + ID].Value = SerializationTools.SerializeObject(Obj);
//					if(ExpirationDate == SqlConvert.ToDateTime(null)) ExpirationDate = DateTime.Now.AddDays(1);
//					context.Response.Cookies[context.Session.SessionID + "_" + ID].Expires = ExpirationDate;
//			}
//
//			if(Persistance >= CachePersistance.Machine) {
//				context.Response.Cookies[ID].Value = SerializationTools.SerializeObject(Obj);
//				if(ExpirationDate == SqlConvert.ToDateTime(null)) ExpirationDate = DateTime.Now.AddYears(5);
//				context.Response.Cookies[ID].Expires = ExpirationDate;
//			}
		}

    #region Save Overloads
	  /// <summary>
	  /// Overload for the save method.
	  /// Sends null as the expiration date.
	  /// </summary>
		public static void Save(object Obj, string ID, CachePersistance Persistance) {
			Save(Obj, ID, Persistance, SqlConvert.ToDateTime(null));
		}
		#endregion
		
		#region Save Support Methods
		private static void WriteSession(object Obj, string ID) {
		  System.Web.HttpContext.Current.Session[ID] = Obj;
		}
		
		private static void WriteCache(object Obj, string ID, DateTime ExpirationDate, DateTime DefaultExpirationDate) {
			if(ExpirationDate == SqlConvert.ToDateTime(null)) ExpirationDate = DefaultExpirationDate;
      System.Web.HttpContext.Current.Cache.Add(ID, Obj, null, ExpirationDate,
        TimeSpan.Zero, CacheItemPriority.Default, null);
		  System.Web.HttpContext.Current.Trace.Write("retrieved cache id from session...");
		}
		
		  #region WriteCache Overloads
		  private static void WriteCache(object Obj, string ID, DateTime ExpirationDate) {
		    WriteCache(Obj, ID, ExpirationDate, DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout));
		  }
		  #endregion
		
		private static void WriteCookie(object Obj, string ID, DateTime ExpirationDate, DateTime DefaultExpirationDate) {
		  System.Web.HttpContext context = System.Web.HttpContext.Current;
			context.Response.Cookies[ID].Value = SerializationTools.SerializeObject(Obj);
			if(ExpirationDate == SqlConvert.ToDateTime(null)) ExpirationDate = DefaultExpirationDate;
			context.Response.Cookies[ID].Expires = ExpirationDate;
		}
		
		  #region WriteCookie Overloads
		  private static void WriteCookie(object Obj, string ID, DateTime ExpirationDate) {
		    WriteCookie(Obj, ID, ExpirationDate, DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout));
		  }
		  #endregion
		#endregion
		#endregion
		
		
		#region Get
	  /// <summary>
	  /// Retrieves an object or piece of data from the Caching System.
	  /// </summary>
		public static object Get(string ID,CachePersistance Persistance) {
			object obj = null;
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			
			if (Persistance == CachePersistance.SessionCache) {
			  string strCacheId;
			  try {
			    strCacheId = context.Session[ID].ToString();
   			  context.Trace.Write("retrieved cache id from session...");
			  } catch {
			    // If the cache id isn't found, build one out of the session id and the id passed.
			    strCacheId = context.Session.SessionID+ "_" +ID;
			  }
			  context.Trace.Write("strCacheId = " +strCacheId);
			  try { obj = SerializationTools.DeserializeObject(context.Cache[strCacheId].ToString()); } catch { obj = null; }
			} else {
			  // This is the original code before the SessionCache Persistance.
			  if(Persistance >= CachePersistance.Session) obj = context.Session[ID];

			  if(Persistance == CachePersistance.Session && obj == null) {
				  if (context.Request.Cookies.Get(context.Session.SessionID + "_" + ID) == null)
					  obj = null;
				  else
				  {
					  obj = SerializationTools.DeserializeObject(context.Request.Cookies[context.Session.SessionID + "_" + ID].Value);
				  }
			  }

			  if(Persistance >= CachePersistance.Machine && obj == null) {
				  try {
					  obj = SerializationTools.DeserializeObject(context.Request.Cookies[ID].Value);
				  } catch {
					  obj = null;
				  }
			  }
			}
			
			return (obj);
		}
		#endregion
	}

	/// <summary>
	/// Enumerates cache persistance options
	/// </summary>
	public enum CachePersistance
	{
		/// <summary>
		/// Saves object to ASP.Net session, also backs up in cookie on users machine in case of application restart.
		/// </summary>
		Session = 1,
		
		/// <summary>
		/// Saves object to ASP.Net session, does not backup to cookie.
		/// </summary>
		SessionNoCookie = 2,
		
		/// <summary>
		/// Saves object to ASP.Net cache using a generated key, only the key is stored in the session backed with a cookie.
		/// </summary>
		SessionCache = 3,
		
		/// <summary>
		/// Saves object to ASP.Net session, and stores it in a cookie that is not linked to the session.
		/// </summary>
		Machine = 4,
		
		/// <summary>
		/// Saves object to cookie on users machine, does not save to ASP.Net session.
		/// </summary>
		MachineOnly = 5
	}
}
