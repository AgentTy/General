using System;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using General;

namespace General.Model
{
	/// <summary>
    /// An AddressBookEntry is an object that contains a PostalAddress + additional fields such as Name, Phone, Email, etc.
	/// </summary>
	[Serializable, DataContract]
	public class AddressBookEntry : PostalAddress
	{

		#region Private Variables
		/// <summary>
		/// First Name
		/// </summary>
		protected string _strFirstName;
		/// <summary>
		/// Last Name
		/// </summary>
		protected string _strLastName;
		/// <summary>
		/// Company
		/// </summary>
		protected string _strCompany;
		/// <summary>
		/// Phone
		/// </summary>
		protected PhoneNumber _objPhone;
		/// <summary>
		/// Fax
		/// </summary>
		protected PhoneNumber _objFax;
		/// <summary>
		/// Email
		/// </summary>
		protected EmailAddress _objEmail;
		/// <summary>
		/// URL
		/// </summary>
		protected URL _objURL;
		#endregion

		#region Constructors

		/// <summary>
		/// Creates a blank AddressBookEntry
		/// </summary>
		public AddressBookEntry()
		{

		}

		/// <summary>
		/// Creates a new AddressBookEntry
		/// </summary>
		public AddressBookEntry(string FirstName, string LastName, PhoneNumber Phone, EmailAddress Email, string Reference,string Address1,string Address2,string Address3,string City,string StateCode,string PostalCode,string CountryCode) : base(Reference, Address1, Address2, Address3, City, StateCode, PostalCode, CountryCode)
		{
			_strFirstName = FirstName;
			_strLastName = LastName;
			_objPhone = Phone;
			_objEmail = Email;
		}

		#endregion

		#region Public Properties
		/// <summary>
		/// First Name
		/// </summary>
        [DataMember]
		public string FirstName
		{
			get { return _strFirstName; }
			set { _strFirstName = PokeSeal(_strFirstName,value); }
		}

		/// <summary>
		/// Last Name
		/// </summary>
        [DataMember]
        public string LastName
		{
			get { return _strLastName; }
			set { _strLastName = PokeSeal(_strLastName,value); }
		}

		/// <summary>
		/// Company
		/// </summary>
        [DataMember]
        public virtual string Company 
		{ 
			get { return _strCompany; } 
			set { _strCompany = PokeSeal(_strCompany,value); } 
		}

        /// <summary>
        /// Company
        /// </summary>
        public virtual string CompanyURLSafe
        {
            get
            {
                return StringFunctions.MakeNameURLSafe(_strCompany);
            }
        }

        /// <summary>
        /// Company With Email
        /// </summary>
        public virtual string CompanyWithEmail
        {
            get {
                if (_objEmail != null && _objEmail.Valid)
                    return _strCompany + "<br/><i><small>" + _objEmail.ToString() + "</small></i>";
                return _strCompany; 
            }
        }

		/// <summary>
		/// Phone
		/// </summary>
        [DataMember]
		public PhoneNumber Phone
		{
			get { return _objPhone; }
			set { _objPhone = PokeSeal(_objPhone,value); }
		}

		/// <summary>
		/// Fax
		/// </summary>
        [DataMember]
		public PhoneNumber Fax
		{
			get { return _objFax; }
			set { _objFax = PokeSeal(_objFax,value); }
		}

		/// <summary>
		/// Email
		/// </summary>
        [DataMember]
		public EmailAddress Email
		{
			get { return _objEmail; }
			set { _objEmail = PokeSeal(_objEmail,value); }
		}

        /// <summary>
        /// Email
        /// </summary>
        public string EmailDataGridSafe
        {
            get 
            {
                if (_objEmail != null)
                    return _objEmail.ToString();
                return String.Empty;
            }
        }

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

		/// <summary>
		/// URL
		/// </summary>
        [DataMember]
		public virtual URL URL 
		{ 
			get { return _objURL; } 
			set { _objURL = PokeSeal(_objURL,value); } 
		}

		/// <summary>
		/// Outputs HTML String
		/// </summary>
		public string HTMLString
		{
			get { return ToHTMLString(); }
		}

		/// <summary>
		/// Outputs HTML String With Postal Address Data Only
		/// </summary>
		public string HTMLAddressString
		{
			get { return ToHTMLAddressString(true); }
		}

        /// <summary>
        /// Outputs HTML String With Postal Address Data Only
        /// </summary>
        public string HTMLAddressStringNoName
        {
            get { return ToHTMLAddressString(false); }
        }

        [DataMember]
        public virtual string FullName { get { return GetFullName(); } set { ReadFullName(value); } }

        /// <summary>
        /// Outputs Inline String for Google
        /// </summary>
        public override string PostalAddressGoogleString
        {
            //get { return Company + " " + base.PostalAddressGoogleString; }
            get { return HttpUtility.UrlEncode(base.ToHTMLString(",").TrimEnd(new [] {','})); }
        }

		#endregion 

		#region Public Methods

        /// <summary>
        /// Validates the data
        /// </summary>
        public bool ValidateAddress(ref string Errors)
        {
            return base.Validate(ref Errors, "<br>\n");
        }

		/// <summary>
		/// Validates the data
		/// </summary>
		public override bool Validate(ref string Errors)
		{
			return Validate(ref Errors, "<br>\n");
		}

		/// <summary>
		/// Validates the data
		/// </summary>
		public override bool Validate(ref string Errors, string LineBreak)
		{
			string strRequiredList = "firstname,lastname,phone,fax,email,address1,city,statecode,postalcode,countrycode"; 
			return Validate(ref Errors, "<br>\n",strRequiredList);
		}

