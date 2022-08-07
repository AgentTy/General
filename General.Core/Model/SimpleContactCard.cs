using System;
using System.Text;
using System.Web;

using System.Runtime.Serialization;
using General;

namespace General.Model
{
    /// <summary>
    /// A SimpleContactCard is an object that contains a PostalAddress + additional fields such as Name, Phone, Email, etc.
    /// </summary>
    [Serializable, DataContract]
	public class SimpleContactCard
	{

        #region Constructors

        /// <summary>
        /// Creates a blank SimpleContactCard
        /// </summary>
        public SimpleContactCard()
		{

		}

		#endregion

		#region Public Properties
		/// <summary>
		/// First Name
		/// </summary>
        [DataMember]
		public string FirstName { get; set; }

		/// <summary>
		/// Last Name
		/// </summary>
        [DataMember]
        public string LastName { get; set; }

		/// <summary>
		/// Phone
		/// </summary>
        [DataMember]
		public PhoneNumber Phone { get; set; }
		
		/// <summary>
		/// Email
		/// </summary>
        [DataMember]
		public EmailAddress Email { get; set; }

        [DataMember]
        public String EmailWithName
        {
            get
            {
                if (Email == null)
                    return "";
                Email.Name = this.FullName;
                return Email.GetEmailWithName;
            }
        }

        [DataMember]
        public virtual string FullName { get { return GetFullName(); } set { ReadFullName(value); } }

        /// <summary>
        /// Outputs Inline String for Google
        /// </summary>
        public string PostalAddressGoogleString
        {
            //get { return Company + " " + base.PostalAddressGoogleString; }
            get { return HttpUtility.UrlEncode(ToHTMLString(",").TrimEnd(new [] {','})); }
        }

        #endregion

        #region PostalAddress Properties
        /// <summary>
        /// Returns address line 1
        /// </summary>
        [DataMember]
        public string Address1 { get; set; }

        /// <summary>
        /// Returns address line 2
        /// </summary>
        [DataMember]
        public string Address2 { get; set; }

        /// <summary>
        /// Returns address line 3
        /// </summary>
        [DataMember]
        public virtual string Address3 { get; set; }

        private string _strCity;
        /// <summary>
        /// Returns the city
        /// </summary>
        [DataMember]
        public string City
        {
            get { return _strCity; }
            set
            {
                _strCity = value;
                if (_strCity != null)
                    _strCity = _strCity.Trim();
            }
        }

        private string _strStateCode;
        /// <summary>
        /// Returns the state
        /// </summary>
        [DataMember]
        public string StateCode
        {
            get { return _strStateCode; }
            set
            {
                _strStateCode = value;
                if (_strStateCode != null)
                    _strStateCode = _strStateCode.Trim();
            }
        }

        private string _strPostalCode;
        /// <summary>
        /// Returns the postal code
        /// </summary>
        [DataMember]
        public string PostalCode
        {
            get { return _strPostalCode; }
            set
            {
                _strPostalCode = value;
                if (_strPostalCode != null)
                    _strPostalCode = _strPostalCode.Trim();
            }
        }


        private string _strCountryCode;
        /// <summary>
        /// Returns the country code
        /// </summary>
        [DataMember]
        public string CountryCode
        {
            get { return _strCountryCode; }
            set {
                _strCountryCode = value;
                if (_strCountryCode != null)
                    _strCountryCode = _strCountryCode.Trim();
            }
        }

        /// <summary>
        /// Outputs HTML String
        /// </summary>
        private string HTMLString
        {
            get { return ToHTMLString("<br/>"); }
        }

