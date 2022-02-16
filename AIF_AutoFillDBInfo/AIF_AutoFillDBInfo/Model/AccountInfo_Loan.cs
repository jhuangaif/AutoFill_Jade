using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class AccountInfo_Loan : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public AccountInfo_Loan()
        {
            _id = "";
            _isCompleted = false;
            _applyYear = "";
            _applyMonth = "";
            _applyDay = "";
            _applyAmount = "";
            _accountType = "";
            _investmentTo = "";
            _loanFrom = "";
            _loanNo = "";
            _loanType= "";
            _salesCharge= "";
            _fundCode1 = "";
            _fundCode2 = "";
            _fundCode3 = "";
            _fundCode4 = "";
            _fundCode5 = "";
            _fundCode6 = "";
            _fundCode7 = "";
            _fundCode8 = "";
            _chequeAccount = "";
            _chequeTransit = "";
            _chequeInstitution = "";
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
        private string _loanFrom;
        public string LoanFrom
        {
            get { return _loanFrom; }
            set
            {
                _loanFrom = value;
                OnPropertyChanged("LoanFrom");
            }
        }

        private string _loanNo;
        public string LoanNo
        {
            get { return _loanNo; }
            set
            {
                _loanNo = value;
                OnPropertyChanged("LoanNo");
            }
        }

        private string _loanType;
        public string LoanType
        {
            get { return _loanType; }
            set
            {
                _loanType = value;
                OnPropertyChanged("LoanType");
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
        private string _chequeAccount;
        public string ChequeAccount
        {
            get { return _chequeAccount; }
            set
            {
                _chequeAccount = value;
                OnPropertyChanged("ChequeAccount");
            }
        }
        private string _chequeTransit;
        public string ChequeTransit
        {
            get { return _chequeTransit; }
            set
            {
                _chequeTransit = value;
                OnPropertyChanged("ChequeTransit");
            }
        }
        private string _chequeInstitution;
        public string ChequeInstitution
        {
            get { return _chequeInstitution; }
            set
            {
                _chequeInstitution = value;
                OnPropertyChanged("ChequeInstitution");
            }
        }
        private string _chequeBankAddress;
        public string ChequeBankAddress
        {
            get { return _chequeBankAddress; }
            set
            {
                _chequeBankAddress = value;
                OnPropertyChanged("ChequeBankAddress");
            }
        }
        #endregion Public Interface
    }
}
