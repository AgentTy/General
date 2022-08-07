using System;
using General.Model;


namespace General.Internal
{
	/// <summary>
	/// This abstract class is for implementing smart data update practices, by aiding other classes in knowing when a data update is necessary and when it can be ignored.
	/// </summary>
	[Serializable]
	public abstract class JITData
	{

		#region Private Variables
        [NonSerialized]
	    bool ClassModified = false;
		#endregion

		#region Constructors

		/// <summary>
		/// This abstract class is for implementing smart data update practices, by aiding other classes in knowing when a data update is necessary and when it can be ignored.
		/// </summary>
		public JITData()
		{

		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Returns true if the class objects data integrity seal has been broken.
		/// </summary>
		public virtual bool SealBroken()
		{
			return ClassModified;
		}

		/// <summary>
		/// Explicitly breaks the data integrity seal.
		/// </summary>
		protected void BreakSeal()
		{
			ClassModified = true;
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected string PokeSeal(string OldValue, string NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected int PokeSeal(int OldValue, int NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected long PokeSeal(long OldValue, long NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected short PokeSeal(short OldValue, short NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected bool PokeSeal(bool OldValue, bool NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected char PokeSeal(char OldValue, char NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected double PokeSeal(double OldValue, double NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected Single PokeSeal(Single OldValue, Single NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected DateTime PokeSeal(DateTime OldValue, DateTime NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected byte PokeSeal(byte OldValue, byte NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected EmailAddress PokeSeal(EmailAddress OldValue, EmailAddress NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected PhoneNumber PokeSeal(PhoneNumber OldValue, PhoneNumber NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected PostalAddress PokeSeal(PostalAddress OldValue, PostalAddress NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		/// <summary>
		/// Compares two objects and breaks the data integrity seal if the objects are not equal.
		/// </summary>
		protected AddressBookEntry PokeSeal(AddressBookEntry OldValue, AddressBookEntry NewValue)
		{
			if(OldValue != NewValue)
				BreakSeal();
			return(NewValue);
		}

		#endregion

		#region Private Functions
		

		#endregion

	}
}
