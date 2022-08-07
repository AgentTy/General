using System;
using General;
using General.Units.Distance;

namespace General.Model
{
	/// <summary>
	/// Contains information about a GPS Coordinate
	/// Longitude, Latitude, and Altitude
	/// </summary>
	public class GPSCoordinate
	{

		#region Private Variables
		private double _dblLon;
		private double _dblLat;

		private Distance _objAlt;
		private string _strAltRef;
		#endregion

		#region Constructors

		#region Constructors Without Altitude
		public GPSCoordinate(double dblLongitude, double dblLatitude)
		{
			Fill(dblLongitude,dblLatitude);
		}

		public GPSCoordinate(enumLongitudeReference enumLongitudeDirection, int intLongitudeDegrees, int intLongitudeMinutes, double dblLongitudeSeconds, enumLatitudeReference enumLatitudeDirection, int intLatitudeDegrees, int intLatitudeMinutes, double dblLatitudeSeconds)
		{
			Fill(enumLongitudeDirection, intLongitudeDegrees, intLongitudeMinutes, dblLongitudeSeconds, enumLatitudeDirection, intLatitudeDegrees, intLatitudeMinutes, dblLatitudeSeconds);
		}

		public GPSCoordinate(int intLongitudeDegrees, int intLongitudeMinutes, double dblLongitudeSeconds, int intLatitudeDegrees, int intLatitudeMinutes, double dblLatitudeSeconds)
		{
			Fill(intLongitudeDegrees, intLongitudeMinutes, dblLongitudeSeconds, intLatitudeDegrees, intLatitudeMinutes, dblLatitudeSeconds);
		}

		public GPSCoordinate(enumLongitudeReference enumLongitudeDirection, int intLongitudeDegrees, double dblLongitudeMinutes, enumLatitudeReference enumLatitudeDirection, int intLatitudeDegrees, double dblLatitudeMinutes)
		{
			Fill(enumLongitudeDirection, intLongitudeDegrees, dblLongitudeMinutes, enumLatitudeDirection, intLatitudeDegrees, dblLatitudeMinutes);
		}

		public GPSCoordinate(int intLongitudeDegrees, double dblLongitudeMinutes, int intLatitudeDegrees, double dblLatitudeMinutes)
		{
			Fill(intLongitudeDegrees, dblLongitudeMinutes, intLatitudeDegrees, dblLatitudeMinutes);		
		}
		#endregion

		#region Constructors With Altitude
		public GPSCoordinate(double dblLongitude, double dblLatitude, Distance objAltitude, string strAltitudeReference)
		{
			_objAlt = objAltitude;
			_strAltRef = strAltitudeReference;

			Fill(dblLongitude,dblLatitude);
		}

		public GPSCoordinate(enumLongitudeReference enumLongitudeDirection, int intLongitudeDegrees, int intLongitudeMinutes, double dblLongitudeSeconds, enumLatitudeReference enumLatitudeDirection, int intLatitudeDegrees, int intLatitudeMinutes, double dblLatitudeSeconds, Distance objAltitude, string strAltitudeReference)
		{
			_objAlt = objAltitude;
			_strAltRef = strAltitudeReference;

			Fill(enumLongitudeDirection, intLongitudeDegrees, intLongitudeMinutes, dblLongitudeSeconds, enumLatitudeDirection, intLatitudeDegrees, intLatitudeMinutes, dblLatitudeSeconds);
		}

		public GPSCoordinate(int intLongitudeDegrees, int intLongitudeMinutes, double dblLongitudeSeconds, int intLatitudeDegrees, int intLatitudeMinutes, double dblLatitudeSeconds, Distance objAltitude, string strAltitudeReference)
		{
			_objAlt = objAltitude;
			_strAltRef = strAltitudeReference;

			Fill(intLongitudeDegrees, intLongitudeMinutes, dblLongitudeSeconds, intLatitudeDegrees, intLatitudeMinutes, dblLatitudeSeconds);
		}

