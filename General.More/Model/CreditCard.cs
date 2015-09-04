using System;
using System.Collections;
using System.Text;

namespace General {
	/// <summary>
	/// General definition for a Credit Card. Testing.. This has been changed 10/6/11
	/// </summary>
	[Serializable]
	public class CreditCard
    {

        #region CleanNumber
        public static string CleanNumber(string CardNumber)
        {
            CardNumber = CardNumber.Trim();
            if (CardNumber.Contains("-"))
                CardNumber = CardNumber.Replace("-", ""); //Fix dashes
            if (CardNumber.Contains(" "))
                CardNumber = CardNumber.Replace(" ", ""); //Fix spaces
            return CardNumber;
        }
        #endregion


        #region Private Variables
        private string _strCardName;
		private CreditCardType _objCardType;
		private string _strCardNumber;
		private string _strCardCVV2;
		private int _intCardExpirationMonth;
		private int _intCardExpirationYear;
		private CreditCardResponse _objResponse;
		#endregion

		#region Public Constructors
		/// <summary>
		/// Creates a new Credit Card. All properties must be added
		/// individually.
		/// </summary>
		public CreditCard() { }
		#endregion
	  
		#region Public Properties
		public string Name { set { _strCardName = value; } get { return _strCardName; } }
		public CreditCardType CardType { set { _objCardType = value; } get { return _objCardType; } }
		public string CardNumber { set { _strCardNumber = value; } get { return CleanNumber(_strCardNumber); } }
		public string CardNumberHash { get { return GetCardNumberHash(); } }
		public string CVV2 { set { _strCardCVV2 = value; } get { return _strCardCVV2; } }
		public int ExpirationMonth { set { _intCardExpirationMonth = value; } get { return _intCardExpirationMonth; } }
		public int ExpirationYear { set { _intCardExpirationYear = value; } get { return _intCardExpirationYear; } }
		public CreditCardResponse Response { set { _objResponse = value; } get { return _objResponse; } }
		#endregion
	  
		#region Public Methods
		/// <summary>
		/// Gets the card type description based off of the card type enumeration set to the instance.
		/// </summary>
		/// <returns>string</returns>
		public string GetCardType() { return _GetCardType(); }
		
		/// <summary>
		/// Gets the CreditCardType based off of the string that is passed in.
		/// </summary>
		/// <param name="strCardType">string - A valid CreditCardType Description</param>
		/// <returns>CreditCardType</returns>
		public static CreditCardType GetCardType(string strCardType) { return _GetCardType(strCardType); }
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string Validate() { return _Validate(); }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool Validate(ref ArrayList Errors) { return _Validate(ref Errors); }
		#endregion
       
		#region Private Methods
		#region GetCardType
		// These methods are based off the enumeration defined below. If that changes
		// then these have to be changed to accommodate the change. If we can come up
		// with a better way to do this, that would be the BOMB!!
		private string _GetCardType() {
			switch(_objCardType) {
				case CreditCardType.Visa: return "VISA";
				case CreditCardType.Mastercard: return "MASTERCARD";
				case CreditCardType.AmericanExpress: return "AMEX";
				case CreditCardType.Discover: return "DISCOVER";
				case CreditCardType.CarCareOne: return "CC1";
				case CreditCardType.MatrixPlus: return "MatrixPlus";
				case CreditCardType.BillMeLater: return "BML";
				default: return _objCardType.ToString();
			}
		}
		
		private static CreditCardType _GetCardType(string strCardType) {
			switch(strCardType.ToUpper()) {
				case "VISA": return CreditCardType.Visa;
				case "MASTERCARD": return CreditCardType.Mastercard;
				case "AMEX": return CreditCardType.AmericanExpress;
				case "DISCOVER": return CreditCardType.Discover;
				case "CC1": case "CARCAREONE": return CreditCardType.CarCareOne;
				case "MatrixPlus": return CreditCardType.MatrixPlus;
				case "BML": return CreditCardType.BillMeLater;
				default: return CreditCardType.Other;
			}
		}
		#endregion
		
