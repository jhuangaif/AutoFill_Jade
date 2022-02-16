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
using AIFAutoFillDB.Service;
using AIFAutoFillDB.Model;

namespace AIFAutoFillDB.ViewModel
{
    public class InvestmentViewModel : ViewModelBase
    {

        #region Fields

        readonly Dispatcher _dispatcher;
        private List<Investment> _InvtAppList;

        #endregion

        #region Constructor

        public InvestmentViewModel(AppHelper appHelper) 
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
            //DataBaseService.Instance.Init(_appHelper);

            _invSourceButtonList = new List<ButtonTab>();
            MakeInvSourceButtonsList(_invSourceButtonList);
            _invToButtonList = new List<ButtonTab>();
            MakeInvToButtonsList(_invToButtonList);
        }

        #endregion Constructor
        #region Command

        private ICommand _generateFormsCommand;
        public ICommand GenerateFormsCommand
        {
            get
            {
                if (_generateFormsCommand == null)
                {
                    _generateFormsCommand = new CommandBase(o => this.GenerateForms(), null);
                }
                return _generateFormsCommand;
            }
        }

        private void GenerateForms()
        {
            MakePersonInvestList(_InvtAppList);
            string originalformfolder = @"C:\Users\Cheng HUANG\Desktop\Jade\Application Form";
            string outfolder= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            foreach (Investment it in _InvtAppList)
            {
                switch (it.InvestTo)
                {
                    case "CL":
                        if (!string.IsNullOrEmpty(it.SourceLoan.LoanFrom) && it.SourceLoan.LoanFrom == "NB")
                        {
                            _appHelper.AutoFillservice.AutoFill_NB_Loan(it, originalformfolder, outfolder);
                        }
                        else if (!string.IsNullOrEmpty(it.SourceLoan.LoanFrom) && it.SourceLoan.LoanFrom == "IA")
                        {
                            //_appHelper.AutoFillservice.AutoFill_IA_Loan(it, originalformfolder, outfolder);
                        }
                        break;
                    case "iA":
                        break;
                    case "ML":
                        break;
                }
            }
        }
        private void MakeInvToButtonsList(List<ButtonTab> lbt)
        {
            //List<ButtonTab> bllist = new List<ButtonTab>();
            ButtonTab bl = new ButtonTab();
            bl.ButtonName = "New Account";
            bl.ButtonUC = AccompanyUCNameEnum.ID;
            bl.IsSelected = false;
            lbt.Add(bl);
            bl = new ButtonTab();
            bl.ButtonName = "Deposit";
            bl.ButtonUC = AccompanyUCNameEnum.Family;
            bl.IsSelected = false;
            lbt.Add(bl);
        }
        private void MakeInvSourceButtonsList(List<ButtonTab> lbt)
        {
            //List<ButtonTab> bllist = new List<ButtonTab>();
            ButtonTab bl = new ButtonTab();
            bl.ButtonName = "OwnFund";
            bl.ButtonUC = AccompanyUCNameEnum.ID;
            bl.IsSelected = false;
            lbt.Add(bl);
            bl = new ButtonTab();
            bl.ButtonName = "Loan";
            bl.ButtonUC = AccompanyUCNameEnum.Family;
            bl.IsSelected = false;
            lbt.Add(bl);
            bl = new ButtonTab();
            bl.ButtonName = "Transfer";
            bl.ButtonUC = AccompanyUCNameEnum.Employment;
            bl.IsSelected = false;
            lbt.Add(bl);
            bl = new ButtonTab();
            bl.ButtonName = "Beneficiary";
            bl.ButtonUC = AccompanyUCNameEnum.Cheque;
            bl.IsSelected = false;
            lbt.Add(bl);
            bl = new ButtonTab();
            bl.ButtonName = "KYC";
            bl.ButtonUC = AccompanyUCNameEnum.KYC;
            bl.IsSelected = false;
            lbt.Add(bl);
            bl = new ButtonTab();
        }
        private void MakePersonInvestList(List<Investment> InvtList)
        {
            Investment iv = new Investment();
            iv.InvestTo = "IA";
            iv.AccountType = "NON-REG";
            iv.OpenDate = "20220122";
            iv.CoAdvisor = true;
            iv.Advisor1.Agency = "FQ6";
            iv.Advisor1.AgencyName = "CitiStar Financial Group";
            iv.Advisor1.SalesRepCode = "1111";
            iv.Advisor1.DealerCode = "3507";
            iv.Advisor1.AdvisorCode_iA = "123456";
            iv.Advisor1.AdvisorSU_iA = "77";
            iv.Advisor1.AdvisorCode_CL = "c65432";
            iv.Advisor1.AdvisorCode_ML_Invest = "i22222";
            iv.Advisor1.AdvisorCode_ML_Loan = "l12121";
            iv.Advisor1.AdvisorName = "San Zhang";
            iv.Advisor1.AdvisorEmail = "123@abc.com";
            iv.Advisor1.AdvisorTelephone = "6478888888";
            iv.Advisor1.AdvisorTelephoneExt = "789";

            iv.Advisor2.Agency = "FQ6";
            iv.Advisor2.AgencyName = "CitiStar Financial Group";
            iv.Advisor2.SalesRepCode = "6666";
            iv.Advisor2.DealerCode = "3507";
            iv.Advisor2.AdvisorCode_iA = "987654";
            iv.Advisor2.AdvisorSU_iA = "66";
            iv.Advisor2.AdvisorCode_CL = "c98765";
            iv.Advisor2.AdvisorCode_ML_Invest = "i88888";
            iv.Advisor2.AdvisorCode_ML_Loan = "l89898";
            iv.Advisor2.AdvisorName = "Si Li";
            iv.Advisor2.AdvisorEmail = "789@abc.com";
            iv.Advisor2.AdvisorTelephone = "6476666666";
            iv.Advisor2.AdvisorTelephoneExt = "987";

            //start of Xieli's data
            iv.Applicant.PersonIDNo = "EON000020220204101710";
            iv.Applicant.FirstName = "YIMING";
            iv.Applicant.LastName = "XU";
            iv.Applicant.EnglishName = "";
            iv.Applicant.Gender = "Male";
            iv.Applicant.IsFemale = false;
            iv.Applicant.IsMale = true;
            iv.Applicant.DateofBirth = Convert.ToDateTime("1982-07-26");
            iv.Applicant.DobYear = "1982";
            iv.Applicant.DobMonth = "07";
            iv.Applicant.DobDay = "26";
            iv.Applicant.CountryofBirth = "China";
            iv.Applicant.ProvinceofBirth = "Hunan";
            iv.Applicant.Citizenship = "Canada";
            iv.Applicant.TaxStatus = "Canada";
            iv.Applicant.LiveCAsince = Convert.ToDateTime("2012-09-01");
            iv.Applicant.MaritalStatus = "Single";
            iv.Applicant.Cellphone = "2893545698";
            iv.Applicant.Homephone = "";
            iv.Applicant.Workphone = "";
            iv.Applicant.Email = "YIMING_XU@HOTMAIL.COM";
            iv.Applicant.Bankrupcty = false;
            iv.Applicant.DischargeDate = Convert.ToDateTime( "2022-02-28");

            ID tempID = new ID();
            tempID.PID = "EON000020220204101710";
            tempID.IdType = "SIN";
            tempID.IdNumber = "452315987";
            tempID.IssueDate = "";
            tempID.ExpiryDate = "";
            tempID.IssueCountry = "Canada";
            tempID.IssueProvince = "ON";
            tempID.CurrentFlag = true;
            tempID.VerifyDate = "20210128";
            tempID.Notes = "";
            iv.Applicant.PersonIDs.Add(tempID);

            tempID = new ID();
            tempID.PID = "EON000020220204101710";
            tempID.IdType = "Provincial Driver's License";
            tempID.IdNumber = "X4859-23658-20726";
            tempID.IssueDate = "20180508";
            tempID.ExpiryDate = "20220726";
            tempID.IssueCountry = "Canada";
            tempID.IssueProvince = "ON";
            tempID.CurrentFlag = true;
            tempID.VerifyDate = "20210128";
            tempID.Notes = "";
            iv.Applicant.PersonIDs.Add(tempID);

            Address tempadd = new Address();
            tempadd.PID = "EON000020220204101710";
            tempadd.AptNo = "";
            tempadd.StreetNo = "142";
            tempadd.StreetName = "MAVIS RD";
            tempadd.City = "MISSISSAUGA";
            tempadd.Province = "ON";
            tempadd.Country = "Canada";
            tempadd.Postcode = "L3N 2H4";
            tempadd.LivingStatus = "Own";
            tempadd.StartDate = "20120901";
            tempadd.EndDate = "";
            tempadd.CurrentFlag = true;
            tempadd.VerifyDate = "2021-01-28";
            tempadd.Notes = "";
            iv.Applicant.PersonAddress.Add(tempadd);

            Employment tempemp =new Employment();
            tempemp.EmplStatus = "Employed";
            tempemp.Employer = "BELL CANADA";
            tempemp.Industry = "";
            tempemp.Occupation = "";
            tempemp.Unit = "";
            tempemp.StNo = "";
            tempemp.StName = "";
            tempemp.City = "Mississauga";
            tempemp.Prov = "ON";
            tempemp.Country = "Canada";
            tempemp.PostCode = "";
            tempemp.WorkPhone = "";
            tempemp.Income = "50000";
            tempemp.StartDate = "";
            tempemp.EndDate = "";
            tempemp.CurrentFlag = true;
            tempemp.VerifyDate = "2021-01-28";
            tempemp.Notes = "";
            tempemp.WorkSinceYear = "";
            tempemp.WorkSinceMonth = "";
            iv.Applicant.PersonEmployment.Add(tempemp);

            Income tempincome =new Income();

            tempincome.IncomeType = "Salary";
            tempincome.IncomeAmount = "50000";
            tempincome.InstituionName = "Bell Canada";
            tempincome.verifyDate = "2021-01-28";
            tempincome.Notes = "";
            iv.Applicant.PersonIncome.Add(tempincome);

            Asset tempasset =new Asset();
            tempasset.AssetsType = "Principal Residence";
            tempasset.MarketValue = "2000000";
            tempasset.Institution = "";
            tempasset.verifyDate = "2021-01-20";
            tempasset.Notes = "";
            iv.Applicant.PersonAsset.Add(tempasset);

            tempasset =new Asset();
            tempasset.AssetsType = "Other Real Estate";
            tempasset.MarketValue = "1000000";
            tempasset.Institution = "";
            tempasset.verifyDate = "2021-01-20";
            tempasset.Notes = "";
            iv.Applicant.PersonAsset.Add(tempasset);

            tempasset =new Asset();
            tempasset.AssetsType = "Other Real Estate";
            tempasset.MarketValue = "3000000";
            tempasset.Institution = "";
            tempasset.verifyDate = "2021-01-20";
            tempasset.Notes = "";
            iv.Applicant.PersonAsset.Add(tempasset);

            Liability templiab =new Liability();
            templiab.LiType = "Mortgage";
            templiab.LiBalance = "1500000";
            templiab.LiMonthlyPayt = "7000";
            templiab.Institution = "";
            templiab.VerifyDate = "2021-01-20";
            templiab.Notes = "";
            iv.Applicant.PersonLiability.Add(templiab);

            templiab = new Liability();
            templiab.LiType = "Property Tax";
            templiab.LiBalance = "";
            templiab.LiMonthlyPayt = "1000";
            templiab.Institution = "City";
            templiab.VerifyDate = "2021-01-20";
            templiab.Notes = "";
            iv.Applicant.PersonLiability.Add(templiab);

            templiab = new Liability();
            templiab.LiType = "Mortgage";
            templiab.LiBalance = "500000";
            templiab.LiMonthlyPayt = "2300";
            templiab.Institution = "";
            templiab.VerifyDate = "2021-01-20";
            templiab.Notes = "";
            iv.Applicant.PersonLiability.Add(templiab);

            templiab =new  Liability();
            templiab.LiType = "Property Tax";
            templiab.LiBalance = "";
            templiab.LiMonthlyPayt = "200";
            templiab.Institution = "City";
            templiab.VerifyDate = "2021-01-20";
            templiab.Notes = "";
            iv.Applicant.PersonLiability.Add(templiab);


            templiab = new Liability();
            templiab.LiType = "Condo fee";
            templiab.LiBalance = "";
            templiab.LiMonthlyPayt = "450";
            templiab.Institution = "Condo Corporation";
            templiab.VerifyDate = "2021-01-20";
            templiab.Notes = "";
            iv.Applicant.PersonLiability.Add(templiab);


            templiab =new  Liability();
            templiab.LiType = "Mortgage";
            templiab.LiBalance = "2000000";
            templiab.LiMonthlyPayt = "10000";
            templiab.Institution = "";
            templiab.VerifyDate = "2021-01-20";
            templiab.Notes = "";
            iv.Applicant.PersonLiability.Add(templiab);

            templiab = new Liability();
            templiab.LiType = "Property Tax";
            templiab.LiBalance = "";
            templiab.LiMonthlyPayt = "1500";
            templiab.Institution = "City";
            templiab.VerifyDate = "2021-01-20";
            templiab.Notes = "";
            iv.Applicant.PersonLiability.Add(templiab);

            KYC tempkyc = new KYC();
            tempkyc.KYC_QS_No = "";
            tempkyc.ANSRNo = "";
            tempkyc.ANSRScore = "";
            tempkyc.VerifyDate = "2021-01-20";
            iv.Applicant.PersonKYC.Add(tempkyc);

            iv.Applicant.Usertype = "";


            InvtList.Add(iv);

        }
        #endregion Command

        #region public interface

        private List<ButtonTab> _invToButtonList;
        public List<ButtonTab> InvToButtonList
        {

            get { return _invToButtonList; }
            set
            {
                _invToButtonList = value;
                OnPropertyChanged("InvToButtonList");
            }
        }
        private List<ButtonTab> _invSourceButtonList;
        public List<ButtonTab> InvSourceButtonList
        {

            get { return _invSourceButtonList; }
            set
            {
                _invSourceButtonList = value;
                OnPropertyChanged("InvSourceButtonList");
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
        private List<ButtonTab> _invTobuttonList;
        public List<ButtonTab> InvTobuttonList
        {

            get { return _invTobuttonList; }
            set
            {
                _invTobuttonList = value;
                OnPropertyChanged("InvTobuttonList");
            }
        }
        #endregion public interface
        #region Clean Up

        protected override void OnClose()
        {
            //_appHelper.startupfirstWin = false;
            base.OnClose();
        }

        #endregion Clean Up
    }
}