        public bool WorthValidating
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Address1) || String.IsNullOrWhiteSpace(City))
                    return false;
                return true;
            }
        }

        public bool WorthMakingLabel
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Address1) || String.IsNullOrWhiteSpace(City) || String.IsNullOrWhiteSpace(PostalCode))
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
        public bool ValidateAddress(ref string Errors)
        {
            return AddressValidate(ref Errors, "<br>\n");
        }

		/// <summary>
		/// Validates the data
		/// </summary>
		public bool Validate(ref string Errors)
		{
			return Validate(ref Errors, "<br>\n");
		}

		/// <summary>
		/// Validates the data
		/// </summary>
		public bool Validate(ref string Errors, string LineBreak)
		{
			string strRequiredList = "firstname,lastname,phone,fax,email,address1,city,statecode,postalcode,countrycode"; 
			return Validate(ref Errors, "<br>\n",strRequiredList);
		}

		/// <summary>
		/// Validates the data
		/// </summary>
		public bool Validate(ref string Errors, string LineBreak, string RequiredList)
		{
			StringBuilder sb = new StringBuilder();
			
			bool valid = true;
			RequiredList = RequiredList.ToLower();

			if(StringFunctions.Contains(RequiredList,"firstname") || StringFunctions.Contains(RequiredList,"name"))
			{
				if(FirstName == string.Empty)
				{
					valid = false;
					sb.Append("Must fill out first name." + LineBreak);
				}
			}

			if(StringFunctions.Contains(RequiredList,"lastname") || StringFunctions.Contains(RequiredList,"name"))
			{
				if(LastName == string.Empty)
				{
					valid = false;
					sb.Append("Must fill out last name." + LineBreak);
				}
			}

			if(StringFunctions.Contains(RequiredList,"phone") || StringFunctions.Contains(RequiredList,"phonenumber"))
			{
				if(Phone == null)
				{
					valid = false;
					sb.Append("Must fill out phone number." + LineBreak);
				}
				else if(!Phone.Valid)
				{
					valid = false;
					sb.Append("Must enter valid phone number." + LineBreak);
				}
			}


			if(StringFunctions.Contains(RequiredList,"email") || StringFunctions.Contains(RequiredList,"emailaddress"))
			{
				if(Email == null)
				{
					valid = false;
					sb.Append("Must fill out email address." + LineBreak);
				}
				else if(!Email.Valid)
				{
					valid = false;
					sb.Append("Must enter valid email address." + LineBreak);
				}
			}
			
			string err = sb.ToString();

			if(AddressValidate(ref err, LineBreak, RequiredList) == false)
				valid = false;

			Errors += err;
			return valid;
		}
		#endregion

		#region Private Functions
		private string GetFullName() 
		{
			return (FirstName + " " + LastName).Trim();
		}

        private void ReadFullName(string strFullName)
        {
            if (!String.IsNullOrWhiteSpace(strFullName))
            {
                strFullName = strFullName.Trim();
                if (strFullName.Contains(" "))
                {
                    string[] aryNameParts = strFullName.Split(' ');
                    if (aryNameParts.Length == 1)
                        this.FirstName = strFullName;
                    else
                    {
                        this.LastName = StringFunctions.AllAfterReverse(strFullName, " ");
                        this.FirstName = strFullName.Replace(this.LastName, "").Trim();
                    }
                }
                else
                {
                    this.FirstName = strFullName;
                }
            }
        }
        #endregion

        #region Output
        public string ToLocationString()
        {
            return ToLocationString(false);
        }

        public string ToLocationString(bool blnShort)
        {
            StringBuilder sb = new StringBuilder();

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
                sb.Append(_strStateCode);

                if (_strPostalCode != null && _strPostalCode != "")
                    sb.Append(" ");
                else if (_strCountryCode != null && _strCountryCode != "" && _strCountryCode.ToLower() != "us" && _strCountryCode.ToLower() != "usa")
                    sb.Append("; ");
            }

            //COUNTRY
            if (_strCountryCode != null && _strCountryCode != "" && _strCountryCode.ToLower() != "us" && _strCountryCode.ToLower() != "usa")
            {
                sb.Append(_strCountryCode);
            }


            return (sb.ToString());
        }
        [DataMember]
        public string GetLocationString
        {
            get
            {
                return ToLocationString();
            }
        }

        /// <summary>
        /// Returns a string formatted ContactCard with html line breaks 
        /// </summary>
        public override string ToString()
		{
			return(ToString("<br>"));
		}

        /// <summary>
		/// Returns a string formatted address with custom line breaks 
		/// </summary>
		public virtual string ToString(string LineBreak)
        {
            return ToHTMLString(LineBreak);
        }

        /// <summary>
		/// Returns a string formatted AddressBookEntry with specified line breaks 
		/// </summary>
        public string ToHTMLString()
        {
            return ToHTMLString("<br>\n");
        }

		/// <summary>
		/// Returns a string formatted AddressBookEntry with specified line breaks 
		/// </summary>
		public string ToHTMLString(string LineBreak)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(FirstName);
			if(FirstName != "") sb.Append(" ");
			
			sb.Append(LastName);
			if(sb.Length > 0) sb.Append(LineBreak);
			
			sb.Append(GenerateAddressHtmlString(LineBreak));
			
			if(Phone != null)
			{
				if (Phone.Valid) 
				{
					sb.Append(Phone.ToString());
					sb.Append(LineBreak);
				}
			}
			
			if(Email != null)
			{
				if(Email.Valid) 
				{
					if(StringFunctions.Contains(LineBreak,"<br"))
						sb.Append(Email.ToLink());
					else
						sb.Append(Email.ToString());
					sb.Append(LineBreak);
				}
			}
			
			return sb.ToString();
		}


        public string ToHTMLAddressString()
        {
            return ToHTMLAddressString(true);
        }

		/// <summary>
		/// Returns a string formatted AddressBookEntry with specified line breaks 
		/// </summary>
		public string ToHTMLAddressString(bool blnIncludeName)
		{
			string LineBreak = "<br>\n";
			StringBuilder sb = new StringBuilder();

            if (blnIncludeName)
            {
                sb.Append(FirstName);
                if (LastName != "") sb.Append(" ");

                sb.Append(LastName);
                if (sb.Length > 0) sb.Append(LineBreak);
            }

			sb.Append(ToString(LineBreak));
			return sb.ToString();
		}

        public string GenerateAddressHtmlString(string LineBreak)
        {
            StringBuilder sb = new StringBuilder();

            if (Address1 != null && Address1.Trim() != "") sb.Append(Address1 + LineBreak);
            if (Address2 != null && Address2.Trim() != "") sb.Append(Address2 + LineBreak);
            if (Address3 != null && Address3.Trim() != "") sb.Append(Address3 + LineBreak);

            //CITY
            if (City != null && City != "")
            {
                sb.Append(City);
                if (StateCode != null && StateCode != "")
                    sb.Append(", ");
                else if (PostalCode != null && PostalCode != "")
                    sb.Append(" ");
                else if (CountryCode != null && CountryCode != "" && CountryCode.ToLower() != "us" && CountryCode.ToLower() != "usa")
                    sb.Append("; ");
            }

            //STATE
            if (StateCode != null && StateCode != "")
            {
                sb.Append(StateCode);

                if (PostalCode != null && PostalCode != "")
                    sb.Append(" ");
                else if (CountryCode != null && CountryCode != "" && CountryCode.ToLower() != "us" && CountryCode.ToLower() != "usa")
                    sb.Append("; ");
            }

            //POSTAL
            if (PostalCode != null && PostalCode != "")
            {
                sb.Append(PostalCode);
                if (CountryCode != null && CountryCode != "" && CountryCode.ToLower() != "us" && CountryCode.ToLower() != "usa")
                    sb.Append(LineBreak);
            }

            //COUNTRY
            if (CountryCode != null && CountryCode != "" && CountryCode.ToLower() != "us" && CountryCode.ToLower() != "usa")
            {
                sb.Append(CountryCode);
            }

            sb.Append(LineBreak);

            return (sb.ToString());
        }

        #region ToDebuggingString
        public virtual string ToDebuggingString()
		{
			return ToDebuggingString("<br>");
		}

		public virtual string ToDebuggingString(string strLineBreak)
		{
			return ToString(strLineBreak);
		}
        #endregion ToDebuggingString

        #endregion

        #region Operators
        /// <summary>
        /// Compares two SimpleContactCard objects
        /// </summary>
        public static bool operator ==(SimpleContactCard Address1, SimpleContactCard Address2)
		{
			string x,y;

            if (((object)Address1) == null)
                return ((object)Address2) == null;
            else if (((object)Address2) == null)
                return ((object)Address1) == null;

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
		/// Compares two AddressBookEntry objects
		/// </summary>
		public static bool operator !=(SimpleContactCard Address1, SimpleContactCard Address2)
		{
			string x,y;

            if (((object)Address1) == null)
                return ((object)Address2) != null;
            else if (((object)Address2) == null)
                return ((object)Address1) != null;

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
		/// Compares two AddressBookEntry objects
		/// </summary>
		public override bool Equals(object obj)
		{
            if (obj.GetType() != typeof(SimpleContactCard))
                return false;
			return(this==(SimpleContactCard) obj);
		}

		/// <summary>
		/// Returns a HashCode for this AddressBookEntry object
		/// </summary>
		public override int GetHashCode()
		{
			return(this.ToString().GetHashCode());
		}

        #endregion


        #region Postal Address Methods

        /// <summary>
        /// Validates the data
        /// </summary>
        public virtual bool AddressValidate(ref string Errors)
        {
            return AddressValidate(ref Errors, "<br>\n");
        }

        /// <summary>
        /// Validates the data
        /// </summary>
        public virtual bool AddressValidate(ref string Errors, string LineBreak)
        {
            string strRequiredList = "address1,city,statecode,postalcode,countrycode";
            return AddressValidate(ref Errors, "<br>\n", strRequiredList);
        }

        /// <summary>
        /// Validates the data
        /// </summary>
        public virtual bool AddressValidate(ref string Errors, string LineBreak, string RequiredList)
        {
            bool valid = true;
            StringBuilder sb = new StringBuilder();
            RequiredList = RequiredList.ToLower();

            if (StringFunctions.Contains(RequiredList, "address1") || StringFunctions.Contains(RequiredList, "address"))
            {
                if (Address1 == string.Empty)
                {
                    valid = false;
                    sb.Append("Must fill out address line 1." + LineBreak);
                }
            }

            if (StringFunctions.Contains(RequiredList, "city"))
            {
                if (_strCity == string.Empty)
                {
                    valid = false;
                    sb.Append("Must fill out city." + LineBreak);
                }
            }

            if (StringFunctions.Contains(RequiredList, "statecode") || StringFunctions.Contains(RequiredList, "state"))
            {
                if (_strStateCode == string.Empty)
                {
                    valid = false;
                    sb.Append("Must enter State/Province." + LineBreak);
                }
            }

            if (StringFunctions.Contains(RequiredList, "postalcode") || StringFunctions.Contains(RequiredList, "postal"))
            {
                if (_strPostalCode == string.Empty)
                {
                    valid = false;
                    sb.Append("Must fill out postal code." + LineBreak);
                }
            }

            if (StringFunctions.Contains(RequiredList, "countrycode") || StringFunctions.Contains(RequiredList, "country"))
            {
                if (_strCountryCode == string.Empty)
                {
                    valid = false;
                    sb.Append("Must fill out country code." + LineBreak);
                }
            }

            Errors += sb.ToString();
            return valid;
        }

        #endregion
    }
}
