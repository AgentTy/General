using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExtendedControls
{

    #region RepeaterExtended
    /// <summary>
    /// Extends the normal Repeater so we can determine do cool things like find elements inside it.
    /// </summary>
    public class RepeaterExtended : Repeater
    {

        #region FindHeaderControl
        public Control FindHeaderControl(string strID)
        {
            try
            {
                return (Control)this.Controls[0].FindControl(strID);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region FindFooterControl
        public Control FindFooterControl(string strID)
        {
            try
            {
                return (Control)this.Controls[this.Controls.Count - 1].FindControl(strID);
            }
            catch
            {
                return null;
            }
        }
        #endregion

    }
    #endregion

    #region LinkButtonExtended
    /// <summary>
    /// Extends the normal LinkButton so we can determine do cool things like add a confirm dialogue.
    /// </summary>
    public class LinkButtonExtended : LinkButton
    {

        #region ConfirmText
        public String ConfirmText
        {
            get
            {
                return (string) ViewState["ConfirmText"];
            }
            set
            {
                ViewState["ConfirmText"] = value;
                this.OnClientClick = "return confirm('" + value + "');";
            }
        }
        #endregion

    }
    #endregion

    #region CompositeBoundField
    public class CompositeBoundField : BoundField
    {
        protected override object GetValue(Control controlContainer)
        {
            object item = DataBinder.GetDataItem(controlContainer);
            return DataBinder.Eval(item, this.DataField);
        }
    }
    #endregion

    #region DumbMenu
    /// <summary>
    /// This is a custom ASP:Menu control that disables the javascript
    /// </summary>
    public class DumbMenu : System.Web.UI.WebControls.Menu
    {

        #region Constructor
        public DumbMenu()
        {

        }
        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            // Don't call base OnPreRender
            //base.OnPreRender(e);
        }

    }
    #endregion

}