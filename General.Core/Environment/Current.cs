using System;
using General.Configuration;

namespace General.Environment
{
	/// <summary>
	/// Current Environment
	/// </summary>
	public class Current
    {

        #region Settings

        private static string _strHostingEnvironment;
        public static string HostingEnvironment {
            get {
                /*
                //Support for environmental variables in Azure
                string azureValue = System.Environment.GetEnvironmentVariable("HostingEnvironment");
                if (!String.IsNullOrWhiteSpace(azureValue))
                    return azureValue;
                azureValue = System.Environment.GetEnvironmentVariable("hosting_environment");
                if (!String.IsNullOrWhiteSpace(azureValue))
                    return azureValue;
                */
                if (_strHostingEnvironment != null)
                    return _strHostingEnvironment;
                _strHostingEnvironment = GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("HostingEnvironment") ?? GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("hosting_environment") ?? "";
                return _strHostingEnvironment;
            }
        }
        public static string ServerNameList_Dev { get { return GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("ServerNameList_Dev") ?? ""; } }
        public static string ServerNameList_QA { get { return GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("ServerNameList_QA") ?? ""; } }
        public static string ServerNameList_Staging { get { return GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("ServerNameList_Staging") ?? ""; } }
        public static string ServerNameList_CustomEnv { get { return GlobalConfiguration.GlobalSettings.GetAppSettingOrEnvironmentalVariable("ServerNameList_CustomEnv") ?? ""; } }

        #endregion
        
        #region Constructor
        /// <summary>
		/// Current Environment
		/// </summary>
		public Current()
		{

        }
        #endregion

        #region WhereAmI

        #region Shortcuts
        /// <summary>
		/// Returns false if the current MachineName is in the list of Developement Servers or Stage Servers
		/// </summary>
		public static bool AmILive()
		{
			return(WhereAmI() == EnvironmentContext.Live);
		}

		/// <summary>
		/// Returns true if the current MachineName is in the list of Developement Servers
		/// </summary>
		public static bool AmIDev()
		{
			return(WhereAmI() == EnvironmentContext.Dev);
		}

        /// <summary>
        /// Returns true if the current MachineName is in the list of Developement Servers
        /// </summary>
        public static bool AmIQA()
        {
            return (WhereAmI() == EnvironmentContext.QA);
        }

		/// <summary>
		/// Returns true if the current MachineName is in the list of Stage Servers
		/// </summary>
		public static bool AmIStage()
		{
			return(WhereAmI() == EnvironmentContext.Stage);
		}

        /// <summary>
        /// Returns true if the current MachineName is in the list of Stage Servers
        /// </summary>
        public static bool AmICustomEnv()
        {
            return (WhereAmI() == EnvironmentContext.CustomEnv);
        }
        #endregion


		/// <summary>
		/// Returns an EnvironmentContext Enumeration based on the current server name compared to the list of Developement, QA and Stage servers.
		/// </summary>
		public static EnvironmentContext WhereAmI()
		{

            if(!String.IsNullOrEmpty(HostingEnvironment))
            {
                EnvironmentContext manualEnv = (EnvironmentContext) Enum.Parse(typeof(EnvironmentContext), HostingEnvironment, true);
                return manualEnv;
            }

            //.Net Core Support
            string coreEnv = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (!String.IsNullOrWhiteSpace(coreEnv))
            {
                switch(coreEnv)
                {
                    case "Development":
                    case "Dev":
                        return EnvironmentContext.Dev;
                    case "QA":
                        return EnvironmentContext.QA;
                    case "Staging":
                    case "Stage":
                        return EnvironmentContext.Stage;
                    default:
                        return EnvironmentContext.Live;
                }
            }

            //.Net Framework Support
            string server_name = System.Environment.MachineName.ToUpperInvariant();

            string dev_list = ServerNameList_Dev.ToUpperInvariant();
            string qa_list = ServerNameList_QA.ToUpperInvariant();
            string stage_list = ServerNameList_Staging.ToUpperInvariant();
            string custom_list = ServerNameList_CustomEnv.ToUpperInvariant();

            if (!String.IsNullOrEmpty(dev_list))
            {
                string[] list = dev_list.Split(',');
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == server_name)
                        return (EnvironmentContext.Dev);
                }
            }

            if (!String.IsNullOrEmpty(qa_list))
            {
                string[] list = qa_list.Split(',');
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == server_name)
                        return (EnvironmentContext.QA);
                }
            }

            if (!String.IsNullOrEmpty(stage_list))
            {
                string[] list = stage_list.Split(',');
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == server_name)
                        return (EnvironmentContext.Stage);
                }
            }

            if (!String.IsNullOrEmpty(custom_list))
            {
                string[] list = custom_list.Split(',');
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == server_name)
                        return (EnvironmentContext.CustomEnv);
                }
            }

			return EnvironmentContext.Live;
        }
        #endregion

        #region GetAppDirectory
        /// <summary>
		/// Returns the root directory of the current application
		/// </summary>
		public static string GetAppDirectory()
		{
			return System.AppDomain.CurrentDomain.BaseDirectory.Replace("/","\\");
        }
        #endregion

        #region GetBinDirectory
        /// <summary>
		/// Returns the config directory of the current application
		/// </summary>
		public static string GetBinDirectory()
		{
			if(StringFunctions.Contains(System.AppDomain.CurrentDomain.BaseDirectory.ToLower(),"\\bin\\"))
				return System.AppDomain.CurrentDomain.BaseDirectory.Replace("/","\\");
			else
				return System.AppDomain.CurrentDomain.BaseDirectory.Replace("/","\\") + "bin" + "\\";
        }
        #endregion

    }

	public enum EnvironmentContext
	{
		Dev = 1,
        QA = 2,
		Stage = 3,
        CustomEnv = 4,
        Live = 5
	}
}
