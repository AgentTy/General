using System;
using General;
using System.Data;
using System.Xml;
using System.Data.SqlClient;

namespace General.DAO
{
	/// <summary>
	/// Summary description for SQLFileCache.
	/// </summary>
	public class SQLFileCache
	{

		#region Constructors
		/// <summary>
		/// Summary description for SQLFileCache.
		/// </summary>
		public SQLFileCache()
		{

		}

		#endregion

		#region GetFromFileCache
		/// <summary>
		/// Get object from SQLFileCache
		/// </summary>
		public static object GetFromFileCache(string Key,ref SQLOptions o)
		{
			o.WriteToLog("searching file cache..." + GetCacheFilePath(Key));
			General.Debug.Trace("attempting file cache retrieval..." +GetCacheFilePath(Key));
			if(System.IO.File.Exists(GetCacheFilePath(Key)))
			{
				if(!FileExpired(Key,ref o))
				{
					o.WriteToLog("found");
					DataSet ds = new DataSet();
					ds.ReadXml(GetCacheFilePath(Key));
					return ds;
				}
				else
				{
					o.WriteToLog("file is expired..." +GetCacheFilePath(Key));
					General.Debug.Trace("file is expired..." +GetCacheFilePath(Key));
					DeleteCacheFile(Key);
					return null;
				}
			}
			else
			{
				o.WriteToLog("file does not exist..." +GetCacheFilePath(Key));
				General.Debug.Trace("file does not exist..." +GetCacheFilePath(Key));
				return null;
			}
		}
		#endregion

		#region AddToFileCache
		/// <summary>
		/// Get object from SQLFileCache
		/// </summary>
		public static void AddToFileCache(string Key, object Obj,ref SQLOptions o)
		{
			General.Debug.Trace("attempting file cache save..." +GetCacheFilePath(Key));
			o.WriteToLog("attempting file cache save..." +GetCacheFilePath(Key));
			DataSet ds;
			try
			{
				ds = (DataSet) Obj;
			}
			catch
			{
				General.Debug.Trace("not a DataSet object..." +Key);
				o.WriteToLog("not a dataset object");
				//Not a DataSet object
				return;
			}
			ds.WriteXml(GetCacheFilePath(Key));
			AddFileExpiration(Key,ref o);

		}
		#endregion

		#region GetCacheFilePath
		/// <summary>
		/// Get the FilePath to an object in the SQLFileCache
		/// </summary>
		public static string GetCacheFilePath(string Key)
		{
            if (System.Web.HttpContext.Current == null)
                throw new Exception("HttpContext is not available");
			return(System.Web.HttpContext.Current.Server.MapPath("/cache/" + Key + ".xml"));
		}
		#endregion

		#region DeleteCacheFile
		/// <summary>
		/// Delete an object from the SQLFileCache
		/// </summary>
		public static void DeleteCacheFile(string Key)
		{
			string tempkey = Key.Replace("SQLHelper:","");
			try
			{
				//log.Write("deleting cache file..." +GetCacheFilePath(tempkey));
				General.Debug.Trace("deleting cache file..." +GetCacheFilePath(tempkey));
				System.IO.File.Delete(GetCacheFilePath(tempkey));
			}
			catch
			{
				//log.Write("deleting failed..." +ex.Message);
				General.Debug.Trace("deleting failed..." +GetCacheFilePath(tempkey));
			}
		}
		#endregion

		#region FileCache Expiration Management

		#region FileExpired
		/// <summary>
		/// Returns true if file is expired
		/// </summary>
		public static bool FileExpired(string Key, ref SQLOptions o)
		{
			string strXMLXPath = "/SQLHelper_FileExpirations/File";
			string strXMLForeignKeyAttribute = "Key";
			System.Xml.XmlDocument doc = GetFileExpirations(ref o);
			o.WriteToLog("searching expiration file for this value..." +Key);
			General.Debug.Trace("searching expiration file for this value..." +Key);
			System.Xml.XmlNode node = doc.SelectSingleNode(strXMLXPath + "[@" + strXMLForeignKeyAttribute + "='" + Key + "']");
			if(node == null)
			{
				o.WriteToLog("record not found... " +Key);
				General.Debug.Trace("record not found... " +Key);
				return true;
			}
			else
			{
				DateTime Expiration = Convert.ToDateTime(node.Attributes["ExpirationDate"].Value);
				if(DateTime.Now >= Expiration)
				{
					o.WriteToLog("file is expired... " +Key);
					DeleteFileExpiration(Key, ref o);
					return true;
				}
				else
					return false;
			}
		}
		#endregion

