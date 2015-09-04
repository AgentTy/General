
using System;

namespace General.Utilities.Text
{
	//################################################################################################
	//
	// RegxLibrary
	//
	/// <summary>
	/// Contains a library of all Regular Expression used in a site.
	/// </summary>
	//
	//################################################################################################
	public class RegxLibrary
	{
		#region Constructors 
		//*******************************************************
		//
		// RegxLibrary()
		//
		/// <summary>
		/// Default Constructor
		/// </summary>
		//
		//*******************************************************
		public RegxLibrary()
		{
		}
		#endregion

		#region Private level RegExpression Constants 
		//--------------------------------------------------------------
		// Private level constants for Regular Expression validators
		//--------------------------------------------------------------
		const string m_strPhoneUS	= @"^((\(\d{3}\) ?)|(\d{3}[-. ]))?\d{3}[-. ]\d{4}$";
		const string m_strEmailStd	= @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
		const string m_strEmailAdv	= @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$";
		#endregion

		#region Properties 
		//*******************************************************
		//
		// PhoneNumberStdRegExPattern()
		//
		/// <summary>
		/// Gets a regular expression validator for Standard US Phone Numbers
		/// </summary>
		//
		//*******************************************************
		public static string PhoneNumberStdRegExPattern()
		{
			return m_strPhoneUS;
		}

		//*******************************************************
		//
		// EmailStdRegExPattern()
		//
		/// <summary>
		/// Gets a regular expression validator for Email Addresses
		/// </summary>
		//
		//*******************************************************
		public static string EmailStdRegExPattern()
		{
			return m_strEmailStd;
		}

		//*******************************************************
		//
		// EmailAdvancedPattern()
		//
		/// <summary>
		/// Gets a regular expression validator for Email Addresses
		/// </summary>
		/// <remarks>EX: d_n+nic@com.com|||dav@dav.com|||dn@da.v.id.ca</remarks>
		//
		//*******************************************************
		public static string EmailAdvancedPattern()
		{
			return m_strEmailAdv;
		}


        //*******************************************************
        //
        // LocalHostServerPattern()
        //
        /// <summary>
        /// Gets a regular expression that matches local servers such as http://localhost:42234/
        /// </summary>
        /// <remarks>EX: http://localhost:42234, http://localhost:42234/Test.aspx </remarks>
        //
        //*******************************************************
        public static string LocalHostServerPattern()
        {
            return @"http://localhost:\d*/*";
        }
		#endregion
	}
}
