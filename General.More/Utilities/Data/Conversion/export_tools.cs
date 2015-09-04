using System;
using System.Data;
using VM.xPort;

namespace General.Utilities.Data.Conversion
{
	/// <summary>
	/// Summary description for export_tools.
	/// </summary>
	public class export_tools
	{
		/// <summary>
		/// export_tools.
		/// </summary>
		public export_tools()
		{

		}



		/// <summary>
		/// export_data
		/// </summary>
		public static string export_data(data_table_export_format format, DataSet ds, string temp_directory)
		{
			string destination;
			string file_name;
			Random rnd = new Random(DateTime.Now.Millisecond);
			file_name = rnd.Next(10000,1000000000) + "." + format.ToString();
			destination = temp_directory + "\\" + file_name;

			if(format == data_table_export_format.xls)
            {

                #region Check Table Name For Excel Length Limit of 31 Characters
                foreach (DataTable objTable in ds.Tables)
                    if (objTable.TableName.Length > 31)
                        objTable.TableName = StringFunctions.Right(objTable.TableName, 31);
                #endregion

                #region Remove Secure Fields From Export
                try
                {
                    foreach (DataTable objTable in ds.Tables)
                    {
                    lookagain: foreach (DataColumn col in objTable.Columns)
                        {
                            string strColName = col.ColumnName.ToLower();
                            if (strColName == "password" || strColName == "pass" || strColName == "exhibitorpassword" || strColName == "administratorpassword" || strColName == "customerpassword")
                            {
                                objTable.Columns.Remove(col.ColumnName);
                                goto lookagain;
                            }
                            if (strColName == "username" || strColName == "user" || strColName == "exhibitorusername" || strColName == "administratorusername" || strColName == "customerusername")
                            {
                                objTable.Columns.Remove(col.ColumnName);
                                goto lookagain;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    General.Debugging.Report.SendError("Error removing secured fields in Excel Spreadsheet for export", ex);
                }
                #endregion

                /* THE OLD WAY
		 		string xml = WorkbookEngine.CreateWorkbook(ds);
				System.IO.StreamWriter sw = new System.IO.StreamWriter(destination,false);
				sw.Write(xml);
				sw.Close();
                */

                xpOutputFormat outputFormat;
                outputFormat = xpOutputFormat.Excel8;
                _xportToolsDemo = new DS2XL();

                //Check if we need to optimize export output for smaller export size. If not, then
                //export will be optimized for speed (default). This option works only for export
                //into XLS file format.
                _xportToolsDemo.OptimizeForSize = false;

                try
                {

                    //Export loaded data
                    ExportDataSetIntoResponse(ds, null, outputFormat, true,
                        false, _xportToolsDemo, destination);
                }
                finally
                {
                    ReleaseGlobalResources();
                }

			}
            else if (format == data_table_export_format.xml)
            {
                //Remove Spaces From Column Names
                DataTable objTable = ds.Tables[0];
                foreach (DataColumn objColumn in objTable.Columns)
                {
                    if (objColumn.ColumnName.Contains(" "))
                        objColumn.ColumnName = objColumn.ColumnName.Replace(" ", "");
                }
                ds.WriteXml(destination);
            }
			else if(format == data_table_export_format.mdb)
			{
				access_tools.create_mdb_file(ds,destination,temp_directory + "\\Templates\\template.mdb");
			}
			else if(format == data_table_export_format.csv)
			{
				text_tools.create_txt_file(ds,destination);
			}
			else if(format == data_table_export_format.txt)
			{
				text_tools.create_txt_file(ds,destination,txt_data_type.tab,"",true);
			}

			return(file_name);
		}




        #region Excel xPorter functions
        private static DS2XL _xportToolsDemo = null; //Excel xPorter needs this

        /// <summary>
        /// Export data from source DataSet to browser's response.
        /// </summary>
        /// <param name="datasetToExport">Source DataSet to export data from.</param>
        /// <param name="tablesToExport">List of DataTables inside of DataSet to export data from.</param>
        /// <param name="outputFormat">Output export format.</param>
        /// <param name="exportHeaders">Specifies if we need to export column names.</param>
        /// <param name="exportIntoStream">Specifies if we want to export data into some stream first.</param>
        /// <param name="xportToolsDemo">Exporter class instance.</param>
        /// <remarks></remarks>
        private static void ExportDataSetIntoResponse(DataSet datasetToExport, ExportTable[] tablesToExport,
            xpOutputFormat outputFormat, bool exportHeaders, bool exportIntoStream, DS2XL xportToolsDemo, string strFileName)
        {

            //string tempExportFileName;
            //System.IO.FileInfo outputFile = null;
            //MemoryStream exportStream = null;

            /*
            //Generate temporary file name. 
            tempExportFileName = xPortDemoHelper.GenerateTempFileName(outputFormat);

            if (outputFormat == xpOutputFormat.HTML)
            {
                Response.ContentType = @"text/html";
            }
            else
            {
                Response.ContentType = @"application/vnd.ms-excel";
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=" + tempExportFileName);
            */

            //If you need to export data from all DataTables then use overloaded Export/ExportToStream
            //method that does not require tablesToExport parameter. For example,
            //xportTools.Export(products, "Products", outputFormat, True, chkHeaders.Checked)

            /*
            if (exportIntoStream)
            {

                //For the demo purposes we export into memory stream, but it could be any type of stream.
                //When Export method executed, make sure that DataTable's TableName property is not blank
                //and set to any valid DataTable name. If DataTable does not have name, 'Incorrect table name'
                //exception will be thrown
                exportStream = (MemoryStream)xportToolsDemo.ExportToStream(datasetToExport,
                    tablesToExport, outputFormat, exportHeaders);

                //Do something with stream here. For the demo purposes we store stream content into the response output
                Response.OutputStream.Write(exportStream.ToArray(), 0, (int)exportStream.Length);
                exportStream.Close();
                exportStream = null;
            }
            else
            {
*/
                //Export data into temporary file
            xportToolsDemo.Export(datasetToExport, tablesToExport, strFileName, outputFormat, true, exportHeaders);
                //Output file into client's response
            //outputFile = new System.IO.FileInfo(strFileName);
            /*
                if (outputFile.Exists && Response.IsClientConnected)
                {
                    Response.WriteFile(tempExportFileName);
                }
            */
                //outputFile = null;
            //}

            //Response.Flush();
            //We need to close response to make sure that generated HTML markup from our
            //aspx page does not attach to output Excel file.
            //Response.Close();

        }

        /// <summary>
        /// Closes and releases all resources (variables) declared on page level
        /// </summary>
        /// <remarks></remarks>
        private static void ReleaseGlobalResources()
        {
            //_dataHelper = null;
            _xportToolsDemo.Dispose();
            _xportToolsDemo = null;
        }
        #endregion


    }





    /// <summary>
	/// data_table_export_format
	/// </summary>
	public enum data_table_export_format
	{
		/// <summary>
		/// Excel
		/// </summary>
		xls = 1, //Excel
		/// <summary>
		/// Access
		/// </summary>
		mdb = 2, //Access
		/// <summary>
		/// Comma Delimited
		/// </summary>
		csv = 3, //Comma Delimited
		/// <summary>
		/// Tab Delimited
		/// </summary>
		txt = 4,  //Tab Delimited
        /// <summary>
        /// XML File
        /// </summary>
        xml = 5  //XML File
	}
}
