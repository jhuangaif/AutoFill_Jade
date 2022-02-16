using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class LookUpInfo : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public LookUpInfo()
        {
            _lookUpInfo_id = "";
            _lookUpInfo_str = "";
        }
        #endregion Constructor

        #region Public Interface    
        private string _lookUpInfo_id;
        public string LookUpInfo_id
        {
            get { return _lookUpInfo_id; }
            set
            {
                _lookUpInfo_id = value;
                OnPropertyChanged("LookUpInfo_id");
            }
        }
        private string _lookUpInfo_str;
        public string LookUpInfo_str
        {
            get { return _lookUpInfo_str; }
            set
            {
                _lookUpInfo_str = value;
                OnPropertyChanged("LookUpInfo_str");
            }
        }
        #endregion Public Interface
    }
}
