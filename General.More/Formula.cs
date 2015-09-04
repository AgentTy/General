using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General
{
    public class Formula
    {
        public static bool Validate(string strFormula, string strVariables)
        {
            string strValidChars = strVariables.Replace(",", "");
            strValidChars += "+-*/%^().,";

            Array aryValidChars = strValidChars.ToUpper().ToCharArray();

            foreach (char chrTest in strFormula.ToUpper().ToCharArray())
            {
                bool blnTemp = false;
                if (char.IsLetterOrDigit(chrTest))
                    blnTemp = true;
                else
                {
                    foreach (char chrValid in aryValidChars)
                    {
                        if (chrTest == chrValid)
                            blnTemp = true;
                    }
                }
                if(!blnTemp)
                    return false;
            }

            return true;

        }
    }
}
