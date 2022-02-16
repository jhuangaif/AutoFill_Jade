using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class ID : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public ID()
        {
            _pid="";
            _idType = "";
            _idNumber = "";
            _issueDate = "";
            _expiryDate = "";
            _issueCountry = "";
            _issueProvince = "";
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
        private string _idType;
        public string IdType
        {
            get { return _idType; }
            set
            {
                _idType = value;
                OnPropertyChanged("IdType");
            }
        }
        private string _idNumber;
        public string IdNumber
        {
            get { return _idNumber; }
            set
            {
                _idNumber = value;
                OnPropertyChanged("IdNumber");
            }
        }
        private string _issueDate;
        public string IssueDate
        {
            get { return _issueDate; }
            set
            {
                _issueDate = value;
                OnPropertyChanged("IssueDate");
            }
        }
        private string _expiryDate;
        public string ExpiryDate
        {
            get { return _expiryDate; }
            set
            {
                _expiryDate = value;
                OnPropertyChanged("ExpiryDate");
            }
        }
        private string _issueCountry;
        public string IssueCountry
        {
            get { return _issueCountry; }
            set
            {
                _issueCountry = value;
                OnPropertyChanged("IssueCountry");
            }
        }
        private string _issueProvince;
        public string IssueProvince
        {
            get { return _issueProvince; }
            set
            {
                _issueProvince = value;
                OnPropertyChanged("IssueProvince");
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
