using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General.Environment.Host;

namespace General.Environment
{
    public class HostState
    {

        #region CurrentHost
        public static string CurrentHost
        {
            get
            {
                return Configuration.GlobalConfiguration.GetCurrentHost();
            }
        }
        #endregion

        public static HostApplicationState Application = new HostApplicationState();
        public static HostCache Cache = new HostCache();

    }
}
