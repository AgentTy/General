using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Nanometre.
	/// </summary>
	public class Nanometre : Distance
	{

		#region Constructors

		public Nanometre(double dblValue)
		{
			Fill("Nanometre", "nm", "Nanometres", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Nanometre as a Metre object
		/// </summary>
		public static implicit operator Metre(Nanometre obj)
		{
			return new Metre(obj.Value * .000000001);
		}

		/// <summary>
		/// Casts a Nanometre as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(Nanometre obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(Nanometre obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a Centimetre object
		/// </summary>
		public static implicit operator Centimetre(Nanometre obj)
		{
			return (Centimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(Nanometre obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a Micron object
		/// </summary>
		public static implicit operator Micron(Nanometre obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(Nanometre obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a Inch object
		/// </summary>
		public static implicit operator Inch(Nanometre obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a Foot object
		/// </summary>
		public static implicit operator Foot(Nanometre obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a Yard object
		/// </summary>
		public static implicit operator Yard(Nanometre obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a Mile object
		/// </summary>
		public static implicit operator Mile(Nanometre obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Nanometre obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Nanometre obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Nanometre as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Nanometre obj)
		{
			return (LightYear) obj.BaseValue();
		}

		#endregion

	}
}
