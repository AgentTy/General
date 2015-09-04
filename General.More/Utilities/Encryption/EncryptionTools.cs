using System;
using General.Configuration;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace General.Utilities.Encryption
{
	/// <summary>
	/// RC4 Encryption
	/// </summary>
	public class EncryptionTools
	{
		/// <summary>
		/// RC4 Encryption
		/// </summary>
		public EncryptionTools()
		{ }

        private static byte[] key = { };
        private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };

        public static string DecryptNew(string stringToDecrypt)
        {
            return (DecryptNew(stringToDecrypt, GlobalConfiguration.GlobalSettings["querystring_encrypt_key"]));
        }

        public static string DecryptNew(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,
                  des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool CanDecryptNew(string stringToDecrypt, string sEncryptionKey)
        {
            try
            {
                DecryptNew(stringToDecrypt, sEncryptionKey);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string DecryptNewCatchUnencryptedValue(string stringToDecrypt, string sEncryptionKey)
        {
            if (CanDecryptNew(stringToDecrypt, sEncryptionKey))
                return DecryptNew(stringToDecrypt, sEncryptionKey);
            else
                return stringToDecrypt;
        }

        public static string EncryptNew(string stringToEncrypt)
        {
            return (EncryptNew(stringToEncrypt, GlobalConfiguration.GlobalSettings["querystring_encrypt_key"]));
        }

        public static string EncryptNew(string stringToEncrypt, string SEncryptionKey)
        {
            if (String.IsNullOrEmpty(stringToEncrypt))
                return null;
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,
                  des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        } 

        #region RC4 Encryption
        /// <summary>
		/// Encrypt a string
		/// </summary>
		public static string Encrypt(string Text)
		{
			return(Encrypt(Text,GlobalConfiguration.GlobalSettings["rc4_encrypt_key"]));
		}

		/// <summary>
		/// Encrypt a string
		/// </summary>
		public static string Encrypt(string Text,string Key)
		{
			string result = Text;
			rc4encrypt crypt = new rc4encrypt();
			crypt.password = Key;
			crypt.plaintext = Text;
			result = crypt.EnDeCrypt();
			return(result);
		}

		/// <summary>
		/// Decrypt a string
		/// </summary>
		public static string Decrypt(string Text)
		{
			return(Decrypt(Text,GlobalConfiguration.GlobalSettings["rc4_encrypt_key"]));
		}

		/// <summary>
		/// Decrypt a string
		/// </summary>
		public static string Decrypt(string Text,string Key)
		{
			string result = Text;
			rc4encrypt crypt = new rc4encrypt();
			crypt.password = Key;
			crypt.plaintext = Text;
			result = crypt.EnDeCrypt();
			return(result);
		}

		#region RC4Encrypt Class
		private class rc4encrypt
		{
			private int[] sbox = new int[256];
			private int[] key = new int[256];
			public string plaintext, password;

			private void RC4Initialize(string strPwd)
			{
				// Get the length of the password
				// Instead of Len(), we need to use the Length property
				// of the string
				int intLength = strPwd.Length;


				// Set up our for loop.  In C#, we need to change our syntax.

				// The first argument is the initializer.  Here we declare a
				// as an integer and set it equal to zero.

				// The second argument is expression that is used to test
				// for the loop termination.  Since our arrays have 256
				// elements and are always zero based, we need to loop as long
				// as a is less than or equal to 255.

				// The third argument is an iterator used to increment the
				// value of a by one each time through the loop.  Note that
				// we can use the ++ increment notation instead of a = a + 1
				for (int a = 0; a <= 255; a++)
				{
					// Since we don't have Mid()  in C#, we use the C#
					// equivalent of Mid(), String.Substring, to get a
					// single character from strPwd.  We declare a character
					// variable, ctmp, to hold this value.

					// A couple things to note.  First, the Mod keyword we
					// used in VB need to be replaced with the %
					// operator C# uses.  Next, since the return type of
					// String.Substring is a string, we need to convert it to
					// a char using String.ToCharArray() and specifying that
					// we want the first value in the array, [0].
					char ctmp = (strPwd.Substring((a % intLength),1).ToCharArray()[0]);

					// We now have our character and need to get the ASCII
					// code for it.  C# doesn't have the  VB Asc(), but that
					// doesn't mean we can't use it.  In the beginning of our
					// code, we imported the Microsoft.VisualBasic namespace.
					// This allows us to use many of the native VB functions
					// in C#
                
					// Note that we need to use [] instead of () for our
					// array members.
					
					key[a] = Microsoft.VisualBasic.Strings.Asc(ctmp);
					sbox[a] = a;
				}

				// Declare an integer x and initialize it to zero.
				int x = 0;

				// Again, create a for loop like the one above.  Note that we
				// need to use a different variable since we've already
				// declared a above.
				for (int b = 0; b <= 255; b++)
				{
					x = (x + sbox[b] + key[b]) % 256;
					int tempSwap = sbox[b];
					sbox[b] = sbox[x];
					sbox[x] = tempSwap;
				}
			}

			public string EnDeCrypt()
			{
				int i = 0;
				int j = 0;
				string cipher = "";

				// Call our method to initialize the arrays used here.
				RC4Initialize(password);


				// Set up a for loop.  Again, we use the Length property
				// of our String instead of the Len() function
				for (int a = 1; a <= plaintext.Length; a++)
				{
					// Initialize an integer variable we will use in this loop
					int itmp = 0;

					// Like the RC4Initialize method, we need to use the %
					// in place of Mod
					i = (i + 1) % 256;
					j = (j + sbox[i]) % 256;
					itmp = sbox[i];
					sbox[i] = sbox[j];
					sbox[j] = itmp;

					int k = sbox[(sbox[i] + sbox[j]) % 256];

					// Again, since the return type of String.Substring is a
					// string, we need to convert it to a char using
					// String.ToCharArray() and specifying that we want the
					// first value, [0].
					char ctmp = plaintext.Substring(a - 1, 1).ToCharArray()
						[0];

					// Use Asc() from the Microsoft.VisualBasic namespace
					itmp = Microsoft.VisualBasic.Strings.Asc(ctmp);

					// Here we need to use ^ operator that C# uses for Xor
					int cipherby = itmp ^ k;

					// Use Chr() from the Microsoft.VisualBasic namespace
					cipher += Microsoft.VisualBasic.Strings.Chr(cipherby);
				}

				// Return the value of cipher as the return value of our
				// method
				return cipher;
			}

        }
        #endregion

        #endregion
    }
}
