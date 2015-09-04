using System;
using General;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;


namespace General.Utilities.Text
{
	/// <summary>
	/// Contains static methods that convert string of text into an image file.
	/// </summary>
	public class TextToImage
	{

		#region Constructors

		/// <summary>
		/// Contains static methods that convert string of text into an image file.
		/// </summary>
		public TextToImage()
		{

		}

		#endregion

		#region Static Methods
		/// <summary>
		/// Outputs a string of text to a stream as a Jpeg image
		/// </summary>
		public static void Save(Stream OutputStream, string Text, Font TextFont, Color TextColor, Color BackgroundColor)
		{
			Save(OutputStream,Text,TextFont,TextColor,BackgroundColor,TextRenderingHint.SystemDefault);
		}

		/// <summary>
		/// Outputs a string of text to a stream as a Jpeg image
		/// </summary>
		public static void Save(Stream OutputStream, string Text, Font TextFont, Color TextColor, Color BackgroundColor, TextRenderingHint RenderingHint)
		{
			Bitmap bmp = GetBitmap(Text,TextFont,TextColor,BackgroundColor,RenderingHint);
			bmp.Save(OutputStream, ImageFormat.Jpeg);
			bmp.Dispose();
		}

		/// <summary>
		/// Saves a string of text to a file as an image, the format is determined by the file extension.
		/// Supported extensions are *.bmp,*.jpg,*.jpeg,*.gif,*.png,*.tiff
		/// </summary>
		public static void Save(string FileName, string Text, Font TextFont, Color TextColor, Color BackgroundColor)
		{
			Save(FileName,Text,TextFont,TextColor,BackgroundColor,TextRenderingHint.SystemDefault);
		}

		/// <summary>
		/// Saves a string of text to a file as an image, the format is determined by the file extension.
		/// Supported extensions are *.jpg,*.jpeg,*.gif,*.bmp,*.png,*.tiff
		/// </summary>
		public static void Save(string FileName, string Text, Font TextFont, Color TextColor, Color BackgroundColor, TextRenderingHint RenderingHint)
		{
			ImageFormat objFormat;
			switch(StringFunctions.AllAfter(FileName,".").ToLower())
			{
				case "jpg":
				case "jpeg":
					objFormat = ImageFormat.Jpeg;
					break;
				case "gif":
					objFormat = ImageFormat.Gif;
					break;
				case "bmp":
					objFormat = ImageFormat.Bmp;
					break;
				case "png":
					objFormat = ImageFormat.Png;
					break;
				case "tiff":
					objFormat = ImageFormat.Tiff;
					break;
				default:
					objFormat = ImageFormat.Jpeg;
					break;
			}
			Bitmap bmp = GetBitmap(Text,TextFont,TextColor,BackgroundColor,RenderingHint);
			FileStream fs = new FileStream(FileName,FileMode.Create);
			bmp.Save(fs,objFormat);
			bmp.Dispose();
			fs.Close();
		}

		/// <summary>
		/// Returns a Bitmap object representing the supplied text
		/// </summary>
		private static Bitmap GetBitmap(string Text, Font TextFont, Color TextColor, Color BackgroundColor, TextRenderingHint RenderingHint)
		{
			// we initialise these here because it's needed to calculate the width
			// and height of the provided text.  the 1,1 is only a temporary 
			// measurement
			Bitmap bmp = new Bitmap(1, 1);
			Graphics graphic = System.Drawing.Graphics.FromImage(bmp);

			// measure the text
			StringFormat stringformat = new StringFormat(StringFormat.GenericTypographic);
			int height = Convert.ToInt32(graphic.MeasureString(Text, TextFont, new PointF(0,0), stringformat).Height) + 6;
			int width = Convert.ToInt32(graphic.MeasureString(Text, TextFont, new PointF(0,0), stringformat).Width);

			// add some padding for the width - i think the kerning is throwing
			// it off by a little.  5% seems to work well, and the +10 is necessary
			// if the string is only a few letters
			width += Convert.ToInt32(0.1 * width) + 10;

			// recreate our bmp and graphic objects with the new measurements
			bmp = new Bitmap(width, height);
			//bmp.SetResolution(600,600);

			graphic = System.Drawing.Graphics.FromImage(bmp);

			// our background colour
			graphic.Clear(BackgroundColor);

			// aliasing mode
			graphic.TextRenderingHint = RenderingHint;

			/* 
			the different aliasing modes:
			---------------------------------------------------------------------------------
			graphic.TextRenderingHint = TextRenderingHint.SystemDefault;
			- each character is drawn using its glyph bitmap, with the system default rendering hint

			graphic.TextRenderingHint = TextRenderingHint.AntiAlias;
			- each character is drawn using its antialiased glyph bitmap without hinting

			graphic.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			- each character is drawn using its antialiased glyph bitmap with hinting

			graphic.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			- each character is drawn using its glyph CT bitmap with hinting

			graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
			- each character is drawn using its glyph bitmap

			graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
			- each character is drawn using its glyph bitmap
			*/

			// our brush which is the writing colour
			SolidBrush brush = new SolidBrush(TextColor);

			/*
			alternate brushes
			-------------------------------------------------------------------------------
			System.Drawing.Drawing2D.HatchBrush
			System.Drawing.Drawing2D.LinearGradientBrush
			System.Drawing.Drawing2D.PathGradientBrush
			System.Drawing.SolidBrush
			System.Drawing.TextureBrush
			*/

			// create our graphic
			graphic.DrawString(Text, TextFont, brush, new Rectangle(0, 3, width, height));

			// dispose of our objects
			TextFont.Dispose();
			stringformat.Dispose();
			graphic.Dispose();
			return bmp;
		}
		#endregion

	}
}
