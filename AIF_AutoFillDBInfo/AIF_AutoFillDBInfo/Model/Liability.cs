using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Liability : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Liability()
        {
            _liType = "";
            _liBalance = "";
            _liMonthlyPayt = "";
            _institution = "";            
            _verifyDate = "";
            _notes = "";
        }
        #endregion Constructor

        #region Public Interface    
        private string _liType;
        public string LiType
        {
            get { return _liType; }
            set
            {
                _liType = value;
                OnPropertyChanged("LiType");
            }
        }
        private string _liBalance;
        public string LiBalance
        {
            get { return _liBalance; }
            set
            {
                _liBalance = value;
                OnPropertyChanged("LiBalance");
            }
        }
        private string _liMonthlyPayt;
        public string LiMonthlyPayt
        {
            get { return _liMonthlyPayt; }
            set
            {
                _liMonthlyPayt = value;
                OnPropertyChanged("LiMonthlyPayt");
            }
        }
        private string _institution;
        public string Institution
        {
            get { return _institution; }
            set
            {
                _institution = value;
                OnPropertyChanged("Institution");
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
        #endregion Public Interface
    }
}
