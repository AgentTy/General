using System;
using System.Collections;

namespace General.Utilities.Date {
	/// <summary>
	/// General definition of a Collection of Months.
	/// </summary>
	public class Months : IEnumerable, IEnumerator {
		#region Public Constructors
		/// <summary>
		/// Creates a Months collection.
		/// </summary>
		public Months() { Fill(false); }

		public Months(bool AddMonthHeader) { Fill(AddMonthHeader); }
		#endregion
		
		#region Public Properties
		/// <summary>
		/// Returns the number of items in the collection
		/// </summary>
		/// <returns>int</returns>
		public int Count { get { return _objLines.Count; } }
		
		/// <summary>
		/// Returns the Month at the specified index
		/// </summary>
		/// <returns>Month</returns>
		public Month this[int intIndex] { get {
			try { return (Month) _objLines[intIndex]; } catch { return null; }
		} }
		#endregion
		
		#region Private Variables
		private ArrayList _objLines;
		private int _intIndex = -1;
		#endregion
		
		#region Private Methods
		private void Fill(bool boolAddMonthHeader) {
			try {
				_objLines = new ArrayList();
				
				if(boolAddMonthHeader)
					_objLines.Add(Month.CreateMonth("00", "Month", "Month", 31));
				
				_objLines.Add(Month.CreateMonth("01", "January", "Jan", 31));
				_objLines.Add(Month.CreateMonth("02", "February", "Feb", 28, true));
				_objLines.Add(Month.CreateMonth("03", "March", "Mar", 31));
				_objLines.Add(Month.CreateMonth("04", "April", "Apr", 30));
				_objLines.Add(Month.CreateMonth("05", "May", "May", 31));
				_objLines.Add(Month.CreateMonth("06", "June", "Jun", 30));
				_objLines.Add(Month.CreateMonth("07", "July", "Jul", 31));
				_objLines.Add(Month.CreateMonth("08", "August", "Aug", 31));
				_objLines.Add(Month.CreateMonth("09", "September", "Sept", 30));
				_objLines.Add(Month.CreateMonth("10", "October", "Oct", 31));
				_objLines.Add(Month.CreateMonth("11", "November", "Nov", 30));
				_objLines.Add(Month.CreateMonth("12", "December", "Dec", 31));
			} catch (Exception ex) {
				throw new Exception(ex.Message);
			}
		}
		#endregion
	
	    #region IEnumerable Implementation
		#region IEnumerable Members
		/// <summary>
		/// Gets the Enumerator object
		/// </summary>
		/// <returns>IEnumerator</returns>
		public IEnumerator GetEnumerator() { Reset(); return (IEnumerator) this; }
		#endregion

		#region IEnumerator Members
		#region Public Properties
		/// <summary>
		/// Gets the current object
		/// </summary>
		/// <returns>object</returns>
		public object Current { get {
			try { return _objLines[_intIndex]; } catch { return null; }
		} }
		#endregion

		#region Public Methods
		/// <summary>
		/// Resets the enumeration index
		/// </summary>
		public void Reset() {
			_intIndex = -1;
		}
		
		/// <summary>
		/// Moves to the next object in the enumeration
		/// </summary>
		/// <returns>bool</returns>
		public bool MoveNext() {
			if(_objLines != null && _intIndex < _objLines.Count -1) {
				_intIndex++;
				return true;
			}
			
			Reset();
			return false;
		}
		#endregion
		#endregion
		#endregion
	}
	
	/// <summary>
	/// Represents an individual month.
	/// </summary>
	public class Month {
		#region Creation Methods
		/// <summary>
		/// Creates a month using the passed in Leap parameter
		/// </summary>
		/// <param name="strValue">string - The two digit month value</param>
		/// <param name="strName">string - The full name of the month</param>
		/// <param name="strAbbreviation">string - The digit abbreviation for the month (up to 4 digits)</param>
		/// <param name="intDays">int - The number of days in the month for a non-leap year</param>
		/// <param name="blnLeap">bool - Whether or not the month is affected by leap year</param>
		/// <returns>Month</returns>
		public static Month CreateMonth(string strValue, string strName, string strAbbreviation, int intDays, bool blnLeap) {
			return new Month(strValue, strName, strAbbreviation, intDays, blnLeap);
		}

		/// <summary>
		/// Creates a month that is not affected by leap year
		/// </summary>
		/// <param name="strValue">string - The two digit month value</param>
		/// <param name="strName">string - The full name of the month</param>
		/// <param name="strAbbreviation">string - The digit abbreviation for the month (up to 4 digits)</param>
		/// <param name="intDays">int - The number of days in the month for a non-leap year</param>
		/// <returns>Month</returns>
		public static Month CreateMonth(string strValue, string strName, string strAbbreviation, int intDays) {
			return new Month(strValue, strName, strAbbreviation, intDays, false);
		}
		#endregion
		
		#region Private Constructors (Support for creation methods)
		private Month(string strValue, string strName, string strAbbreviation, int intDays, bool blnLeap) {
			_strValue = strValue;
			_strName = strName;
			_strAbbreviation = strAbbreviation;
			_intDays = intDays;
			_blnLeap = blnLeap;
		}
		#endregion
		
		#region Public Properties
		public string Value { get { return _strValue; } }
		public string Name { get { return _strName; } }
		public string Abbreviation { get { return _strAbbreviation; } }
		public int Days { get { return GetDays(); } }
		#endregion
		
		#region Private Variables
		private string _strValue, _strName, _strAbbreviation;
		private int _intDays;
		private bool _blnLeap;
		#endregion
		
		#region Private Methods
		private int GetDays() {
			if (!_blnLeap) return _intDays;
			
			// Check if this is leap year.
			int intYear = DateTime.Now.Year;
			bool blnIsLeap = false;
			
			if (intYear % 4 == 0) {
				blnIsLeap = true;
				
				// Leap year is every 4 years, except if that year is an even 100 AND even 400
				if (intYear % 100 == 0) {
					if (intYear % 400 == 0) blnIsLeap = false;
				}
			}

			return (blnIsLeap) ? _intDays + 1 : _intDays;
		}
		#endregion
	}
}
