using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Environment.Host
{
    public class HostApplicationState
    {
        private static string HostMarker = "_h0st_";

        #region This Accessor
        public virtual object this[string name]
        {
            get 
            {
                return System.Web.HttpContext.Current.Application[GetHostKey(name)]; 
            }
            set
            {
                System.Web.HttpContext.Current.Application[GetHostKey(name)] = value;
            }
        }
        #endregion

        #region GetHostKey
        public string GetHostKey(string key)
        {
            return key + HostMarker + HostState.CurrentHost;
        }
        #endregion

    }
}
