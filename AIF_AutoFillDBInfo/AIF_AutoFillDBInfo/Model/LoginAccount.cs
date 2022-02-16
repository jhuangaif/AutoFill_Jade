using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class LoginAccount : NotifyBase
    {

        #region Fields

        private string _username;
        private string _email;
        private string _rolename;
        private string _privilege;
        private string _password;
        private string _PID;

        #endregion  Fields

        #region Constructor

        public LoginAccount()
        {
            _PID = "";
            _username = "";//Login Account
            _rolename = "";//DB name
            _firstname = "";
            _lastname = "";
            _gender = "";
            _email = "";
            _phone = "";
            _usertype = "";
            _password = "";
            _privilege = "";      
        }

        #endregion Constructor

        #region Public Interface

        public string PID
        {
            get { return _PID; }
            set
            {
                _PID = value;
                OnPropertyChanged("PID");
            }
        }
        public string username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged("username");
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        public string Rolename
        {
            get { return _rolename; }
            set
            {
                _rolename = value;
                OnPropertyChanged("Rolename");
            }
        }
        private string _firstname;
        public string Firstname
        {
            get { return _firstname; }
            set
            {
                _firstname = value;
                OnPropertyChanged("Firstname");
            }
        }
        private string _lastname;
        public string Lastname
        {
            get { return _lastname; }
            set
            {
                _lastname = value;
                OnPropertyChanged("Lastname");
            }
        }
        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                OnPropertyChanged("Gender");
            }
        }
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged("Phone");
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
        public string Privilege
        {
            get { return _privilege; }
            set
            {
                _privilege = value;
                OnPropertyChanged("Privilege");
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        #endregion Public Interface
    }
}
