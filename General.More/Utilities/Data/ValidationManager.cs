
using System;

namespace General.Utilities.Data
{
	//################################################################################################
	//
	// ValidationManager
	//
	/// <summary>
	/// Central location for validation routines that are generic enough to be used across multiple
	/// pages and methods
	/// </summary>
	//
	//################################################################################################
	public class ValidationManager
	{
		#region Constructors 
		//*******************************************************
		//
		// ValidationManager()
		//
		/// <summary>
		/// Default Constructor 
		/// </summary>
		//
		//*******************************************************
		public ValidationManager()
		{
		}
		#endregion

		#region Methods 
		//*****************************************************************************
		// 
		// ValidateDecimalFormat()
		//
		/// <summary>
		/// Validate passed string is a valid Decimal format
		/// </summary>
		//
		//*****************************************************************************
		public static bool ValidateDecimalFormat(string strValue)
		{
			bool blnValid = false;

			try
			{
				Convert.ToDecimal(strValue.ToString());
				blnValid = true;
			}
			catch
			{
				return false;
			}

			return blnValid;
		}

		//*****************************************************************************
		// 
		// ValidateNumericFormat()
		//
		/// <summary>
		/// Validate passed string is a valid Numeric format
		/// </summary>
		//
		//*****************************************************************************
		public static bool ValidateNumericFormat(string strValue)
		{
			bool blnValid = false;

			try
			{
				Convert.ToInt32(strValue.ToString());
				blnValid = true;
			}
			catch
			{
				return false;
			}

			return blnValid;
		}
		#endregion
	}
}
