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
    public class MyPersonViewModel : ViewModelBase
    {

        #region Fields

        readonly Dispatcher _dispatcher;
        
        #endregion

        #region Constructor

        public MyPersonViewModel(AppHelper appHelper) 
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
            object _pList = new List<Person>();
            _appHelper.DBservice.Select("Person", "", out _pList);

            if (_pList == null || ((List<Person>)_pList).Count == 0)
            {
                _personsList = new ObservableCollection<Person>();
            }
            else
            {
                _personsList = new ObservableCollection<Person>((List<Person>)_pList);
            }
        }

        #endregion Constructor
        #region command

        private ICommand _newPersonCommand;
        public ICommand NewPersonCommand
        {
            get
            {
                if (_newPersonCommand == null)
                {
                    _newPersonCommand = new CommandBase(o => this.NewPerson(o), null);
                }
                return _newPersonCommand;
            }
        }

        private void NewPerson(object o)
        {
            _appHelper.NavTo(AppHelper.ViewID.PersonalInfo);
        }
        private ICommand _showPersonDetailCommand;
        public ICommand ShowPersonDetailCommand
        {
            get
            {
                if (_showPersonDetailCommand == null)
                {
                    _showPersonDetailCommand = new CommandBase(o => this.ShowPersonDetail(o), null);
                }
                return _showPersonDetailCommand;
            }
        }

        private void ShowPersonDetail(object o)
        {
            Person ps=new Person();
            if (o is Person)
            {
                ps = o as Person;
            }
            else if (((ListBoxItem)o).Content is Person)
            {
                ps = (((ListBoxItem)o).Content) as Person;
            }
            if (ps==null)
            {
                return;
            }
            _appHelper.NavTo(AppHelper.ViewID.PersonalInfo, ps);


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
                if (_personsList.Count >= 1 && !string.IsNullOrEmpty(_personsList[0].PersonIDNo))
                {
                    ListCollectionView personListView = (ListCollectionView)CollectionViewSource.GetDefaultView(_personsList);

                    personListView.Filter = delegate (object item)
                    {
                        Person uc = (Person)item;
                        return uc.FirstName.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.LastName.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.EnglishName.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.Email.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.Cellphone.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.Homephone.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0
                            || uc.Workphone.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0;
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
        private ObservableCollection<Person> _personsList;
        public ObservableCollection<Person> PersonsList
        {
            get { return _personsList; }
            set
            {
                _personsList = value;
                OnPropertyChanged("PersonsList");
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