using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Centimetre.
	/// </summary>
	public class Centimetre : Distance
	{

		#region Constructors

		public Centimetre(double dblValue)
		{
			Fill("Centimetre", "cm", "Centimetres", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Centimetre as a Metre object
		/// </summary>
		public static implicit operator Metre(Centimetre obj)
		{
			return new Metre(obj.Value * .01);
		}

		/// <summary>
		/// Casts a Centimetre as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(Centimetre obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(Centimetre obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(Centimetre obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a Micron object
		/// </summary>
		public static implicit operator Micron(Centimetre obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(Centimetre obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(Centimetre obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a Inch object
		/// </summary>
		public static implicit operator Inch(Centimetre obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a Foot object
		/// </summary>
		public static implicit operator Foot(Centimetre obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a Yard object
		/// </summary>
		public static implicit operator Yard(Centimetre obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a Mile object
		/// </summary>
		public static implicit operator Mile(Centimetre obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Centimetre obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Centimetre obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Centimetre as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Centimetre obj)
		{
			return (LightYear) obj.BaseValue();
		}

		#endregion

	}
}
