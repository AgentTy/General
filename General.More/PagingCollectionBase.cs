using System;
using General;
using System.Data;
using System.Collections;
using System.Web;

namespace General {
	/// <summary>
	/// Summary description for PagingCollectionBase.
	/// </summary>
	[Serializable]
	public abstract class PagingCollectionBase : IEnumerable,IEnumerator {
		#region Public Properties
		public int Count { get { return _objLines.Count; } }
		public string CacheKey { get { return _strCacheKey; } }
		public ArrayList Items { get { return _objLines; } }
		#endregion 

		#region Public Methods
		public PaginationManager Paging
		{
			get { return _objPaging; }
		}

		protected void InitPaging(DataTable objTable, short intPageNumber, short intPageSize)
		{
			_objPaging = new PaginationManager(objTable, intPageNumber, intPageSize, new PaginationManager.PageChangeDelegate(PageChanged));
		}
		#endregion

		#region Private Variables
		protected object _objKey;
		protected ArrayList _objLines;
		
		// Caching Variables
		protected string _strCacheKey = string.Empty;
		protected int _intCacheHours = 6;
		
		// IEnumerable Variables
		protected int _intIndex = -1;

		private PaginationManager _objPaging;
		#endregion

		#region Pagination
		private void PageChanged(DataTable objCurrentPageData)
		{
			Fill(objCurrentPageData);
		}
		#endregion

		#region Private Functions
		#region Population
		protected abstract void FillItem(DataRow dr);
		  
		protected void Fill(DataTable dt) {
			try {
				_objLines = new ArrayList();
				foreach (DataRow dr in dt.Rows) {
					FillItem(dr);
				}
			} catch (Exception ex) {
				// Output using the environment.output method when it's done...
				throw new Exception("Error trying to populate collection line.",ex);
			} finally {
				dt.Dispose();
			}
		}

		protected void Fill(DataRow[] objRows) 
		{
			try 
			{
				_objLines = new ArrayList();
				foreach (DataRow dr in objRows) 
				{
					FillItem(dr);
				}
			} 
			catch (Exception ex) 
			{
				// Output using the environment.output method when it's done...
				throw new Exception("Error trying to populate collection line.",ex);
			} 
		}
		  
		#region Fill Overloads
		protected void Fill(DataSet ds) {
			try { Fill(ds.Tables[0]); } catch (Exception ex) { throw new Exception("Error trying to populate collection.",ex); } finally { ds.Dispose(); }
		}
		#endregion
		#endregion
		#endregion

		#region Caching
		protected virtual void SetCacheKey() {}
		
		/// <summary>
		/// This method returns an ArrayList so that other private methods
		/// can use it. We may want to change this later, but for now everything
		/// is using an ArrayList and processing it in its own way on the object
		/// side.
		/// </summary>
		/// <returns>ArrayList</returns>
		protected ArrayList GetCache() {
			SetCacheKey();
			if (_strCacheKey == string.Empty) throw new ArgumentNullException("You must implement SetCacheKey to use caching.");
			
			ArrayList al;
			try { al = (ArrayList) HttpContext.Current.Cache[_strCacheKey]; } catch { al = null; }
			
			return al;
		}
		
		/// <summary>
		/// This method returns an ArrayList so that other private methods
		/// can use it. We may want to change this later, but for now everything
		/// is using an ArrayList and processing it in its own way on the object
		/// side.
		/// </summary>
		/// <returns>ArrayList</returns>
		protected ArrayList SetCache() {
			SetCacheKey();
			
			// Try to set the cache. Fail silently for non-web applications. This will still return _objLines.
			try {
				HttpContext.Current.Cache.Insert(_strCacheKey, _objLines, null, DateTime.Now.AddHours(_intCacheHours), TimeSpan.Zero);
			} catch {}
			
			return _objLines;
		}
		
		protected static void ClearCache(string strCacheKey) {
			// This is wrapped in a try for non-web applications. Fail silent is okay.
			try {
				HttpContext.Current.Cache.Remove(strCacheKey);
			} catch {}
		}
		#endregion

	    #region IEnumerable Implementation
		#region IEnumerable Members
		/// <summary>
		/// Gets the Enumerator object
		/// </summary>
		/// <returns>IEnumerator</returns>
		public IEnumerator GetEnumerator() { Reset(); return this; }
		#endregion

		#region IEnumerator Members
		#region Public Properties
		/// <summary>
		/// Gets the current object
		/// </summary>
		/// <returns>object</returns>
		public object Current { get {
			try { return _objLines[_intIndex]; } catch { return null; }
		} }
		#endregion

		#region Public Methods
		/// <summary>
		/// Resets the enumeration index
		/// </summary>
		public void Reset() {
			_intIndex = -1;
		}
		
		/// <summary>
		/// Moves to the next object in the enumeration
		/// </summary>
		/// <returns>bool</returns>
		public bool MoveNext() {
			if(_objLines != null && _intIndex < _objLines.Count -1) {
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
}
