using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Cheque : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Cheque()
        {
            _checkID = "";
            _transitNo = "";
            _institutionNo = ""; 
            _accountNo = "";
            _accountOwerName = "";
            _institutionName = "";
            _institutionAddressApart = "";
            _institutionAddressNo = "";
            _institutionAddressStreet = "";
            _institutionAddressCity = "";
            _institutionAddressProvince = "";
            _institutionAddressPostcode = "";
            _verifyDate = "";
        }
        #endregion Constructor

        #region Public Interface    
        private string _checkID;
        public string CheckID
        {
            get { return _checkID; }
            set
            {
                _checkID = value;
                OnPropertyChanged("CheckID");
            }
        }
        private string _transitNo;
        public string TransitNo
        {
            get { return _transitNo; }
            set
            {
                _transitNo = value;
                OnPropertyChanged("transitNo");
            }
        }
        private string _institutionNo;
        public string InstitutionNo
        {
            get { return _institutionNo; }
            set
            {
                _institutionNo = value;
                OnPropertyChanged("InstitutionNo");
            }
        }
        private string _accountNo;
        public string AccountNo
        {
            get { return _accountNo; }
            set
            {
                _accountNo = value;
                OnPropertyChanged("AccountNo");
            }
        }
        private string _accountOwerName;
        public string AccountOwerName
        {
            get { return _accountOwerName; }
            set
            {
                _accountOwerName = value;
                OnPropertyChanged("AccountOwerName");
            }
        }
        private string _institutionName;
        public string InstitutionName
        {
            get { return _institutionName; }
            set
            {
                _institutionName = value;
                OnPropertyChanged("InstitutionName");
            }
        }

        private string _institutionAddressApart;
        public string InstitutionAddressApart
        {
            get { return _institutionAddressApart; }
            set
            {
                _institutionAddressApart = value;
                OnPropertyChanged("InstitutionAddressApart");
            }
        }
        private string _institutionAddressNo;
        public string InstitutionAddressNo
        {
            get { return _institutionAddressNo; }
            set
            {
                _institutionAddressNo = value;
                OnPropertyChanged("InstitutionAddressNo");
            }
        }
        private string _institutionAddressStreet;
        public string InstitutionAddressStreet
        {
            get { return _institutionAddressStreet; }
            set
            {
                _institutionAddressStreet = value;
                OnPropertyChanged("InstitutionAddressStreet");
            }
        }
        private string _institutionAddressCity;
        public string InstitutionAddressCity
        {
            get { return _institutionAddressCity; }
            set
            {
                _institutionAddressCity = value;
                OnPropertyChanged("InstitutionAddressCity");
            }
        }
        private string _institutionAddressProvince;
        public string InstitutionAddressProvince
        {
            get { return _institutionAddressProvince; }
            set
            {
                _institutionAddressProvince = value;
                OnPropertyChanged("InstitutionAddressProvince");
            }
        }
        private string _institutionAddressPostcode;
        public string InstitutionAddressPostcode
        {
            get { return _institutionAddressPostcode; }
            set
            {
                _institutionAddressPostcode = value;
                OnPropertyChanged("InstitutionAddressPostcode");
            }
        }
        private string _verifyDate;
        public string VerifyDate
        {
            get { return _verifyDate; }
            set
            {
                _verifyDate = value;
                OnPropertyChanged("VerifyDate");
            }
        }
        #endregion Public Interface
    }
}
