using System;
using General;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Reflection;

namespace ExtendedControls
{
    /// <summary>
    /// Extends the Normal UpdatePanel so we can determine if the 
    /// UpdatePanel is in PartialRendering stage.
    /// </summary>
    public class UpdatePanelExtended : UpdatePanel
    {

        #region IsUpdating
        /// <summary>
        /// Indicates that the UpdatePanel is updating
        /// </summary>
        public bool IsUpdating
        {
            get
            {
                if (this.RequiresUpdate)
                    return true;
                else if (this.RequiresUpdateInternal)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region RequiresUpdateInternal
        private bool RequiresUpdateInternal
        {
            get
            {
                string strTriggerID = System.Web.UI.ScriptManager.GetCurrent(Page).AsyncPostBackSourceElementID;
                Control objTriggerControl = Page.FindControl(strTriggerID);
                if (objTriggerControl == null && strTriggerID != String.Empty)
                {
                    #region Check For Repeater
                    string[] aryControlPath = strTriggerID.Split('$');
                    string strPath = String.Empty;
                    foreach(string strID in aryControlPath)
                    {
                        if (strPath != String.Empty)
                            strPath += "$";
                        strPath += strID;   
                        Control objControl = Page.FindControl(strPath);
                        if (objControl == null)
                            break;
                        if (objControl is System.Web.UI.WebControls.Repeater)
                        {
                            objTriggerControl = objControl;
                            break;
                        }
                    }
                    #endregion
                }

                UpdatePanel objTriggerUpdatePanel = GetUpdatePanel(objTriggerControl);

                if (this.UpdateMode == UpdatePanelUpdateMode.Always)
                    return true;

                if (objTriggerUpdatePanel == null)
                    return false;

                if (objTriggerUpdatePanel.UniqueID == this.UniqueID)
                    return true;

                

                return false;
            }
        }
        #endregion

        #region GetUpdatePanel
        private UpdatePanel GetUpdatePanel(Control objControl)
        {
            while (objControl != null && !(objControl is UpdatePanel)) { objControl = objControl.Parent; }
            if (objControl != null) 
                return ((UpdatePanel)objControl);
            else 
                return null;
        }
        #endregion

    }

    /// <summary>
    /// Extends the Normal UserControl so we can find
    /// the UpdatePanel that contains it.
    /// </summary>
    public class UserControlExtended : UserControl
    {

        #region WasDataBound
        /*
        protected bool WasDataBound
        {
            get;
            set;
        }
        */
        public bool WasDataBound
        {
            get
            {
                if (ViewState["WasDataBound"] == null)
                    return false;

                return (bool)ViewState["WasDataBound"];
            }
            set
            {
                ViewState["WasDataBound"] = value;
            }
        }
        #endregion

        #region UpdatePanel
        public UpdatePanelExtended UpdatePanel
        {
            get
            {
                Control objControl = this;
                while (objControl != null && !(objControl is UpdatePanel)) { objControl = objControl.Parent; }
                if (objControl != null)
                {
                    try
                    {
                        return ((UpdatePanelExtended)objControl);
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
        }
        #endregion

        #region IsUpdating
        /// <summary>
        /// Indicates that the Control is updating
        /// </summary>
        public bool IsUpdating
        {
            get
            {
                if (this.IsFirstLoad)
                    return true;
                else if (this.UpdatePanelIsUpdating)
                    return true;
                else if(this.UpdatePanel == null)
                    return true;
                return false;
            }
        }
        #endregion

        #region UpdatePanelIsUpdating
        public bool UpdatePanelIsUpdating
        {
            get
            {
                UpdatePanelExtended pnlUpdate = UpdatePanel;
                if (pnlUpdate != null)
                    return pnlUpdate.IsUpdating;
                else
                    return false;
            }
        }
        #endregion

        #region IsFirstLoad
        public bool IsFirstLoad
        {
            get
            {
                if (!Page.IsPostBack)
                    return true;
                return false;
            }
        }
        #endregion

        #region CheckForTrigger

        public bool CheckForTrigger(string strTrigger)
        {
            object objValue1;
            object objValue2;
            return CheckForTrigger(strTrigger, out objValue1, out objValue2);
        }

        public bool CheckForTrigger(string strTrigger, out object objValue1)
        {
            object objValue2;
            return CheckForTrigger(strTrigger, out objValue1, out objValue2);
        }

        public bool CheckForTrigger(string strTrigger, out object objValue1, out object objValue2)
        {
            String sEventArguments = this.Request.Params["__EVENTARGUMENT"];

            if (sEventArguments != null)
            {
                string[] aryArgs = sEventArguments.Split('$');
                if (aryArgs.Length >= 2)
                {
                    string strJavascriptID = aryArgs[0];
                    if (strJavascriptID == JavascriptID)
                    {
                        if (aryArgs[1] == strTrigger)
                        {
                            if (aryArgs.Length > 2)
                                objValue1 = aryArgs[2];
                            else
                                objValue1 = null;
                            if (aryArgs.Length > 3)
                                objValue2 = aryArgs[3];
                            else
                                objValue2 = null;
                            return true;
                        }
                    }
                }
            }
            objValue1 = null;
            objValue2 = null;
            return false;
        }
        #endregion

        #region GenerateTriggerScript
        public string GenerateTriggerScript(string strTriggerName)
        {
            return "__doPostBack('" + UpdatePanel.UniqueID + "','" + JavascriptID + "$" + strTriggerName + "');";
        }

        public string GenerateTriggerScript(string strTriggerName, string strArgument)
        {
            return "__doPostBack('" + UpdatePanel.UniqueID + "','" + JavascriptID + "$" + strTriggerName + "$" + strArgument + "');";
        }

        public string GenerateTriggerScript(string strTriggerName, string strArgument1, string strArgument2)
        {
            return "__doPostBack('" + UpdatePanel.UniqueID + "','" + JavascriptID + "$" + strTriggerName + "$" + strArgument1 + "$" + strArgument2 + "');";
        }
        #endregion

        #region JavascriptID
        public virtual string JavascriptID
        {
            get
            {
                string strID = this.UniqueID;
                strID = strID.Replace(":", "_");
                strID = strID.Replace("$", "_");
                return strID;
            }
        }
        #endregion

        #region ExecuteJavascript
        private int _intScriptIndex = 0;
        protected void ExecuteJavascript(string strScript)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, typeof(Page), "Control_" + this.UniqueID + "_" + _intScriptIndex++, strScript, true);
        }
        #endregion

        #region HasDataBoundProperties
        //http://stackoverflow.com/questions/1417028/how-to-detect-if-asp-net-control-properties-contain-databinding-expressions

        private static readonly object EventDataBinding;
        public bool HasDataBoundProperties
        {
            get
            {
                // check for the presence of DataBinding event handler
                if (this.HasEvents())
                {
                    EventHandler handler = this.Events[UserControlExtended.EventDataBinding] as EventHandler;
                    if (handler != null)
                    {
                        // flag that databinding is needed
                        return true;
                    }
                }
                return false;
            }
        }

        static UserControlExtended()
        {
            try
            {
                FieldInfo field = typeof(Control).GetField(
                    "EventDataBinding",
                    BindingFlags.NonPublic | BindingFlags.Static);

                if (field != null)
                {
                    UserControlExtended.EventDataBinding = field.GetValue(null);
                }
            }
            catch { }

            if (UserControlExtended.EventDataBinding == null)
            {
                // effectively disables the auto-binding feature
                UserControlExtended.EventDataBinding = new object();
            }
        }
        #endregion

    }

    #region IUserControlExtended
    public interface IUserControlExtended
    {
        UpdatePanelExtended UpdatePanel
        {
            get;
        }

        bool UpdatePanelIsUpdating
        {
            get;
        }

        bool IsFirstLoad
        {
            get;
        }
    }
    #endregion

    #region CheckBoxExtended
    /// <summary>
    /// Extends the normal Checkbox so we can determine do cool things like cookie the value.
    /// </summary>
    public class CheckBoxExtended : System.Web.UI.WebControls.CheckBox, IUserControlExtended
    {

        #region Private Variables
        private bool _blnOverride = false;
        private bool _blnOverrideValue = false;
        #endregion

        #region IUserControlExtended Implimentation

        #region UpdatePanel
        public UpdatePanelExtended UpdatePanel
        {
            get
            {
                Control objControl = this;
                while (objControl != null && !(objControl is UpdatePanel)) { objControl = objControl.Parent; }
                if (objControl != null)
                {
                    try
                    {
                        return ((UpdatePanelExtended)objControl);
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
        }
        #endregion

        #region UpdatePanelIsUpdating
        public bool UpdatePanelIsUpdating
        {
            get
            {
                UpdatePanelExtended pnlUpdate = UpdatePanel;
                if (pnlUpdate != null)
                    return pnlUpdate.IsUpdating;
                else
                    return false;
            }
        }
        #endregion

        #region IsFirstLoad
        public bool IsFirstLoad
        {
            get
            {
                if (!Page.IsPostBack)
                    return true;
                return false;
            }
        }
        #endregion

        #endregion

        #region OnLoad
        protected override void OnLoad(EventArgs e)
        {
            if (SaveToCookie && (IsFirstLoad || !UpdatePanelIsUpdating))
            {
                try
                {
                    this.Checked = bool.Parse(General.Cookies.GetCookie(CookieID));
                }
                catch
                {
                    this.Checked = DefaultValue;
                }

            }
            else if (AJAXOverrideMode)
            {

                bool blnCookieValue = bool.Parse(General.Cookies.GetCookie(CookieID));
                if (Checked != blnCookieValue)
                {
                    _blnOverride = true;
                    _blnOverrideValue = this.Checked;
                    this.Checked = blnCookieValue;
                }
            }
            this.CheckedChanged += new EventHandler(OnCheckedChanged);
            base.OnLoad(e);
        }
        #endregion

        #region SaveToCookie
        public bool SaveToCookie
        {
            get
            {
                if (ViewState["SaveToCookie"] == null)
                    return false;
                return (bool)ViewState["SaveToCookie"];
            }
            set
            {
                ViewState["SaveToCookie"] = value;
            }
        }
        #endregion

        #region DefaultValue
        public bool DefaultValue
        {
            get
            {
                if (ViewState["DefaultValue"] == null)
                    return false;
                return (bool)ViewState["DefaultValue"];
            }
            set
            {
                ViewState["DefaultValue"] = value;
            }
        }
        #endregion

        #region CookiePerPage
        public bool CookiePerPage
        {
            get
            {
                if (ViewState["CookiePerPage"] == null)
                    return false;
                return (bool)ViewState["CookiePerPage"];
            }
            set
            {
                ViewState["CookiePerPage"] = value;
            }
        }
        #endregion

        #region OnCheckedChanged
        private void OnCheckedChanged(object o, EventArgs e)
        {
            if (_blnOverride)
                Checked = _blnOverrideValue;

            if (SaveToCookie)
            {
                General.Cookies.SetCookie(CookieID, Checked.ToString());
            }
        }
        #endregion

        #region CookieID
        private string _strCookieID;
        public string CookieID
        {
            get
            {
                if (!StringFunctions.IsNullOrWhiteSpace(_strCookieID))
                {
                    if (CookiePerPage)
                        return General.Web.WebTools.GetRequestedPage() + "_" + _strCookieID + "_ChkVal";
                    else
                        return _strCookieID + "_ChkVal";
                }
                else
                {
                    if (CookiePerPage)
                        return General.Web.WebTools.GetRequestedPage() + "_" + this.UniqueID + "_ChkVal";
                    else
                        return this.UniqueID + "_ChkVal";
                }
            }
            set
            {
                _strCookieID = value;
            }
        }
        #endregion

        #region AJAXOverrideMode
        private bool _blnAJAXOverrideMode = false;
        public bool AJAXOverrideMode
        {
            get
            {
                return _blnAJAXOverrideMode;
            }
            set
            {
                _blnAJAXOverrideMode = value;
            }
        }
        #endregion

    }
    #endregion

}