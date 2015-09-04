using System;
using System.Collections.Generic;
using System.Linq;

namespace General.Utilities.Date
{
	/// <summary>
	/// Tools for working with DateTime objects
	/// </summary>
	public class DateTools
    {
        #region ParseAny
        public static DateTime ParseAny(string strDate)
        {
            DateTime dtDate;
            if (DateTime.TryParse(strDate, out dtDate))
                return dtDate;

            strDate = strDate.ToLowerInvariant();

            strDate = strDate.Replace("august", "aug"); //Prevent problem with st below
            strDate = strDate.Replace("sept ", "sep ");
            strDate = strDate.Replace("sept,", "sep,");

            strDate = strDate.Replace("st,", ",");
            strDate = strDate.Replace("nd,", ",");
            strDate = strDate.Replace("rd,", ",");
            strDate = strDate.Replace("th,", ",");

            strDate = strDate.Replace("st ",",");
            strDate = strDate.Replace("nd ", ",");
            strDate = strDate.Replace("rd ", ",");
            strDate = strDate.Replace("th ", ",");

            strDate = strDate.Replace("\\", "/");
            strDate = strDate.Replace("tbd", "");

            strDate = strDate.Replace("winter", "january");
            strDate = strDate.Replace("spring", "april");
            strDate = strDate.Replace("summer", "july");
            strDate = strDate.Replace("fall", "october");

            strDate = strDate.Trim();

            if (DateTime.TryParse(strDate, out dtDate))
                return dtDate;

            strDate = StringFunctions.ForceNumeric(strDate);
            if (strDate.Length == 4 && StringFunctions.IsNumeric(strDate)) //4 digit year
                return new DateTime(int.Parse(strDate), 1, 1);
            
            return DateTime.Parse(strDate);
        }
        #endregion

        #region FromUnixTime / ToUnixTime
        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static long ToUnixTime(DateTime date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
        }
        #endregion

        #region GetRFC822Date
        /// <summary>
        /// Converts a regular DateTime to a RFC822 date string.
        /// </summary>
        /// <returns>The specified date formatted as a RFC822 date string.</returns>
        public static string GetRFC822Date(DateTime date)
        {
            int offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            string timeZone = "+" + offset.ToString().PadLeft(2, '0');

            if (offset < 0)
            {
                int i = offset * -1;
                timeZone = "-" + i.ToString().PadLeft(2, '0');
            }

            return date.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'));
        }
        #endregion

        #region GetWeeks
        /// <summary>
		/// Calculate weeks between starting date and ending date
		/// </summary>
		/// <param name="stdate"></param>
		/// <param name="eddate"></param>
		/// <returns></returns>
		public static int GetWeeks(DateTime stdate, DateTime eddate )
		{
			TimeSpan t= eddate - stdate;
			int iDays;

			if( t.Days < 7)
			{
				if(stdate.DayOfWeek > eddate.DayOfWeek)
					return 1; //It is accross the week 

				else
					return 0; // same week
			}
			else
			{
				iDays = t.Days -7 +(int) stdate.DayOfWeek ;
				int i;
				int k=0;

				for(i=1;k<iDays ;i++)
				{
					k+=7;
				}

				if(i>1 && eddate.DayOfWeek != DayOfWeek.Sunday ) i-=1; 
				return i;
			} 
		}

		#endregion

