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
using AIFAutoFillDB.Model;
using System.Windows.Controls;
using System.Windows.Data;

namespace AIFAutoFillDB.ViewModel
{
    public class MycasesViewModel : ViewModelBase
    {

        #region Fields

        readonly Dispatcher _dispatcher;
        
        #endregion

        #region Constructor

        public MycasesViewModel(AppHelper appHelper) 
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
            //_loansList = new ObservableCollection<Loan>();
            //object _loanList = new List<Loan>();
            //_appHelper.DBservice.Select("Loan", "", out _loanList);
            object _investList = new List<Investment>();
            _appHelper.DBservice.Select("Investment", "", out _investList);
            if (_investList == null)
            {
                _investmentList = new ObservableCollection<Investment>();
            }
            else
            {
                _investmentList = new ObservableCollection<Investment>((List<Investment>)_investList);
            }
            Investment it = new Investment();
            it.AccountType = "TFSA";
            //it.TransferMethod = "Whole Amount";// TransferMethodEnum.Whole_Amount;//
            it.CoApplicationFlag = true;
            _investmentList.Add(it);
            it = new Investment();
            it.AccountType = "RRSP";
            //it.TransferMethod = "All in Cash"; //TransferMethodEnum.All_in_Cash;// 
            it.CoApplicationFlag = false;
            _investmentList.Add(it);
            it = new Investment();
            it.AccountType = "RESP";
            //it.TransferMethod = "Partial";// TransferMethodEnum.Partial;// 
            _investmentList.Add(it);
            it.CoApplicationFlag = true;
            it = new Investment();
            it.AccountType = "Non-Reg";
            //it.TransferMethod = "Whole Amount";// TransferMethodEnum.Whole_Amount;// 
            it.CoApplicationFlag = false;
            _investmentList.Add(it);
            //}
            //_loansList = new ObservableCollection<Loan>((List<Loan>)_loanList);
        }

        #endregion Constructor
        #region command

        private ICommand _newCaseCommand;
        public ICommand NewCaseCommand
        {
            get
            {
                if (_newCaseCommand == null)
                {
                    _newCaseCommand = new CommandBase(o => this.NewCase(o), null);
                }
                return _newCaseCommand;
            }
        }

        private void NewCase(object o)
        {
            _appHelper.NavTo(AppHelper.ViewID.MyPersons);
        }
        private ICommand _showCaseDetailCommand;
        public ICommand ShowCaseDetailCommand
        {
            get
            {
                if (_showCaseDetailCommand == null)
                {
                    _showCaseDetailCommand = new CommandBase(o => this.ShowCaseDetail(o), null);
                }
                return _showCaseDetailCommand;
            }
        }

        private void ShowCaseDetail(object o)
        {
            _appHelper.NavTo(AppHelper.ViewID.PersonalInfo,o);
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new CommandBase(o => this.Search(o), null);
                }
                return _searchCommand;
            }
        }

        private void Search(object o)
        {
            do
            {
                if (o == null)
                {
                    break;
                }

                //System.Windows.Controls.TextBox txt = o as System.Windows.Controls.TextBox;

                SearchTextBox = o as System.Windows.Controls.TextBox;

                string str = SearchTextBox.Text.TrimEnd();
                //if (_membersList.Count == 1 && string.IsNullOrEmpty(_membersList[0].EmailAddress)
                // && _foldersList.Count == 1 && string.IsNullOrEmpty(_foldersList[0].FullPath))
                //{
                //    return;
                //}
                if (_investmentList.Count >= 1 )//&& !string.IsNullOrEmpty(_investmentList[0].InvestmentNo))
                {
                    ListCollectionView InvestmentListView = (ListCollectionView)CollectionViewSource.GetDefaultView(_investmentList);

                    InvestmentListView.Filter = delegate (object item)
                    {
                        Investment uc = (Investment)item;
                        return uc.Applicant.FirstName.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.Applicant.FirstName.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.Applicant.Email.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.Applicant.Cellphone.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.Applicant.Homephone.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.Applicant.Workphone.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            //|| uc.Applicant.PersonAddress[0].City.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            //|| uc.Applicant.PersonAddress[0].Postcode.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.InvestTo.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.CapitalSourceType.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.SourceLoan.LoanFrom.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.AccountType.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            } while (false);
        }
        private ICommand _clearSearchCommand;
        public ICommand ClearSearchCommand
        {
            get
            {
                if (_clearSearchCommand == null)
                {
                    _clearSearchCommand = new CommandBase(o => this.ClearSearch(o), null);
                }
                return _clearSearchCommand;
            }
        }

        private void ClearSearch(object o)
        {
            if (SearchTextBox != null)
            {
                SearchTextBox.Text = "";
            }
        }
        #endregion command
        #region Interface
        private ObservableCollection<Loan> _loansList;
        public ObservableCollection<Loan> LoansList
        {
            get { return _loansList; }
            set
            {
                _loansList = value;
                OnPropertyChanged("LoansList");
            }
        }

        private ObservableCollection<Investment> _investmentList;
        public ObservableCollection<Investment> InvestmentList
        {
            get { return _investmentList; }
            set
            {
                _investmentList = value;
                OnPropertyChanged("InvestmentList");
            }
        }
        private TextBox _searchTextBox;
        public TextBox SearchTextBox
        {
            get
            {
                return _searchTextBox;
            }
            set
            {
                _searchTextBox = value;
                OnPropertyChanged("SearchTextBox");
            }
        }
        #endregion Interface



        #region Clean Up

        protected override void OnClose()
        {
            //_appHelper.startupfirstWin = false;
            base.OnClose();
        }

        #endregion Clean Up
    }
}