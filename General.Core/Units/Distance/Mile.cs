using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Mile.
	/// </summary>
	public class Mile : Distance
	{

		#region Constructors

		public Mile(double dblValue)
		{
			Fill("Mile", "mi", "Miles", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a Mile as a Metre object
		/// </summary>
		public static implicit operator Metre(Mile obj)
		{
			return new Metre(obj.Value * 1609.344);
		}

		/// <summary>
		/// Casts a Mile as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(Mile obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Mile as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(Mile obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Mile as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(Mile obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Mile as a Micron object
		/// </summary>
		public static implicit operator Micron(Mile obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Mile as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(Mile obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Mile as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(Mile obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Mile as a Inch object
		/// </summary>
		public static implicit operator Inch(Mile obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Mile as a Foot object
		/// </summary>
		public static implicit operator Foot(Mile obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Mile as a Yard object
		/// </summary>
		public static implicit operator Yard(Mile obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Mile as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Mile obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Mile as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Mile obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		/// <summary>
		/// Casts a Mile as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Mile obj)
		{
			return (LightYear) obj.BaseValue();
		}

		#endregion
	}
}
