using System;
using System.Xml;
using System.Web.Caching;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;

namespace General.Configuration
{
	/// <summary>
	/// Class for accessing application settings
	/// </summary>
	public sealed class GlobalConfiguration
	{

		#region Private Variables
		private static System.Collections.Specialized.NameValueCollection _objGlobalSettings;
        private static System.Collections.Specialized.NameValueCollection _objHostSettings;
        private static GlobalSettingsContainer _objGlobalSettingsContainer = new GlobalSettingsContainer();
        private static HostSettingsContainer _objHostSettingsContainer = new HostSettingsContainer();
		#endregion

		#region Global Settings
		private static NameValueCollection GetGlobalSettings()
		{
            object objCache = GetGlobalSettingsFromCache();
			if(objCache != null)
			{
				return (NameValueCollection) objCache;
			}
			else
			{
				NameValueCollection objCol = LoadGlobalSettings();
                AddGlobalSettingsToCache(objCol);
				return objCol;
			}
		}

		private static NameValueCollection LoadGlobalSettings()
		{
			NameValueCollection objCol;
			System.Configuration.ConfigXmlDocument objDoc = new System.Configuration.ConfigXmlDocument();

            try
            {
                if (System.IO.File.Exists(General.Environment.Current.GetBinDirectory() + "General.config"))
                {
                    objDoc.Load(General.Environment.Current.GetBinDirectory() + "General.config");
                    System.Configuration.NameValueSectionHandler nvsh = new System.Configuration.NameValueSectionHandler();
                    objCol = (NameValueCollection)nvsh.Create(null, null, objDoc.SelectSingleNode("General.Configuration/GlobalSettings"));
                }
                else if (System.IO.File.Exists(General.Environment.Current.GetAppDirectory() + "General.config"))
                {
                    objDoc.Load(General.Environment.Current.GetAppDirectory() + "General.config");
                    System.Configuration.NameValueSectionHandler nvsh = new System.Configuration.NameValueSectionHandler();
                    objCol = (NameValueCollection)nvsh.Create(null, null, objDoc.SelectSingleNode("General.Configuration/GlobalSettings"));
                }
                else
                {
                    throw new System.IO.FileNotFoundException("General.config could not be loaded");
                }
            }
            catch
            {
                objCol = (NameValueCollection)null;
            }
            return objCol;
		}

        private static object GetGlobalSettingsFromCache()
		{
            if (System.Web.HttpRuntime.Cache == null)
			{
				return _objGlobalSettings;
			}
			else
			{
                System.Web.Caching.Cache GlobalCache = System.Web.HttpRuntime.Cache;
				return GlobalCache.Get("General.Configuration.GlobalSettings");
			}
		}

		private static void AddGlobalSettingsToCache(NameValueCollection Obj)
		{
            if (System.Web.HttpRuntime.Cache == null)
				_objGlobalSettings = Obj;
			else
			{
				if(Obj != null)
				{
                    System.Web.Caching.Cache GlobalCache = System.Web.HttpRuntime.Cache;
					System.Web.Caching.CacheDependency objDepend = new System.Web.Caching.CacheDependency(General.Environment.Current.GetBinDirectory() + "General.config");
					GlobalCache.Add("General.Configuration.GlobalSettings",Obj,objDepend,DateTime.Now.AddMinutes(10),Cache.NoSlidingExpiration,CacheItemPriority.High,null);
				}
			}
		}
		#endregion

        #region Host Settings

        #region HostWhenNull
        private static string _strHostWhenNull;
        public static string HostWhenNull
        {
            get
            {
                return _strHostWhenNull;
            }
            set
            {
                _strHostWhenNull = value;
            }
        }
        #endregion

