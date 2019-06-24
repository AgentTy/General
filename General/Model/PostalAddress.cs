using System;
using System.Text;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using General;
using System.ComponentModel.DataAnnotations.Schema;

namespace General.Model
{
	/// <summary>
	/// Address Class
	/// </summary>
	[Serializable, DataContract]
	public class PostalAddress : Internal.JITData
	{

		#region Private Variables
		/// <summary>
		/// Reference
		/// </summary>
        [NonSerialized]
		protected string _strReference = string.Empty;
		/// <summary>
		/// Address1
		/// </summary>
        [NonSerialized]
        protected string _strAddress1 = string.Empty;
		/// <summary>
		/// Address2
		/// </summary>
        [NonSerialized]
        protected string _strAddress2 = string.Empty;
		/// <summary>
		/// Address3
		/// </summary>
        [NonSerialized]
        protected string _strAddress3 = string.Empty;
		/// <summary>
		/// City
		/// </summary>
        [NonSerialized]
        protected string _strCity = string.Empty;
		/// <summary>
		/// State
		/// </summary>
        [NonSerialized]
        protected string _strStateCode = string.Empty;
		/// <summary>
		/// State
		/// </summary>
        [NonSerialized]
        protected string _strStateName = string.Empty;
		/// <summary>
		/// Postal
		/// </summary>
        [NonSerialized]
        protected string _strPostalCode = string.Empty;
		/// <summary>
		/// Country
		/// </summary>
        [NonSerialized]
        protected string _strCountryCode = string.Empty;
		/// <summary>
		/// Country
		/// </summary>
        [NonSerialized]
        protected string _strCountryName = string.Empty;
		#endregion

		#region Constructors
		/// <summary>
		/// Address Class
		/// </summary>
		public PostalAddress()
		{
			_strCountryCode = "US";
		}

        /// <summary>
        /// Address Class
        /// </summary>
        public PostalAddress(string strAddress)
        {
            Parse(strAddress);
        }

		/// <summary>
		/// Address Class
		/// </summary>
		public PostalAddress(string Reference,string Address1,string Address2,string Address3,string City,string StateCode,string PostalCode,string CountryCode)
		{
			_strReference = Reference;
			_strAddress1 = Address1;
			_strAddress2 = Address2;
			_strAddress3 = Address3;
			_strCity = City;
			_strStateCode = StateCode;
			_strPostalCode = PostalCode;
			_strCountryCode = CountryCode;
			if(_strCountryCode == null || _strCountryCode == String.Empty) _strCountryCode = "US";
		}
        #endregion

		#region Output
		/// <summary>
		/// Returns a string formatted address with html line breaks 
		/// </summary>
		public override string ToString()
		{
			return(ToString("<br/>"));
		}

		/// <summary>
		/// Returns a string formatted address with custom line breaks 
		/// </summary>
		public virtual string ToString(string LineBreak)
		{
            return ToHTMLString(LineBreak);
		}

        public string ToLocationString()
        {
            return ToLocationString(false);
        }

        public string ToLocationString(bool blnShort)
		{
			StringBuilder sb = new StringBuilder();
			
			//CITY
			if(_strCity != null && _strCity != "")
			{
				sb.Append(_strCity);
				if(_strStateCode != null && _strStateCode != "")
					sb.Append(", ");
				else if (_strPostalCode != null && _strPostalCode != "")
					sb.Append(" ");
				else if (_strCountryCode != null && _strCountryCode != "" && _strCountryCode.ToLower() != "us" && _strCountryCode.ToLower() != "usa")
					sb.Append("; ");
			}

            //STATE
            if (_strStateCode != null && _strStateCode != "")
            {
                if (!StringFunctions.IsNullOrWhiteSpace(_strStateName) && !blnShort)
                    sb.Append(_strStateName);
                else
                    sb.Append(_strStateCode);

                if (_strPostalCode != null && _strPostalCode != "")
                    sb.Append(" ");
                else if (_strCountryCode != null && _strCountryCode != "" && _strCountryCode.ToLower() != "us" && _strCountryCode.ToLower() != "usa")
                    sb.Append("; ");
            }

            //COUNTRY
            if (_strCountryCode != null && _strCountryCode != "" && _strCountryCode.ToLower() != "us" && _strCountryCode.ToLower() != "usa")
            {
                if (!StringFunctions.IsNullOrWhiteSpace(_strCountryName))
                    sb.Append(_strCountryName);
                else
                    sb.Append(_strCountryCode);
            }
			

			return(sb.ToString());
		}
        [DataMember]
        public string GetLocationString
        {
            get
            {
                return ToLocationString();
            }
        }

