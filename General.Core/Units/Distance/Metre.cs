using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Summary description for Metre.
	/// </summary>
	public class Metre : Distance
	{

		#region Constructors

		public Metre(double dblValue)
		{
			Fill("Metre", "m", "Metres", dblValue);
		}

		#endregion

		#region Conversion
		/// <summary>
		/// Casts a Metre as a Kilometre object
		/// </summary>
		public static implicit operator Kilometre(Metre obj)
		{
			return new Kilometre(obj.Value * 0.001);
		}

		/// <summary>
		/// Casts a Metre as a Decimetre object
		/// </summary>
		public static implicit operator Decimetre(Metre obj)
		{
			return new Decimetre(obj.Value * 10);
		}

		/// <summary>
		/// Casts a Metre as a Centimetre object
		/// </summary>
		public static implicit operator Centimetre(Metre obj)
		{
			return new Centimetre(obj.Value * 100);
		}

		/// <summary>
		/// Casts a Metre as a Millimetre object
		/// </summary>
		public static implicit operator Millimetre(Metre obj)
		{
			return new Millimetre(obj.Value * 1000);
		}

		/// <summary>
		/// Casts a Metre as a Micron object
		/// </summary>
		public static implicit operator Micron(Metre obj)
		{
			return new Micron(obj.Value * 1000000);
		}

		/// <summary>
		/// Casts a Metre as a Nanometre object
		/// </summary>
		public static implicit operator Nanometre(Metre obj)
		{
			return new Nanometre(obj.Value * 1000000000);
		}

		/// <summary>
		/// Casts a Metre as a Angstrom object
		/// </summary>
		public static implicit operator Angstrom(Metre obj)
		{
			return new Angstrom(obj.Value * 10000000000);
		}

		/// <summary>
		/// Casts a Metre as a Inch object
		/// </summary>
		public static implicit operator Inch(Metre obj)
		{
			return new Inch(obj.Value * 39.37008);
		}

		/// <summary>
		/// Casts a Metre as a Foot object
		/// </summary>
		public static implicit operator Foot(Metre obj)
		{
			return new Foot(obj.Value * 3.2808399);
		}

		/// <summary>
		/// Casts a Metre as a Yard object
		/// </summary>
		public static implicit operator Yard(Metre obj)
		{
			return new Yard(obj.Value * 1.093613);
		}

		/// <summary>
		/// Casts a Metre as a Mile object
		/// </summary>
		public static implicit operator Mile(Metre obj)
		{
			return new Mile(obj.Value * .0006213712);
		}

		/// <summary>
		/// Casts a Metre as a NauticalMile object
		/// </summary>
		public static implicit operator NauticalMile(Metre obj)
		{
			return new NauticalMile(obj.Value * .0005399568);
		}

		/// <summary>
		/// Casts a Metre as a AstronomicalUnit object
		/// </summary>
		public static implicit operator AstronomicalUnit(Metre obj)
		{
			return new AstronomicalUnit(obj.Value * 6.684587e-12);
		}

		/// <summary>
		/// Casts a Metre as a LightYear object
		/// </summary>
		public static implicit operator LightYear(Metre obj)
		{
			return new LightYear(obj.Value * 1.057023e-16);
		}
		#endregion

	}
}
