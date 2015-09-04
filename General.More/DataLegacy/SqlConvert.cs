using System;
using System.Collections;
using General.Model;

namespace General.DAO
{
    public class Col<T>
    {
        T t;
        public T Val { get { return t; } set { t = value; } }

        public Col(object obj)
        {
            Val = (T) obj;
        }
    }

	/// <summary>
	/// Conversion functions that default values when catching DBNull
	/// </summary>
	public class SqlConvert
	{
		#region Constructors
		/// <summary>
		/// Conversion functions that default values when catching DBNull
		/// </summary>
		public SqlConvert() {}
		#endregion

        #region ChangeType
        public static object ChangeType(object value, Type type)
        {
            if (value.GetType() == typeof(DBNull) && type == typeof(String))
                return String.Empty;
            else if (value.GetType() == typeof(DBNull))
                return null;

            switch (type.Name)
            {
                case "PhoneNumber":
                    return (PhoneNumber) (string) value;
                case "EmailAddress":
                    return (EmailAddress) (string) value;
                case "URL":
                    return (URL) (string) value;
                case "PostalAddress":
                    return PostalAddress.Deserialize((string) value);
                case "Boolean":
                    value = ((string)value).Replace("0", "false");
                    value = ((string)value).Replace("1", "true");
                    break;
                case "SimpleXMLNameValueCollection":
                    return new SimpleXMLNameValueCollection(SqlConvert.ToXML((string)value));
            }
            try
            {
                return Convert.ChangeType(value, type);
            }
            catch
            {
                if (type == typeof(String))
                    return String.Empty;
                else 
                    return null; 
            }
            
        }
        #endregion

        #region ToSql
        /// <summary>
        /// Convert an object into a sql object
        /// </summary>
        public static object ToSql(object input)
        {
            if (input == null)
                return DBNull.Value;

            switch (input.GetType().Name)
            {
                case "String":
                    return ToSql((String)input);
                case "Int32":
                    return ToSql((Int32)input);
                case "Int16":
                    return ToSql((Int16)input);
                case "Int64":
                    return ToSql((Int64)input);
                case "Double":
                    return ToSql((Double)input);
                case "Decimal":
                    return ToSql((Decimal)input);
                case "Single":
                    return ToSql((Single)input);
                case "Boolean":
                    return ToSql((Boolean)input);
                case "DateTime":
                    return ToSql((DateTime)input);
                case "PhoneNumber":
                    return ToSql((PhoneNumber)input);
                case "EmailAddress":
                    return ToSql((EmailAddress)input);
                case "URL":
                    return ToSql((URL)input);
                case "XMLNameValueCollection":
                    return ToSql((XMLNameValueCollection)input);
                case "SimpleXMLNameValueCollection":
                    return ToSql((SimpleXMLNameValueCollection)input);
                case "PostalAddress":
                    return ToSql((PostalAddress)input);
                case "ArrayList":
                    return ToSql((ArrayList)input);
                default: throw new TypeLoadException("Unhandled type (" + input.GetType().Name + ")");
            }
        }


        /// <summary>
        /// Convert an XMLNameValueCollection into a sql object
        /// </summary>
        public static object ToSql(ArrayList input)
        {
            return ToSql(input, true);
        }

        public static object ToSql(ArrayList input, bool blnStrongDataType)
        {
            if (input == null)
                return DBNull.Value;
            else
            {
                string strData = General.Utilities.Serialization.ArrayList.SerializeArrayList(input, ",",blnStrongDataType);
                if (!StringFunctions.IsNullOrWhiteSpace(strData))
                    return strData;
                else
                    return DBNull.Value;
            }
        }

        /// <summary>
        /// Convert an XMLNameValueCollection into a sql object
        /// </summary>
        public static object ToSql(PostalAddress input)
        {
            if (input == null)
                return DBNull.Value;
            else
                return PostalAddress.Serialize(input);
        }

        /// <summary>
        /// Convert an XMLNameValueCollection into a sql object
        /// </summary>
        public static object ToSql(General.XMLNameValueCollection input)
        {
            if (input == null)
                return DBNull.Value;
            else
                return input.XMLDocument.OuterXml;
        }

