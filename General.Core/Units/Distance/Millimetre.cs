using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Millimetre.
	/// </summary>
	public class Millimetre : Distance
	{

		#region Constructors

		public Millimetre(double dblValue)
		{
			Fill("Millimetre", "mm", "Millimetres", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Millimetre as a Metre object
		/// </summary>
		public static implicit operator Metre(Millimetre obj)
		{
			return new Metre(obj.Value * .001);
		}

		/// <summary>
		/// Casts a Millimetre as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(Millimetre obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(Millimetre obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a Centimetre object
		/// </summary>
		public static implicit operator Centimetre(Millimetre obj)
		{
			return (Centimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a Micron object
		/// </summary>
		public static implicit operator Micron(Millimetre obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(Millimetre obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(Millimetre obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a Inch object
		/// </summary>
		public static implicit operator Inch(Millimetre obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a Foot object
		/// </summary>
		public static implicit operator Foot(Millimetre obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a Yard object
		/// </summary>
		public static implicit operator Yard(Millimetre obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a Mile object
		/// </summary>
		public static implicit operator Mile(Millimetre obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Millimetre obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Millimetre obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Millimetre as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Millimetre obj)
		{
			return (LightYear) obj.BaseValue();
		}

		#endregion

	}
}
