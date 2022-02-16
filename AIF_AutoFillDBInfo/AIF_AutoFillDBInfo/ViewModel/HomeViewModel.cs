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

namespace AIFAutoFillDB.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {

        #region Fields

        readonly Dispatcher _dispatcher;
        
        #endregion

        #region Constructor

        public HomeViewModel(AppHelper appHelper) 
            : base(appHelper)
        {
            //_isHomeVMFirstConstructed = true;

            //OnLoadVM(null);

            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public override void OnLoadVM(object o)
        {
            base.OnLoadVM(o);
            if (_appHelper != null)
            {
                _appHelper.CurrentVM = this;
            }

        }

        #endregion Constructor

        private ICommand _navToSettingCommand;
        public ICommand NavToSettingCommand
        {
            get
            {
                if (_navToSettingCommand == null)
                {
                    _navToSettingCommand = new CommandBase(o => this.NavToSettingPage(o), null);
                }
                return _navToSettingCommand;
            }
        }

        private void NavToSettingPage(object o)
        {
            //_appHelper.NavTo(AppHelper.ViewID.Settings);
        }

        private ICommand _navToBalanceCommand;
        public ICommand NavToBalanceCommand
        {
            get
            {
                if (_navToBalanceCommand == null)
                {
                    _navToBalanceCommand = new CommandBase(o => this.NavToBalancePage(o), null);
                }
                return _navToBalanceCommand;
            }
        }

        private void NavToBalancePage(object o)
        {
            //_appHelper.NavTo(AppHelper.ViewID.Balance);
        }

        #region Clean Up

        protected override void OnClose()
        {
            //_appHelper.startupfirstWin = false;
            base.OnClose();
        }

        #endregion Clean Up
    }
}
