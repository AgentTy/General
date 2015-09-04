using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace General.Utilities.Serialization
{
	/// <summary>
	/// Serialize and Deserialize objects
	/// </summary>
	public class SerializationTools
	{
		/// <summary>
		/// Serialize and Deserialize objects
		/// </summary>
		public SerializationTools()
		{}

		/// <summary>
		/// Formats a long integer representing a number of bytes into a more friendly format rounded to two decimal places
		/// </summary>
		public static string FormatSize(long amt)
		{
			return FormatSize(amt,2);	
		}    

		/// <summary>
		/// Formats a long integer representing a number of bytes into a more friendly format
		/// </summary>
		public static string FormatSize(long amt, int rounding)
		{
			if (amt == -1) return("Unknown");
			if (amt >= Math.Pow(2, 80)) return Math.Round(amt / Math.Pow(2, 70),rounding).ToString() + " YB"; //yettabyte
			if (amt >= Math.Pow(2, 70)) return Math.Round(amt / Math.Pow(2, 70),rounding).ToString() + " ZB"; //zettabyte
			if (amt >= Math.Pow(2, 60)) return Math.Round(amt / Math.Pow(2, 60),rounding).ToString() + " EB"; //exabyte
			if (amt >= Math.Pow(2, 50)) return Math.Round(amt / Math.Pow(2, 50),rounding).ToString() + " PB"; //petabyte
			if (amt >= Math.Pow(2, 40)) return Math.Round(amt / Math.Pow(2, 40),rounding).ToString() + " TB"; //terabyte
			if (amt >= Math.Pow(2, 30)) return Math.Round(amt / Math.Pow(2, 30),rounding).ToString() + " GB"; //gigabyte
			if (amt >= Math.Pow(2, 20)) return Math.Round(amt / Math.Pow(2, 20),rounding).ToString() + " MB"; //megabyte
			if (amt >= Math.Pow(2, 10)) return Math.Round(amt / Math.Pow(2, 10),rounding).ToString() + " KB"; //kilobyte

			return amt.ToString() + " Bytes"; //byte
		}    


		/// <summary>
		/// Returns the size in bytes of any object
		/// </summary>
		public static long GetObjectByteSize(object Obj)
		{
			if(Obj.GetType() == typeof(string))
				return GetObjectByteSize(Obj.ToString());

			if(Obj.GetType() == typeof(System.Web.UI.Triplet))
				return GetObjectByteSize((System.Web.UI.Triplet) Obj);

			try
			{
				long result;
				MemoryStream s = new MemoryStream();
				BinaryFormatter b=new BinaryFormatter();
				b.Serialize(s,Obj);
				result = s.Length;
				s.Close();
				s = null;
				b = null;
				System.GC.Collect();
				return(result);
			}
			catch
			{
				return(-1);
			}
		}

		/// <summary>
		/// Returns the size in bytes of any string
		/// </summary>
		public static long GetObjectByteSize(string Obj)
		{
			return System.Text.Encoding.ASCII.GetByteCount(Obj);
		}

		/// <summary>
		/// Returns the size in bytes of any string
		/// </summary>
		public static long GetObjectByteSize(System.Web.UI.Triplet Obj)
		{
			long intResult = 0;

			if(Obj.First != null) intResult += GetObjectByteSize(Obj.First);
			if(Obj.Second != null) intResult += GetObjectByteSize(Obj.Second);
			if(Obj.Third != null) intResult += GetObjectByteSize(Obj.Third);

			return intResult;
		}

		/// <summary>
		/// Serialize object
		/// </summary>
		public static string SerializeObject(object Obj)
		{
			string result;
			result = "";

            if (Obj == null)
            {
                result = "<#type:Null#>";
            }
            else
            {
                SerializedObjectArgs objPacket = SerializeObjectForXML(Obj);

                if (objPacket.Type != "Binary")
                {
                    result += "<#type:" + objPacket.Type + "#>";
                    result += objPacket.Value;
                }
                else
                {
                    result = objPacket.Value;
                }
            }

			return(result);
		}


        public struct SerializedObjectArgs
        {
            public string Type;
            public string Value;
        }

        /// <summary>
        /// Serialize object
        /// </summary>
        public static SerializedObjectArgs SerializeObjectForXML(object Obj)
        {
            SerializedObjectArgs result = new SerializedObjectArgs();

            if (Obj == null)
            {
                result.Type = "Null";
            }
            else
            {
                switch (Obj.GetType().Name)
                {
                    case "Boolean":
                    case "Byte":
                    case "SByte":
                    case "Char":
                    case "Decimal":
                    case "Double":
                    case "Single":
                    case "Int32":
                    case "UInt32":
                    case "Int64":
                    case "UInt64":
                    case "Int16":
                    case "UInt16":
                    case "String":
                        result.Type = Obj.GetType().Name;
                        result.Value = Obj.ToString();
                        break;
                    case "DateTime":
                        result.Type = Obj.GetType().Name;
                        result.Value = ((DateTime)Obj).ToString();
                        break;
                    default:
                        if (Obj.GetType().IsEnum)
                        {
                            int intValue = (int)Obj;
                            result.Type = "Int32";
                            result.Value = ((int) Obj).ToString();
                        }
                        else
                        {
                            result.Type = "Binary";
                            MemoryStream s = new MemoryStream();
                            BinaryFormatter b = new BinaryFormatter();
                            b.Serialize(s, Obj);
                            result.Value = Convert.ToBase64String(s.ToArray());
                            s.Close();
                        }
                        break;
                }
            }

            return (result);
        }

		/// <summary>
		/// Deserialize object
		/// </summary>
		public static object DeserializeObject(string Packet)
		{
			if(Packet == null) return null;
            object objResult;

			TypeCode type;
			string temp_packet = Packet;
            type = GetTypeFromPacket(temp_packet);
			if(type != TypeCode.Object)
				temp_packet = temp_packet.Replace(StringFunctions.AllBetween(Packet,"<#type:","#>",true),"");
			
			switch(type)
			{
                case TypeCode.DBNull:
                    objResult = null;
                    break;
				case TypeCode.Object:
					MemoryStream s = new MemoryStream(Convert.FromBase64String(temp_packet));
					BinaryFormatter b=new BinaryFormatter();
                    objResult = (object)b.Deserialize(s);
					s.Close();
					break;
				default:
                    objResult = Convert.ChangeType(temp_packet, type);
					break;
			}

            return (objResult);
		}

        /// <summary>
        /// Deserialize object
        /// </summary>
        public static object DeserializeObject(string strType, string strValue)
        {
            if (strType == null) return null;
            if (strValue == null) return null;
            object objResult;

            TypeCode objType;
            objType = GetTypeFromString(strType);

            switch (objType)
            {
                case TypeCode.DBNull:
                    objResult = null;
                    break;
                case TypeCode.Object:
                    MemoryStream s = new MemoryStream(Convert.FromBase64String(strValue));
                    BinaryFormatter b = new BinaryFormatter();
                    objResult = (object)b.Deserialize(s);
                    s.Close();
                    break;
                case TypeCode.DateTime:
                    objResult = DateTime.Parse(strValue);
                    break;
                default:
                    objResult = Convert.ChangeType(strValue, objType);
                    break;
            }

            return (objResult);
        }

        private static TypeCode GetTypeFromPacket(string packet)
		{
			string type_string;
			if(packet.IndexOf("<#type:") > -1)
			{
				type_string = StringFunctions.AllBetween(packet,"<#type:","#>");
                return GetTypeFromString(type_string);
			}
			else
			{
				return(TypeCode.Object);
			}
		}

        private static TypeCode GetTypeFromString(string strTypeCode)
        {
            switch (strTypeCode)
            {
                case "Null":
                    return (TypeCode.DBNull);
                case "Boolean":
                    return (TypeCode.Boolean);
                case "Byte":
                    return (TypeCode.Byte);
                case "SByte":
                    return (TypeCode.SByte);
                case "Char":
                    return (TypeCode.Char);
                case "Decimal":
                    return (TypeCode.Decimal);
                case "Double":
                    return (TypeCode.Double);
                case "Single":
                    return (TypeCode.Single);
                case "Int32":
                    return (TypeCode.Int32);
                case "UInt32":
                    return (TypeCode.UInt32);
                case "Int64":
                    return (TypeCode.Int64);
                case "UInt64":
                    return (TypeCode.UInt64);
                case "Int16":
                    return (TypeCode.Int16);
                case "UInt16":
                    return (TypeCode.UInt16);
                case "String":
                    return (TypeCode.String);
                case "DateTime":
                    return (TypeCode.DateTime);
                default:
                    return (TypeCode.Object);
            }
        }


	}
}
