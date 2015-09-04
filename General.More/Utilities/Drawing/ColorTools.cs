using System;
using General;
using System.Drawing;

namespace General.Utilities.Drawing
{
	/// <summary>
	/// ColorTools
	/// </summary>
	public class ColorTools
	{

		#region Constructors

		/// <summary>
		/// ColorTools
		/// </summary>
		public ColorTools()
		{

		}

		#endregion

		#region Static Methods
		
		#region GetColorFromHtmlColor
		/// <summary>
		/// Returns Color object from an html color value (#FFFFFF)
		/// </summary>
		public static Color GetColorFromHtmlColor(string HtmlColor)
		{
			string temp = HtmlColor;
			temp = temp.Replace("#","");

			string sr = temp.Substring(0,2);
			string sg = temp.Substring(2,2);
			string sb = temp.Substring(4,2);

			int r = int.Parse(sr,System.Globalization.NumberStyles.HexNumber);
			int g = int.Parse(sg,System.Globalization.NumberStyles.HexNumber);
			int b = int.Parse(sb,System.Globalization.NumberStyles.HexNumber);

			return Color.FromArgb(r,g,b);
		}
		#endregion

		#endregion

	}

}
