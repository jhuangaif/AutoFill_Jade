using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class FundTransaction : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public FundTransaction()
        {
            _fund = "";
            _netAmount = "";
            _unitValue = "";
            _numberOfUnit = ""; 
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

        private string _effectiveDate;
        public string EffectiveDate
        {
            get { return _effectiveDate; }
            set
            {
                _effectiveDate = value;
                OnPropertyChanged("EffectiveDate");
            }
        }
        private string _fund;
        public string Fund
        {
            get { return _fund; }
            set
            {
                _fund = value;
                OnPropertyChanged("Fund");
            }
        }
        private string _netAmount;
        public string NetAmount
        {
            get { return _netAmount; }
            set
            {
                _netAmount = value;
                OnPropertyChanged("NetAmount");
            }
        }
        private string _unitValue;
        public string UnitValue
        {
            get { return _unitValue; }
            set
            {
                _unitValue = value;
                OnPropertyChanged("UnitValue");
            }
        }
        private string _numberOfUnit;
        public string NamberOfUnit
        {
            get { return _numberOfUnit; }
            set
            {
                _numberOfUnit = value;
                OnPropertyChanged("NamberOfUnit");
            }
        }

        #endregion Public Interface
    }
}
