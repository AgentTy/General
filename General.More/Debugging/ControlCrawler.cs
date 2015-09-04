using System;
using General;
using System.Web.UI;

namespace General.Debugging
{
	/// <summary>
	/// Summary description for ControlCrawler.
	/// </summary>
	public class ControlCrawler
	{

		#region Constructors

		public ControlCrawler()
		{

		}

		#endregion

		public static string Crawl(Control objControl)
		{
			return Crawl(objControl,0);
		}

		private static string Crawl(Control objControl, int intLevel)
		{
			string result = new string('-',intLevel) + objControl.ClientID + "<br>";
			if(objControl.HasControls())
			{
				foreach(Control c in objControl.Controls)
					result += Crawl(c,intLevel + 1);
			}
			return result;
		}

	}
}
