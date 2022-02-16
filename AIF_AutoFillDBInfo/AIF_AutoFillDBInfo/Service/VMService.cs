using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.ViewModel;

namespace AIFAutoFillDB.Service
{
    public class VMService
    {
        #region FIELDS

        private static VMService _instance;

        private HomeViewModel _homeViewModel;
        private LoginViewModel _loginViewModel;
        private MycasesViewModel _mycasesViewModel;
        private MyPersonViewModel _mypersonsViewModel;
        private PersonalInfoViewModel _personalInfoViewModel;
        private CalculateTDSRViewModel _calculateTDSRViewModel;
        private IncomesViewModel _incomesViewModel;
        private InvestmentViewModel _investmentViewModel;
        private AssetsViewModel _assetsViewModel;
        private LiabilitiesViewModel _liabilitiesViewModel;
        private LoanViewModel _loanViewModel;
       
        private AppHelper _appHelper;
        #endregion FIELDS

        #region PUBLIC INTERFACE

        public VMService()
        {
            _homeViewModel = null;
            _loginViewModel = null;
            _mycasesViewModel = null;
            _mypersonsViewModel = null;
            _personalInfoViewModel = null;
            _calculateTDSRViewModel = null;
            _incomesViewModel = null;
            _assetsViewModel = null;
            _liabilitiesViewModel = null;
            _loanViewModel = null;
            _investmentViewModel = null;
        }

        private void createAllViewModels()
        {
            _homeViewModel = new HomeViewModel(_appHelper);
            _loginViewModel = new LoginViewModel(_appHelper);
            _mycasesViewModel = new MycasesViewModel(_appHelper);
            _mypersonsViewModel = new MyPersonViewModel(_appHelper);
            _personalInfoViewModel = new PersonalInfoViewModel(_appHelper);
            _calculateTDSRViewModel = new CalculateTDSRViewModel(_appHelper);
            _incomesViewModel = new IncomesViewModel(_appHelper);
            _assetsViewModel = new AssetsViewModel(_appHelper);
            _liabilitiesViewModel = new LiabilitiesViewModel(_appHelper);
            _loanViewModel = new LoanViewModel(_appHelper);
            _investmentViewModel = new InvestmentViewModel(_appHelper);
        }


        public static VMService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VMService();
                }
                return _instance;
            }
        }

        public void Init(AppHelper appHelper)
        {
            _appHelper = appHelper;

            createAllViewModels();
        }

        public HomeViewModel HomeVM
        {
            get { return _homeViewModel; }
        }
        public LoginViewModel LoginVM
        {
            get { return _loginViewModel; }
        }
        public MycasesViewModel MyCasesVM
        {
            get { return _mycasesViewModel; }
        }
        public MyPersonViewModel MyPersonsVM
        {
            get { return _mypersonsViewModel; }
        }
        public PersonalInfoViewModel PersonalInfoVM
        {
            get { return _personalInfoViewModel; }
        }
        public CalculateTDSRViewModel CalculateTDSRVM
        {
            get { return _calculateTDSRViewModel; }
        }
        public IncomesViewModel IncomesVM
        {
            get { return _incomesViewModel; }
        }
        public AssetsViewModel AssetsVM
        {
            get { return _assetsViewModel; }
        }
        public LiabilitiesViewModel LiabilitiesVM
        {
            get { return _liabilitiesViewModel; }
        }
        public LoanViewModel LoanVM
        {
            get { return _loanViewModel; }
        }
        public InvestmentViewModel InvestmentVM
        {
            get { return _investmentViewModel; }
        }

        #endregion PUBLIC INTERFACE
    }
}