		public GPSCoordinate(enumLongitudeReference enumLongitudeDirection, int intLongitudeDegrees, double dblLongitudeMinutes, enumLatitudeReference enumLatitudeDirection, int intLatitudeDegrees, double dblLatitudeMinutes, Distance objAltitude, string strAltitudeReference)
		{
			_objAlt = objAltitude;
			_strAltRef = strAltitudeReference;

			Fill(enumLongitudeDirection, intLongitudeDegrees, dblLongitudeMinutes, enumLatitudeDirection, intLatitudeDegrees, dblLatitudeMinutes);
		}

		public GPSCoordinate(int intLongitudeDegrees, double dblLongitudeMinutes, int intLatitudeDegrees, double dblLatitudeMinutes, Distance objAltitude, string strAltitudeReference)
		{
			_objAlt = objAltitude;
			_strAltRef = strAltitudeReference;

			Fill(intLongitudeDegrees, dblLongitudeMinutes, intLatitudeDegrees, dblLatitudeMinutes);		
		}
		#endregion

		#region Private Constructors
		private GPSCoordinate()
		{

		}
		#endregion

		#endregion

		#region Null Value
		public static GPSCoordinate Null = new GPSCoordinate();
		#endregion

		#region Validation
		public bool Valid
		{
			get {return (!(this == Null));}
		}
		#endregion

		#region Public Properties
		
		public double Latitude
		{
			get { return _dblLat; }
		}

		public enumLatitudeReference LatitudeReference
		{
			get { return GetLatitudeReference(_dblLat); }
		}

		public int LatitudeWholeDegrees
		{
			get { return GetWholeDegrees(_dblLat); }
		}

		public double LatitudeMinutes
		{
			get { return GetMinutes(_dblLat); }
		}

		public int LatitudeWholeMinutes
		{
			get { return GetWholeMinutes(_dblLat); }
		}

		public double LatitudeSeconds
		{
			get { return GetSeconds(_dblLat); }
		}

		public double Longitude
		{
			get { return _dblLon; }
		}

		public enumLongitudeReference LongitudeReference
		{
			get { return GetLongitudeReference(_dblLon); }
		}

		public int LongitudeWholeDegrees
		{
			get { return GetWholeDegrees(_dblLon); }
		}

		public double LongitudeMinutes
		{
			get { return GetMinutes(_dblLon); }
		}

		public int LongitudeWholeMinutes
		{
			get { return GetWholeMinutes(_dblLon); }
		}

		public double LongitudeSeconds
		{
			get { return GetSeconds(_dblLon); }
		}



		public Distance Altitude
		{
			get { return _objAlt; }
			set { _objAlt = value; }
		}

		public string AltitudeReference
		{
			get { return _strAltRef; }
		}

		#endregion 

		#region Public Methods

		#endregion

		#region ToString
		/// <summary>
		/// Output class data in string format
		/// </summary>
		public override string ToString()
		{
			return ToString("<br>\r\n");
		}

		/// <summary>
		/// Output class data in string format
		/// </summary>
		public string ToString(string strLineBreak)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("Latitude" + " = " + _dblLat.ToString() + strLineBreak);
			sb.Append("Longitude" + " = " + _dblLon.ToString() + strLineBreak);
			if(_objAlt == null)
				sb.Append("Altitude Unknown" + strLineBreak);
			else
				sb.Append("Altitude" + " = " + _objAlt.ToFoot().Round(2).ToString() + strLineBreak);
			return sb.ToString();
		}
		#endregion

		#region Operators
		/// <summary>
		/// Compares two GPSCoordinate objects
		/// </summary>
		public static bool operator ==(GPSCoordinate Coord1, GPSCoordinate Coord2)
		{
			if((object) Coord1 == null && (object) Coord2 != null)
				return false;
			if((object)Coord2 == null && (object) Coord1 != null)
				return false;
			if((object) Coord1 == null && (object) Coord2 == null)
				return true;
			return(Coord1.Latitude == Coord2.Latitude && Coord1.Longitude == Coord2.Longitude && Coord1.Altitude == Coord2.Altitude);
		}