        /// <summary>
        /// Convert a SimpleXMLNameValueCollection into a sql object
        /// </summary>
        public static object ToSql(General.SimpleXMLNameValueCollection input)
        {
            if (input == null)
                return DBNull.Value;
            else
                return input.XMLDocument.OuterXml;
        }

        /// <summary>
        /// Convert a string into a PhoneNumber object
        /// </summary>
        public static object ToSql(PhoneNumber input)
        {
            if (input == null)
                return DBNull.Value;
            else
                return input.ToSql();
        }

        /// <summary>
        /// Convert a string into a EmailAddress object
        /// </summary>
        public static object ToSql(EmailAddress input)
        {
            if (input == null)
                return DBNull.Value;
            else
                return input.ToSql();
        }

        /// <summary>
        /// Convert a string into a URL object
        /// </summary>
        public static object ToSql(URL input)
        {
            if (input == null)
                return DBNull.Value;
            else
                return input.ToSql();
        }

		/// <summary>
		/// Convert a string into a sql object
		/// </summary>
		public static object ToSql(string input)
		{
			if(input == null)
				return DBNull.Value;
			else if(input == String.Empty)
				return DBNull.Value;
			else
				return input;
		}

		/// <summary>
		/// Convert an integer into a sql object
		/// </summary>
		public static object ToSql(int input)
		{
			return input;
		}

		/// <summary>
		/// Convert an integer into a sql object
		/// </summary>
		public static object ToSql(int input, int NullValue)
		{
			if(input == NullValue)
				return DBNull.Value;
			else 
				return input;
		}

		
		/// <summary>
		/// Convert an integer into a sql object
		/// </summary>
		public static object ToSql(short input)
		{
			return input;
		}

		/// <summary>
		/// Convert an integer into a sql object
		/// </summary>
		public static object ToSql(long input)
		{
			return input;
		}

		/// <summary>
		/// Convert a double into a sql object
		/// </summary>
		public static object ToSql(double input)
		{
			return input;
		}

        /// <summary>
        /// Convert an double into a sql object
        /// </summary>
        public static object ToSql(double input, double NullValue)
        {
            if (input == NullValue)
                return DBNull.Value;
            else
                return input;
        }

        /// <summary>
        /// Convert a double into a sql object
        /// </summary>
        public static object ToSql(decimal input)
        {
            return input;
        }

		/// <summary>
		/// Convert a double into a sql object
		/// </summary>
		public static object ToSql(Single input)
		{
			return input;
		}

		/// <summary>
		/// Convert an integer into a sql object
		/// </summary>
		public static object ToSql(bool input)
		{
			try
			{
                if (input)
                    return 1;
                else
                    return 0;
			}
			catch
			{
				return DBNull.Value;
			}
		}

        /// <summary>
        /// Convert a ShortGuid into a sql UniqueIdentifier object
        /// </summary>
        public static object ToSql(ShortGuid input)
        {
            if (input == ShortGuid.Empty)
                return DBNull.Value;
            else
                return input.Guid.ToString();
        }

		/// <summary>
		/// Convert an integer into a sql object
		/// </summary>
		public static object ToSqlYesNo(bool input)
		{
			try
			{
                if (input)
                    return "Y";
                else
                    return "N";
			}
			catch
			{
				return DBNull.Value;
			}
		}

        public static object ToSqlBit(bool input)
        {
            return ToSql(input);
        }

		
		/// <summary>
		/// Convert a DataTime into a sql object
		/// </summary>
		public static object ToSql(DateTime input)
		{
			try
			{
                if (input.Date == SqlConvert.ToDateTime(null).Date || input.Date == DateTime.MinValue.Date)
					return DBNull.Value;
				else
					return input;
			}
			catch
			{
				return DBNull.Value;
			}
		}

        /// <summary>
        /// Convert a DataTime into a sql object
        /// </summary>
        public static object ToSql(DateTime input, DateTime NullValue)
        {
            try
            {
                if (input == NullValue)
                    return DBNull.Value;
                else
                    return input;
            }
            catch
            {
                return DBNull.Value;
            }
        }
		#endregion ToSql

