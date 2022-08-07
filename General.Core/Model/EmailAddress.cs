using System;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using General;

namespace General.Model
{
	/// <summary>
	/// Email Address Class
	/// </summary>
	[Serializable, DataContract]
	public class EmailAddress
	{
		#region Private Variables
		bool _blnValid;
		string _strSource;
		string _strName;
		string _strUser;
		string _strDomain;
		#endregion

		#region Constructors
		/// <summary>
		/// Email Address
		/// </summary>
		public EmailAddress()
		{

		}

		/// <summary>
		/// Email Address
		/// </summary>
		/*
        public EmailAddress(string User, string Domain)
		{
            _strSource = User + "@" + Domain;
			_strUser = User;
			_strDomain = Domain;
			if(_strUser != null && _strUser != "" && _strDomain != null && _strDomain != "")
				_blnValid = true;
			else
				_blnValid = false;


		}
        */
		/// <summary>
		/// Email Address
		/// </summary>
		public EmailAddress(string Email)
		{
			SetEmail(Email);
		}

		/// <summary>
		/// Email Address
		/// </summary>
		public EmailAddress(string Email, string FullName)
		{
			_strName = FullName;
			SetEmail(Email);
		}

		/// <summary>
		/// Email Address
		/// </summary>
		public EmailAddress(object DataCell)
		{
			if (DataCell == null)
			{
				_blnValid = false;
				_strSource = "";
				_strUser = "";
				_strDomain = "";
			}
			else if (Convert.IsDBNull(DataCell))
			{
				_blnValid = false;
				_strSource = "";
				_strUser = "";
				_strDomain = "";
			}
			else
			{
				SetEmail(DataCell.ToString());
			}
		}
		#endregion

		#region Private Functions
		/// <summary>
		/// Overrides the email address and parses the new string
		/// </summary>
		private void SetEmail(string strEmail)
		{
			if (strEmail != null)
			{
				if (strEmail.Contains("(") && strEmail.Contains(")"))
				{
					_strName = strEmail;
					strEmail = StringFunctions.AllBetween(strEmail, "(", ")");
					strEmail = strEmail.Trim();
					_strName = _strName.Replace("(" + strEmail + ")", "");
					_strName = _strName.Trim();
				}

				_blnValid = IsValid(strEmail);
			}
			else
				_blnValid = false;

			if (_blnValid)
			{
				_strSource = strEmail;
				_strUser = strEmail.Substring(0, strEmail.IndexOf("@"));
				_strDomain = strEmail.Substring(strEmail.IndexOf("@")).TrimStart('@');
			}
		}

		/// <summary>
		/// Checks if the email address is valid
		/// </summary>
		/// <param name="strEmail">string - A potential email address</param>
		/// <returns>bool</returns>
		public static bool IsValid(string strEmail)
		{
			if (String.IsNullOrWhiteSpace(strEmail))
				return false;
			string strEmailRegEx = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
				@"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
				@".)+))([a-zA-Z]{2,24}|[0-9]{1,3})(\]?)$";
			Regex regxEmail = new Regex(strEmailRegEx);

			return regxEmail.IsMatch(strEmail);
		}
		#endregion

		#region Output
		/// <summary>
		/// Returns the email address as a string
		/// </summary>
		public override string ToString()
		{
			return _strSource;
		}

		/// <summary>
		/// Returns the email address as a sql object
		/// </summary>
		public object ToSql()
		{
			if (_strSource == null)
				return DBNull.Value;
			else if (_strSource == String.Empty)
				return DBNull.Value;
			else
				return _strSource;
		}

		/// <summary>
		/// Returns the email address as an html link
		/// </summary>
		public string ToLink()
		{
			return "<a href=\"mailto:" + _strSource + "\">" + _strSource + "</a>";
		}

		#endregion

		#region Public Properties
		/// <summary>
		/// Gets or sets the Name linked with this EmailAddress
		/// </summary>
		[DataMember]
		public string Name
		{
			get { return _strName; }
			set { _strName = value; }
		}

		/// <summary>
		/// Returns the validation status of the email address
		/// </summary>
		[DataMember]
		public bool Valid
		{
			get { return _blnValid; }
		}

		/// <summary>
		/// Returns the email address as a string
		/// </summary>
		[DataMember]
		public string Value
		{
			get { return _strSource; }
			set { _strSource = value; SetEmail(value); }
		}

		/// <summary>
		/// Returns the email address as a string
		/// </summary>
		public string GetString
		{
			get { return _strSource; }
		}

		/// <summary>
		/// Returns the email address as a string with the owners name
		/// </summary>
		public string EmailWithName
		{
			get { return GetEmailWithName; }
		}

		/// <summary>
		/// Returns the email address as a string with the owners name
		/// </summary>
		public string GetEmailWithName
		{
			get
			{
				if (!String.IsNullOrWhiteSpace(_strName))
					return _strName + " <" + _strSource + ">";
				else
					return _strSource;
			}
		}

		/// <summary>
		/// Returns the user identifier portion of the email address
		/// </summary>
		public string User
		{
			get { return _strUser; }
		}

		/// <summary>
		/// Returns the doman portion of the email address
		/// </summary>
		public string Domain
		{
			get { return _strDomain; }
		}

		#endregion

		#region Operators
		/// <summary>
		/// Compares two EmailAddress objects
		/// </summary>
		public static bool operator ==(EmailAddress Email1, EmailAddress Email2)
		{
			if ((object)Email1 == null && (object)Email2 != null)
				return false;
			if ((object)Email2 == null && (object)Email1 != null)
				return false;
			if ((object)Email1 == null && (object)Email2 == null)
				return true;
			return (Email1.ToString() == Email2.ToString());
		}

		/// <summary>
		/// Compares two EmailAddress objects
		/// </summary>
		public static bool operator !=(EmailAddress Email1, EmailAddress Email2)
		{
			if ((object)Email1 == null && (object)Email2 != null)
				return true;
			if ((object)Email2 == null && (object)Email1 != null)
				return true;
			if ((object)Email1 == null && (object)Email2 == null)
				return false;
			return (Email1.ToString() != Email2.ToString());
		}

		/// <summary>
		/// Casts an EmailAddress as a string
		/// </summary>
		public static implicit operator string(EmailAddress Email)
		{
			try
			{
				return Email.ToString();
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Casts a string as an EmailAddress
		/// </summary>
		public static implicit operator EmailAddress(string Email)
		{
			return new EmailAddress(Email);
		}

		/// <summary>
		/// Compares two EmailAddress objects
		/// </summary>
		public override bool Equals(object obj)
		{
			return (this == (EmailAddress)obj);
		}


		#endregion

		#region GetHashCode
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion

		#region Static RemoveEmailAddresses (from a string)
		public static string RemoveEmailAddresses(string strInput)
		{
			return RemoveEmailAddresses(strInput, String.Empty);
		}

		public static string RemoveEmailAddresses(string strInput, string strReplacement)
		{
			const string MatchEmailPattern =
			   @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
			   + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
				 + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
			   + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";

			Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			return rx.Replace(strInput, strReplacement);
		}
		#endregion
	}
}
