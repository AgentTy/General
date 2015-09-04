using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Micron.
	/// </summary>
	public class Micron : Distance
	{

		#region Constructors

		public Micron(double dblValue)
		{
			Fill("Micron", "micron", "Microns", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Micron as a Metre object
		/// </summary>
		public static implicit operator Metre(Micron obj)
		{
			return new Metre(obj.Value * .000001);
		}

		/// <summary>
		/// Casts a Micron as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(Micron obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(Micron obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a Centimetre object
		/// </summary>
		public static implicit operator Centimetre(Micron obj)
		{
			return (Centimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(Micron obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(Micron obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(Micron obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a Inch object
		/// </summary>
		public static implicit operator Inch(Micron obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a Foot object
		/// </summary>
		public static implicit operator Foot(Micron obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a Yard object
		/// </summary>
		public static implicit operator Yard(Micron obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a Mile object
		/// </summary>
		public static implicit operator Mile(Micron obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Micron obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Micron obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Micron as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Micron obj)
		{
			return (LightYear) obj.BaseValue();
		}


		#endregion

	}
}
