using System;
using System.Xml;
using System.Runtime.Caching;
//using System.Configuration;
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
        #endregion

        #region Global Settings
        /*
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
                    objCol = null;
                    //throw new System.IO.FileNotFoundException("General.config could not be loaded");
                }
            }
            catch
            {
                objCol = null;
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
                    System.Web.Caching.CacheDependency objDepend = null;
                    if (System.IO.File.Exists(General.Environment.Current.GetBinDirectory() + "General.config"))
                    {
                        objDepend = new System.Web.Caching.CacheDependency(General.Environment.Current.GetBinDirectory() + "General.config");
                    }
                    else if (System.IO.File.Exists(General.Environment.Current.GetAppDirectory() + "General.config"))
                    {
                        objDepend = new System.Web.Caching.CacheDependency(General.Environment.Current.GetAppDirectory() + "General.config");
                    }

                    if (objDepend != null)
                        GlobalCache.Add("General.Configuration.GlobalSettings", Obj, objDepend, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                    else
                        _objGlobalSettings = Obj;
				}
			}
		}
        */
        #endregion

		#region Public Properties
        //public static bool GetFromAppConfig { get; set; }
        public static string DefaultConnectionStringName { get; set; }
        public static string ExplicitRuntimeConnectionString { get; set; }

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
                    return GetAppSettingOrEnvironmentalVariable(Key);
                    /*
                    if (GetFromAppConfig)
                    {
                        if (!String.IsNullOrWhiteSpace(AppConfigBranch))
                            return ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection(AppConfigBranch))[Key];
                        else
                            return GetAppSettingOrEnvironmentalVariable(Key);
                    }
                    else if (GetFromAppConfigFirst && GetAppSettingOrEnvironmentalVariable(Key) != null)
                        return GetAppSettingOrEnvironmentalVariable(Key);
                    else if (System.Web.HttpContext.Current != null || !String.IsNullOrWhiteSpace(HostWhenNull))
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
                    */
                }
			}

            
            public string GetGlobalOnly(string Key)
            {
                return GetAppSettingOrEnvironmentalVariable(Key);
                /*
                try
                {
                    if (GetFromAppConfig)
                    {
                        if (!String.IsNullOrWhiteSpace(AppConfigBranch))
                            return ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection(AppConfigBranch))[Key];
                        else
                            return GetAppSettingOrEnvironmentalVariable(Key);
                    }
                    else if (GetFromAppConfigFirst && GetAppSettingOrEnvironmentalVariable(Key) != null)
                        return GetAppSettingOrEnvironmentalVariable(Key);
                    else
                    {
                        var settings = GetGlobalSettings();
                        if (settings != null)
                            return settings[Key];
                        else return null;
                    }
                }
                catch
                {
                    return null;
                }
                */
            }
            

            public string GetByEnviromnentModifers(string Key)
            {
                if (!String.IsNullOrWhiteSpace(this[Key]))
                    return this[Key];
                else if (!String.IsNullOrWhiteSpace(this[Key + "_" + General.Environment.Current.WhereAmI()]))
                    return this[Key + "_" + General.Environment.Current.WhereAmI()];
                else
                    return null;
            }

            public string GetAppSettingOrEnvironmentalVariable(string Key, bool UseOldSysConfigLibrary = false)
            {
                string value;
                value = System.Environment.GetEnvironmentVariable(Key);
                if (!String.IsNullOrWhiteSpace(value))
                    return value;

                /*
                if (!UseOldSysConfigLibrary)
                    return null;

                if (System.Configuration.ConfigurationManager.AppSettings[Key] != null)
                    value = System.Configuration.ConfigurationManager.AppSettings[Key];
                if (!String.IsNullOrWhiteSpace(value))
                    return value;
                else
                    value = null;
                */

                return value;
            }

		}
		#endregion


    }

}
