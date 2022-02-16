using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;

using AIFAutoFillDB.Common;
using AIFAutoFillDB.Service;
using System.Windows;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using AIFAutoFillDB.Service;

namespace AIFAutoFillDB.ViewModel
{
    public class AssetsViewModel : ViewModelBase
    {

        #region Fields

        readonly Dispatcher _dispatcher;
        private DataBaseService _dbs;

        #endregion

        #region Constructor

        public AssetsViewModel(AppHelper appHelper) 
            : base(appHelper)
        {
            //_isHomeVMFirstConstructed = true;

            //OnLoadVM(null);

            _dispatcher = Dispatcher.CurrentDispatcher;
            //_dbs = new DataBaseService();
        }

        public override void OnLoadVM(object o)
        {
            base.OnLoadVM(o);
            if (_appHelper != null)
            {
                _appHelper.CurrentVM = this;
            }
            _OkexBalance = "";
        }

        #endregion Constructor
        #region public interface
            
        private string _OkexBalance;
        public string OkexBalance
        {
            get
            {
                return _OkexBalance;
            }
            set
            {
                _OkexBalance = value;
                OnPropertyChanged("OkexBalance");
            }
        }
        #endregion public interface
        #region public command

        

        #endregion public command

        #region Clean Up

        protected override void OnClose()
        {
            //_appHelper.startupfirstWin = false;
            base.OnClose();
        }

        #endregion Clean Up
    }
}