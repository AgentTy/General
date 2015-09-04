using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using General.Data;

namespace General.Calculation
{

    #region Parameters (Variables) Enumeration
    public enum ParameterKey
    {
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }
    #endregion

    public class FormulaParser
    {

        #region String Constants
        const string PiSymbol = "π";
        #endregion

        #region Private Variables

        private Dictionary<ParameterKey, object> _objParameters = new Dictionary<ParameterKey, object>();
        private Dictionary<string, object> _objConstants = new Dictionary<string, object>();
        private Dictionary<string, Functions.IMathFunction> _objFunctions = new Dictionary<string, Functions.IMathFunction>();
        private List<String> OperationOrder = new List<string>();

        private string _strLastFormula;
        private string _strLastFormulaSimplified;

        #endregion

        #region Public Properties
        public Dictionary<ParameterKey, object> Parameters
        {
            get { return _objParameters; }
            set { _objParameters = value; }
        }

        public String LastFormula
        {
            get { return _strLastFormula; }
            set { _strLastFormula = value; }
        }

        public String LastFormulaSimplified
        {
            get { return _strLastFormulaSimplified; }
            set { _strLastFormulaSimplified = value; }
        }
        #endregion

        #region Constructor
        public FormulaParser()
        {

            #region Declare Order of Operations
            OperationOrder.Add("^");
            OperationOrder.Add("/");
            OperationOrder.Add("%");
            OperationOrder.Add("*");
            OperationOrder.Add("-");
            OperationOrder.Add("+");
            #endregion

            _objConstants = Functions.Dictionary.Constants;
            _objFunctions = Functions.Dictionary.Functions;

        }
        #endregion

        #region Validate Formula
        public bool Validate(string Formula)
        {
            if (StringFunctions.Count(Formula.Replace('(', '<'), @"<") != StringFunctions.Count(Formula.Replace(')', '>'), @">"))
                throw new ArithmeticException("Mismatched Parenthesis in Formula: " + Formula);

            if (Formula.StartsWith("*") || Formula.EndsWith("*"))
                throw new ArithmeticException("Malformed Formula: " + Formula);

            if (Formula.StartsWith("/") || Formula.EndsWith("/"))
                throw new ArithmeticException("Malformed Formula: " + Formula);

            if (Formula.StartsWith("%") || Formula.EndsWith("%"))
                throw new ArithmeticException("Malformed Formula: " + Formula);

            return true;
        }
        #endregion

        #region Calculate Formula

        public decimal Calculate(string Formula)
        {
            _strLastFormula = Formula;
            return CalculateSegment(Formula);
        }

