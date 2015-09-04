using System;
using General;
using System.Data;
using System.Data.OleDb;
using General.Utilities.Text;
using General.Data;

namespace General.Utilities.Data.Conversion
{
	/// <summary>
	/// Summary description for oledb.
	/// </summary>
	public class oledb
	{
		/// <summary>
		/// Summary description for oledb.
		/// </summary>
		public oledb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		///	Transfers the contents of a DataSet to an OLEDB destination
		/// </summary>
		public static void insert_dataset(DataSet ds, OleDbCommand ocmd)
		{
			int i = 0;
			foreach(DataTable tbl in ds.Tables)
			{
				i++;
				insert_table(tbl,ocmd,i);
			}
		}

		/// <summary>
		///	Transfers the contents of a DataTable to an OLEDB destination
		/// </summary>
		public static void insert_table(DataTable tbl, OleDbCommand ocmd)
		{
			insert_table(tbl,ocmd,1);
		}

		/// <summary>
		///	Transfers the contents of a DataView to an OLEDB destination
		/// </summary>
		public static void insert_table(DataView dv, OleDbCommand ocmd)
		{
			insert_table(dv.Table,ocmd,1);
		}

		private static void insert_table(DataTable tbl, OleDbCommand ocmd, int i)
		{

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

			bool visible;
			int temp_max;
			string sql = "";
			string field_list = "";
			//WRITE CREATE TABLE STATEMENT
			string tbl_name = tbl.TableName.Trim();
			if(tbl_name.ToLower() == "table")
				tbl_name = "Page_" + i;
			tbl_name = tbl_name.Replace(" ","_");
            tbl_name = tbl_name.Replace(",", "");
			sql += "CREATE TABLE " + tbl_name + " (";
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
					switch(col.DataType.Name)
					{
						case "DateTime":
							sql += "" + col.ColumnName + " " + "DATETIME" + ",";
							break;
						case "Boolean":
							sql += "" + col.ColumnName + " " + "BIT" + ",";
							break;
						case "Decimal":
						case "Double":
						case "Single":
						case "Int32":
						case "UInt32":
						case "Int64":
						case "UInt64":
						case "Int16":
						case "UInt16":
							sql += "" + col.ColumnName + " " + "NUMBER" + ",";
							break;
						case "Char":
							sql += "" + col.ColumnName + " " + "VARCHAR(1)" + ",";
							break;
						case "Byte":
						case "SByte":
						case "String":
							temp_max = get_max_length(tbl,col);
							if(temp_max >= 256)
								sql += "" + col.ColumnName + " " + "MEMO" + ",";
							else if(temp_max == 1)
								sql += "" + col.ColumnName + " " + "VARCHAR(1)" + ",";
							else
								sql += "" + col.ColumnName + " " + "VARCHAR(255)" + ",";
							break;
						default:
							temp_max = get_max_length(tbl,col);
							if(temp_max >= 256)
								sql += "" + col.ColumnName + " " + "MEMO" + ",";
							else if(temp_max == 1)
								sql += "" + col.ColumnName + " " + "VARCHAR(1)" + ",";
							else
								sql += "" + col.ColumnName + " " + "VARCHAR(255)" + ",";
							break;
					}
					field_list += col.ColumnName + ",";
				}
			}
			sql = StringFunctions.Shave(sql,1);
			field_list = StringFunctions.Shave(field_list,1);
			sql += ")";
			ocmd.CommandText = sql;
			//System.Web.HttpContext.Current.Response.Write(sql + "<br><br>");
			ocmd.ExecuteNonQuery();
			sql = "";
			foreach(DataRow row in tbl.Rows)
			{
				sql += "INSERT INTO " + tbl_name + " (" + field_list + ") VALUES (";
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
						//System.Web.HttpContext.Current.Response.Write(col.DataType.Name);
						//System.Web.HttpContext.Current.Response.Flush();
						switch(col.DataType.Name)
						{
							case "DateTime":
								DateTime dt = SqlConvert.ToDateTime(row[col]);
								if(dt.Year == 1900)
									sql += "null" + ",";
								else
									sql += "'" + dt.ToString() + "'" + ",";
								break;
							case "Boolean":
								if(row[col] != DBNull.Value)
									sql += "" + row[col].ToString() + "" + ",";
								else
									sql += "False" + ",";
								break;
							case "Decimal":
							case "Double":
							case "Single":
							case "Int32":
							case "UInt32":
							case "Int64":
							case "UInt64":
							case "Int16":
							case "UInt16":
								sql += "" + SqlConvert.ToNumber(row[col]).ToString() + "" + ",";
								break;
							case "Byte":
							case "SByte":
							case "Char":
							case "String":
								sql += "\"" + SqlConvert.Parse(row[col].ToString().Replace("\"","")) + "\"" + ",";
								break;
							default:
                                sql += "\"" + SqlConvert.Parse(row[col].ToString().Replace("\"", "")) + "\"" + ",";
								break;
						}
					}
				}
				sql = StringFunctions.Shave(sql,1);
				sql += ")";
				ocmd.CommandText = sql;
				//System.Web.HttpContext.Current.Response.Write(sql + "<br><br>");
				//System.Web.HttpContext.Current.Response.Flush();
				ocmd.ExecuteNonQuery();
				sql = "";
			}
		}

		private static int get_max_length(DataTable tbl,DataColumn col)
		{
			int max = 0;
			foreach(DataRow row in tbl.Rows)
			{
				if(row[col].ToString().Length > max)
					max = row[col].ToString().Length;
			}
			return(max);
		}
	}
}
