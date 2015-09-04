/*using System;
using System.Configuration;
using System.Data;
using General;

namespace General.Utilities.Data.Conversion
{
	/// <summary>
	/// Summary description for excel_tools.
	/// </summary>
	public class excel_tools
	{
		/// <summary>
		/// excel_tools
		/// </summary>
		public excel_tools()
		{
			//
			// TODO: Add constructor logic here
			//
			
		}

		/// <summary>
		/// Create excel spreadsheet file
		/// </summary>
		public static void create_xls_file(DataSet ds, string destination, string template)
		{
			/*
			Excel.Application oXL;
			Excel._Workbook oWB;
			Excel._Worksheet oSheet;
			Excel.Range oRng;

			try
			{
				//Start Excel and get Application object.
				oXL = new Excel.Application();
				oXL.Visible = true;

				//Get a new workbook.
				oWB = (Excel._Workbook)(oXL.Workbooks.Add( null ));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;

				//Add table headers going cell by cell.
				oSheet.Cells[1, 1] = "First Name";
				oSheet.Cells[1, 2] = "Last Name";
				oSheet.Cells[1, 3] = "Full Name";
				oSheet.Cells[1, 4] = "Salary";

				//Format A1:D1 as bold, vertical alignment = center.
				oSheet.get_Range("A1", "D1").Font.Bold = true;
				oSheet.get_Range("A1", "D1").VerticalAlignment = 
					Excel.XlVAlign.xlVAlignCenter;
		
				// Create an array to multiple values at once.
				string[,] saNames = new string[5,2];
		
				saNames[ 0, 0] = "John";
				saNames[ 0, 1] = "Smith";
				saNames[ 1, 0] = "Tom";
				saNames[ 1, 1] = "Brown";
				saNames[ 2, 0] = "Sue";
				saNames[ 2, 1] = "Thomas";
				saNames[ 3, 0] = "Jane";
				saNames[ 3, 1] = "Jones";
				saNames[ 4, 0] = "Adam";
				saNames[ 4, 1] = "Johnson";

				//Fill A2:B6 with an array of values (First and Last Names).
				oSheet.get_Range("A2", "B6").Value2 = saNames;

				//Fill C2:C6 with a relative formula (=A2 & " " & B2).
				oRng = oSheet.get_Range("C2", "C6");
				oRng.Formula = "=A2 & \" \" & B2";

				//Fill D2:D6 with a formula(=RAND()*100000) and apply format.
				oRng = oSheet.get_Range("D2", "D6");
				oRng.Formula = "=RAND()*100000";
				oRng.NumberFormat = "$0.00";

				//AutoFit columns A:D.
				oRng = oSheet.get_Range("A1", "D1");
				oRng.EntireColumn.AutoFit();

				//Manipulate a variable number of columns for Quarterly Sales Data.
				//DisplayQuarterlySales(oSheet);

				//Make sure Excel is visible and give the user control
				//of Microsoft Excel's lifetime.
				//oXL.Visible = true;
				//oXL.UserControl = true;
				oXL.Save("C:\\Data\\Test.xls");
			}
			catch( Exception ex ) 
			{
				String errorMessage;
				errorMessage = "Error: ";
				errorMessage = String.Concat( errorMessage, ex.Message );
				errorMessage = String.Concat( errorMessage, " Line: " );
				errorMessage = String.Concat( errorMessage, ex.Source );

				throw new Exception(errorMessage,ex);
			}

			
			*-/
			if(SqlConvert.ToString(template) == "")
				throw new Err("You must specify the location of a template xls file inherit from.");
			if(!System.IO.File.Exists(template))
				throw new Err("The specified template file \"" + template + "\" could not be found.");
			if(SqlConvert.ToString(destination) == "")
				throw new Err("You must specify the destination location and filename of the xls file to create.");

			//COPY THE TEMPLATE AND CREATE DESTINATION FILE
			System.IO.File.Copy(template,destination,true);
			//CONNECT TO THE DESTINATION FILE
			string sconn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + destination + ";Extended Properties=Excel 8.0;";
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
				System.Web.HttpContext.Current.Response.Write(ex.ToString());
			}
			finally
			{
				// Close the connection.
				oconn.Close();
			}

			
		}

	}
}
*/