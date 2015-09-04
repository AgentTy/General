using General.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Calculation
{

    public enum ComparisonOperators
    {
        Equal = 0,
        GreaterThanOrEqual = 1,
        LessThanOrEqual = 2,
        GreaterThan = 3,
        LessThan = 4,
        NotEqual = 5
    }

    public class Comparator
    {

        #region Compare
        public static bool Compare(object objValue1, object objValue2, ComparisonOperators enuOperator, Type objInnerType)
        {
            if (objInnerType == typeof(Double))
            {
                return Comparator.NumericComparison(SqlConvert.ToDouble(objValue1), SqlConvert.ToDouble(objValue2), enuOperator);
            }
            else if (objInnerType == typeof(DateTime))
            {
                return Comparator.DateComparison(SqlConvert.ToDateTime(objValue1), SqlConvert.ToDateTime(objValue2), enuOperator);
            }
            else if (objInnerType == typeof(TimeSpan))
            {
                return Comparator.TimeComparison((TimeSpan)objValue1, (TimeSpan)objValue2, enuOperator);
            }
            else
            {
                throw new ArgumentException("Unsupported Type for Comparison");
            }
        }
        #endregion

        #region NumericComparison
        public static bool NumericComparison(double objValue1, double objValue2, ComparisonOperators enuOperator)
        {
            switch (enuOperator)
            {
                case ComparisonOperators.Equal: return objValue1 == objValue2;
                case ComparisonOperators.NotEqual: return objValue1 != objValue2;
                case ComparisonOperators.GreaterThan: return objValue1 > objValue2;
                case ComparisonOperators.GreaterThanOrEqual: return objValue1 >= objValue2;
                case ComparisonOperators.LessThan: return objValue1 < objValue2;
                case ComparisonOperators.LessThanOrEqual: return objValue1 <= objValue2;
                default: return false;
            }
        }
        #endregion

        #region DateComparison
        public static bool DateComparison(DateTime objValue1, DateTime objValue2, ComparisonOperators enuOperator)
        {
            switch (enuOperator)
            {
                case ComparisonOperators.Equal: return objValue1 == objValue2;
                case ComparisonOperators.NotEqual: return objValue1 != objValue2;
                case ComparisonOperators.GreaterThan: return objValue1 > objValue2;
                case ComparisonOperators.GreaterThanOrEqual: return objValue1 >= objValue2;
                case ComparisonOperators.LessThan: return objValue1 < objValue2;
                case ComparisonOperators.LessThanOrEqual: return objValue1 <= objValue2;
                default: return false;
            }
        }
        #endregion

        #region TimeComparison
        public static bool TimeComparison(TimeSpan objValue1, TimeSpan objValue2, ComparisonOperators enuOperator)
        {
            switch (enuOperator)
            {
                case ComparisonOperators.Equal: return objValue1 == objValue2;
                case ComparisonOperators.NotEqual: return objValue1 != objValue2;
                case ComparisonOperators.GreaterThan: return objValue1 > objValue2;
                case ComparisonOperators.GreaterThanOrEqual: return objValue1 >= objValue2;
                case ComparisonOperators.LessThan: return objValue1 < objValue2;
                case ComparisonOperators.LessThanOrEqual: return objValue1 <= objValue2;
                default: return false;
            }
        }
        #endregion

    }
}
