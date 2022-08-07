using System;
using General;

namespace General.Units
{
	/// <summary>
	/// Base Class For Units of Measurement
	/// </summary>
	public abstract class Unit
	{

		#region Private Variables
		private string _strUnitName;
		private string _strUnitAbbrev;
		private string _strUnitPlural;
		private double _dblValue;
		#endregion

		#region Public Properties
		public string UnitName
		{
			get { return _strUnitName; }
		}

		public string UnitAbbrev
		{
			get { return _strUnitAbbrev; }
		}

		public double Value
		{
			get { return _dblValue; }
		}
		#endregion 

		#region Protected Methods
		protected void Fill(string strUnitName, string strUnitAbbrev, string strUnitPlural, double dblValue)
		{
			_strUnitName = strUnitName;
			_strUnitAbbrev = strUnitAbbrev;
			_strUnitPlural = strUnitPlural;
			_dblValue = dblValue;
		}
		#endregion

		#region Public Methods
		public void Add(double dblValue)
		{
			_dblValue += dblValue;
		}

		public Unit Round(int intDecimalPlaces)
		{
			_dblValue = Math.Round(_dblValue,intDecimalPlaces);
			return this;
		}
		#endregion

		#region ToString
		public override string ToString()
		{
			string strResult = String.Empty;
			strResult += _dblValue.ToString() + " ";
			if(_dblValue == 1) 
				strResult += _strUnitName;
			else
				strResult += _strUnitPlural;

			return strResult;
		}

		public string ToString(bool boolAbbreviated)
		{
			if(!boolAbbreviated)
				return ToString();

			return _dblValue.ToString() + _strUnitAbbrev;
		}
		#endregion

		#region Private Functions

		protected static bool IsEqual(double dblValue1, double dblValue2)
		{
			double tolerance = .000001; 
			double a, b, dif;

			//Place the larger value (in absolute terms) in the a variable
			a = Math.Max(Math.Abs(dblValue1),Math.Abs(dblValue2));
			//Place the smaller value (in absolute terms) in the b variable
			b = Math.Min(Math.Abs(dblValue1),Math.Abs(dblValue2));
			
			//Get the percentage of difference between the values
			dif = (a / b) - 1;

			//Return true if the percentage of difference is less than the set tolerance
			return (dif < tolerance);
		}

		#endregion

	}
}