		/// <summary>
		/// Compares two GPSCoordinate objects
		/// </summary>
		public static bool operator !=(GPSCoordinate Coord1, GPSCoordinate Coord2)
		{
			if((object) Coord1 == null && (object) Coord2 != null)
				return true;
			if((object)Coord2 == null && (object) Coord1 != null)
				return true;
			if((object) Coord1 == null && (object) Coord2 == null)
				return false;
			return(Coord1.Latitude != Coord2.Latitude || Coord1.Longitude != Coord2.Longitude || Coord1.Altitude != Coord2.Altitude);
		}	

		/// <summary>
		/// Casts an GPSCoordinate as a string
		/// </summary>
		public static implicit operator string(GPSCoordinate Coord)
		{
			try
			{
				return Coord.ToString();
			}
			catch
			{
				return null;
			}
		}


		/// <summary>
		/// Compares two GPSCoordinate objects
		/// </summary>
		public override bool Equals(object obj)
		{
			return(this==(GPSCoordinate) obj);
		}


		#endregion

		#region Private Functions

		#region Fill

		#region Fill Overloads

		public void Fill(int intLongitudeDegrees, int intLongitudeMinutes, double dblLongitudeSeconds, int intLatitudeDegrees, int intLatitudeMinutes, double dblLatitudeSeconds)
		{
			//First I need to figure out and store the direction (N,S,E,W) of the Longitude and Latitude
			enumLongitudeReference enumLonRef = GetLongitudeReference(intLongitudeDegrees);
			enumLatitudeReference enumLatRef = GetLatitudeReference(intLatitudeDegrees);
			//Now Fill
			Fill(enumLonRef, intLongitudeDegrees, intLongitudeMinutes, dblLongitudeSeconds, enumLatRef, intLatitudeDegrees, intLatitudeMinutes, dblLatitudeSeconds);
		}

		public void Fill(int intLongitudeDegrees, double dblLongitudeMinutes, int intLatitudeDegrees, double dblLatitudeMinutes)
		{
			//First I need to figure out and store the direction (N,S,E,W) of the Longitude and Latitude
			enumLongitudeReference enumLonRef = GetLongitudeReference(intLongitudeDegrees);
			enumLatitudeReference enumLatRef = GetLatitudeReference(intLatitudeDegrees);
			//Now Fill
			Fill(enumLonRef, intLongitudeDegrees, dblLongitudeMinutes, enumLatRef, intLatitudeDegrees, dblLatitudeMinutes);
		}
		#endregion

		public void Fill(double dblLongitude, double dblLatitude)
		{
			_dblLon = dblLongitude;
			_dblLat = dblLatitude;
		}

		private void Fill(enumLongitudeReference enumLonRef, int intLongitudeDegrees, double dblLongitudeMinutes, enumLatitudeReference enumLatRef, int intLatitudeDegrees, double dblLatitudeMinutes)
		{
			//First I need to get the Absolute value before doing any math
			intLongitudeDegrees = Math.Abs(intLongitudeDegrees);
			intLatitudeDegrees = Math.Abs(intLatitudeDegrees);
			//Now I will build the numeric coordinate
			_dblLon = ((double) intLongitudeDegrees) + (dblLongitudeMinutes / 60);
			_dblLat = ((double) intLatitudeDegrees) + (dblLatitudeMinutes / 60);
			//Now I will restore the negative values where necessary
			_dblLon = SetDirectionSign(_dblLon,enumLonRef);
			_dblLat = SetDirectionSign(_dblLat,enumLatRef);
		}

