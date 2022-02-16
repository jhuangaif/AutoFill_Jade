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

namespace AIFAutoFillDB.ViewModel
{
    public class PersonalInfoViewModel : ViewModelBase
    {

        #region Fields

        readonly Dispatcher _dispatcher;
        
        #endregion

        #region Constructor

        public PersonalInfoViewModel(AppHelper appHelper) 
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
            object _piList = new List<Person>();

            if (o is Person)
            {
                _applicantInfoList = new ObservableCollection<Person>();
                _applicantInfoList.Add((Person)o);
            }
            else
            {
                _applicantInfoList = new ObservableCollection<Person>();
                _applicantInfoList.Add(new Person());
            }
            _piList = new List<Person>();
            _appHelper.DBservice.Select("Person", "PersonID='E000202112261000'", out _piList);
            _appHelper.DBservice.Select("Person", "PersonID='jade20211230013828PM'", out _piList);
            _appHelper.DBservice.Select("Person", "PersonID='test2022012407440112'", out _piList);

            if (_piList == null || ((List<Person>)_piList).Count == 0)
            {
                _coapplicantInfoList = new ObservableCollection<Person>();
                _coapplicantInfoList.Add(new Person());
            }
            else
            {
                _coapplicantInfoList = new ObservableCollection<Person>((List<Person>)_piList);
            }
            _buttonList = new List<ButtonTab>();
            MakeButtonList(_buttonList);
            _accompanyUC = AccompanyUCNameEnum.None;
            if (_buttonList.Count >= 1)
            {
                _accompanyUC = _buttonList[0].ButtonUC;
                _buttonList[0].IsSelected = true;
            }

            _applicantbuttonList = new List<ButtonTab>();
            ButtonTab bl = new ButtonTab();
            bl.ButtonName = "Applicant";
            bl.ButtonUC = AccompanyUCNameEnum.None;
            bl.IsSelected = true;
            _applicantbuttonList.Add(bl);
            bl = new ButtonTab();
            bl.ButtonName = "CoApplicant";
            bl.ButtonUC = AccompanyUCNameEnum.None;
            bl.IsSelected = false;
            _applicantbuttonList.Add(bl);

            object _LookupList = new List<LookUpInfo>();
            _appHelper.DBservice.Select("Lookup_AssetType", "", out _LookupList);
            _assetType = new ObservableCollection<string>(((List<LookUpInfo>)_LookupList).Select(t => t.LookUpInfo_str).ToList());

            _LookupList = new List<LookUpInfo>();
            _appHelper.DBservice.Select("Lookup_LivingStatus", "", out _LookupList);
            LivingStatusType = new ObservableCollection<string>(((List<LookUpInfo>)_LookupList).Select(t => t.LookUpInfo_str).ToList());


            _selectedAssetType = "";

            _paddress = new Address();
            _pAddressList = new ObservableCollection<Address>();
            _paddress.LivingStatus = "Owner";
            _paddress.Country = "Canada";
            _paddress.Province = "Ontario";
            _paddress.City = "Richmond Hill";//350 Hwy 7 suite 310, Richmond Hill, ON L4B 3N2
            _paddress.Postcode = "L4B 3N2";
            _paddress.AptNo = "310";
            _paddress.StreetNo = "350";
            _paddress.StreetName = "Hwy 7";
            _paddress.StartDate = "20191226";
            _paddress.EndDate = "";
            _paddress.CurrentFlag = true;
            _pAddressList.Add(_paddress);

            VerifyDate = DateTime.Now;

            IsApplicantvisible = true;

        }

        #endregion Constructor
        #region command

        private ICommand _navigateToMyCasesCommand;
        public ICommand NavigateToMyCasesCommand
        {
            get
            {
                if (_navigateToMyCasesCommand == null)
                {
                    _navigateToMyCasesCommand = new CommandBase(o => this.NavigateToMyCases(o), null);
                }
                return _navigateToMyCasesCommand;
            }
        }

        private void NavigateToMyCases(object o)
        {
            _appHelper.NavTo(AppHelper.ViewID.MyCases);
        }
        private ICommand _navToAccompanyUCCommand;
        public ICommand NavToAccompanyUCCommand
        {
            get
            {
                if (_navToAccompanyUCCommand == null)
                {
                    _navToAccompanyUCCommand = new CommandBase(o => this.NavToAccompanyUC(o), null);
                }
                return _navToAccompanyUCCommand;
            }
        }

