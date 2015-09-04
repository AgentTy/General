using System;
using System.Data;
using General;
using General.Data;

namespace General.Utilities.Data.Conversion
{
	/// <summary>
	/// Summary description for access_tools.
	/// </summary>
	public class access_tools
	{
		/// <summary>
		/// Summary description for access_tools.
		/// </summary>
		public access_tools()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Create access database file
		/// </summary>
		public static void create_mdb_file(DataSet ds, string destination, string template)
		{
			if(SqlConvert.ToString(template) == "")
				throw new Err("You must specify the location of a template mdb file inherit from.");
			if(!System.IO.File.Exists(template))
				throw new Err("The specified template file \"" + template + "\" could not be found.");
			if(SqlConvert.ToString(destination) == "")
				throw new Err("You must specify the destination location and filename of the mdb file to create.");

			//COPY THE TEMPLATE AND CREATE DESTINATION FILE
			System.IO.File.Copy(template,destination,true);
			//CONNECT TO THE DESTINATION FILE
			string sconn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + destination + ";";
			System.Data.OleDb.OleDbConnection oconn = new System.Data.OleDb.OleDbConnection(sconn);
			oconn.Open();
			System.Data.OleDb.OleDbCommand ocmd = new System.Data.OleDb.OleDbCommand();
			ocmd.Connection = oconn;
			//WRITE TO THE DESTINATION FILE
			try
			{ 
				oledb.insert_dataset(ds,ocmd);
			}
			catch(Exception ex)
			{
				//System.Web.HttpContext.Current.Response.Write(ex.ToString());
                General.Debugging.Report.SendError("Error exporting to MS Access", ex.ToString() + "\r\n\r\n\r\n" + ocmd.CommandText);
			}
			finally
			{
				// Close the connection.
				oconn.Close();
			}
		}
	}
}