		#region AddFileExpiration
		/// <summary>
		/// Adds expiration record to the SQLFileCache
		/// </summary>
		public static void AddFileExpiration(string Key,ref SQLOptions o)
		{
			string strXMLXPath = "/SQLHelper_FileExpirations/File";
			string strXMLForeignKeyAttribute = "Key";
			System.Xml.XmlDocument doc = GetFileExpirations(ref o);
			o.WriteToLog("updating expiration file for this value..." +Key);
			General.Debug.Trace("updating expiration file for this value..." +Key);
			System.Xml.XmlNode node = doc.SelectSingleNode(strXMLXPath + "[@" + strXMLForeignKeyAttribute + "='" + Key + "']");
			if(node == null)
			{
				o.WriteToLog("record not found... creating..." +Key);
				General.Debug.Trace("record not found... creating..." +Key);
				node = doc.CreateNode(System.Xml.XmlNodeType.Element,"File","");
				node.Attributes.Append(doc.CreateAttribute("Key"));
				node.Attributes["Key"].Value = Key;
				node.Attributes.Append(doc.CreateAttribute("ExpirationDate"));
				node.Attributes["ExpirationDate"].Value = o.Expiration.ToString();
				doc.DocumentElement.AppendChild(node);
			}
			else
			{
				o.WriteToLog("record found... updating..." +Key);
				General.Debug.Trace("record found... updating..." +Key);
				node.Attributes["ExpirationDate"].Value = o.Expiration.ToString();
			}
			SaveFileExpirations(doc, ref o);
		}
		#endregion

		#region DeleteFileExpiration
		/// <summary>
		/// Removes expiration record from the SQLFileCache
		/// </summary>
		public static void DeleteFileExpiration(string Key, ref SQLOptions o)
		{
			string strXMLXPath = "/SQLHelper_FileExpirations/File";
			string strXMLForeignKeyAttribute = "Key";
			System.Xml.XmlDocument doc = GetFileExpirations(ref o);
			o.WriteToLog("deleting expiration file for this value..." +Key);
			General.Debug.Trace("deleting expiration file for this value..." +Key);
			System.Xml.XmlNode node = doc.SelectSingleNode(strXMLXPath + "[@" + strXMLForeignKeyAttribute + "='" + Key + "']");
			if(node == null)
			{
				//DO NOTHING
			}
			else
			{
				o.WriteToLog("record found... deleting..." +Key);
				General.Debug.Trace("record found... deleting..." +Key);
				doc.DocumentElement.RemoveChild(node);
			}
			SaveFileExpirations(doc,ref o);
		}
		#endregion

		#region GetFileExpirations
		/// <summary>
		/// Gets all expiration records
		/// </summary>
		private static System.Xml.XmlDocument GetFileExpirations(ref SQLOptions o)
		{
			System.Xml.XmlDocument doc;
			object obj;
			o.WriteToLog("attempt loading cache_expirations from memory");
			obj = SQLMemoryCache.GetFromMemoryCache("cache_expirations",ref o);
			if(obj == null)
			{
				o.WriteToLog("attempt loading cache_expirations from file");
				string FileName = GetCacheFilePath("cache_expirations");
							
				if(System.IO.File.Exists(FileName))
				{
					doc = new System.Xml.XmlDocument();
					doc.Load(FileName);
					o.WriteToLog("saving cache_expirations to memory");
					SQLMemoryCache.AddToMemoryCache("cache_expirations", doc,ref o,DateTime.Now.AddDays(30),false);
					return doc;
				}
				else
				{
					o.WriteToLog("creating file and saving cache_expirations to memory");
					doc = CreateFileExpirationsFile(null,ref o);
					SQLMemoryCache.AddToMemoryCache("cache_expirations", doc,ref o, DateTime.Now.AddDays(30),false);
					return doc;
				}
			}
			else
			{
				return (System.Xml.XmlDocument) obj;
			}
		}
		#endregion

		#region SaveFileExpirations
		/// <summary>
		/// Saves expiration records
		/// </summary>
		public static void SaveFileExpirations(System.Xml.XmlDocument doc, ref SQLOptions o)
		{
			SQLMemoryCache.AddToMemoryCache("cache_expirations", doc, ref o, DateTime.Now.AddDays(30),false);
			CreateFileExpirationsFile(doc, ref o);
		}
		#endregion

		#region CreateFileExpirationsFile
		private static System.Xml.XmlDocument CreateFileExpirationsFile(System.Xml.XmlDocument doc, ref SQLOptions o)
		{
			string FileName = GetCacheFilePath("cache_expirations");
			if(doc == null)
			{
				doc = new System.Xml.XmlDocument();
				System.Xml.XmlDeclaration dec = doc.CreateXmlDeclaration("1.0","ASCII","yes");
				doc.AppendChild(dec);
				System.Xml.XmlElement ele = doc.CreateElement("SQLHelper_FileExpirations");
				doc.AppendChild(ele);
			}
			//General.IO.IOTools.QueueWriteFile(doc,FileName, "cache_expirations", new General.IO.IOTools.QueueWriteFileCompletedCallback(CreateFileExpirationsFileCallback));
			try
			{
				o.WriteToLog("saving cache_expirations.xml");
				doc.Save(FileName);
			}
			catch(System.UnauthorizedAccessException)
			{
				o.WriteToLog("failure to save cache_expirations.xml");
				//DO NOTHING
			}
			return doc;
		}
		#endregion

		#region CreateFileExpirationsFileCallback
		private static void CreateFileExpirationsFileCallback(object Data, string Key, string FileName, bool Success, int TryCount)
		{
			//XmlDocument doc = (XmlDocument) Data;
			//General.Debugging.Report.SendDebug("CALLBACK",doc.ToString() + "<br>\n" + FileName + "<br>\n" + Success + "<br>\n" + TryCount.ToString());
		}
		#endregion

		#endregion

	}
}
