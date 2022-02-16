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
    public class LoginViewModel : ViewModelBase
    {

        #region Fields

        readonly Dispatcher _dispatcher;
        
        #endregion

        #region Constructor

        public LoginViewModel(AppHelper appHelper) 
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

        private ICommand _signinCommand;
        public ICommand SigninCommand
        {
            get
            {
                if (_signinCommand == null)
                {
                    _signinCommand = new CommandBase(o => this.Signin(o), null);
                }
                return _signinCommand;
            }
        }

        private void Signin(object o)
        {
            ViewModelBase.UserAccount.username = UserName; //"jadehuang";// 
            ViewModelBase.UserAccount.Password = Password; //"AI0801";// 
            if (_appHelper.DBservice.isLogin(ViewModelBase.UserAccount))
            {
                _appHelper.NavTo(AppHelper.ViewID.MyCases);
            }
            //_appHelper.NavTo(AppHelper.ViewID.MyCases);
        }
        #region interface
        private string _userName;
        public string UserName
        {

            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        #endregion interface
        #region Clean Up

        protected override void OnClose()
        {
            //_appHelper.startupfirstWin = false;
            base.OnClose();
        }

        #endregion Clean Up
    }
}
