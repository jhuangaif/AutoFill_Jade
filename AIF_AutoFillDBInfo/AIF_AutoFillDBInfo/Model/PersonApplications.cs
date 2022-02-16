using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class PersonApplications : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor

        public PersonApplications()
        {
            _personInfo = new Person();
            _personIDs = new List<ID>();
            _personAddress = new List<Address>();
            _personFamilies = new List<Families>();
            _personEmployment = new List<Employment>();
            _personAssets = new List<Asset>();
            _personLiabilitys = new List<Liability>();
            _personLoans = new List<Loan>();
            _personInvestments = new List<Investment>();

        }

        #endregion Constructor

        #region Public Interface    

        private Person _personInfo;
        public Person PersonInfo
        {
            get { return _personInfo; }
            set
            {
                _personInfo = value;
                OnPropertyChanged("PersonInfo");
            }
        }

        private List<ID> _personIDs;
        public List<ID> PersonIDs
        {
            get { return _personIDs; }
            set
            {
                _personIDs = value;
                OnPropertyChanged("PersonIDs");
            }
        }
        private List<Address> _personAddress;
        public List<Address> PersonAddress
        {
            get { return _personAddress; }
            set
            {
                _personAddress = value;
                OnPropertyChanged("PersonAddress");
            }
        }
        private List<Families> _personFamilies;
        public List<Families> PersonFamilies
        {
            get { return _personFamilies; }
            set
            {
                _personFamilies = value;
                OnPropertyChanged("PersonFamilies");
            }
        }
        private List<Employment> _personEmployment;
        public List<Employment> PersonEmployment
        {
            get { return _personEmployment; }
            set
            {
                _personEmployment = value;
                OnPropertyChanged("PersonEmployment");
            }
        }
        private List<Asset> _personAssets;
        public List<Asset> PersonAssets
        {
            get { return _personAssets; }
            set
            {
                _personAssets = value;
                OnPropertyChanged("PersonAssets");
            }
        }
        private List<Liability> _personLiabilitys;
        public List<Liability> PersonLiabilitys
        {
            get { return _personLiabilitys; }
            set
            {
                _personLiabilitys = value;
                OnPropertyChanged("PersonLiabilitys");
            }
        }
        private List<Loan> _personLoans;
        public List<Loan> PersonLoans
        {
            get { return _personLoans; }
            set
            {
                _personLoans = value;
                OnPropertyChanged("PersonLoans");
            }
        }
        private List<Investment> _personInvestments;
        public List<Investment> PersonInvestments
        {
            get { return _personInvestments; }
            set
            {
                _personInvestments = value;
                OnPropertyChanged("PersonInvestments");
            }
        }
        #endregion Public Interface
    }
}
