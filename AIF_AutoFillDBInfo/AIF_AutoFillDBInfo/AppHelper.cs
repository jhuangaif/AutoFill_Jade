using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Timers;

using System.Diagnostics;
using Microsoft.Win32;


using AIFAutoFillDB.Common;
using AIFAutoFillDB.Service;
using AIFAutoFillDB.View;
using AIFAutoFillDB.Service;

namespace AIFAutoFillDB
{
    public class AppHelper
    {
        #region VIEW IDs

        public enum ViewID
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
        };

        #endregion VIEW IDs

        #region APPHELPER FIELDS

        private Stack<ViewID> _BackwardList;
        private Stack<ViewID> _ForwardList;

        private String _ProductName;
        private string _assemblyName;

        private ViewModelBase _currentViewModel;

        private Window _view;
        private ViewModelBase _vm;
        private System.Windows.Threading.Dispatcher _dispatcher;

        private VMService _VMService;
        private DataBaseService _DBService;
        private AutoFillService _AutoService;

        private Window _theCurrentRunningWindow;
        private Window _theMotherView;
        
        #endregion APPHELPER FIELDS


        #region Constructor

        public AppHelper()
        {
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            if (assemblyName != null)
            {
                _assemblyName = assemblyName;
            }
            _ProductName = "AIF_AutoFill";
        }


        public void CreateApplicationMainWindow()
        {
            _view = new AIFCIMainWindow();

            Application.Current.MainWindow = _view;    // Make it official to the application, this is our main window.
            _theMotherView = _view;                    // The Mother View never changes, it's always the first and last window of the application
            _theCurrentRunningWindow = _theMotherView; // And to start off this is the current running window.

            _view.ShowInTaskbar = true;
            _view.ShowActivated = true;
            //_view.WindowState = WindowState.Minimized;
            _view.Show();
            _dispatcher = _view.Dispatcher;
        }
        public void Init()
        {
            _VMService = VMService.Instance;
            _VMService.Init(this);
            _DBService = DataBaseService.Instance;
            _DBService.Init(this);
            _AutoService = AutoFillService.Instance;
            _AutoService.Init(this);
        }

        public void PostInit()
        {

            _BackwardList = new Stack<ViewID>();
            _ForwardList = new Stack<ViewID>();
            ViewModelBase.UCName = UCNameEnum.None;
            ViewModelBase.UserAccount = new Model.LoginAccount();
        }

        #endregion Constructor
        #region public interface

        public ViewModelBase CurrentVM
        {
            set { _currentViewModel = value; }
            get { return _currentViewModel; }
        }
        public bool CanGoBackward
        {
            get { return _BackwardList.Count != 0; }
        }

        public bool CanGoForward
        {
            get { return _ForwardList.Count != 0; }
        }

        public bool CloseWindowByCode
        {
            get;
            set;
        }

        public System.Windows.Threading.Dispatcher Dispatcher
        {
            get { return _dispatcher; }
        }
        public VMService VMservice{ get { return _VMService; } }
        public DataBaseService DBservice { get { return _DBService; } }
        public AutoFillService AutoFillservice { get { return _AutoService; } }

        #endregion public interface

        #region Page navigation

        public void NavBackward()
        {
            if (_BackwardList.Count == 0)
            {
                return;
            }
            ViewID tempvid = (ViewID)(ViewModelBase.UCName);
            _ForwardList.Push((ViewID)(ViewModelBase.UCName));
            ViewModelBase.UCName = UCNameEnum.None;
            ViewID vm = _BackwardList.Pop();
            NavTo(vm);
        }

