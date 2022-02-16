using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Channel : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Channel()
        {
            _personID = "";
            _channelType = "";
            _channelID = "";
            _channelPrivilege = "";
            _currentFlag = false;
            _verifyDate = "";
        }
        #endregion Constructor

        #region Public Interface    
        private string _personID;
        public string PersonID
        {
            get { return _personID; }
            set
            {
                _personID = value;
                OnPropertyChanged("PersonID");
            }
        }
        private string _channelType;
        public string ChannelType
        {
            get { return _channelType; }
            set
            {
                _channelType = value;
                OnPropertyChanged("ChannelType");
            }
        }
        private string _channelID;
        public string ChannelID  //Employer, Property Addresss,......
        {
            get { return _channelID; }
            set
            {
                _channelID = value;
                OnPropertyChanged("ChannelID");
            }
        }
        private string _channelPrivilege;
        public string ChannelPrivilege  //Employer, Property Addresss,......
        {
            get { return _channelPrivilege; }
            set
            {
                _channelPrivilege = value;
                OnPropertyChanged("ChannelPrivilege");
            }
        }
        private bool _currentFlag;
        public bool CurrentFlag  //Employer, Property Addresss,......
        {
            get { return _currentFlag; }
            set
            {
                _currentFlag = value;
                OnPropertyChanged("CurrentFlag");
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
        //private string _notes;
        //public string Notes
        //{
        //    get { return _notes; }
        //    set
        //    {
        //        _notes = value;
        //        OnPropertyChanged("Notes");
        //    }
        //}
        #endregion Public Interface
    }
}
