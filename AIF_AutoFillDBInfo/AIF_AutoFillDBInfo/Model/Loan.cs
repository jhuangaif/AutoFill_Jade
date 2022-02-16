using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;
using iTextSharp.text.pdf;

namespace AIFAutoFillDB.Model
{
    public class Loan : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public Loan()
        {
            _loanNo = "";
            _applyDate = "";
            //_advisorInfo = new Advisor();
            //_applicant = new Person();
            //_coapplicationFlag = false;
            //_coApplicant = new Person();
            _tdsr = "";
            _tdsrVerifyDate = "";
            //_submitDate = "";
            _loanFrom = "";
            _applyAmount = "";
            _loanType = "";
            _settleDate = "";
            _settleAmount = "";           
            _notes = "";
            _paymentcheque = new Cheque(); //interest payment cheque
        }
        #endregion Constructor

        #region Public Interface    
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
        private string _applyDate;
        public string ApplyDate
        {
            get { return _applyDate; }
            set
            {
                _applyDate = value;
                OnPropertyChanged("ApplyDate");
            }
        }
        //private Advisor _advisorInfo;
        //public Advisor AdvisorInfo
        //{
        //    get { return _advisorInfo; }
        //    set
        //    {
        //        _advisorInfo = value;
        //        OnPropertyChanged("AdvisorInfo");
        //    }
        //}
        //private Person _applicant;
        //public Person Applicant
        //{
        //    get { return _applicant; }
        //    set
        //    {
        //        _applicant = value;
        //        OnPropertyChanged("Applicant");
        //    }
        //}

        //private bool _coapplicationFlag;
        //public bool CoapplicationFlag
        //{
        //    get { return _coapplicationFlag; }
        //    set
        //    {
        //        _coapplicationFlag = value;
        //        OnPropertyChanged("CoapplicationFlag");
        //    }
        //}
        //private Person _coApplicant;
        //public Person CoApplicant
        //{
        //    get { return _coApplicant; }
        //    set
        //    {
        //        _coApplicant = value;
        //        OnPropertyChanged("CoApplicant");
        //    }
        //}
        private string _tdsr;
        public string Tdsr
        {
            get { return _tdsr; }
            set
            {
                _tdsr = value;
                OnPropertyChanged("Tdsr");
            }
        }
        private string _tdsrVerifyDate;
        public string TdsrVerifyDate
        {
            get { return _tdsrVerifyDate; }
            set
            {
                _tdsrVerifyDate = value;
                OnPropertyChanged("TdsrVerifyDate");
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
        private string _settleDate;
        public string SettleDate
        {
            get { return _settleDate; }
            set
            {
                _settleDate = value;
                OnPropertyChanged("SettleDate");
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
        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                OnPropertyChanged("Notes");
            }
        }
        private Cheque _paymentcheque;
        public Cheque Paymentcheque
        {
            get { return _paymentcheque; }
            set
            {
                _paymentcheque = value;
                OnPropertyChanged("Paymentcheque");
            }
        }

        public AcroFields fillAcroFields(AcroFields fields, PdfStamper pdfStamper)
        {
            // WIP...
            return fields;
        }
        #endregion Public Interface
    }
}