        public string ToCountryStateString()
        {
                return _strStateName + "," + _strCountryCode;
        }

        public string GetCountryStateString
        {
            get
            {
                return ToCountryStateString();
            }
        }

        public string ToCountryStateStringForUS()
        {
            if (_strCountryCode != "US")
                if(StringFunctions.IsNullOrWhiteSpace(_strStateName))
                    return _strCountryName;
                else
                    return _strStateName + "," + _strCountryName;
            else
                return _strStateName;
        }

        public string GetCountryStateStringForUS
        {
            get
            {
                return ToCountryStateStringForUS();
            }
        }

        public string ToHTMLString(string LineBreak)
        {
            StringBuilder sb = new StringBuilder();

            if (_strReference != null && _strReference.Trim() != "") sb.Append(_strReference + LineBreak);
            if (_strAddress1 != null && _strAddress1.Trim() != "") sb.Append(_strAddress1 + LineBreak);
            if (_strAddress2 != null && _strAddress2.Trim() != "") sb.Append(_strAddress2 + LineBreak);
            if (_strAddress3 != null && _strAddress3.Trim() != "") sb.Append(_strAddress3 + LineBreak);

            //CITY
            if (_strCity != null && _strCity != "")
            {
                sb.Append(_strCity);
                if (_strStateCode != null && _strStateCode != "")
                    sb.Append(", ");
                else if (_strPostalCode != null && _strPostalCode != "")
                    sb.Append(" ");
                else if (_strCountryCode != null && _strCountryCode != "" && _strCountryCode.ToLower() != "us" && _strCountryCode.ToLower() != "usa")
                    sb.Append("; ");
            }

            //STATE
            if (_strStateCode != null && _strStateCode != "")
            {
                if (!StringFunctions.IsNullOrWhiteSpace(_strStateName))
                    sb.Append(_strStateName);
                else
                    sb.Append(_strStateCode);

                if (_strPostalCode != null && _strPostalCode != "")
                    sb.Append(" ");
                else if (_strCountryCode != null && _strCountryCode != "" && _strCountryCode.ToLower() != "us" && _strCountryCode.ToLower() != "usa")
                    sb.Append("; ");
            }

            //POSTAL
            if (_strPostalCode != null && _strPostalCode != "")
            {
                sb.Append(_strPostalCode);
                if (_strCountryCode != null && _strCountryCode != "" && _strCountryCode.ToLower() != "us" && _strCountryCode.ToLower() != "usa")
                    sb.Append(LineBreak);
            }

            //COUNTRY
            if (_strCountryCode != null && _strCountryCode != "" && _strCountryCode.ToLower() != "us" && _strCountryCode.ToLower() != "usa")
            {
                if (!StringFunctions.IsNullOrWhiteSpace(_strCountryName))
                    sb.Append(_strCountryName);
                else
                    sb.Append(_strCountryCode);
            }

            sb.Append(LineBreak);

            return (sb.ToString());
        }
		#endregion

		#region Public Properties
		/// <summary>
		/// Returns the address reference
		/// </summary>
        [NotMapped]
		public string Reference
		{
			get	{return _strReference;}
            set { 
                _strReference = PokeSeal(_strReference, value);
                if (_strReference != null)
                    _strReference = _strReference.Trim();
            }
		}

		/// <summary>
		/// Returns address line 1
		/// </summary>
        [DataMember]
        public string Address1
		{
			get	{return _strAddress1;}
			set { _strAddress1 = PokeSeal(_strAddress1,value); }
		}

