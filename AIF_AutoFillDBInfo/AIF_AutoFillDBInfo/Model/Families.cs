using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Families : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Families()
        {
            _primaryPID = "";
            _memberPID = "";
            _relationship = "";
            _currentFlag = "";
            _updateDate = "";
            _notes = "";
        }
        #endregion Constructor

        #region Public Interface    
        private string _primaryPID;
        public string PrimaryPID
        {
            get { return _primaryPID; }
            set
            {
                _primaryPID = value;
                OnPropertyChanged("PrimaryPID");
            }
        }
        private string _memberPID;
        public string MemberPID
        {
            get { return _memberPID; }
            set
            {
                _memberPID = value;
                OnPropertyChanged("MemberPID");
            }
        }
        private string _relationship;
        public string Relationship
        {
            get { return _relationship; }
            set
            {
                _relationship = value;
                OnPropertyChanged("Relationship");
            }
        }
        private string _currentFlag;
        public string CurrentFlag
        {
            get { return _currentFlag; }
            set
            {
                _currentFlag = value;
                OnPropertyChanged("CurrentFlag");
            }
        }
        private string _updateDate;
        public string UpdateDate
        {
            get { return _updateDate; }
            set
            {
                _updateDate = value;
                OnPropertyChanged("UpdateDate");
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
