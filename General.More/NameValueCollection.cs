using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using General.Model;

namespace General
{
    /// <summary>

    /// A generic NameValueCollection.

    /// </summary>

    /// <typeparam name="valueT">Value type.</typeparam>

    public class GenericNameValueCollection<valueT> : NameObjectCollectionBase, IEnumerable, IEnumerator
    {
        public delegate void ChangeDetectedDelegate();
        public ChangeDetectedDelegate AnnounceChange;

        /// <summary>
        /// Cretaes an empty collection.
        /// </summary>

        public GenericNameValueCollection()
        {

        }

        public GenericNameValueCollection(ChangeDetectedDelegate ChangeDetectedCallback)
        {
            AnnounceChange = ChangeDetectedCallback;
        }

        /// <summary>
        /// Creates a collection from the IDictionary elements.
        /// </summary>
        /// <param name="dic">IDictionary object.</param>
        /// <param name="readOnly">Use TRUE to create a read-only collection.</param>


        public GenericNameValueCollection(IDictionary dic, bool readOnly)
        {
            foreach (DictionaryEntry de in dic)
            {
                this.BaseAdd(de.Key.ToString(), de.Value);
            }
            this.IsReadOnly = readOnly;
            AnnounceChange();
        }

        /// <summary>
        /// Gets a value using an index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>

        public valueT this[int index]
        {
            get { return (valueT)this.BaseGet(index); }
        }

        /// <summary>
        /// Gets or sets a value for the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public valueT this[string key]
        {
            get { return (valueT)this.BaseGet(key); }
            set { this.BaseSet(key, value); AnnounceChange(); }
        }

        /// <summary>
        /// Gets a value for the given key, and casts as an Integer.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Int32 GetAsInteger(string key)
        {
            return int.Parse((string)this.BaseGet(key));
        }

        /// <summary>
        /// Gets a value for the given key, and casts as a Double.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Double GetAsDouble(string key)
        {
            return double.Parse((string)this.BaseGet(key));
        }

        /// <summary>
        /// Gets a value for the given key, and casts as a Boolean.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean GetAsBoolean(string key)
        {
            return bool.Parse((string)this.BaseGet(key));
        }

        /// <summary>
        /// Gets a value for the given key, and casts as a DateTime.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DateTime GetAsDateTime(string key)
        {
            return DateTime.Parse((string)this.BaseGet(key));
        }

        /// <summary>
        /// Gets a value for the given key, and casts as a PhoneNumber.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public PhoneNumber GetAsPhoneNumber(string key)
        {
            return new PhoneNumber((string)this.BaseGet(key));
        }

        /// <summary>
        /// Gets a value for the given key, and casts as an EmailAddress.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public EmailAddress GetAsEmailAddress(string key)
        {
            return new EmailAddress((string)this.BaseGet(key));
        }

        /// <summary>
        /// Gets a value for the given key, and casts as a URL.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public URL GetAsURL(string key)
        {
            return new URL((string)this.BaseGet(key));
        }

        /// <summary>
        /// Gets an array containing all the keys in the collection.
        /// </summary>

        public string[] AllKeys
        {
            get { return (string[])this.BaseGetAllKeys(); }
        }

        /// <summary>
        /// Gets an object array that contains all the values in the collection.
        /// </summary>

        public object[] AllValues
        {
            get { return this.BaseGetAllValues(); }
        }

        /// <summary>
        /// Gets a value indicating if the collection contains keys that are not null.
        /// </summary>

        public Boolean HasKeys
        {
            get { return this.BaseHasKeys(); }
        }

        /// <summary>
        /// Adds an entry to the collection.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>

        public void Add(string key, valueT value)
        {
            if (!Exists(key))
            {
                this.BaseAdd(key, value);

                if (AnnounceChange != null)
                    AnnounceChange();
            }
            else
            {
                throw new DuplicateWaitObjectException(key);
            }
        }

        private bool Exists(string strFind)
        {
            foreach (string strKey in this.AllKeys)
            {
                if (strKey == strFind)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Removes an entry with the specified key from the collection.
        /// </summary>
        /// <param name="key"></param>

        public void Remove(string key)
        {
            this.BaseRemove(key);
            AnnounceChange();
        }

        /// <summary>
        /// Removes an entry in the specified index from the collection.
        /// </summary>
        /// <param name="index"></param>

        public void Remove(int index)
        {
            this.BaseRemoveAt(index);
            AnnounceChange();
        }

        /// <summary>
        /// Clears all the elements in the collection.
        /// </summary>

        public void Clear()
        {
            this.BaseClear();
            AnnounceChange();
        }

        #region IEnumerable Implementation
        private int _intIndex;
        #region IEnumerable Members
        /// <summary>
        /// Gets the Enumerator object
        /// </summary>
        /// <returns>IEnumerator</returns>
        public override IEnumerator GetEnumerator() { Reset(); return (IEnumerator)this; }
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
                try { return new DictionaryEntry(this.AllKeys[_intIndex], this.AllValues[_intIndex]); }
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
            if (_intIndex < this.Count - 1)
            {
                _intIndex++;
                return true;
            }

            Reset();
            return false;
        }
        #endregion
        #endregion

        #endregion IEnumerable Implementation
    }
}
