using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Kilometre.
	/// </summary>
	public class Kilometre : Distance
	{

		#region Constructors

		public Kilometre(double dblValue)
		{
			Fill("Kilometre", "km", "Kilometres", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Kilometre as a Metre object
		/// </summary>
		public static implicit operator Metre(Kilometre obj)
		{
			return new Metre(obj.Value * 1000);
		}

		/// <summary>
		/// Casts a Kilometre as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(Kilometre obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a Centimetre object
		/// </summary>
		public static implicit operator Centimetre(Kilometre obj)
		{
			return (Centimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(Kilometre obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a Micron object
		/// </summary>
		public static implicit operator Micron(Kilometre obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(Kilometre obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(Kilometre obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a Inch object
		/// </summary>
		public static implicit operator Inch(Kilometre obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a Foot object
		/// </summary>
		public static implicit operator Foot(Kilometre obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a Yard object
		/// </summary>
		public static implicit operator Yard(Kilometre obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a Mile object
		/// </summary>
		public static implicit operator Mile(Kilometre obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Kilometre obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Kilometre obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Kilometre as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Kilometre obj)
		{
			return (LightYear) obj.BaseValue();
		}

		#endregion

	}
}
