using System;
using System.Data;
using System.Collections;
using System.Web;
using General;
using General.Internal;

namespace General
{
    /// <summary>
    /// Summary description for Collection.
    /// </summary>
    [Serializable]
    public class ModelCollection : IEnumerable, IEnumerator
    {

        #region Private Variables
        protected object _objKey;
        protected ArrayList _objLines = new ArrayList();

        // IEnumerable Variables
        protected int _intIndex = -1;

        //Pagination Variables
        private int _intPageSize = 15;
        private int _intCurrentPage = 1;
        private bool _blnPagingEnabled = false;
        #endregion

        #region Public Properties
        public int Count
        {
            get
            {
                if(!_blnPagingEnabled)
                    return _objLines.Count;
                else
                    return _intPageSize;
            }
        }
        public ArrayList Items { get { return _objLines; } }

        public int PageSize
        {
            get
            {
                return _intPageSize;
            }
            set
            {
                _intPageSize = value;
            }
        }

        #endregion

        #region Public Methods

        public virtual IObjectBase GetByID(int intID)
        {
            foreach (IObjectBase obj in _objLines)
            {
                if (obj.ID == intID)
                    return obj;
            }
            return null;
        }

        public virtual bool Exists(int intID)
        {
            foreach (IObjectBase obj in _objLines)
            {
                if (obj.ID == intID)
                    return true;
            }
            return false;
        }

        public virtual IObjectBase GetByIndex(int intIndex)
        {
            return (IObjectBase)_objLines[intIndex];
        }

        public void Add(IObjectBase obj)
        {
            _objLines.Add(obj);
        }

        #endregion

        #region Pagination

        public int TotalCount
        {
            get { return _objLines.Count; }
        }

        public void Paginate()
        {
            Paginate(_intPageSize);
        }

        public void Paginate(int intPageSize)
        {
            _intPageSize = intPageSize;
            _blnPagingEnabled = true;
        }

        public int CurrentPage
        {
            get
            {
                return _intCurrentPage;
            }
        }

        public int PageCount
        {
            get
            {
                return (int) Math.Ceiling(((decimal)_objLines.Count) / ((decimal)_intPageSize));
            }
        }

        public void SetPage(int intPage)
        {
            _intCurrentPage = intPage;
            SyncPage();
        }

        public void SetNextPage()
        {
            _intCurrentPage++;
            SyncPage();
        }

        public void SetPreviousPage()
        {
            _intCurrentPage--;
            SyncPage();
        }

        private void SyncPage()
        {
            _intIndex = _intPageSize * (_intCurrentPage - 1) - 1;

            if (_intIndex + 1 >= _objLines.Count && _intCurrentPage > 1)
                SetPreviousPage();

            if (_intCurrentPage < 1)
                SetNextPage();
        }

        private bool IsInPage(int intIndex)
        {
            int _intMinIndex = _intPageSize * (_intCurrentPage - 1) - 1;
            int _intMaxIndex = ((_intCurrentPage - 1) * _intPageSize) + _intPageSize - 2;

            return (_intIndex <= _intMaxIndex && _intIndex >= _intMinIndex);
        }

        public bool AllowPreviousPage
        {
            get
            {
                return _intIndex + 1 >= _intPageSize;
            }
        }

        public bool AllowNextPage
        {
            get
            {
                return _intIndex + 1 <= _objLines.Count - _intPageSize;
            }
        }

        #endregion

        #region ToString
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (IObjectBase obj in _objLines)
            {
                sb.Append(obj.ToString() + ", ");
            }
            if (sb.Length > 2)
                return StringFunctions.Shave(sb.ToString(), 2);
            else
                return String.Empty;
        }
        #endregion

        #region IEnumerable Implementation
        #region IEnumerable Members
        /// <summary>
        /// Gets the Enumerator object
        /// </summary>
        /// <returns>IEnumerator</returns>
        public IEnumerator GetEnumerator() { Reset(); return (IEnumerator)this; }
        #endregion

        #region IEnumerator Members
        #region Public Properties
        /// <summary>
        /// Gets the current object
        /// </summary>
        /// <returns>object</returns>
        public virtual object Current
        {
            get
            {
                try { return _objLines[_intIndex]; }
                catch { return null; }
            }
        }

        public virtual IObjectBase this[int ID]
        {
            get { return GetByID(ID); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Resets the enumeration index
        /// </summary>
        public void Reset()
        {
            if (_blnPagingEnabled)
                SyncPage();
            else
                _intIndex = -1;
        }

        /// <summary>
        /// Moves to the next object in the enumeration
        /// </summary>
        /// <returns>bool</returns>
        public bool MoveNext()
        {
            if (_objLines != null && _intIndex < _objLines.Count - 1 && (!_blnPagingEnabled || IsInPage(_intIndex)))
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
}
