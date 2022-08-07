using System;
using General;

namespace General.Units.Speed
{
	/// <summary>
	/// MilePerHour
	/// </summary>
	public class MilePerHour : Speed
	{

		#region Constructors

		public MilePerHour(double dblValue)
		{
			Fill("Mile Per Hour", "mph", "Miles Per Hour", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a MilePerHour as a KilometrePerHour object
		/// </summary>
		public static implicit operator KilometrePerHour(MilePerHour obj)
		{
			return new KilometrePerHour(obj.Value * 1.609344);
		}

		/// <summary>
		/// Casts a MilePerHour as a Knot object
		/// </summary>
		public static implicit operator Knot(MilePerHour obj)
		{
			return (Knot) obj.BaseValue();
		}

		#endregion
	}
}
