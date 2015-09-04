using System;
using General;
using System.IO;
using System.Drawing;
using System.Drawing.Text;

namespace General.Utilities.Barcode
{
	/// <summary>
	/// Contains static methods for generating barcode image files.
	/// Requires font files to be installed on server.
	/// FREE3OF9.TTF 
	/// FREE3OF9X.TTF
	/// </summary>
	public class BarcodeGenerator
	{
		private Color _objBackgroundColor;
		private Color _objFontColor;
		private int _intFontSize;

		#region Constructors
		/// <summary>
		/// Contains static methods for generating barcode image files.
		/// </summary>
		public BarcodeGenerator()
		{
			_objBackgroundColor = Color.White;
			_objFontColor = Color.Black;
			_intFontSize = 32;
		}

		#endregion

		#region Public Properties
		
		/// <summary>
		/// Modify the barcode background color
		/// </summary>
		public Color BackgroundColor
		{
			get	{return _objBackgroundColor;}
			set { _objBackgroundColor = value; }
		}

		/// <summary>
		/// Modify the barcode font color
		/// </summary>
		public Color FontColor
		{
			get	{return _objFontColor;}
			set { _objFontColor = value; }
		}

		/// <summary>
		/// Modify the barcode font size
		/// </summary>
		public int FontSize
		{
			get	{return _intFontSize;}
			set { _intFontSize = value; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Creates a barcode of the specified type in the specified file.
		/// </summary>
		public void Generate(string Data, string Type, string FileName)
		{
			Generate(Data,GetType(Type),FileName);
		}

		/// <summary>
		/// Outputs a barcode of the specified type to a Stream object.
		/// </summary>
		public void Generate(string Data, string Type, Stream OutputStream)
		{
			Generate(Data,GetType(Type),OutputStream);
		}

		/// <summary>
		/// Creates a barcode of the specified type in the specified file.
		/// </summary>
		public void Generate(string Data, BarcodeType Format, string FileName)
		{
			switch(Format)
			{
				case BarcodeType.Code39:
					GenerateCode39(Data,FileName);
					break;
				case BarcodeType.Code39Extended:
					GenerateCode39Extended(Data,FileName);
					break;
			}
		}

		/// <summary>
		/// Outputs a barcode of the specified type to a Stream object.
		/// </summary>
		public void Generate(string Data, BarcodeType Format, Stream OutputStream)
		{
			switch(Format)
			{
				case BarcodeType.Code39:
					GenerateCode39(Data,OutputStream);
					break;
				case BarcodeType.Code39Extended:
					GenerateCode39Extended(Data,OutputStream);
					break;
			}
		}

		#endregion

		#region Private Functions
		private BarcodeType GetType(string Type)
		{
			if(Type == null || Type == "")
				return BarcodeType.Code39Extended; //DEFAULT VALUE

			BarcodeType objType;
			switch(Type.ToLower())
			{
				case "code39":
					objType = BarcodeType.Code39;
					break;
				case "code39extended":
					objType = BarcodeType.Code39Extended;
					break;
				default:
					objType = BarcodeType.Code39Extended; //DEFAULT VALUE
					break;
			}
			return objType;
		}

		private void GenerateCode39(string Data, string FileName)
		{
			Font font = new Font("Free 3 of 9", _intFontSize, FontStyle.Regular);
			
			Utilities.Text.TextToImage.Save(FileName,"*" + Data + "*",font,_objFontColor,_objBackgroundColor,TextRenderingHint.SingleBitPerPixel);
		}

		private void GenerateCode39(string Data, Stream OutputStream)
		{
			Font font = new Font("Free 3 of 9", _intFontSize, FontStyle.Regular);
			Utilities.Text.TextToImage.Save(OutputStream,"*" + Data + "*",font,_objFontColor,_objBackgroundColor,TextRenderingHint.SingleBitPerPixel);
		}

		private void GenerateCode39Extended(string Data, string FileName)
		{
			Font font = new Font("Free 3 of 9 Extended", _intFontSize, FontStyle.Regular);
			Utilities.Text.TextToImage.Save(FileName,"*" + Data + "*",font,_objFontColor,_objBackgroundColor,TextRenderingHint.SingleBitPerPixel);
		}

		private void GenerateCode39Extended(string Data, Stream OutputStream)
		{
			Font font = new Font("Free 3 of 9 Extended", _intFontSize, FontStyle.Regular);
			Utilities.Text.TextToImage.Save(OutputStream,"*" + Data + "*",font,_objFontColor,_objBackgroundColor,TextRenderingHint.SingleBitPerPixel);
		}
		#endregion

	}

	#region Enumerators
	/// <summary>
	/// Enumerates possible barcode formats
	/// </summary>
	public enum BarcodeType
	{
		/// <summary>
		/// Code 39
		/// </summary>
		Code39 = 1,
		/// <summary>
		/// Code 39 Extended
		/// Supports alphanumeric data
		/// </summary>
		Code39Extended = 2
	}
	#endregion
}
