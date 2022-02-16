using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Transfer : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public Transfer()
        {
            _id = "";
            _applyDate = "";
            _transferAmount = "";
            _transferMethod = "";
            _transferPercent = "";
            _transferFromAccountInfo = new Cheque();            
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
        //private bool _isCompleted;
        //public bool IsCompleted
        //{
        //    get { return _isCompleted; }
        //    set
        //    {
        //        _isCompleted = value;
        //        OnPropertyChanged("IsCompleted");
        //    }
        //}
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
        //private string _applyMonth;
        //public string ApplyMonth
        //{
        //    get { return _applyMonth; }
        //    set
        //    {
        //        _applyMonth = value;
        //        OnPropertyChanged("ApplyMonth");
        //    }
        //}
        //private string _applyDay;
        //public string ApplyDay
        //{
        //    get { return _applyDay; }
        //    set
        //    {
        //        _applyDay = value;
        //        OnPropertyChanged("ApplyDay");
        //    }
        //}
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
        //private string _accountType;
        //public string AccountType
        //{
        //    get { return _accountType; }
        //    set
        //    {
        //        _accountType = value;
        //        OnPropertyChanged("AccountType");
        //    }
        //}
        //private string _investmentTo;
        //public string InvestmentTo
        //{
        //    get { return _investmentTo; }
        //    set
        //    {
        //        _investmentTo = value;
        //        OnPropertyChanged("InvestmentTo");
        //    }
        //}
        private string _transferMethod; //Whole Amount, All in cash, Partial, All Muture Funds
        public string TransferMethod
        {
            get { return _transferMethod; }
            set
            {
                _transferMethod = value;
                OnPropertyChanged("TransferMethod");
            }
        }
        private string _transferPercent;
        public string TransferPercent //transfer partial percentage
        {
            get { return _transferPercent; }
            set
            {
                _transferPercent = value;
                OnPropertyChanged("TransferPercent");
            }
        }
        private Cheque _transferFromAccountInfo; //Account no, Relinquishing Institution name, Address
        public Cheque TransferFromAccountInfo
        {
            get { return _transferFromAccountInfo; }
            set
            {
                _transferFromAccountInfo = value;
                OnPropertyChanged("TransferFromAccountInfo");
            }
        }
        //private string _relinquishingInsName;
        //public string RelinquishingInsName
        //{
        //    get { return _relinquishingInsName; }
        //    set
        //    {
        //        _relinquishingInsName = value;
        //        OnPropertyChanged("RelinquishingInsName");
        //    }
        //}

        #endregion Public Interface
    }
}