		#region Validation
		private string _Validate() 
		{
			StringBuilder sb = new StringBuilder();
      
			// Preliminary Validation. Just make sure everything has a value.
			if (_objCardType == 0) sb.Append("<br> - Credit Card Type is required.");
			if (_strCardName == "" || _strCardName == null) sb.Append("<br> - Name on Card is required.");
			if (_strCardNumber == "" || _strCardNumber == null) sb.Append("<br> - Credit Card Number is required.");
			if (!ValidateCardNumber(_strCardNumber)) sb.Append("<br> - Credit Card Number is invalid.");
			if (_intCardExpirationMonth < 1) sb.Append("<br> - Expiration Month is required.");
			if (_intCardExpirationYear < 1) sb.Append("<br> - Expiration Year is required.");
			//if (_strCardCVV2 == "") sb.Append("<br> - CVV2 Number is required.");
      
			// Secondary Validation. Check for formatting.
			// come back and finish this.
      
			return sb.ToString();
		}

		private bool _Validate(ref ArrayList Errors)
		{
			bool result = true;
			if(Errors == null)
				Errors = new ArrayList();

			if (_objCardType == 0) { Errors.Add("Credit Card Type is required."); result = false; }
			if (_strCardName == "" || _strCardName == null) { Errors.Add("Name on Card is required."); result = false; }
			if (_strCardNumber == "" || _strCardNumber == null) { Errors.Add("Credit Card Number is required."); result = false; }
			if (!ValidateCardNumber(_strCardNumber)) { Errors.Add("Credit Card Number is invalid."); result = false; }
			if (_intCardExpirationMonth < 1) { Errors.Add("Expiration Month is required."); result = false; }
			if (_intCardExpirationYear < 1) { Errors.Add("Expiration Year is required."); result = false; }

			return result;
		}

		#region ValidateCardNumber MOD10 Check
		private static bool ValidateCardNumber( string cardNumber )
		{
			try 
			{
				// Array to contain individual numbers
				ArrayList CheckNumbers = new ArrayList();
				// So, get length of card
				int CardLength = cardNumber.Length;
    
				// Double the value of alternate digits, starting with the second digit
				// from the right, i.e. back to front.
				// Loop through starting at the end
				for (int i = CardLength-2; i >= 0; i = i - 2)
				{
					// Now read the contents at each index, this
					// can then be stored as an array of integers

					// Double the number returned
					CheckNumbers.Add( Int32.Parse(cardNumber[i].ToString())*2 );
				}

				int CheckSum = 0;    // Will hold the total sum of all checksum digits
            
				// Second stage, add separate digits of all products
				for (int iCount = 0; iCount <= CheckNumbers.Count-1; iCount++)
				{
					int _count = 0;    // will hold the sum of the digits

					// determine if current number has more than one digit
					if ((int)CheckNumbers[iCount] > 9)
					{
						int _numLength = ((int)CheckNumbers[iCount]).ToString().Length;
						// add count to each digit
						for (int x = 0; x < _numLength; x++)
						{
							_count = _count + Int32.Parse( 
								((int)CheckNumbers[iCount]).ToString()[x].ToString() );
						}
					}
					else
					{
						// single digit, just add it by itself
						_count = (int)CheckNumbers[iCount];   
					}
					CheckSum = CheckSum + _count;    // add sum to the total sum
				}
				// Stage 3, add the unaffected digits
				// Add all the digits that we didn't double still starting from the
				// right but this time we'll start from the rightmost number with 
				// alternating digits
				int OriginalSum = 0;
				for (int y = CardLength-1; y >= 0; y = y - 2)
				{
					OriginalSum = OriginalSum + Int32.Parse(cardNumber[y].ToString());
				}

				// Perform the final calculation, if the sum Mod 10 results in 0 then
				// it's valid, otherwise its false.
				return (((OriginalSum+CheckSum)%10)==0);
			}
			catch
			{
				return false;
			}
		}
		#endregion
		#endregion
		
		private string GetCardNumberHash() {
			if(_strCardNumber.Length > 4)
				return "**** " + StringFunctions.Right(_strCardNumber, 4);
			else
				return _strCardNumber;
		}
		#endregion

		#region ToString
		public override string ToString()
		{
			return ToString("<br>");
		}

