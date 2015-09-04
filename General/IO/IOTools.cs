using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Threading;

namespace General.IO
{
	/// <summary>
	/// IO Tools
	/// </summary>
	public class IOTools
	{
		private static XmlDocument _xmldoc;
		private static string _strFilePath;
		private static string _strCallbackKey;
		private static int _intTryCount;
		private static QueueWriteFileCompletedCallback _objCallback;

		/// <summary>
		/// IO Tools
		/// </summary>
		public IOTools()
		{}

        #region GetFileBytes
        public static byte[] GetFileBytes(string strFilePath)
        {
            Uri objUri = new Uri(strFilePath);
            return GetFileBytes(objUri);
        }


        public static byte[] GetFileBytes(Uri uriFilePath)
        {
            if (uriFilePath.HostNameType == UriHostNameType.Basic)
            { //Local Files
                if (File.Exists(uriFilePath.LocalPath))
                {
                    StreamReader sr = new StreamReader(uriFilePath.LocalPath);
                    byte[] bf = Encoding.ASCII.GetBytes(sr.ReadToEnd());
                    sr.Close();
                    return bf;
                }
            }
            else
            { //Remote Files over IP

                System.Net.HttpWebRequest objWebRequest;
                objWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uriFilePath.OriginalString);
                //objWebRequest.Timeout = 10;
                objWebRequest.Method = "GET";
                objWebRequest.ContentType = "text/html";

                System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objWebRequest.GetResponse();
                if (objResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    StreamReader sr = new StreamReader(objResponse.GetResponseStream());
                    byte[] bf = Encoding.ASCII.GetBytes(sr.ReadToEnd());
                    sr.Close();
                    return bf;
                }
            }
            return null;
        }
        #endregion

		#region GetTextFile
		/// <summary>
		/// Read a text file and return it as a string
		/// </summary>
		public static string GetTextFile(string FilePath)
		{
			string result;
			StreamReader r = File.OpenText(FilePath);
			result = r.ReadToEnd();
			r.Close();
			return(result);
		}
		#endregion

		#region WriteFile
		/// <summary>
		/// Save the content of a XmlDocument to a file
		/// </summary>
		public static void WriteFile(XmlDocument Doc, string FilePath)
		{
			Doc.Save(FilePath);
		}

		/// <summary>
		/// Save the content of a StringBuilder to a file
		/// </summary>
		public static void WriteFile(StringBuilder Body, string FilePath)
		{
			WriteFile(Body.ToString(),FilePath);
		}

		/// <summary>
		/// Save the content of string to a file
		/// </summary>
		public static void WriteFile(string Body, string FilePath)
		{
			StreamWriter w = File.CreateText(FilePath);
			w.Write(Body);
			w.Close();
		}

		/// <summary>
		/// Creates/Overwrites a file from a byte array
		/// </summary>
		public static void WriteFile(ref byte[] Buffer, string FilePath)
		{
			// Create a file
			FileStream newFile = new FileStream(FilePath, FileMode.Create);
			// Write data to the file
			newFile.Write(Buffer, 0, Buffer.Length);
			// Close file
			newFile.Close();
		}

		/// <summary>
		/// Creates/Overwrites a file from a System.Web.HttpPostedFile object
		/// </summary>
		public static void WriteFile(ref System.Web.HttpPostedFile File, string FilePath)
		{
			byte[] bytes = new byte[File.ContentLength];
			File.InputStream.Read(bytes,0,File.ContentLength);
			WriteFile(ref bytes,FilePath);
		}
		#endregion

		#region AppendFile
		/// <summary>
		/// Creates/Overwrites a file from a byte array
		/// </summary>
		public static void AppendFile(ref byte[] Buffer, string FilePath)
		{
			// Open a file
			FileStream newFile = new FileStream(FilePath, FileMode.Append);
			// Write data to the file
			newFile.Write(Buffer, 0, Buffer.Length);
			// Close file
			newFile.Close();
		}

        /// <summary>
        /// Save the content of string to a file
        /// </summary>
        public static void AppendFile(string Body, string FilePath)
        {
            StreamWriter w = File.AppendText(FilePath);
            w.Write(Body);
            w.Close();
        }

        /// <summary>
        /// Save the content of string to a file
        /// </summary>
        public static void AppendFileLine(string Line, string FilePath)
        {
            StreamWriter w = File.AppendText(FilePath);
            w.WriteLine(Line);
            w.Close();
        }
		#endregion

        #region ReSaveFile
        /// <summary>
        /// Opens a file and saves it without making any changes
        /// </summary>
        public static void ReSaveFile(string FilePath)
        {
            System.IO.File.SetLastWriteTime(FilePath, DateTime.Now);
        }
        #endregion

		#region QueueWriteFile
		/// <summary>
		/// QueueWriteFileCompletedCallback
		/// </summary>
		public delegate void QueueWriteFileCompletedCallback(object Data, string Key, string FileName, bool Success, int TryCount);

		/// <summary>
		/// Save the content of string to a file
		/// </summary>
		public static void QueueWriteFile(XmlDocument Doc, string FilePath, string CallbackKey, QueueWriteFileCompletedCallback Callback)
		{
			try
			{
				WriteFile(Doc,FilePath);
				Callback(Doc,CallbackKey,FilePath,true,1);
			}
			catch(System.UnauthorizedAccessException)
			{
				_xmldoc = Doc;
				_strFilePath = FilePath;
				_strCallbackKey = CallbackKey;
				_intTryCount = 1;
				_objCallback = Callback;
				ThreadPool.QueueUserWorkItem(new WaitCallback(QueueWriteFileCallback));
			}
		}

		/// <summary>
		/// Save the content of string to a file
		/// </summary>
		private static void QueueWriteFileCallback(Object stateInfo)
		{
			Thread.Sleep(new TimeSpan(0,0,1)); //Wait for 1 second
			try
			{
				WriteFile(_xmldoc,_strFilePath);
				//General.Debugging.Report.SendDebug("QueueWriteFile Succeeded","File \"" + _strFilePath + "\" after " + _intTryCount.ToString() + " tries");
				_objCallback(_xmldoc,_strCallbackKey,_strFilePath,true,_intTryCount);
				_xmldoc = null;
				_strFilePath = null;
				_strCallbackKey = null;
				_intTryCount = 0;
			}
			catch(System.UnauthorizedAccessException)
			{
				_intTryCount++;
				if(_intTryCount <= 10)
					ThreadPool.QueueUserWorkItem(new WaitCallback(QueueWriteFileCallback));
				else
				{
					General.Debugging.Report.SendError("QueueWriteFile Failed","Could not write the file \"" + _strFilePath + "\" after " + _intTryCount.ToString() + " tries");
					_objCallback(_xmldoc,_strCallbackKey,_strFilePath,false,_intTryCount);
				}
			}
		}
		#endregion
	}
}
