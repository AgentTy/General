using System;
using System.Data;

namespace General.Utilities.Data {
	/// <summary>
	/// This is a collection of utilities that are useful for Selecting
	/// records within a DataSet.
	/// </summary>
	public class DataSetUtilities	{
		/// <summary>
		/// This is a collection of utilities that are useful for Selecting
		/// records within a DataSet.
		/// </summary>
		private DataSetUtilities() { }
		
	    #region Select Distinct
		//################################################################################################
		//
		// DataSetUtilities.SelectDistinct
		// Author: Joel Flint
		//
		/// <summary>
		/// Provides the ability to select all distinct values in a dataset. This closely mimics the T-SQL
		/// command DISTINCT as in:
		///    select DISTINCT id from tbl
		/// </summary>
		//
		//################################################################################################
		public static DataTable SelectDistinct(DataTable dtblSubject, int intDistinctColumn, int intSortColumn, string strOrder, string strTableName) {
			if (intDistinctColumn < dtblSubject.Columns.Count) {
				// Duplicate the table structure.
				DataTable dtblDistinct = dtblSubject.Clone();
				dtblDistinct.TableName = strTableName;
		        
				// Sort the original table data.
				DataRow[] aDrSorted = dtblSubject.Select("", dtblSubject.Columns[intSortColumn].ColumnName + " " + strOrder);
		        
				// Add only the distinct data for the column specified.
				string strPrevious = string.Empty, strCurrent;
				int intRowCount = aDrSorted.Length;

				for (int i = 0; i < intRowCount; i++) {
				strCurrent = aDrSorted[i][intDistinctColumn].ToString();
				if (strPrevious != strCurrent) dtblDistinct.ImportRow(aDrSorted[i]);
				strPrevious = strCurrent;
				}
		        
				return dtblDistinct;
			} else {
				return null;
			}
		}
    
		#region Select Distinct Overloads
		//################################################################################################
		//
		// DataSetUtilities.SelectDistinct - overload
		//
		/// <summary>
		/// An overload of SelectDistinct
		/// </summary>
		//
		//################################################################################################
		public static DataTable SelectDistinct(DataTable dtblSubject, int intDistinctColumn, string strTableName) {
			return SelectDistinct(dtblSubject, intDistinctColumn, intDistinctColumn, "ASC", strTableName);
		}

		//################################################################################################
		//
		// DataSetUtilities.SelectDistinct - overload
		//
		/// <summary>
		/// An overload of SelectDistinct
		/// </summary>
		//
		//################################################################################################
		public static DataTable SelectDistinct(DataTable dtblSubject, int intDistinctColumn, string strOrder, string strTableName) {
			return SelectDistinct(dtblSubject, intDistinctColumn, intDistinctColumn, strOrder, strTableName);
		}
	      
		//################################################################################################
		//
		// DataSetUtilities.SelectDistinct - overload
		//
		/// <summary>
		/// An overload of SelectDistinct
		/// </summary>
		//
		//################################################################################################
		public static DataTable SelectDistinct(DataRow[] aDrSubject, DataColumnCollection dccFields, int intDistinctColumn, string strTableName) {
			return SelectDistinct(aDrSubject, dccFields, intDistinctColumn, "ASC", strTableName);
		}
	      
		//################################################################################################
		//
		// DataSetUtilities.SelectDistinct - overload
		//
		/// <summary>
		/// An overload of SelectDistinct
		/// </summary>
		//
		//################################################################################################
		public static DataTable SelectDistinct(DataRow[] aDrSubject, DataColumnCollection dccFields, int intDistinctColumn, string strOrder, string strTableName) {
			if (aDrSubject[0].ItemArray.Length == dccFields.Count) {
			// Create a DataTable out of the DataRow array and DataColumnCollection
			DataTable dtblSubject = new DataTable(strTableName + "_sorted");
			foreach (DataColumn dc in dccFields) { dtblSubject.Columns.Add(dc.ColumnName, dc.DataType); }

			// For a DataRow Array, we need to put the rows back into a table so we can use the Select method.
			// There is probably a better way to accomplish this, so I'll come back and play with this more
			// when I can.
			int intRowLength = aDrSubject.Length;
			for (int i = 0; i < intRowLength; i++) { dtblSubject.ImportRow(aDrSubject[i]); }
	          
			return SelectDistinct(dtblSubject, intDistinctColumn, intDistinctColumn, strOrder, strTableName);
			}
			return null;
		}
		#endregion
		#endregion
	  
		#region DataRowValue
		private static object DataRowValue(DataRow dr, string[] aStrColumns) {
			object obj;
			foreach (string strCol in aStrColumns) {
			try { obj = dr[strCol]; } catch { obj = null; }
			if (obj != null) return obj;
			}
			return null;
		}
		  
		/// <summary>
		/// Returns the value of ONE out of an array of DataRow fields. This method was developed
		/// to support objects that may possibly receive data from multiple sources and could
		/// possible call the fields something different. The return type of this method is
		/// determined by the DEFAULT field. What ever data type that field is will be the data type
		/// returned.
		/// </summary>
		/// <param name="dr">DataRow - The data row to pull the field from</param>
		/// <param name="aStrColumns">string[] - An array of field names to attempt pulling the data from</param>
		/// <param name="strDefault">string - A default value to use if none of the fields are found.</param>
		/// <returns>string</returns>
		public static string DataRowValue(DataRow dr, string[] aStrColumns, string strDefault) {
			string strReturn;
			try { strReturn = DataRowValue(dr, aStrColumns).ToString(); } catch { strReturn = strDefault; }
			return strReturn;
		}