		/// <summary>
		/// Returns address line 2
		/// </summary>
        [DataMember]
        public string Address2
		{
			get	{return _strAddress2;}
			set { _strAddress2 = PokeSeal(_strAddress2,value); }
		}

		/// <summary>
		/// Returns address line 3
		/// </summary>
        [DataMember]
        public virtual string Address3
		{
			get	{return _strAddress3;}
			set { _strAddress3 = PokeSeal(_strAddress3,value); }
		}

		/// <summary>
		/// Returns the city
		/// </summary>
        [DataMember]
        public string City
		{
			get	{return _strCity;}
            set { 
                _strCity = PokeSeal(_strCity, value);
                if (_strCity != null)
                    _strCity = _strCity.Trim();
            }
		}

		/// <summary>
		/// Returns the state
		/// </summary>
        [DataMember]
        public string StateCode
		{
			get	{return _strStateCode;}
            set { 
                _strStateCode = PokeSeal(_strStateCode, value);
                if (_strStateCode != null)
                    _strStateCode = _strStateCode.Trim();
            }
		}

        /// <summary>
        /// Returns the state
        /// </summary>
        [NotMapped]
        public string StateName
		{
			get	{return _strStateName;}
			set { _strStateName = PokeSeal(_strStateName,value); }
		}

		/// <summary>
		/// Returns the postal code
		/// </summary>
        [DataMember]
        public string PostalCode
		{
			get	{return _strPostalCode;}
            set { 
                _strPostalCode = PokeSeal(_strPostalCode, value);
                if (_strPostalCode != null)
                    _strPostalCode = _strPostalCode.Trim();
            }
		}
		
		/// <summary>
		/// Returns the country code
		/// </summary>
        [DataMember]
        public string CountryCode
		{
			get	{return _strCountryCode;}
			set { _strCountryCode = PokeSeal(_strCountryCode,value); }
		}

        /// <summary>
        /// Returns the country code
        /// </summary>
        [NotMapped]
        public string CountryName
		{
			get	{return _strCountryName;}
			set { _strCountryName = PokeSeal(_strCountryName,value); }
		}

		/// <summary>
		/// Outputs HTML String
		/// </summary>
		private string HTMLString
		{
			get { return ToHTMLString("<br/>"); }
		}

        /// <summary>
        /// Outputs Inline String for Google
        /// </summary>
        public virtual string PostalAddressGoogleString
        {
            get { return ToHTMLString(" "); }
        }

        public bool WorthValidating
        {
            get
            {
                if (StringFunctions.IsNullOrWhiteSpace(Address1) || StringFunctions.IsNullOrWhiteSpace(City))
                    return false;
                return true;
            }
        }

        public bool WorthMakingLabel
        {
            get
            {
                if (StringFunctions.IsNullOrWhiteSpace(Address1) || StringFunctions.IsNullOrWhiteSpace(City) || StringFunctions.IsNullOrWhiteSpace(PostalCode))
                    return false;
                return true;
            }
        }

        public bool HasZipPlus4
        {
            get
            {
                return (System.Text.RegularExpressions.Regex.IsMatch(PostalCode, "\\d{5}-\\d{4}"));
            }
        }
		#endregion

		#region Public Methods

		/// <summary>
		/// Validates the data
		/// </summary>
		public virtual bool Validate(ref string Errors)
		{
			return Validate(ref Errors,"<br>\n");
		}

		/// <summary>
		/// Validates the data
		/// </summary>
		public virtual bool Validate(ref string Errors, string LineBreak)
		{
			string strRequiredList = "address1,city,statecode,postalcode,countrycode";
			return Validate(ref Errors,"<br>\n",strRequiredList);
		}

