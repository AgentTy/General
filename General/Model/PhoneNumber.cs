using System;
using System.Xml;
using System.Text;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using General;
using General.Configuration;
using System.ComponentModel.DataAnnotations.Schema;

namespace General.Model
{
	
	/* ToString(int format) FUNCTION WILL OUTPUT A PHONE NUMBER IN THE FOLLOWING FORMATS
	*		1 - DEFAULT FORMAT (ToUSDialString)
	*			[001-CountryCode-]AreaCode-Number [xExtension]
	*			EXAMPLE: 1-702-222-3333 x64
	*			EXAMPLE 001-41-22-7305989 (A swiss phone number)
	* 
	*		2 - ITU COMPLIANT FORMAT (ToInternationalNumber)
	*			+CountryCode-AreaCode-Number[xExtension]
	*			EXAMPLE: +1-800-555-1212 x365
	*			EXAMPLE: +41-22-7305989
	* 
	*		3 - RAW FORMAT (ToUnformattedNumber)
	*			CountryCodeAreaCodeNumber[xExtension]
	*			EXAMPLE: 18005551212x365
	*			EXAMPLE: 41227305989
	* 
	* ToDialString(PhoneNumber OriginatingPhoneNumber) 
	* ToDialString(string OriginatingCountryCode) 
	* ToDialString(Int64 OriginatingCountryCode)
	*
	* RETURNS A DIALING STRING FROM A POINT OF ORIGINATION
	*/
	
  
	/// <summary>
	/// Phone Number Class - 
	/// Requires country database General.PhoneNumber.xml, which is compiled into this library.
	/// </summary>
    [Serializable, DataContract]
	public class PhoneNumber
    {

        #region Settings
        private string _strDefaultUSAreaCode;
        public string DefaultUSAreaCode
        {
            get { if (!String.IsNullOrEmpty(_strDefaultUSAreaCode)) return _strDefaultUSAreaCode; else return GlobalConfiguration.GlobalSettings["PhoneNumberDefaultUSAreaCode"]; }
        }
        #endregion

        #region PRIVATE VARIABLES
        private string _strSource;
		private long _intCountryCode;
		private long _intAreaCode;
		private long _intNumber;
		private int? _intExtension;
		private string _strCountryName;
		private string _strAreaDescription;
        private bool _blnFlaggedInvalid;
        private ParseArgument _enuParseAs;

        [NonSerialized]
		private static XmlDocument _Countries;
		#endregion
		
