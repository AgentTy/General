using System;
using General;

namespace General.Units.Speed
{
	/// <summary>
	/// Knot
	/// </summary>
	public class Knot : Speed
	{

		#region Constructors

		public Knot(double dblValue)
		{
			Fill("Knot", "knot", "Knots", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Knot as a KilometrePerHour object
		/// </summary>
		public static implicit operator KilometrePerHour(Knot obj)
		{
			return new KilometrePerHour(obj.Value * 1.852);
		}

		/// <summary>
		/// Casts a Knot as a MilePerHour object
		/// </summary>
		public static implicit operator MilePerHour(Knot obj)
		{
			return (MilePerHour) obj.BaseValue();
		}

		#endregion
	}
}
