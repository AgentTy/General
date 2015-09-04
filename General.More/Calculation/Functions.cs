using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Calculation.Functions
{

    #region Function & Constant Dictionary
    public class Dictionary
    {
        #region String Constants
        const string PiSymbol = "π";
        #endregion

        #region Declare Constants
        public static Dictionary<string, object> Constants
        {
            get
            {
                Dictionary<string, object> _objConstants = new Dictionary<string, object>();
                _objConstants.Add(PiSymbol, Math.PI);
                _objConstants.Add("pi", Math.PI);
                _objConstants.Add("e", Math.E);
                return _objConstants;
            }
        }
        #endregion

        #region Declare Functions
        public static Dictionary<string, Functions.IMathFunction> Functions
        {
            get
            {
                Dictionary<string, Functions.IMathFunction> _objFunctions = new Dictionary<string, Functions.IMathFunction>();
                _objFunctions.Add("abs", new Functions.AbsoluteValue());
                _objFunctions.Add("sum", new Functions.Sum());
                _objFunctions.Add("count", new Functions.Count());
                _objFunctions.Add("max", new Functions.Max());
                _objFunctions.Add("min", new Functions.Min());
                _objFunctions.Add("sqrt", new Functions.Sqrt());
                _objFunctions.Add("exp", new Functions.Exponent());
                _objFunctions.Add("power", new Functions.Exponent());
                _objFunctions.Add("pow", new Functions.Exponent());
                _objFunctions.Add("ceiling", new Functions.Ceiling());
                _objFunctions.Add("ceil", new Functions.Ceiling());
                _objFunctions.Add("floor", new Functions.Floor());
                _objFunctions.Add("round", new Functions.Round());
                _objFunctions.Add("remainder", new Functions.Remainder());
                _objFunctions.Add("average", new Functions.Average());
                _objFunctions.Add("avg", new Functions.Average());
                _objFunctions.Add("mean", new Functions.Average());
                _objFunctions.Add("median", new Functions.Median());
                _objFunctions.Add("mode", new Functions.Mode());
                _objFunctions.Add("stdev", new Functions.StandardDeviation());
                _objFunctions.Add("stddev", new Functions.StandardDeviation());
                _objFunctions.Add("standarddeviation", new Functions.StandardDeviation());
                _objFunctions.Add("log10", new Functions.Log10());
                _objFunctions.Add("log", new Functions.Log());
                

                #region Trig Functions
                _objFunctions.Add("asine", new Functions.ASine());
                _objFunctions.Add("asin", new Functions.ASine());
                _objFunctions.Add("acosine", new Functions.ACosine());
                _objFunctions.Add("acos", new Functions.ACosine());
                _objFunctions.Add("atangent", new Functions.ATangent());
                _objFunctions.Add("atan", new Functions.ATangent());

                _objFunctions.Add("sineh", new Functions.SineH());
                _objFunctions.Add("sinh", new Functions.SineH());
                _objFunctions.Add("cosineh", new Functions.CosineH());
                _objFunctions.Add("cosh", new Functions.CosineH());
                _objFunctions.Add("tangenth", new Functions.TangentH());
                _objFunctions.Add("tanh", new Functions.TangentH());

                _objFunctions.Add("sine", new Functions.Sine());
                _objFunctions.Add("sin", new Functions.Sine());
                _objFunctions.Add("cosine", new Functions.Cosine());
                _objFunctions.Add("cos", new Functions.Cosine());
                _objFunctions.Add("tangent", new Functions.Tangent());
                _objFunctions.Add("tan", new Functions.Tangent());
                #endregion

                return _objFunctions;
            }
        }
        #endregion 

    }
    #endregion

    /*
     * All Functions must inherit MathFunction and impliment IMathFunction
     *  - Must have defined minimum parameter count
     *  - Must impliment Solve method
     *  - Must retorn a decimal
    */

    #region IMathFunction
    public interface IMathFunction
    {
        int ParameterCount();
        decimal Solve(params decimal[] arguments);
    }

    public class MathFunction
    {
        public bool Validate(decimal[] arguments, int intParameterCount)
        {
            if (arguments.Length < intParameterCount)
                throw new ArgumentException("Wrong number of parameters specified");
            return true;
        }
    }
    #endregion

    #region Absolute Value
    public class AbsoluteValue : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return Math.Abs(arguments[0]);
        }
    }
    #endregion

    #region Sum
    public class Sum : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            decimal decSum = 0;
            foreach (decimal decValue in arguments)
            {
                decSum += decValue;
            }
            return decSum;
        }
    }
    #endregion

    #region Count
    public class Count : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            return arguments.Length;
        }
    }
    #endregion

    #region Max
    public class Max : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 2;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            decimal decMax = decimal.MinValue;
            foreach (decimal decValue in arguments)
            {
                decMax = Math.Max(decMax, decValue);
            }
            return decMax;
        }
    }
    #endregion

    #region Min
    public class Min : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 2;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            decimal decMin = decimal.MaxValue;
            foreach (decimal decValue in arguments)
            {
                decMin = Math.Min(decMin, decValue);
            }
            return decMin;
        }
    }
    #endregion

    #region Square Root
    public class Sqrt : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            double dblResult = Math.Sqrt((double)arguments[0]);
            if (dblResult.ToString() != double.NaN.ToString())
                return (decimal)dblResult;
            else
                throw new ArithmeticException("Square Root of a Negative Number and Imaginary Numbers are not supported.");
        }
    }
    #endregion

    #region Exponent
    public class Exponent : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 2;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            double dblResult = Math.Pow((double)arguments[0], (double)arguments[1]);
            if (dblResult.ToString() != double.NaN.ToString())
                return (decimal)dblResult;
            else
                throw new ArithmeticException("Not a Number error encountered.");
        }
    }
    #endregion

    #region Ceiling
    public class Ceiling : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return Math.Ceiling(arguments[0]);
        }
    }
    #endregion

    #region Floor
    public class Floor : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return Math.Floor(arguments[0]);
        }
    }
    #endregion

    #region Round
    public class Round : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            if (arguments.Length == 2)
                return Math.Round(arguments[0], (int)arguments[1]);
            else
                return Math.Round(arguments[0], 0);
        }
    }
    #endregion

    #region Remainder
    public class Remainder : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 2;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            long intResult = 0;
            Math.DivRem((long)arguments[0], (long)arguments[1], out intResult);
            return (decimal)intResult;
        }
    }
    #endregion

    #region Mean / Average
    public class Average : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 2;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            decimal decTotal = 0;
            foreach (decimal decValue in arguments)
            {
                decTotal += decValue;
            }
            decimal decAverage = decTotal / arguments.Length;
            return decAverage;
        }
    }
    #endregion

    #region Median
    public class Median : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 2;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());

            //Sort
            Array.Sort(arguments);

            //Solve
            int size = arguments.Length - 1;
            int mid = size / 2;
            decimal median = (size % 2 == 0) ? arguments[mid] :
            (arguments[mid] + arguments[mid + 1]) / 2;

            return median;
        }
    }
    #endregion

    #region Mode
    public class Mode : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 3;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());

            System.Collections.Hashtable h = new System.Collections.Hashtable();
            int count = 0;
            foreach (decimal value in arguments)
            {
                if (h[value] == null)
                    h[value] = 0;

                h[value] = ((int)h[value]) + 1;
                if ((int) h[value] > count)
                    count = (int) h[value];
            }

            int intCurModeCount = 1;
            decimal decCurModeValue = 0;
            foreach (System.Collections.DictionaryEntry e in h)
            {
                if ((int)e.Value > intCurModeCount)
                {
                    decCurModeValue = (decimal)e.Key;
                    intCurModeCount = (int)e.Value;
                }
            }

            return decCurModeValue;
        }
    }
    #endregion

    #region Standard Deviation
    public class StandardDeviation : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 2;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());

            decimal decSum = 0.0M, decSumOfSqrs = 0.0M;
            for (int i = 0; i < arguments.Length; i++)
            {
                decSum += arguments[i];
                decSumOfSqrs += (decimal) Math.Pow((double) arguments[i], 2);
            }
            decimal decTopSum = (arguments.Length * decSumOfSqrs) - ((decimal) Math.Pow((double) decSum, 2));
            decimal decCount = (decimal)arguments.Length;
            return (decimal) Math.Sqrt((double) (decTopSum / (decCount * (decCount - 1)))); 
        }
    }
    #endregion

    #region Log
    public class Log : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return (decimal)Math.Log((double)arguments[0]);
        }
    }
    #endregion

    #region Log10
    public class Log10 : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return (decimal)Math.Log10((double)arguments[0]);
        }
    }
    #endregion

    #region Trig

    #region Sine
    public class Sine : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return (decimal)Math.Sin((double)arguments[0]);
        }
    }
    #endregion

    #region Cosine
    public class Cosine : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return (decimal)Math.Cos((double)arguments[0]);
        }
    }
    #endregion

    #region Tangent
    public class Tangent : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return (decimal)Math.Tan((double)arguments[0]);
        }
    }
    #endregion

    #region ASine
    public class ASine : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return (decimal)Math.Asin((double)arguments[0]);
        }
    }
    #endregion

    #region ACosine
    public class ACosine : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return (decimal)Math.Acos((double)arguments[0]);
        }
    }
    #endregion

    #region ATan
    public class ATangent : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return (decimal)Math.Atan((double)arguments[0]);
        }
    }
    #endregion

    #region SineH
    public class SineH : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return (decimal)Math.Sinh((double)arguments[0]);
        }
    }
    #endregion

    #region CosineH
    public class CosineH : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return (decimal)Math.Cosh((double)arguments[0]);
        }
    }
    #endregion

    #region TangentH
    public class TangentH : MathFunction, IMathFunction
    {
        public int ParameterCount()
        {
            return 1;
        }

        public decimal Solve(params decimal[] arguments)
        {
            Validate(arguments, ParameterCount());
            return (decimal)Math.Tanh((double)arguments[0]);
        }
    }
    #endregion

    #endregion

}