		/// <summary>
		/// Returns the value of ONE out of an array of DataRow fields. This method was developed
		/// to support objects that may possibly receive data from multiple sources and could
		/// possible call the fields something different. The return type of this method is
		/// determined by the DEFAULT field. What ever data type that field is will be the data type
		/// returned.
		/// </summary>
		/// <param name="dr">DataRow - The data row to pull the field from</param>
		/// <param name="aStrColumns">string[] - An array of field names to attempt pulling the data from</param>
		/// <param name="intDefault">int - A default value to use if none of the fields are found.</param>
		/// <returns>int</returns>
		public static int DataRowValue(DataRow dr, string[] aStrColumns, int intDefault) {
			int intReturn;
			try { intReturn = Convert.ToInt32(DataRowValue(dr, aStrColumns)); } catch { intReturn = intDefault; }
			return intReturn;
		}

		/// <summary>
		/// Returns the value of ONE out of an array of DataRow fields. This method was developed
		/// to support objects that may possibly receive data from multiple sources and could
		/// possible call the fields something different. The return type of this method is
		/// determined by the DEFAULT field. What ever data type that field is will be the data type
		/// returned.
		/// </summary>
		/// <param name="dr">DataRow - The data row to pull the field from</param>
		/// <param name="aStrColumns">string[] - An array of field names to attempt pulling the data from</param>
		/// <param name="intDefault">long - A default value to use if none of the fields are found.</param>
		/// <returns>long</returns>
		public static long DataRowValue(DataRow dr, string[] aStrColumns, long intDefault) {
			long intReturn;
			try { intReturn = Convert.ToInt64(DataRowValue(dr, aStrColumns)); } catch { intReturn = intDefault; }
			return intReturn;
		}

		/// <summary>
		/// Returns the value of ONE out of an array of DataRow fields. This method was developed
		/// to support objects that may possibly receive data from multiple sources and could
		/// possible call the fields something different. The return type of this method is
		/// determined by the DEFAULT field. What ever data type that field is will be the data type
		/// returned.
		/// </summary>
		/// <param name="dr">DataRow - The data row to pull the field from</param>
		/// <param name="aStrColumns">string[] - An array of field names to attempt pulling the data from</param>
		/// <param name="dblDefault">double - A default value to use if none of the fields are found.</param>
		/// <returns>double</returns>
		public static double DataRowValue(DataRow dr, string[] aStrColumns, double dblDefault) {
			double dblReturn;
			try { dblReturn = Convert.ToDouble(DataRowValue(dr, aStrColumns)); } catch { dblReturn = dblDefault; }
			return dblReturn;
		}

		/// <summary>
		/// Returns the value of ONE out of an array of DataRow fields. This method was developed
		/// to support objects that may possibly receive data from multiple sources and could
		/// possible call the fields something different. The return type of this method is
		/// determined by the DEFAULT field. What ever data type that field is will be the data type
		/// returned.
		/// </summary>
		/// <param name="dr">DataRow - The data row to pull the field from</param>
		/// <param name="aStrColumns">string[] - An array of field names to attempt pulling the data from</param>
		/// <param name="blnDefault">boolean - A default value to use if none of the fields are found.</param>
		/// <returns>bool</returns>
		public static bool DataRowValue(DataRow dr, string[] aStrColumns, bool blnDefault) {
			bool blnReturn;
			try { blnReturn = Convert.ToBoolean(DataRowValue(dr, aStrColumns)); } catch { blnReturn = blnDefault; }
			return blnReturn;
		}
		
		/// <summary>
		/// Returns the value of ONE out of an array of DataRow fields. This method was developed
		/// to support objects that may possibly receive data from multiple sources and could
		/// possible call the fields something different. The return type of this method is
		/// determined by the DEFAULT field. What ever data type that field is will be the data type
		/// returned.
		/// </summary>
		/// <param name="dr">DataRow - The data row to pull the field from</param>
		/// <param name="aStrColumns">string[] - An array of field names to attempt pulling the data from</param>
		/// <param name="dtDefault">DateTime - A default value to use if none of the fields are found.</param>
		/// <returns>DateTime</returns>
		public static DateTime DataRowValue(DataRow dr, string[] aStrColumns, DateTime dtDefault) {
			DateTime dtReturn;
			try { dtReturn = Convert.ToDateTime(DataRowValue(dr, aStrColumns)); } catch { dtReturn = dtDefault; }
			return dtReturn;
		}
		#endregion

		#region TentativeLoadCheck
		public static bool TentativeLoadCheck(DataRow dr, string[] aryColumns)
		{
			object obj;
			//The following columns must exist in order for a TentativeLoad to succeed, 
			//if they do not exist, the TentativeLoad will fail silently,
			//leaving Data On Demand to load the object.
			foreach (string strCol in aryColumns) 
			{
				try { obj = dr[strCol]; } 
				catch { return false; }
				if (obj == null) return false;
			}
			return true;
		}
		#endregion
	}
}
