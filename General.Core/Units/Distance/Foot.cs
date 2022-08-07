using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Foot.
	/// </summary>
	public class Foot : Distance
	{

		#region Constructors

		public Foot(double dblValue)
		{
			Fill("Foot", "ft", "Feet", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Foot as a Metre object
		/// </summary>
		public static implicit operator Metre(Foot obj)
		{
			return new Metre(obj.Value * .3048);
		}

		/// <summary>
		/// Casts a Foot as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(Foot obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Foot as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(Foot obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Foot as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(Foot obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Foot as a Micron object
		/// </summary>
		public static implicit operator Micron(Foot obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Foot as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(Foot obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Foot as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(Foot obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Foot as a Inch object
		/// </summary>
		public static implicit operator Inch(Foot obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Foot as a Yard object
		/// </summary>
		public static implicit operator Yard(Foot obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Foot as a Mile object
		/// </summary>
		public static implicit operator Mile(Foot obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Foot as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Foot obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Foot as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Foot obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Foot as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Foot obj)
		{
			return (LightYear) obj.BaseValue();
		}

		#endregion

	}
}
