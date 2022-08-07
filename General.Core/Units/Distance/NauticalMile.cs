using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for NauticalMile.
	/// </summary>
	public class NauticalMile : Distance
	{

		#region Constructors

		public NauticalMile(double dblValue)
		{
			Fill("Nautical Mile", "nautical miles", "Nautical Miles", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a NauticalMile as a Metre object
		/// </summary>
		public static implicit operator Metre(NauticalMile obj)
		{
			return new Metre(obj.Value * 1852);
		}

		/// <summary>
		/// Casts a NauticalMile as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(NauticalMile obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a NauticalMile as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(NauticalMile obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a NauticalMile as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(NauticalMile obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a NauticalMile as a Micron object
		/// </summary>
		public static implicit operator Micron(NauticalMile obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a NauticalMile as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(NauticalMile obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a NauticalMile as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(NauticalMile obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a NauticalMile as a Inch object
		/// </summary>
		public static implicit operator Inch(NauticalMile obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a NauticalMile as a Foot object
		/// </summary>
		public static implicit operator Foot(NauticalMile obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a NauticalMile as a Yard object
		/// </summary>
		public static implicit operator Yard(NauticalMile obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a NauticalMile as a Mile object
		/// </summary>
		public static implicit operator Mile(NauticalMile obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a NauticalMile as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(NauticalMile obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a NauticalMile as a LightYear object
		/// </summary>
		public static implicit operator LightYear(NauticalMile obj)
		{
			return (LightYear) obj.BaseValue();
		}

		#endregion
	}
}