        private decimal CalculateSegment(string Formula)
        {

            try
            {
                string[] arr;

                #region Validate Formula
                if (!Validate(Formula))
                    return 0;
                #endregion

                #region Clean Formula

                //Remove whitespace
                Formula = Formula.Replace(" ", "");

                //Check for double variables (AB becomes A*B)
                arr = Formula.Split("/+-*%^0123456789().,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                while (CheckForDoubleVariables(arr))
                {
                    foreach (string s in arr)
                    {
                        if (s.Replace("()", "").Replace(")", "").Length > 1 && !IsReservedWord(s))
                        {

                            Formula = Formula.Replace(s, s.Insert(1, "*"));
                            arr = Formula.Split("/+-*%^0123456789().,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            break;
                        }
                    }
                }

                //Check for a number in front of a variable (5X becomes 5*X)
                arr = Formula.Split("/+-*%^(),".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in arr)
                {
                    int intNumberIndex = s.LastIndexOfAny("0123456789.".ToCharArray());
                    if (intNumberIndex > -1 && intNumberIndex + 1 < s.Length)
                    {
                        Formula = Formula.Replace(s, s.Insert(intNumberIndex + 1, "*"));
                        arr = Formula.Split("/+-*%^()".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    }
                }

                #endregion

                #region Solve Functions in Formula

                // abs(x) becomes {abs[x]}
                #region Find Functions
                arr = Formula.Split("()".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (IsFunction(CleanFunction(arr[i])))
                    {
                        arr[i] = CleanFunction(arr[i]);
                        int intSkip = 0;
                        int intIndexOfFunctionStart = Formula.IndexOf(arr[i] + "(");
                        Formula = StringFunctions.ReplaceOnce(Formula, arr[i] + "(", "{" + arr[i] + "[");

                        for (int i2 = intIndexOfFunctionStart; i2 < Formula.Length; i2++)
                        {
                            if (Formula[i2] == '(')
                                intSkip++;

                            if (Formula[i2] == ')')
                                intSkip--;

                            if (Formula[i2] == ')' && intSkip < 0)
                            {
                                Formula = Formula.Remove(i2, 1);
                                Formula = Formula.Insert(i2, "]}");
                                break;
                            }
                        }
                    }
                }
                #endregion

                // {abs[x]} becomes 345 etc..
                arr = Formula.Split("{}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                while (Formula.Contains("["))
                {
                    foreach (string strSegment in arr)
                    {
                        if (strSegment.Contains("[") && strSegment.Contains("]"))
                        {
                            if (IsFunction(CleanFunction(strSegment)))
                            {
                                Functions.IMathFunction objFunction = GetFunction(CleanFunction(strSegment));
                                Formula = Formula.Replace("{" + strSegment + "}", objFunction.Solve(GetParams(strSegment)).ToString());
                                arr = Formula.Split("{}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            }
                        }
                    }
                }

                #endregion

                #region Insert Constants into Formula
                // Pi becomes 3.14....
                arr = Formula.Split("/+-*%^".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in arr)
                {
                    if (s.Contains("(") && !s.Contains(")"))
                        s.Replace("(", "");
                    if (s.Contains(")") && !s.Contains("("))
                        s.Replace(")", "");

                    foreach (KeyValuePair<string, object> objReservedWord in _objConstants)
                    {
                        if (objReservedWord.Key.Length > 1)
                        {
                            if (s.ToLower() == objReservedWord.Key.ToLower() || s.ToLower() == objReservedWord.Key.ToLower() + "()")
                                Formula = Formula.Replace(s, objReservedWord.Value.ToString());
                        }
                        else
                        {

                            if (s.ToLower() == objReservedWord.Key.ToLower() + "()")
                                Formula = Formula.Replace(s, objReservedWord.Value.ToString());
                            else if (s.ToLower() == objReservedWord.Key.ToLower()
                                && (
                                s == objReservedWord.Key.ToLower()
                                || !Parameters.Keys.Contains((ParameterKey)Enum.Parse(typeof(ParameterKey), s.ToUpper()))
                                )
                                )
                                Formula = Formula.Replace(s, objReservedWord.Value.ToString());
                        }
                    }
                }
                #endregion

                #region Insert Variables into Formula
                // A becomes 2 etc...
                arr = Formula.Split("/+-*%^()[]{}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries); 
                foreach (KeyValuePair<ParameterKey, object> de in _objParameters)
                {
                    foreach (string s in arr)
                    {
                        if (s.ToUpper() != de.Key.ToString() && s.ToUpper().EndsWith(de.Key.ToString()))
                        {
                            if (de.Value.GetType() == typeof(decimal[]))
                            {
                                throw new ArgumentException("Cannot do this! (" + s + ")");
                            }
                            else
                                Formula = Formula.Replace(s, (Convert.ToDecimal(s.Replace(de.Key.ToString(), "")) * SqlConvert.ToDecimal(de.Value)).ToString());
                        }
                    }
                    Formula = Formula.Replace(de.Key.ToString(), de.Value.ToString());
                    Formula = Formula.Replace(de.Key.ToString().ToLower(), de.Value.ToString());
                }

                //Make sure there are no variables left
                arr = Formula.Split("/+-*%^0123456789().".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 0)
                {
                    string strUnassignedVariables = String.Empty;
                    foreach (string s in arr)
                    {
                        strUnassignedVariables += s + ", ";
                    }
                    strUnassignedVariables = StringFunctions.Shave(strUnassignedVariables,2);
                    throw new ArithmeticException("Forumla contains variables that are unassigned: " + strUnassignedVariables);
                }
                #endregion

                #region Calculate Parenthetic portions of Formula
                // (4 * 7) becomes 28 etc...
                while (Formula.LastIndexOf("(") > -1)
                {
                    int lastOpenParenthesisIndex = Formula.LastIndexOf("(");
                    int firstCloseParenthesisIndexAfterLastOpened = Formula.IndexOf(")", lastOpenParenthesisIndex);
                    decimal result = ProcessOperation(Formula.Substring(lastOpenParenthesisIndex + 1, firstCloseParenthesisIndexAfterLastOpened - lastOpenParenthesisIndex - 1));
                    bool AppendAsterix = false;
                    if (lastOpenParenthesisIndex > 0)
                    {
                        if (Formula.Substring(lastOpenParenthesisIndex - 1, 1) != "(" && !OperationOrder.Contains(Formula.Substring(lastOpenParenthesisIndex - 1, 1)))
                        {
                            AppendAsterix = true;
                        }
                    }

                    Formula = Formula.Substring(0, lastOpenParenthesisIndex) + (AppendAsterix ? "*" : "") + result.ToString() + Formula.Substring(firstCloseParenthesisIndexAfterLastOpened + 1);

                }
                #endregion

                _strLastFormulaSimplified = Formula;
                return ProcessOperation(Formula);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (ArithmeticException ex)
            {
                throw ex;
            }
            //catch (Exception ex)
            //{
            //return Calculate(strOriginalFormula);
            //throw new Exception("Error Occured While Calculating. Check Syntax", ex);
            //}
        }

        #region ProcessOperation
        private decimal ProcessOperation(string operation)
        {
            ArrayList arr = new ArrayList();

            #region Clean Operation

            #region Clean up triple negatives
            // ---5 becomes -5
            while (operation.Contains("---"))
                operation = operation.Replace("---", "-");
            #endregion

            #region Clean up +-
            // 4+-6 becomes 4-6
            while (operation.Contains("+-"))
                operation = operation.Replace("+-", "-");
            #endregion

            #endregion

            #region Tokenize Operation
            string s = "";
            for (int i = 0; i < operation.Length; i++)
            {
                string currentCharacter = operation.Substring(i, 1);
                if (OperationOrder.IndexOf(currentCharacter) > -1)
                {
                    if (s != "")
                    {
                        arr.Add(s);
                    }
                    arr.Add(currentCharacter);
                    s = "";
                }
                else
                {
                    s += currentCharacter;
                }
            }
            arr.Add(s);
            s = "";
            #endregion

            #region Solve Operation
            //Split Formula into its seperate operations, then solve with defined Order of Operations
            foreach (string op in OperationOrder)
            {
                while (arr.IndexOf(op) > -1)
                {
                    int operatorIndex = arr.IndexOf(op);

                    #region Get Digit Before Operator
                    decimal digitBeforeOperator = 0;
                    if (operatorIndex > 0)
                        digitBeforeOperator = Convert.ToDecimal(arr[operatorIndex - 1]);
                    else
                        digitBeforeOperator = 0;
                    #endregion

                    #region Get Digit After Operator
                    decimal digitAfterOperator = 0;
                    if (arr[operatorIndex + 1].ToString() == "-")
                    {
                        arr.RemoveAt(operatorIndex + 1);
                        digitAfterOperator = Convert.ToDecimal(arr[operatorIndex + 1]) * -1;
                    }
                    else
                    {
                        digitAfterOperator = Convert.ToDecimal(arr[operatorIndex + 1]);
                    }
                    #endregion

                    #region Do Math
                    arr[operatorIndex] = CalculateByOperator(digitBeforeOperator, digitAfterOperator, op);
                    #endregion

                    #region Clean Up
                    if (operatorIndex > 0)
                    {
                        arr.RemoveAt(operatorIndex - 1);
                        arr.RemoveAt(operatorIndex);
                    }
                    else
                    {
                        arr.RemoveAt(operatorIndex + 1);
                    }
                    #endregion

                }
            }
            #endregion

            return Convert.ToDecimal(arr[0]);
        }
        #endregion

        #region CalculateByOperator
        private decimal CalculateByOperator(decimal number1, decimal number2, string op)
        {
            if (op == "/")
            {
                return number1 / number2;
            }
            else if (op == "*")
            {
                return number1 * number2;
            }
            else if (op == "%")
            {
                return number1 % number2;
            }
            else if (op == "-")
            {
                return number1 - number2;
            }
            else if (op == "+")
            {
                return number1 + number2;
            }
            else if (op == "^")
            {
                return (decimal)Math.Pow((double)number1, (double)number2);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region Helpers

        private decimal[] GetParams(string strParams)
        {
            strParams = General.StringFunctions.AllBetween(strParams, "[", "]");
            string[] aryParamFormulas = strParams.Split(',');
            decimal[] aryParams = new decimal[aryParamFormulas.Length];
            int intIndexOffset = 0;
            for (int i = 0; i < aryParamFormulas.Length; i++)
            {
                if (IsSetOfNumbers(aryParamFormulas[i]))
                {
                    decimal[] arySet = GetSetOfNumbers(aryParamFormulas[i]);
                    decimal[] aryExistingParams = aryParams;
                    aryParams = new decimal[(aryExistingParams.Length - 1) + arySet.Length];
                    aryExistingParams.CopyTo(aryParams, 0);
                    arySet.CopyTo(aryParams, i + intIndexOffset);
                    intIndexOffset = intIndexOffset + arySet.Length - 1;
                }
                else
                {
                    aryParams[i + intIndexOffset] = CalculateSegment(aryParamFormulas[i]);
                }
            }
            return aryParams;
        }

        private bool IsVariable(string strSegment)
        {
            foreach (KeyValuePair<ParameterKey, object> de in _objParameters)
            {
                if (strSegment.ToUpper() == de.Key.ToString())
                    return true;
            }
            return false;
        }

        private object GetVariable(string strSegment)
        {
            foreach (KeyValuePair<ParameterKey, object> de in _objParameters)
            {
                if (strSegment.ToUpper() == de.Key.ToString())
                    return de.Value;
            }
            return null;
        }

        private bool IsSetOfNumbers(string strSegment)
        {
            object o = GetVariable(strSegment);
            if (o == null)
                return false;
            else if (o.GetType() == typeof(decimal[]))
                return true;
            else
                return false;
        }

        private decimal[] GetSetOfNumbers(string strSegment)
        {
            object o = GetVariable(strSegment);
            if (o.GetType() == typeof(decimal[]))
                return (decimal[]) o;
            else
                return null;
        }

        private bool CheckForDoubleVariables(string[] arr)
        {
            foreach (string s in arr)
            {
                if (s.Replace("()", "").Replace(")", "").Length > 1 && !IsReservedWord(s))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsReservedWord(string strPhrase)
        {
            if (_objConstants.ContainsKey(strPhrase.ToLower().Replace("(", "").Replace(")", "")))
                return true;

            if (IsFunction(strPhrase))
                return true;

            return false;
        }

        private bool IsFunction(string strPhrase)
        {
            if (_objFunctions.ContainsKey(strPhrase.Split("(".ToCharArray())[0].ToLower()))
                return true;

            return false;
        }

        private string CleanFunction(string strPhrase)
        {
            string[] aryResult = strPhrase.Split("/+-*%^()[]{}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries); //^?
            if (aryResult.Length > 0)
                return aryResult[0];
            else
                return String.Empty;
        }

        private Functions.IMathFunction GetFunction(string strPhrase)
        {
            return (Functions.IMathFunction)_objFunctions[strPhrase.ToLower().Replace("(", "").Replace(")", "")];
        }

        #endregion

        #endregion

    }

}
