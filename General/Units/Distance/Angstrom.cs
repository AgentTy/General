using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Angstrom.
	/// </summary>
	public class Angstrom : Distance
	{

		#region Constructors

		public Angstrom(double dblValue)
		{
			Fill("Angstrom", "angstrom", "Angstroms", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Angstrom as a Metre object
		/// </summary>
		public static implicit operator Metre(Angstrom obj)
		{
			return new Metre(obj.Value * .0000000001);
		}

		/// <summary>
		/// Casts a Angstrom as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(Angstrom obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(Angstrom obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a Centimetre object
		/// </summary>
		public static implicit operator Centimetre(Angstrom obj)
		{
			return (Centimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(Angstrom obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a Micron object
		/// </summary>
		public static implicit operator Micron(Angstrom obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(Angstrom obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a Inch object
		/// </summary>
		public static implicit operator Inch(Angstrom obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a Foot object
		/// </summary>
		public static implicit operator Foot(Angstrom obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a Yard object
		/// </summary>
		public static implicit operator Yard(Angstrom obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a Mile object
		/// </summary>
		public static implicit operator Mile(Angstrom obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Angstrom obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Angstrom obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Angstrom as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Angstrom obj)
		{
			return (LightYear) obj.BaseValue();
		}


		#endregion

	}
}
