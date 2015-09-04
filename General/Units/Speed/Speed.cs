using System;
using General;

namespace General.Units.Speed
{
	/// <summary>
	/// Base Class For Units of Measuring Speed
	/// </summary>
	public abstract class Speed : Unit
	{

		#region BaseValue
		protected KilometrePerHour BaseValue()
		{
			if(this.GetType() == typeof(KilometrePerHour))
				return (KilometrePerHour) this;
			else if(this.GetType() == typeof(MilePerHour))
				return ((KilometrePerHour) new MilePerHour(this.Value));
			else if(this.GetType() == typeof(Knot))
				return ((KilometrePerHour) new Knot(this.Value));
			else
				throw new Exception("No Base Value Conversion Specified For Type " + this.GetType().Name);
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Compares two Speed objects
		/// </summary>
		public static bool operator ==(Speed s1, Speed s2)
		{
			if((object) s1 == null && (object) s2 != null)
				return false;
			if((object) s2 == null && (object) s1 != null)
				return false;
			if((object) s1 == null && (object) s2 == null)
				return true;
			if(s1.GetType() != s2.GetType())
				return(IsEqual(s1.BaseValue().Value, s2.BaseValue().Value));
			return(s1.Value == s2.Value);
		}

		/// <summary>
		/// Compares two Speed objects
		/// </summary>
		public static bool operator != (Speed s1, Speed s2)
		{
			if((object) s1 == null && (object) s2 != null)
				return true;
			if((object)s2 == null && (object) s1 != null)
				return true;
			if((object) s1 == null && (object) s2 == null)
				return false;
			if(s1.GetType() != s2.GetType())
				return(!IsEqual(s1.BaseValue().Value, s2.BaseValue().Value));
			return(s1.Value != s2.Value);
		}	

		/// <summary>
		/// Compares two Speed objects
		/// </summary>
		public override bool Equals(object obj)
		{
			return(this==(Speed) obj);
		}
		#endregion

		#region Conversion

		/// <summary>
		/// Casts A Speed as a KilometrePerHour object
		/// </summary>
		public KilometrePerHour ToKilometrePerHour()
		{
			return BaseValue();
		}

		/// <summary>
		/// Casts A Speed as a MilePerHour object
		/// </summary>
		public MilePerHour ToMilePerHour()
		{
			return (MilePerHour) BaseValue();
		}

		/// <summary>
		/// Casts A Speed as a Decimetre object
		/// </summary>
		public Knot ToKnot()
		{
			return (Knot) BaseValue();
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
