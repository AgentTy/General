using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Environment.Host
{
    public class HostCache
    {
        private static string HostMarker = "_h0st_";
        private static Dictionary<string, HostCacheItemRemovedCallback> Callbacks = new Dictionary<string, HostCacheItemRemovedCallback>();
        public delegate void HostCacheItemRemovedCallback(string key, object value, System.Web.Caching.CacheItemRemovedReason reason, string host);

        public static List<string> AllHosts = new List<string>();

        #region This Accessor
        public virtual object this[string host, string key]
        {
            get
            {
                return System.Web.HttpRuntime.Cache[GetHostKey(host, key)];
            }
            set
            {
                System.Web.HttpRuntime.Cache[GetHostKey(host, key)] = value;
            }
        }

        public virtual object this[string key]
        {
            get
            {
                if (IsHostKey(key))
                    return System.Web.HttpRuntime.Cache[key];
                else
                    return System.Web.HttpRuntime.Cache[GetHostKey(key)];
            }
            set
            {
                if (IsHostKey(key))
                    System.Web.HttpRuntime.Cache[key] = value;
                else
                    System.Web.HttpRuntime.Cache[GetHostKey(key)] = value;
            }
        }
        #endregion

        #region Add
        public object Add(string host, string key, object value, DateTime absoluteExpiration)
        {
            return Add(host, key, value, null, absoluteExpiration, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
        }

        public object Add(string host, string key, object value, TimeSpan slidingExpiration)
        {
            return Add(host, key, value, null, DateTime.MinValue, slidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
        }

        public object Add(string host, string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, HostCacheItemRemovedCallback onRemoveCallback)
        {
            if(!IsHostKey(key))
                key = GetHostKey(host, key);

            if (onRemoveCallback != null)
            {
                Callbacks[key] = onRemoveCallback;
                return System.Web.HttpRuntime.Cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, new System.Web.Caching.CacheItemRemovedCallback(CacheItemRemoved));
            }
            else
            {
                return System.Web.HttpRuntime.Cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, null);
            }
        }

        public object Add(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, HostCacheItemRemovedCallback onRemoveCallback)
        {
            if (!IsHostKey(key))
                key = GetHostKey(key);

            if (onRemoveCallback != null)
            {
                Callbacks[key] = onRemoveCallback;
                return System.Web.HttpRuntime.Cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, new System.Web.Caching.CacheItemRemovedCallback(CacheItemRemoved));
            }
            else
            {
                return System.Web.HttpRuntime.Cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, null);
            }
        }
        #endregion

        #region Remove
        public object Remove(string key)
        {
            if (!IsHostKey(key))
                key = GetHostKey(key);
            if (Callbacks.ContainsKey(key))
                Callbacks.Remove(key);
            return System.Web.HttpRuntime.Cache.Remove(key);
        }

        public object Remove(string host, string key)
        {
            if (!IsHostKey(key))
                key = GetHostKey(host, key);
            if (Callbacks.ContainsKey(key))
                Callbacks.Remove(key);
            return System.Web.HttpRuntime.Cache.Remove(key);
        }
        #endregion

        #region RemoveAllHosts
        public int RemoveAllHosts(string key)
        {
            int intCount = 0;
            foreach (string host in AllHosts)
            {
                string hostkey = GetHostKey(host, key);
                if (Callbacks.ContainsKey(hostkey))
                    Callbacks.Remove(hostkey);
                System.Web.HttpRuntime.Cache.Remove(hostkey);
                intCount++;
            }
            return intCount;
        }
        #endregion

        #region CacheItemRemoved
        private static void CacheItemRemoved(string key, object value, System.Web.Caching.CacheItemRemovedReason reason)
        {
            if (Callbacks.ContainsKey(key))
            {
                if(String.IsNullOrEmpty(System.Threading.Thread.CurrentThread.Name))
                    System.Threading.Thread.CurrentThread.Name = key;
                Callbacks[key].Invoke(GetKeyFromHostKey(key), value, reason, GetHostFromHostKey(key));
            }
        }
        #endregion

        #region Functions
        private string GetHostKey(string key)
        {
            if (!AllHosts.Contains(HostState.CurrentHost))
                AllHosts.Add(HostState.CurrentHost);

            return key + HostMarker + HostState.CurrentHost; 
        }

        public static string GetHostKey(string host, string key)
        {
            if (!AllHosts.Contains(host))
                AllHosts.Add(host);

            return key + HostMarker + host;
        }

        public static bool IsHostKey(string key)
        {
            return key.Contains(HostMarker);
        }

        public static string GetHostFromHostKey(string key)
        {
            return StringFunctions.AllAfter(key, HostMarker);
        }

        private static string GetKeyFromHostKey(string key)
        {
            return key.Substring(0, key.IndexOf(HostMarker));
        }

        public bool ValidateHostKey(string key)
        {
            if (IsHostKey(key))
            {
                string strHost = GetHostFromHostKey(key);
                if (strHost != HostState.CurrentHost)
                {
                    return false;
                }
            }
            return true;
        }

        /*
        private bool HostMismatchCheck(string key)
        {
            if (IsHostKey(key))
            {
                string strHost = GetHostFromHostKey(key);
                if (strHost != CurrentHost)
                {
                    throw new InvalidOperationException("Host Cache stopped you from changing variables for a different host");
                }
                return true; //It was a HostKey, but a valid one
            }
            return false; //This was not a HostKey anyways
        }
        */
        #endregion

    }
}
