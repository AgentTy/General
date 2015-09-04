
using System;
using System.Globalization;

namespace General.Utilities.Data {
	//################################################################################################
	//
	// DataConversions
	//
	/// <summary>
	/// Helper class that contains methods and functions to convert data from one format to another
	/// </summary>
	//
	//################################################################################################
	public class DataConversions {
		private DataConversions() {} // All methods are static

		//*******************************************************
		//
		// ConvertCurrencyToNumeric() 
		//
		/// <summary>
		/// Converts the currency value to a numeric value. Acceptable values are $1,000.00, 123.45, $50
		/// Function converts $1,234.56 to 1234.56.
		/// </summary>
		/// <param name="strCurrencyValue">The currency value</param>
		/// <returns>A double representing the dollar amount and any cents</returns>
		//
		//*******************************************************
		public static double ConvertCurrencyToNumeric(string strCurrencyValue)
		{
			//Strips out any special characters but leaves the decimal point intact.
			Double dblValue = Double.Parse(strCurrencyValue, NumberStyles.Any);

			return dblValue;
		}

		//********************************************************** 
		//
		// ConvertEmptyStringToZero()
		//
		/// <summary>
		/// Local method to convert incoming string values to string zero (0)
		/// </summary>
		/// <param name="strValue">The string value to evaluate</param>
		/// <returns>Either "0", or the string passed in</returns>
		//
		//********************************************************** 
		public static string ConvertEmptyStringToZero(string strValue)
		{
			if (strValue.Length > 0)
			{
				return strValue;
			}
			else
			{
				return "0";
			}
		}

		//********************************************************** 
		//
		// ConvertNullStringToEmptyString()
		//
		/// <summary>
		/// Converts a Null string value to a useable empty string.
		/// </summary>
		/// <remarks>This is needed in several situations when assigning fields the value of a db field</remarks>
		/// <param name="strField">The DB field to check</param>
		/// <returns>Non-null empty string or original value passed</returns>
		//
		//********************************************************** 
		public static string ConvertNullStringToEmptyString(string strField)
		{
			//Is this field being passed in NULL?
			if (strField == null)
			{
				//Yes, return an empty string
				return "";
			}
			else
			{
				//No, return the object that was passed in
				return strField;
			}
		}

		//********************************************************** 
		//
		// ConvertEmptyStringToDBNull()
		//
		/// <summary>
		/// Converts a Null string value to a DBNull value.
		/// </summary>
		/// <remarks>This is needed in several situations when assigning fields the value of a db field</remarks>
		/// <param name="strField">The DB field to check</param>
		/// <returns>DBNull or original values passed</returns>
		//
		//********************************************************** 
		public static Object ConvertEmptyStringToDBNull(string strField)
		{
			//Is this field being passed in NULL?
			if (strField == "")
			{
				//Yes, return an empty string
				return DBNull.Value;
			}
			else
			{
				//No, return the object that was passed in
				return strField;
			}
		}

		//********************************************************** 
		//
		// ConvertDBNullToEmptyString()
		//
		/// <summary>
		/// Converts a DB Null value to a useable empty string.
		/// </summary>
		/// <remarks>This is needed in several situations when assigning fields the value of a db field</remarks>
		/// <param name="objField">The DB field to check</param>
		/// <returns>Empty string or original values passed</returns>
		//
		//********************************************************** 
		public static Object ConvertDBNullToEmptyString(Object objField)
		{
			//Is this field being passed in NULL?
			if (objField == DBNull.Value)
			{
				//Yes, return an empty string
				return "";
			}
			else
			{
				//No, return the object that was passed in
				return objField;
			}
		}

		//********************************************************** 
		//
		// ConvertDBNullToZero()
		//
		/// <summary>
		/// Converts a DB Null value to a useable 0.
		/// </summary>
		/// <remarks>This is needed in several situations when assigning fields the value of a db field</remarks>
		/// <param name="objField">The DB field to check</param>
		/// <returns>Zero (0) or original values passed</returns>
		//
		//********************************************************** 
		public static Object ConvertDBNullToZero(Object objField)
		{
			//Is this field being passed in NULL?
			if (objField == DBNull.Value)
			{
				//Yes, return a zero
				return 0;
			}
			else
			{
				//No, return the object that was passed in
				return objField;
			}
		}

		//********************************************************** 
		//
		// ConvertDBNullToZero()
		//
		/// <summary>
		/// Converts a DB Null value to a useable 0.
		/// </summary>
		/// <remarks>This is needed in several situations when assigning fields the value of a db field</remarks>
		/// <param name="objField">The DB field to check</param>
		/// <param name="intDefaultValue">The numeric value to default to</param>
		/// <returns>Zero (0) or original values passed</returns>
		//
		//********************************************************** 
		public static Object ConvertDBNullToNumeric(Object objField, int intDefaultValue)
		{
			//Is this field being passed in NULL?
			if (objField == DBNull.Value)
			{
				//Yes, return the defaulted value
				return intDefaultValue;
			}
			else
			{
				//No, return the object that was passed in
				return objField;
			}
		}
	  
	  /// <summary>
	  /// Converts a "y" or an "n" to a boolean.
	  /// </summary>
	  /// <param name="str">string - Y = true and N = false</param>
	  /// <param name="blnDefault">bool - Default value if Y or N is not found</param>
	  /// <returns>bool</returns>
	  public static bool ConvertYNToBoolean(string str, bool blnDefault) {
	    switch (str.ToUpper()) {
	      case "Y": return true;
	      case "N": return false;
	      default: return blnDefault;
	    }
	  }
	  
	  #region ConvertYNToBoolean Overloads
	  /// <summary>
	  /// Converts a "y" or an "n" to a boolean. Sends a default of false.
	  /// </summary>
	  /// <param name="str">string - Y = true and N = false</param>
	  /// <returns>bool</returns>
	  public static bool ConvertYNToBoolean(string str) {
	    return ConvertYNToBoolean(str, false);
	  }

	  public static bool ConvertYNToBoolean(char chr, bool blnDefault) {
	    return ConvertYNToBoolean(chr.ToString(), blnDefault);
	  }
	  
	  /// <summary>
	  /// Converts a 'y' or an 'n' to a boolean. Allows the
	  /// ConvertYNToBoolean(string) overload to decide on the default.
	  /// </summary>
	  /// <param name="chr">char - 'Y' = true and 'N' = false</param>
	  /// <returns>bool</returns>
	  public static bool ConvertYNToBoolean(char chr) {
	    return ConvertYNToBoolean(chr.ToString());
	  }
		#endregion
	}
}
