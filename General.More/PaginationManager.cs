using System;
using System.Data;
using System.Data.SqlClient;
using General.Model;

namespace General
{

	/// <summary>
	/// General::PaginationManager
	/// Object facilitates Pagination of a DataTable object
	/// Use with PagingCollectionBase for Pagination of a Collection
	/// </summary>
	public class PaginationManager
	{

		#region Delegates
		/// <summary>
		/// Delegate used to notify a higher object of a page change
		/// </summary>
		public delegate void PageChangeDelegate(DataTable objCurrentPageData);
		/// <summary>
		/// += to this property to add more PageChange listeners
		/// </summary>
		public PageChangeDelegate NotifyPageChange;
		#endregion

		#region Private Variables
		private URL _objURL;
		private Int16 _intCurrentPage;
		private Int16 _intRowsPerPage = 10;
		private Int16 _intTotalPages;
		private DataTable _objCurrentPageData;
		private DataTable _objTable;
		#endregion

		#region Public Properties
		/// <summary>
		/// Optional property for storing a URL with a PaginationManager object
		/// </summary>
		public URL URL
		{
			get { return _objURL; }
			set { _objURL = value; }
		}

		/// <summary>
		/// Current page index
		/// </summary>
		public Int16 CurrentPage
		{
			get { return _intCurrentPage; }
			//set { _intCurrentPage = value; }
		}

        /// <summary>
        /// Rows Per Page
        /// </summary>
        public Int16 RowsPerPage
        {
            get { return _intRowsPerPage; }
            set { 
                _intRowsPerPage = value;
			    _intTotalPages = GetTotalPages();
			    _intCurrentPage = 1;
			    FillData(); //Fill first page
            }
        }

		/// <summary>
		/// Number of pages
		/// </summary>
		public Int16 TotalPages
		{
			get { return _intTotalPages; }
			//set { _intTotalPages = value; }
		}

		/// <summary>
		/// DataTable object with rows from current page
		/// </summary>
		public DataTable CurrentPageData
		{
			get { return _objCurrentPageData; }
		}

        /// <summary>
        /// Original DataTable
        /// </summary>
        public DataTable SourceData
        {
            get { return _objTable; }
        }
		#endregion

		#region Public Methods

		/// <summary>
		/// Move to the next page
		/// </summary>
		public void GoToNextPage()
		{
			_intCurrentPage++;
			FillData();
		}

		/// <summary>
		/// Move to the previous page
		/// </summary>
		public void GoToPreviousPage()
		{
			_intCurrentPage--;
			FillData();
		}

		/// <summary>
		/// Move to the first page
		/// </summary>
		public void GoToFirstPage()
		{
			_intCurrentPage = 1;
			FillData();
		}

		/// <summary>
		/// Move to the last page
		/// </summary>
		public void GoToLastPage()
		{
			_intCurrentPage = _intTotalPages;
			FillData();
		}

		/// <summary>
		/// Move to specified page
		/// </summary>
		public void GoToPage(Int16 intPage)
		{
			_intCurrentPage = intPage;
			FillData();
		}

		#endregion

		#region Constructors
		/// <summary>
		/// Construct a PaginationManager object 
		/// Defaults to first page
		/// Does not impliment PageChangeDelegate
		/// </summary>
		public PaginationManager(DataTable objTable)
		{
			_objTable = objTable;
			_intTotalPages = GetTotalPages();
			_intCurrentPage = 1;
			FillData(); //Fill first page
		}

		/// <summary>
		/// Construct a PaginationManager object 
		/// Starts at specified page
		/// Does not impliment PageChangeDelegate
		/// </summary>
		public PaginationManager(DataTable objTable, Int16 intStartPage, Int16 intRowsPerPage)
		{
			_objTable = objTable;
            _intRowsPerPage = intRowsPerPage;
			_intTotalPages = GetTotalPages();
			_intCurrentPage = intStartPage;
			FillData(); //Fill first page
		}
        

		/// <summary>
		/// Construct a PaginationManager object 
		/// Starts at specified page
		/// Fires PageChangeDelegate on parent object
		/// </summary>
        public PaginationManager(DataTable objTable, Int16 intStartPage, Int16 intRowsPerPage, PageChangeDelegate delNotifyPageChange)
		{
			_objTable = objTable;
            _intRowsPerPage = intRowsPerPage;
			_intTotalPages = GetTotalPages();
			_intCurrentPage = intStartPage;
			NotifyPageChange += delNotifyPageChange;
			FillData(); //Fill first page
			
		}
		#endregion Constructors

		#region GetTotalPages
		private Int16 GetTotalPages()
		{
			return (short) Math.Ceiling((double) _objTable.Rows.Count / (double) _intRowsPerPage);
		}
		#endregion

		#region FillData
		private void FillData()
		{
			_objCurrentPageData = _objTable.Clone();
			int intStartRow = (_intCurrentPage - 1) * _intRowsPerPage;
			for(int i = intStartRow; i < intStartRow + _intRowsPerPage && i < _objTable.Rows.Count; i++)
				_objCurrentPageData.ImportRow(_objTable.Rows[i]);

			if(NotifyPageChange != null) NotifyPageChange(_objCurrentPageData);
		}


		#endregion

		#region ToString
		/// <summary>
		/// ToString assumes HTML line break
		/// </summary>
		public override string ToString()
		{
			return ToString("<br>");
		}

		/// <summary>
		/// ToString with specified line break
		/// </summary>
		public string ToString(string strLineBreak)
		{
			return ToDebuggingString(strLineBreak);
		}

		#region ToDebuggingString
		/// <summary>
		/// ToString assumes HTML line break
		/// </summary>
		public string ToDebuggingString()
		{
			return ToDebuggingString("<br>");
		}

		/// <summary>
		/// ToString with specified line break
		/// </summary>
		public string ToDebuggingString(string strLineBreak)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("URL" + " = " + _objURL.ToString() + strLineBreak);
			sb.Append("CurrentPage" + " = " + _intCurrentPage.ToString() + strLineBreak);
			sb.Append("TotalPages" + " = " + _intTotalPages.ToString() + strLineBreak);
			return sb.ToString();
		}
		#endregion ToDebuggingString

		#endregion ToString

	} //End Class

}