        private void NavToAccompanyUC(object o)
        {
            ButtonTab bt;
            if (o is ButtonTab)
            {
                bt = (ButtonTab)o;
            }
            else if (((ListBoxItem)o).Content is ButtonTab)
            {
                bt = (ButtonTab)((ListBoxItem)o).Content;
            }
            else
            {
                return;
            }
            bt.IsSelected = true;
            if (bt.ButtonName == "Applicant")
            {
                IsApplicantvisible = true;
            }
            else if (bt.ButtonName == "CoApplicant")
            {
                IsApplicantvisible = false;
            }
            else
            {
                AccompanyUC = bt.ButtonUC;
            }
        }
        //private ICommand _switchApplicantCommand;
        //public ICommand SwitchApplicantCommand
        //{
        //    get
        //    {
        //        if (_switchApplicantCommand == null)
        //        {
        //            _switchApplicantCommand = new CommandBase(o => this.SwitchApplicant(o), null);
        //        }
        //        return _switchApplicantCommand;
        //    }
        //}

        //private void SwitchApplicant(object o)
        //{
        //    ButtonTab bt;
        //    if (o is ButtonTab)
        //    {
        //        bt = (ButtonTab)o;
        //    }
        //    else if (((ListBoxItem)o).Content is ButtonTab)
        //    {
        //        bt = (ButtonTab)((ListBoxItem)o).Content;
        //    }
        //    else
        //    {
        //        return;
        //    }
        //    bt.IsSelected = true;
            
        //    //AccompanyUC =bt.ButtonUC;
        //}
        private ICommand _savePersonalInfoCommand;
        public ICommand SavePersonalInfoCommand
        {
            get
            {
                if (_savePersonalInfoCommand == null)
                {
                    _savePersonalInfoCommand = new CommandBase(o => this.SavePersonalInfo(), null);
                }
                return _savePersonalInfoCommand;
            }
        }

        private void SavePersonalInfo()
        {
            if (string.IsNullOrEmpty(ApplicantInfoList[0].PersonIDNo))
            {
                ApplicantInfoList[0].PersonIDNo = ViewModelBase.UserAccount.username.Substring(0,4) + DateTime.Now.ToString("yyyyMMddhhmmsstt").ToUpper();
            }
            _appHelper.DBservice.Insert(ApplicantInfoList[0]);
            MessageBox.Show("Person information was saved!");// ApplicantInfoList[0].Gender + ", " + ApplicantInfoList[0].TaxStatus);
        }
        private ICommand _deletePersonalInfoCommand;
        public ICommand DeletePersonalInfoCommand
        {
            get
            {
                if (_deletePersonalInfoCommand == null)
                {
                    _deletePersonalInfoCommand = new CommandBase(o => this.DeletePersonalInfo(), null);
                }
                return _deletePersonalInfoCommand;
            }
        }

        private void DeletePersonalInfo()
        {
            bool ret=false;
            MessageBoxResult mbr= MessageBox.Show("You're deleting Person Information, do you want to continou?","Deleting Person Information!!!",MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                if (string.IsNullOrEmpty(ApplicantInfoList[0].PersonIDNo))
                {
                    ret=_appHelper.DBservice.Delete("Person", "PersonID is Null or PersonID=''");
                }
                else
                {
                    ret=_appHelper.DBservice.Delete("Person", "PersonID='" + ApplicantInfoList[0].PersonIDNo + "'");
                }
            }
            if (ret)
            {
                _appHelper.NavTo(AppHelper.ViewID.MyPersons);
            }
        }
        private ICommand _addPersonAddressCommand;
        public ICommand AddPersonAddressCommand
        {
            get
            {
                if (_addPersonAddressCommand == null)
                {
                    _addPersonAddressCommand = new CommandBase(o => this.AddPersonAddress(), null);
                }
                return _addPersonAddressCommand;
            }
        }

