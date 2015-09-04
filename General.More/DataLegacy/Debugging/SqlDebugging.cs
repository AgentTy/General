using System;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace General.DAO.Debugging {
  /// <summary>
  /// OutputMode describing how to display the output information.
  /// </summary>
  public enum OutputMode : int {
    /// <summary>
    /// Outputs the SqlCommand attributes using Trace.Write.
    /// </summary>
    Trace = 0,

    /// <summary>
    /// Outputs the SqlCommand attributes using Response.Write.
    /// </summary>
    Response = 1,

    /// <summary>
    /// Throws a new Exception with the SqlCommand output.
    /// </summary>
    Exception = 2
  }
  
	/// <summary>
	/// Debugging tools specific to the SqlClient Objects.
	/// </summary>
	public class SqlDebugging {

		/// <summary>
		/// Outputs attributes of a SqlCommand.
		/// </summary>
		/// <param name="cmd">SqlCommand</param>
		/// <param name="om">OutputMode</param>
		public static void OutputCommand(SqlCommand cmd, OutputMode om) {
		  StringBuilder sbOutput = new StringBuilder();
		  
		  sbOutput.Append("Outputting \"" +cmd.CommandText+ "\":");
		  sbOutput.Append(System.Environment.NewLine);
		  
		  foreach (SqlParameter param in cmd.Parameters) {
		    sbOutput.Append(param.ParameterName+ " = ");

		    if (IsText(param.SqlDbType)) sbOutput.Append("'");
		    sbOutput.Append(param.Value);
		    if (IsText(param.SqlDbType)) sbOutput.Append("'");

		    sbOutput.Append(System.Environment.NewLine);
		  }
		  
		  _OutputCommand(sbOutput.ToString(), om);
		}
		
		/// <summary>
		/// Responsible for the actual output to the display.
		/// </summary>
		/// <param name="strOutput">string</param>
		/// <param name="om">OutputMode</param>
		private static void _OutputCommand(string strOutput, OutputMode om) {
		  switch (om) {
		    case OutputMode.Trace:
		      HttpContext.Current.Trace.Write(strOutput);
		    break;
		    case OutputMode.Response:
		      HttpContext.Current.Response.Write(strOutput);
		    break;
		    
		    // This needs to be the last case switch because there is no break;
		    case OutputMode.Exception:
		      throw new Exception(strOutput);
		  }
		}
		
		private static bool IsText(System.Data.SqlDbType type) {
		  switch(type) {
		    case System.Data.SqlDbType.VarChar:
		    case System.Data.SqlDbType.Text:
		    case System.Data.SqlDbType.NVarChar:
		    case System.Data.SqlDbType.NText:
		    case System.Data.SqlDbType.NChar:
		    case System.Data.SqlDbType.Char:
		    case System.Data.SqlDbType.DateTime:
		    case System.Data.SqlDbType.SmallDateTime:
		    case System.Data.SqlDbType.Timestamp:
		    case System.Data.SqlDbType.UniqueIdentifier:
		      return true;
		    
		    default:
		      return false;
		  }
		}
	}
}
