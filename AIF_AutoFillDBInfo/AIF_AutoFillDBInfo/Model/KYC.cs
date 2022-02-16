using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class KYC : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public KYC()
        {
            _KYC_QS_No = "";
            _ansrNo = "";
            _ansrScore = "";
            _verifyDate = "";


        }
        #endregion Constructor

        #region Public Interface    
        private string _KYC_QS_No;
        public string KYC_QS_No
        {
            get { return _KYC_QS_No; }
            set
            {
                _KYC_QS_No = value;
                OnPropertyChanged("KYC_QS_No");
            }
        }
        private string _ansrNo;
        public string ANSRNo
        {
            get { return _ansrNo; }
            set
            {
                _ansrNo = value;
                OnPropertyChanged("ANSRNo");
            }
        }
        private string _ansrScore;
        public string ANSRScore
        {
            get { return _ansrScore; }
            set
            {
                _ansrScore = value;
                OnPropertyChanged("ANSRScore");
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
        #endregion Public Interface
    }
}
