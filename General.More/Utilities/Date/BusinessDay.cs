using System;
using General;

namespace General.Utilities.Date
{
	/// <summary>
	/// Class for working with Business Days
	/// </summary>
	public class BusinessDay
	{

		#region Constructors

		/// <summary>
		/// Class for working with Business Days
		/// </summary>
		public BusinessDay()
		{

		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Returns the nearest business day to the given date, or the given value when it is a business day.
		/// </summary>
		public static DateTime Parse(DateTime Input)
		{
			return CheckDate(Input, true);
		}

		/// <summary>
		/// Returns the next business day.
		/// </summary>
		public static DateTime Next()
		{
			return Parse(DateTime.Now.AddDays(1));
		}

		/// <summary>
		/// Returns the next business day.
		/// </summary>
		public static DateTime Next(DateTime Start)
		{
			return Parse(Start.AddDays(1));
		}

        /// <summary>
        /// Returns the previous business day.
        /// </summary>
        public static DateTime Previous()
        {
            return CheckDate(DateTime.Now.AddDays(-1), false);
        }

        /// <summary>
        /// Returns the previous business day.
        /// </summary>
        public static DateTime Previous(DateTime Start)
        {
            return CheckDate(Start.AddDays(-1), false);
        }

		/// <summary>
		/// Returns the nearest business day to the current date, or the current date when it is a business day.
		/// </summary>
		public static DateTime Current()
		{
			return Parse(DateTime.Now);
		}

		/// <summary>
		/// Returns true if the given date is a business day
		/// </summary>
		public static bool Test(DateTime Input)
		{
			return IsBusinessDay(Input);
		}

		/// <summary>
		/// Adds a number of business days and returns a new date
		/// </summary>
		public static DateTime Add(int DaysToAdd)
		{
			return Add(DateTime.Now,DaysToAdd);
		}

		/// <summary>
		/// Adds a number of business days and returns a new date
		/// </summary>
		public static DateTime Add(DateTime Start, int DaysToAdd)
		{
            if (DaysToAdd > 0)
                return Add(Next(Start), DaysToAdd - 1);
            else if (DaysToAdd == 0)
                return Parse(Start);
            else if (DaysToAdd < 0)
                return Add(Previous(Start), DaysToAdd + 1);

            return Start;
		}

		#region CountBusinessDays

		/// <summary>
		/// Calulates Business Days within the given range of days.
		/// Start date and End date inclusive.
		/// </summary>
		/// <param name="StartDate">Datetime object 
		/// containing Starting Date</param>
		/// <param name="EndDate">Datetime object containing 
		/// End Date</param>
		/// <returns></returns>
		public static double CountBusinessDays(DateTime StartDate, DateTime EndDate)
		{
			return CountBusinessDays(StartDate,EndDate,5);
		}


		/// <summary>
		/// Calulates Business Days within the given range of days.
		/// Start date and End date inclusive.
		/// </summary>
		/// <param name="StartDate">Datetime object 
		/// containing Starting Date</param>
		/// <param name="EndDate">Datetime object containing 
		/// End Date</param>
		/// <param name="BusinessDaysPerWeek">integer denoting No of Business 
		/// Day in a week</param>
		/// <returns></returns>
		public static double CountBusinessDays(DateTime StartDate, DateTime EndDate, int BusinessDaysPerWeek)
		{
			double iWeek, iDays, isDays, ieDays;
			//* Find the number of weeks between the dates. Subtract 1 */
			// since we do not want to count the current week. * /
			iWeek =DateTools.DateDiff("ww",StartDate,EndDate)-1 ;
			iDays = iWeek * BusinessDaysPerWeek;
			//
			if( BusinessDaysPerWeek == 5)
			{
				//-- If Saturday, Sunday is holiday
				if ( StartDate.DayOfWeek == DayOfWeek.Saturday ) 
					isDays = 7 -(int) StartDate.DayOfWeek;
				else
					isDays = 7 - (int)StartDate.DayOfWeek - 1;
			} 
			else
			{
				//-- If Sunday is only <st1:place>Holiday</st1:place>
				isDays = 7 - (int)StartDate.DayOfWeek;
			}
			//-- Calculate the days in the last week. These are not included in the
			//-- week calculation. Since we are starting with the end date, we only
			//-- remove the Sunday (datepart=1) from the number of days. If the end
			//-- date is Saturday, correct for this.
			if( BusinessDaysPerWeek == 5)
			{
				if( EndDate.DayOfWeek == DayOfWeek.Saturday ) 
					ieDays = (int)EndDate.DayOfWeek - 2;
				else
					ieDays = (int)EndDate.DayOfWeek - 1;
			}
			else
			{
				ieDays = (int)EndDate.DayOfWeek - 1 ;
			}
			//-- Sum everything together.
			iDays = iDays + isDays + ieDays;
			
			return iDays;
		}
		#endregion

		#endregion

		#region Private Functions
        private static DateTime CheckDate(DateTime Input)
        {
            return CheckDate(Input, true);
        }

		private static DateTime CheckDate(DateTime Input, bool DirectionForward)
		{
            if (!IsBusinessDay(Input))
            {
                if(DirectionForward)
                    return CheckDate(Input.AddDays(1), DirectionForward);
                else
                    return CheckDate(Input.Subtract(new TimeSpan(1, 0, 0, 0)), DirectionForward);
            }
            else
                return Input;
		}

		private static bool IsBusinessDay(DateTime Input)
		{
			if(Input.DayOfWeek == DayOfWeek.Saturday || Input.DayOfWeek == DayOfWeek.Sunday) return false; //Weekend
			if(Holidays.IsIndependenceHoliday(Input)) return false; //Independence Day
			if(Holidays.IsChristmasHoliday(Input)) return false; //Christmas
			if(Holidays.IsLaborDay(Input)) return false; //Labor Day
			if(Holidays.IsMartinLutherKingDay(Input)) return false; //Martin Luther King
			if(Holidays.IsMemorialDay(Input)) return false; //Memorial Day
			if(Holidays.IsNewYearsHoliday(Input)) return false; //New Years
			if(Holidays.IsPresidentsDay(Input)) return false; //Presidents Day
			if(Holidays.IsThanksgiving(Input)) return false; //Thanksgiving
			if(Holidays.IsColumbusDay(Input)) return false; //Columbus Day
			return true;
		}
		#endregion

	}
}