        #region GetCurrentHost
        public static string GetCurrentHost()
        {
            string strCurrentHost;
            if (General.Environment.Current.AmILive())
            {
                if(System.Web.HttpContext.Current != null)
                    strCurrentHost = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower();
                else if (!String.IsNullOrEmpty(System.Threading.Thread.CurrentThread.Name) && General.Environment.Host.HostCache.IsHostKey(System.Threading.Thread.CurrentThread.Name))
                    strCurrentHost = General.Environment.Host.HostCache.GetHostFromHostKey(System.Threading.Thread.CurrentThread.Name);
                else if (System.Web.HttpContext.Current == null && !StringFunctions.IsNullOrWhiteSpace(HostWhenNull))
                    strCurrentHost = HostWhenNull;
                else throw new Exception("CurrentHost is unavailable");
            }
            else
            {
                if (System.Web.HttpContext.Current == null && StringFunctions.IsNullOrWhiteSpace(HostWhenNull) && !(!String.IsNullOrEmpty(System.Threading.Thread.CurrentThread.Name) && General.Environment.Host.HostCache.IsHostKey(System.Threading.Thread.CurrentThread.Name)))
                    throw new Exception("In a Live environment I will encounter an error here, System.Web.HttpContext.Current will be null.");

                if (!StringFunctions.IsNullOrWhiteSpace(GlobalSettings.GetGlobalOnly("spoof_host")) && !General.Environment.Current.AmILive())
                    strCurrentHost = GlobalSettings.GetGlobalOnly("spoof_host").ToString().ToLower();
                else if (System.Web.HttpContext.Current != null)
                    strCurrentHost = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower();
                else if (!String.IsNullOrEmpty(System.Threading.Thread.CurrentThread.Name) && General.Environment.Host.HostCache.IsHostKey(System.Threading.Thread.CurrentThread.Name))
                    strCurrentHost = General.Environment.Host.HostCache.GetHostFromHostKey(System.Threading.Thread.CurrentThread.Name);
                else if (System.Web.HttpContext.Current == null && !StringFunctions.IsNullOrWhiteSpace(HostWhenNull))
                    strCurrentHost = HostWhenNull;
                else throw new Exception("CurrentHost is unavailable");
            }
            strCurrentHost = strCurrentHost.Replace("http://", "");
            strCurrentHost = strCurrentHost.Replace("https://", "");
            return strCurrentHost;
        }
        #endregion

        public static NameValueCollection GetHostSettings(string host)
        {
            object objCache = null;
            if (System.Web.HttpRuntime.Cache != null)
                objCache = GetHostSettingsFromCache(host);

            if (objCache != null)
            {
                return (NameValueCollection)objCache;
            }
            else
            {
                NameValueCollection objCol = LoadHostSettings(host);
                if (System.Web.HttpRuntime.Cache != null)
                    AddHostSettingsToCache(host, objCol);
                return objCol;
            }
        }

        private static NameValueCollection LoadHostSettings(string host)
        {
            NameValueCollection objCol;
            System.Configuration.ConfigXmlDocument objDoc = new System.Configuration.ConfigXmlDocument();

            try
            {
                if (System.IO.File.Exists(General.Environment.Current.GetBinDirectory() + "General.config"))
                {
                    objDoc.Load(General.Environment.Current.GetBinDirectory() + "General.config");
                    XmlNode node = objDoc.SelectSingleNode("//HostSettings" + "[@host='" + host + "']");
                    if (node == null)
                        return new NameValueCollection();
                    node.Attributes.RemoveAll();
                    System.Configuration.NameValueSectionHandler nvsh = new System.Configuration.NameValueSectionHandler();
                    objCol = (NameValueCollection)nvsh.Create(null, null, node);
                }
                else if (System.IO.File.Exists(General.Environment.Current.GetAppDirectory() + "General.config"))
                {
                    objDoc.Load(General.Environment.Current.GetAppDirectory() + "General.config");
                    XmlNode node = objDoc.SelectSingleNode("//HostSettings" + "[@host='" + host + "']");
                    if (node == null)
                        return new NameValueCollection();
                    node.Attributes.RemoveAll();
                    System.Configuration.NameValueSectionHandler nvsh = new System.Configuration.NameValueSectionHandler();
                    objCol = (NameValueCollection)nvsh.Create(null, null, node);
                }
                else
                {
                    objCol = (NameValueCollection)null;
                }
            }
            catch
            {
                objCol = (NameValueCollection)null;
            }
            return objCol;

            /*
             This is the old code that throws exceptions all the time
            try
            {
                objDoc.Load(General.Environment.Current.GetBinDirectory() + "General.config");
                XmlNode node = objDoc.SelectSingleNode("//HostSettings" + "[@host='" + GetCurrentHost() + "']");
                if (node == null)
                    return null;
                node.Attributes.RemoveAll();
                System.Configuration.NameValueSectionHandler nvsh = new System.Configuration.NameValueSectionHandler();
                objCol = (NameValueCollection)nvsh.Create(null, null, node);
            }
            catch
            {
                try
                {
                    objDoc.Load(General.Environment.Current.GetAppDirectory() + "General.config");
                    XmlNode node = objDoc.SelectSingleNode("//HostSettings" + "[@host='" + GetCurrentHost() + "']");
                    if (node == null)
                        return null;
                    node.Attributes.RemoveAll();
                    System.Configuration.NameValueSectionHandler nvsh = new System.Configuration.NameValueSectionHandler();
                    objCol = (NameValueCollection)nvsh.Create(null, null, node);
                }
                catch
                {
                    objCol = (NameValueCollection)null;
                }
            }
            return objCol;
            */
        }