		#region DateDiff
		/// <summary>
		/// Mimic the Implementation of DateDiff function of VB.Net.
		/// Note : Number of Year/Month is calculated
		///        as how many times you have crossed the boundry.
		/// e.g. if you say starting date is 29/01/2005
		///        and 01/02/2005 the year will be 0,month will be 1.
		/// 
		/// </summary>
		/// <param name="datePart">specifies on which part 
		///   of the date to calculate the difference </param>
		/// <param name="startDate">Datetime object containing
		///   the beginning date for the calculation</param>
		/// <param name="endDate">Datetime object containing
		///   the ending date for the calculation</param>
		/// <returns></returns>
		public static double DateDiff(string datePart, 
			DateTime startDate, DateTime endDate)
		{

			//Get the difference in terms of TimeSpan
			TimeSpan T;

			T = endDate - startDate;

			//Get the difference in terms of Month and Year.
			int sMonth, eMonth, sYear, eYear;
			sMonth = startDate.Month;
			eMonth = endDate.Month;
			sYear = startDate.Year;
			eYear = endDate.Year; 
			double Months,Years;
			Months = eMonth - sMonth;
			Years = eYear - sYear;
			Months = Months + ( Years*12);

			switch(datePart.ToUpper())
			{
				case "WW":
				case "DW":
					return GetWeeks(startDate,endDate);
				case "MM":
					return Months;
				case "YY":
				case "YYYY":
					return Years;
				case "QQ":
				case "QQQQ":
					//Difference in Terms of Quater
					return Math.Ceiling(T.Days / 90.0);
				case "MI":
				case "N":
					return T.TotalMinutes ;
				case "HH":
					return T.TotalHours ;
				case "SS":
					return T.TotalSeconds;
				case "MS":
					return T.TotalMilliseconds;
				case "DD":
				default:
					return T.Days; 
			}
		}
		#endregion

		#region Age

		/// <summary>
		/// Calculate Age on given date.
		/// Calculates as Years, Months and Days.
		/// </summary>
		/// <param name="DOB">Datetime object 
		/// containing DOB value</param>
		/// <param name="OnDate">Datetime object containing given 
		/// date, for which we need to calculate the age</param>
		/// <returns></returns>

		public static string Age(DateTime DOB, DateTime OnDate)
		{
			//Get the difference in terms of Month and Year.
			int sMonth, eMonth, sYear, eYear;
			double Months, Years;

			sMonth = DOB.Month;
			eMonth = OnDate.Month;
			sYear = DOB.Year;
			eYear = OnDate.Year; 

			// calculate Year
			if( eMonth >= sMonth) 
				Years = eYear - sYear;
			else
				Years = eYear - sYear -1;

			//calculate Months
			if( eMonth >= sMonth) 
				Months = eMonth - sMonth;
			else
				if ( OnDate.Day > DOB.Day)
				Months = (12-sMonth)+eMonth-1;
			else
				Months = (12-sMonth)+eMonth-2;

			double tDays=0;

			//calculate Days
			if( eMonth != sMonth && OnDate.Day != DOB.Day ) 
			{
				if(OnDate.Day > DOB.Day) 
					tDays = DateTime.DaysInMonth(OnDate.Year, 
						OnDate.Month) - DOB.Day; 
				else
					tDays = DateTime.DaysInMonth(OnDate.Year, 
						OnDate.Month-1) - DOB.Day + OnDate.Day ; 
			}
			string strAge = Years+"/"+Months+"/"+tDays; 
			return strAge;
		}
		#endregion

		#region GetDayOfWeek
		public static DayOfWeek GetDayOfWeek(string strDayOfWeek)
		{
			switch(strDayOfWeek.ToLower().Trim())
			{
				case "sunday": return DayOfWeek.Sunday;
				case "monday": return DayOfWeek.Monday;
				case "tuesday": return DayOfWeek.Tuesday;
				case "wednesday": return DayOfWeek.Wednesday;
				case "thursday": return DayOfWeek.Thursday;
				case "friday": return DayOfWeek.Friday;
				case "saturday": return DayOfWeek.Saturday;
				default: throw new Exception("Invalid DayOfWeek: " + strDayOfWeek); 
			}
		}
		#endregion
		
		#region Date Between
		/// <summary>
		/// Determines whether or not a specified date is between two other dates.
		/// </summary>
		/// <param name="dtSubject">DateTime - The subject date</param>
		/// <param name="dtStart">DateTime - The start date</param>
		/// <param name="dtEnd">DateTime - The end date</param>
		/// <returns>bool</returns>
		public static bool IsBetween(DateTime dtSubject, DateTime dtStart, DateTime dtEnd) 
		{
			return (dtSubject.CompareTo(dtStart) > -1 && dtSubject.CompareTo(dtEnd) < 1);
		}
		#endregion
		
