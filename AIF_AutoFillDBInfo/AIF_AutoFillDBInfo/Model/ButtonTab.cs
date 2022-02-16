using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class ButtonTab : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public ButtonTab()
        {
            _buttonName = "";
            _buttonUC = AccompanyUCNameEnum.None;
        }
        #endregion Constructor

        #region Public Interface    
        private string _buttonName;
        public string ButtonName
        {
            get { return _buttonName; }
            set
            {
                _buttonName = value;
                OnPropertyChanged("ButtonName");
            }
        }
        private AccompanyUCNameEnum _buttonUC;
        public AccompanyUCNameEnum ButtonUC
        {
            get { return _buttonUC; }
            set
            {
                _buttonUC = value;
                OnPropertyChanged("ButtonUC");
            }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        #endregion Public Interface
    }
}