		#region CONSTRUCTORS
		/// <summary>
		/// Creates a new phone number		
		/// </summary>
		public PhoneNumber() {
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
			_intExtension  = null;
			_strCountryName = "";
			_strAreaDescription = "";
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
		public PhoneNumber(long CountryCode, long AreaCode, long Number, int? Extension)
		{
			_intCountryCode = CountryCode;
			_intAreaCode = AreaCode;
			_intNumber = Number;
			_intExtension = Extension;
			_strCountryName = "";
			_strAreaDescription = "";
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(long AreaCode, long Number, int? Extension)
		{
			_intCountryCode = 1;
			_intAreaCode = AreaCode;
			_intNumber = Number;
			_intExtension = Extension;
			_strCountryName = "";
			_strAreaDescription = "";
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(long Number, int? Extension, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = Number.ToString();
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
			_intExtension = Extension;
			_strCountryName = "";
			_strAreaDescription = "";
			ParseNumber(Number,ParseArgument.Unknown);
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(long Number, int? Extension, ParseArgument ParseAs, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = Number.ToString();
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
			_intExtension = Extension;
			_strCountryName = "";
			_strAreaDescription = "";
			ParseNumber(Number,ParseAs);
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(object Number, object Extension, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = Number.ToString();
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
			_intExtension = null;
			_strCountryName = "";
			_strAreaDescription = "";

			if(Number == null) _intNumber = -1;
			else if(Convert.IsDBNull(Number)) _intNumber = -1;
			else if(Convert.ToString(Number) == "") _intNumber = -1;
			else
			{
				if(StringFunctions.IsNumeric(Number.ToString()))
					ParseNumber(Convert.ToInt64(Number.ToString()), ParseArgument.Unknown);
				else
					ParseNumber(Number.ToString(), ParseArgument.Unknown);
			}

            int? intExtension;
            if (Extension == null) intExtension = null;
            else if (Convert.IsDBNull(Extension)) intExtension = null;
            else if (Convert.ToString(Extension) == "") intExtension = null;
            else if (StringFunctions.ForceNumeric(Extension.ToString()) == "") intExtension = null;
            else intExtension = Convert.ToInt32(StringFunctions.ForceNumeric(Extension.ToString()));

            if (intExtension.HasValue)
                _intExtension = intExtension;
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(object Number, object Extension, ParseArgument ParseAs, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = Number.ToString();
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
			_intExtension = null;
			_strCountryName = "";
			_strAreaDescription = "";

			if(Number == null) _intNumber = -1;
			else if(Convert.IsDBNull(Number)) _intNumber = -1;
            else if (Convert.ToString(Number) == "") _intNumber = -1;
			else
			{
				if(StringFunctions.IsNumeric(Number.ToString()))
					ParseNumber(Convert.ToInt64(Number.ToString()),ParseAs);
				else
					ParseNumber(Number.ToString(),ParseAs);
			}

            int? intExtension;
            if (Extension == null) intExtension = null;
            else if (Convert.IsDBNull(Extension)) intExtension = null;
            else if (Convert.ToString(Extension) == "") intExtension = null;
            else intExtension = Convert.ToInt32(StringFunctions.ForceNumeric(Extension.ToString()));

            if (intExtension.HasValue)
                _intExtension = intExtension;
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(string Number, string Extension, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = Number.ToString();
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
			_intExtension = null;
			_strCountryName = "";
			_strAreaDescription = "";

			if(Number == null) _intNumber = -1;
			else if(Number == "") _intNumber = -1;
			else
			{
				if(StringFunctions.IsNumeric(Number))
					ParseNumber(Convert.ToInt64(Number),ParseArgument.Unknown);
				else
					ParseNumber(Number,ParseArgument.Unknown);
			}

            int? intExtension;
            if (Extension == null) intExtension = null;
            else if (Extension == "") intExtension = null;
            else intExtension = Convert.ToInt32(StringFunctions.ForceNumeric(Extension));

            if (intExtension.HasValue)
                _intExtension = intExtension;
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(string Number, string Extension, ParseArgument ParseAs, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = Number.ToString();
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
			_intExtension = null;
			_strCountryName = "";
			_strAreaDescription = "";

			if(Number == null) _intNumber = -1;
			else if(Number == "") _intNumber = -1;
			else
			{
				if(StringFunctions.IsNumeric(Number))
					ParseNumber(Convert.ToInt64(Number),ParseAs);
				else
					ParseNumber(Number,ParseAs);
			}

            int? intExtension;
            if (Extension == null) intExtension = null;
            else if (Extension == "") intExtension = null;
            else intExtension = Convert.ToInt32(StringFunctions.ForceNumeric(Extension));

            if (intExtension.HasValue)
                _intExtension = intExtension;
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(Int64 Number, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = Number.ToString();
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
			_intExtension = null;
			_strCountryName = "";
			_strAreaDescription = "";
			ParseNumber(Number,ParseArgument.Unknown);
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(Int64 Number, ParseArgument ParseAs, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = Number.ToString();
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
            _intExtension = null;
			_strCountryName = "";
			_strAreaDescription = "";
			ParseNumber(Number,ParseAs);
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(string Number, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = Number;
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
            _intExtension = null;
			_strCountryName = "";
			_strAreaDescription = "";
			if(StringFunctions.IsNumeric(_strSource))
				ParseNumber(Convert.ToInt64(_strSource),ParseArgument.Unknown);
            else if (_strSource != null)
				ParseNumber(_strSource,ParseArgument.Unknown);
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(string Number, ParseArgument ParseAs, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = Number;
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
            _intExtension = null;
			_strCountryName = "";
			_strAreaDescription = "";
			if(StringFunctions.IsNumeric(_strSource))
				ParseNumber(Convert.ToInt64(_strSource),ParseAs);
            else if (_strSource != null)
				ParseNumber(_strSource,ParseAs);
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(object DataCell, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = "";
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
            _intExtension = null;
			_strCountryName = "";
			_strAreaDescription = "";
			if(DataCell != null)
			{
				if(!Convert.IsDBNull(DataCell))
				{
					_strSource = DataCell.ToString();
					if(StringFunctions.IsNumeric(_strSource))
						ParseNumber(Convert.ToInt64(_strSource),ParseArgument.Unknown);
					else
						ParseNumber(_strSource,ParseArgument.Unknown);
				}
			}
		}

		/// <summary>
		/// Creates a new phone number		
		/// </summary>
        public PhoneNumber(object DataCell, ParseArgument ParseAs, string DefaultUSAreaCode = null)
		{
            if (!StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                _strDefaultUSAreaCode = DefaultUSAreaCode;
			_strSource = "";
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
            _intExtension = null;
			_strCountryName = "";
			_strAreaDescription = "";
			if(DataCell != null)
			{
				if(!Convert.IsDBNull(DataCell))
				{
					_strSource = DataCell.ToString();
					if(StringFunctions.IsNumeric(_strSource))
						ParseNumber(Convert.ToInt64(_strSource),ParseAs);
					else
						ParseNumber(_strSource,ParseAs);
				}
			}
		}

		#endregion

		#region NUMBER PARSING

		private void ParseNumber(Int64 Number, ParseArgument ParseAs)
		{
            _blnFlaggedInvalid = false;
            _enuParseAs = ParseAs;

			switch(ParseAs)
			{
				case ParseArgument.NANP:
					ParseNANPNumber(Number);
					break;
				default:
					ParseNumber(Number);
					break;
			}
		}

		private void ParseNumber(string Number, ParseArgument ParseAs)
		{
            _blnFlaggedInvalid = false;
            _enuParseAs = ParseAs;

			switch(ParseAs)
			{
				case ParseArgument.NANP:
                    if (StringFunctions.ForceInteger(Number).StartsWith("0"))
                        _blnFlaggedInvalid = true; //NANP numbers and dial strings don't start with zero
					ParseNANPNumber(CleanNumber(Number));
					break;
				default:
					ParseNumber(CleanNumber(Number));
					break;
			}
		}

		/// <summary>
		/// Takes in a phone number and updates the PhoneNumber object	
		/// </summary>
		private void ParseNANPNumber(Int64 intNumber)
		{
			if(intNumber != -1)
			{
				string strTemp = intNumber.ToString();

                //HERE WE WILL CATCH ANY US/CANADIAN PHONE NUMBERS MISSING A COUNTRY CODE
                if (strTemp.Length == 7 && !StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                {
                    strTemp = DefaultUSAreaCode + strTemp;
                }

				if(strTemp.Length == 10 || (strTemp.Length == 11 && StringFunctions.StartsWith(strTemp,"1")))
				{
					if(strTemp.Length == 11)
						strTemp = StringFunctions.Right(strTemp, 10);
					_intCountryCode = 1;
					_intAreaCode = Convert.ToInt64(strTemp.Substring(0, 3));
                    if (_intAreaCode.ToString() != strTemp.Substring(0, 3))
                        _blnFlaggedInvalid = true; //Flaged because the area code starts with a zero
					_intNumber = Convert.ToInt64(strTemp.Substring(3, 7));
                    if (_intNumber.ToString() != strTemp.Substring(3, 7))
                        _blnFlaggedInvalid = true; //Flaged because the area code starts with a zero
				}
                else if(strTemp.Length == 7)
				{
					_intCountryCode = 1;
                    _intAreaCode = -1;
                    _intNumber = intNumber;
				}
				else
				{
					ParseNumber(intNumber);
				}
			}
		}
		/// <summary>
		/// Takes in a phone number and updates the PhoneNumber object	
		/// </summary>
		private void ParseNumber(Int64 intNumber)
		{
			_intCountryCode = -1;
			_intAreaCode = -1;
			_intNumber = -1;
			_strCountryName = "";
			_strAreaDescription = "";

			if(intNumber != -1)
			{
				string strTemp = intNumber.ToString();
				XmlDocument Countries = GetCountryDatabase();
				XmlNodeList nl = Countries.DocumentElement.GetElementsByTagName("Country");
				XmlNode Country = nl.Item(0);

                //HERE WE WILL CATCH ANY US/CANADIAN PHONE NUMBERS MISSING A COUNTRY CODE
                if (strTemp.Length == 7 && !StringFunctions.IsNullOrWhiteSpace(DefaultUSAreaCode))
                {
                    string strDefaultUSAreaCode = DefaultUSAreaCode;
                    strTemp = strDefaultUSAreaCode + strTemp;
                }

				//HERE WE WILL CATCH ANY US/CANADIAN PHONE NUMBERS MISSING A COUNTRY CODE
				if(strTemp.Length == 10) 
				{
					//UNITED STATES
					foreach(XmlNode n in nl)
					{
						if(n.Attributes["ID"].Value == "US")
						{
							Country = n;
							break;
						}
					}
					foreach(XmlNode n in Country.ChildNodes)
					{
						if(StringFunctions.StartsWith(strTemp,n.Attributes["Code"].Value))
						{					
							strTemp = "1" + strTemp;
							ParseNumber(Convert.ToInt64(strTemp));
							return;
						}
					}

					//CANADA
					foreach(XmlNode n in nl)
					{
						if(n.Attributes["ID"].Value == "CA")
						{
							Country = n;
							break;
						}
					}
					foreach(XmlNode n in Country.ChildNodes)
					{
						if(StringFunctions.StartsWith(strTemp,n.Attributes["Code"].Value))
						{					
							strTemp = "1" + strTemp;
							ParseNumber(Convert.ToInt64(strTemp));
							return;
						}
					}

				}
				//DONE WITH CATCH

				//SCAN THE COUNTRY CODE DATABASE FOR A MATCH
				foreach(XmlNode n in nl)
				{
					if(StringFunctions.StartsWith(strTemp,n.Attributes["CountryCode"].Value) && n.Attributes["CountryCode"].Value != "")
					{
						_intCountryCode = Convert.ToInt64(n.Attributes["CountryCode"].Value);
						_strCountryName = n.Attributes["Name"].Value;
						if(ParseNumber(n,StringFunctions.Right(strTemp,strTemp.Length - n.Attributes["CountryCode"].Value.Length)))
							break;
					}
					else if(StringFunctions.StartsWith(strTemp,n.Attributes["CountryCode2"].Value) && n.Attributes["CountryCode2"].Value != "")
					{
						_intCountryCode = Convert.ToInt64(n.Attributes["CountryCode2"].Value);
						_strCountryName = n.Attributes["Name"].Value;
						if(ParseNumber(n,StringFunctions.Right(strTemp,strTemp.Length - n.Attributes["CountryCode2"].Value.Length)))
							break;
					}
					else
					{
					}
				}
				//DONE WITH SCAN
			
				if(_intCountryCode == -1)
				{
					_intNumber = Convert.ToInt64(strTemp);
					_strCountryName = "Unknown";
					_strAreaDescription = "Unknown";
				}
				else if(_intAreaCode == -1)
				{
					if(strTemp.Length <= _intCountryCode.ToString().Length)
						_intNumber = -1;
					else
						_intNumber = Convert.ToInt64(StringFunctions.Right(strTemp,strTemp.Length - _intCountryCode.ToString().Length));
					_strAreaDescription = "Unknown";
				}
			}
		}

		private bool ParseNumber(XmlNode Country, string Number)
		{
			string strTemp = Number;
			bool result = false;

			if(Country.ChildNodes.Count > 0)
			{
				foreach(XmlNode n in Country.ChildNodes)
				{
					if(StringFunctions.StartsWith(strTemp,n.Attributes["Code"].Value))
					{					
						_intAreaCode = Convert.ToInt64(n.Attributes["Code"].Value);
						_strAreaDescription = n.Attributes["Description"].Value;
						strTemp = StringFunctions.Right(strTemp,strTemp.Length - n.Attributes["Code"].Value.Length);
                        try
                        {
                            _intNumber = Convert.ToInt64(strTemp);
                            result = true;
                        }
                        catch
                        {
                            result = false;
                        }
						break;
					}
					else if(Country.Attributes["NDD"] != null)
					{
						if(StringFunctions.StartsWith(strTemp,Country.Attributes["NDD"].Value + n.Attributes["Code"].Value))
						{
							_intAreaCode = Convert.ToInt64(n.Attributes["Code"].Value);
							_strAreaDescription = n.Attributes["Description"].Value;
							strTemp = StringFunctions.Right(strTemp,strTemp.Length - _intAreaCode.ToString().Length);
                            try
                            {
                                _intNumber = Convert.ToInt64(strTemp);
                                result = true;
                            }
                            catch
                            {
                                result = false;
                            }
							break;
						}
					}
				}
			}
			else
			{
                try
                {
                    _intNumber = Convert.ToInt64(Number);
                    result = true;
                }
                catch
                {
                    result = false;
                }
				_strAreaDescription = "Unknown";
				result = true;
			}
			return(result);
		}

		private Int64 CleanNumber(string strNumber)
		{
			if (StringFunctions.IsNumeric(strNumber))
			{
				return Convert.ToInt64(strNumber);
			}
			else
			{
				string strTemp;
				string strPreExt;
				Int64 intTempNumber;
				string strExt;
				strTemp = strNumber.ToLower();
				strTemp = strTemp.Trim();
                strTemp = strTemp.Replace("(0)", ""); //Remove alternate dial instruction used in international numbers
				strTemp = strTemp.Replace(" ","|");
				strTemp = strTemp.Replace("(","|");
				strTemp = strTemp.Replace(")","|");
				strTemp = strTemp.Replace("-","|");
				strTemp = strTemp.Replace(".","|");
				while(StringFunctions.Contains(strTemp,"||"))
					strTemp = strTemp.Replace("||","|");
				strTemp = strTemp.Trim('|');
                if (StringFunctions.ContainsAfterDigits(strTemp, "extension"))
				    strTemp = strTemp.Replace("extension","^");
                if (StringFunctions.ContainsAfterDigits(strTemp, "ext"))
				    strTemp = strTemp.Replace("ext","^");
                strTemp = strTemp.Replace("*", "^");
                strTemp = strTemp.Replace("#", "^");
                while (StringFunctions.Contains(strTemp, "~^"))
                    strTemp = strTemp.Replace("~^", "^");
                while (StringFunctions.Contains(strTemp, "^~"))
                    strTemp = strTemp.Replace("^~", "^");
                //Pause Here to see if there are letters in the Phone Number
                if (StringFunctions.CountAlphaLetters(StringFunctions.AllAfter(strTemp,"|")) >= 3)
                {
                    int intLimit = 0;
                    if (StringFunctions.Count(strTemp, "|") >= 2) 
                        intLimit = 4;
                    else
                        intLimit = 7;

                    if (StringFunctions.CountAlphaLetters(StringFunctions.AllAfterReverse(strTemp, "|")) > intLimit) //If there are extra letters that you don't dial
                    {
                        string strLettersBefore;
                        string strLettersAfter;
                        if (StringFunctions.Contains(strTemp, "^")) //Has Extension
                            strLettersBefore = StringFunctions.AllBetween(strTemp, "|", "^");
                        else
                            strLettersBefore = StringFunctions.AllAfterReverse(strTemp, "|");

                        strLettersAfter = strLettersBefore.Substring(0, intLimit);
                        strTemp = strTemp.Replace(strLettersBefore, strLettersAfter);
                        strTemp = ConvertLettersToNumbers(strTemp);
                    }
                    else
                        strTemp = ConvertLettersToNumbers(strTemp);

                }
                else
                {
                    if (StringFunctions.ContainsAfterDigits(strTemp, "xt"))
                        strTemp = strTemp.Replace("xt", "^");
                    if (StringFunctions.ContainsAfterDigits(strTemp, "x"))
                        strTemp = strTemp.Replace("x", "^");
                    while (StringFunctions.Contains(strTemp, "~^"))
                        strTemp = strTemp.Replace("~^", "^");
                    while (StringFunctions.Contains(strTemp, "^~"))
                        strTemp = strTemp.Replace("^~", "^");
                }
				
				
				if (StringFunctions.Contains(strTemp,"^"))
				{
					strPreExt = StringFunctions.Left(strTemp,strTemp.IndexOf("^"));
					strExt = StringFunctions.AllAfter(strTemp,strTemp.IndexOf("^"));
					strPreExt = StringFunctions.ForceInteger(strPreExt);
					intTempNumber = Convert.ToInt64(strPreExt);
					strExt = StringFunctions.ForceInteger(strExt);
					_intExtension = Convert.ToInt32(strExt);
					strTemp = strPreExt;
					return intTempNumber;
				}
				else
				{	
					if(StringFunctions.ForceInteger(strTemp) != "")
					{
						intTempNumber = Convert.ToInt64(StringFunctions.ForceInteger(strTemp));
                        //if (intTempNumber.ToString() != StringFunctions.ForceInteger(strTemp))  //Leading zero in the phone number
                            //_blnFlaggedInvalid = true;
						return intTempNumber;
					}
					else
					{
						_intCountryCode = -1;
						_intAreaCode = -1;
						_intNumber = -1;
						_intExtension  = null;
						return -1;
					}
				}
			}
		}
		
        
		#endregion

		#region OUTPUT

        #region Enum
        /// <summary>
        /// Returns phone number selectable format
        /// 1 = Default Output
        /// 2 = ToInternationalDialString
        /// 3 = ToUSDialString
        /// 4 = ToUnformattedNumber	
        /// </summary>
        public enum Format
        {
            Default = 1,
            InternationalDialString = 2,
            USDialString = 3,
            UnformattedNumber = 4,
            CustomFormat = 5,
            Debug = 6
        }
        #endregion

		/// <summary>
		/// Returns phone number in default output format with optional Extension
		/// </summary>
		public string ToString(bool AddExtension) {
			return ToDefaultOutput(AddExtension);
		}


		/// <summary>
		/// Returns phone number in default output format appending any Extension to the end of the string
		/// </summary>
		public override string ToString() {
			return ToDefaultOutput(true);
		}

       

		/// <summary>
		/// Returns phone number selectable format
        /// 1 = Default Output
        /// 2 = ToInternationalDialString
        /// 3 = ToUSDialString
        /// 4 = ToUnformattedNumber	
		/// </summary>
        public string ToString(Format enuFormat)
        {
			StringBuilder sbPhone = new StringBuilder();
            switch (enuFormat)
            {
				case Format.Default:
					sbPhone.Append(ToDefaultOutput(true));
				break;
				case Format.InternationalDialString:
					sbPhone.Append(ToInternationalDialString());
				break;
                case Format.USDialString:
                sbPhone.Append(ToUSDialString());
                break;
				case Format.UnformattedNumber:
					sbPhone.Append(ToUnformattedNumber());
				break;
				case Format.Debug:
					sbPhone.Append("Country=" + _intCountryCode.ToString() + " ");
					sbPhone.Append("Area=" + _intAreaCode.ToString() + " ");
					sbPhone.Append("Number=" + _intNumber.ToString() + " ");
                    if (_intExtension.HasValue)
					    sbPhone.Append("Extension=" + _intExtension.Value.ToString() + " ");
					sbPhone.Append("Country Name=" + _strCountryName + " ");
					sbPhone.Append("Area Description=" + _strAreaDescription + " ");
				break;
			}
			return sbPhone.ToString();
		}

		private string ToDefaultOutput()
		{
			return ToDefaultOutput(true);
		}

		private string ToDefaultOutput(bool AddExtension)
		{
			if(_intCountryCode == 1)
			{
				return ToShortUSString(AddExtension);
			}
			else
			{
				return ToInternationalDialString(AddExtension);
			}
		}

        /// <summary>
        /// Returns phone number in simple format familiar to US citizens
        /// EX: (702) 222-3456
        /// </summary>
		private string ToShortUSString(bool AddExtension)
		{
			StringBuilder sb = new StringBuilder();
			if(_intNumber <= 0)
			{
                if(EnableNoneText)
				    sb.Append("None"); //INVALID PHONE NUMBER
			}
			else
			{
				if (_intAreaCode != -1)
				{
					sb.Append("(");
					sb.Append(_intAreaCode.ToString());
					sb.Append(") ");
				}
				if (_intNumber.ToString().Length == 7)
				{
					sb.Append(_intNumber.ToString().Insert(3,"-"));
				}
				else
				{
					sb.Append(_intNumber.ToString());
				}
				if (_intExtension.HasValue && _intExtension.Value >= 0 && AddExtension)
				{
					sb.Append(" x");
                    sb.Append(_intExtension.Value.ToString());
				}
			}
			return(sb.ToString());
		}

		/// <summary>
		/// Returns phone number in a raw format
		/// EX: 14802223456x365
		/// </summary>
		public string ToUnformattedNumber()
		{
			StringBuilder sb = new StringBuilder();
			
			if (_intCountryCode != -1) sb.Append(_intCountryCode.ToString());
			
			if (_intAreaCode != -1) sb.Append(_intAreaCode.ToString());
			sb.Append(_intNumber.ToString());

            if (_intExtension.HasValue && _intExtension >= 0) sb.Append("x" + _intExtension.Value.ToString() + "");
			
			return sb.ToString();
		}

		/// <summary>
		/// Returns phone number as a long integer excluding the extension (SQL Friendly)
		/// EX: 14802223456
		/// </summary>
		public long ToLong()
		{
			return ToInt64();
		}

		/// <summary>
		/// Returns phone number as a 64 bit integer excluding the extension (SQL Friendly)
		/// EX: 14802223456
		/// A null phone number returns -1
		/// </summary>
		public long ToInt64()
		{
			string result = "";
			if(_intNumber <= 0)
			{
                return -1; //INVALID PHONE NUMBER
			}
			else
			{
				if (_intCountryCode != -1)
				{result += _intCountryCode.ToString();}
				if (_intAreaCode != -1)
				{result += _intAreaCode.ToString();}
				if(_intNumber != -1)
				{result += _intNumber.ToString();}
			}
			return(Convert.ToInt64(result));
		}

        /// <summary>
        /// Returns phone number as a sql big int excluding the extension
        /// EX: 14802223456
        /// A null phone number returns DBNull
        /// </summary>
        public object ToEFSQL()
        {
            return ToUnformattedNumber();
        }

        /// <summary>
        /// Returns phone number as a sql big int excluding the extension
        /// EX: 14802223456
        /// A null phone number returns DBNull
        /// </summary>
        public object ToSql()
		{
			return ToSql("all");
		}

		/// <summary>
		/// Returns a portion of a phone number as a sql big int
		/// EX: 14802223456
		/// A null phone number returns DBNull
		/// </summary>
		/// <param name="Element">
		/// "all" returns the main portion of the phone number
		/// "extension" returns the extension of the phone number
		/// </param>
		public object ToSql(string Element)
		{
			switch(Element.ToLower())
			{
				case "all":
					if(ToInt64() == -1)
						return DBNull.Value;
					else
						return ToInt64();
				case "extension":
					if(!_intExtension.HasValue || _intExtension.Value <0)
						return DBNull.Value;
					else
						return _intExtension.Value;
				default:
					return ToSql("all");
			}
		}


		/// <summary>
		/// Returns US originating dial string
		/// EX: 1-480-222-3456 x365
		/// EX: 001-41-22-7305989
		/// </summary>
		public string ToUSDialString()
		{
			return(ToDialString("1"));
		}

		/// <summary>
		/// Returns a dial string from a point of origination
		/// </summary>
		public string ToDialString(PhoneNumber OriginatingPhoneNumber)
		{
			return(ToDialString(OriginatingPhoneNumber.CountryCode.ToString()));
		}

		/// <summary>
		/// Returns a dial string from a point of origination
		/// </summary>
		public string ToDialString(string OriginatingCountryCode)
		{
			if(StringFunctions.IsNumeric(OriginatingCountryCode))
				return(ToDialString(Convert.ToInt64(OriginatingCountryCode)));
			else
				return(ToDialString(GetPhoneCountryCodeFromISOCountryCode(OriginatingCountryCode)));
		}

		/// <summary>
		/// Returns a dial string from a point of origination
		/// </summary>
		public string ToDialString(Int64 OriginatingCountryCode)
		{
			string strNDD;
			string strIDD;
			XmlDocument Countries = GetCountryDatabase();
			XmlNodeList nl = Countries.DocumentElement.GetElementsByTagName("Country");
			XmlNode Country = nl.Item(0);
			//SCAN THE COUNTRY CODE DATABASE FOR A MATCH
			//Countries.SelectSingleNode("./Countries/Country[@CountryCode=1]");
			foreach(XmlNode n in nl)
			{
				if(n.Attributes["CountryCode"].Value == OriginatingCountryCode.ToString())
				{
					strIDD = n.Attributes["IDD"].Value;
					strNDD = n.Attributes["NDD"].Value;
					return(ToDialString(OriginatingCountryCode,strIDD,strNDD));
				}
				else if(n.Attributes["CountryCode2"].Value == OriginatingCountryCode.ToString())
				{
					strIDD = n.Attributes["IDD"].Value;
					strNDD = n.Attributes["NDD"].Value;
					return(ToDialString(OriginatingCountryCode,strIDD,strNDD));
				}
			}
			//DONE WITH SCAN

			return(ToInternationalDialString());
		}

		private string ToDialString(Int64 OriginatingCountryCode,string IDD, string NDD)
		{
            if (String.IsNullOrEmpty(IDD))
                IDD = "00"; //Most common IDD, use this when XML file doesn't specify

			StringBuilder sb = new StringBuilder();
			if(_intNumber <= 0)
			{
                if (EnableNoneText)
				    sb.Append("None"); //INVALID PHONE NUMBER
			}
			else
			{
				string[] aryIDD = IDD.Split(',');
				if(_intCountryCode == OriginatingCountryCode)
				{
					//LOCAL CALL
					if (NDD != "")
					{
						sb.Append(NDD);
						sb.Append("-");
					}
				}
				else
				{
					//INTERNATIONAL CALL
					if (aryIDD.Length > 0)
					{
						sb.Append(aryIDD[0]);
						sb.Append("-");
					}
					if (_intCountryCode != -1)
					{
						sb.Append(_intCountryCode.ToString());
						sb.Append("-");
					}
				}

				if (_intAreaCode != -1)
				{
					sb.Append(_intAreaCode.ToString());
					sb.Append("-");
				}
				if (_intNumber.ToString().Length == 7)
				{
					sb.Append(_intNumber.ToString().Insert(3,"-"));
				}
				else
				{
					sb.Append(_intNumber.ToString());
				}
                if (_intExtension.HasValue && _intExtension >= 0)
				{
					sb.Append(" x");
					sb.Append(_intExtension.Value.ToString());
				}
			}
			return(sb.ToString());
		}

		/// <summary>
		/// Returns a dial string in a standard international format appending any extension to the end of the string.
		/// EX: +41-22-334325343x4544
		/// </summary>
		public string ToInternationalDialString()
		{
			return ToInternationalDialString(true);
		}

		/// <summary>
		/// Returns a dial string in a standard international format with optional extension.
		/// EX: +41-22-334325343x4544
		/// </summary>
		public string ToInternationalDialString(bool AddExtension)
		{
			StringBuilder sb = new StringBuilder();
			if(_intNumber <= 0)
			{
                if (EnableNoneText)
				    sb.Append("None"); //INVALID PHONE NUMBER
			}
			else
			{
				if (_intCountryCode != -1)
				{
					sb.Append("+");
					sb.Append(_intCountryCode.ToString());
					sb.Append("-");
				}
				if (_intAreaCode != -1)
				{
					sb.Append(_intAreaCode.ToString());
					sb.Append("-");
				}
				if (_intNumber.ToString().Length == 7)
				{
					sb.Append(_intNumber.ToString().Insert(3,"-"));
				}
				else
				{
					sb.Append(_intNumber.ToString());
				}
                if (_intExtension.HasValue && _intExtension.Value > 0 && AddExtension)
				{
					sb.Append(" x");
                    sb.Append(_intExtension.Value.ToString());
				}
			}
			return(sb.ToString());
		}
		#endregion

		#region DATA
		/// <summary>
        /// Returns the General.PhoneNumber.xml document
		/// </summary>
		public static XmlDocument GetCountryDatabase()
		{
			
			if (_Countries != null)
			{
				return(_Countries); //GET STATIC OBJECT
			}
			else
            {
                #region First Try Loading From File
                try
                {
                    string strPath = General.Environment.Current.GetBinDirectory() + "Resources\\General.PhoneNumber.xml";
                    if (!System.IO.File.Exists(strPath))
                        strPath = General.Environment.Current.GetBinDirectory() + "General.PhoneNumber.xml";

                    if (System.IO.File.Exists(strPath))
                    {
                        XmlDocument doc;
                        doc = new XmlDocument();
                        doc.Load(strPath);
                        _Countries = doc; //SET STATIC OBJECT
                        return _Countries;
                    }
                }
                catch { }
                #endregion

                #region Next Try Loading From DLL Resources
                if (_Countries == null)
                {
                    try
                    {
                        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                        var resourceName = "General.Resources.General.PhoneNumber.xml";

                        using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceName))
                        {
                            if(stream != null)
                            { 
                                XmlDocument doc = new XmlDocument();
                                doc.Load(stream);
                                _Countries = doc; //SET STATIC OBJECT
                                return _Countries;
                            }
                        }
                    }
                    catch { }
                }
                #endregion

                #region Finally Use Default Value
                if (_Countries == null)
                { 
                    _Countries = new XmlDocument();
                    string strDefaultXML = @"<?xml version=""1.0"" encoding=""utf-8"" ?>  
                            <General.PhoneNumber>
                                <Country Name=""UNITED STATES OF AMERICA"" ID=""US"" CountryCode=""1"" CountryCode2="""" IDD=""011"" NDD=""1"" PhonePattern=""CAAANNNNNNN""></Country>
                            </General.PhoneNumber>
                            ";
                    _Countries.LoadXml(strDefaultXML);
                    return _Countries;
                }
                #endregion

                return _Countries;
            }
		}
        #endregion

        #region PUBLIC PROPERTIES
        /// <summary>
        /// Returns the default output string
        /// </summary>
        [DataMember]
        public string Value
        {
            get { return ToString(); }
            set
            {
                if (value != null)
                {
                    if (StringFunctions.IsNumeric(value))
                        ParseNumber(Convert.ToInt64(value), ParseArgument.Unknown);
                    else
                        ParseNumber(value, ParseArgument.Unknown);
                }
            }
        }


        /// <summary>
        /// Returns the validation status of the PhoneNumber	
        /// </summary>
        [DataMember]
        public bool Valid
		{
            get { 
                if (_blnFlaggedInvalid) 
                    return false; 
                return ToString(PhoneNumber.Format.UnformattedNumber).Length >= 7 && (!Extension.HasValue || Extension.Value <= 32767); 
            }
		}

        private int _intPropertiesLeftToPopulateObject = 2;
		/// <summary>
		/// Returns the original data use to create the PhoneNumber
		/// </summary>
		public string Source
		{
			get	{ return _strSource; }
		}

        /// <summary>
        /// Returns the original data use to create the PhoneNumber
        /// </summary>
        [DataMember, NotMapped]
        public ParseArgument ParseAs
        {
            get { return _enuParseAs; }
            set { _enuParseAs = value; _intPropertiesLeftToPopulateObject--;
            if (_intPropertiesLeftToPopulateObject <= 0)
                ParseNumber(_strSource, _enuParseAs);
            }
        }

        /// <summary>
        /// Returns the international dialing format
        /// </summary>
        [DataMember]
        public string InternationalDialString
        {
            get { return ToInternationalDialString(); }
        }

		/// <summary>
		/// Returns the CountryCode
		/// </summary>
        [DataMember]
        public Int64 CountryCode
		{
			get	{return _intCountryCode;}
			//set {_intCountryCode = value;}
		}

		/// <summary>
		/// Returns the AreaCode
		/// </summary>
        [DataMember]
        public Int64 AreaCode
		{
			get	{return _intAreaCode;}
			//set {_intAreaCode = value;}
		}

		/// <summary>
		/// Returns the remaining digits in the PhoneNumber
		/// </summary>
        [DataMember]
        public Int64 Number
		{
			get	{return _intNumber;}
			//set { if (value.HasValue) _intNumber = value.Value; else _intNumber = -1; }
		}

		/// <summary>
		/// Returns the Extension
		/// </summary>
        [DataMember]
        public Int32? Extension
		{
			get	{return _intExtension;}
			//set {_intExtension = value;}
		}

		/// <summary>
		/// Returns the name of the Country the phone number is based out of
		/// </summary>
        [DataMember]
        public string CountryName
		{
			get	{return _strCountryName;}
		}

		/// <summary>
		/// Returns a description of the Area derived from the AreaCode
		/// </summary>
        [DataMember]
        public string AreaDescription
		{
			get	{return _strAreaDescription;}
		}
		#endregion

		#region Operators
        /// <summary>
        /// Casts a string as a PhoneNumber
        /// </summary>
        public static implicit operator PhoneNumber(string PhoneNumber)
        {
            return new PhoneNumber(PhoneNumber);
        }

		/// <summary>
		/// Compares two PhoneNumber objects
		/// </summary>
		public static bool operator ==(PhoneNumber Phone1, PhoneNumber Phone2)
		{
			if((object) Phone1 == null && (object) Phone2 != null)
				return false;
			if((object) Phone2 == null && (object) Phone1 != null)
				return false;
			if((object) Phone1 == null && (object) Phone2 == null)
				return true;
			return(Phone1.ToUnformattedNumber() == Phone2.ToUnformattedNumber());
		}

		/// <summary>
		/// Compares two PhoneNumber objects
		/// </summary>
		public static bool operator !=(PhoneNumber Phone1, PhoneNumber Phone2)
		{
			if((object) Phone1 == null && (object) Phone2 != null)
				return true;
			if((object)Phone2 == null && (object) Phone1 != null)
				return true;
			if((object) Phone1 == null && (object) Phone2 == null)
				return false;
			return(Phone1.ToUnformattedNumber() != Phone2.ToUnformattedNumber());
		}	

		/// <summary>
		/// Compares two PhoneNumber objects
		/// </summary>
		public override bool Equals(object obj)
		{
			return(this==(PhoneNumber) obj);
		}

		/// <summary>
		/// Returns a HashCode for this PhoneNumber object
		/// </summary>
		public override int GetHashCode()
		{
			return(this.ToUnformattedNumber().GetHashCode());
		}

		#endregion

		#region Enumerators
		/// <summary>
		/// Enumerates possible PhoneNumber parsing arguments
		/// </summary>
		public enum ParseArgument
		{
			/// <summary>
			/// Parses via PhoneNumber database, best for unkown and international numbers.
			/// </summary>
			Unknown = 1,
			/// <summary>
			/// For PhoneNumbers that abide by the rules of the North American Numbering Plan (3 digit area code, 7 digit number)
			/// </summary>
			NANP = 2
		}
		#endregion

		#region Static

        #region IsValid
        public static bool IsValid(string strPhoneNumber)
        {
            return new PhoneNumber(strPhoneNumber).Valid;
        }
        #endregion

        #region GetPhoneCountryCodeFromISOCountryCode
        /// <summary>
		/// Returns a telephone numeric CountryCode for a specified ISO string CountryCode
		/// EX: US returns 1
		/// </summary>
		public static long GetPhoneCountryCodeFromISOCountryCode(string strISOCountryCode)
		{
			XmlDocument Countries = GetCountryDatabase();
			//SCAN THE COUNTRY CODE DATABASE FOR A MATCH
			XmlNode Country;
			string strPath = "/General.PhoneNumber/Country[@ID='" + strISOCountryCode + "']";
			Country = Countries.SelectSingleNode(strPath);
			return long.Parse(Country.Attributes["CountryCode"].Value);
		}
		#endregion

        #region GetCountryCodeFromUserLocale
        /// <summary>
		/// Returns an country code from the web site visitors specified language preference
		/// en-us becomes 1 (United States)
		/// ja becomes 81 (Japan)
		/// </summary>
		public static long GetCountryCodeFromUserLocale()
		{
			return GetPhoneCountryCodeFromISOCountryCode(GetISOCountryCodeFromUserLocale());
		}


		/// <summary>
		/// Returns an ISO country code from the web site visitors specified language preference
		/// en-us becomes US (United States)
		/// ja becomes JP (Japan)
		/// </summary>
		public static string GetISOCountryCodeFromUserLocale()
		{
			if(System.Web.HttpContext.Current == null)
				throw new NotSupportedException("HttpContext Unavailable");

			string strLanguageCodes = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
            if (StringFunctions.IsNullOrWhiteSpace(strLanguageCodes))
                strLanguageCodes = String.Empty;

			string[] aryCodes = strLanguageCodes.Split(',');
			foreach(string strCode in aryCodes)
			{
				string strCountryCode = GetISOCountryCodeFromLanguageCode(strCode);
				if(!StringFunctions.IsNullOrWhiteSpace(strCountryCode))
					return strCountryCode;
			}
			return String.Empty;
        }
        #endregion

        #region GetCountryCodeFromLanguageCode
        /// <summary>
		/// Returns an ISO country code for the specified language code
		/// en-us becomes 1 (United States)
		/// ja becomes 81 (Japan)
		/// </summary>
		public static long GetCountryCodeFromLanguageCode(string strLanguageCode)
		{
			return GetPhoneCountryCodeFromISOCountryCode(GetISOCountryCodeFromLanguageCode(strLanguageCode));
		}

		/// <summary>
		/// Returns an ISO country code for the specified language code
		/// en-us becomes US (United States)
		/// ja becomes JP (Japan)
		/// </summary>
		public static string GetISOCountryCodeFromLanguageCode(string strLanguageCode)
		{
			switch(strLanguageCode.ToLower().Trim())
			{
				case "sq": return "AL";
				case "ar-ae": return "AE";
				case "ar-bh": return "BH";
				case "ar-dz": return "DZ";
				case "ar-eg": return "EG";
				case "ar-iq": return "IQ";
				case "ar-jo": return "JO";
				case "ar-kw": return "KW";
				case "ar-lb": return "LB";
				case "ar-ly": return "LY";
				case "ar-ma": return "MA";
				case "ar-om": return "OM";
				case "ar-qa": return "QA";
				case "ar-sa": return "SA";
				case "ar-sy": return "SY";
				case "ar-tn": return "TN";
				case "ar-ye": return "YE";
				case "hy": return "AM";
				case "bg": return "BG";
				case "zh-cn": return "CN";
				case "zh-hk": return "HK";
				case "zh-mo": return "MO";
				case "zh-sg": return "SG";
				case "zh-tw": return "TW";
				case "nl-nl": return "NL";
				case "nl-be": return "BE";
				case "en-au": return "AU";
				case "en-bz": return "BZ";
				case "en-ca": return "CA";
				case "en-ie": return "IE";
				case "en-jm": return "JM";
				case "en-nz": return "NZ";
				case "en-ph": return "PH";
				case "en-za": return "ZA";
				case "en-tt": return "TT";
				case "en-gb": return "GB";
				case "en-us": return "US";
				case "en": return "US";
				case "et": return "EE";
				case "fr-fr": return "FR";
				case "fr-be": return "BE";
				case "fr-ca": return "CA";
				case "fr-lu": return "LU";
				case "fr-ch": return "CH";
				case "gd-ie": return "IE";
				case "gd": return "GB";
				case "de-de": return "DE";
				case "de-at": return "AT";
				case "de-li": return "LI";
				case "de-lu": return "LU";
				case "de-ch": return "CH";
				case "el": return "GR";
				case "he": return "IL";
				case "hu": return "HU";
				case "is": return "IS";
				case "id": return "ID";
				case "it-it": return "IT";
				case "it-ch": return "CH";
				case "ja": return "JP";
				case "ko": return "KR";
				case "lv": return "LV";
				case "lt": return "LT";
				case "mk": return "MK";
				case "ms-my": return "MY";
				case "ms-bn": return "BN";
				case "mt": return "MT";
				case "pl": return "PL";
				case "pt-pt": return "PT";
				case "pt-br": return "BR";
				case "ro": return "RO";
				case "ro-mo": return "MD";
				case "ru": return "RU";
				case "ru-mo": return "MD";
				case "sr-sp": return "CS";
				case "es-es": return "ES";
				case "es-ar": return "AR";
				case "es-bo": return "BO";
				case "es-cl": return "CL";
				case "es-co": return "CO";
				case "es-cr": return "CR";
				case "es-do": return "DO";
				case "es-ec": return "EC";
				case "es-gt": return "GT";
				case "es-hn": return "HN";
				case "es-mx": return "MX";
				case "es-ni": return "NI";
				case "es-pa": return "PA";
				case "es-pe": return "PE";
				case "es-pr": return "PR";
				case "es-py": return "PY";
				case "es-sv": return "SV";
				case "es-uy": return "UY";
				case "es-ve": return "VE";
				case "sv-se": return "SE";
				case "sv-fi": return "FI";
				case "th": return "TH";
				case "uk": return "UA";
				case "uz-uz": return "UZ";
				case "vi": return "VN";

				default: return "US";
			}
        }

        #endregion

        #region EnableNoneText
        [NonSerialized]
        private static bool _blnEnableNoneText = false;

        public static bool EnableNoneText
        {
            get
            {
                return _blnEnableNoneText;
            }
            set
            {
                _blnEnableNoneText = value;
            }
        }
        #endregion

        public static String ConvertLettersToNumbers(string strPhone)
        {
            strPhone = System.Text.RegularExpressions.Regex.Replace(strPhone, "[a-cA-C]", "2");
            strPhone = System.Text.RegularExpressions.Regex.Replace(strPhone, "[d-fD-F]", "3");
            strPhone = System.Text.RegularExpressions.Regex.Replace(strPhone, "[g-iG-I]", "4");
            strPhone = System.Text.RegularExpressions.Regex.Replace(strPhone, "[j-lJ-L]", "5");
            strPhone = System.Text.RegularExpressions.Regex.Replace(strPhone, "[m-oM-O]", "6");
            strPhone = System.Text.RegularExpressions.Regex.Replace(strPhone, "[p-sP-S]", "7");
            strPhone = System.Text.RegularExpressions.Regex.Replace(strPhone, "[t-vT-V]", "8");
            strPhone = System.Text.RegularExpressions.Regex.Replace(strPhone, "[w-zW-Z]", "9");
            return strPhone;
        }
        #endregion

    }

	
}
