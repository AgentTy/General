using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;


namespace General
{
    public class ScriptManager
    {

        public static bool AlwaysCompress = false;

        #region ScriptPreRender
        public static void ScriptPreRender(System.Web.UI.ScriptManager objScriptManager)
        {
            if (!objScriptManager.IsInAsyncPostBack) //I added this on 7/1/14, it seems like a safe idea
            {

                #region Debugging Clock
                DateTime dtStart = DateTime.Now;
                #endregion

                string strPage = GetPageKey(objScriptManager);
                string strFileRelative = "Scripts/Compiled/" + strPage + ".js";
                string strFile = objScriptManager.Page.MapPath(strFileRelative);

                if (objScriptManager.Scripts.Count > 0)
                {
                    if (!File.Exists(strFile))
                    {
                        //General.Debug.Write("Starting Script Compilation");
                        CompileClientScript(objScriptManager);
                        //General.Debug.Write("Finished Script Compilation");
                    }
                    else
                    {
                        if (!HashMatch(objScriptManager))
                        {
                            //General.Debug.Write("Starting Script Re-Compilation");
                            CompileClientScript(objScriptManager);
                            //General.Debug.Write("Finished Script Re-Compilation");
                        }
                    }
                    objScriptManager.Scripts.Clear();
                    if (objScriptManager.CompositeScript.Scripts.Count > 0)
                        objScriptManager.CompositeScript.Scripts.Add(new System.Web.UI.ScriptReference(strFileRelative));
                    else
                        objScriptManager.Scripts.Add(new System.Web.UI.ScriptReference(strFileRelative));
                }
                else
                {
                    if (File.Exists(strFile))
                    {
                        //General.Debug.Write("Deleting Old Script");
                        File.Delete(strFile);
                    }
                }

                #region Debugging Clock
                DateTime dtEnd = DateTime.Now;
                TimeSpan time = dtEnd - dtStart;
                //General.Debug.JQueryDebugWrite("ScriptPreRender (" + time.TotalMilliseconds + "ms)");
                #endregion

            }

        }
        #endregion

        #region ScriptUnload
        public static void ScriptUnload(System.Web.UI.ScriptManager objScriptManager)
        {
            /*
            if (General.Environment.Current.ExecuteJavascriptQueue != String.Empty)
            {
                objScriptManager.Page.RegisterStartupScript("DLLLibraryScripts", "<script type=\"text/javascript\">" + General.Environment.Current.ExecuteJavascriptQueue + "</script>");
                //System.Web.HttpContext.Current.Response.Write("<script type=\"text/javascript\">" + General.Environment.Current.ExecuteJavascriptQueue + "</script>");
                General.Environment.Current.ExecuteJavascriptQueue = String.Empty;
            }
            */
        }
        #endregion

        #region HashMatch
        private static bool HashMatch(System.Web.UI.ScriptManager objScriptManager)
        {
            //General.Debug.Trace("Starting Hash Match");
            string strPage = GetPageKey(objScriptManager);
            string strTargetFile = objScriptManager.Page.MapPath("Scripts/Compiled/" + strPage + ".js");
            string strData = File.ReadAllText(strTargetFile);
            strData = StringFunctions.AllBetween(strData, "<HASH>", "</HASH>");
            if (StringFunctions.IsNullOrWhiteSpace(strData))
                return false;

            string[] aryHashKeys = strData.Split(',');
            if (aryHashKeys.Length != objScriptManager.Scripts.Count)
                return false;

            foreach (string strHashKey in aryHashKeys)
            {
                string[] aryTemp = strHashKey.Split(':');
                string strFile = aryTemp[0];

                if (!AlwaysCompress)
                {
                    System.Web.UI.ScriptReference objRef = FindScriptReference(strFile, objScriptManager);
                    
                    if (objRef == null)
                        return false;

                    bool blnCompressed = General.Data.SqlConvert.ToBoolean(aryTemp[2]);
                    if (blnCompressed != IsCompressed(strFile, objScriptManager))
                        return false;
                }

                string strOldHash = aryTemp[1];
                string strNewHash = GetHashString(objScriptManager.Page.MapPath(strFile));

                if (strOldHash != strNewHash)
                    return false;

            }
            //General.Debug.Trace("Finished Hash Match");
            return true;
        }
        #endregion

        #region IsCompressed
        private static bool IsCompressed(string strFile, System.Web.UI.ScriptManager objScriptManager)
        {
            return IsCompressed(FindScriptReference(strFile, objScriptManager));
        }

        private static bool IsCompressed(System.Web.UI.ScriptReference objRef)
        {
            if (objRef.ScriptMode == System.Web.UI.ScriptMode.Debug)
                return false;
            else
                return true;
        }
        #endregion

        #region FindScriptReference
        private static System.Web.UI.ScriptReference FindScriptReference(string strFile, System.Web.UI.ScriptManager objScriptManager)
        {
            foreach (System.Web.UI.ScriptReference objRef in objScriptManager.Scripts)
            {
                if (objRef.Path == strFile)
                    return objRef;
            }

            return null;
        }
        #endregion

