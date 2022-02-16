using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Beneficiary : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Beneficiary()
        {
            _investNo = "";
            _bnfPID = "";
            _bnfFirstName = "";
            _bnfLastName = "";
            _bnfGender = "";
            _bnfBirthday = "";
            _bnfSIN = "";
            _bnfType = "";
            _bnfRelationship = "";
            _revokable = "";
            _bnfPercentage = "";
            _trusteePID = "";
            _trusteeFirstName = "";
            _trusteeLastName = "";
            _trusteeBirthday = "";
            _trRelationship = "";
            _currentFlag = "";
            //_startDate = "";
            //_endDate = "";
            _verifyDate = "";
            //_updateDate = "";
        }

        #endregion Constructor

        #region Public Interface    
        private string _investNo;
        public string InvestNo
        {
            get { return _investNo; }
            set
            {
                _investNo = value;
                OnPropertyChanged("InvestNo");
            }
        }
        private string _bnfPID;
        public string BnfPID
        {
            get { return _bnfPID; }
            set
            {
                _bnfPID = value;
                OnPropertyChanged("BnfPID");
            }
        }
        private string _bnfFirstName;
        public string BnfFirstName
        {
            get { return _bnfFirstName; }
            set
            {
                _bnfFirstName = value;
                OnPropertyChanged("BnfFirstName");
            }
        }
        private string _bnfLastName;
        public string BnfLastName
        {
            get { return _bnfFirstName; }
            set
            {
                _bnfFirstName = value;
                OnPropertyChanged("BnfLastName");
            }
        }
        private string _bnfGender;
        public string BnfGender
        {
            get { return _bnfGender; }
            set
            {
                _bnfGender = value;
                OnPropertyChanged("BnfGender");
            }
        }
        private string _bnfBirthday;
        public string BnfBirthday
        {
            get { return _bnfBirthday; }
            set
            {
                _bnfBirthday = value;
                OnPropertyChanged("BnfBirthday");
            }
        }
        private string _bnfSIN;
        public string BnfSIN
        {
            get { return _bnfSIN; }
            set
            {
                _bnfSIN = value;
                OnPropertyChanged("BnfSIN");
            }
        }
        private string _bnfType;
        public string BnfType
        {
            get { return _bnfType; }
            set
            {
                _bnfType = value;
                OnPropertyChanged("BnfType");
            }
        }
        private string _bnfRelationship;
        public string BnfRelationship
        {
            get { return _bnfRelationship; }
            set
            {
                _bnfRelationship = value;
                OnPropertyChanged("BnfRelationship");
            }
        }
        private string _revokable;
        public string Revokable
        {
            get { return _revokable; }
            set
            {
                _revokable = value;
                OnPropertyChanged("Revokable");
            }
        }
        private string _bnfPercentage;
        public string BnfPercentage
        {
            get { return _bnfPercentage; }
            set
            {
                _bnfPercentage = value;
                OnPropertyChanged("BnfPercentage");
            }
        }
        private string _trusteePID;
        public string TrusteePID
        {
            get { return _trusteePID; }
            set
            {
                _trusteePID = value;
                OnPropertyChanged("TrusteePID");
            }
        }
        private string _trusteeFirstName;
        public string TrusteeFirstName
        {
            get { return _trusteeFirstName; }
            set
            {
                _trusteeFirstName = value;
                OnPropertyChanged("TrusteeFirstName");
            }
        }
        private string _trusteeLastName;
        public string TrusteeLastName
        {
            get { return _trusteeLastName; }
            set
            {
                _trusteeLastName = value;
                OnPropertyChanged("TrusteeLastName");
            }
        }
        private string _trusteeBirthday;
        public string TrusteeBirthday
        {
            get { return _trusteeBirthday; }
            set
            {
                _trusteeBirthday = value;
                OnPropertyChanged("TrusteeBirthday");
            }
        }
        private string _trRelationship;
        public string TrRelationship
        {
            get { return _trRelationship; }
            set
            {
                _trRelationship = value;
                OnPropertyChanged("TrRelationship");
            }
        }
        //private string _startDate;
        //public string StartDate
        //{
        //    get { return _startDate; }
        //    set
        //    {
        //        _startDate = value;
        //        OnPropertyChanged("StartDate");
        //    }
        //}
        //private string _endDate;
        //public string EndDate
        //{
        //    get { return _endDate; }
        //    set
        //    {
        //        _endDate = value;
        //        OnPropertyChanged("EndDate");
        //    }
        //}
        private string _currentFlag;
        public string CurrentFlag
        {
            get { return _currentFlag; }
            set
            {
                _currentFlag = value;
                OnPropertyChanged("CurrentFlag");
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
        //private string _updateDate;
        //public string UpdateDate
        //{
        //    get { return _updateDate; }
        //    set
        //    {
        //        _updateDate = value;
        //        OnPropertyChanged("UpdateDate");
        //    }
        //}
        #endregion Public Interface
    }
}
