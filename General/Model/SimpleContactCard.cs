using System;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using General;

namespace General.Model
{
    /// <summary>
    /// A SimpleContactCard is an object that contains a PostalAddress + additional fields such as Name, Phone, Email, etc.
    /// </summary>
    [Serializable, DataContract]
	public class SimpleContactCard : PostalAddress
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

			if(base.Validate(ref err, LineBreak, RequiredList) == false)
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
		/// Returns a string formatted ContactCard with html line breaks 
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

			sb.Append(FirstName);
			if(FirstName != "") sb.Append(" ");
			
			sb.Append(LastName);
			if(sb.Length > 0) sb.Append(LineBreak);
			
			sb.Append(base.ToString(LineBreak));
			
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
