using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class AccountInfo_OwnFunds : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public AccountInfo_OwnFunds()
        {
            _id = "";
            _isCompleted = false;
            _applyYear = "";
            _applyMonth = "";
            _applyDay = "";
            _applyAmount = "";
            _accountType = "";
            _investmentTo = "";
            _payMethod = "";
            _invInstruction = "";
            _onetimePADdate = "";
            _accountNo = "";
            _transitNo = "";
            _institutionNo = "";
            _institutionName = "";
            _accountOwnerName = "";
            _regularPAD1stDate = "";
            _frequency = "";
            _regularPADday = "";
            _salesCharge = "";
            _contractNo = "";
            _fundCode1 = "";
            _fundCode2 = "";
            _fundCode3 = "";
            _fundCode4 = "";
            _fundCode5 = "";
            _fundCode6 = "";
            _fundCode7 = "";
            _fundCode8 = "";

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
        private string _applyAmount;
        public string ApplyAmount
        {
            get { return _applyAmount; }
            set
            {
                _applyAmount = value;
                OnPropertyChanged("ApplyAmount");
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
        private string _payMethod;
        public string PayMethod
        {
            get { return _payMethod; }
            set
            {
                _payMethod = value;
                OnPropertyChanged("PayMethod");
            }
        }
        private string _invInstruction;
        public string InvInstruction
        {
            get { return _invInstruction; }
            set
            {
                _invInstruction = value;
                OnPropertyChanged("InvInstruction");
            }
        }
        private string _onetimePADdate;
        public string OnetimePADdate
        {
            get { return _onetimePADdate; }
            set
            {
                _onetimePADdate = value;
                OnPropertyChanged("OnetimePADdate");
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
        private string _transitNo;
        public string TransitNo
        {
            get { return _transitNo; }
            set
            {
                _transitNo = value;
                OnPropertyChanged("TransitNo");
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
        private string _accountOwnerName;
        public string AccountOwnerName
        {
            get { return _accountOwnerName; }
            set
            {
                _accountOwnerName = value;
                OnPropertyChanged("AccountOwnerName");
            }
        }
        private string _regularPAD1stDate;
        public string RegularPAD1stDate
        {
            get { return _regularPAD1stDate; }
            set
            {
                _regularPAD1stDate = value;
                OnPropertyChanged("RegularPAD1stDate");
            }
        }
        private string _frequency;
        public string Frequency
        {
            get { return _frequency; }
            set
            {
                _frequency = value;
                OnPropertyChanged("Frequency");
            }
        }
        private string _regularPADday;
        public string RegularPADday
        {
            get { return _regularPADday; }
            set
            {
                _regularPADday = value;
                OnPropertyChanged("RegularPADday");
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
        private string _contractNo;
        public string ContractNo
        {
            get { return _contractNo; }
            set
            {
                _contractNo = value;
                OnPropertyChanged("ContractNo");
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
