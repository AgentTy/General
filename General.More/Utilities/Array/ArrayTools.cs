using System;
using System.Collections.Generic;
using System.Linq;
using General;
using General.Utilities.Text;
using General.Model;

namespace General.Utilities.Array
{
	/// <summary>
	/// Array Tools
	/// </summary>
	public class ArrayTools
	{
		#region Constructors
		/// <summary>
		/// Array Tools
		/// </summary>
		private ArrayTools() {} // All methods are private
		#endregion

		#region Contains

		/// <summary>
		/// Returns true if an array contains a specified value
		/// </summary>
		public static bool Contains(object[] Array,object Value)
		{
			foreach(object i in Array)
			{
				if(i == Value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Returns true if an array contains a specified value
		/// </summary>
		public static bool Contains(int[] Array,int Value)
		{
			foreach(int i in Array)
			{
				if(i == Value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Returns true if an array contains a specified value
		/// </summary>
		public static bool Contains(string[] Array,string Value)
		{
			foreach(string i in Array)
			{
				if(i == Value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Returns true if an array contains a specified value
		/// </summary>
		public static bool Contains(double[] Array,double Value)
		{
			foreach(double i in Array)
			{
				if(i == Value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Returns true if an array contains a specified value
		/// </summary>
		public static bool Contains(EmailAddress[] Array,EmailAddress Value)
		{
			foreach(object i in Array)
			{
				if(i.ToString() == Value.ToString())
					return true;
			}
			return false;
		}

		/// <summary>
		/// Returns true if an array contains a specified value
		/// </summary>
		public static bool Contains(PhoneNumber[] Array,PhoneNumber Value)
		{
			foreach(object i in Array)
			{
				if(i.ToString() == Value.ToString())
					return true;
			}
			return false;
		}
		#endregion

		#region Join
		/// <summary>
		/// Returns a string with the contents of an array
		/// </summary>
		public static string Join(string[] Array,string Delimiter)
		{
			string result = "";
			foreach(string i in Array)
			{
				result+= i+Delimiter;
			}
			if(result != String.Empty) result = StringFunctions.Shave(result,1);
			return result;
		}
		
		#region Join String Overloads
		public static string Join(string[] aStrArray, string strPrefix, string strSuffix) {
			string strResult = string.Empty;
			foreach (string str in aStrArray) {
				strResult += strPrefix + str + strSuffix;
			}
			return strResult;
		}
		#endregion

		/// <summary>
		/// Returns a string with the contents of an array
		/// </summary>
		public static string Join(int[] Array,string Delimiter)
		{
			string result = "";
			foreach(int i in Array)
			{
				result+= i+Delimiter;
			}
			if(result != String.Empty) result = StringFunctions.Shave(result,1);
			return result;
		}

		/// <summary>
		/// Returns a string with the contents of an array
		/// </summary>
		public static string Join(double[] Array,string Delimiter)
		{
			string result = "";
			foreach(double i in Array)
			{
				result+= i+Delimiter;
			}
			if(result != String.Empty) result = StringFunctions.Shave(result,1);
			return result;
		}
		#endregion

        #region CreateArrayOfRangeIncrement
        public static List<int> CreateArrayOfRangeIncrement(int start, int end, int increment)
        {
            return Enumerable
                .Repeat(start, (int)((end - start) / increment) + 1)
                .Select((tr, ti) => tr + (increment * ti))
                .ToList();
        }

        public static List<decimal> CreateArrayOfRangeIncrement(decimal start, decimal end, decimal increment)
        {
            return Enumerable
                .Repeat(start, (int)((end - start) / increment) + 1)
                .Select((tr, ti) => tr + (increment * ti))
                .ToList();
        }
        #endregion

    }
}
