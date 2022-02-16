using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIFAutoFillDB.Common;

namespace AIFAutoFillDB.Model
{
    public class Advisor : NotifyBase
    {

        #region Fields

        #endregion  Fields

        #region Constructor
        public Advisor()
        {/*alan
            _agencyCode = "FQ6";
            _agencyName = "CitiStar Financial Group";
            _salesRepCode = "";// "0131"; 
            _dealerCode = "";// "3571";
            _advisorCode = "265276";
            _advisorSU = "001";
            _agentcommissionPercent = "100";
            _agentName = "Jun Lu";
            _agentEmail = "AIFAutoFillDB.ca@gmail.com";
            _agentTelephone = "6475882558";
            _agentTelephoneExt = "";
            */

            /*Junchao Li
            _agencyCode = "FQ6";
            _agencyName = "CitiStar Financial Group";
            _salesRepCode = "";// "0131"; 
            _dealerCode = "";// "3571";
            _advisorCode = "588687";
            _advisorSU = "003";
            _agentcommissionPercent = "100";
            _agentName = "JUNCHAO LI";
            _agentEmail = "AIFAutoFillDB.ca@gmail.com";
            _agentTelephone = "4168245503";
            _agentTelephoneExt = "";
           */
            _advisorNo = "";
            _agency = "";
            _agencyName = "";
            _salesRepCode = "";
            _dealerCode = "";
            _advisorCode_iA = "";
            _advisorCode_CL = "";
            _advisorSU_iA = "";
            _advisorcommissionPercent = "";
            _advisorName = "";
            _dvisorEmail = "";
            _advisorgentTelephone = "";
            _advisorTelephoneExt = "";
            _advisorCode_CL = "";
            _MGA_CL = "";
            _B2B_Dealer = "";
            _advisorCode_B2B = "";
            _licenses = "";           
            _agencyCode = "";
            _advisorCode_NB = "";
            _advisorCode_ML_Loan = "";
            _advisorCode_ML_Invest = "";
            
        }
        #endregion Constructor

        #region Public Interface    
        private string _advisorNo;
        public string AdvisorNo
        {
            get { return _advisorNo; }
            set
            {
                _advisorNo = value;
                OnPropertyChanged("AdvisorNo");
            }
        }
        private string _advisorPID;
        public string AdvisorPID
        {
            get { return _advisorPID; }
            set
            {
                _advisorPID = value;
                OnPropertyChanged("PersonID");
            }
        }
        private string _agency;
        public string Agency
        {
            get { return _agency; }
            set
            {
                _agency = value;
                OnPropertyChanged("Agency");
            }
        }
        private string _agencyName;//name of district or agency
        public string AgencyName
        {
            get { return _agencyName; }
            set
            {
                _agencyName = value;
                OnPropertyChanged("AgencyName");
            }
        }
        private string _salesRepCode;
        public string SalesRepCode
        {
            get { return _salesRepCode; }
            set
            {
                _salesRepCode = value;
                OnPropertyChanged("SalesRepCode");
            }
        }
        private string _dealerCode;
        public string DealerCode
        {
            get { return _dealerCode; }
            set
            {
                _dealerCode = value;
                OnPropertyChanged("DealerCode");
            }
        }
        private string _advisorCode_iA;
        public string AdvisorCode_iA
        {
            get { return _advisorCode_iA; }
            set
            {
                _advisorCode_iA = value;
                OnPropertyChanged("AdvisorCode_iA");
            }
        }        
        private string _advisorSU_iA;
        public string AdvisorSU_iA
        {
            get { return _advisorSU_iA; }
            set
            {
                _advisorSU_iA = value;
                OnPropertyChanged("AdvisorSU_iA");
            }
        }
        private string _advisorCode_CL;
        public string AdvisorCode_CL
        {
            get { return _advisorCode_CL; }
            set
            {
                _advisorCode_CL = value;
                OnPropertyChanged("AdvisorCode_CL");
            }
        }
        private string _advisorcommissionPercent;
        public string AdvisorcommissionPercent
        {
            get { return _advisorcommissionPercent; }
            set
            {
                _advisorcommissionPercent = value;
                OnPropertyChanged("AdvisorcommissionPercent");
            }
        }
        private string _advisorName;
        public string AdvisorName
        {
            get { return _advisorName; }
            set
            {
                _advisorName = value;
                OnPropertyChanged("AdvisorName");
            }
        }
        private string _dvisorEmail;
        public string AdvisorEmail
        {
            get { return _dvisorEmail; }
            set
            {
                _dvisorEmail = value;
                OnPropertyChanged("AdvisorEmail");
            }
        }
        private string _advisorgentTelephone;
        public string AdvisorTelephone
        {
            get { return _advisorgentTelephone; }
            set
            {
                _advisorgentTelephone = value;
                OnPropertyChanged("AdvisorTelephone");
            }
        }
        private string _advisorTelephoneExt;
        public string AdvisorTelephoneExt
        {
            get { return _advisorTelephoneExt; }
            set
            {
                _advisorTelephoneExt = value;
                OnPropertyChanged("AdvisorTelephoneExt");
            }
        }
        private string _MGA_CL;
        public string MGA_CL
        {
            get { return _MGA_CL; }
            set
            {
                _MGA_CL = value;
                OnPropertyChanged("MGA_CL");
            }
        }
        private string _B2B_Dealer;
        public string B2B_Dealer
        {
            get { return _B2B_Dealer; }
            set
            {
                _B2B_Dealer = value;
                OnPropertyChanged("B2B_Dealer");
            }
        }
        private string _advisorCode_B2B;
        public string AdvisorCode_B2B
        {
            get { return _advisorCode_B2B; }
            set
            {
                _advisorCode_B2B = value;
                OnPropertyChanged("AdvisorCode_B2B");
            }
        }
        private string _licenses; //ON, BC, QC
        public string Licenses
        {
            get { return _licenses; }
            set
            {
                _licenses = value;
                OnPropertyChanged("Licenses");
            }
        }
        private string _agencyCode;
        public string AgencyCode
        {
            get { return _agencyCode; }
            set
            {
                _agencyCode = value;
                OnPropertyChanged("AgencyCode");
            }
        }
        private string _advisorCode_NB;
        public string AdvisorCode_NB
        {
            get { return _advisorCode_NB; }
            set
            {
                _advisorCode_NB = value;
                OnPropertyChanged("AdvisorCode_NB");
            }
        }
        private string _advisorCode_ML_Loan;
        public string AdvisorCode_ML_Loan
        {
            get { return _advisorCode_ML_Loan; }
            set
            {
                _advisorCode_ML_Loan = value;
                OnPropertyChanged("AdvisorCode_ML_Loan ");
            }
        }
        private string _advisorCode_ML_Invest;
        public string AdvisorCode_ML_Invest
        {
            get { return _advisorCode_ML_Invest; }
            set
            {
                _advisorCode_ML_Invest = value;
                OnPropertyChanged("AdvisorCode_ML_Invest");
            }
        }
        
        private string _otherLicenses; 
        public string OtherLicenses
        {
            get { return _otherLicenses; }
            set
            {
                _otherLicenses = value;
                OnPropertyChanged("OtherLicenses");
            }
        }
        private string _otherLicensesInc; //ON, BC, QC
        public string OtherLicensesInc
        {
            get { return _otherLicensesInc; }
            set
            {
                _otherLicensesInc = value;
                OnPropertyChanged("OtherLicensesInc");
            }
        }
        #endregion Public Interface




    }
}
