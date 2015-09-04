using System;
using General;

namespace General.Utilities.Validation
{
	/// <summary>
	/// Summary description for Password.
	/// </summary>
	public class Password
	{

		#region Constructors

		/// <summary>
		/// Summary description for Password.
		/// </summary>
		public Password()
		{

		}

		#endregion

		/// <summary>
		/// This function validates a password string based on the following criteria.
		///	Passwords must be at least 8 characters in length.
		///	Passwords must have at least 3 of the following attributes.
		///		- Lower Case Letters
		///		- Upper Case Letters
		///		- Numbers
		///		- Characters i.e. !@#$%*?	
		/// </summary>
		public static bool Validate(string Pass)
		{
			string pass = Pass;

			int score;
			score = 0;
			System.Array char_array = pass.ToCharArray();

			//ROUND 1
			for (int inc = 0; inc < char_array.GetLength(0); inc++)
			{
				if (char.IsNumber(Convert.ToChar(char_array.GetValue(inc))))
				{
					score++;
					break;
				}
			}
			

			//ROUND 2
			for (int inc = 0; inc < char_array.GetLength(0); inc++)
			{
				if (char.IsLower(Convert.ToChar(char_array.GetValue(inc))))
				{
					score++;
					break;
				}
			}

			//ROUND 3
			for (int inc = 0; inc < char_array.GetLength(0); inc++)
			{
				if (char.IsUpper(Convert.ToChar(char_array.GetValue(inc))))
				{
					score++;
					break;
				}
			}

			//ROUND 4
			string symbol_list;
			symbol_list = "!#$@%^&*?-_()\\|:;<>+.,/";
			for (int inc = 0; inc < char_array.GetLength(0); inc++)
			{
				if (symbol_list.IndexOf(Convert.ToChar(char_array.GetValue(inc))) > 0)
				{
					score++;
					break;
				}
			}

			//DECISION
			if (score >= 3 && pass.Length >= 8)
			{
				return (true);
			}
			else
			{
				return (false);
			}

		}

	}
}
