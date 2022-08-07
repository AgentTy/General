using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Decimetre.
	/// </summary>
	public class Decimetre : Distance
	{

		#region Constructors

		public Decimetre(double dblValue)
		{
			Fill("Decimetre", "dm", "Decimetres", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Decimetre as a Metre object
		/// </summary>
		public static implicit operator Metre(Decimetre obj)
		{
			return new Metre(obj.Value * .1);
		}

		/// <summary>
		/// Casts a Decimetre as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(Decimetre obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a Centimetre object
		/// </summary>
		public static implicit operator Centimetre(Decimetre obj)
		{
			return (Centimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(Decimetre obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a Micron object
		/// </summary>
		public static implicit operator Micron(Decimetre obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(Decimetre obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(Decimetre obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a Inch object
		/// </summary>
		public static implicit operator Inch(Decimetre obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a Foot object
		/// </summary>
		public static implicit operator Foot(Decimetre obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a Yard object
		/// </summary>
		public static implicit operator Yard(Decimetre obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a Mile object
		/// </summary>
		public static implicit operator Mile(Decimetre obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Decimetre obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Decimetre obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Decimetre as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Decimetre obj)
		{
			return (LightYear) obj.BaseValue();
		}


		#endregion

	}
}
