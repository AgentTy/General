using System;
using System.Collections;

namespace General.Utilities.Date 
{
	/// <summary>
	/// General definition of a Collection of Months.
	/// </summary>
	public class Years : IEnumerable, IEnumerator 
	{
		#region Public Constructors
		/// <summary>
		/// Creates a Years collection.
		/// </summary>
		public Years() { Fill(); }
		#endregion
		
		#region Public Properties
		/// <summary>
		/// Returns the number of items in the collection
		/// </summary>
		/// <returns>int</returns>
		public int Count { get { return _objLines.Count; } }
		
		/// <summary>
		/// Returns the Month at the specified index
		/// </summary>
		/// <returns>Month</returns>
		public Year this[int intIndex] 
		{
			get 
			{
				try { return (Year) _objLines[intIndex]; } 
				catch { return null; }
			} 
		}
		#endregion
		
		#region Private Variables
		private ArrayList _objLines;
		private int _intIndex = -1;
		#endregion
		
		#region Private Methods
		private void Fill() 
		{
			try 
			{
				_objLines = new ArrayList();
				
				for(int i = DateTime.Now.Year; i<DateTime.Now.Year + 15; i++)
					_objLines.Add(Year.CreateYear(i));
			} 
			catch (Exception ex) 
			{
				throw new Exception(ex.Message);
			}
		}
		#endregion
	
		#region IEnumerable Implementation
		#region IEnumerable Members
		/// <summary>
		/// Gets the Enumerator object
		/// </summary>
		/// <returns>IEnumerator</returns>
		public IEnumerator GetEnumerator() { return this; }
		#endregion

		#region IEnumerator Members
		#region Public Properties
		/// <summary>
		/// Gets the current object
		/// </summary>
		/// <returns>object</returns>
		public object Current 
		{
			get 
			{
				try { return _objLines[_intIndex]; } 
				catch { return null; }
			} 
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Resets the enumeration index
		/// </summary>
		public void Reset() 
		{
			_intIndex = -1;
		}
		
		/// <summary>
		/// Moves to the next object in the enumeration
		/// </summary>
		/// <returns>bool</returns>
		public bool MoveNext() 
		{
			if(_objLines != null && _intIndex < _objLines.Count -1) 
			{
				_intIndex++;
				return true;
			}
			
			Reset();
			return false;
		}
		#endregion
		#endregion
		#endregion
	}
	
	/// <summary>
	/// Represents an individual month.
	/// </summary>
	public class Year 
	{
		#region Creation Methods
		/// <summary>
		/// Creates a month using the passed in Leap parameter
		/// </summary>
		/// <param name="intValue">string - The two digit month value</param>
		/// <returns>Year</returns>
		public static Year CreateYear(int intValue) 
		{
			return new Year(intValue);
		}

		#endregion
		
		#region Private Constructors (Support for creation methods)
		private Year(int intValue) 
		{
			_intValue = intValue;
		}
		#endregion
		
		#region Public Properties
		public int Value { get { return _intValue; } }
		#endregion
		
		#region Private Variables
		private int _intValue;
		#endregion
		
		#region Private Methods

		#endregion
	}
}
