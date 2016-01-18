using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using General.Model;

namespace General.Drawing.Labels
{
    /// <summary>
    /// Interaction logic for AveryBarcodeLabel.xaml
    /// </summary>
    public partial class AveryBarcodeLabel : UserControl
    {

        #region Constructors
        public AveryBarcodeLabel() : this("","","",new PhoneNumber(), new EmailAddress(), DateTime.MinValue)
        {
        }

        public AveryBarcodeLabel(AveryLabelDataModel objData)
        {
            InitializeComponent();

            //set visual elements
            Line1 = objData.Line1;
            Line2 = objData.Line2;
            Line3 = objData.Line3;
            Phone = objData.Phone;
            Email = objData.Email;
            Date = objData.Date;
        }

        public AveryBarcodeLabel(string strLine1, string strLine2, string strLine3, PhoneNumber objPhone, EmailAddress objEmail, DateTime objDate)
        {
            InitializeComponent();

            //set visual elements
            Line1 = strLine1;
            Line2 = strLine2;
            Line3 = strLine3;
            Phone = objPhone;
            Email = objEmail;
            Date = objDate;
        }
        #endregion

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
                this.txtLine1.Text = _strLine1;
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
                this.txtLine2.Text = _strLine2;
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
                this.txtLine3.Text = _strLine3;
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
                if (_objEmail != null)
                    if (_objEmail.Valid)
                        this.txtEmail.Text = _objEmail.ToString();
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
                if (_objPhone != null)
                    if (_objPhone.Valid)
                        this.txtPhone.Text = _objPhone.ToString();
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
                if (_objDate != null)
                    if (_objDate != DateTime.MinValue && _objDate != DateTime.MaxValue)
                        this.txtDate.Text = _objDate.ToShortDateString();
            }
        }
        #endregion


    }
}
