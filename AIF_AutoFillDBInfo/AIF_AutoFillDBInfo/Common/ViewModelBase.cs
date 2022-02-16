using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Interop;
using AIFAutoFillDB.Model;

namespace AIFAutoFillDB.Common
{
    public enum UCNameEnum
    {
        None,
        Login,
        MyCases,
        MyPersons,
        MyAccounts,
        MyCustomers,
        PersonalInfo,
        CalculateTDSR,
        Incomes,
        Assets,
        Liabilities,
        KYC,
        Loan,
        Investment
    }
    public enum AccompanyUCNameEnum
    {
        None,
        ID,
        Family,
        Employment,
        Cheque,
        Address,
        Channel,
        Ownfund,
        Loan,
        Transfer,
        Beneficiary,
        NewAccount,
        Deposit,
        KYC
    }

    public enum GenderEnum
    {
        None,
        Male,
        Female,
    }

    public enum ChannelTypeEnum
    {
        AIF_User,
        Email,
        Wechat,
        Telegram,
        Twitter,
        Facebook
    }

    public enum CapitalSourceEnum
    {
        None,
        Loan,
        Ownfund,
        Transfer,
    }
    public enum TransferMethodEnum
    {
        None,
        Whole_Amount,
        All_in_Cash,
        Partial,
        All_Muture_Funds,
    }
    public abstract class ViewModelBase : NotifyBase
    {
        protected AppHelper _appHelper;

        private ICommand _BackwardCommand;

        private ICommand _ForwardCommand;

        private ICommand _windowClosedCommand;

        protected Window _view;

        
        private int _mainWinHeight;
        private int _mainWinWidth;
        private string _resizable;


        public ViewModelBase(AppHelper appHelper)
        {
            do
            {
                _appHelper = appHelper;               

            } while (false);
        }

        #region COMMON POP UP WINDOWS

        protected ICommand _closePMWindowCommand;

        protected Window _parentView;

        #endregion COMMON POP UP WINDOWS

        protected static System.Windows.Window _popupWindow = null;

        public static readonly DependencyProperty PopupWindowProperty = DependencyProperty.RegisterAttached("PopupWindow",
                                                                     typeof(System.Windows.Window), typeof(ViewModelBase), 
                                                                     new FrameworkPropertyMetadata(OnPopupWindowChanged));

        public static void SetPopupWindow(DependencyObject element, System.Windows.Window value)
        {
            element.SetValue(PopupWindowProperty, value);
        }

        public static System.Windows.Window GetPopupWindow(DependencyObject element)
        {
            return (System.Windows.Window)element.GetValue(PopupWindowProperty);
        }

        public static void OnPopupWindowChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _popupWindow = obj as System.Windows.Window;
        }


        public static UCNameEnum UCName { get; set; }
        public static LoginAccount UserAccount { get; set; }
        //public static AssetTypeList _assetsList = new List<LookUpInfo>();
        //_appHelper.DBservice.Select("Lookup_AssetType", "",out _assetsList);
        public string Resizable
        {
            get
            {
                if (UCName == UCNameEnum.None)
                {
                    _resizable = "NoResize";
                }
                else
                {
                    _resizable = "CanResize";
                }
                return _resizable;
            }
            set
            {
                _resizable = value;
                OnPropertyChanged("UCName");
            }
        }

        
        public ICommand BackwardCommand
        {
            get
            {
                if (_BackwardCommand == null)
                {
                    _BackwardCommand = new CommandBase((o) => this.Backward(o), this.CanBackward);
                }
                return _BackwardCommand;
            }
        }

        private void Backward(object o)
        {
            if (this._appHelper != null)
            {
                _appHelper.NavBackward();
            }
        }

        public Window CurrentView
        {
            get { return _view; }
            set { _view = value; }
        }
       
        private bool _OKButtonIsDefault;
        public bool OKButtonIsDefault
        {
            get
            {
                return _OKButtonIsDefault;
            }
            set
            {
                _OKButtonIsDefault = value;
                OnPropertyChanged("OKButtonIsDefault");
            }
        }
         private bool CanBackward(object o)
        {
            if (this._appHelper != null)
            {
                return _appHelper.CanGoBackward;
            }
            return true;
        }


        public ICommand ForwardCommand
        {
            get
            {
                if (_ForwardCommand == null)
                {
                    _ForwardCommand = new CommandBase((o) => this.Forward(o), this.CanForward);
                }
                return _ForwardCommand;
            }
        }

        private void Forward(object o)
        {
            if (this._appHelper != null)
            {
                _appHelper.NavForward();
            }
        }

