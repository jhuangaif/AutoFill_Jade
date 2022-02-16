using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Income : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Income()
        {
            _incomeType = "";
            _incomeAmount = "";
            _InstituionName = "";
            _verifyDate = "";
            _notes = "";
        }
        #endregion Constructor

        #region Public Interface    
        private string _incomeType;
        public string IncomeType
        {
            get { return _incomeType; }
            set
            {
                _incomeType = value;
                OnPropertyChanged("IncomeType");
            }
        }
        private string _incomeAmount;
        public string IncomeAmount
        {
            get { return _incomeAmount; }
            set
            {
                _incomeAmount = value;
                OnPropertyChanged("IncomeAmount");
            }
        }
        private string _InstituionName;
        public string InstituionName  //Employer, Property Addresss,......
        {
            get { return _InstituionName; }
            set
            {
                _InstituionName = value;
                OnPropertyChanged("InstituionName");
            }
        }
        private string _verifyDate;
        public string verifyDate
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