        public void NavForward()
        {            
            if (_ForwardList.Count == 0)
            {
                return;
            }
            _BackwardList.Push((ViewID)(ViewModelBase.UCName));
            ViewModelBase.UCName = UCNameEnum.None;
            ViewID vm = _ForwardList.Pop();
            NavTo(vm);
        }
        public void NavTo(ViewID ToViewID, object o = null, bool notInHistory = false)
        {
            TcDebug.Start();
            switch (ToViewID)
            {
                case ViewID.Login:
                    TcDebug.WriteLine("ViewUC_Login");
                    ViewUC_Login(o);
                    break;
                case ViewID.MyCases:
                    TcDebug.WriteLine("ViewUC_MyCase");
                    ViewUC_MyCases(o);
                    break;
                case ViewID.MyPersons:
                    TcDebug.WriteLine("ViewUC_MyPerson");
                    ViewUC_MyPersons(o);
                    break;
                case ViewID.PersonalInfo:
                    TcDebug.WriteLine("ViewUC_PersonalInfo");
                    ViewUC_PersonalInfo(o);
                    break;
                case ViewID.CalculateTDSR:
                    TcDebug.WriteLine("ViewUC_CalculateTDSR");
                    ViewUC_CalculateTDSR(o);
                    break;
                case ViewID.Incomes:
                    TcDebug.WriteLine("ViewUC_Incomes");
                    ViewUC_Incomes(o);
                    break;
                case ViewID.Assets:
                    TcDebug.WriteLine("ViewUC_Assets");
                    ViewUC_Assets(o);
                    break;
                case ViewID.Liabilities:
                    TcDebug.WriteLine("ViewUC_Liabilities");
                    ViewUC_Liabilities(o);
                    break;
                case ViewID.Loan:
                    TcDebug.WriteLine("ViewUC_Loan");
                    ViewUC_Loan(o);
                    break;
                case ViewID.Investment:
                    TcDebug.WriteLine("ViewUC_Investment");
                    ViewUC_Investment(o);
                    break;
                default:
                    break;
            }
            TcDebug.End();
        }

        public void BackWardClear()
        {
            _BackwardList.Clear();
        }
        private void NavToEvent()
        {
            
            if (ViewModelBase.UCName != UCNameEnum.None)
            {
                _BackwardList.Push((ViewID)ViewModelBase.UCName);
                _ForwardList.Clear();
            }
        }
        void UpdateUserControlView()
        {
            if (_view == null)
            {
                return;
            }

            _view.DataContext = _vm;

            // current running window will always be the mother view every time a new NavTo happens
            // this will be the basis for the running view of any pop-ups.
            _theCurrentRunningWindow = _theMotherView;

        }
        #endregion Page navigation

        #region ViewUC

        void ViewUC_Login(object o)
        {
            NavToEvent();
            _vm = _VMService.LoginVM;
            _vm.OnLoadVM(o);
            ViewModelBase.UCName = UCNameEnum.Login;
            UpdateUserControlView();
        }
        void ViewUC_MyCases(object o)
        {
            NavToEvent();
            _vm = _VMService.MyCasesVM;
            _vm.OnLoadVM(o);
            ViewModelBase.UCName = UCNameEnum.MyCases;
            UpdateUserControlView();
        }
        void ViewUC_MyPersons(object o)
        {
            NavToEvent();
            _vm = _VMService.MyPersonsVM;
            _vm.OnLoadVM(o);
            ViewModelBase.UCName = UCNameEnum.MyPersons;
            UpdateUserControlView();
        }
        void ViewUC_Assets(object o)
        {
            NavToEvent();
            _vm = _VMService.AssetsVM;
            _vm.OnLoadVM(o);
            ViewModelBase.UCName = UCNameEnum.Assets;
            UpdateUserControlView();
        }
        void ViewUC_Liabilities(object o)
        {
            NavToEvent();
            _vm = _VMService.LiabilitiesVM;
            _vm.OnLoadVM(o);
            ViewModelBase.UCName = UCNameEnum.Liabilities;
            UpdateUserControlView();
        }
        void ViewUC_Loan(object o)
        {
            NavToEvent();
            _vm = _VMService.LoanVM;
            _vm.OnLoadVM(o);
            ViewModelBase.UCName = UCNameEnum.Loan;
            UpdateUserControlView();
        }
        void ViewUC_Investment(object o)
        {
            NavToEvent();
            _vm = _VMService.InvestmentVM;
            _vm.OnLoadVM(o);
            ViewModelBase.UCName = UCNameEnum.Investment;
            UpdateUserControlView();
        }
        void ViewUC_CalculateTDSR(object o)
        {
            NavToEvent();
            _vm = _VMService.CalculateTDSRVM;
            _vm.OnLoadVM(o);
            ViewModelBase.UCName = UCNameEnum.CalculateTDSR;
            UpdateUserControlView();
        }
        void ViewUC_Incomes(object o)
        {
            NavToEvent();
            _vm = _VMService.IncomesVM;
            _vm.OnLoadVM(o);
            ViewModelBase.UCName = UCNameEnum.Incomes;
            UpdateUserControlView();
        }
        void ViewUC_PersonalInfo(object o)
        {
            NavToEvent();
            _vm = _VMService.PersonalInfoVM;
            _vm.OnLoadVM(o);
            ViewModelBase.UCName = UCNameEnum.PersonalInfo;
            UpdateUserControlView();
        }
        #endregion ViewUC
    }
}
