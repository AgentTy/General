using System;
using System.Text;
using General.Utilities.Serialization;

namespace General.Debugging
{
	/// <summary>
	/// Contains static methods that report the contents of the Web Cache
	/// </summary>
	public sealed class CacheReporting
	{
		
		#region GetCacheReport
		/// <summary>
		/// Returns an HTML Table listing all the objects in the Cache.
		/// </summary>
		public static string GetCacheReport()
		{
			return GetCacheReport(false);
		}

		/// <summary>
		/// Returns an HTML Table listing all the objects in the Cache, and when requested their bytesize in memory.
		/// </summary>
		public static string GetCacheReport(bool boolGetBytes)
		{
			if(System.Web.HttpContext.Current == null)
				throw new Exception("Cache not available outside of web contest");

			StringBuilder sb = new StringBuilder();
			int intCount = 1;
			long intTotalBytes = 0;
			sb.Append("<table border=1 cellspacing=2 cellpadding=1>");
			sb.Append("<tr><td  colspan=3>Cache Report</td></tr>");
			sb.Append("<tr>");
			sb.Append("<td>");
			sb.Append("Count");
			sb.Append("</td>");
			sb.Append("<td>");
			sb.Append("Key");
			sb.Append("</td>");
			if(boolGetBytes)
			{
				sb.Append("<td>");
				sb.Append("ByteSize");
				sb.Append("</td>");
			}
			sb.Append("</tr>");
			System.Web.Caching.Cache GlobalCache = System.Web.HttpContext.Current.Cache;
			foreach(System.Collections.DictionaryEntry o in GlobalCache)
			{
				long intByteSize = 0;
				if(boolGetBytes)
				{
					intByteSize = SerializationTools.GetObjectByteSize(o.Value);
					intTotalBytes += intByteSize;
				}
				sb.Append("<tr>");
				sb.Append("<td>");
				sb.Append(intCount);
				sb.Append("</td>");
				sb.Append("<td>");
				sb.Append(o.Key.ToString());
				sb.Append("</td>");
				if(boolGetBytes)
				{
					sb.Append("<td>");
					sb.Append(SerializationTools.FormatSize(intByteSize));
					sb.Append("</td>");
				}
				sb.Append("</tr>");
				intCount++;
			}
			if(boolGetBytes)
			{
				sb.Append("<tr>");
				sb.Append("<td colspan=3 align=right>");
				sb.Append("Total Bytes: ");
				sb.Append(SerializationTools.FormatSize(intTotalBytes));
				sb.Append("</td>");
				sb.Append("</tr>");
			}
			sb.Append("</table>");

			return(sb.ToString());
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Contains static methods that report the contents of the Web Cache
		/// </summary>
		public CacheReporting(){}
		#endregion

	}
}
