using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General.Model;

namespace General.Drawing.Labels
{
    public class AveryLabelDataModel
    {

        #region Line1
        private string _strLine1;
        public string Line1
        {
            get
            {
                return _strLine1;
            }
            set
            {
                _strLine1 = value;
            }
        }
        #endregion

        #region Line2
        private string _strLine2;
        public string Line2
        {
            get
            {
                return _strLine2;
            }
            set
            {
                _strLine2 = value;
            }
        }
        #endregion

        #region Line3
        private string _strLine3;
        public string Line3
        {
            get
            {
                return _strLine3;
            }
            set
            {
                _strLine3 = value;
            }
        }
        #endregion

        #region Email
        private EmailAddress _objEmail;
        public EmailAddress Email
        {
            get
            {
                return _objEmail;
            }
            set
            {
                _objEmail = value;
            }
        }
        #endregion

        #region Phone
        private PhoneNumber _objPhone;
        public PhoneNumber Phone
        {
            get
            {
                return _objPhone;
            }
            set
            {
                _objPhone = value;
            }
        }
        #endregion

        #region Date
        private DateTime _objDate;
        public DateTime Date
        {
            get
            {
                return _objDate;
            }
            set
            {
                _objDate = value;
            }
        }
        #endregion

    }
}
