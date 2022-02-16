using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Asset : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Asset()
        {
            _assetsType = "";
            _marketValue = "";
            _institution = "";
            _verifyDate = "";
            _notes = "";
        }
        #endregion Constructor

        #region Public Interface    
        private string _assetsType;
        public string AssetsType
        {
            get { return _assetsType; }
            set
            {
                _assetsType = value;
                OnPropertyChanged("AssetsType");
            }
        }
        private string _marketValue;
        public string MarketValue
        {
            get { return _marketValue; }
            set
            {
                _marketValue = value;
                OnPropertyChanged("MarketValue");
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