        private static object GetHostSettingsFromCache(string host)
        {
            System.Web.Caching.Cache GlobalCache = System.Web.HttpRuntime.Cache;
            return GlobalCache.Get("General.Configuration.HostSettings." + host);
        }

        private static void AddHostSettingsToCache(string host, NameValueCollection Obj)
        {
            if (System.Web.HttpRuntime.Cache == null)
                _objHostSettings = Obj;
            else
            {
                if (Obj != null)
                {
                    System.Web.Caching.Cache GlobalCache = System.Web.HttpRuntime.Cache;
                    System.Web.Caching.CacheDependency objDepend = new System.Web.Caching.CacheDependency(General.Environment.Current.GetBinDirectory() + "General.config");
                    GlobalCache.Add("General.Configuration.HostSettings." + host, Obj, objDepend, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }
        }
        #endregion

		#region Public Properties
        public static bool GetFromAppConfig { get; set; }
        public static string DefaultConnectionStringName { get; set; }

        private static bool _blnGetFromAppConfigFirst = true;
        public static bool GetFromAppConfigFirst
        {
            get { return _blnGetFromAppConfigFirst; }
            set { _blnGetFromAppConfigFirst = value; }
        }

        public static String AppConfigBranch { get; set; }

		/// <summary>
		/// This static object will return an object from a specified key, it will look first in the applications config file (Web.config), then if not found it will look in the global config file (/bin/General.config)
		/// </summary>
        public static GlobalSettingsContainer GlobalSettings
		{
			get
			{
                return _objGlobalSettingsContainer;
			}
		}

		/// <summary>
		/// This static object will return an object from a specified key directly from the global config file (/bin/General.config)
		/// </summary>
		public static NameValueCollection GlobalSettingsExplicit
		{
			get
			{
				return GetGlobalSettings();
			}
		}

        /// <summary>
        /// This static object will return an object from a specified key, it will look first in the applications config file (Web.config), then if not found it will look in the global config file (/bin/General.config)
        /// </summary>
        public static HostSettingsContainer HostSettings
        {
            get
            {
                return _objHostSettingsContainer;
            }
        }

        /// <summary>
        /// This static object will return an object from a specified key directly from the global config file (/bin/General.config)
        /// </summary>
        public static NameValueCollection HostSettingsExplicit
        {
            get
            {
                string host = GetCurrentHost();
                return GetHostSettings(host);
            }
        }
		#endregion

		#region Constructors
		/// <summary>
		/// Class for accessing application settings
		/// </summary>
		public GlobalConfiguration()
		{}
		#endregion

        #region GlobalSettings Container Class
        /// <summary>
		/// This class will return an object from a specified key, it will look first in the applications config file (Web.config), then if not found it will look in the global config file (/bin/General.config)
		/// </summary>
        public sealed class GlobalSettingsContainer
		{
			/// <summary>
			/// This accessor will return an object from a specified key, it will look first in the applications config file (Web.config), then if not found it will look in the global config file (/bin/General.config)
			/// </summary>
			public string this[string Key]
			{
				get
				{
                    if (GetFromAppConfig)
                    {
                        if (!StringFunctions.IsNullOrWhiteSpace(AppConfigBranch))
                            return ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection(AppConfigBranch))[Key];
                        else
                            return System.Configuration.ConfigurationManager.AppSettings[Key];
                    }
                    else if (GetFromAppConfigFirst && System.Configuration.ConfigurationManager.AppSettings[Key] != null)
                        return System.Configuration.ConfigurationManager.AppSettings[Key];
                    else if (System.Web.HttpContext.Current != null || !StringFunctions.IsNullOrWhiteSpace(HostWhenNull))
                    {  //I still have to check for HttpContext.Current and HostWhenNull ONLY because otherwise I don't know which Host I'm working with
                        string host = GetCurrentHost();
                        if (GetHostSettings(host) != null && GetHostSettings(host)[Key] != null)
                            return GetHostSettings(host)[Key];
                        else
                        {
                            try
                            {
                                return GetGlobalSettings()[Key];
                            }
                            catch
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            return GetGlobalSettings()[Key];
                        }
                        catch
                        {
                            return null;
                        }
                    }
				}
			}

            public string GetGlobalOnly(string Key)
            {
                try
                {
                    if (GetFromAppConfig)
                    {
                        if (!StringFunctions.IsNullOrWhiteSpace(AppConfigBranch))
                            return ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection(AppConfigBranch))[Key];
                        else
                            return System.Configuration.ConfigurationManager.AppSettings[Key];
                    }
                    else if (GetFromAppConfigFirst && System.Configuration.ConfigurationManager.AppSettings[Key] != null)
                        return System.Configuration.ConfigurationManager.AppSettings[Key];
                    else
                        return GetGlobalSettings()[Key];
                }
                catch
                {
                    return null;
                }
            }

            public string GetByEnviromnentModifers(string Key)
            {
                if (!StringFunctions.IsNullOrWhiteSpace(this[Key]))
                    return this[Key];
                else if (!StringFunctions.IsNullOrWhiteSpace(this[Key + "_" + General.Environment.Current.WhereAmI()]))
                    return this[Key + "_" + General.Environment.Current.WhereAmI()];
                else
                    return null;
            }

		}
		#endregion