		public string ToString(string strLineBreak)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("CardName" + " = " + _strCardName + strLineBreak);
			sb.Append("CardType" + " = " + _objCardType.ToString() + strLineBreak);
			sb.Append("CardNumber" + " = " + _strCardNumber + strLineBreak);
			sb.Append("CardCVV2" + " = " + _strCardCVV2 + strLineBreak);
			sb.Append("CardExpirationMonth" + " = " + _intCardExpirationMonth.ToString() + strLineBreak);
			sb.Append("CardExpirationYear" + " = " + _intCardExpirationYear.ToString() + strLineBreak);
			sb.Append(_objResponse.ToString() + strLineBreak);
			return sb.ToString();
		}

		#endregion

        #region ToPublicDetailString
        public string ToPublicDetailString()
        {
            if (String.IsNullOrEmpty(_strCardNumber) || _strCardNumber.Length < 4)
                return _objCardType.ToString();
            return _objCardType.ToString() + " *" + StringFunctions.Right(_strCardNumber, 4);
        }
        #endregion

        [Serializable]
		public class CreditCardResponse {
			#region Private Variables
			private int _intResponseCode;
			private string _strBMLAccountNumber;
			private string _strAuthCode;
			private string _strDescription;
			private string _strAVSCode;
			private string _strRequestID;
			private string _strCVCode;
			private bool _boolAccepted;
			private bool _boolContactAdmin;
			//private string _strDecision;
			#endregion

			#region Public Constructors
			/// <summary>
			/// Creates a new Credit Card. All properties must be added
			/// individually.
			/// </summary>
			public CreditCardResponse() {}
			#endregion
		
			#region Public Properties
			public int ResponseCode { 
				get { return _intResponseCode; } 
				set { _intResponseCode = value; }
			}
			public string AuthCode { set { _strAuthCode = value; } get { return _strAuthCode; } }
			public string BMLAccountNumber { set { _strBMLAccountNumber = value; } get { return _strBMLAccountNumber; } }
			public string Description { set { _strDescription = value; } get { return _strDescription; } }
			public string AVSCode { set { _strAVSCode = value; } get { return _strAVSCode; } }
			public string CVCode { set { _strCVCode = value; } get { return _strCVCode; } }
			public string RequestID { set { _strRequestID = value; } get { return _strRequestID; } }
			//public string Decision { set { _strDecision = value; } get { return _strDecision; } }
			public bool Accepted { set { _boolAccepted = value; } get { return _boolAccepted; } }
			public bool ContactAdmin { set { _boolContactAdmin = value; } get { return _boolContactAdmin; } }
			#endregion
		
			#region Public Methods

			#endregion
	    
			#region Private Functions

			#endregion

			#region ToString
			public override string ToString()
			{
				return ToString("<br>");
			}

			public string ToString(string strLineBreak)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append("ResponseCode" + " = " + _intResponseCode.ToString() + strLineBreak);
				sb.Append("BMLAccountNumber" + " = " + _strBMLAccountNumber + strLineBreak);
				sb.Append("AuthCode" + " = " + _strAuthCode + strLineBreak);
				sb.Append("Description" + " = " + _strDescription + strLineBreak);
				sb.Append("AVSCode" + " = " + _strAVSCode + strLineBreak);
				sb.Append("RequestID" + " = " + _strRequestID + strLineBreak);
				sb.Append("CVCode" + " = " + _strCVCode + strLineBreak);
				sb.Append("Accepted" + " = " + _boolAccepted.ToString() + strLineBreak);
				sb.Append("ContactAdmin" + " = " + _boolContactAdmin.ToString() + strLineBreak);
				return sb.ToString();
			}

			#endregion
		}

		#region ENUM CreditCardType
		/// <summary>
		/// *** READ THIS IF YOU ARE CHANGING THIS ENUMERATION ***
		/// 
		/// This enumeration defines accepted payment types. If this enumeration changes, there
		/// are two methods above under "GetCardType" that must be changed to match as well.
		/// </summary>
		[Serializable]
		public enum CreditCardType : int {
			Other = 0,
			Visa = 1,
			Mastercard = 2,
			AmericanExpress = 3,
			Discover = 4,
			CarCareOne = 5,
			MatrixPlus = 6,
			BillMeLater = 7
		}
		#endregion

	}
}
