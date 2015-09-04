using System;
using System.Collections;

namespace General.Utilities.Data {
	/// <summary>
	/// First In Last Out (FIFO) stack that only accepts Google.SiteMapURL objects.
	/// </summary>
	public class FIFOStack {
		#region Public Constructors
		public FIFOStack() {
			_al = new ArrayList();
			_blnClosed = false;
		}
		#endregion
		
		#region Private Variables
		private ArrayList _al;
		private bool _blnClosed = false;
		#endregion
		
		#region Public Properties
		public bool Closed { set { _blnClosed = value; } get { return _blnClosed; } }
		#endregion
		
		#region Public Methods
		/// <summary>
		/// Appends a new SiteMapURL object to the top of the stack.
		/// </summary>
		/// <param name="item">object</param>
		public virtual void Push(object item) {
			_al.Add(item);
		}
		
		/// <summary>
		/// Removes the bottom SiteMapURL object from the stack and returns it.
		/// </summary>
		/// <returns>object</returns>
		public virtual object Pop() {
			#region Validation
			if (_al == null || _al.Count == 0)
				return null;
			#endregion
			
			object item;
			try {
				item = _al[0];
			} catch (Exception ex) {
				throw new ArgumentException("Could not access slot 0.", ex);
			}
			_al.RemoveAt(0);
			
			return item;
		}
		
		/// <summary>
		/// Clears the stack.
		/// </summary>
		public void Clear() {
			_al = new ArrayList();
		}
		#endregion
	}
}
