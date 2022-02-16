using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Person : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Person()
        {
            _personIDNo = "";
            _firstName = "";
            _lastName = "";
            _englishName = "";
            _gender = "";
            _isFemale = false;
            _isMale = false;
            _dateofBirth = new DateTime();
            _dobYear = "";
            _dobMonth = "";
            _dobDay = "";
            _countryofBirth = "";
            _provinceofBirth = "";
            _citizenship = "";
            _taxStatus = "";
            _liveCAsince = new DateTime();
            _maritalStatus = "";
            _cellphone = "";
            _homephone = "";
            _workphone = "";
            _email = "";
            _bankrupcty = false;
            _dischargeDate = new DateTime();
            _personIDs = new List<ID>();
            _personAddress = new List<Address>();
            _personEmployment = new List<Employment>();
            _personIncome = new List<Income>();
            _personAsset = new List<Asset>();
            _personLiability = new List<Liability>();
            _personKYC = new List<KYC>();
            _usertype = "";
        }

        #endregion Constructor

        #region Public Interface  
        private string _personIDNo;
        public string PersonIDNo
        {
            get { return _personIDNo; }
            set
            {
                _personIDNo = value;
                OnPropertyChanged("PersonIDNo");
            }
        }
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public string fullName
        {
            get
            {
                string fullName = "";
                if (LastName != null)
                {
                    fullName = FirstName.ToUpper();
                }
                if (FirstName != null)
                {
                    fullName = string.IsNullOrEmpty(fullName) ? FirstName.ToUpper() : (fullName + (new string(' ', 53)) + FirstName.ToUpper());
                }
                return fullName;
            }
        }

        private string _englishName;
        public string EnglishName
        {
            get { return _englishName; }
            set
            {
                _englishName = value;
                OnPropertyChanged("EnglishName");
            }
        }
        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                if (_gender=="FEMALE")
                {
                    _isFemale = true;
                    _isMale = false;
                }
                else
                {
                    _isFemale = false;
                    _isMale = true;
                }
                OnPropertyChanged("Gender");
            }
        }
        private bool _isFemale;
        public bool IsFemale
        {
            get { return _isFemale; }
            set
            {
                _isFemale = value;
                if(_isFemale)
                {
                    _gender = "FEMALE";
                }
                else
                {
                    _gender = "MALE";
                }
                OnPropertyChanged("IsFemale");
            }
        }
        private bool _isMale;
        public bool IsMale
        {
            get { return _isMale; }
            set
            {
                _isMale = value;
                if (_isMale)
                {
                    _gender = "MALE";
                }
                else
                {
                    _gender = "FEMALE";
                }
                OnPropertyChanged("IsMale");
            }
        }
        private DateTime _dateofBirth;
        public DateTime DateofBirth
        {
            get { return _dateofBirth; }
            set
            {
                _dateofBirth = value;
                OnPropertyChanged("DateofBirth");
            }
        }
        private string _dobYear;
        public string DobYear
        {
            get { return _dobYear; }
            set
            {
                _dobYear = value;
                OnPropertyChanged("DobYear");
            }
        }
        private string _dobMonth;
        public string DobMonth
        {
            get { return _dobMonth; }
            set
            {
                _dobMonth = value;
                OnPropertyChanged("DobMonth");
            }
        }
        private string _dobDay;
        public string DobDay
        {
            get { return _dobDay; }
            set
            {
                _dobDay = value;
                OnPropertyChanged("DobDay");
            }
        }
        private string _countryofBirth;
        public string CountryofBirth
        {
            get { return _countryofBirth; }
            set
            {
                _countryofBirth = value;
                OnPropertyChanged("CountryofBirth");
            }
        }
        private string _provinceofBirth;
        public string ProvinceofBirth
        {
            get { return _provinceofBirth; }
            set
            {
                _provinceofBirth = value;
                OnPropertyChanged("ProvinceofBirth");
            }
        }
        private string _citizenship;
        public string Citizenship
        {
            get { return _citizenship; }
            set
            {
                _citizenship = value;
                OnPropertyChanged("Citizenship");
            }
        }
        private string _taxStatus;
        public string TaxStatus
        {
            get { return _taxStatus; }
            set
            {
                _taxStatus = value;
                OnPropertyChanged("TaxStatus");
            }
        }
        private DateTime _liveCAsince;
        public DateTime LiveCAsince
        {
            get { return _liveCAsince; }
            set
            {
                _liveCAsince = value;
                OnPropertyChanged("LiveCAsince");
            }
        }
        private string _maritalStatus;
        public string MaritalStatus
        {
            get { return _maritalStatus; }
            set
            {
                _maritalStatus = value;
                OnPropertyChanged("MaritalStatus");
            }
        }
        private string _cellphone;
        public string Cellphone
        {
            get { return _cellphone; }
            set
            {
                _cellphone = value;
                OnPropertyChanged("Cellphone");
            }
        }
        private string _homephone;
        public string Homephone
        {
            get { return _homephone; }
            set
            {
                _homephone = value;
                OnPropertyChanged("Homephone");
            }
        }
        private string _workphone;
        public string Workphone
        {
            get { return _workphone; }
            set
            {
                _workphone = value;
                OnPropertyChanged("Workphone");
            }
        }
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        private bool _bankrupcty;
        public bool Bankrupcty
        {
            get { return _bankrupcty; }
            set
            {
                _bankrupcty = value;
                OnPropertyChanged("Bankrupcty");
            }
        }
        private DateTime _dischargeDate;
        public DateTime DischargeDate
        {
            get { return _dischargeDate; }
            set
            {
                _dischargeDate = value;
                OnPropertyChanged("DischargeDate");
            }
        }
        private string _usertype;
        public string Usertype
        {
            get { return _usertype; }
            set
            {
                _usertype = value;
                OnPropertyChanged("Usertype");
            }
        }
        private List<ID> _personIDs;
        public List<ID> PersonIDs //Sorted by ID type: SIN, Driver License, Passport, PR Card, Health Card,  ......
        {
            get { return _personIDs; }
            set
            {
                _personIDs = value;
                OnPropertyChanged("PersonIDs");
            }
        }
        private List<Address> _personAddress;
        public List<Address> PersonAddress //Sorted by Date, from recent to previous,  ......
        {
            get { return _personAddress; }
            set
            {
                _personAddress = value;
                OnPropertyChanged("PersonAddress");
            }
        }
        private List<Employment> _personEmployment;
        public List<Employment> PersonEmployment //Sorted by Date, from recent to previous,  ......
        {
            get { return _personEmployment; }
            set
            {
                _personEmployment = value;
                OnPropertyChanged("PersonEmployment");
            }
        }
        private List<Income> _personIncome;
        public List<Income> PersonIncome 
        {
            get { return _personIncome; }
            set
            {
                _personIncome = value;
                OnPropertyChanged("PersonIncome");
            }
        }
        private List<Asset> _personAsset;
        public List<Asset> PersonAsset 
        {
            get { return _personAsset; }
            set
            {
                _personAsset = value;
                OnPropertyChanged("PersonAsset");
            }
        }
        private List<Liability> _personLiability;
        public List<Liability> PersonLiability 
        {
            get { return _personLiability; }
            set
            {
                _personLiability = value;
                OnPropertyChanged("PersonLiability");
            }
        }
        private List<KYC> _personKYC;
        public List<KYC> PersonKYC
        {
            get { return _personKYC; }
            set
            {
                _personKYC = value;
                OnPropertyChanged("PersonKYC");
            }
        }
        #endregion Public Interface
    }
}
