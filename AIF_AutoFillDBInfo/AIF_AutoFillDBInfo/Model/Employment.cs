using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Employment : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Employment()
        {
            _emplStatus = "";
            _employer = "";
            _industry = "";
            _occupation = "";
            _unit = "";
            _stNo = "";
            _stName = "";
            _city = "";
            _prov = "";
            _country = "";
            _postCode = "";
            _workPhone = "";
            _income = "";
            _startDate = "";
            _endDate = "";
            _currentFlag = true;
            _verifyDate = "";
            _notes = "";
            _workSinceYear = "";
            _workSinceMonth = "";
        }
        #endregion Constructor

        #region Public Interface    
        private string _emplStatus;
        public string EmplStatus
        {
            get { return _emplStatus; }
            set
            {
                _emplStatus = value;
                OnPropertyChanged("EmplStatus");
            }
        }
        private string _employer;
        public string Employer
        {
            get { return _employer; }
            set
            {
                _employer = value;
                OnPropertyChanged("Employer");
            }
        }
        private string _industry;
        public string Industry
        {
            get { return _industry; }
            set
            {
                _industry = value;
                OnPropertyChanged("Industry");
            }
        }
        private string _occupation;
        public string Occupation
        {
            get { return _occupation; }
            set
            {
                _occupation = value;
                OnPropertyChanged("Occupation");
            }
        }
        private string _unit;
        public string Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                OnPropertyChanged("Unit");
            }
        }
        private string _stNo;
        public string StNo
        {
            get { return _stNo; }
            set
            {
                _stNo = value;
                OnPropertyChanged("StNo");
            }
        }
        private string _stName;
        public string StName
        {
            get { return _stName; }
            set
            {
                _stName = value;
                OnPropertyChanged("stName");
            }
        }
        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("City");
            }
        }
        private string _prov;
        public string Prov
        {
            get { return _prov; }
            set
            {
                _prov = value;
                OnPropertyChanged("Prov");
            }
        }
        private string _country;
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged("Country");
            }
        }
        private string _postCode;
        public string PostCode
        {
            get { return _postCode; }
            set
            {
                _postCode = value;
                OnPropertyChanged("PostCode");
            }
        }
        private string _workPhone;
        public string WorkPhone
        {
            get { return _workPhone; }
            set
            {
                _workPhone = value;
                OnPropertyChanged("WorkPhone");
            }
        }
        private string _income;
        public string Income
        {
            get { return _income; }
            set
            {
                _income = value;
                OnPropertyChanged("Income");
            }
        }
        private string _startDate;
        public string StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        private string _endDate;
        public string EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }
        private bool _currentFlag;
        public bool CurrentFlag
        {
            get { return _currentFlag; }
            set
            {
                _currentFlag = value;
                OnPropertyChanged("CurrentFlag");
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
        private string _workSinceYear;
        public string WorkSinceYear
        {
            get { return _workSinceYear; }
            set
            {
                _workSinceYear = value;
                OnPropertyChanged("WorkSinceYear");
            }
        }
        private string _workSinceMonth;
        public string WorkSinceMonth
        {
            get { return _workSinceMonth; }
            set
            {
                _workSinceMonth = value;
                OnPropertyChanged("WorkSinceMonth");
            }
        }
        #endregion Public Interface
    }
}