        private void AddPersonAddress()
        {
            _pAddressList.Add(_paddress);
            _paddress = new Address();
        }
        private void MakeButtonList(List<ButtonTab> lbt)
        {
            //List<ButtonTab> bllist = new List<ButtonTab>();
            ButtonTab bl = new ButtonTab();
            bl.ButtonName = "ID";
            bl.ButtonUC = AccompanyUCNameEnum.ID;
            bl.IsSelected = false;
            lbt.Add(bl);
            bl = new ButtonTab();
            bl.ButtonName = "Family";
            bl.ButtonUC = AccompanyUCNameEnum.Family;
            bl.IsSelected = false;
            lbt.Add(bl);
            bl = new ButtonTab();
            bl.ButtonName = "Employment";
            bl.ButtonUC = AccompanyUCNameEnum.Employment;
            bl.IsSelected = false;
            lbt.Add(bl);
            bl = new ButtonTab();
            bl.ButtonName = "Cheque";
            bl.ButtonUC = AccompanyUCNameEnum.Cheque;
            bl.IsSelected = false;
            lbt.Add(bl);
            bl = new ButtonTab();
            bl.ButtonName = "Address";
            bl.ButtonUC = AccompanyUCNameEnum.Address;
            bl.IsSelected = false;
            lbt.Add(bl);
            bl = new ButtonTab();
            bl.ButtonName = "Channel";
            bl.ButtonUC = AccompanyUCNameEnum.Channel;
            bl.IsSelected = false;
            lbt.Add(bl);
        }
        #endregion command
        #region Interface

        private List<ButtonTab> _buttonList;
        public List<ButtonTab> ButtonList
        {

            get { return _buttonList; }
            set
            {
                _buttonList = value;
                OnPropertyChanged("ButtonList");
            }
        }
        private AccompanyUCNameEnum _accompanyUC;
        public AccompanyUCNameEnum AccompanyUC
        {

            get { return _accompanyUC; }
            set
            {
                _accompanyUC = value;
                OnPropertyChanged("AccompanyUC");
            }
        }
        private List<ButtonTab> _applicantbuttonList;
        public List<ButtonTab> ApplicantbuttonList
        {

            get { return _applicantbuttonList; }
            set
            {
                _applicantbuttonList = value;
                OnPropertyChanged("ApplicantbuttonList");
            }
        }
        private ObservableCollection<string> _assetType;
        public ObservableCollection<string> AssetType
        {

            get { return _assetType; }
            set
            {
                _assetType = value;
                OnPropertyChanged("AssetType");
            }
        }
        private ObservableCollection<Person> _applicantInfoList;
        public ObservableCollection<Person> ApplicantInfoList
        {

            get { return _applicantInfoList; }
            set
            {
                _applicantInfoList = value;
                OnPropertyChanged("ApplicantInfoList");
            }
        }
        private ObservableCollection<Person> _coapplicantInfoList;
        public ObservableCollection<Person> CoApplicantInfoList
        {

            get { return _coapplicantInfoList; }
            set
            {
                _coapplicantInfoList = value;
                OnPropertyChanged("CoApplicantInfoList");
            }
        }
        private string _selectedAssetType;
        public string SelectedAssetType
        {

            get { return _selectedAssetType; }
            set
            {
                _selectedAssetType = value;
                MessageBox.Show(value);
                OnPropertyChanged("SelectedAssetType");
            }
        }

        private bool _isapplicantvisible;
        public bool IsApplicantvisible
        {

            get { return _isapplicantvisible; }
            set
            {
                _isapplicantvisible = value;
                OnPropertyChanged("IsApplicantvisible");
            }
        }
        //private ObservableCollection<string> _livingStatusType;
        public ObservableCollection<string> LivingStatusType { get; set; }

        private Address _paddress;
        public Address Paddress
        {

            get { return _paddress; }
            set
            {
                _paddress = value;
                OnPropertyChanged("Paddress");
            }
        }
        private ObservableCollection<Address> _pAddressList;
        public ObservableCollection<Address> PAddressList
        {

            get { return _pAddressList; }
            set
            {
                _pAddressList = value;
                OnPropertyChanged("PAddressList");
            }
        }private DateTime _verifyDate;
        public DateTime VerifyDate
        {

            get { return _verifyDate; }
            set
            {
                _verifyDate = value;
                OnPropertyChanged("VerifyDate");
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