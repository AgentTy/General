using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for AstronomicalUnit.
	/// </summary>
	public class AstronomicalUnit: Distance
	{

		#region Constructors

		public AstronomicalUnit(double dblValue)
		{
			Fill("Astronomical Unit", "au", "Astronomical Units", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a AstronomicalUnit as a Metre object
		/// </summary>
		public static implicit operator Metre(AstronomicalUnit obj)
		{
			return new Metre(obj.Value * 149597900000);
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(AstronomicalUnit obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(AstronomicalUnit obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a Centimetre object
		/// </summary>
		public static implicit operator Centimetre(AstronomicalUnit obj)
		{
			return (Centimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(AstronomicalUnit obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a Micron object
		/// </summary>
		public static implicit operator Micron(AstronomicalUnit obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(AstronomicalUnit obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(AstronomicalUnit obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a Inch object
		/// </summary>
		public static implicit operator Inch(AstronomicalUnit obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a Foot object
		/// </summary>
		public static implicit operator Foot(AstronomicalUnit obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a Yard object
		/// </summary>
		public static implicit operator Yard(AstronomicalUnit obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a Mile object
		/// </summary>
		public static implicit operator Mile(AstronomicalUnit obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(AstronomicalUnit obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a AstronomicalUnit as a LightYear object
		/// </summary>
		public static implicit operator LightYear(AstronomicalUnit obj)
		{
			return (LightYear) obj.BaseValue();
		}

		#endregion
	}
}
