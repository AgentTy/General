using System;
using General;

namespace General.Units.Speed
{
	/// <summary>
	/// KilometrePerHour
	/// </summary>
	public class KilometrePerHour : Speed
	{

		#region Constructors

		public KilometrePerHour(double dblValue)
		{
			Fill("Kilometre Per Hour", "kph", "Kilometres Per Hour", dblValue);
		}

		#endregion

		#region Conversion
		/// <summary>
		/// Casts a KilometrePerHour as a MilePerHour object
		/// </summary>
		public static implicit operator MilePerHour(KilometrePerHour obj)
		{
			return new MilePerHour(obj.Value * 0.6213712);
		}

		/// <summary>
		/// Casts a KilometrePerHour as a Knot object
		/// </summary>
		public static implicit operator Knot(KilometrePerHour obj)
		{
			return new Knot(obj.Value * 0.5399568);
		}
		#endregion

	}
}