        private bool CanForward(object o)
        {
            if (this._appHelper != null)
            {
                return _appHelper.CanGoForward;
            }
            return true;
        }

        
        public ICommand WindowClosingCommand
        {
            get
            {
                if (_windowClosedCommand == null)
                {
                    _windowClosedCommand = new CommandBase((o) => this.WindowClosed(o), null);
                }
                return _windowClosedCommand;
            }
        }
        private ICommand _windowCancelClosingCommand;
        public ICommand WindowCancelClosingCommand
        {
            get
            {
                if (_windowCancelClosingCommand == null)
                {
                    _windowCancelClosingCommand = new CommandBase((o) => this.WindowCancelClosing(o), null);
                }
                return _windowCancelClosingCommand;
            }
        }
        private void WindowCancelClosing(object o)
        {}
        
        protected virtual void OnClose()
        {
        }
        
        public virtual void OnLoadVM(object o)
        {
            
            
        }

        private void WindowClosed(object o)
        {
            OnClose();

            if (_appHelper.CloseWindowByCode)
            {
                // normal navigation by calling Close()
                _appHelper.CloseWindowByCode = false;
            }
            else
            {
                // Closed some other way (i.e. by clicking X button)
                Application.Current.Shutdown();
            }
        }

        #region General Command

        private ICommand _navToHomeCommand;
        public ICommand NavToHomeCommand
        {
            get
            {
                if (_navToHomeCommand == null)
                {
                    _navToHomeCommand = new CommandBase(o => this.NavToHome(o), null);
                }
                return _navToHomeCommand;
            }
        }

        private void NavToHome(object o)
        {
            //_appHelper.NavTo(AppHelper.ViewID.Home);
        }