		#region Parse
		/// <summary>
		/// Fix quotes for sql query strings
		/// </summary>
		public static string Parse(string input)
		{
			//Change single quote to double quote.
			//THIS WEIRD CRAP IS DESIGNED TO AVOID DUPLICATION OF SINGLE QUOTES IN INSTANCES WHERE Parse IS CALLED MORE THAN ONCE ON THE SAME STRING
			string result = input;
			result = result.Replace("''", "@youshouldntseeme@");
			result = result.Replace("'", "''");
			result = result.Replace("@youshouldntseeme@", "''");
			return(result);
		}
		#endregion

		#region ToDataType
        /// <summary>
        /// Fix quotes for sql query strings
        /// </summary>
        public static ShortGuid ToShortGuid(object obj)
        {
            if (Convert.IsDBNull(obj))
            {
                return ShortGuid.Empty;
            }
            else if (obj == null)
            {
                return ShortGuid.Empty;
            }
            else
            {
                return (ShortGuid) ShortGuid.Encode(obj.ToString());
            }
        }

		/// <summary>
		/// Fix quotes for sql query strings
		/// </summary>
		public static object ToNumber(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(0);
			}
			else if(obj == null)
			{
				return(0);
			}
			else if(obj.ToString().Trim() == String.Empty)
			{
				return(0);
			}
			else
			{
				return(obj);
			}
		}

