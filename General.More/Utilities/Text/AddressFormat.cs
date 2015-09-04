using System.Text;

namespace General.Utilities.Text {
	/// <summary>
	/// Text tools specific to address formatting.
	/// </summary>
	public class AddressFormat {
	  private AddressFormat() {}
	  
	  /// <summary>
	  /// Creates a Full Name out of a First, Last and Middle name.
	  /// </summary>
	  /// <param name="strFirstName">string - First Name</param>
	  /// <param name="strLastName">string - Last Name</param>
	  /// <param name="strMiddle">string - Middle Name or Initial</param>
	  /// <returns>string</returns>
	  public static string FullName(string strFirstName, string strLastName, string strMiddle) {
	    StringBuilder sbName = new StringBuilder();
	    
	    // Append the first name
	    if (strFirstName != "") sbName.Append(strFirstName);
	    if (strFirstName != "" && strLastName != "") sbName.Append(" ");
	    
	    // Append the middle name
	    if (strMiddle != "") {
	      sbName.Append(strMiddle);
	      if (strMiddle.Length == 1) sbName.Append("."); // Adds a "." if this is an initial.
	      sbName.Append(" ");
	    }
	    
	    // Append the last name
	    if (strLastName != "") sbName.Append(strLastName);
	    
	    return sbName.ToString();
	  }
	  
	  #region FullName Overloads
	  /// <summary>
	  /// Creates a Full Name out of a First, Last.
	  /// </summary>
	  /// <param name="strFirstName">string - First Name</param>
	  /// <param name="strLastName">string - Last Name</param>
	  /// <returns>string</returns>
	  public static string FullName(string strFirstName, string strLastName) {
	    return FullName(strFirstName, strLastName, "");
	  }
	  #endregion
	}
}