		private void Fill(enumLongitudeReference enumLonRef, int intLongitudeDegrees, int intLongitudeMinutes, double dblLongitudeSeconds, enumLatitudeReference enumLatRef, int intLatitudeDegrees, int intLatitudeMinutes, double dblLatitudeSeconds)
		{
			//First I need to get the Absolute value before doing any math
			intLongitudeDegrees = Math.Abs(intLongitudeDegrees);
			intLatitudeDegrees = Math.Abs(intLatitudeDegrees);
			//Now I will build the numeric coordinate
			_dblLon = ((double) intLongitudeDegrees) + (((double) intLongitudeMinutes) / 60) + (dblLongitudeSeconds / 3600);
			_dblLat = ((double) intLatitudeDegrees) + (((double) intLatitudeMinutes) / 60) + (dblLatitudeSeconds / 3600);
			//Now I will restore the negative values where necessary
			_dblLon = SetDirectionSign(_dblLon,enumLonRef);
			_dblLat = SetDirectionSign(_dblLat,enumLatRef);
		}

		#endregion

		#region Conversion Functions
		private double GetDegrees(double dblCoord)
		{
			return Math.Abs(dblCoord);
		}

		private int GetWholeDegrees(double dblCoord)
		{
			return (int) Math.Floor(GetDegrees(dblCoord));
		}

		private int GetWholeMinutes(double dblCoord)
		{
			return (int) Math.Floor((GetDegrees(dblCoord) - GetWholeDegrees(dblCoord)) * 60);
		}

		private double GetMinutes(double dblCoord)
		{
			return (GetDegrees(dblCoord) - GetWholeDegrees(dblCoord)) * 60;
		}

		private double GetSeconds(double dblCoord)
		{
			return (GetMinutes(dblCoord) - GetWholeMinutes(dblCoord)) * 60;
		}
		#endregion

		#region Directional Funtions
		private enumLongitudeReference GetLongitudeReference(double dblCoord)
		{
			if(dblCoord <= 0)
				return enumLongitudeReference.West;
			else
				return enumLongitudeReference.East;
		}

		private enumLatitudeReference GetLatitudeReference(double dblCoord)
		{
			if(dblCoord <= 0)
				return enumLatitudeReference.South;
			else
				return enumLatitudeReference.North;
		}

		private double SetDirectionSign(double dblCoord, enumLongitudeReference enumLonRef)
		{
			if(enumLonRef == enumLongitudeReference.West)
				return -(Math.Abs(dblCoord));
			else
				return Math.Abs(dblCoord);
		}

		private double SetDirectionSign(double dblCoord, enumLatitudeReference enumLatRef)
		{
			if(enumLatRef == enumLatitudeReference.South)
				return -(Math.Abs(dblCoord));
			else
				return Math.Abs(dblCoord);
		}
		#endregion

		#endregion

		#region Enumerators
		public enum enumLongitudeReference
		{
			East = 1,
			West = -1
		}

		public enum enumLatitudeReference
		{
			North = 1,
			South = -1
		}
		#endregion

		#region Static Methods

		#region Calculate Distance
		/// <summary>
		/// Calculates the distance between two GPS points, and returns a Distance object.
		/// </summary>
		/// <param name="objStart">Coordinate 1</param>
		/// <param name="objEnd">Coordinate 2</param>
		/// <returns>Distance</returns>
		public static Distance GetDistance(GPSCoordinate objStart, GPSCoordinate objEnd) 
		{ 
			///Credits to John Bowen for the formula in C# syntax
			///http://blog.gobowen.org/
			///
			///Credits to 'The Math Forum' for the formula
			///http://mathforum.org/library/drmath/view/51879.html 
			
			double degrad = Math.PI/180;
			double a, c, R;
			R = 0;
			double lat1, lon1;
			double lat2, lon2; 
			double dlon, dlat; 
						
			//convert the degree values to radians before calculation
			lat1 = objStart.Latitude * degrad;
			lon1 = objStart.Longitude * degrad;
			lat2 = objEnd.Latitude * degrad;
			lon2 = objEnd.Longitude * degrad;

			dlon = lon2 - lon1; 
			dlat = lat2 - lat1; 
			
			a = Math.Pow(Math.Sin(dlat/2),2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon/2),2);
			c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a)) ;
			
			// R (Earth Radius) 6367.0 km
			R = 6367.0;
			return new Kilometre(R * c); 
		}

		#endregion

		#endregion

        #region GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

	}

}
