using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Cryptography
{
    public class Password
    {
        public static string GeneratePassword()
        {
            return GeneratePassword(10);
        }

        public static string GeneratePassword(int intLength)
        {
            return GeneratePassword(intLength, 0);
        }

        public static string GeneratePassword(int intLength, int intNumberOfNonAlphaNumericCharacters)
        {
            int intNonAlphaCount = 0;
            byte[] buffer = new byte[intLength];
            char[] aryPassword = new char[intLength];
            char[] aryPunctuations = "!@@$%$%*()@-+=[{]}#%>|%#?".ToCharArray();

            System.Security.Cryptography.RNGCryptoServiceProvider objCrypto = new System.Security.Cryptography.RNGCryptoServiceProvider();
            objCrypto.GetBytes(buffer);

            for (int i = 0; i < intLength; i++)
            {
                //Convert each byte into its representative character
                int intRndChr = (buffer[i] % 87);
                if (intRndChr < 10)
                    aryPassword[i] = Convert.ToChar(Convert.ToUInt16(48 + intRndChr));
                else if (intRndChr < 36)
                    aryPassword[i] = Convert.ToChar(Convert.ToUInt16((65 + intRndChr) - 10));
                else if (intRndChr < 62)
                    aryPassword[i] = Convert.ToChar(Convert.ToUInt16((97 + intRndChr) - 36));
                else
                {
                    if (intNonAlphaCount >= intNumberOfNonAlphaNumericCharacters)
                    {
                        i--;
                        objCrypto.GetBytes(buffer);
                    }
                    else
                    {
                        aryPassword[i] = aryPunctuations[intRndChr - 62];
                        intNonAlphaCount++;
                    }
                }
            }

            if (intNonAlphaCount < intNumberOfNonAlphaNumericCharacters)
            {
                Random objRandom = new Random();
                for (int i = 0; i < (intNumberOfNonAlphaNumericCharacters - intNonAlphaCount); i++)
                {
                    int intIndex;
                    do
                    {
                        intIndex = objRandom.Next(0, intLength);
                    } while (!Char.IsLetterOrDigit(aryPassword[intIndex]));
                    aryPassword[intIndex] = aryPunctuations[objRandom.Next(0, aryPunctuations.Length)];
                }
            }

            Array.Reverse(aryPassword);
            return new string(aryPassword);
        }
    }
}
