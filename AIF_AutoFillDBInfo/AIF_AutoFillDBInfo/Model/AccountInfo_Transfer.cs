using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class AccountInfo_Transfer : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public AccountInfo_Transfer()
        {
            _id = "";
            _isCompleted = false;
            _applyYear = "";
            _applyMonth = "";
            _applyDay = "";
            _transferType = "";
            _transferAmount = "";
            _transferMethod = "";
            _accountNo = "";
            _accountType = "";
            _investmentTo = "";
            _relinquishingInstitutionName = "";
            _institutionAddress = "";
            _institutionCity = "";
            _institutionProvince = "";
            _institutionPostcode = "";
            _salesCharge = "";
            _fundCode1 = "";
            _fundCode2 = "";
            _fundCode3 = "";
            _fundCode4 = "";
            _fundCode5 = "";
            _fundCode6 = "";
            _fundCode7 = "";
            _fundCode8 = "";
            _contractno = "";

        }

        #endregion Constructor

        #region Public Interface    

        private string _id;
        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }
        private bool _isCompleted;
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                OnPropertyChanged("IsCompleted");
            }
        }
        private string _applyYear;
        public string ApplyYear
        {
            get { return _applyYear; }
            set
            {
                _applyYear = value;
                OnPropertyChanged("ApplyYear");
            }
        }
        private string _applyMonth;
        public string ApplyMonth
        {
            get { return _applyMonth; }
            set
            {
                _applyMonth = value;
                OnPropertyChanged("ApplyMonth");
            }
        }
        private string _applyDay;
        public string ApplyDay
        {
            get { return _applyDay; }
            set
            {
                _applyDay = value;
                OnPropertyChanged("ApplyDay");
            }
        }
        private string _transferAmount;
        public string TransferAmount
        {
            get { return _transferAmount; }
            set
            {
                _transferAmount = value;
                OnPropertyChanged("TransferAmount");
            }
        }
        private string _accountType;
        public string AccountType
        {
            get { return _accountType; }
            set
            {
                _accountType = value;
                OnPropertyChanged("AccountType");
            }
        }
        private string _investmentTo;
        public string InvestmentTo
        {
            get { return _investmentTo; }
            set
            {
                _investmentTo = value;
                OnPropertyChanged("InvestmentTo");
            }
        }
        private string _transferMethod;
        public string TransferMethod
        {
            get { return _transferMethod; }
            set
            {
                _transferMethod = value;
                OnPropertyChanged("TransferMethod");
            }
        }
        private string _transferType;
        public string TransferType
        {
            get { return _transferType; }
            set
            {
                _transferType = value;
                OnPropertyChanged("TransferType");
            }
        }
        private string _relinquishingInstitutionName;
        public string RelinquishingInstitutionName
        {
            get { return _relinquishingInstitutionName; }
            set
            {
                _relinquishingInstitutionName = value;
                OnPropertyChanged("RelinquishingInstitutionName");
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
        private string _institutionAddress;
        public string InstitutionAddress
        {
            get { return _institutionAddress; }
            set
            {
                _institutionAddress = value;
                OnPropertyChanged("InstitutionAddress");
            }
        }
        private string _institutionCity;
        public string InstitutionCity
        {
            get { return _institutionCity; }
            set
            {
                _institutionCity = value;
                OnPropertyChanged("InstitutionCity");
            }
        }
        private string _institutionProvince;
        public string InstitutionProvince
        {
            get { return _institutionProvince; }
            set
            {
                _institutionProvince = value;
                OnPropertyChanged("InstitutionProvince");
            }
        }
        private string _institutionPostcode;
        public string InstitutionPostcode
        {
            get { return _institutionPostcode; }
            set
            {
                _institutionPostcode = value;
                OnPropertyChanged("InstitutionPostcode");
            }
        }
        
        private string _salesCharge;
        public string SalesCharge
        {
            get { return _salesCharge; }
            set
            {
                _salesCharge = value;
                OnPropertyChanged("SalesCharge");
            }
        }
        private string _contractno;
        public string Contractno
        {
            get { return _contractno; }
            set
            {
                _contractno = value;
                OnPropertyChanged("Contractno");
            }
        }
        private string _fundCode1;
        public string FundCode1
        {
            get { return _fundCode1; }
            set
            {
                _fundCode1 = value;
                OnPropertyChanged("FundCode1");
            }
        }
        private string _fundCode2;
        public string FundCode2
        {
            get { return _fundCode2; }
            set
            {
                _fundCode2 = value;
                OnPropertyChanged("FundCode2");
            }
        }
        private string _fundCode3;
        public string FundCode3
        {
            get { return _fundCode3; }
            set
            {
                _fundCode3 = value;
                OnPropertyChanged("FundCode3");
            }
        }
        private string _fundCode4;
        public string FundCode4
        {
            get { return _fundCode4; }
            set
            {
                _fundCode4 = value;
                OnPropertyChanged("FundCode4");
            }
        }
        private string _fundCode5;
        public string FundCode5
        {
            get { return _fundCode5; }
            set
            {
                _fundCode5 = value;
                OnPropertyChanged("FundCode5");
            }
        }
        private string _fundCode6;
        public string FundCode6
        {
            get { return _fundCode6; }
            set
            {
                _fundCode6 = value;
                OnPropertyChanged("FundCode6");
            }
        }
        private string _fundCode7;
        public string FundCode7
        {
            get { return _fundCode7; }
            set
            {
                _fundCode7 = value;
                OnPropertyChanged("FundCode7");
            }
        }
        private string _fundCode8;
        public string FundCode8
        {
            get { return _fundCode8; }
            set
            {
                _fundCode8 = value;
                OnPropertyChanged("FundCode8");
            }
        }
        #endregion Public Interface
    }
}
