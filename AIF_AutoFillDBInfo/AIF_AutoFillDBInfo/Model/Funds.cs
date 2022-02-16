using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Funds : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public Funds()
        {
            _fundcode = "";
            _fundname = "";
            _fundpercentage = "";
            _fundnetAmount = "";
            _fundUnitValue = "";
            _numberOfUnit = ""; 
        }

        #endregion Constructor

        #region Public Interface    

        private string _fundcode;
        public string Fundcode
        {
            get { return _fundcode; }
            set
            {
                _fundcode = value;
                OnPropertyChanged("Fundcode");
            }
        }
        private string _fundname;
        public string Fundname
        {
            get { return _fundname; }
            set
            {
                _fundname = value;
                OnPropertyChanged("Fundname");
            }
        }
        private string _fundpercentage;
        public string Fundpercentage
        {
            get { return _fundpercentage; }
            set
            {
                _fundpercentage = value;
                OnPropertyChanged("Fundpercentage");
            }
        }
        private string _fundnetAmount;
        public string FundNetAmount
        {
            get { return _fundnetAmount; }
            set
            {
                _fundnetAmount = value;
                OnPropertyChanged("FundNetAmount");
            }
        }
        private string _fundUnitValue;
        public string FundUnitValue
        {
            get { return _fundUnitValue; }
            set
            {
                _fundUnitValue = value;
                OnPropertyChanged("FundUnitValue");
            }
        }
        private string _numberOfUnit;
        public string NamberOfUnit
        {
            get { return _numberOfUnit; }
            set
            {
                _numberOfUnit = value;
                OnPropertyChanged("NamberOfUnit");
            }
        }

        #endregion Public Interface
    }
}