		/// <summary>
		/// Returns 0 if object IsDBNull
		/// </summary>
		public static Int64 ToInt64(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(0);
			}
			else if(obj == null)
			{
				return(0);
			}
			else if(obj.ToString().Trim() == String.Empty)
			{
				return(0);
			}
			else
			{
				return(Convert.ToInt64(obj));
			}
		}

		/// <summary>
		/// Returns 0 if object IsDBNull
		/// </summary>
		public static Int32 ToInt32(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(0);
			}
			else if(obj == null)
			{
				return(0);
			}
			else if(obj.ToString().Trim() == String.Empty)
			{
				return(0);
			}
			else
			{
				return(Convert.ToInt32(obj));
			}
		}

		/// <summary>
		/// Returns 0 if object IsDBNull
		/// </summary>
		public static Int16 ToInt16(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(0);
			}
			else if(obj == null)
			{
				return(0);
			}
			else if(obj.ToString().Trim() == String.Empty)
			{
				return(0);
			}
			else
			{
				return(Convert.ToInt16(obj));
			}
		}

		/// <summary>
		/// Returns String.Empty if object IsDBNull
		/// </summary>
		public static string ToString(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(String.Empty);
			}
			else
			{
				//return(StringFunctions.WebSafe(Convert.ToString(obj)));
                return (Convert.ToString(obj));
			}
		}

        /// <summary>
        /// Returns System.Type
        /// </summary>
        public static Type ToType(object obj)
        {
            if (obj.GetType() == typeof(System.Type))
            {
                return (Type) obj;
            }
            else
            {
                return Type.GetType(obj.ToString());
            }
        }

		/// <summary>
		/// Returns 0 if object IsDBNull
		/// </summary>
		public static Single ToSingle(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(0);
			}
			else if(obj == null)
			{
				return(0);
			}
			else if(obj.ToString().Trim() == String.Empty)
			{
				return(0);
			}
			else
			{
				return(Convert.ToSingle(obj));
			}
		}

		/// <summary>
		/// Returns 0 if object IsDBNull
		/// </summary>
		public static double ToDouble(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(0);
			}
			else if(obj == null)
			{
				return(0);
			}
			else if(obj.ToString().Trim() == String.Empty)
			{
				return(0);
			}
			else
			{
				return(Convert.ToDouble(obj));
			}
		}

		/// <summary>
		/// Returns 0 if object IsDBNull
		/// </summary>
		public static byte ToByte(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(0);
			}
			else
			{
				return(Convert.ToByte(obj));
			}
		}

		/// <summary>
		/// Returns 0 if object IsDBNull
		/// </summary>
		public static decimal ToDecimal(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(0);
			}
			else
			{
				return(Convert.ToDecimal(obj));
			}
		}

		/// <summary>
		/// Returns SqlConvert.ToDateTime(null) if object IsDBNull
		/// </summary>
		public static DateTime ToDateTime(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(Convert.ToDateTime("1/1/1900"));
			}
			else if (obj == null)
			{
				return(Convert.ToDateTime("1/1/1900"));
			}
			else
			{
				return(Convert.ToDateTime(obj));
			}
		}

		/// <summary>
		/// Returns false if object IsDBNull
		/// </summary>
		public static bool ToBoolean(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(false);
			}
			else if(obj == null)
			{
				return(false);
			}
			else if(obj.ToString() == "1")
			{
				return(true);
			}
            else if (obj.ToString().ToUpper() == "Y" || obj.ToString().ToUpper() == "YES")
            {
                return (true);
            }
            else if (obj.ToString() == "0")
            {
                return (false);
            }
            else if (obj.ToString().ToUpper() == "N" || obj.ToString().ToUpper() == "NO")
			{
				return(false);
			}
			else
			{
				try
				{
					return(bool.Parse(obj.ToString()));
				}
				catch
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Returns new char() if object IsDBNull
		/// </summary>
		public static char ToChar(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(new char());
			}
			else
			{
				return(Convert.ToChar(obj));
			}
		}

		/// <summary>
		/// Returns new PhoneNumber() if object IsDBNull
		/// </summary>
		public static PhoneNumber ToPhoneNumber(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(new PhoneNumber());
			}
			else
			{
				return(new PhoneNumber(obj.ToString()));
			}
		}

		/// <summary>
		/// Returns new EmailAddress() if object IsDBNull
		/// </summary>
		public static EmailAddress ToEmailAddress(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(new EmailAddress(""));
			}
			else
			{
				return(new EmailAddress(obj.ToString()));
			}
		}

		/// <summary>
		/// Returns new URL() if object IsDBNull
		/// </summary>
		public static URL ToURL(object obj)
		{
			if (Convert.IsDBNull(obj))
			{
				return(new URL());
			}
			else
			{
				return(new URL(obj.ToString()));
			}
		}


        /// <summary>
        /// Returns new XmlDocument() if object IsDBNull
        /// </summary>
        public static System.Xml.XmlDocument ToXML(object obj)
        {
            if (Convert.IsDBNull(obj) || StringFunctions.IsNullOrWhiteSpace((string) obj))
            {
                return (new System.Xml.XmlDocument());
            }
            else
            {
                System.Xml.XmlDocument objXML = new System.Xml.XmlDocument();
                objXML.LoadXml((string) obj);
                return (objXML);
            }
        }

        /// <summary>
        /// Returns PostalAddress from object
        /// </summary>
        public static PostalAddress ToPostalAddress(object obj)
        {
            if (obj == null)
            {
                return (new PostalAddress());
            }
            else if (Convert.IsDBNull(obj))
            {
                return (new PostalAddress());
            }
            else if(obj.GetType() == typeof(PostalAddress))
            {
                return (PostalAddress) obj;
            }
            else
            {
                return PostalAddress.Deserialize((string) obj);
            }
        }

        public static System.Collections.ArrayList ToArrayList(object obj)
        {
            if (obj == null)
            {
                return new System.Collections.ArrayList();
            }
            else if (Convert.IsDBNull(obj))
            {
                return new System.Collections.ArrayList();
            }
            else if (obj.GetType() == typeof(ArrayList))
            {
                return (System.Collections.ArrayList)obj;
            }
            else
            {
                return General.Utilities.Serialization.ArrayList.DeserializeArrayList(obj.ToString(), ",");
            }
        }


		#endregion
		
		#region SqlMinDate, SqlMaxDate
		/// <summary>
		/// Returns the smallest DateTime that SQL Server can use.
		/// "1/1/1753 12:00:00 AM"
		/// </summary>
		/// <returns>DateTime</returns>
		public static DateTime MinDate() {
			return DateTime.Parse("1/1/1753 12:00:00 AM");
		}

		/// <summary>
		/// Returns the largest DateTime that SQL Server can use.
		/// "12/31/9999 11:59:59 PM"
		/// </summary>
		/// <returns>DateTime</returns>
		public static DateTime MaxDate() {
			return DateTime.Parse("12/31/9999 11:59:59 PM");
		}
		#endregion


    }
}
