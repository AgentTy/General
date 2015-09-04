
using System;
using System.Data;

namespace General.Utilities.Data
{
	//################################################################################################
	//
	// DataFormatting
	//
	/// <summary>
	/// Helper class that contains methods and functions to format and clean data
	/// </summary>
	//
	//################################################################################################
	public class DataFormatting
	{
		#region Constructors 
		//*******************************************************
		//
		// DataFormatting()
		//
		/// <summary>
		/// Default Constructor
		/// </summary>
		//
		//*******************************************************
		public DataFormatting()
		{
		}
		#endregion

		#region Methods 
		//*******************************************************
		//
		// AdjustCaseToCapsLower() 
		//
		/// <summary>
		/// Adjusts the case of the string passed in.
		/// </summary>
		/// <remarks>The string passed in will take on the following format: Xxxx y Zzzzz</remarks>
		/// <example>
		/// string strField = "I like to TEST THingS for FUN!!!"
		/// strField = AdjustCase(strField)
		/// returned:   "I Like To Test Things For Fun!!!"
		/// </example>
		//
		//*******************************************************
		public static string AdjustCaseToCapsLower(string strField)
		{
			//Set defaults
			string strCleanedAndFormattedField = "";
			string strCurrentFieldAdjusted = "";

			//Adjust the field to lower case
			string strLowerCased = strField.ToLower().Trim();

			//Break the string into an array and manipulate each line (or word)
			string[] strSplitFields = strLowerCased.Split(' ');

			//Iterate through each line in the string and make the 1st character uppercase
			foreach (string strCurrentField in strSplitFields)
			{
				//Does the field have more than 1 character?
				if (strCurrentField.Length > 1)
				{
					//Yes, set the 1st character to an Uppercase
					strCurrentFieldAdjusted = char.ToUpper(strCurrentField[0]) + strCurrentField.Substring(1, strCurrentField.Length - 1);

					//Put the string back together again
					strCleanedAndFormattedField = strCleanedAndFormattedField + strCurrentFieldAdjusted + " ";
				}
				else
				{
					//Put the string back together again
					strCleanedAndFormattedField = strCleanedAndFormattedField + strCurrentField + " ";
				}
			}			

			//Return the cleaned-up field
			return strCleanedAndFormattedField.Trim();
		}

		//*******************************************************
		//
		// FixSingleQuotes()
		//
		/// <summary>
		/// Adds an escape character to any occurrences of the single quote character
		/// in the passed string. This is used to avoid breaking concatenated commands 
		/// in SQL used by search pages. 
		/// </summary>
		/// <param name="sInputValue">The input string to be modified</param>
		/// <returns>The input string formatted with escape characters as needed</returns>
		//
		//*******************************************************
		public static string FixSingleQuotes(string sInputValue)
		{
			//Search for one single quote (') and add escape character ('')
			return sInputValue.Replace("'", "''");
		}

		//*******************************************************
		//
		// FixDoubleQuotes()
		//
		/// <summary>
		/// Removes the escape character to any occurrences of the double-single quote character ('')
		/// in the returned string. This is used to avoid displaying two single quotes when only one
		/// was originally entered.
		/// </summary>
		/// <param name="sInputValue">The input string to be modified</param>
		/// <returns>The input string formatted with escape characters as needed</returns>
		//
		//*******************************************************
		public static string FixDoubleQuotes(string sInputValue)
		{
			//Search for two single quotes ('') and remove one of them (')
			return sInputValue.Replace("''", "'");
		}

		//*******************************************************
		//
		// FixDoubleQuotesInDataSet()
		//
		/// <summary>
		/// Replaces '' with ' in every table, row, and cell of the DataSet passed to the function.
		/// </summary>
		/// <param name="DS"></param>
		/// <returns></returns>
		//
		//*******************************************************
		public static DataSet FixDoubleQuotesInDataSet(DataSet DS)
		{
			for (int i = 0; i < DS.Tables.Count; i++)
			{
				for (int j = 0; j < DS.Tables[i].Rows.Count; j++)
				{
					for (int k = 0; k < DS.Tables[i].Columns.Count; k++)
					{
						if (DS.Tables[i].Columns[k].DataType == System.Type.GetType("System.String"))
						{
							DS.Tables[i].Rows[j][k] = FixDoubleQuotes(DS.Tables[i].Rows[j][k].ToString());
						}
					}
				}
			}

			return DS;
		}

		//*******************************************************
		//
		// FixDoubleQuotesInDataSet()
		//
		/// <summary>
		/// Replaces '' with ' in every table, row, and cell of the DataSet passed to the function.
		/// </summary>
		/// <param name="DT"></param>
		/// <returns></returns>
		//
		//*******************************************************
		public static DataTable FixDoubleQuotesInDataTable(DataTable DT)
		{
			for (int j = 0; j < DT.Rows.Count; j++)
			{
				for (int k = 0; k < DT.Columns.Count; k++)
				{
					if (DT.Columns[k].DataType == System.Type.GetType("System.String"))
					{
						DT.Rows[j][k] = FixDoubleQuotes(DT.Rows[j][k].ToString());
					}
				}
			}

			return DT;
		}
		#endregion
	}
}
