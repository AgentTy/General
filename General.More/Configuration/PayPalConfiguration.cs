using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace General.Configuration
{
    public class PayPalConfiguration
    {

        #region GetAccountAndConfig
        public static Dictionary<string, string> GetAccountAndConfig()
        {
            Dictionary<string, string> configMap = GetConfigOnly();
            #region Sample
            // Signature Credential
            // configMap.Add("account1.apiUsername", "jb-us-seller_api1.paypal.com");
            // configMap.Add("account1.apiPassword", "WX4WTU3S8MY44S7F");
            // configMap.Add("account1.apiSignature", "AFcWxV21C7fd0v3bYYYRCpSSRl31A7yDhhsPUU2XhtMoZXsWHFxu-RWy");
            // Optional
            // configMap.Add("account1.Subject", "");

            // Sample Certificate Credential
            // configMap.Add("account2.apiUsername", "certuser_biz_api1.paypal.com");
            // configMap.Add("account2.apiPassword", "D6JNKKULHN3G5B8A");
            // configMap.Add("account2.apiCertificate", "resource/sdk-cert.p12");
            // configMap.Add("account2.privateKeyPassword", "password");
            // Optional
            // configMap.Add("account2.Subject", "");
            #endregion

            if (configMap["mode"] == "sandbox" && (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_ApiUsername_SandBox"]) || !String.IsNullOrEmpty(GlobalConfiguration.GlobalSettings["PayPal_ApiUsername_SandBox"])))
            {
                #region Explicit Sandbox Credentials
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_ApiUsername_SandBox"]))
                {
                    configMap.Add("account1.apiUsername", ConfigurationManager.AppSettings["PayPal_ApiUsername_SandBox"]);
                    configMap.Add("account1.apiPassword", ConfigurationManager.AppSettings["PayPal_ApiPassword_SandBox"]);

                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_ApiSignature_SandBox"]))
                        configMap.Add("account1.apiSignature", ConfigurationManager.AppSettings["PayPal_ApiSignature_SandBox"]);
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_ApiCertificate_SandBox"]))
                        configMap.Add("account1.apiCertificate", ConfigurationManager.AppSettings["PayPal_ApiCertificate_SandBox"]);
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_ApiPrivateKeyPassword_SandBox"]))
                        configMap.Add("account1.privateKeyPassword", ConfigurationManager.AppSettings["PayPal_ApiPrivateKeyPassword_SandBox"]);
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_ApiSubject_SandBox"]))
                        configMap.Add("account1.Subject", ConfigurationManager.AppSettings["PayPal_ApiSubject_SandBox"]);
                }
                else
                {
                    configMap.Add("account1.apiUsername", GlobalConfiguration.GlobalSettings["PayPal_ApiUsername_SandBox"]);
                    configMap.Add("account1.apiPassword", GlobalConfiguration.GlobalSettings["PayPal_ApiPassword_SandBox"]);

                    if (!String.IsNullOrEmpty(GlobalConfiguration.GlobalSettings["PayPal_ApiSignature_SandBox"]))
                        configMap.Add("account1.apiSignature", GlobalConfiguration.GlobalSettings["PayPal_ApiSignature_SandBox"]);
                    if (!String.IsNullOrEmpty(GlobalConfiguration.GlobalSettings["PayPal_ApiCertificate_SandBox"]))
                        configMap.Add("account1.apiCertificate", GlobalConfiguration.GlobalSettings["PayPal_ApiCertificate_SandBox"]);
                    if (!String.IsNullOrEmpty(GlobalConfiguration.GlobalSettings["PayPal_ApiPrivateKeyPassword_SandBox"]))
                        configMap.Add("account1.privateKeyPassword", GlobalConfiguration.GlobalSettings["PayPal_ApiPrivateKeyPassword_SandBox"]);
                    if (!String.IsNullOrEmpty(GlobalConfiguration.GlobalSettings["PayPal_ApiSubject_SandBox"]))
                        configMap.Add("account1.Subject", GlobalConfiguration.GlobalSettings["PayPal_ApiSubject_SandBox"]);
                }
                #endregion
            }
            else
            {
                #region Normal Credentials
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_ApiUsername"]))
                {
                    configMap.Add("account1.apiUsername", ConfigurationManager.AppSettings["PayPal_ApiUsername"]);
                    configMap.Add("account1.apiPassword", ConfigurationManager.AppSettings["PayPal_ApiPassword"]);

                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_ApiSignature"]))
                        configMap.Add("account1.apiSignature", ConfigurationManager.AppSettings["PayPal_ApiSignature"]);
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_ApiCertificate"]))
                        configMap.Add("account1.apiCertificate", ConfigurationManager.AppSettings["PayPal_ApiCertificate"]);
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_ApiPrivateKeyPassword"]))
                        configMap.Add("account1.privateKeyPassword", ConfigurationManager.AppSettings["PayPal_ApiPrivateKeyPassword"]);
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_ApiSubject"]))
                        configMap.Add("account1.Subject", ConfigurationManager.AppSettings["PayPal_ApiSubject"]);
                }
                else
                {
                    configMap.Add("account1.apiUsername", General.Configuration.GlobalConfiguration.GlobalSettings["PayPal_ApiUsername"]);
                    configMap.Add("account1.apiPassword", General.Configuration.GlobalConfiguration.GlobalSettings["PayPal_ApiPassword"]);

                    if (!String.IsNullOrEmpty(GlobalConfiguration.GlobalSettings["PayPal_ApiSignature"]))
                        configMap.Add("account1.apiSignature", GlobalConfiguration.GlobalSettings["PayPal_ApiSignature"]);
                    if (!String.IsNullOrEmpty(GlobalConfiguration.GlobalSettings["PayPal_ApiCertificate"]))
                        configMap.Add("account1.apiCertificate", GlobalConfiguration.GlobalSettings["PayPal_ApiCertificate"]);
                    if (!String.IsNullOrEmpty(GlobalConfiguration.GlobalSettings["PayPal_ApiPrivateKeyPassword"]))
                        configMap.Add("account1.privateKeyPassword", GlobalConfiguration.GlobalSettings["PayPal_ApiPrivateKeyPassword"]);
                    if (!String.IsNullOrEmpty(GlobalConfiguration.GlobalSettings["PayPal_ApiSubject"]))
                        configMap.Add("account1.Subject", GlobalConfiguration.GlobalSettings["PayPal_ApiSubject"]);
                }
                #endregion
            }
            return configMap;
        }
        #endregion

        #region GetConfigOnly
        public static Dictionary<string, string> GetConfigOnly()
        {
            // Configuration map containing signature credentials and other required configuration.
            // For a full list of configuration parameters refer in wiki page 
            // [https://github.com/paypal/sdk-core-dotnet/wiki/SDK-Configuration-Parameters]

            Dictionary<string, string> configMap = new Dictionary<string, string>();

            if(Environment.Current.AmIBeta())
                configMap.Add("mode", "live");
            else if(Environment.Current.AmILive())
                configMap.Add("mode", "live");
            else
                configMap.Add("mode", "sandbox");

            //This will check for PayPal_APISandBoxMode being explicitly set to use SandBox instead of Live
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["PayPal_APISandBoxMode"]))
            {
                bool blnSandBoxOverride = bool.Parse(ConfigurationManager.AppSettings["PayPal_APISandBoxMode"]);
                if (blnSandBoxOverride)
                    configMap["mode"] = "sandbox";
            }
            else if (!String.IsNullOrEmpty(GlobalConfiguration.GlobalSettings["PayPal_APISandBoxMode"]))
            {
                bool blnSandBoxOverride = bool.Parse(GlobalConfiguration.GlobalSettings["PayPal_APISandBoxMode"]);
                if (blnSandBoxOverride)
                    configMap["mode"] = "sandbox";
            }

            // These values are defaulted in SDK. If you want to override default values, uncomment it and add your value.
            // configMap.Add("connectionTimeout", "5000");
            // configMap.Add("requestRetries", "2");

            return configMap;
        }
        #endregion

    }
}
