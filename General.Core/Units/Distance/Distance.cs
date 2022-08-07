using System;
using General;

namespace General.Units.Distance
{
	/// <summary>
	/// Base Class For Units of Measuring Distance
	/// </summary>
	public abstract class Distance : Unit
	{

		#region BaseValue

		protected Metre BaseValue()
		{
			if(this.GetType() == typeof(Metre))
				return (Metre) this;
			else if(this.GetType() == typeof(Kilometre))
				return ((Metre) new Kilometre(this.Value));
			else if(this.GetType() == typeof(Decimetre))
				return ((Metre) new Decimetre(this.Value));
			else if(this.GetType() == typeof(Centimetre))
				return ((Metre) new Centimetre(this.Value));
			else if(this.GetType() == typeof(Millimetre))
				return ((Metre) new Millimetre(this.Value));
			else if(this.GetType() == typeof(Micron))
				return ((Metre) new Micron(this.Value));
			else if(this.GetType() == typeof(Nanometre))
				return ((Metre) new Nanometre(this.Value));
			else if(this.GetType() == typeof(Angstrom))
				return ((Metre) new Angstrom(this.Value));
			else if(this.GetType() == typeof(Inch))
				return ((Metre) new Inch(this.Value));
			else if(this.GetType() == typeof(Foot))
				return ((Metre) new Foot(this.Value));
			else if(this.GetType() == typeof(Yard))
				return ((Metre) new Yard(this.Value));
			else if(this.GetType() == typeof(Mile))
				return ((Metre) new Mile(this.Value));
			else if(this.GetType() == typeof(NauticalMile))
				return ((Metre) new NauticalMile(this.Value));
			else if(this.GetType() == typeof(AstronomicalUnit))
				return ((Metre) new AstronomicalUnit(this.Value));
			else if(this.GetType() == typeof(LightYear))
				return ((Metre) new LightYear(this.Value));
			else
				throw new Exception("No Base Value Conversion Specified For Type " + this.GetType().Name);
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Compares two Distance objects
		/// </summary>
		public static bool operator ==(Distance d1, Distance d2)
		{
			if((object) d1 == null && (object) d2 != null)
				return false;
			if((object) d2 == null && (object) d1 != null)
				return false;
			if((object) d1 == null && (object) d2 == null)
				return true;
			if(d1.GetType() != d2.GetType())
				return(IsEqual(d1.BaseValue().Value, d2.BaseValue().Value));
			return(d1.Value == d2.Value);
		}

		/// <summary>
		/// Compares two Distance objects
		/// </summary>
		public static bool operator != (Distance d1, Distance d2)
		{
			if((object) d1 == null && (object) d2 != null)
				return true;
			if((object)d2 == null && (object) d1 != null)
				return true;
			if((object) d1 == null && (object) d2 == null)
				return false;
			if(d1.GetType() != d2.GetType())
				return(!IsEqual(d1.BaseValue().Value, d2.BaseValue().Value));
			return(d1.Value != d2.Value);
		}	

		/// <summary>
		/// Compares two Distance objects
		/// </summary>
		public override bool Equals(object obj)
		{
			return(this==(Distance) obj);
		}
		#endregion

		#region Conversion

		/// <summary>
		/// Casts A Distance as a Metre object
		/// </summary>
		public Metre ToMetre()
		{
			return BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a Kilometre object
		/// </summary>
		public Kilometre ToKilometre()
		{
			return (Kilometre) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a Decimetre object
		/// </summary>
		public Decimetre ToDecimetre()
		{
			return (Decimetre) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a Millimetre object
		/// </summary>
		public Millimetre ToMillimetre()
		{
			return (Millimetre) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a Micron object
		/// </summary>
		public Micron ToMicron()
		{
			return (Micron) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a Nanometre object
		/// </summary>
		public Nanometre ToNanometre()
		{
			return (Nanometre) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a Angstrom object
		/// </summary>
		public Angstrom ToAngstrom()
		{
			return (Angstrom) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a Inch object
		/// </summary>
		public Inch ToInch()
		{
			return (Inch) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a Foot object
		/// </summary>
		public Foot ToFoot()
		{
			return (Foot) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a Yard object
		/// </summary>
		public Yard ToYard()
		{
			return (Yard) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a Mile object
		/// </summary>
		public Mile ToMile()
		{
			return (Mile) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a NauticalMile object
		/// </summary>
		public NauticalMile ToNauticalMile()
		{
			return (NauticalMile) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a AstronomicalUnit object
		/// </summary>
		public AstronomicalUnit ToAstronomicalUnit()
		{
			return (AstronomicalUnit) BaseValue();
		}

		/// <summary>
		/// Casts A Distance as a LightYear object
		/// </summary>
		public LightYear ToLightYear()
		{
			return (LightYear) BaseValue();
		}

		#endregion

        #region GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

	}
}
