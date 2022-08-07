using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Yard.
	/// </summary>
	public class Yard : Distance
	{

		#region Constructors

		public Yard(double dblValue)
		{
			Fill("Yard", "yd", "Yards", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Yard as a Metre object
		/// </summary>
		public static implicit operator Metre(Yard obj)
		{
			return new Metre(obj.Value * .9144);
		}

		/// <summary>
		/// Casts a Yard as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(Yard obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Yard as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(Yard obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Yard as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(Yard obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Yard as a Micron object
		/// </summary>
		public static implicit operator Micron(Yard obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Yard as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(Yard obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Yard as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(Yard obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Yard as a Inch object
		/// </summary>
		public static implicit operator Inch(Yard obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Yard as a Foot object
		/// </summary>
		public static implicit operator Foot(Yard obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Yard as a Mile object
		/// </summary>
		public static implicit operator Mile(Yard obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Yard as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Yard obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Yard as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Yard obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Yard as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Yard obj)
		{
			return (LightYear) obj.BaseValue();
		}

		#endregion

	}
}
