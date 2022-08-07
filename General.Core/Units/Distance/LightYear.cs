using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for LightYear.
	/// </summary>
	public class LightYear: Distance
	{

		#region Constructors

		public LightYear(double dblValue)
		{
			Fill("Light Year", "light years", "Light Years", dblValue);
		}

		#endregion

		#region Conversion

		/// <summary>
		/// Casts a LightYear as a Metre object
		/// </summary>
		public static implicit operator Metre(LightYear obj)
		{
			return new Metre(obj.Value * 9460528000000000);
		}

		/// <summary>
		/// Casts a LightYear as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(LightYear obj)
		{
			return (Kilometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(LightYear obj)
		{
			return (Decimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a Centimetre object
		/// </summary>
		public static implicit operator Centimetre(LightYear obj)
		{
			return (Centimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(LightYear obj)
		{
			return (Millimetre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a Micron object
		/// </summary>
		public static implicit operator Micron(LightYear obj)
		{
			return (Micron) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(LightYear obj)
		{
			return (Nanometre) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(LightYear obj)
		{
			return (Angstrom) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a Inch object
		/// </summary>
		public static implicit operator Inch(LightYear obj)
		{
			return (Inch) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a Foot object
		/// </summary>
		public static implicit operator Foot(LightYear obj)
		{
			return (Foot) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a Yard object
		/// </summary>
		public static implicit operator Yard(LightYear obj)
		{
			return (Yard) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a Mile object
		/// </summary>
		public static implicit operator Mile(LightYear obj)
		{
			return (Mile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(LightYear obj)
		{
			return (NauticalMile) obj.BaseValue();
		}

		/// <summary>
		/// Casts a LightYear as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(LightYear obj)
		{
			return (AstronomicalUnit) obj.BaseValue();
		}

		#endregion

	}
}
