using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class SwitchFundConditions : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public SwitchFundConditions()
        {
            _contractNo = "";
            
        }

        #endregion Constructor

        #region Public Interface    

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
        private string _originalFund;
        public string OriginalFund
        {
            get
            {
                return _originalFund;
            }
            set
            {
                _originalFund = value;
                OnPropertyChanged("OriginalFund");
            }
        }
        private string _condition;
        public string Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                _condition = value;
                OnPropertyChanged("Condition");
            }
        }
        private string _percentage;
        public string Percentage
        {
            get
            {
                return _percentage;
            }
            set
            {
                _percentage = value;
                OnPropertyChanged("Percentage");
            }
        }
        private Funds _transferCode1;
        public Funds TransferCode1
        {
            get
            {
                return _transferCode1;
            }
            set
            {
                _transferCode1 = value;
                OnPropertyChanged("TransferCode1");
            }
        }
        private Funds _transferCode2;
        public Funds TransferCode2
        {
            get
            {
                return _transferCode2;
            }
            set
            {
                _transferCode2 = value;
                OnPropertyChanged("TransferCode2");
            }
        }
        private Funds _transferCode3;
        public Funds TransferCode3
        {
            get
            {
                return _transferCode3;
            }
            set
            {
                _transferCode3 = value;
                OnPropertyChanged("TransferCode3");
            }
        }
        private Funds _transferCode4;
        public Funds TransferCode4
        {
            get
            {
                return _transferCode4;
            }
            set
            {
                _transferCode4 = value;
                OnPropertyChanged("TransferCode4");
            }
        }
        private Funds _transferCode5;
        public Funds TransferCode5
        {
            get
            {
                return _transferCode5;
            }
            set
            {
                _transferCode5 = value;
                OnPropertyChanged("TransferCode5");
            }
        }
        private Funds _transferCode6;
        public Funds TransferCode6
        {
            get
            {
                return _transferCode6;
            }
            set
            {
                _transferCode6 = value;
                OnPropertyChanged("TransferCode6");
            }
        }
        #endregion Public Interface
    }
}
