using System;
using General;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.SessionState;

namespace General
{

    /// <summary>
    /// Summary description for Cookies
    /// </summary>
    public class Cookies
    {

        #region Get Cookie
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Session["Cookie_" + strName] != null)
            {
                string strValue = ((String)HttpContext.Current.Session["Cookie_" + strName]);
                HttpContext.Current.Session["Cookie_" + strName] = null;
                SetCookie(strName, strValue);
                return strValue;
            }

            if (HttpContext.Current.Request.Cookies[strName] == null)
                return String.Empty;

            return HttpContext.Current.Request.Cookies[strName].Value;
        }
        #endregion

        #region Set Cookie
        public static void SetCookie(string strName, string strValue)
        {
            HttpContext.Current.Session["Cookie_" + strName] = strValue;
            HttpContext.Current.Response.Cookies.Remove(strName);
            HttpCookie obj = new HttpCookie(strName, strValue);
            obj.Expires = DateTime.Today.AddMonths(12);
            HttpContext.Current.Response.Cookies.Add(obj);
        }
        #endregion

    }
}