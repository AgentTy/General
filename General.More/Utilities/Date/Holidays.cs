using System;
using System.Collections;
using General;

namespace General.Utilities.Date
{
	/// <summary>
	/// Summary description for Holidays.
	/// </summary>
	public class Holidays : IEnumerable, IEnumerator 
	{
		#region Public Constructors
		/// <summary>
		/// Creates a Months collection.
		/// </summary>
		public Holidays(int intYear) { Fill(intYear); }

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
		public Holiday this[int intIndex] 
		{
			get 
			{
				try { return (Holiday) _objLines[intIndex]; } 
				catch { return null; }
			} 
		}
		#endregion
		
		#region Private Variables
		private ArrayList _objLines;
		private int _intIndex = -1;
		#endregion
		
		#region Private Methods
		private void Fill(int intYear) 
		{
			try 
			{
				_objLines = new ArrayList();
				
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.ChristmasDay,intYear));
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.ChristmasObserved,intYear));
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.ColumbusDay,intYear));
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.IndependenceDay,intYear));
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.IndependenceDayObserved,intYear));
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.LaborDay,intYear));
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.MartinLutherKingDay,intYear));
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.MemorialDay,intYear));
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.NewYearsDay,intYear));
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.NewYearsObserved,intYear));
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.PresidentsDay,intYear));
				_objLines.Add(Holiday.CreateHoliday(HolidayTypes.ThanksgivingDay,intYear));
			} 
			catch (Exception ex) 
			{
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
		public object Current 
		{
			get {
									try { return _objLines[_intIndex]; } 
									catch { return null; }
								} 
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Resets the enumeration index
		/// </summary>
		public void Reset() 
		{
			_intIndex = -1;
		}
		
		/// <summary>
		/// Moves to the next object in the enumeration
		/// </summary>
		/// <returns>bool</returns>
		public bool MoveNext() 
		{
			if(_objLines != null && _intIndex < _objLines.Count -1) 
			{
				_intIndex++;
				return true;
			}
			
			Reset();
			return false;
		}
		#endregion
		#endregion
		#endregion

		#region Static Methods

		#region Definitions

		/// <summary>
		/// Returns Christmas Day
		/// </summary>
		public static DateTime ChristmasDay(int intYear) 
		{
			DateTime _Xmas = new DateTime(intYear, 12, 25);
			return _Xmas;
		}

		/// <summary>
		/// Returns Christmas Holiday
		/// </summary>
		public static DateTime ChristmasHoliday(int intYear) 
		{
			DateTime _Xmas = new DateTime(intYear, 12, 25);
			switch(_Xmas.DayOfWeek)
			{
				case DayOfWeek.Saturday:
					return _Xmas.AddDays(-1);
				case DayOfWeek.Sunday:
					return _Xmas.AddDays(+1);
				default:
					return _Xmas;
			}
		}


		
		/// <summary>
		/// Returs true if the given date is Independance Day
		/// </summary>
		public static DateTime IndependenceDay(int intYear) 
		{
			DateTime _4th = new DateTime(intYear, 7, 4);
			return _4th;
		}

		/// <summary>
		/// Returs true if the given date is the observed Independance Holiday, this could be before or after July 4th if Indepandance Day falls on a weekend.
		/// </summary>
		public static DateTime IndependenceHoliday(int intYear) 
		{
			DateTime _4th = new DateTime(intYear, 7, 4);
			switch(_4th.DayOfWeek)
			{
				case DayOfWeek.Saturday:
					return _4th.AddDays(-1);
				case DayOfWeek.Sunday:
					return _4th.AddDays(+1);
				default:
					return _4th;
			}

		}

		/// <summary>
		/// Returs true if the given date is New Years Day
		/// </summary>
		public static DateTime NewYearsDay(int intYear)  
		{
			DateTime _day = new DateTime(intYear, 1, 1);
			return _day;
		}

		/// <summary>
		/// Returs true if the given date is the observed New Years Holiday, this could be before or after January 1st if New Years falls on a weekend.
		/// </summary>
		public static DateTime NewYearsHoliday(int intYear)  
		{

			DateTime _newYear;
			_newYear = new DateTime(intYear,1,1);
        
			switch( _newYear.DayOfWeek)
			{
				case DayOfWeek.Saturday:
					return _newYear.AddDays(-1);
				case DayOfWeek.Sunday:
					return _newYear.AddDays(+1);
				default:
					return _newYear;
			}
		}


		
		/// <summary>
		/// Returs true if the given date is Thanksgiving Day
		/// </summary>
		public static DateTime ThanksgivingDay(int intYear)  
		{
			//Last Thursday in Nov.
			DateTime _dt29 = new DateTime(intYear, 11, 28);
			DateTime _dt1 = new DateTime(intYear, 11, 1);
			DateTime _returnDT = _dt29.AddDays(-1 * DateTools.OffsetFromFirstDOW(_dt1, DayOfWeek.Friday));
			return _returnDT;
		}


		/// <summary>
		/// Returs true if the given date is Martin Luther King Day
		/// </summary>
		public static DateTime MartinLutherKingDay(int intYear)  
		{
			//Third monday in january
			DateTime p_date = new DateTime(intYear,1,1);
			DateTime _day = new DateTime(intYear,1,DateTools.NthDayOfWeekOfMonth(3, DayOfWeek.Monday, p_date));
			return _day;
		}

		/// <summary>
		/// Returs true if the given date is Memorial Day
		/// </summary>
		public static DateTime MemorialDay(int intYear)  
		{
			//Last monday in May
			DateTime p_date = new DateTime(intYear,5,1);
			DateTime _day = new DateTime(intYear,5,DateTools.NthDayOfWeekOfMonth(5, DayOfWeek.Monday, p_date));
			return _day;
		}

		/// <summary>
		/// Returs true if the given date is Presidents Day
		/// </summary>
		public static DateTime PresidentsDay(int intYear)  
		{
			//Third Monday in Feb
			DateTime p_date = new DateTime(intYear,2,1);
			DateTime _day = new DateTime(intYear,2, DateTools.NthDayOfWeekOfMonth(3, DayOfWeek.Monday, p_date));
			return _day;
		}

		/// <summary>
		/// Returs true if the given date is Labor Day
		/// </summary>
		public static DateTime LaborDay(int intYear)  
		{
			//First Monday in Sept.
			DateTime p_date = new DateTime(intYear,9,1);
			DateTime _day = new DateTime(intYear,9, DateTools.NthDayOfWeekOfMonth(1, DayOfWeek.Monday, p_date));
			return _day;
		}

		/// <summary>
		/// Returs true if the given date is Columbus Day
		/// </summary>
		public static DateTime ColumbusDay(int intYear)  
		{
			//Second Monday in Oct
			DateTime p_date = new DateTime(intYear,10,1);
			DateTime _day = new DateTime(intYear,10,DateTools.NthDayOfWeekOfMonth(2, DayOfWeek.Monday, p_date));
			return _day;
		}

		#endregion

		#region Fixed Holidays
		/// <summary>
		/// Returs true if the given date is Christmas Day
		/// </summary>
		public static bool IsChristmasDay( DateTime p_date)
		{
			return p_date.Month == 12 && p_date.Day == 25;
		}

		/// <summary>
		/// Returs true if the given date is the observed Christmas Holiday, this could be before or after December 25th if Christmas day falls on a weekend.
		/// </summary>
		public static bool IsChristmasHoliday( DateTime p_date) 
		{
			bool _return;
			DateTime _Xmas = new DateTime(p_date.Year, 12, 25);
			switch(_Xmas.DayOfWeek)
			{
				case DayOfWeek.Saturday:
					_return = p_date == _Xmas.AddDays(-1);
					break;
				case DayOfWeek.Sunday:
					_return = p_date == _Xmas.AddDays(+1);
					break;
				default:
					_return = p_date == _Xmas;
					break;
			}
			return _return;
		}



		/// <summary>
		/// Returs true if the given date is Independance Day
		/// </summary>
		public static bool IsIndependenceDay( DateTime p_date) 
		{
			return p_date.Month == 7 && p_date.Day == 4;
		}

		/// <summary>
		/// Returs true if the given date is the observed Independance Holiday, this could be before or after July 4th if Indepandance Day falls on a weekend.
		/// </summary>
		public static bool IsIndependenceHoliday( DateTime p_date) 
		{
			bool _return;
			DateTime _4th = new DateTime(p_date.Year, 7, 4);
			switch(_4th.DayOfWeek)
			{
				case DayOfWeek.Saturday:
					_return = (p_date == _4th.AddDays(-1));
					break;
				case DayOfWeek.Sunday:
					_return = p_date == _4th.AddDays(+1);
					break;
				default:
					_return = p_date == _4th;
					break;
			}
			return _return;
		}

		/// <summary>
		/// Returs true if the given date is New Years Day
		/// </summary>
		public static bool IsNewYearsDay( DateTime p_date)  
		{
			return p_date.Month == 1 && p_date.Day == 1;
		}

		/// <summary>
		/// Returs true if the given date is the observed New Years Holiday, this could be before or after January 1st if New Years falls on a weekend.
		/// </summary>
		public static bool IsNewYearsHoliday( DateTime p_date)  
		{

			DateTime _newYear;
			if(p_date.Day == 31)
				_newYear = DateTime.Parse("1/1/" + p_date.AddDays(1).Year.ToString());
			else
				_newYear = DateTime.Parse("1/1/" + p_date.Year.ToString());
        

			bool _return ;
			switch( _newYear.DayOfWeek)
			{
				case DayOfWeek.Saturday:
					_return = p_date == _newYear.AddDays(-1);
					break;
				case DayOfWeek.Sunday:
					_return = p_date == _newYear.AddDays(+1);
					break;
				default:
					_return = p_date == _newYear;
					break;
			}
			return _return;
		}
		#endregion

		#region Variable Holidays
		/// <summary>
		/// Returs true if the given date is Thanksgiving Day
		/// </summary>
		public static bool IsThanksgiving( DateTime p_date)  
		{
			//Last Thursday in Nov.
			DateTime _dt29 = new DateTime(p_date.Year, 11, 28);
			DateTime _dt1 = new DateTime(p_date.Year, 11, 1);
			DateTime _returnDT = _dt29.AddDays(-1 * DateTools.OffsetFromFirstDOW(_dt1, DayOfWeek.Friday));

			return p_date.Equals(_returnDT);
		}


		/// <summary>
		/// Returs true if the given date is Martin Luther King Day
		/// </summary>
		public static bool IsMartinLutherKingDay( DateTime p_date)  
		{
			//Third monday in january
			return p_date.Month == 1 && (p_date.Day == DateTools.NthDayOfWeekOfMonth(3, DayOfWeek.Monday, p_date));
		}

		/// <summary>
		/// Returs true if the given date is Memorial Day
		/// </summary>
		public static bool IsMemorialDay( DateTime p_date)  
		{
			//Last monday in May
			bool _return ;
			_return = p_date.Month == 5 && (p_date.Day == DateTools.NthDayOfWeekOfMonth(5, DayOfWeek.Monday, p_date));
			return _return;
		}

		/// <summary>
		/// Returs true if the given date is Presidents Day
		/// </summary>
		public static bool IsPresidentsDay( DateTime p_date)  
		{
			//Third Monday in Feb
			return p_date.Month == 2 && (p_date.Day == DateTools.NthDayOfWeekOfMonth(3, DayOfWeek.Monday, p_date));
		}

		/// <summary>
		/// Returs true if the given date is Labor Day
		/// </summary>
		public static bool IsLaborDay( DateTime p_date)  
		{
			//First Monday in Sept.
			return p_date.Month == 9 && (p_date.Day == DateTools.NthDayOfWeekOfMonth(1, DayOfWeek.Monday, p_date));
		}

		/// <summary>
		/// Returs true if the given date is Columbus Day
		/// </summary>
		public static bool IsColumbusDay( DateTime p_date)  
		{
			//Second Monday in Oct
			return p_date.Month == 10 && (p_date.Day == DateTools.NthDayOfWeekOfMonth(2, DayOfWeek.Monday, p_date));
		}
		#endregion

		#endregion
	}
	

	/// <summary>
	/// Represents an individual Holiday.
	/// </summary>
	public class Holiday 
	{
		#region Creation Methods
		/// <summary>
		/// Creates a Holiday using the passed in parameters
		/// </summary>
		/// <param name="dtValue">DateTime - The date of the holiday</param>
		/// <param name="strName">String - The name of the holiday</param>
		/// <returns>Holiday</returns>
		public static Holiday CreateHoliday(HolidayTypes objHolidayType, int intYear) 
		{
			return new Holiday(objHolidayType, intYear);
		}
		#endregion
		
		#region Private Constructors (Support for creation methods)
		private Holiday(HolidayTypes objHolidayType, int intYear) 
		{
			_objHolidayType = objHolidayType;

			switch(_objHolidayType)
			{
				case HolidayTypes.ChristmasDay: 
					_dtValue = Holidays.ChristmasDay(intYear);
					_strName = "Christmas";
					break;
				case HolidayTypes.ChristmasObserved: 
					_dtValue = Holidays.ChristmasHoliday(intYear);
					_strName = "Christmas (observed)";
					break;
				case HolidayTypes.ColumbusDay: 
					_dtValue = Holidays.ColumbusDay(intYear);
					_strName = "Columbus Day";
					break;
				case HolidayTypes.IndependenceDay: 
					_dtValue = Holidays.IndependenceDay(intYear);
					_strName = "Independence Day (US)";
					break;
				case HolidayTypes.IndependenceDayObserved: 
					_dtValue = Holidays.IndependenceHoliday(intYear);
					_strName = "Independance Day (US) (obs.)";
					break;
				case HolidayTypes.LaborDay: 
					_dtValue = Holidays.LaborDay(intYear);
					_strName = "Labor Day";
					break;
				case HolidayTypes.MartinLutherKingDay: 
					_dtValue = Holidays.MartinLutherKingDay(intYear);
					_strName = "Martin Luther King Day";
					break;
				case HolidayTypes.MemorialDay: 
					_dtValue = Holidays.MemorialDay(intYear);
					_strName = "Memorial Day";
					break;
				case HolidayTypes.NewYearsDay: 
					_dtValue = Holidays.NewYearsDay(intYear);
					_strName = "New Years Day";
					break;
				case HolidayTypes.NewYearsObserved: 
					_dtValue = Holidays.NewYearsHoliday(intYear);
					_strName = "New Years (observed)";
					break;
				case HolidayTypes.PresidentsDay: 
					_dtValue = Holidays.PresidentsDay(intYear);
					_strName = "Presidents Day (US)";
					break;
				case HolidayTypes.ThanksgivingDay: 
					_dtValue = Holidays.ThanksgivingDay(intYear);
					_strName = "Thanksgiving Day";
					break;
			}

			_strName = _strName + " " + _dtValue.Year + "-" + StringFunctions.TwoDigitFormat(_dtValue.Month) + "-" + StringFunctions.TwoDigitFormat(_dtValue.Day);
		}
		#endregion
		
		#region Public Properties
		public HolidayTypes HolidayType { get {return _objHolidayType;} }
		public int HolidayTypeID { get {return (int) _objHolidayType;} }
		public DateTime Value { get { return _dtValue; } }
		public string Name { get { return _strName; } }
		#endregion
		
		#region Private Variables
		private HolidayTypes _objHolidayType;
		private DateTime _dtValue;
		private string _strName;
		#endregion
		
	}

	public enum HolidayTypes
	{
		ChristmasDay = 1,
		ChristmasObserved = 2,
		ColumbusDay = 3,
		IndependenceDay = 4,
		IndependenceDayObserved = 5,
		LaborDay = 6,
		MartinLutherKingDay = 7,
		MemorialDay = 8,
		NewYearsDay = 9,
		NewYearsObserved = 10,
		PresidentsDay = 11,
		ThanksgivingDay = 12
	}
}