        private ICommand _closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                if (_closeWindowCommand == null)
                {
                    _closeWindowCommand = new CommandBase(o => this.CloseMainWindow(), null);
                }
                return _closeWindowCommand;
            }
        }
        private void CloseMainWindow()
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Close();
            }
        }

        private ICommand _minimizeWindowCommand;
        public ICommand MinimizeWindowCommand
        {
            get
            {
                if (_minimizeWindowCommand == null)
                {
                    _minimizeWindowCommand = new CommandBase(o => this.MinimizeMainWindow(), null);
                }
                return _minimizeWindowCommand;
            }
        }
        private void MinimizeMainWindow()
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            }
        }
        private ICommand _maximizeWindowCommand;
        public ICommand MaximizeWindowCommand
        {
            get
            {
                if (_maximizeWindowCommand == null)
                {
                    _maximizeWindowCommand = new CommandBase(o => this.MaximizeMainWindow(), null);
                }
                return _maximizeWindowCommand;
            }
        }
        private void MaximizeMainWindow()
        {
            if (Application.Current.MainWindow != null)
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                    //WinMaxButtonVisibility = Visibility.Collapsed;
                }
                else
                {
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
                    //WinMaxButtonVisibility = Visibility.Visible;
                }
                if (CurrentView != null)
                {
                    string tempvm = CurrentView.DataContext.ToString();
                    ViewModelBase tvm = (ViewModelBase)CurrentView.DataContext;
                    //CurrentView.DataContext = _appHelper.VMservice.AboutVM;
                    //CurrentView.DataContext = tvm;
                }
            }
        }

        private ICommand _navigateToPersonalInfoCommand;
        public ICommand NavigateToPersonalInfoCommand
        {
            get
            {
                if (_navigateToPersonalInfoCommand == null)
                {
                    _navigateToPersonalInfoCommand = new CommandBase(o => this.NavigateToPersonalInfo(), null);
                }
                return _navigateToPersonalInfoCommand;
            }
        }
        private void NavigateToPersonalInfo()
        {
            _appHelper.NavTo(AppHelper.ViewID.PersonalInfo);
        }

        private ICommand _navigateToCalculateTDSRCommand;
        public ICommand NavigateToCalculateTDSRCommand
        {
            get
            {
                if (_navigateToCalculateTDSRCommand == null)
                {
                    _navigateToCalculateTDSRCommand = new CommandBase(o => this.NavigateToCalculateTDSR(), null);
                }
                return _navigateToCalculateTDSRCommand;
            }
        }
        private void NavigateToCalculateTDSR()
        {
            _appHelper.NavTo(AppHelper.ViewID.CalculateTDSR);
        }

        private ICommand _navigateToKYCCommand;
        public ICommand NavigateToKYCCommand
        {
            get
            {
                if (_navigateToKYCCommand == null)
                {
                    _navigateToKYCCommand = new CommandBase(o => this.NavigateToKYC(), null);
                }
                return _navigateToKYCCommand;
            }
        }
        private void NavigateToKYC()
        {
            _appHelper.NavTo(AppHelper.ViewID.KYC);
        }
        private ICommand _navigateToIncomesCommand;
        public ICommand NavigateToIncomesCommand
        {
            get
            {
                if (_navigateToIncomesCommand == null)
                {
                    _navigateToIncomesCommand = new CommandBase(o => this.NavigateToIncomes(), null);
                }
                return _navigateToIncomesCommand;
            }
        }
        private void NavigateToIncomes()
        {
            _appHelper.NavTo(AppHelper.ViewID.Incomes);
        }

        private ICommand _navigateToAssetsCommand;
        public ICommand NavigateToAssetsCommand
        {
            get
            {
                if (_navigateToAssetsCommand == null)
                {
                    _navigateToAssetsCommand = new CommandBase(o => this.NavigateToAssets(), null);
                }
                return _navigateToAssetsCommand;
            }
        }
        private void NavigateToAssets()
        {
            _appHelper.NavTo(AppHelper.ViewID.Assets);
        }

        private ICommand _navigateToLiabilitiesCommand;
        public ICommand NavigateToLiabilitiesCommand
        {
            get
            {
                if (_navigateToLiabilitiesCommand == null)
                {
                    _navigateToLiabilitiesCommand = new CommandBase(o => this.NavigateToLiabilities(), null);
                }
                return _navigateToLiabilitiesCommand;
            }
        }
        private void NavigateToLiabilities()
        {
            _appHelper.NavTo(AppHelper.ViewID.Liabilities);
        }
        private ICommand _navigateToLoanCommand;
        public ICommand NavigateToLoanCommand
        {
            get
            {
                if (_navigateToLoanCommand == null)
                {
                    _navigateToLoanCommand = new CommandBase(o => this.NavigateToLoan(), null);
                }
                return _navigateToLoanCommand;
            }
        }
        private void NavigateToLoan()
        {
            _appHelper.NavTo(AppHelper.ViewID.Loan);
        }
        private ICommand _navigateToInvestmentCommand;
        public ICommand NavigateToInvestmentCommand
        {
            get
            {
                if (_navigateToInvestmentCommand == null)
                {
                    _navigateToInvestmentCommand = new CommandBase(o => this.NavigateToInvestment(), null);
                }
                return _navigateToInvestmentCommand;
            }
        }
        private void NavigateToInvestment()
        {
            _appHelper.NavTo(AppHelper.ViewID.Investment);
        }
        #endregion General Command

        #region name textbox

        private static System.Windows.Controls.TextBox _nameTextBox = null;

        public static readonly DependencyProperty NameTextBoxProperty = DependencyProperty.RegisterAttached("NameTextBox",
                                                                     typeof(System.Windows.Controls.TextBox), typeof(ViewModelBase),
                                                                     new FrameworkPropertyMetadata(OnNameTextBoxChanged));

        public static void SetNameTextBox(DependencyObject element, System.Windows.Controls.TextBox value)
        {
            element.SetValue(NameTextBoxProperty, value);
        }

        public static System.Windows.Controls.TextBox GetNameTextBox(DependencyObject element)
        {
            return (System.Windows.Controls.TextBox)element.GetValue(NameTextBoxProperty);
        }

        public static void OnNameTextBoxChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _nameTextBox = obj as System.Windows.Controls.TextBox;
        }
        #endregion Name textbox

        #region Control is focused
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }


        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }


        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached(
             "IsFocused", typeof(bool), typeof(ViewModelBase),
             new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));


        private static void OnIsFocusedPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var uie = (UIElement)d;
            if ((bool)e.NewValue)
            {
                uie.Focus(); // Don't care about false values.
            }
        }
       
        private bool _isSearchBoxFocused;
        public bool IsSearchBoxFocused
        {
            get { return _isSearchBoxFocused; }
            set { _isSearchBoxFocused = value; OnPropertyChanged("IsSearchBoxFocused"); }
        }
        private bool _isContactSearchBoxFocused;
        public bool IsContactSearchBoxFocused
        {
            get { return _isContactSearchBoxFocused; }
            set { _isContactSearchBoxFocused = value; OnPropertyChanged("IsContactSearchBoxFocused"); }
        }
        private bool _isAddContactBoxFocused;
        public bool IsAddContactBoxFocused
        {
            get { return _isAddContactBoxFocused; }
            set { _isAddContactBoxFocused = value; OnPropertyChanged("IsAddContactBoxFocused"); }
        }
        private bool _loginAccountNameBoxFocused;
        public bool LoginAccountNameBoxFocused
        {
            get { return _loginAccountNameBoxFocused; }
            set { _loginAccountNameBoxFocused = value; OnPropertyChanged("LoginAccountNameBoxFocused"); }
        }
        private bool _setupAccountNameFocused;
        public bool SetupAccountNameFocused
        {
            get { return _setupAccountNameFocused; }
            set { _setupAccountNameFocused = value; OnPropertyChanged("SetupAccountNameFocused"); }
        }
        private bool _setupAccountReTypePasswordBoxFocused;
        public bool SetupAccountReTypePasswordBoxFocused
        {
            get { return _setupAccountReTypePasswordBoxFocused; }
            set { _setupAccountReTypePasswordBoxFocused = value; OnPropertyChanged("SetupAccountReTypePasswordBoxFocused"); }
        }

        #endregion control is focused
    }
}
