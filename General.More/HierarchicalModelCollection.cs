using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using General;
using General.Internal;
using General.Model;

namespace General
{

    /// <summary>
    /// Summary description for HierarchicalModelCollection.
    /// </summary>
    [Serializable]
    public class HierarchicalModelCollection : List<HierarchicalObjectBase>, IHierarchicalEnumerable
    {

        #region GetRootNodes
        public HierarchicalModelCollection GetRootNodes()
        {
            HierarchicalModelCollection objRootNodes = new HierarchicalModelCollection();
            foreach (HierarchicalObjectBase objNavItem in this)
            {
                if (StringFunctions.IsNullOrWhiteSpace(objNavItem.ParentID))
                    objRootNodes.Add(objNavItem);
            }
            return objRootNodes;
        }
        #endregion

        #region IHierarchicalEnumerable Members

        // Returns a hierarchical data item for the specified enumerated item.
        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return enumeratedItem as IHierarchyData;
        }

        #endregion

    }

    #region NavigationObjectBase
    [Serializable]
    public class NavigationObjectBase : HierarchicalObjectBase
    {

        #region Constructor
        public NavigationObjectBase(ref HierarchicalModelCollection Data) : base(ref Data)
        {

        }
        #endregion

        #region Private Variables

        private URL _objURL;
        private string _strImageFile;
        private string _strToolTip;
        private string _strPostbackArgument;
        private bool _blnActive;

        #endregion

        #region Public Properties

        #region URL
        public URL URL
        {
            get { return _objURL; }
            set { _objURL = value; }
        }
        #endregion

        #region ImageFile
        public string ImageFile
        {
            get { return _strImageFile; }
            set { _strImageFile = value; }
        }
        #endregion

        #region ToolTip
        public string ToolTip
        {
            get {
                if (StringFunctions.IsNullOrWhiteSpace(_strToolTip))
                    return Text;
                return _strToolTip; 
            }
            set { _strToolTip = value; }
        }
        #endregion

        #region PostbackArgument
        public string PostbackArgument
        {
            get { return _strPostbackArgument; }
            set { _strPostbackArgument = value; }
        }
        #endregion

        #region Active
        public virtual bool Active
        {
            get
            {
                if (_objURL != null)
                    if (CurrentPage == General.Web.WebTools.GetScriptName(_objURL.ToString().ToLower()))
                        return true;

                return _blnActive;
            }
            set { _blnActive = value; }
        }

        public virtual bool ActiveQueryString
        {
            get
            {
                if (_objURL != null)
                    if (CurrentPageWithQueryString == _objURL.ToString().ToLower())
                        return true;

                return _blnActive;
            }
            set { _blnActive = value; }
        }
        #endregion

        #endregion

        #region CurrentPage
        public static string CurrentPage
        {
            get
            {
                return General.Web.WebTools.GetRequestedPage().ToLower();
            }
        }

        public static string CurrentPageWithQueryString
        {
            get
            {
                return General.Web.WebTools.GetRequestedPageWithQueryString().ToLower();
            }
        }
        #endregion

    }
    #endregion

    #region CheckBoxTreeItemObjectBase
    [Serializable]
    public class CheckBoxTreeItemObjectBase : HierarchicalObjectBase
    {

        #region Constructor
        public CheckBoxTreeItemObjectBase(ref HierarchicalModelCollection Data)
            : base(ref Data)
        {

        }
        #endregion

        #region Private Variables

        private bool _blnActive;

        #endregion

        #region Public Properties

        public bool Active
        {
            get
            {
                return _blnActive;
            }
            set { _blnActive = value; }
        }

        #endregion

    }
    #endregion

    #region HierarchicalObjectBase
    [Serializable]
    public class HierarchicalObjectBase : ObjectBase, IHierarchyData
    {

        #region AllData
        private HierarchicalModelCollection AllData;
        #endregion

        #region Constructor
        public HierarchicalObjectBase(ref HierarchicalModelCollection Data)
        {
            AllData = Data;
        }
        #endregion

        #region Private Variables

        private String _strUniqueID;
        private String _strParentID;
        private String _strText;

        #endregion

        #region Public Properties

        public String UniqueID
        {
            get { return _strUniqueID; }
            set { _strUniqueID = value; }
        }

        public String ParentID
        {
            get { return _strParentID; }
            set { _strParentID = value; }
        }

        public String Text
        {
            get { return _strText; }
            set { _strText = value; }
        }

        #endregion

        #region IHierarchyData Members

        // Gets an enumeration object that represents all the child 
        // nodes of the current hierarchical node.
        public IHierarchicalEnumerable GetChildren()
        {

            // Call to the local cache for the data
            HierarchicalModelCollection objChildren = new HierarchicalModelCollection();

            // Loop through your local data and find any children
            foreach (HierarchicalObjectBase objItem in AllData)
            {
                if (objItem.ParentID == this.UniqueID)
                {
                    objChildren.Add(objItem);
                }
            }

            return objChildren;
        }

        // Gets an IHierarchyData object that represents the parent node 
        // of the current hierarchical node.
        public IHierarchyData GetParent()
        {

            // Loop through your local data and report back with the parent
            foreach (HierarchicalObjectBase objItem in AllData)
            {
                if (objItem.UniqueID == this.ParentID)
                    return objItem;
            }

            return null;
        }

        public bool HasChildren
        {
            get
            {
                HierarchicalModelCollection objChildren = GetChildren() as HierarchicalModelCollection;
                return objChildren.Count > 0;
            }
        }

        // Gets the hierarchical data node that the object represents.
        public object Item
        {
            get { return this; }
        }

        // Gets the hierarchical path of the node.
        public string Path
        {
            get { return this.UniqueID; }
        }

        public string Type
        {
            get { return this.GetType().ToString(); }
        }

        #endregion   

    }
    #endregion

}
