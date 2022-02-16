using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Address : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Address()
        {
            _pid = "";
            _aptNo = "";
            _streetNo = "";
            _streetName = "";
            _city = "";
            _province = "";
            _country = "";
            _postcode = "";
            //_homephone = "";
            _livingStatus = "";
            _startDate = "";
            _endDate = "";
            _currentFlag = true;
            _verifyDate = "";
            _notes = "";
        }
        #endregion Constructor

        #region Public Interface    
        private string _pid;
        public string PID
        {
            get { return _pid; }
            set
            {
                _pid = value;
                OnPropertyChanged("PID");
            }
        }
        private string _aptNo;
        public string AptNo
        {
            get { return _aptNo; }
            set
            {
                _aptNo = value;
                OnPropertyChanged("AptNo");
            }
        }
        private string _streetNo;
        public string StreetNo
        {
            get { return _streetNo; }
            set
            {
                _streetNo = value;
                OnPropertyChanged("StreetNo");
            }
        }
        private string _streetName;
        public string StreetName
        {
            get { return _streetName; }
            set
            {
                _streetName = value;
                OnPropertyChanged("StreetName");
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
        private string _province;
        public string Province
        {
            get { return _province; }
            set
            {
                _province = value;
                OnPropertyChanged("Province");
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
        private string _postcode;
        public string Postcode
        {
            get { return _postcode; }
            set
            {
                _postcode = value;
                OnPropertyChanged("Postcode");
            }
        }
        private string _homephone;
        public string Homephone
        {
            get { return _homephone; }
            set
            {
                _homephone = value;
                OnPropertyChanged("Homephone");
            }
        }
        private string _livingStatus;
        public string LivingStatus
        {
            get { return _livingStatus; }
            set
            {
                _livingStatus = value;
                OnPropertyChanged("LivingStatus");
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
        #endregion Public Interface
    }
}