		#region TwoDigitYear
		public static string TwoDigitYear(int intYear) {
			string strYear = intYear.ToString();
			if (strYear.Length == 2) return strYear;
			
			if (strYear.Length != 4) throw new ArgumentException("The year specified is not valid.");
			
			// From here on we can assume we're working with 4 digits.
			return strYear.Substring(2);
		}
		#endregion

        #region MakeMonthArray
        public static IEnumerable<DateTime> MakeMonthArray(DateTime dtStart, DateTime dtEnd)
        {
            return Enumerable.Range(0, (dtEnd.Year - dtStart.Year) * 12 + (dtEnd.Month - dtStart.Month + 1))
                             .Select(m => new DateTime(dtStart.Year, dtStart.Month, 1).AddMonths(m));
        }
        #endregion

        #region Private Functions
        public static int OffsetFromFirstDOW(DateTime p_dateTime ,DayOfWeek p_1stDOW )  
		{
			int _return;
			if (p_dateTime.DayOfWeek < p_1stDOW )
			{
				_return = 7 - (p_1stDOW - p_dateTime.DayOfWeek);
			}
			else 
			{
				_return = p_dateTime.DayOfWeek - p_1stDOW;
			}
			return _return;
		}


		/*   
		' === Nth DayOfWeek of specified Month ===
			' e.g., 1st Monday in Jan 2006 is the 2nd
			'   If asked for a date that doesn't exist, e.g., 5th Wed in Jan 2006, return
			' the last Nth that does exist, e.g. 4th Wed 06.  To find out if 5th DayOfWeek
			' exists, use Is5thDayOfWeek()
		*/
		public static int NthDayOfWeekOfMonth(int intNth  , DayOfWeek objDayOfWeek , DateTime objDate) 
		{

			DateTime _dt = new DateTime(objDate.Year, objDate.Month, 1);
			//'   This is translated code that used VB Namespace.  The Weekday() function 
			//'is 1 based, my non-VB namespace equivalent is zero based.  Thats' why the 
			//'expression has a +1.
			int _off;
			int _return;
			if (objDayOfWeek  < _dt.DayOfWeek)
			{
				_off = 7 - (_dt.DayOfWeek - objDayOfWeek);
				_return = 7 * (intNth - 1) + _off + 1;
			}
			else 
			{
				_off = (_dt.DayOfWeek - objDayOfWeek);
				_return = 7 * (intNth - 1) - _off + 1;
			}

			if(_return > LastDayOfMonth(objDate).Day)
			{
				intNth -= 1;
				_return = NthDayOfWeekOfMonth(intNth, objDayOfWeek, objDate);
			}

			return _return;
		}

		public static int NthDayOfWeekOfMonth(int intNth, DayOfWeek objDayOfWeek , int intYear , int intMonth )
		{
			DateTime _dt = new DateTime(intYear, intMonth, 1);
			//'   This is translated code that used VB Namewpace.  The Weekday() function 
			//'is 1 based, my non-VB namespace equivalent is zero based.  Thats' why the 
			//'expression has a +1.
			int _off;
			int _return;
			if (objDayOfWeek  < _dt.DayOfWeek)
			{
				_off = 7 - (_dt.DayOfWeek - objDayOfWeek);
				_return = 7 * (intNth - 1) + _off + 1;
			}
			else 
			{
				_off = (_dt.DayOfWeek - objDayOfWeek);
				_return = 7 * (intNth - 1) - _off + 1;
			}

			if(_return > LastDayOfMonth(_dt).Day)
			{         
				intNth -= 1;
				_return = NthDayOfWeekOfMonth(intNth, objDayOfWeek, intYear, intMonth);
			}

			return _return;
		}


		public static DateTime LastDayOfMonth(DateTime p_date) 
		{
			int m = p_date.Month;
			int y = p_date.Year;
			int d = DateTime.DaysInMonth(y, m);
			return new DateTime(y, m, d);
		}

		#endregion

	}
}
