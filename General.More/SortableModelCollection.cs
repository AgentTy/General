using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using General;
using General.Internal;

namespace General
{
    /// <summary>
    /// Summary description for Collection.
    /// </summary>
    [Serializable]
    public class SortableModelCollection<T> : System.Collections.Generic.List<T>, ICloneable
    {

        #region Public Methods

        public virtual T GetByID(int intID)
        {
            foreach (IObjectBase obj in this)
            {
                if (obj.ID == intID)
                    return (T) obj;
            }
            return default(T);
        }

        public virtual bool Exists(int intID)
        {
            foreach (IObjectBase obj in this)
            {
                if (obj.ID == intID)
                    return true;
            }
            return false;
        }

        public virtual T GetByIndex(int intIndex)
        {
            return (T)this[intIndex];
        }

        public new void Add(T obj)
        {
            base.Add(obj);
        }

        public void AddCollection(ModelCollection objCollection)
        {
            foreach (T obj in objCollection)
                Add(obj);
        }

        public void AddCollection(SortableModelCollection<T> objCollection)
        {
            foreach (T obj in objCollection)
                Add(obj);
        }

        public void Sort(string strColumnName, GenericComparer<T>.SortOrder enuSortOrder)
        {
            base.Sort(new GenericComparer<T>(strColumnName, enuSortOrder));  
        }

        #endregion

        #region Clone
        public object Clone()
        {
            SortableModelCollection<T> objCopy = new SortableModelCollection<T>();
            foreach (T o in this)
                objCopy.Add(o);
            return objCopy;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (T obj in this)
            {
                sb.Append(obj.ToString() + ", ");
            }
            if (sb.Length > 2)
                return StringFunctions.Shave(sb.ToString(), 2);
            else
                return String.Empty;
        }
        #endregion

    }

    #region GenericComparer Class
    /// <summary>
    /// This class is used to compare any
    /// type(property) of a class for sorting.
    /// This class automatically fetches the
    /// type of the property and compares.
    /// </summary>
    public sealed class GenericComparer<T> : IComparer<T>
    {
        public enum SortOrder { Ascending, Descending };

        #region member variables
        private string sortColumn;
        private SortOrder sortingOrder;
        #endregion

        #region constructor
        public GenericComparer(string sortColumn, SortOrder sortingOrder)
        {
            this.sortColumn = sortColumn;
            this.sortingOrder = sortingOrder;
        }
        #endregion

        #region public property
        /// <summary>
        /// Column Name(public property of the class) to be sorted.
        /// </summary>
        public string SortColumn
        {
            get { return sortColumn; }
        }

        /// <summary>
        /// Sorting order.
        /// </summary>
        public SortOrder SortingOrder
        {
            get { return sortingOrder; }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Compare interface implementation
        /// </summary>
        /// <param name="x">custom Object</param>
        /// <param name="y">custom Object</param>
        /// <returns>int</returns>
        public int Compare(T x, T y)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(sortColumn);
            IComparable obj1 = (IComparable)propertyInfo.GetValue(x, null);
            IComparable obj2 = (IComparable)propertyInfo.GetValue(y, null);
            if (sortingOrder == SortOrder.Ascending)
            {
                return (obj1.CompareTo(obj2));
            }
            else
            {
                return (obj2.CompareTo(obj1));
            }
        }
        #endregion

    }
    #endregion

}
