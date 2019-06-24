using System;
using System.Data;
using General;
using General.Data;

namespace General.Utilities.Data.Conversion
{
	/// <summary>
	/// Summary description for text_tools.
	/// </summary>
	public class text_tools
	{
		/// <summary>
		/// Summary description for text_tools.
		/// </summary>
		public text_tools()
		{

		}

		/// <summary>
		/// Create text delimited file from DataSet
		/// </summary>
		public static void create_txt_file(DataSet ds, string destination)
		{
			create_txt_file(ds,destination,txt_data_type.csv,"\"",true); //DEFAULT SETTING COMMA SEPERATED WITH QUOTES AROUND EACH CELL
		}

		/// <summary>
		/// Create text delimited file from DataSet
		/// </summary>
		public static void create_txt_file(DataSet ds, string destination, txt_data_type type, string txt_qualifier, bool output_field_names)
		{
			if(SqlConvert.ToString(destination) == "")
				throw new Err("You must specify the destination location and filename of the xls file to create.");

			System.Text.StringBuilder sh = new System.Text.StringBuilder();
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			bool visible;
			DataTable tbl = ds.Tables[0];

            #region Set Export Visible Properties
            try
            {
                foreach (DataColumn col in tbl.Columns)
                {
                    string strColName = col.ColumnName.ToLower();
                    if (strColName == "password" || strColName == "pass" || strColName == "exhibitorpassword" || strColName == "administratorpassword" || strColName == "customerpassword")
                        col.ExtendedProperties["export_visible"] = false;
                    if (strColName == "username" || strColName == "user" || strColName == "exhibitorusername" || strColName == "administratorusername" || strColName == "customerusername")
                        col.ExtendedProperties["export_visible"] = false;
                }
            }
            catch (Exception ex)
            {
                General.Debugging.Report.SendError("Error Setting ExportVisible property in DataTable", ex);
            }
            #endregion

			if(output_field_names)
			{
				foreach(DataColumn col in tbl.Columns)
				{
					try
					{
						visible = (bool) col.ExtendedProperties["export_visible"];
					}
					catch
					{
						visible = true;
					}
					if(visible)
					{
						if(txt_qualifier != null) sh.Append(txt_qualifier);
						sh.Append(col.ColumnName);
						if(txt_qualifier != null) sh.Append(txt_qualifier);
						sh.Append(seperator(type));
					}
				}
				if(sh.Length > 0) sh.Remove(sh.Length - seperator(type).Length,seperator(type).Length);
				sh.Append("\r\n");
			}

			foreach(DataRow row in tbl.Rows)
			{
				foreach(DataColumn col in tbl.Columns)
				{
					try
					{
						visible = (bool) col.ExtendedProperties["export_visible"];
					}
					catch
					{
						visible = true;
					}
					if(visible)
					{
						if(txt_qualifier != null) sb.Append(txt_qualifier);
						sb.Append(row[col].ToString());
						if(txt_qualifier != null) sb.Append(txt_qualifier);
						sb.Append(seperator(type));
					}
				}
				if(sb.Length > 0) sb.Remove(sb.Length - seperator(type).Length,seperator(type).Length);
				sb.Append("\r\n");
			}

			System.IO.StreamWriter sw = new System.IO.StreamWriter(destination,false);
			if(output_field_names) sw.Write(sh.ToString());
			sw.Write(sb.ToString());
			sw.Close();
		}

		private static string seperator(txt_data_type type)
		{
			switch(type)
			{
				case txt_data_type.csv:
					return(",");
				case txt_data_type.tab:
					return("\t");
				default:
					return(",");
			}
		}
	}

	/// <summary>
	/// Text delimited data types
	/// </summary>
	public enum txt_data_type:int
	{
		/// <summary>
		/// Comma seperated values
		/// </summary>
		csv = 1,
		/// <summary>
		/// Tab seperated values
		/// </summary>
		tab = 2
	}
}