        #region HostSettings Container Class
        /// <summary>
        /// This class will return an object from a specified key, it will look first in the applications config file (Web.config), then if not found it will look in the global config file (/bin/General.config)
        /// </summary>
        public sealed class HostSettingsContainer
        {
            /// <summary>
            /// This accessor will return an object from a specified key, it will look first in the applications config file (Web.config), then if not found it will look in the global config file (/bin/General.config)
            /// </summary>
            public string this[string Key]
            {
                get
                {
                    try
                    {
                        string host = GetCurrentHost();
                        return GetHostSettings(host)[Key];
                    }
                    catch
                    {
                        return null;
                    }
                    
                }
            }
        }
        #endregion

        #region GetClientID
        private static string ClientIDKey = "client_id";
        public static int? GetClientID()
        {
            var strClientID = GlobalSettings[ClientIDKey];
            if (!String.IsNullOrEmpty(strClientID))
            {
                return int.Parse(strClientID);
            }
            else
            {
                return null;
            }
        }

        public static int? GetClientID(string strHost)
        {
            var strClientID = GetHostSettings(strHost)[ClientIDKey];
            if (!String.IsNullOrEmpty(strClientID))
            {
                return int.Parse(strClientID);
            }
            else
            {
                return null;
            }
        }
        #endregion

    }

}
