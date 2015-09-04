using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Inch.
	/// </summary>
	public class Inch: Distance
	{

		#region Constructors

		public Inch(double dblValue)
		{
			Fill("Inch", "in", "Inches", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Inch as a Metre object
		/// </summary>
		public static implicit operator Metre(Inch obj)
		{
			return new Metre(obj.Value * .0254);
		}

		/// <summary>
		/// Casts a Inch as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(Inch obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Inch as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(Inch obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Inch as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(Inch obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Inch as a Micron object
		/// </summary>
		public static implicit operator Micron(Inch obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Inch as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(Inch obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Inch as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(Inch obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Inch as a Foot object
		/// </summary>
		public static implicit operator Foot(Inch obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Inch as a Yard object
		/// </summary>
		public static implicit operator Yard(Inch obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Inch as a Mile object
		/// </summary>
		public static implicit operator Mile(Inch obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Inch as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Inch obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Inch as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Inch obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Inch as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Inch obj)
		{
			return (LightYear) obj.BaseValue();
		}
		#endregion

	}
}