        #region GetHashString
        private static string GetHashString(string strFile)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider objHash = new System.Security.Cryptography.MD5CryptoServiceProvider();
            FileStream fs = File.OpenRead(strFile);
            objHash.ComputeHash(fs);
            fs.Close();
            fs.Dispose();
            //return General.Serialization.SerializationTools.SerializeObject(objHash.Hash);
            return Convert.ToBase64String(objHash.Hash); //I made this change on 3/18/15 to avoid including the SerializationTools class in General
        }
        #endregion

        #region CompileClientScript
        private static TextLog _objLog = new TextLog(); 
        public static FileInfo CompileClientScript(System.Web.UI.ScriptManager objScriptManager)
        {
            _objLog.Clear();
            ArrayList objScripts = new ArrayList();
            string strBody = String.Empty;
            string strNoCompressBody = String.Empty;
            string strHeader = "/*DO NOT REMOVE!! THIS CODE IS USED FOR VERSION CHECKING::: <HASH>";
            string strPage = GetPageKey(objScriptManager);
            string strTargetFolder = objScriptManager.Page.MapPath("Scripts/Compiled/");
            string strTargetFile = strTargetFolder + strPage + ".js";

            

            foreach (System.Web.UI.ScriptReference objRef in objScriptManager.Scripts)
            {
                string strFile = objScriptManager.Page.MapPath(objRef.Path);
                strHeader += objRef.Path + ":" + GetHashString(strFile) + ":" + IsCompressed(objRef) + ",";

                if (AlwaysCompress)
                {
                    strBody += File.ReadAllText(strFile) + "\r\n\r\n";
                }
                else
                {
                    if (!IsCompressed(objRef))
                        strNoCompressBody += File.ReadAllText(strFile) + "\r\n\r\n";
                    else
                        strBody += File.ReadAllText(strFile) + "\r\n\r\n";
                }

            }
            if(StringFunctions.Right(strHeader,1) == ",")
                strHeader = StringFunctions.Shave(strHeader, 1); //Remove last comma

            strHeader += "</HASH>*/";

            //Compression DISABLED 7/2014 for being buggy!!!
            //strBody = General.Utilities.Web.Compression.WebCompress(strBody, General.Utilities.Web.Compression.EnumContentType.Javascript);
            strBody = strNoCompressBody + strBody;
            //_objLog.Write(General.Utilities.Web.Compression.Log.ToString() + StringFunctions.NewLine);
            if (!System.IO.Directory.Exists( strTargetFolder ))
            {
                Directory.CreateDirectory(strTargetFolder);
            }
            FileStream objStream = new FileStream(strTargetFile, FileMode.Create);
            StreamWriter objWriter = new StreamWriter(objStream);
            objWriter.Write(strHeader);
            objWriter.Write(strBody);
            objWriter.Close();
            objWriter.Dispose();
            objStream.Close();
            objStream.Dispose();

            try
            {
                //General.Debugging.Report.SendDebug("Compiling Javascript File: " + strTargetFile, Log.ToString());
            }
            catch { }

            return new FileInfo(strTargetFile);
        }
        #endregion

        #region Log
        public static TextLog Log
        {
            get { return _objLog; }
        }
        #endregion

        #region CompileClientScript OLD
        /*
        public static FileInfo CompileClientScript(string strWebPage, string strTargetFile)
        {
            ArrayList objScripts = new ArrayList();
            string source = File.ReadAllText(strWebPage);
            string directory;
            string strBody = "";
            FileInfo objPageInfo = new FileInfo(strWebPage);
            directory = objPageInfo.DirectoryName;

            while (source.Contains("<asp:ScriptReference"))
            {
                /-*<asp:ScriptReference Path="Scripts/Common.js" />*-/
                string key = StringFunctions.AllBetween(source,"<asp:ScriptReference",">",true);
                string script = StringFunctions.AllBetween(key,"Path=\"","\"");
                objScripts.Add(script);
                source = source.Replace(key, "");
            }

            
            foreach (string script in objScripts)
            {

                strBody += File.ReadAllText(directory + "\\" + script.Replace("/", "\\"));
            }

            strBody = General.Utilities.Web.Compression.WebCompress(strBody, General.Utilities.Web.Compression.EnumContentType.Javascript);

            FileStream objStream = new FileStream(strTargetFile, FileMode.Create);
            StreamWriter objWriter = new StreamWriter(objStream);
            objWriter.Write(strBody);
            objWriter.Close();
            objWriter.Dispose();
            objStream.Close();
            objStream.Dispose();


            return new FileInfo(strTargetFile);

        }
        */
        #endregion

        #region GetPageKey
        public static string GetPageKey(System.Web.UI.ScriptManager objScriptManager)
        {
            string strPage = objScriptManager.Page.Request.ServerVariables["URL"];
            strPage = StringFunctions.ReplaceCharacters(strPage, new string[] { "/", "\\", "." }, "_");
            if (StringFunctions.StartsWith(strPage, "_"))
                strPage = StringFunctions.AllAfter(strPage, 0);

            return strPage;
        }
        #endregion

    }
}