		/// <summary>
		/// Validates the data
		/// </summary>
		public virtual bool Validate(ref string Errors, string LineBreak, string RequiredList)
		{
			bool valid = true;
			StringBuilder sb = new StringBuilder();
			RequiredList = RequiredList.ToLower();

			if(StringFunctions.Contains(RequiredList,"address1") || StringFunctions.Contains(RequiredList,"address"))
			{
				if(_strAddress1 == string.Empty)
				{
					valid = false;
					sb.Append("Must fill out address line 1." + LineBreak);
				}
			}

			if(StringFunctions.Contains(RequiredList,"city"))
			{
				if(_strCity == string.Empty)
				{
					valid = false;
					sb.Append("Must fill out city." + LineBreak);
				}
			}

			if(StringFunctions.Contains(RequiredList,"statecode") || StringFunctions.Contains(RequiredList,"state"))
			{
                if (_strStateCode == string.Empty)
				{
					valid = false;
					sb.Append("Must enter State/Province." + LineBreak);
				}
			}

			if(StringFunctions.Contains(RequiredList,"postalcode") || StringFunctions.Contains(RequiredList,"postal"))
			{
				if(_strPostalCode == string.Empty)
				{
					valid = false;
					sb.Append("Must fill out postal code." + LineBreak);
				}
			}

			if(StringFunctions.Contains(RequiredList,"countrycode") || StringFunctions.Contains(RequiredList,"country"))
			{
				if(_strCountryCode == string.Empty)
				{
					valid = false;
					sb.Append("Must fill out country code." + LineBreak);
				}
			}

			Errors += sb.ToString();
            return valid;
		}
		#endregion

        #region Parse
        private void Parse(string str)
        {
            throw new NotImplementedException("This ability will require extensive coding as well as access to a database of address info (cities, states)");
        }        
        #endregion

        #region Operators
        /// <summary>
		/// Compares two PostalAddress objects
		/// </summary>
		public static bool operator ==(PostalAddress Address1, PostalAddress Address2)
		{
			string x,y;

			try
			{x = Address1.ToString();}
			catch(NullReferenceException ex)
			{
				string temp = ex.Message;
				x = "null";
			}	

			try
			{y = Address2.ToString();}	
			catch(NullReferenceException ex)
			{
				string temp = ex.Message;
				y = "null";
			}

			return(x == y);
		}

		/// <summary>
		/// Compares two PostalAddress objects
		/// </summary>
		public static bool operator !=(PostalAddress Address1, PostalAddress Address2)
		{
			string x,y;

			try
			{x = Address1.ToString();}
			catch(NullReferenceException ex)
			{
				string temp = ex.Message;
				x = "null";
			}	

			try
			{y = Address2.ToString();}	
			catch(NullReferenceException ex)
			{
				string temp = ex.Message;
				y = "null";
			}

			return(x != y);
		}	

		/// <summary>
		/// Compares two PostalAddress objects
		/// </summary>
		public override bool Equals(object obj)
		{
			return(this==(PostalAddress) obj);
		}

		/// <summary>
		/// Returns a HashCode for this PostalAddress object
		/// </summary>
		public override int GetHashCode()
		{
			return(ToString().GetHashCode());
		}

		#endregion

        #region Serialization
        [NonSerialized]
        private static char Delimiter = '~';

        #region Serialize
        public static string Serialize(PostalAddress obj)
        {
            string str = String.Empty;
            str += obj.Reference + Delimiter;
            str += obj.Address1 + Delimiter;
            str += obj.Address2 + Delimiter;
            str += obj.Address3 + Delimiter;
            str += obj.City + Delimiter;
            str += obj.StateCode + Delimiter;
            str += obj.StateName + Delimiter;
            str += obj.PostalCode + Delimiter;
            str += obj.CountryCode + Delimiter;
            str += obj.CountryName;
            return str;
        }
        #endregion

        #region Deserialize
        public static PostalAddress Deserialize(string str)
        {
            if (str.Contains(Delimiter.ToString()))
            {
                PostalAddress obj = new PostalAddress();
                string[] ary = str.Split(Delimiter);
                int i = 0;
                obj.Reference = ary[i++];
                obj.Address1 = ary[i++];
                obj.Address2 = ary[i++];
                obj.Address3 = ary[i++];
                obj.City = ary[i++];
                obj.StateCode = ary[i++];
                obj.StateName = ary[i++];
                obj.PostalCode = ary[i++];
                obj.CountryCode = ary[i++];
                obj.CountryName = ary[i++];
                return obj;
            }
            else
            {
                return new PostalAddress(str);
            }
        }
        #endregion

        #endregion

    }
}
