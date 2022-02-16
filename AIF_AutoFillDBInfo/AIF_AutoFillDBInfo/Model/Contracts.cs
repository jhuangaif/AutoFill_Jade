using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Contracts : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public Contracts()
        {
            _contractNo = "";
            _clientName = "";
            _clientDOB = DateTime.Now.ToString("yyyyMMdd");
            _lastTransactionDate = DateTime.Now.ToString("yyyyMMdd");
            _contractFunds = new List<Funds>();
        }

        #endregion Constructor

        #region Public Interface    

        private bool _isSwitch;
        public bool IsSwitch
        {
            get { return _isSwitch; }
            set
            {
                _isSwitch = value;
                OnPropertyChanged("IsSwitch");
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

        private string _toInstitution;
        public string ToInstitution
        {
            get { return _toInstitution; }
            set
            {
                _toInstitution = value;
                OnPropertyChanged("ToInstitution");
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
        private string _fundCapitalSource;
        public string FundCapitalSource
        {
            get { return _fundCapitalSource; }
            set
            {
                _fundCapitalSource = value;
                OnPropertyChanged("FundCapitalSource");
            }
        }
        private string _clientName;
        public string ClientName
        {
            get { return _clientName; }
            set
            {
                _clientName = value;
                OnPropertyChanged("ClientName");
            }
        }

        private string _clientDOB;
        public string ClientDOB
        {
            get { return _clientDOB; }
            set
            {
                _clientDOB = value;
                OnPropertyChanged("ClientDOB");
            }
        }
        private string _contractStatus;
        public string ContractStatus
        {
            get { return _contractStatus; }
            set
            {
                _contractStatus = value;
                OnPropertyChanged("ContractStatus");
            }
        }
        private string _lastTransactionDate;
        public string LastTransactionDate
        {
            get { return _lastTransactionDate; }
            set
            {
                _lastTransactionDate = value;
                OnPropertyChanged("LastTransactionDate");
            }
        }
        private List<Funds> _contractFunds;
        public List<Funds> ContractFunds
        {
            get { return _contractFunds; }
            set
            {
                _contractFunds = value;
                OnPropertyChanged("ContractFunds");
            }
        }
        private string _advisorName;
        public string AdvisorName
        {
            get { return _advisorName; }
            set
            {
                _advisorName = value;
                OnPropertyChanged("AdvisorName");
            }
        }
        private string _advisorCode;
        public string AdvisorCode
        {
            get { return _advisorCode; }
            set
            {
                _advisorCode = value;
                OnPropertyChanged("AdvisorCode");
            }
        }
        private bool _isClicked;
        public bool IsClicked
        {
            get { return _isClicked; }
            set
            {
                _isClicked = value;
                OnPropertyChanged("IsClicked");
            }
        }
        #endregion Public Interface
    }
}