		/// <summary>
		/// Validates the data
		/// </summary>
		public override bool Validate(ref string Errors, string LineBreak, string RequiredList)
		{
			StringBuilder sb = new StringBuilder();
			
			bool valid = true;
			RequiredList = RequiredList.ToLower();

			if(StringFunctions.Contains(RequiredList,"firstname") || StringFunctions.Contains(RequiredList,"name"))
			{
				if(_strFirstName == string.Empty)
				{
					valid = false;
					sb.Append("Must fill out first name." + LineBreak);
				}
			}

			if(StringFunctions.Contains(RequiredList,"lastname") || StringFunctions.Contains(RequiredList,"name"))
			{
				if(_strLastName == string.Empty)
				{
					valid = false;
					sb.Append("Must fill out last name." + LineBreak);
				}
			}

			if(StringFunctions.Contains(RequiredList,"company") || StringFunctions.Contains(RequiredList,"companyname"))
			{
				if(_strCompany == string.Empty)
				{
					valid = false;
					sb.Append("Must fill out company." + LineBreak);
				}
			}

			if(StringFunctions.Contains(RequiredList,"phone") || StringFunctions.Contains(RequiredList,"phonenumber"))
			{
				if(_objPhone == null)
				{
					valid = false;
					sb.Append("Must fill out phone number." + LineBreak);
				}
				else if(!_objPhone.Valid)
				{
					valid = false;
					sb.Append("Must enter valid phone number." + LineBreak);
				}
			}


			if(StringFunctions.Contains(RequiredList,"fax") || StringFunctions.Contains(RequiredList,"faxnumber"))
			{
				if(_objFax == null)
				{
					valid = false;
					sb.Append("Must fill out fax number." + LineBreak);
				}
				else if(!_objFax.Valid)
				{
					valid = false;
					sb.Append("Must enter valid fax number." + LineBreak);
				}
			}

			if(StringFunctions.Contains(RequiredList,"email") || StringFunctions.Contains(RequiredList,"emailaddress"))
			{
				if(_objEmail == null)
				{
					valid = false;
					sb.Append("Must fill out email address." + LineBreak);
				}
				else if(!_objEmail.Valid)
				{
					valid = false;
					sb.Append("Must enter valid email address." + LineBreak);
				}
			}
			
			if(StringFunctions.Contains(RequiredList,"url") || StringFunctions.Contains(RequiredList,"website"))
			{
				if(_objURL == null)
				{
					valid = false;
					sb.Append("Must fill out URL." + LineBreak);
				}
				else if(!_objURL.Valid)
				{
					valid = false;
					sb.Append("Must enter valid URL." + LineBreak);
				}
			}
			
			string err = sb.ToString();

			if(base.Validate(ref err, LineBreak, RequiredList) == false)
				valid = false;

			Errors += err;
			return valid;
		}
		#endregion

		#region Private Functions
		private string GetFullName() 
		{
			return (_strFirstName + " " + _strLastName).Trim();
		}

        private void ReadFullName(string strFullName)
        {
            if (!StringFunctions.IsNullOrWhiteSpace(strFullName))
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
		/// <summary>
		/// Returns a string formatted AddressBookEntry with html line breaks 
		/// </summary>
		public override string ToString()
		{
			return(ToString("<br>"));
		}

        public override string ToString(string LineBreak)
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
		public new string ToHTMLString(string LineBreak)
		{
			StringBuilder sb = new StringBuilder();
			
			
			if(!StringFunctions.IsNullOrWhiteSpace(_strCompany)) sb.Append(_strCompany + LineBreak);

			sb.Append(_strFirstName);
			if(_strFirstName != "") sb.Append(" ");
			
			sb.Append(LastName);
			if(sb.Length > 0) sb.Append(LineBreak);
			
			sb.Append(base.ToString(LineBreak));
			
			if(_objPhone != null)
			{
				if (_objPhone.Valid) 
				{
					sb.Append(_objPhone.ToString());
					sb.Append(LineBreak);
				}
			}

			if(_objFax != null)
			{
				if (_objFax.Valid) 
				{
					sb.Append("Fax: " + _objFax.ToString());
					sb.Append(LineBreak);
				}
			}
			
			if(_objEmail != null)
			{
				if(_objEmail.Valid) 
				{
					if(StringFunctions.Contains(LineBreak,"<br"))
						sb.Append(_objEmail.ToLink());
					else
						sb.Append(_objEmail.ToString());
					sb.Append(LineBreak);
				}
			}
			
			if(_objURL != null)
			{
				if(_objURL.Valid) 
				{
					if(StringFunctions.Contains(LineBreak,"<br"))
						sb.Append(_objURL.ToLink());
					else
						sb.Append(_objURL.ToString());
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
                sb.Append(_strFirstName);
                if (_strFirstName != "") sb.Append(" ");

                sb.Append(LastName);
                if (sb.Length > 0) sb.Append(LineBreak);
            }

			sb.Append(base.ToString(LineBreak));
			return sb.ToString();
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
		/// Compares two AddressBookEntry objects
		/// </summary>
		public static bool operator ==(AddressBookEntry Address1, AddressBookEntry Address2)
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
		public static bool operator !=(AddressBookEntry Address1, AddressBookEntry Address2)
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
            if (obj.GetType() != typeof(AddressBookEntry))
                return false;
			return(this==(AddressBookEntry) obj);
		}

		/// <summary>
		/// Returns a HashCode for this AddressBookEntry object
		/// </summary>
		public override int GetHashCode()
		{
			return(this.ToString().GetHashCode());
		}

		#endregion

	}
}
