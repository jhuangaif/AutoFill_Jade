using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Investment : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Investment()
        {
            _investmentNo = "";
            _openDate = "";
            _coAdvisor = false;
            _advisor1 = new Advisor();
            _advisor2 = new Advisor(); 
            _coApplicationFlag = false;
            _applicant = new Person();
            _coApplicant = new Person();
            _capitalSourceType = "";//CapitalSourceEnum.None;//OwnFund-OneTimePAD,OwnFund-RegularPAD, Loan, Transfer, Mixed
            _SourceOF = new OwnFund();
            _SourceLoan =  new Loan();
            _SourceTF = new Transfer();
            //_planAmount = "";            
            _investTo = "";
            _submitDate = "";
            _policyNo = "";
            _settleAmount = "";
            _investmentBeneficiary = new List<Beneficiary>();
            _KYC = new KYC();
            _investmentFunds = new List<Funds>();
        }
        #endregion Constructor

        #region Public Interface
        private string _investmentNo;
        public string InvestmentNo
        {
            get { return _investmentNo; }
            set
            {
                _investmentNo = value;
                OnPropertyChanged("InvestmentNo");
            }
        }
        private string _openDate;
        public string OpenDate
        {
            get { return _openDate; }
            set
            {
                _openDate = value;
                OnPropertyChanged("OpenDate");
            }
        }
        private Advisor _advisor1;
        public Advisor Advisor1
        {
            get { return _advisor1; }
            set
            {
                _advisor1 = value;
                OnPropertyChanged("Advisor1");
            }
        }
        private Advisor _advisor2;
        public Advisor Advisor2
        {
            get { return _advisor2; }
            set
            {
                _advisor2 = value;
                OnPropertyChanged("Advisor2");
            }
        }
        private Person _applicant;
        public Person Applicant
        {
            get { return _applicant; }
            set
            {
                _applicant = value;
                OnPropertyChanged("Applicant");
            }
        }
        private Person _coApplicant;
        public Person CoApplicant
        {
            get { return _coApplicant; }
            set
            {
                _coApplicant = value;
                OnPropertyChanged("CoApplicant");
            }
        }
        private bool _coAdvisor;
        public bool CoAdvisor
        {
            get { return _coAdvisor; }
            set
            {
                _coAdvisor = value;
                OnPropertyChanged("CoAdvisor");
            }
        }
        private bool _coApplicationFlag;
        public bool CoApplicationFlag
        {
            get { return _coApplicationFlag; }
            set
            {
                _coApplicationFlag = value;
                OnPropertyChanged("CoAppFlag");
            }
        }
        private string _capitalSourceType;
        public string CapitalSourceType
        {
            get { return _capitalSourceType; }
            set
            {
                _capitalSourceType = value;
                OnPropertyChanged("CapitalSourceType");
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
        private OwnFund _SourceOF;
        public OwnFund SourceOF  
        {
            get { return _SourceOF; }
            set
            {
                _SourceOF = value;
                OnPropertyChanged("SourceOF");
            }
        }
        private Loan _SourceLoan;
        public Loan SourceLoan  
        {
            get { return _SourceLoan; }
            set
            {
                _SourceLoan = value;
                OnPropertyChanged("SourceLoan");
            }
        }
        private Transfer _SourceTF;
        public Transfer SourceTF
        {
            get { return _SourceTF; }
            set
            {
                _SourceTF = value;
                OnPropertyChanged("SourceTF");
            }
        }
        
        private string _investTo;
        public string InvestTo
        {
            get { return _investTo; }
            set
            {
                _investTo = value;
                OnPropertyChanged("InvestTo");
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
        private KYC _KYC;
        public KYC KYC
        {
            get { return _KYC; }
            set
            {
                _KYC = value;
                OnPropertyChanged("KYC");
            }
        }
        //private string _payMethod;
        //public string PayMethod
        //{
        //    get { return _payMethod; }
        //    set
        //    {
        //        _payMethod = value;
        //        OnPropertyChanged("PayMethod");
        //    }
        //}
        //private string _paymentType;
        //public string PaymentType
        //{
        //    get { return _paymentType; }
        //    set
        //    {
        //        _paymentType = value;
        //        OnPropertyChanged("PaymentType");
        //    }
        //}
        //private string _PADFrequency;
        //public string PADFrequency
        //{
        //    get { return _PADFrequency; }
        //    set
        //    {
        //        _PADFrequency = value;
        //        OnPropertyChanged("PADFrequency");
        //    }
        //}
        //private string _onetimePADdate;
        //public string OnetimePADdate
        //{
        //    get { return _onetimePADdate; }
        //    set
        //    {
        //        _onetimePADdate = value;
        //        OnPropertyChanged("OnetimePADdate");
        //    }
        //}
        private List<Beneficiary> _investmentBeneficiary;
        public List<Beneficiary> InvestmentBeneficiary
        {
            get { return _investmentBeneficiary; }
            set
            {
                _investmentBeneficiary = value;
                OnPropertyChanged("InvestmentBeneficiary");
            }
        }
        private List<Funds> _investmentFunds;
        public List<Funds> InvestmentFunds
        {
            get { return _investmentFunds; }
            set
            {
                _investmentFunds = value;
                OnPropertyChanged("InvestmentFunds");
            }
        }
        private string _submitDate;
        public string SubmitDate
        {
            get { return _submitDate; }
            set
            {
                _submitDate = value;
                OnPropertyChanged("SubmitDate");
            }
        }
        private string _policyNo;
        public string PolicyNo
        {
            get { return _policyNo; }
            set
            {
                _policyNo = value;
                OnPropertyChanged("PolicyNo");
            }
        }
        private string _settleAmount;
        public string SettleAmount
        {
            get { return _settleAmount; }
            set
            {
                _settleAmount = value;
                OnPropertyChanged("SettleAmount");
            }
        }
        #endregion Public Interface
    }
}
