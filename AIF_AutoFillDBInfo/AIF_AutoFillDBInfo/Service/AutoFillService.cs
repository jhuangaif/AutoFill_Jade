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
using Newtonsoft.Json.Linq;
using AIFAutoFillDB.Model;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;
using iTextSharp.text.pdf;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace AIFAutoFillDB.Service
{
    public class AutoFillService
    {
        #region Fields
        private static AutoFillService _instance;
        private AppHelper _appHelper;
        readonly Dispatcher _dispatcher;
        private string ErrMessage = "";
        
        #endregion

        #region Constructor
        public AutoFillService()
        {

        }

        public static AutoFillService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AutoFillService();
                }
                return _instance;
            }
        }

        public void Init(AppHelper appHelper)
        {
            _appHelper = appHelper;            
            //Initialize();
        }
        

        #endregion Constructor
        #region command

        public void AutoFill_NB_Loan(Investment LoanApplication, string SourePDFfolder, string outputdirectory)
        {
            PdfReader pdfReader;
            PdfStamper pdfStamper;
            AcroFields pdfFormFields;
            if (LoanApplication.SourceLoan == null)
            {
                //ErrMessage = string.IsNullOrEmpty(ErrMessage) ? "No NATIONAL BANK Loan application!" : ErrMessage + " \nNo NATIONAL BANK Loan application!";
                MessageBox.Show("No NATIONAL BANK Loan application!");
                return;
            }
            //string iAappFileName = DefaultOutFolder + "\\" + applicantName + "\\" + applicantName + "_NB_Loan_" + DateTime.Now.ToString("yyMMdd_hhmmss") + ".pdf";
            string NBLoanAppFileName = Path.Combine(outputdirectory, 
                                                LoanApplication.Applicant.LastName+", "+ LoanApplication.Applicant.FirstName+
                                                (LoanApplication.CoApplicationFlag ? " & "+LoanApplication.Applicant.LastName + ", " + LoanApplication.Applicant.FirstName:"") + 
                                                "_NB_Loan_" + DateTime.Now.ToString("yyMMdd_hhmmss") + ".pdf");
            pdfReader = new PdfReader(SourePDFfolder + @"\NB\NB_Loan_Applicaiton.pdf");
            PdfReader.unethicalreading = true;
            pdfStamper = new PdfStamper(pdfReader, new FileStream(NBLoanAppFileName, FileMode.Create));
            pdfFormFields = pdfStamper.AcroFields;

            // Agent Info
            //AgentInfo agentI = ViewModelBase.Advisor; 
            pdfFormFields.SetField("_.2-1", LoanApplication.Advisor1.AdvisorName.ToUpper());// ViewModelBase.Advisor.AgentName);
            pdfFormFields.SetField("_.2-2", LoanApplication.Advisor1.AdvisorCode_iA);//ViewModelBase.Advisor.AdvisorCode_iA);
            pdfFormFields.SetField("_.2-3", LoanApplication.Advisor1.MGA_CL);//"SCIO");
            pdfFormFields.SetField("_.2-4", LoanApplication.Advisor1.AdvisorTelephone);//ViewModelBase.Advisor.AgentTelephone);
            
            //APPLICANT INFOMATION
            //Excel._Worksheet xlWorksheet = readWorkbook.Sheets["Personal Info"];
            //Excel.Range xlRange = xlWorksheet.UsedRange;

            pdfFormFields.SetField("_.2-8", /*Last Name*/ LoanApplication.Applicant.LastName.ToUpper()); //(xlRange.Range["B7"] != null && xlRange.Range["B7"].Value2 != null) ? xlRange.Range["B7"].Value2.ToString().ToUpper() : "");
            pdfFormFields.SetField("_.2-12", /*First name*/ LoanApplication.Applicant.FirstName.ToUpper()); //(xlRange.Range["E7"] != null && xlRange.Range["E7"].Value2 != null) ? xlRange.Range["E7"].Value2.ToString().ToUpper() : "");

            string abc = "";
            if (LoanApplication.Applicant.LastName != null)
            {
                abc = LoanApplication.Applicant.FirstName.ToUpper();
            }
            if (LoanApplication.Applicant.FirstName != null)
            {
                abc = string.IsNullOrEmpty(abc) ? LoanApplication.Applicant.FirstName.ToUpper() : (abc + (new string(' ', 53))+LoanApplication.Applicant.FirstName.ToUpper());
            }
            pdfFormFields.SetField("_.3-1", /*Page2 Applicant Name*/ abc); //Page 2
            pdfFormFields.SetField("_.4-1", /*Page2 Applicant Name*/ abc); //Page 3

            //pdfFormFields.SetField("_.2-9", /*Date of Birth - Year*/(xlRange.Range["D8"] != null && xlRange.Range["D8"].Value2 != null) ? xlRange.Range["D8"].Value2.ToString() : "");
            //pdfFormFields.SetField("_.2-10", /*Date of Birth - Month*/(xlRange.Range["F8"] != null && xlRange.Range["F8"].Value2 != null) ? convertMonth(xlRange.Range["F8"].Value2.ToString()) : "00");
            //pdfFormFields.SetField("_.2-11", /*Date of Birth - Day*/(xlRange.Range["H8"] != null && xlRange.Range["H8"].Value2 != null) ? ("00" + xlRange.Range["H8"].Value2.ToString()).Substring(("00" + xlRange.Range["H8"].Value2.ToString()).Length - 2) : "");
            pdfFormFields.SetField("_.2-9", /*Date of Birth-Year*/ LoanApplication.Applicant.DobYear);
            pdfFormFields.SetField("_.2-10", /*Date of Birth-Month*/ LoanApplication.Applicant.DobMonth);
            pdfFormFields.SetField("_.2-11", /*Date of Birth-Day*/ LoanApplication.Applicant.DobDay);

            bool idfilled = false;
            foreach (ID id in LoanApplication.Applicant.PersonIDs)
            {
                switch (id.IdType.ToUpper())
                {
                    case "SIN":
                        pdfFormFields.SetField("_.2-14", /*SIN*/id.IdNumber);
                        pdfFormFields.SetField("_.2-31", /*SIN*/id.IdNumber);
                        pdfFormFields.SetField("_.2-26", id.IdType.ToUpper());
                        pdfFormFields.SetField("_.2-27", "GOVERNMENT OF" + " " + id.IssueProvince.ToUpper());
                        pdfFormFields.SetField("_.2-28", id.IdNumber);
                        pdfFormFields.SetField("_.2-29", /*SIN*/("SIN"));
                        pdfFormFields.SetField("_.2-30", /*SIN*/("GOVERNMENT OF CANADA"));
                        idfilled = true;
                        break;
                    case "PROVINCIAL DRIVER'S LICENSE":
                    case "PROVINCIAL PHOTO ID":
                    case "PASSPORT":
                    case "PR CARD":
                    case "HEALTH CARD":
                        pdfFormFields.SetField("_.2-26", id.IdType.ToUpper());
                        pdfFormFields.SetField("_.2-27", "GOVERNMENT OF" + " " + id.IssueProvince.ToUpper());
                        pdfFormFields.SetField("_.2-28", id.IdNumber);
                        idfilled = true;
                        break;
                }
                if (idfilled)
                {
                    break;
                }
            }
            //if (id.IdType.ToUpper() == "SIN")
            //{
            //    pdfFormFields.SetField("_.2-14", /*SIN*/id.IdNumber);
            //    pdfFormFields.SetField("_.2-31", /*SIN*/id.IdNumber);
            //    idfilled = true;
            //    break;
            //}
            //if (id.IdType.ToUpper() == "PROVINCIAL DRIVER'S LICENSE")
            //{
            //    pdfFormFields.SetField("_.2-26", id.IdType);
            //    pdfFormFields.SetField("_.2-27", "GOVERNMENT OF" + " " + id.IssueProvince);
            //    pdfFormFields.SetField("_.2-28", id.IdNumber);
            //    idfilled = true;
            //    break;
            //}
            //if (id.IdType.ToUpper() == "PROVINCIAL PHOTO ID")
            //{
            //    pdfFormFields.SetField("_.2-26", id.IdType);
            //    pdfFormFields.SetField("_.2-27", "GOVERNMENT OF" + " " + id.IssueProvince);
            //    pdfFormFields.SetField("_.2-28", id.IdNumber);
            //    idfilled = true;
            //    break;
            //}
            //if (id.IdType.ToUpper() == "PASSPORT")
            //{
            //    pdfFormFields.SetField("_.2-26", id.IdType);
            //    pdfFormFields.SetField("_.2-27", "GOVERNMENT OF" + " " + id.IssueProvince);
            //    pdfFormFields.SetField("_.2-28", id.IdNumber);
            //    idfilled = true;
            //    break;
            //}
            //if (id.IdType.ToUpper() == "PASSPORT")
            //{
            //    pdfFormFields.SetField("_.2-26", id.IdType);
            //    pdfFormFields.SetField("_.2-27", "GOVERNMENT OF" + " " + id.IssueProvince);
            //    pdfFormFields.SetField("_.2-28", id.IdNumber);
            //    idfilled = true;
            //    break;
            //}

            //if (id.IdType.ToUpper() == "PR CARD")
            //{
            //    pdfFormFields.SetField("_.2-26", id.IdType);
            //    pdfFormFields.SetField("_.2-27", "GOVERNMENT OF" + " " + id.IssueProvince);
            //    pdfFormFields.SetField("_.2-28", id.IdNumber);
            //    idfilled = true;
            //    break;
            //}
            //}
            //if (!idfilled && LoanApplication.PersonIDs.Count >= 1)
            //{
            //    pdfFormFields.SetField("_.2-26", LoanApplication.PersonIDs[0].IdType);
            //    pdfFormFields.SetField("_.2-27", "GOVERNMENT OF" + " " + LoanApplication.PersonIDs[0].IssueProvince);
            //    pdfFormFields.SetField("_.2-28", LoanApplication.PersonIDs[0].IdType);

            //}
            //pdfFormFields.SetField("_.2-29", /*SIN*/("SIN"));
            //pdfFormFields.SetField("_.2-30", /*SIN*/("GOVERNMENT OF CANADA"));


            if (LoanApplication.Applicant.Gender != null) /*xlRange.Range["B8"] != null && xlRange.Range["B8"].Value2 != null*/
            {
                switch (LoanApplication.Applicant.Gender.ToUpper())
                {
                    case "FEMALE":
                        pdfFormFields.SetField("_.2-15", "female");
                        break;
                    case "MALE":
                        pdfFormFields.SetField("_.2-15", "male");
                        break;
                }
            }
            pdfFormFields.SetField("_.2-17", "english");


            if (LoanApplication.Applicant.MaritalStatus != null /*xlRange.Range["D9"] != null && xlRange.Range["D9"].Value2 != null*/)
            {
                switch (LoanApplication.Applicant.MaritalStatus.ToUpper())
                {
                    case "COMMON LAW":
                        pdfFormFields.SetField("_.2-19", "common");
                        break;
                    case "MARRIED":
                        pdfFormFields.SetField("_.2-19", "married");
                        break;
                    case "SINGLE":
                        pdfFormFields.SetField("_.2-19", "single");
                        break;
                    case "DIVORCE":
                        pdfFormFields.SetField("_.2-19", "divorced");
                        break;
                    case "SPEARATED":
                        pdfFormFields.SetField("_.2-19", "separated");
                        break;
                    case "WIDOWED":
                        pdfFormFields.SetField("_.2-19", "widower");
                        break;
                    case "OTHER":
                        pdfFormFields.SetField("_.2-19", "not provided");
                        break;
                }
            }

            string address01 = "";
            foreach (Address add in LoanApplication.Applicant.PersonAddress)
            {
                if (add.CurrentFlag )
                {
                    if (add.AptNo != null)
                        address01 = add.AptNo;
                    if (add.StreetNo != null)
                        address01 = string.IsNullOrEmpty(address01) ? add.StreetNo.ToUpper() : address01 + "-" + add.StreetNo.ToUpper();
                    if (add.StreetName != null)
                        address01 = string.IsNullOrEmpty(address01) ? add.StreetName.ToUpper() : address01 + " " + add.StreetName.ToUpper();
                    if (add.City != null)
                        address01 = string.IsNullOrEmpty(address01) ? add.City.ToUpper() : address01 + " " + add.City.ToUpper();
                    if (add.Province != null)
                        address01 = string.IsNullOrEmpty(address01) ? add.Province.ToUpper() : address01 + " " + add.Province.ToUpper();
                    pdfFormFields.SetField("_.2-32", /*Current Address*/address01);
                    pdfFormFields.SetField("_.2-33", /*Postal Code*/add.Postcode.Replace(" ", "").ToUpper());
                    pdfFormFields.SetField("_.2-34", /*Home Phone*/add.Homephone);

                    if (add.LivingStatus != null)
                    {
                        switch (add.LivingStatus.ToUpper())
                        {
                            case "OWNER":
                                pdfFormFields.SetField("_.2-35", "owner");
                                break;
                            case "RENT":
                                pdfFormFields.SetField("_.2-35", "tenant");
                                break;
                            case "OTHER":
                            case "WITH PARENTS":
                            case "WITH OTHERS":
                                pdfFormFields.SetField("_.2-35", "other");
                                break;
                        }
                    }

                    if (add.StartDate != null /*xlRange.Range["K12"] != null && xlRange.Range["K12"].Value2 != null*/)
                    {
                        DateTime dt = DateTime.FromOADate(Int32.Parse(add.StartDate));
                        int y = ((DateTime.Now.Year - dt.Year) * 12 + DateTime.Now.Month - dt.Month) / 12;
                        int m = ((DateTime.Now.Year - dt.Year) * 12 + DateTime.Now.Month - dt.Month) % 12;
                        pdfFormFields.SetField("_.2-39", y.ToString());
                        pdfFormFields.SetField("_.2-40", m.ToString());
                    }
                    break;
                }
            }


            string address_principal = "";   //Page 3
            if (LoanApplication.Applicant.PersonAddress[0].AptNo != null /*xlRange.Range["E12"] != null && xlRange.Range["E12"].Value2 != null*/) /*Apt No*/
            {
                address_principal = LoanApplication.Applicant.PersonAddress[0].AptNo.ToUpper();
            }
            if (LoanApplication.Applicant.PersonAddress[0].StreetNo != null /*xlRange.Range["A12"] != null && xlRange.Range["A12"].Value2 != null*/) /*Street No*/
            {
                address_principal = string.IsNullOrEmpty(address_principal) ? LoanApplication.Applicant.PersonAddress[0].StreetNo.ToUpper() : (address_principal + "-" + LoanApplication.Applicant.PersonAddress[0].StreetNo.ToUpper());
            }
            if (LoanApplication.Applicant.PersonAddress[0].StreetName != null /*xlRange.Range["B12"] != null && xlRange.Range["B12"].Value2 != null*/) /*Street*/
            {
                address_principal = string.IsNullOrEmpty(address_principal) ? LoanApplication.Applicant.PersonAddress[0].StreetName.ToUpper() : (address_principal + " " + LoanApplication.Applicant.PersonAddress[0].StreetName.ToUpper());
            }
            if (LoanApplication.Applicant.PersonAddress[0].City != null /*xlRange.Range["F12"] != null && xlRange.Range["F12"].Value2 != null*/) /*City*/
            {
                address_principal = string.IsNullOrEmpty(address_principal) ? LoanApplication.Applicant.PersonAddress[0].City.ToUpper() : (address_principal + " " + LoanApplication.Applicant.PersonAddress[0].City.ToUpper());
            }
            if (LoanApplication.Applicant.PersonAddress[0].Province != null /*xlRange.Range["H12"] != null && xlRange.Range["H12"].Value2 != null*/) /*Province*/
            {
                address_principal = string.IsNullOrEmpty(address_principal) ? LoanApplication.Applicant.PersonAddress[0].Province.ToUpper() : (address_principal + " " + LoanApplication.Applicant.PersonAddress[0].Province.ToUpper());
            }
            if (LoanApplication.Applicant.PersonAddress[0].Postcode != null /*xlRange.Range["I12"] != null && xlRange.Range["I12"].Value2 != null*/) /*Post Code*/
            {
                address_principal = string.IsNullOrEmpty(address_principal) ? LoanApplication.Applicant.PersonAddress[0].Postcode.Replace(" ", "").ToUpper() : (address_principal + " " + LoanApplication.Applicant.PersonAddress[0].Postcode.Replace(" ", "").ToUpper());   //Page 3
            }


            string address_other = "";   //Page 3
            if (LoanApplication.Applicant.PersonAddress[1].AptNo != null /*xlRange.Range["E14"] != null && xlRange.Range["E14"].Value2 != null*/) /*Apt No*/
            {
                address_other = LoanApplication.Applicant.PersonAddress[1].AptNo.ToUpper();
            }
            if (LoanApplication.Applicant.PersonAddress[1].StreetNo != null /*xlRange.Range["A14"] != null && xlRange.Range["A14"].Value2 != null*/) /*Street No*/
            {
                address_other = string.IsNullOrEmpty(address_other) ? LoanApplication.Applicant.PersonAddress[1].StreetNo.ToUpper() : (address_other + "-" + LoanApplication.Applicant.PersonAddress[1].StreetNo.ToUpper());
            }
            if (LoanApplication.Applicant.PersonAddress[1].StreetName != null /*xlRange.Range["B14"] != null && xlRange.Range["B14"].Value2 != null*/) /*Street*/
            {
                address_other = string.IsNullOrEmpty(address_other) ? LoanApplication.Applicant.PersonAddress[1].StreetName.ToUpper() : (address_other + " " + LoanApplication.Applicant.PersonAddress[1].StreetName.ToUpper());
            }
            if (LoanApplication.Applicant.PersonAddress[1].City != null /*xlRange.Range["F14"] != null && xlRange.Range["F14"].Value2 != null*/) /*City*/
            {
                address_other = string.IsNullOrEmpty(address_other) ? LoanApplication.Applicant.PersonAddress[1].City.ToUpper() : (address_other + " " + LoanApplication.Applicant.PersonAddress[1].City.ToUpper());
            }
            if (LoanApplication.Applicant.PersonAddress[1].Province != null /*xlRange.Range["H14"] != null && xlRange.Range["H14"].Value2 != null*/) /*Province*/
            {
                address_other = string.IsNullOrEmpty(address_other) ? LoanApplication.Applicant.PersonAddress[1].Province.ToUpper() : (address_other + " " + LoanApplication.Applicant.PersonAddress[1].Province.ToUpper());
            }
            if (LoanApplication.Applicant.PersonAddress[1].Postcode != null /*xlRange.Range["I14"] != null && xlRange.Range["I14"].Value2 != null*/) /*Post Code*/
            {
                address_other = string.IsNullOrEmpty(address_other) ? LoanApplication.Applicant.PersonAddress[1].Postcode.Replace(" ", "").ToUpper() : (address_other + " " + LoanApplication.Applicant.PersonAddress[1].Postcode.Replace(" ", "").ToUpper());   //Page 3
            }


            ////APPLICANT EMPLOYMENT INFORMATION
            //foreach (Employment empl in LoanApplication.Applicant.PersonEmployment)
            //{
            //    if (empl.EmplStatus != null)
            //    {
            //        switch (empl.EmplStatus.ToUpper())
            //        {
            //            case "EMPLOYED":
            //                pdfFormFields.SetField("_.2-46", "permanent");
            //                break;
            //            case "SELF-EMPLOYED":
            //                pdfFormFields.SetField("_.2-46", "self employed");
            //                break;
            //        }
            //    }
            //}

            //Applicant Current Employment
            if (LoanApplication.Applicant.PersonEmployment[0].EmplStatus != null)
            {
                switch (LoanApplication.Applicant.PersonEmployment[0].EmplStatus.ToUpper())
                {
                    case "EMPLOYED":
                        pdfFormFields.SetField("_.2-46", /*Residential Status-Owner*/ "permanent");
                        break;
                    case "SELF-EMPLOYED":
                        pdfFormFields.SetField("_.2-46", /*Residential Status-Owner*/ "self employed");
                        break;
                }
            }

            pdfFormFields.SetField("_.2-53", LoanApplication.Applicant.PersonEmployment[0].Employer.ToUpper() /*(xlRange.Range["C29"] != null && xlRange.Range["C29"].Value2 != null) ? xlRange.Range["C29"].Value2.ToString() : ""*/);
            pdfFormFields.SetField("_.2-54", LoanApplication.Applicant.PersonEmployment[0].Occupation.ToUpper());
            //if (LoanApplication.Applicant.PersonEmployment[0].Occupation != null /*xlRange.Range["C34"] != null && xlRange.Range["C34"].Value2 != null && xlRange.Range["C34"].Value2.ToUpper() != "OTHER - SPECIFY"*/)
            //{
            //    pdfFormFields.SetField("_.2-54", LoanApplication.Applicant.PersonEmployment[0].Occupation.ToUpper());
            //}
            //else if (xlRange.Range["C34"] != null && xlRange.Range["C34"].Value2 != null && xlRange.Range["C34"].Value2.ToUpper() == "OTHER - SPECIFY")
            //{
            //    pdfFormFields.SetField("_.2-54", /*Other Position*/(xlRange.Range["E34"] != null && xlRange.Range["E34"].Value2 != null) ? xlRange.Range["E34"].Value2.ToString() : "");
            //}

            string empladd = "";
            if (LoanApplication.Applicant.PersonEmployment[0].Unit != null /*xlRange.Range["C31"] != null && xlRange.Range["C31"].Value2 != null*/) /*EMPL Unit*/
            {
                empladd = LoanApplication.Applicant.PersonEmployment[0].Unit;
            }
            if (LoanApplication.Applicant.PersonEmployment[0].StNo != null /*xlRange.Range["C30"] != null && xlRange.Range["C30"].Value2 != null*/) /*EMPL Street No*/
            {
                empladd = string.IsNullOrEmpty(empladd) ? LoanApplication.Applicant.PersonEmployment[0].StNo : (empladd + "-" + LoanApplication.Applicant.PersonEmployment[0].StNo);
            }
            if (LoanApplication.Applicant.PersonEmployment[0].StName != null /*xlRange.Range["D30"] != null && xlRange.Range["D30"].Value2 != null*/) /*EMPL Street Name*/
            {
                empladd = string.IsNullOrEmpty(empladd) ? LoanApplication.Applicant.PersonEmployment[0].StName.ToUpper() : (empladd + " " + LoanApplication.Applicant.PersonEmployment[0].StName.ToUpper());
            }
            if (LoanApplication.Applicant.PersonEmployment[0].City != null /*xlRange.Range["D31"] != null && xlRange.Range["D31"].Value2 != null*/) /*EMPL City*/
            {
                empladd = string.IsNullOrEmpty(empladd) ? LoanApplication.Applicant.PersonEmployment[0].City.ToUpper() : (empladd + " " + LoanApplication.Applicant.PersonEmployment[0].City.ToUpper());
            }
            if (LoanApplication.Applicant.PersonEmployment[0].Prov != null /*xlRange.Range["E31"] != null && xlRange.Range["E31"].Value2 != null*/) /*EMPL Province*/
            {
                empladd = string.IsNullOrEmpty(empladd) ? LoanApplication.Applicant.PersonEmployment[0].Prov.ToUpper() : (empladd + " " + LoanApplication.Applicant.PersonEmployment[0].Prov.ToUpper());
            }
            if (LoanApplication.Applicant.PersonEmployment[0].PostCode != null /*xlRange.Range["F31"] != null && xlRange.Range["F31"].Value2 != null*/) /*EMPL Post Code*/
            {
                empladd = string.IsNullOrEmpty(empladd) ? LoanApplication.Applicant.PersonEmployment[0].PostCode.ToUpper() : (empladd + " " + LoanApplication.Applicant.PersonEmployment[0].PostCode.ToUpper());
            }
            pdfFormFields.SetField("_.2-55", empladd);
            pdfFormFields.SetField("_.2-56", /*Work Phone*/ LoanApplication.Applicant.PersonEmployment[0].WorkPhone /*(xlRange.Range["K16"] != null && xlRange.Range["K16"].Value2 != null) ? xlRange.Range["K16"].Value2.ToString("###-###-####") : ""*/);
            pdfFormFields.SetField("_.2-58", /*Work Since Year*/ LoanApplication.Applicant.PersonEmployment[0].WorkSinceYear /*(xlRange.Range["D32"] != null && xlRange.Range["D32"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["D32"].Value2.ToString())).ToString("yyyy") : ""*/);
            pdfFormFields.SetField("_.2-59", /*Work Since Month*/ LoanApplication.Applicant.PersonEmployment[0].WorkSinceMonth /*(xlRange.Range["D32"] != null && xlRange.Range["D32"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["D32"].Value2.ToString())).ToString("MM") : ""*/);
            pdfFormFields.SetField("_.2-60", /*Annual Income*/ Math.Round(double.Parse(LoanApplication.Applicant.PersonEmployment[0].Income), 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("_.2-61", /*Monthly Income*/ Math.Round(double.Parse(LoanApplication.Applicant.PersonEmployment[0].Income)/12, 2).ToString("#,##0.#0"));


            //Applicant Previous Employment
            pdfFormFields.SetField("_.2-65", LoanApplication.Applicant.PersonEmployment[1].Employer.ToUpper() /*(xlRange.Range["G29"] != null && xlRange.Range["G29"].Value2 != null) ? xlRange.Range["G29"].Value2.ToString() : ""*/);
            if (LoanApplication.Applicant.PersonEmployment[1].Occupation != null /*xlRange.Range["G34"] != null && xlRange.Range["G34"].Value2 != null && xlRange.Range["G34"].Value2.ToUpper() != "OTHER - SPECIFY"*/)
            {
                pdfFormFields.SetField("_.2-66", LoanApplication.Applicant.PersonEmployment[1].Occupation.ToUpper());
            }
            //else if (xlRange.Range["G34"] != null && xlRange.Range["G34"].Value2 != null && xlRange.Range["G34"].Value2.ToUpper() == "OTHER - SPECIFY")
            //{
            //    pdfFormFields.SetField("_.2-66", /*Previous Other Position*/(xlRange.Range["J34"] != null && xlRange.Range["J34"].Value2 != null) ? xlRange.Range["J34"].Value2.ToString() : "");
            //}
            pdfFormFields.SetField("_.2-67", /*Previous Monthly Income*/ Math.Round(double.Parse(LoanApplication.Applicant.PersonEmployment[1].Income) / 12, 2).ToString("#,##0.#0") /*(xlRange.Range["G28"] != null && xlRange.Range["G28"].Value2 != null) ? Math.Round(double.Parse(xlRange.Range["G28"].Value2.ToString()) / 12, 2).ToString("#,##0.#0") : ""*/);
            pdfFormFields.SetField("_.2-69", /*Previous Since*/ DateTime.FromOADate(Int32.Parse(LoanApplication.Applicant.PersonEmployment[1].StartDate)).ToString("yyyyMMM") /*(xlRange.Range["H32"] != null && xlRange.Range["H32"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["H32"].Value2.ToString())).ToString("yyyyMMM") : ""*/);
            pdfFormFields.SetField("_.2-70", /*Previous To*/ DateTime.FromOADate(Int32.Parse(LoanApplication.Applicant.PersonEmployment[1].EndDate)).ToString("yyyyMMM") /*(xlRange.Range["L32"] != null && xlRange.Range["L32"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["L32"].Value2.ToString())).ToString("yyyyMMM") : ""*/);


            //CO-APPLICANT IMFORMATION
            if (LoanApplication.CoApplicationFlag) /*xlRange.Range["L6"] != null && xlRange.Range["L6"].Value2 != null && xlRange.Range["L6"].Value2.ToString() == "Yes"*/
            {
                //xlWorksheet = readWorkbook.Sheets["Co Applicant Info"];
                //xlRange = xlWorksheet.UsedRange;
                pdfFormFields.SetField("_.2-72", /*Last Name*/ LoanApplication.CoApplicant.LastName.ToUpper()); /*(xlRange.Range["B7"] != null && xlRange.Range["B7"].Value2 != null) ? xlRange.Range["B7"].Value2.ToString().ToUpper() : ""*/
                pdfFormFields.SetField("_.2-76", /*First name*/ LoanApplication.CoApplicant.FirstName.ToUpper()); /*(xlRange.Range["E7"] != null && xlRange.Range["E7"].Value2 != null) ? xlRange.Range["E7"].Value2.ToString().ToUpper() : ""*/
            }

            abc = "";
            if (LoanApplication.CoApplicant.LastName != null)
            {
                abc = LoanApplication.CoApplicant.LastName.ToUpper();
            }
            if (LoanApplication.CoApplicant.FirstName != null)
            {
                abc = string.IsNullOrEmpty(abc) ? LoanApplication.CoApplicant.LastName.ToUpper() : (abc + (new string(' ',53)) + LoanApplication.CoApplicant.LastName.ToUpper());
            }
            pdfFormFields.SetField("_.3-2", abc); //Page 2
            pdfFormFields.SetField("_.4-2", abc); //Page 3

            //pdfFormFields.SetField("_.2-73", /*DOB Year*/(xlRange.Range["D8"] != null && xlRange.Range["D8"].Value2 != null) ? xlRange.Range["D8"].Value2.ToString().ToUpper() : "");
            //pdfFormFields.SetField("_.2-74", /*DOB Month*/(xlRange.Range["F8"] != null && xlRange.Range["F8"].Value2 != null) ? convertMonth(xlRange.Range["F8"].Value2.ToString()) : "00");
            //pdfFormFields.SetField("_.2-75", /*DOB Day*/(xlRange.Range["H8"] != null && xlRange.Range["H8"].Value2 != null) ? ("00" + xlRange.Range["H8"].Value2.ToString()).Substring(("00" + xlRange.Range["H8"].Value2.ToString()).Length - 2) : "");
            pdfFormFields.SetField("_.2-73", /*DOB Year*/ LoanApplication.CoApplicant.DobYear /*(xlRange.Range["D8"] != null && xlRange.Range["D8"].Value2 != null) ? xlRange.Range["D8"].Value2.ToString().ToUpper() : ""*/);
            pdfFormFields.SetField("_.2-74", /*DOB Month*/ LoanApplication.CoApplicant.DobMonth /*(xlRange.Range["F8"] != null && xlRange.Range["F8"].Value2 != null) ? convertMonth(xlRange.Range["F8"].Value2.ToString()) : "00"*/);
            pdfFormFields.SetField("_.2-75", /*DOB Day*/ LoanApplication.CoApplicant.DobDay /*(xlRange.Range["H8"] != null && xlRange.Range["H8"].Value2 != null) ? ("00" + xlRange.Range["H8"].Value2.ToString()).Substring(("00" + xlRange.Range["H8"].Value2.ToString()).Length - 2) : ""*/);

            pdfFormFields.SetField("_.2-78", /*SIN*/ LoanApplication.CoApplicant.PersonIDs[0].IdNumber /*(xlRange.Range["K8"] != null && xlRange.Range["K8"].Value2 != null) ? xlRange.Range["K8"].Value2.ToString().ToUpper() : ""*/);
            pdfFormFields.SetField("_.2-87", /*SIN*/ LoanApplication.CoApplicant.PersonIDs[0].IdNumber /*(xlRange.Range["K8"] != null && xlRange.Range["K8"].Value2 != null) ? xlRange.Range["K8"].Value2.ToString().ToUpper() : ""*/);
            pdfFormFields.SetField("_.2-85", /*SIN*/("SIN"));
            pdfFormFields.SetField("_.2-86", /*SIN*/("GOVERNMENT OF CANADA"));

            pdfFormFields.SetField("_.2-82", /*Document Type*/ LoanApplication.CoApplicant.PersonIDs[1].IdType /*(xlRange.Range["A18"] != null && xlRange.Range["A18"].Value2 != null) ? xlRange.Range["A18"].Value2.ToString().ToUpper() : ""*/);
            pdfFormFields.SetField("_.2-83", /*Prov*/ LoanApplication.CoApplicant.PersonIDs[1].IssueProvince /*(xlRange.Range["D19"] != null && xlRange.Range["D19"].Value2 != null) ? ("GOVERNMENT OF" + " " + xlRange.Range["D19"].Value2.ToString().ToUpper()) : ""*/);
            pdfFormFields.SetField("_.2-84", /*ID Number*/ LoanApplication.CoApplicant.PersonIDs[1].IdNumber /*(xlRange.Range["C18"] != null && xlRange.Range["C18"].Value2 != null) ? xlRange.Range["C18"].Value2.ToString().ToUpper() : ""*/);

            if (LoanApplication.CoApplicant.Gender != null)
            {
                switch (LoanApplication.CoApplicant.Gender.ToUpper())
                {
                    case "FEMALE":
                        pdfFormFields.SetField("_.2-79", "female");
                        break;
                    case "MALE":
                        pdfFormFields.SetField("_.2-79", "male");
                        break;
                }
            }
            pdfFormFields.SetField("_.2-80", "english");

            if (LoanApplication.CoApplicant.MaritalStatus != null)
            {
                switch (LoanApplication.CoApplicant.MaritalStatus.ToUpper())
                {
                    case "COMMON LAW":
                        pdfFormFields.SetField("_.2-81", "common");
                        break;
                    case "MARRIED":
                        pdfFormFields.SetField("_.2-81", "married");
                        break;
                    case "SINGLE":
                        pdfFormFields.SetField("_.2-81", "single");
                        break;
                    case "DIVORCE":
                        pdfFormFields.SetField("_.2-81", "divorced");
                        break;
                    case "SPEARATED":
                        pdfFormFields.SetField("_.2-81", "separated");
                        break;
                    case "WIDOWED":
                        pdfFormFields.SetField("_.2-81", "widower");
                        break;
                    case "OTHER":
                        pdfFormFields.SetField("_.2-81", "not provided");
                        break;
                }
            }

            string address02 = "";
            if (LoanApplication.CoApplicant.PersonAddress[0].AptNo != null) /*xlRange.Range["E12"] != null && xlRange.Range["E12"].Value2 != null*/  /*Apt No*/
            {
                address02 = LoanApplication.CoApplicant.PersonAddress[0].AptNo;
            }
            if (LoanApplication.CoApplicant.PersonAddress[0].StreetNo != null) /*xlRange.Range["A12"] != null && xlRange.Range["A12"].Value2 != null*/  /*Street No*/
            {
                address02 = string.IsNullOrEmpty(address02) ? LoanApplication.CoApplicant.PersonAddress[0].StreetNo : (address02 + "-" + LoanApplication.CoApplicant.PersonAddress[0].StreetNo);
            }
            if (LoanApplication.CoApplicant.PersonAddress[0].StreetName != null) /*xlRange.Range["B12"] != null && xlRange.Range["B12"].Value2 != null*/ /*Street*/
            {
                address02 = string.IsNullOrEmpty(address02) ? LoanApplication.CoApplicant.PersonAddress[0].StreetName.ToUpper() : (address02 + " " + LoanApplication.CoApplicant.PersonAddress[0].StreetName.ToUpper());
            }
            if (LoanApplication.CoApplicant.PersonAddress[0].City != null) /*xlRange.Range["F12"] != null && xlRange.Range["F12"].Value2 != null*/ /*City*/
            {
                address02 = string.IsNullOrEmpty(address02) ? LoanApplication.CoApplicant.PersonAddress[0].City.ToUpper() : (address02 + new string(' ', 210) + LoanApplication.CoApplicant.PersonAddress[0].City.ToUpper());
            }
            if (LoanApplication.CoApplicant.PersonAddress[0].Province != null) /*xlRange.Range["H12"] != null && xlRange.Range["H12"].Value2 != null*/ /*Province*/
            {
                address02 = string.IsNullOrEmpty(address02) ? LoanApplication.CoApplicant.PersonAddress[0].Province.ToUpper() : (address02 + new string(' ', 20) + LoanApplication.CoApplicant.PersonAddress[0].Province.ToUpper());
            }
            pdfFormFields.SetField("_.2-88", /*CoApp address as samae as Applicant*/ (address02 == address01) ? "same" : "");


            pdfFormFields.SetField("_.2-91", /*Home Phone*/ (LoanApplication.CoApplicant.Homephone != null) ? LoanApplication.CoApplicant.Homephone : /*Cell Phone*/ (LoanApplication.CoApplicant.Cellphone != null) ? LoanApplication.CoApplicant.Cellphone : "");
            ///*Home Phone*/ (xlRange.Range["H16"] != null && xlRange.Range["H16"].Value2 != null) ? xlRange.Range["H16"].Value2.ToString("###-####-###") : /*Cell Phone*/((xlRange.Range["F16"] != null && xlRange.Range["F16"].Value2 != null) ? xlRange.Range["F16"].Value2.ToString("###-####-####") : ""));

            if (LoanApplication.CoApplicant.PersonAddress[0].LivingStatus != null /*xlRange.Range["B9"] != null && xlRange.Range["B9"].Value2 != null*/)
            {
                switch (LoanApplication.CoApplicant.PersonAddress[0].LivingStatus.ToUpper())
                {
                    case "OWNER":
                        pdfFormFields.SetField("_.2-92", "owner");
                        break;
                    case "RENT":
                        pdfFormFields.SetField("_.2-92", "tenant");
                        break;
                    case "OTHER":
                    case "WITH PARENTS":
                    case "WITH OTHERS":
                        pdfFormFields.SetField("_.2-92", "other");
                        break;
                }
            }

            if (LoanApplication.CoApplicant.PersonAddress[0].StartDate != null /*xlRange.Range["K12"] != null && xlRange.Range["K12"].Value2 != null*/) // && xlRange.Range["K12"].Value2.ToString().Contains("/")
            {
                DateTime dt = DateTime.FromOADate(Int32.Parse(LoanApplication.CoApplicant.PersonAddress[0].StartDate));
                int y = ((DateTime.Now.Year - dt.Year) * 12 + DateTime.Now.Month - dt.Month) / 12;
                int m = ((DateTime.Now.Year - dt.Year) * 12 + DateTime.Now.Month - dt.Month) % 12;
                pdfFormFields.SetField("_.2-93", y.ToString());
                pdfFormFields.SetField("_.2-94", m.ToString());
            }


            //CoAPPLICANT EMPLOYMENT INFOMATION
            if (LoanApplication.CoApplicant.PersonEmployment[0].EmplStatus != null /*xlRange.Range["C27"] != null && xlRange.Range["C27"].Value2 != null*/) /*Employment Status*/
            {
                switch (LoanApplication.CoApplicant.PersonEmployment[0].EmplStatus.ToUpper())
                {
                    case "EMPLOYED":
                        pdfFormFields.SetField("_.2-98", "permanent");
                        break;
                    case "SELF-EMPLOYED":
                        pdfFormFields.SetField("_.2-98", "self employed");
                        break;
                }
            }

            pdfFormFields.SetField("_.2-99", /*Company Name*/ LoanApplication.CoApplicant.PersonEmployment[0].Employer /*(xlRange.Range["C29"] != null && xlRange.Range["C29"].Value2 != null) ? xlRange.Range["C29"].Value2.ToString() : ""*/);
            pdfFormFields.SetField("_.2-100", LoanApplication.CoApplicant.PersonEmployment[0].Occupation.ToUpper());
            //if (xlRange.Range["C34"] != null && xlRange.Range["C34"].Value2 != null && xlRange.Range["C34"].Value2.ToUpper() != "OTHER - SPECIFY")
            //{
            //    pdfFormFields.SetField("_.2-100", /*Position*/ xlRange.Range["C34"].Value2.ToString());
            //}
            //else if (xlRange.Range["C34"] != null && xlRange.Range["C34"].Value2 != null && xlRange.Range["C34"].Value2.ToUpper() == "OTHER - SPECIFY")
            //{
            //    pdfFormFields.SetField("_.2-100", /*Other Position*/(xlRange.Range["E34"] != null && xlRange.Range["E34"].Value2 != null) ? xlRange.Range["E34"].Value2.ToString() : "");
            //}


            empladd = "";
            if (LoanApplication.CoApplicant.PersonEmployment[0].Unit != null /*xlRange.Range["C31"] != null && xlRange.Range["C31"].Value2 != null*/) /*Unit*/
            {
                empladd = LoanApplication.CoApplicant.PersonEmployment[0].Unit;
            }
            if (LoanApplication.CoApplicant.PersonEmployment[0].StNo != null /*xlRange.Range["C30"] != null && xlRange.Range["C30"].Value2 != null*/) /*Street No*/
            {
                empladd = string.IsNullOrEmpty(empladd) ? LoanApplication.CoApplicant.PersonEmployment[0].StNo : (empladd += "-" + LoanApplication.CoApplicant.PersonEmployment[0].StNo);
            }
            if (LoanApplication.CoApplicant.PersonEmployment[0].StName != null /*xlRange.Range["D30"] != null && xlRange.Range["D30"].Value2 != null*/) /*Street Name*/
            {
                empladd = string.IsNullOrEmpty(empladd) ? LoanApplication.CoApplicant.PersonEmployment[0].StName.ToUpper() : (empladd += " " + LoanApplication.CoApplicant.PersonEmployment[0].StName.ToUpper());
            }
            if (LoanApplication.CoApplicant.PersonEmployment[0].City != null /*xlRange.Range["D31"] != null && xlRange.Range["D31"].Value2 != null*/) /*City*/
            {
                empladd = string.IsNullOrEmpty(empladd) ? LoanApplication.CoApplicant.PersonEmployment[0].City.ToUpper() : (empladd += " " + LoanApplication.CoApplicant.PersonEmployment[0].City.ToUpper());
            }
            if (LoanApplication.CoApplicant.PersonEmployment[0].Prov != null /*xlRange.Range["E31"] != null && xlRange.Range["E31"].Value2 != null*/) /*Province*/
            {
                empladd = string.IsNullOrEmpty(empladd) ? LoanApplication.CoApplicant.PersonEmployment[0].Prov.ToUpper() : (empladd += " " + LoanApplication.CoApplicant.PersonEmployment[0].Prov.ToUpper());
            }
            if (LoanApplication.CoApplicant.PersonEmployment[0].PostCode != null /*xlRange.Range["F31"] != null && xlRange.Range["F31"].Value2 != null*/) /*Post Code*/
            {
                empladd = string.IsNullOrEmpty(empladd) ? LoanApplication.CoApplicant.PersonEmployment[0].PostCode : (empladd += " " + LoanApplication.CoApplicant.PersonEmployment[0].PostCode.ToUpper());
            }
            pdfFormFields.SetField("_.2-101", /*CO Working Address*/ empladd);

            pdfFormFields.SetField("_.2-102", /*CO Work Phone*/ LoanApplication.CoApplicant.PersonEmployment[0].WorkPhone); /*(xlRange.Range["K16"] != null && xlRange.Range["K16"].Value2 != null) ? xlRange.Range["K16"].Value2.ToString("###-###-####") : ""*/
            pdfFormFields.SetField("_.2-104", /*CO Work Since Year*/ DateTime.FromOADate(Int32.Parse(LoanApplication.CoApplicant.PersonEmployment[0].StartDate)).ToString("yyyy")); /*(xlRange.Range["D32"] != null && xlRange.Range["D32"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["D32"].Value2.ToString())).ToString("yyyy") : ""*/
            pdfFormFields.SetField("_.2-105", /*CO Work Since Month */ DateTime.FromOADate(Int32.Parse(LoanApplication.CoApplicant.PersonEmployment[0].StartDate)).ToString("MM")); /*(xlRange.Range["D32"] != null && xlRange.Range["D32"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["D32"].Value2.ToString())).ToString("MM") : ""*/
            pdfFormFields.SetField("_.2-106", /*Annual Income*/ Math.Round(double.Parse(LoanApplication.CoApplicant.PersonEmployment[0].Income), 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("_.2-107", /*Monthly Income*/ Math.Round(double.Parse(LoanApplication.CoApplicant.PersonEmployment[0].Income) / 12, 2).ToString("#,##0.#0"));


            //Assets & Liabilities
            //xlWorksheet = readWorkbook.Sheets["Assets & Liabilities"];
            //xlRange = xlWorksheet.UsedRange;

            double TFSASum = 0.0;
            double RRSPSum = 0.0;
            double RESPSum = 0.0;
            double MFSum = 0.0;
            double GICSum = 0.0;
            double StocksSum = 0.0;
            double BankSum = 0.0;
            double InvestSum = 0.0;
            double PrincipalSum = 0.0;
            double OtherPropSum = 0.0;

            string BankStr = "";
            string StocksStr = "STOCK-";
            string MFStr = "MUTUAL FUND-";
            string GICStr = "GIC-";
            string TFSAStr = "TFSA-";
            string RRSPStr = "RRSP-";
            string RESPStr = "RESP-";
            string InvestStr = "Investment-";
            string PrincipalStr = "";
            string OtherPropStr = "";

            foreach (Asset asset in LoanApplication.Applicant.PersonAsset)
            {
                switch (asset.AssetsType.ToUpper())
                {
                    case "CHECKING ACCOUNT":
                    case "SAVING ACCOUNT":
                        BankSum += double.Parse(asset.MarketValue);
                        BankStr = string.IsNullOrEmpty(BankStr) ? asset.Institution.ToUpper() : (BankStr += " & " + asset.Institution.ToUpper());
                        break;
                    case "STOCKS":
                        StocksSum += double.Parse(asset.MarketValue);
                        StocksStr = string.IsNullOrEmpty(StocksStr) ? asset.Institution.ToUpper() : (StocksStr += " & " + asset.Institution.ToUpper());
                        break;
                    case "MUTUAL FUNDS":
                        MFSum += double.Parse(asset.MarketValue);
                        MFStr = string.IsNullOrEmpty(MFStr) ? asset.Institution.ToUpper() : (MFStr += " & " + asset.Institution.ToUpper());
                        break;
                    case "GIC":
                        GICSum += double.Parse(asset.MarketValue);
                        GICStr = string.IsNullOrEmpty(GICStr) ? asset.Institution.ToUpper() : (GICStr += " & " + asset.Institution.ToUpper());
                        break;
                    case "TFSA":
                        TFSASum += double.Parse(asset.MarketValue);
                        TFSAStr = string.IsNullOrEmpty(TFSAStr) ? asset.Institution.ToUpper() : (TFSAStr += " & " + asset.Institution.ToUpper());
                        break;
                    case "RRSP":
                    case "GROUP RRSP":
                    case "SPOUSAL RRSP":
                        RRSPSum += double.Parse(asset.MarketValue);
                        RRSPStr = string.IsNullOrEmpty(RRSPStr) ? asset.Institution.ToUpper() : (RRSPStr += " & " + asset.Institution.ToUpper());
                        break;
                    case "RESP":
                        RESPSum += double.Parse(asset.MarketValue);
                        RESPStr = string.IsNullOrEmpty(RESPStr) ? asset.Institution.ToUpper() : (RESPStr += " & " + asset.Institution.ToUpper());
                        break;
                    case "INVESTMENT":
                        InvestSum += double.Parse(asset.MarketValue);
                        InvestStr = string.IsNullOrEmpty(InvestStr) ? asset.Institution.ToUpper() : (InvestStr += " & " + asset.Institution.ToUpper());
                        break;
                    case "PRINCIPAL":
                        PrincipalSum += double.Parse(asset.MarketValue);
                        PrincipalStr = string.IsNullOrEmpty(PrincipalStr) ? asset.Institution.ToUpper() : (PrincipalStr += " & " + asset.Institution.ToUpper());
                        break;
                    case "OTHER PROPERTY":
                        OtherPropSum += OtherPropSum;
                        OtherPropStr = string.IsNullOrEmpty(OtherPropStr) ? asset.Institution.ToUpper() : (OtherPropStr += " & " + asset.Institution.ToUpper());
                        break;
                }
            }
       
            int ii = 1;
            int iri = 1;
            int rei = 1;            

            if (BankSum > 0.00)
            {
                pdfFormFields.SetField("_.3-3", BankStr);
                pdfFormFields.SetField("_.3-4", Math.Round(BankSum, 2).ToString("#,##0.#0"));
            }

            int count = 0;
            if (StocksSum > 0.00)
            {
                count++;
                //break;
            }
            if (MFSum > 0.00)
            {
                count++;
                //break;
            }
            if (GICSum > 0.00)
            {
                count++;
                //break;
            }
            if (InvestSum > 0.00)
            {
                count++;
                //break;
            }

            if (count <= 3)
            {
                if (StocksSum > 0.00)
                {
                    switch (ii)
                    {
                        case 1:
                            pdfFormFields.SetField("_.3-12", StocksStr);
                            pdfFormFields.SetField("_.3-13", Math.Round(StocksSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                        case 2:
                            pdfFormFields.SetField("_.3-15", StocksStr);
                            pdfFormFields.SetField("_.3-16", Math.Round(StocksSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                        case 3:
                            pdfFormFields.SetField("_.3-18", StocksStr);
                            pdfFormFields.SetField("_.3-19", Math.Round(StocksSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                    }

                }
                if (MFSum > 0.00)
                {
                    switch (ii)
                    {
                        case 1:
                            pdfFormFields.SetField("_.3-12", MFStr);
                            pdfFormFields.SetField("_.3-13", Math.Round(MFSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                        case 2:
                            pdfFormFields.SetField("_.3-15", MFStr);
                            pdfFormFields.SetField("_.3-16", Math.Round(MFSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                        case 3:
                            pdfFormFields.SetField("_.3-18", MFStr);
                            pdfFormFields.SetField("_.3-19", Math.Round(MFSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                    }
                }
                if (GICSum > 0.00)
                {
                    switch (ii)
                    {
                        case 1:
                            pdfFormFields.SetField("_.3-12", GICStr);
                            pdfFormFields.SetField("_.3-13", Math.Round(GICSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                        case 2:
                            pdfFormFields.SetField("_.3-15", GICStr);
                            pdfFormFields.SetField("_.3-16", Math.Round(GICSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                        case 3:
                            pdfFormFields.SetField("_.3-18", GICStr);
                            pdfFormFields.SetField("_.3-19", Math.Round(GICSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                    }
                }
                if (InvestSum > 0.00)
                {
                    switch (ii)
                    {
                        case 1:
                            pdfFormFields.SetField("_.3-12", InvestStr);
                            pdfFormFields.SetField("_.3-13", Math.Round(InvestSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                        case 2:
                            pdfFormFields.SetField("_.3-15", InvestStr);
                            pdfFormFields.SetField("_.3-16", Math.Round(InvestSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                        case 3:
                            pdfFormFields.SetField("_.3-18", InvestStr);
                            pdfFormFields.SetField("_.3-19", Math.Round(InvestSum, 2).ToString("#,##0.#0"));
                            ii++;
                            break;
                    }
                }
            }
            else //不用再测算StocksSum这四个是否大于0 ---因为Count已经大于3 就说明这四项都有值
            {
                //StockSum + MFSum + GICSum
                pdfFormFields.SetField("_.3-12", (StocksStr + "&" + MFStr + "&" + GICStr).ToString());
                pdfFormFields.SetField("_.3-13", Math.Round(StocksSum + MFSum + GICSum, 2).ToString("#,##0.#0"));

                //InvestLoanSum
                pdfFormFields.SetField("_.3-15", InvestStr);
                pdfFormFields.SetField("_.3-16", Math.Round(InvestSum, 2).ToString("#,##0.#0"));
            }
            //////////////////////////////////////////////////////////////////////////////////////


            if (TFSASum > 0.00)
            {
                switch (iri)
                {
                    case 1:
                        pdfFormFields.SetField("_.3-21", TFSAStr);
                        pdfFormFields.SetField("_.3-22", Math.Round(TFSASum, 2).ToString("#,##0.#0"));
                        iri++;
                        break;
                    case 2:
                        pdfFormFields.SetField("_.3-24", TFSAStr);
                        pdfFormFields.SetField("_.3-25", Math.Round(TFSASum, 2).ToString("#,##0.#0"));
                        iri++;
                        break;
                    case 3:
                        pdfFormFields.SetField("_.3-27", TFSAStr);
                        pdfFormFields.SetField("_.3-28", Math.Round(TFSASum, 2).ToString("#,##0.#0"));
                        iri++;
                        break;
                }

            }
            if (RRSPSum > 0.00)
            {
                switch (iri)
                {
                    case 1:
                        pdfFormFields.SetField("_.3-21", RRSPStr);
                        pdfFormFields.SetField("_.3-22", Math.Round(RRSPSum, 2).ToString("#,##0.#0"));
                        iri++;
                        break;
                    case 2:
                        pdfFormFields.SetField("_.3-24", RRSPStr);
                        pdfFormFields.SetField("_.3-25", Math.Round(RRSPSum, 2).ToString("#,##0.#0"));
                        iri++;
                        break;
                    case 3:
                        pdfFormFields.SetField("_.3-27", RRSPStr);
                        pdfFormFields.SetField("_.3-28", Math.Round(RRSPSum, 2).ToString("#,##0.#0"));
                        iri++;
                        break;
                }
            }
            if (RESPSum > 0.00)
            {
                switch (iri)
                {
                    case 1:
                        pdfFormFields.SetField("_.3-21", RESPStr);
                        pdfFormFields.SetField("_.3-22", Math.Round(RESPSum, 2).ToString("#,##0.#0"));
                        iri++;
                        break;
                    case 2:
                        pdfFormFields.SetField("_.3-24", RESPStr);
                        pdfFormFields.SetField("_.3-25", Math.Round(RESPSum, 2).ToString("#,##0.#0"));
                        iri++;
                        break;
                    case 3:
                        pdfFormFields.SetField("_.3-27", RESPStr);
                        pdfFormFields.SetField("_.3-28", Math.Round(RESPSum, 2).ToString("#,##0.#0"));
                        iri++;
                        break;
                }
            }
            if (PrincipalSum > 0.00)
            {
                switch (rei)
                {
                    case 1:
                        pdfFormFields.SetField("_.3-30", PrincipalStr);
                        pdfFormFields.SetField("_.3-31", PrincipalSum.ToString("#,##0.#0"));
                        rei++;
                        break;
                    case 2:
                        pdfFormFields.SetField("_.3-33", PrincipalStr);
                        pdfFormFields.SetField("_.3-34", PrincipalSum.ToString("#,##0.#0"));
                        rei++;
                        break;
                    case 3:
                        pdfFormFields.SetField("_.3-36", PrincipalStr);
                        pdfFormFields.SetField("_.3-37", PrincipalSum.ToString("#,##0.#0"));
                        rei++;
                        break;
                }
            }
            if (OtherPropSum > 0.00)
            {
                switch (rei)
                {
                    case 1:
                        pdfFormFields.SetField("_.3-30", OtherPropStr);
                        pdfFormFields.SetField("_.3-31", OtherPropSum.ToString("#,##0.#0"));
                        rei++;
                        break;
                    case 2:
                        pdfFormFields.SetField("_.3-33", OtherPropStr);
                        pdfFormFields.SetField("_.3-34", OtherPropSum.ToString("#,##0.#0"));
                        rei++;
                        break;
                    case 3:
                        pdfFormFields.SetField("_.3-36", OtherPropStr);
                        pdfFormFields.SetField("_.3-37", OtherPropSum.ToString("#,##0.#0"));
                        rei++;
                        break;
                }
            }            
            pdfFormFields.SetField("_.3-48", /*Total Assets*/ Math.Round(TFSASum + RRSPSum + RESPSum + MFSum + GICSum + StocksSum + BankSum + InvestSum + PrincipalSum + OtherPropSum, 2).ToString("#,##0.#0"));


            //Liabilities
            double LCBal = 0.0;
            double LCMonBal = 0.0;
            double PLBal = 0.0;
            double PLMonBal = 0.0;
            double SLBal = 0.0;
            double SLMonBal = 0.0;
            double ILBal = 0.0;
            double ILMonBal = 0.0;
            double PiMrge = 0.0; /*Mortgage*/
            double PiMonMrge = 0.0; /*Mortgage Monthly Payment*/
            double OtherMrge = 0.0;
            double OtherMonMrge = 0.0;
            double PropertyTax = 0.0;
            double CondoFee = 0.0;
            double Rent_Veh = 0.0;

            string LCStr = "";
            string PLStr = "PERSONAL LOAN-";
            string SLStr = "STUDENT LOAN-";
            string ILStr = "INVESTMENT LOAN-";
            string PiMrgeStr = "";
            string OtherMrgeStr = "";
            string RentVehStr = "";

            foreach (Liability lb in LoanApplication.Applicant.PersonLiability)
            {
                switch (lb.LiType.ToUpper())
                {
                    case "LINE OF CREDIT":
                        LCBal += double.Parse(lb.LiBalance);
                        LCMonBal += double.Parse(lb.LiMonthlyPayt);
                        LCStr = string.IsNullOrEmpty(LCStr) ? lb.Institution.ToUpper() : (LCStr += " & " + lb.Institution.ToUpper());
                        break;
                    case "PERSONAL LOAN":
                        PLBal += double.Parse(lb.LiBalance);
                        PLMonBal += double.Parse(lb.LiMonthlyPayt);
                        PLStr = string.IsNullOrEmpty(PLStr) ? lb.Institution.ToUpper() : (PLStr += " & " + lb.Institution.ToUpper());
                        break;
                    case "STUDENT LOAN":
                        SLBal += double.Parse(lb.LiBalance);
                        SLMonBal += double.Parse(lb.LiMonthlyPayt);
                        SLStr = string.IsNullOrEmpty(SLStr) ? lb.Institution.ToUpper() : (SLStr += " & " + lb.Institution.ToUpper());
                        break;
                    case "INVESTMENT LOAN":
                        ILBal += double.Parse(lb.LiBalance);
                        ILMonBal += double.Parse(lb.LiMonthlyPayt);
                        ILStr = string.IsNullOrEmpty(ILStr) ? lb.Institution.ToUpper() : (ILStr += " & " + lb.Institution.ToUpper());
                        break;
                    case "PRINCIPAL":
                        PiMrge += double.Parse(lb.LiBalance);
                        PiMonMrge += double.Parse(lb.LiMonthlyPayt);
                        PiMrgeStr = string.IsNullOrEmpty(PiMrgeStr) ? lb.Institution.ToUpper() : (PiMrgeStr += " & " + lb.Institution.ToUpper());
                        break;
                    case "OTHER PROPERTY":
                        OtherMrge += double.Parse(lb.LiBalance);
                        OtherMonMrge += double.Parse(lb.LiMonthlyPayt);
                        OtherMrgeStr = string.IsNullOrEmpty(OtherMrgeStr) ? lb.Institution.ToUpper() : (OtherMrgeStr += " & " + lb.Institution.ToUpper());
                        break;
                    case "PROPERTY TAX":
                        PropertyTax += double.Parse(lb.LiBalance);
                        break;
                    case "CONDO FEE":
                        CondoFee += double.Parse(lb.LiBalance);
                        break;
                    case "RENT":
                        Rent_Veh += double.Parse(lb.LiBalance);
                        RentVehStr = string.IsNullOrEmpty(RentVehStr) ? lb.Institution.ToUpper() : (RentVehStr += " & " + lb.Institution.ToUpper());
                        break;
                    case "VEHICLE":
                        Rent_Veh += double.Parse(lb.LiBalance);
                        RentVehStr = string.IsNullOrEmpty(RentVehStr) ? lb.Institution.ToUpper() : (RentVehStr += " & " + lb.Institution.ToUpper());
                        break;
                }
            }
            
            int lci = 1;
            int li = 1;
            int mrgei = 1;

            if (LCBal > 0.00)
            {
                switch (lci)
                {
                    case 1:
                        pdfFormFields.SetField("_.3-50", LCStr);
                        pdfFormFields.SetField("_.3-52", LCBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-54", LCMonBal.ToString("#,##0.#0"));
                        lci++;
                        break;
                    case 2:
                        pdfFormFields.SetField("_.3-55", LCStr);
                        pdfFormFields.SetField("_.3-57", LCBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-59", LCMonBal.ToString("#,##0.#0"));
                        lci++;
                        break;
                    case 3:
                        pdfFormFields.SetField("_.3-60", LCStr);
                        pdfFormFields.SetField("_.3-62", LCBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-64", LCMonBal.ToString("#,##0.#0"));
                        lci++;
                        break;
                }
            }
            if (PLBal > 0.00)
            {
                switch (li)
                {
                    case 1:
                        pdfFormFields.SetField("_.3-65", PLStr);
                        pdfFormFields.SetField("_.3-67", PLBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-69", PLMonBal.ToString("#,##0.#0"));
                        li++;
                        break;
                    case 2:
                        pdfFormFields.SetField("_.3-70", PLStr);
                        pdfFormFields.SetField("_.3-72", PLBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-74", PLMonBal.ToString("#,##0.#0"));
                        li++;
                        break;
                    case 3:
                        pdfFormFields.SetField("_.3-75", PLStr);
                        pdfFormFields.SetField("_.3-77", PLBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-79", PLMonBal.ToString("#,##0.#0"));
                        li++;
                        break;
                }
            }
            if (SLBal > 0.00)
            {
                switch (li)
                {
                    case 1:
                        pdfFormFields.SetField("_.3-65", SLStr);
                        pdfFormFields.SetField("_.3-67", SLBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-69", SLMonBal.ToString("#,##0.#0"));
                        li++;
                        break;
                    case 2:
                        pdfFormFields.SetField("_.3-70", SLStr);
                        pdfFormFields.SetField("_.3-72", SLBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-74", SLMonBal.ToString("#,##0.#0"));
                        li++;
                        break;
                    case 3:
                        pdfFormFields.SetField("_.3-75", SLStr);
                        pdfFormFields.SetField("_.3-77", SLBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-79", SLMonBal.ToString("#,##0.#0"));
                        li++;
                        break;
                }
            }
            if (ILBal > 0.00)
            {
                switch (li)
                {
                    case 1:
                        pdfFormFields.SetField("_.3-65", ILStr);
                        pdfFormFields.SetField("_.3-67", ILBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-69", ILMonBal.ToString("#,##0.#0"));
                        li++;
                        break;
                    case 2:
                        pdfFormFields.SetField("_.3-70", ILStr);
                        pdfFormFields.SetField("_.3-72", ILBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-74", ILMonBal.ToString("#,##0.#0"));
                        li++;
                        break;
                    case 3:
                        pdfFormFields.SetField("_.3-75", ILStr);
                        pdfFormFields.SetField("_.3-77", ILBal.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-79", ILMonBal.ToString("#,##0.#0"));
                        li++;
                        break;
                }
            }
            if (PiMrge > 0.00)
            {
                switch (mrgei)
                {
                    case 1:
                        pdfFormFields.SetField("_.3-95", PiMrgeStr);
                        pdfFormFields.SetField("_.3-97", PiMrge.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-99", PiMonMrge.ToString("#,##0.#0"));
                        mrgei++;
                        break;
                    case 2:
                        pdfFormFields.SetField("_.3-100", PiMrgeStr);
                        pdfFormFields.SetField("_.3-102", PiMrge.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-104", PiMonMrge.ToString("#,##0.#0"));
                        mrgei++;
                        break;
                    case 3:
                        pdfFormFields.SetField("_.3-105", PiMrgeStr);
                        pdfFormFields.SetField("_.3-307", PiMrge.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-309", PiMonMrge.ToString("#,##0.#0"));
                        mrgei++;
                        break;
                }
            }
            if (OtherMrge > 0.00)
            {
                switch (mrgei)
                {
                    case 1:
                        pdfFormFields.SetField("_.3-95", OtherMrgeStr);
                        pdfFormFields.SetField("_.3-97", OtherMrge.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-99", OtherMonMrge.ToString("#,##0.#0"));
                        mrgei++;
                        break;
                    case 2:
                        pdfFormFields.SetField("_.3-100", OtherMrgeStr);
                        pdfFormFields.SetField("_.3-102", OtherMrge.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-104", OtherMonMrge.ToString("#,##0.#0"));
                        mrgei++;
                        break;
                    case 3:
                        pdfFormFields.SetField("_.3-105", OtherMrgeStr);
                        pdfFormFields.SetField("_.3-307", OtherMrge.ToString("#,##0.#0"));
                        pdfFormFields.SetField("_.3-309", OtherMonMrge.ToString("#,##0.#0"));
                        mrgei++;
                        break;
                }
            }
            if (PropertyTax > 0.00)
            {
                pdfFormFields.SetField("_.3-310", "PROPERTY TAX");
                pdfFormFields.SetField("_.3-311", PropertyTax.ToString("#,##0.#0"));
            }
            if (CondoFee > 0.0)
            {
                pdfFormFields.SetField("_.3-312", "CONDO FEE");
                pdfFormFields.SetField("_.3-313", CondoFee.ToString("#,##0.#0"));
            }
            if (Rent_Veh > 0.00)
            {
                pdfFormFields.SetField("_.3-314", RentVehStr);
                pdfFormFields.SetField("_.3-315", Rent_Veh.ToString("#,##0.#0"));
            }

            pdfFormFields.SetField("_.3-318", /*Total Liabilities*/ (LCBal + PLBal + SLBal + ILBal + PiMrge + OtherMrge).ToString("#,##0.#0"));
            pdfFormFields.SetField("_.3-320", /*Total Monthly Payment*/ (LCMonBal + PLMonBal + SLMonBal + ILMonBal + PiMonMrge + OtherMonMrge + PropertyTax + CondoFee + Rent_Veh).ToString("#,##0.#0"));
            pdfFormFields.SetField("_.3-321", /*Net Worth*/ Math.Round(double.Parse(pdfFormFields.GetField("_.3-48")) - double.Parse(pdfFormFields.GetField("_.3-318")), 2).ToString("#,##0.#0"));



            //REQUESTED FINANCING
            //xlWorksheet = readWorkbook.Sheets["Account Info"];
            //xlRange = xlWorksheet.UsedRange;

            pdfFormFields.SetField("_.4-4", "100");

            pdfFormFields.SetField("_.4-6", LoanApplication.SourceLoan.ApplyAmount);
            pdfFormFields.SetField("_.4-7", "0");
            pdfFormFields.SetField("_.4-8", LoanApplication.SourceLoan.ApplyAmount);

            pdfFormFields.SetField("_.4-10", "segregated");
            pdfFormFields.SetField("_.4-11", "NEW");

            pdfFormFields.SetField("_.4-18", "2.45");
            pdfFormFields.SetField("_.4-19", "0.75");
            pdfFormFields.SetField("_.4-20", "3.20");

            pdfFormFields.SetField("_.4-28", "home");
            pdfFormFields.SetField("_.4-30", "no");
            pdfFormFields.SetField("_.4-39", pdfFormFields.GetField("_.2-1"));



            pdfStamper.FormFlattening = false; //False-PDF可更改   //True-PDF不可更改
            pdfStamper.Close();
            pdfReader.Close();
            MessageBox.Show("National Bank Loan Application Finished!");
            ErrMessage = string.IsNullOrEmpty(ErrMessage) ? "Fihished!" : ErrMessage + " \nFinished!";
        }


        private void AutoFill_iATrust_Loan(Investment LoanApplication, string SourePDFfolder, string outputdirectory, string NPCode="")
        {
            PdfReader pdfReader;
            PdfStamper pdfStamper;
            AcroFields pdfFormFields;
            if (LoanApplication.SourceLoan == null)
            {
                ErrMessage = string.IsNullOrEmpty(ErrMessage) ? "No iA Trust Loan application!" : ErrMessage + " \nNo iA Trust Loan application!";
                return;
            }
            string iATrustappFileName = Path.Combine(outputdirectory, 
                                                     LoanApplication.Applicant.LastName+", "+ LoanApplication.Applicant.FirstName + 
                                                     (LoanApplication.CoApplicationFlag?" & "+ LoanApplication.CoApplicant.LastName + ", " + LoanApplication.CoApplicant.FirstName:"") +
                                                     "_IATrust_Loan_" + DateTime.Now.ToString("yyMMdd_hhmmss") + ".pdf");

            pdfReader = new PdfReader(SourePDFfolder + @"\iA Trust\iATrust_a.pdf");
            pdfStamper = new PdfStamper(pdfReader, new FileStream(iATrustappFileName, FileMode.Create));
            pdfFormFields = pdfStamper.AcroFields;

            pdfFormFields.SetField("New application", "On");

            //Part 1
            if (!string.IsNullOrEmpty(NPCode))
            {
                pdfFormFields.SetField("NP_Number", NPCode);
            }


            //B - Borrower           
            pdfFormFields.SetField("Firstname", LoanApplication.Applicant.FirstName.ToUpper());
            pdfFormFields.SetField("Lastname", LoanApplication.Applicant.LastName.ToUpper());
            bool idfilled = false;
            foreach (ID id in LoanApplication.Applicant.PersonIDs)
            {
                switch (id.IdType.ToUpper())
                {
                    case "SIN":
                        pdfFormFields.SetField("SIN", id.IdNumber);
                        idfilled = true;
                        break;
                }
                if (idfilled)
                {
                    break;
                }
            }
                       
            pdfFormFields.SetField("DOB", LoanApplication.Applicant.DateofBirth.ToString("yyyyMMdd"));
            pdfFormFields.SetField("Female", (LoanApplication.Applicant.Gender.ToUpper()=="FEMALE") ? "On" : "");
            pdfFormFields.SetField("Male", (LoanApplication.Applicant.Gender.ToUpper() == "MALE") ? "On" : "");
            pdfFormFields.SetField("English", "On");
            pdfFormFields.SetField("H_Tel", (LoanApplication.Applicant.Homephone != null) ? LoanApplication.Applicant.Homephone : (LoanApplication.Applicant.Cellphone != null) ? LoanApplication.Applicant.Cellphone : "");              
            pdfFormFields.SetField("C_Email", LoanApplication.Applicant.Email);
          
            string address = "";
            foreach (Address add in LoanApplication.Applicant.PersonAddress)
            {
                if (add.CurrentFlag)
                {
                    if (add.StreetNo != null)
                        address = add.StreetNo;
                    if (add.StreetName != null)
                        address = string.IsNullOrEmpty(address) ? add.StreetName.ToUpper() : address + ' ' + add.StreetName.ToUpper();
                    pdfFormFields.SetField("HomeAddress", address);
                    pdfFormFields.SetField("H_AptNo", add.AptNo);
                    pdfFormFields.SetField("H_City", add.City);
                    pdfFormFields.SetField("H_Province", add.Province);
                    pdfFormFields.SetField("H_Postalcode", add.Postcode);
                }
            }

            string pob = "";
            if (LoanApplication.Applicant.CountryofBirth != null)
            {
                pob = LoanApplication.Applicant.CountryofBirth.ToUpper();
            }
            if (LoanApplication.Applicant.ProvinceofBirth != null)
            {
                pob = string.IsNullOrEmpty(pob) ? LoanApplication.Applicant.ProvinceofBirth.ToUpper() : pob + "&" + LoanApplication.Applicant.ProvinceofBirth;
            }
            pdfFormFields.SetField("POB", pob);
            pdfFormFields.SetField("SinceCA",LoanApplication.Applicant.LiveCAsince.ToString("yyyyMM"));


            //C-Current Employment
            pdfFormFields.SetField("C_Companyname", LoanApplication.Applicant.PersonEmployment[0].Employer.ToUpper());
            pdfFormFields.SetField("C_Position", LoanApplication.Applicant.PersonEmployment[0].Occupation.ToUpper());
            pdfFormFields.SetField("SinceEM", DateTime.FromOADate(Int32.Parse(LoanApplication.Applicant.PersonEmployment[0].StartDate)).ToString("yyyyMMdd"));
            pdfFormFields.SetField("TOB", LoanApplication.Applicant.PersonEmployment[0].Industry.ToUpper());

            string em_address = "";
            if (LoanApplication.Applicant.PersonEmployment[0].StNo != null)
            {
                em_address = LoanApplication.Applicant.PersonEmployment[0].StNo;
            }
            if (LoanApplication.Applicant.PersonEmployment[0].StName != null)
            {
                em_address = string.IsNullOrEmpty(em_address) ? LoanApplication.Applicant.PersonEmployment[0].StName.ToUpper() : em_address + " " + LoanApplication.Applicant.PersonEmployment[0].StName.ToUpper();
            }
            pdfFormFields.SetField("C_AddressEM", em_address);

            pdfFormFields.SetField("C_AptNo", LoanApplication.Applicant.PersonEmployment[0].Unit);
            pdfFormFields.SetField("C_CityEM", LoanApplication.Applicant.PersonEmployment[0].City);
            pdfFormFields.SetField("C_ProvinceEM", LoanApplication.Applicant.PersonEmployment[0].Prov);
            pdfFormFields.SetField("C_PostcodeEM", LoanApplication.Applicant.PersonEmployment[0].PostCode);

            //C-Previous Employment
            pdfFormFields.SetField("P_employer", LoanApplication.Applicant.PersonEmployment[1].Employer);
            pdfFormFields.SetField("P_Position", LoanApplication.Applicant.PersonEmployment[1].Occupation);

            DateTime dt_from = DateTime.FromOADate(Int32.Parse(LoanApplication.Applicant.PersonEmployment[1].StartDate));
            DateTime dt_to = DateTime.FromOADate(Int32.Parse(LoanApplication.Applicant.PersonEmployment[1].EndDate));            
            int y = ((dt_to.Year - dt_from.Year) * 12 + dt_to.Month - dt_from.Month + 6) / 12; //+6表示四舍五入
            pdfFormFields.SetField("P_NOY", y.ToString());


            //D – Life Insurance Agent Information
            //if (!ViewModelBase.AsSecondAdvisor)
            //{

            //AgentInfo agentI = ViewModelBase.Advisor;//new AgentInfo();
            pdfFormFields.SetField("AgencyCode", LoanApplication.Advisor1.AgencyCode /*ViewModelBase.Advisor.Agency*/);
            pdfFormFields.SetField("Agency", LoanApplication.Advisor1.AgencyName /*ViewModelBase.Advisor.AgencyName*/);
            //if (string.IsNullOrEmpty(ViewModelBase.Advisor.SalesRepCode) || string.IsNullOrEmpty(ViewModelBase.Advisor.DealerCode))
            //{
                pdfFormFields.SetField("AgentCode", LoanApplication.Advisor1.AdvisorCode_iA /*ViewModelBase.Advisor.AdvisorCode_iA*/);
                pdfFormFields.SetField("SU", LoanApplication.Advisor1.AdvisorSU_iA /*ViewModelBase.Advisor.AdvisorSU_iA*/);
            //}
            //else
            //{
            //    pdfFormFields.SetField("B45t", ViewModelBase.Advisor.SalesRepCode);
            //    pdfFormFields.SetField("B46t", ViewModelBase.Advisor.DealerCode);
            //}
            pdfFormFields.SetField("NOA", LoanApplication.Advisor1.AdvisorName /*ViewModelBase.Advisor.AgentName*/);
            pdfFormFields.SetField("A_Email", LoanApplication.Advisor1.AdvisorEmail /*ViewModelBase.Advisor.AgentEmail*/);
            pdfFormFields.SetField("A_Telephone", LoanApplication.Advisor1.AdvisorTelephone /*ViewModelBase.Advisor.AgentTelephone*/);
            //}

            //Part 2
            //A - Analysis of Your Financial Situation
            pdfFormFields.SetField("No1", "On");
            pdfFormFields.SetField("No2", "On");
            pdfFormFields.SetField("No3", "On");
            pdfFormFields.SetField("No4", "On");


            //B-INCOME
            //xlWorksheet = readWorkbook.Sheets["Assets & Liabilities"];
            //xlRange = xlWorksheet.UsedRange;

            double gross_inc = 0.0;
            double other_inc = 0.0;

            foreach (Income inc in LoanApplication.Applicant.PersonIncome)
            {
                switch (inc.IncomeType.ToUpper())
                {
                    case "EMPLOYMENT":
                    case "SELF-EMPLOYMENT":
                        gross_inc += double.Parse(inc.IncomeAmount);
                        break;
                    case "RENTAL":
                    case "DIVIDEND":
                    case "BONUS":
                        other_inc += double.Parse(inc.IncomeAmount);
                        break;
                }
            }
            pdfFormFields.SetField("GMEI", Math.Round(gross_inc/12, 2).ToString("#,##0.#0")); /*Gross monthly employment income*/
            pdfFormFields.SetField("EMI", Math.Round(other_inc, 2).ToString("#,##0.#0")); /*Other monthly income*/
            pdfFormFields.SetField("TMI", Math.Round(gross_inc + other_inc, 2).ToString("#,##0.#0")); /*Total monthly income*/
            pdfFormFields.SetField("35Value", Math.Round((gross_inc + other_inc) * 0.35, 2).ToString("#,##0.#0")); /*%35 Total monthly income*/


            //C-BALANCE SHEET
            //ASSETS
            double TFSASum = 0.0;
            double RRSPSum = 0.0;
            double GRRSPSum = 0.0;
            double SRRSPSum = 0.0;
            double RESPSum = 0.0;
            double MFSum = 0.0;
            double GICSum = 0.0;
            double StocksSum = 0.0;
            double BankSum = 0.0;           
            double OtherSum = 0.0;
            double RegisteredSum = 0.0;
            double NonRegSum = 0.0;
            double InvestSum = 0.0;
            double PrincipalSum = 0.0;
            double OtherPropSum = 0.0;
            double VehicleSum = 0.0;

            string TFSAStr = "TFSA";
            string RRSPStr = "RRSP";
            string GRRSPStr = "GROUP RRSP";
            string SRRSPStr = "SPOUSAL RRSP";
            string RESPStr = "RESP";
            string MFStr = "MUTUAL FUND";
            string GICStr = "GIC";
            string StocksStr = "STOCK";
            string BankStr = "BANK ACCOUNT";            
            string InvestStr = "NON-REGISTERED ";
            string OtherStr = "OTHER";
            string OtherPropStr = "OTHER PROPERTY";

            foreach (Asset asset in LoanApplication.Applicant.PersonAsset)
            {
                switch (asset.AssetsType.ToUpper())
                {
                    case "TFSA":
                        TFSASum += double.Parse(asset.MarketValue);
                        RegisteredSum += double.Parse(asset.MarketValue);
                        break;
                    case "RRSP":
                        RRSPSum += double.Parse(asset.MarketValue);
                        RegisteredSum += double.Parse(asset.MarketValue);
                        break;
                    case "GROUP RRSP":
                        GRRSPSum += double.Parse(asset.MarketValue);
                        RegisteredSum += double.Parse(asset.MarketValue);
                        break;
                    case "SPOUSAL RRSP":
                        SRRSPSum += double.Parse(asset.MarketValue);
                        RegisteredSum += double.Parse(asset.MarketValue);
                        break;
                    case "RESP":
                        RESPSum += double.Parse(asset.MarketValue);
                        RegisteredSum += double.Parse(asset.MarketValue);
                        break;
                    case "MUTUAL FUNDS":
                        MFSum += double.Parse(asset.MarketValue);
                        NonRegSum += double.Parse(asset.MarketValue);
                        break;
                    case "GIC":
                        GICSum += double.Parse(asset.MarketValue);
                        NonRegSum += double.Parse(asset.MarketValue);
                        break;
                    case "STOCKS":
                        StocksSum += double.Parse(asset.MarketValue);
                        NonRegSum += double.Parse(asset.MarketValue);
                        break;
                    case "CHECKING ACCOUNT":
                    case "SAVING ACCOUNT":
                        BankSum += double.Parse(asset.MarketValue);
                        NonRegSum += double.Parse(asset.MarketValue);
                        break;
                    case "OTHER ASSETS":
                        OtherSum += double.Parse(asset.MarketValue);
                        NonRegSum += double.Parse(asset.MarketValue);
                        break;
                    case "INVESTMENT":
                        InvestSum += double.Parse(asset.MarketValue);
                        break;
                    case "PRINCIPAL":
                        PrincipalSum += double.Parse(asset.MarketValue);
                        break;
                    case "OTHER PROPERTY":
                        OtherPropSum += double.Parse(asset.MarketValue);
                        break;
                    case "VEHICLE":
                        VehicleSum += double.Parse(asset.MarketValue);
                        break;              
                }
            }
            pdfFormFields.SetField("MarketValue", PrincipalSum.ToString("#,##0.#0"));
            pdfFormFields.SetField("V_MV", VehicleSum.ToString("#,##0.#0"));


            int i = 1;
            if (TFSASum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), TFSAStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(TFSASum, 2).ToString("#,##0.#0"));
                i++;
            }
            if (RRSPSum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), RRSPStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(RRSPSum, 2).ToString("#,##0.#0"));
                i++;
            }
            if (GRRSPSum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), GRRSPStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(GRRSPSum, 2).ToString("#,##0.#0"));
                i++;
            }
            if (SRRSPSum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), SRRSPStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(SRRSPSum, 2).ToString("#,##0.#0"));
                i++;
            }
            if (RESPSum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), RESPStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(RESPSum, 2).ToString("#,##0.#0"));
                i++;
            }
            if (MFSum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), MFStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(MFSum, 2).ToString("#,##0.#0"));
                i++;
            }
            if (GICSum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), GICStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(GICSum, 2).ToString("#,##0.#0"));
                i++;
            }
            if (StocksSum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), StocksStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(StocksSum, 2).ToString("#,##0.#0"));
                i++;
            }
            if (BankSum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), BankStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(BankSum, 2).ToString("#,##0.#0"));
                i++;
            }           
            if (InvestSum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), InvestStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(InvestSum, 2).ToString("#,##0.#0"));
                i++;
            }
            if (OtherSum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), OtherStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(OtherSum, 2).ToString("#,##0.#0"));
                i++;
            }
            if (OtherPropSum > 0.00)
            {
                pdfFormFields.SetField("OS" + i.ToString(), OtherPropStr);
                pdfFormFields.SetField("V_OS" + i.ToString(), Math.Round(OtherPropSum, 2).ToString("#,##0.#0"));
                i++;
            }
            pdfFormFields.SetField("V_TotalAssets", Math.Round(TFSASum + RRSPSum + GRRSPSum + SRRSPSum + MFSum + GICSum + StocksSum + BankSum + OtherSum + InvestSum + PrincipalSum + OtherPropSum + VehicleSum, 2).ToString("#,##0.#0"));

            
            //Liabilities
            double MrgeBal = 0.0; 
            double MrgeMon = 0.0;
            double PLBal = 0.0;
            double PLMonBal = 0.0;
            double InvMonBal = 0.0;
            double LCBal = 0.0;
            double LCMonBal = 0.0;
            double VehicleBal = 0.0;
            double VehicleMonBal = 0.0;
            double OtherBal = 0.0;
            double OtherMonBal = 0.0;

            foreach (Liability lb in LoanApplication.Applicant.PersonLiability)
            {
                switch (lb.LiType.ToUpper())
                {
                    case "PRINCIPAL":
                    case "OTHER PROPERTY":
                        MrgeBal += double.Parse(lb.LiBalance);
                        MrgeMon += double.Parse(lb.LiMonthlyPayt);                        
                        break;
                    case "PERSONAL LOAN":
                    case "STUDENT LOAN":                    
                        PLBal += double.Parse(lb.LiBalance);
                        PLMonBal += double.Parse(lb.LiMonthlyPayt);                        
                        break;
                    case "INVESTMENT LOAN":
                        InvMonBal += double.Parse(lb.LiMonthlyPayt);
                        break;
                    case "LINE OF CREDIT":
                        LCBal += double.Parse(lb.LiBalance);
                        LCMonBal += double.Parse(lb.LiMonthlyPayt);                        
                        break;
                    case "VEHICLE":
                        VehicleBal += double.Parse(lb.LiBalance);
                        VehicleMonBal += double.Parse(lb.LiMonthlyPayt);
                        break;
                    case "RENT":
                    case "PROPERTY TAX":
                    case "CONDO FEE":
                    case "OTHER DEBTS":
                        OtherBal += double.Parse(lb.LiBalance);
                        OtherMonBal += double.Parse(lb.LiMonthlyPayt);
                        break;
                }
            }

            pdfFormFields.SetField("M_Balance", Math.Round(MrgeBal, 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("M_MP", Math.Round(MrgeMon, 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("PL_Balance", Math.Round(PLBal, 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("PLMP", Math.Round(PLMonBal + InvMonBal, 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("LOC_Balance", Math.Round(LCBal, 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("LOCMP", Math.Round(LCMonBal, 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("V_Balance", Math.Round(VehicleBal, 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("VMP", Math.Round(VehicleMonBal, 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("OFO_Balance", Math.Round(OtherBal, 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("OBOMP", Math.Round(OtherMonBal, 2).ToString("#,##0.#0"));

            pdfFormFields.SetField("TotalLiabilities", Math.Round(MrgeMon + PLMonBal + InvMonBal + LCMonBal + VehicleMonBal + OtherMonBal, 2).ToString("#,##0.#0"));
            pdfFormFields.SetField("TDSR", Math.Round(double.Parse(pdfFormFields.GetField("35Value")) - double.Parse(pdfFormFields.GetField("TotalLiabilities")), 2).ToString("#,##0.#0"));


            //D - Type of Loan
            pdfFormFields.SetField("Interest only each payment includes payment toward interest only", "On");
            pdfFormFields.SetField("100 investment loan WITHOUT margin call no cash investment required from the Borrower", "On");


            //Part 3
            //A - Information Box
            //xlWorksheet = readWorkbook.Sheets["Account Info"];
            //xlRange = xlWorksheet.UsedRange;

            pdfFormFields.SetField("LoanAmount", LoanApplication.SourceLoan.ApplyAmount);
            pdfFormFields.SetField("ELB", "0");
            pdfFormFields.SetField("TPA", Math.Round(double.Parse(pdfFormFields.GetField("LoanAmount")) + double.Parse(pdfFormFields.GetField("ELB")), 2).ToString("#,##0.#0"));

            pdfFormFields.SetField("PrimeRate", "2.45");
            pdfFormFields.SetField("IncrementRate", "0.75");
            pdfFormFields.SetField("AIR", "3.20");


            //Part 4
            pdfFormFields.SetField("Investment funds", "On");
            pdfFormFields.SetField("IF", "100");
            pdfFormFields.SetField("or_3", "On");


            //Part 5
            pdfFormFields.SetField("Transit", LoanApplication.SourceLoan.Paymentcheque.TransitNo /*fillln.ChequeTransit*/);
            pdfFormFields.SetField("Institution", LoanApplication.SourceLoan.Paymentcheque.InstitutionNo /*fillln.ChequeInstitution*/);
            pdfFormFields.SetField("Account", LoanApplication.SourceLoan.Paymentcheque.AccountNo /*fillln.ChequeAccount*/);
            pdfFormFields.SetField("NOAO", pdfFormFields.GetField("Firstname") + " " + pdfFormFields.GetField("Lastname"));


            pdfStamper.FormFlattening = false; //False-PDF可更改   //True-PDF不可更改
            pdfStamper.Close();
            pdfReader.Close();
            MessageBox.Show("IATrust Loan Application was done!");
            //ErrMessage = string.IsNullOrEmpty(ErrMessage) ? "iA Trust Loan application Fihished!" : ErrMessage + " \nFinished!";
        }


        private void AutoFill_CL_LoanInvest(Investment CLInvestApplication, string SourePDFfolder, string outputdirectory, string NPCode = "")
        {
            if (CLInvestApplication.SourceLoan == null && CLInvestApplication.SourceTF == null)
            {
                ErrMessage = string.IsNullOrEmpty(ErrMessage) ? "No CL Loan application!" : ErrMessage + " \nNo CL Loan application!";
                return;
            }
            string iAappFileName = Path.Combine(outputdirectory,
                                                CLInvestApplication.Applicant.LastName + ", " + CLInvestApplication.Applicant.FirstName +
                                                (CLInvestApplication.CoApplicationFlag ? " & " + CLInvestApplication.CoApplicant.LastName + ", " + CLInvestApplication.CoApplicant.FirstName : "") +
                                                "_CL_LoanInvestment_" + DateTime.Now.ToString("yyMMdd_hhmmss") + ".pdf");

            PdfReader pdfReader = new PdfReader(SourePDFfolder + @"\CL\Canada_Life.pdf");
            PdfReader.unethicalreading = true;
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(iAappFileName, FileMode.Create));
            AcroFields pdfFormFields = pdfStamper.AcroFields;


            // Agent Info
            //AgentInfo agentI = ViewModelBase.Advisor;
            pdfFormFields.SetField("advisor.repCd", CLInvestApplication.Advisor1.AdvisorCode_CL /*ViewModelBase.Advisor.AdvisorCode_CL*/); 

            pdfFormFields.SetField("existing TA form", "No");
            pdfFormFields.SetField("_.Does the applicants have an existing Transaction authorization form", "2");
            pdfFormFields.SetField("_third_party_involvement_que", "2");


            //1-Applicant
            //PERSONAL INFO
            //Excel._Worksheet xlWorksheet = readWorkbook.Sheets["Personal Info"];
            //Excel.Range xlRange = xlWorksheet.UsedRange;

            pdfFormFields.SetField("_segfund_annuitant_question", "1");
            if (CLInvestApplication.Applicant.Gender.ToUpper() == "FEMALE")
            {
                pdfFormFields.SetField("order.targetClient.gender.cd", "2");
            }
            if (CLInvestApplication.Applicant.Gender.ToUpper() == "MALE")
            {
                pdfFormFields.SetField("order.targetClient.gender.cd", "1");
            }

            string app_name = "";
            if (CLInvestApplication.Applicant.FirstName != null)
            {
                app_name = CLInvestApplication.Applicant.FirstName.ToUpper();             
            }
            if (CLInvestApplication.Applicant.LastName != null)
            {
                app_name = string.IsNullOrEmpty(app_name) ? CLInvestApplication.Applicant.LastName.ToUpper() : (app_name + " " + CLInvestApplication.Applicant.LastName);
            }
            pdfFormFields.SetField("client.name.fullName", app_name);

            string app_address = "";
            if (CLInvestApplication.Applicant.PersonAddress[0].AptNo != null)
            {
                app_address = CLInvestApplication.Applicant.PersonAddress[0].AptNo;
            }
            if (CLInvestApplication.Applicant.PersonAddress[0].StreetNo != null)
            {
                app_address = string.IsNullOrEmpty(app_address) ? CLInvestApplication.Applicant.PersonAddress[0].StreetNo : (app_address + "-" + CLInvestApplication.Applicant.PersonAddress[0].StreetNo);
            }
            if (CLInvestApplication.Applicant.PersonAddress[0].StreetName != null)
            {
                app_address = string.IsNullOrEmpty(app_address) ? CLInvestApplication.Applicant.PersonAddress[0].StreetName.ToUpper() : (app_address + " " + CLInvestApplication.Applicant.PersonAddress[0].StreetName);
            }
            pdfFormFields.SetField("client.address[1].streetAddressWithoutSuite", app_address);
            pdfFormFields.SetField("client.address[1].city", CLInvestApplication.Applicant.PersonAddress[0].City.ToUpper());
            pdfFormFields.SetField("client.address[1].province.desc", CLInvestApplication.Applicant.PersonAddress[0].Province);
            pdfFormFields.SetField("client.address[1].postalCode", CLInvestApplication.Applicant.PersonAddress[0].Postcode);
            pdfFormFields.SetField("client.phone[20].number", (CLInvestApplication.Applicant.Homephone != null) ? CLInvestApplication.Applicant.Homephone : (CLInvestApplication.Applicant.Cellphone != null) ? CLInvestApplication.Applicant.Cellphone : "");

            bool idfilled = false;
            foreach (ID id in CLInvestApplication.Applicant.PersonIDs)
            {
                switch (id.IdType.ToUpper())
                {
                    case "SIN":
                        pdfFormFields.SetField("client.sin", id.IdNumber);
                        idfilled = true;
                        break;
                }
                if (idfilled)
                {
                    break;
                }
            }
            
            string app_birthday = "";
            if (CLInvestApplication.Applicant.DobDay != null) /*Birthay_Day*/
            {
                app_birthday = CLInvestApplication.Applicant.DobDay;
            }
            if (CLInvestApplication.Applicant.DobMonth != null) /*Birthday_Month*/
            {
                app_birthday += "/" + CLInvestApplication.Applicant.DobMonth.ToUpper();
            }
            if (CLInvestApplication.Applicant.DobYear != null) /*Birthday_Yea*/
            {
                app_birthday += "/" + CLInvestApplication.Applicant.DobYear;
            }
            pdfFormFields.SetField("client.birthDate[ddMMyyyy]", app_birthday);

            pdfFormFields.SetField("_purpose_of_plan_long_term", "1");
            pdfFormFields.SetField("_source_of_funds_other", "1");
            pdfFormFields.SetField("_source_of_funds_detail_info", "NATIONAL BANK");
   
            pdfFormFields.SetField("client.kyc.career.desc", CLInvestApplication.Applicant.PersonEmployment[0].Occupation.ToUpper());
            pdfFormFields.SetField("_kyc_employment_question_responsibilities", CLInvestApplication.Applicant.PersonEmployment[0].Industry.ToUpper());
            pdfFormFields.SetField("client.kyc.businessType.desc", CLInvestApplication.Applicant.PersonEmployment[0].Industry.ToUpper());
            pdfFormFields.SetField("client.kyc.employerName", CLInvestApplication.Applicant.PersonEmployment[0].Employer.ToUpper());


            idfilled = false;
            foreach (ID id in CLInvestApplication.Applicant.PersonIDs)
            {
                switch (id.IdType.ToUpper())
                {
                    case "PROVINCIAL DRIVER'S LICENSE":
                        pdfFormFields.SetField("client.kyc.idVerification[1].cd", "1");
                        idfilled = true;
                        break;
                    case "PASSPORT":
                        pdfFormFields.SetField("client.kyc.idVerification[1].cd", "2");
                        idfilled = true;
                        break;
                    case "PROVINCIAL PHOTO ID":
                    case "PR CARD":
                    case "HEALTH CARD":
                        pdfFormFields.SetField("client.kyc.idVerification[1].cd", "3");
                        pdfFormFields.SetField("_kyc_id_type_specify", id.IdType.ToUpper());
                        idfilled = true;
                        break;
                }
                if (idfilled)
                {
                    break;
                }

                pdfFormFields.SetField("client.kyc.idVerification[1].number", id.IdNumber);
                pdfFormFields.SetField("client.kyc.idVerification[1].provinceOfIssue.desc", id.IssueProvince.ToUpper());
                string issueDate = "";
                if (id.IssueDate != null)
                {
                    issueDate += DateTime.FromOADate(Int32.Parse(id.IssueDate)).ToString("dd");
                    issueDate += "/" + DateTime.FromOADate(Int32.Parse(id.IssueDate)).ToString("MM");
                    issueDate += "/" + DateTime.FromOADate(Int32.Parse(id.IssueDate)).ToString("yy");
                }
                pdfFormFields.SetField("client.kyc.idVerification[1].issueDate[ddMMyyyy]", issueDate);

                string expiryDate = "";
                if (id.ExpiryDate != null)
                {
                    expiryDate += DateTime.FromOADate(Int32.Parse(id.ExpiryDate)).ToString("dd");
                    expiryDate += "/" + DateTime.FromOADate(Int32.Parse(id.ExpiryDate)).ToString("MM");
                    expiryDate += "/" + DateTime.FromOADate(Int32.Parse(id.ExpiryDate)).ToString("yy");
                }
                pdfFormFields.SetField("client.kyc.idVerification[1].expiryDate[ddMMyyyy]", expiryDate);
            }

            pdfFormFields.SetField("_tax_us_citizen", "2");
            pdfFormFields.SetField("_tax_us_can_citizen_other", "2");


            //Is the Nation Bank Loan > $100,000
            double fillamount = 0.0;
            if (CLInvestApplication.SourceLoan != null)
            {
                fillamount = double.Parse(CLInvestApplication.SourceLoan.ApplyAmount);
            }
            else
            {
                fillamount = double.Parse(CLInvestApplication.SourceTF.TransferAmount);
            }

            if (fillamount > 100000)
            {
                pdfFormFields.SetField("_pep_premium_applied", "1");
            }
            else
            {
                pdfFormFields.SetField("_pep_premium_applied", "2");
            }


            //Co-applicant Info
            if (CLInvestApplication.CoApplicationFlag != false)
            {
                pdfFormFields.SetField("_joint_segfund_annuitant_question", "1");
            }
            else
            {
                pdfFormFields.SetField("_joint_policyowner", "2");
            }
            
            if (CLInvestApplication.CoApplicant.Gender != null)
            {
                switch (CLInvestApplication.CoApplicant.Gender.ToUpper())
                {
                    case "FEMALE":
                        pdfFormFields.SetField("_joint_contact_gender", "2");
                        break;
                    case "MALE":
                        pdfFormFields.SetField("_joint_contact_gender", "1");
                        break;
                }
            }

            string coapp_name = "";            
            if (CLInvestApplication.CoApplicant.FirstName != null)
            {
                coapp_name = CLInvestApplication.CoApplicant.FirstName.ToUpper();
            }
            if (CLInvestApplication.CoApplicant.LastName != null)
            {
                coapp_name = string.IsNullOrEmpty(coapp_name) ? CLInvestApplication.CoApplicant.LastName.ToUpper() : (coapp_name + " " + CLInvestApplication.CoApplicant.LastName.ToUpper());
            }
            pdfFormFields.SetField("client.coOwner[J][1].name.fullName", coapp_name);

            if (CLInvestApplication.CoApplicant.MaritalStatus != null)
            {
                switch (CLInvestApplication.CoApplicant.MaritalStatus.ToUpper())
                {
                    case "COMMON LAW":
                        pdfFormFields.SetField("client.coOwner[J][1].relation.desc", "COMMON LAW");
                        break;
                    case "MARRIED":
                        pdfFormFields.SetField("client.coOwner[J][1].relation.desc", "MARRIED");
                        break;
                }
            }

            string coapp_address = "";
            if (CLInvestApplication.CoApplicant.PersonAddress[0].AptNo != null)
            {
                coapp_address = CLInvestApplication.CoApplicant.PersonAddress[0].AptNo;
            }
            if (CLInvestApplication.CoApplicant.PersonAddress[0].StreetNo != null)
            {
                coapp_address = string.IsNullOrEmpty(coapp_address) ? CLInvestApplication.CoApplicant.PersonAddress[0].StreetNo : (coapp_address + "-" + CLInvestApplication.CoApplicant.PersonAddress[0].StreetNo);
            }
            if (CLInvestApplication.CoApplicant.PersonAddress[0].StreetName != null)
            {
                coapp_address = string.IsNullOrEmpty(coapp_address) ? CLInvestApplication.CoApplicant.PersonAddress[0].StreetName.ToUpper() : (coapp_address + " " + CLInvestApplication.CoApplicant.PersonAddress[0].StreetName);
            }
            pdfFormFields.SetField("_same_address_as_owner", (coapp_address == app_address) ? "1" : "");
            pdfFormFields.SetField("client.coOwner[J][1].address[1].city", CLInvestApplication.CoApplicant.PersonAddress[0].City.ToUpper());
            pdfFormFields.SetField("client.coOwner[J][1].address[1].province.desc", CLInvestApplication.CoApplicant.PersonAddress[0].Province.ToUpper());
            pdfFormFields.SetField("client.coOwner[J][1].address[1].postalCode", CLInvestApplication.CoApplicant.PersonAddress[0].Postcode.ToUpper());

            idfilled = false;
            foreach (ID id in CLInvestApplication.CoApplicant.PersonIDs)
            {
                switch (id.IdType.ToUpper())
                {
                    case "SIN":
                        pdfFormFields.SetField("client.coOwner[J][1].sin", id.IdNumber);
                        idfilled = true;
                        break;
                }
                if (idfilled)
                {
                    break;
                }
            }

            pdfFormFields.SetField("client.coOwner[J][1].phone[96].number", (CLInvestApplication.CoApplicant.Homephone != null) ? CLInvestApplication.CoApplicant.Homephone : (CLInvestApplication.CoApplicant.Cellphone != null) ? CLInvestApplication.CoApplicant.Cellphone : "");

            string coapp_birthday = "";
            if (CLInvestApplication.CoApplicant.DobDay != null)
            {
                coapp_birthday = CLInvestApplication.CoApplicant.DobDay;
            }
            if (CLInvestApplication.CoApplicant.DobMonth != null)
            {
                coapp_birthday += "/" + CLInvestApplication.CoApplicant.DobMonth;
            }
            if (CLInvestApplication.CoApplicant.DobYear != null)
            {
                coapp_birthday += "/" + CLInvestApplication.CoApplicant.DobYear;
            }
            pdfFormFields.SetField("client.birthDate[ddMMyyyy]", coapp_birthday);

            pdfFormFields.SetField("joint.client.kyc.career.desc", CLInvestApplication.CoApplicant.PersonEmployment[0].Occupation.ToUpper());
            pdfFormFields.SetField("_joint_kyc_employment_question_responsibilities", CLInvestApplication.CoApplicant.PersonEmployment[0].Occupation.ToUpper());
            pdfFormFields.SetField("_joint_kyc_employment_question_responsibilities", CLInvestApplication.CoApplicant.PersonEmployment[0].Employer.ToUpper());
            pdfFormFields.SetField("joint.client.kyc.businessType.desc", CLInvestApplication.CoApplicant.PersonEmployment[0].Industry.ToUpper());

            idfilled = false;
            foreach (ID id in CLInvestApplication.CoApplicant.PersonIDs)
            {
                switch (id.IdType.ToUpper())
                {
                    case "PROVINCIAL DRIVER'S LICENSE":
                        pdfFormFields.SetField("joint.client.kyc.idVerification[1].cd", "1");
                        idfilled = true;
                        break;
                    case "PASSPORT":
                        pdfFormFields.SetField("joint.client.kyc.idVerification[1].cd", "2");
                        idfilled = true;
                        break;
                    case "PROVINCIAL PHOTO ID":
                    case "PR CARD":
                    case "HEALTH CARD":
                        pdfFormFields.SetField("joint.client.kyc.idVerification[1].cd", "3");
                        pdfFormFields.SetField("_joint_kyc_id_type_specify", id.IdType.ToUpper());
                        idfilled = true;
                        break;
                }
                if (idfilled)
                {
                    break;
                }

                pdfFormFields.SetField("joint.client.kyc.idVerification[1].number", id.IdNumber);
                pdfFormFields.SetField("joint.client.kyc.idVerification[1].provinceOfIssue.desc", id.IssueProvince.ToUpper());
                string issueDate = "";
                if (id.IssueDate != null)
                {
                    issueDate += DateTime.FromOADate(Int32.Parse(id.IssueDate)).ToString("dd");
                    issueDate += "/" + DateTime.FromOADate(Int32.Parse(id.IssueDate)).ToString("MM");
                    issueDate += "/" + DateTime.FromOADate(Int32.Parse(id.IssueDate)).ToString("yy");
                }
                pdfFormFields.SetField("joint.client.kyc.idVerification[1].issueDate[ddMMyyyy]", issueDate);
                string expiryDate = "";
                if (id.ExpiryDate != null)
                {
                    expiryDate += DateTime.FromOADate(Int32.Parse(id.ExpiryDate)).ToString("dd");
                    expiryDate += "/" + DateTime.FromOADate(Int32.Parse(id.ExpiryDate)).ToString("MM");
                    expiryDate += "/" + DateTime.FromOADate(Int32.Parse(id.ExpiryDate)).ToString("yy");
                }
                pdfFormFields.SetField("joint.client.kyc.idVerification[1].expiryDate[ddMMyyyy]", expiryDate);
            }

            pdfFormFields.SetField("_joint_tax_us_citizen", "2");
            pdfFormFields.SetField("_joint_tax_us_can_citizen_other", "2");
            
            //3-What guarantee level do you want?
            pdfFormFields.SetField("_segfund_guarantee_level", "1"); //75%

            //8-How are you paying for this policy?
            pdfFormFields.SetField("_deposit_lumpsum_method_seg", "3");


            int count = 0;            
            foreach (Beneficiary bnf in CLInvestApplication.InvestmentBeneficiary)
            {
                if (bnf.BnfType.ToUpper() == "BENEFICIARY")
                {
                    if (bnf.BnfFirstName != null || bnf.BnfLastName != null)
                    {
                        count++;
                    }
                    if (count > 0 && count < 2)
                    {
                        pdfFormFields.SetField("_beneficiary_full_name1", (CLInvestApplication.InvestmentBeneficiary[0].BnfFirstName + " " + CLInvestApplication.InvestmentBeneficiary[0].BnfLastName).ToUpper());
                        pdfFormFields.SetField("_beneficiary_revocable1", "1");
                        pdfFormFields.SetField("_beneficiary_revocable1", "1");
                        pdfFormFields.SetField("_beneficiary_relationship1", CLInvestApplication.InvestmentBeneficiary[0].BnfRelationship.ToUpper());
                        pdfFormFields.SetField("_beneficiary_percentage1", ((double.Parse(CLInvestApplication.InvestmentBeneficiary[0].BnfPercentage) * 100) + "%").ToString());
                    }
                    else if (count == 2)
                    {
                        pdfFormFields.SetField("_beneficiary_full_name2", (CLInvestApplication.InvestmentBeneficiary[1].BnfFirstName + " " + CLInvestApplication.InvestmentBeneficiary[1].BnfLastName).ToUpper());
                        pdfFormFields.SetField("_beneficiary_revocable2", "1");
                        pdfFormFields.SetField("_beneficiary_revocable2", "1");
                        pdfFormFields.SetField("_beneficiary_relationship2", CLInvestApplication.InvestmentBeneficiary[1].BnfRelationship.ToUpper());
                        pdfFormFields.SetField("_beneficiary_percentage2", ((double.Parse(CLInvestApplication.InvestmentBeneficiary[1].BnfPercentage) * 100) + "%").ToString());
                    }
                    else
                    {
                        pdfFormFields.SetField("_beneficiary_full_name1", "SEE ATTACHMENT");
                    }
                }
            }
        
            //8
            pdfFormFields.SetField("_deposit_lumpsum_amount", Math.Round(fillamount, 0).ToString()); //"fillln.ApplyAmount");
            
            //11-Third-party determination
            pdfFormFields.SetField("_third_party_involvement_que123456", "2");


            pdfStamper.FormFlattening = false; //False-PDF可更改   //True-PDF不可更改
            pdfStamper.Close();
            pdfReader.Close();
            MessageBox.Show("Canada Life Loan Investment Done!");
            ErrMessage = string.IsNullOrEmpty(ErrMessage) ? "Canada Life Loan Investment Fihished!" : ErrMessage + " \nCanada Life Loan Investment Finished!";
        }

       
//        private void AutoFill_CL_Seg_Registered(Investment CLInvestApplication, string SourePDFfolder, string outputdirectory, string NPCode = "")
//        {
//            if (CLInvestApplication.SourceLoan == null && CLInvestApplication.SourceTF == null)
//                {
//                    ErrMessage = string.IsNullOrEmpty(ErrMessage) ? "No CL Loan application!" : ErrMessage + " \nNo CL Loan application!";
//                    return;
//                }
//                //string iAappFileName = DefaultOutFolder + "\\CL_Reg_" + DateTime.Now.ToString("yyMMdd_hhmmss") + ".pdf";
//                string iAappFileName = Path.Combine(outputdirectory,
//                                                CLInvestApplication.Applicant.LastName + ", " + CLInvestApplication.Applicant.FirstName +
//                                                (CLInvestApplication.CoApplicationFlag ? " & " + CLInvestApplication.CoApplicant.LastName + ", " + CLInvestApplication.CoApplicant.FirstName : "") +
//                                                "_CL_RegInvestment_" + DateTime.Now.ToString("yyMMdd_hhmmss") + ".pdf");


//            PdfReader pdfReader = new PdfReader(SourePDFfolder + @"\CL_Seg_Registered.pdf");
//            PdfReader.unethicalreading = true;
//            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(iAappFileName, FileMode.Create));
//            AcroFields pdfFormFields = pdfStamper.AcroFields;


//            //Agent Info
//            //AgentInfo agentI = ViewModelBase.Advisor;
//            pdfFormFields.SetField("advisor.repCd", CLInvestApplication.Advisor1.AdvisorCode_iA /*ViewModelBase.Advisor.AdvisorCode_iA*/);
//            pdfFormFields.SetField("advisor.name.fullName", CLInvestApplication.Advisor1.AdvisorName.ToUpper() /*ViewModelBase.Advisor.AgentName*/);
//            pdfFormFields.SetField("advisor.phone[40].bracketNumberSlashExt", CLInvestApplication.Advisor1.AdvisorTelephone /*ViewModelBase.Advisor.AgentTelephone*/);
//            pdfFormFields.SetField("_advisor_centre_number", "SCIO");

//            //Does the applicant(s) have an existing Transaction authorization form?
//            pdfFormFields.SetField("_.Does the applicants have an existing Transaction authorization form", "Yes");


//            //1. Information about the Policyowner / Annuitant
//            //Excel._Worksheet xlWorksheet = readWorkbook.Sheets["Personal Info"];
//            //Excel.Range xlRange = xlWorksheet.UsedRange;

//            if (CLInvestApplication.Applicant.Gender != null)
//            {
//                switch (CLInvestApplication.Applicant.Gender.ToUpper())
//                {
//                    case "FEMALE":
//                        pdfFormFields.SetField("order.targetClient.gender.cd", "female");
//                        break;
//                    case "MALE":
//                        pdfFormFields.SetField("order.targetClient.gender.cd", "male");
//                        break;
//                }
//            }
//            pdfFormFields.SetField("client.name.fullName1", CLInvestApplication.Applicant.FirstName.ToUpper());
//            pdfFormFields.SetField("client.name.fullName3", CLInvestApplication.Applicant.LastName.ToUpper());

//            bool idfilled = false;
//            foreach (ID id in CLInvestApplication.Applicant.PersonIDs)
//            {
//                switch (id.IdType.ToUpper())
//                {
//                    case "SIN":
//                        pdfFormFields.SetField("client.sin", id.IdNumber);
//                        break;
//                }
//                if (idfilled)
//                {
//                    break;
//                }
//            }

//            string app_bd = "";
//            if (CLInvestApplication.Applicant.DobDay != null)
//            {
//                app_bd = CLInvestApplication.Applicant.DobDay;
//            }
//            if (CLInvestApplication.Applicant.DobMonth != null)
//            {
//                app_bd += "/" + CLInvestApplication.Applicant.DobMonth;
//            }
//            if (CLInvestApplication.Applicant.DobYear != null)
//            {
//                app_bd += "/" + CLInvestApplication.Applicant.DobYear;
//            }
//            pdfFormFields.SetField("client.birthDate[ddMMyyyy]", app_bd);

//            string app_address = "";
//            if (CLInvestApplication.Applicant.PersonAddress[0].AptNo != null)
//            {
//                app_address = CLInvestApplication.Applicant.PersonAddress[0].AptNo;
//            }
//            if (CLInvestApplication.Applicant.PersonAddress[0].StreetNo != null)
//            {
//                app_address = string.IsNullOrEmpty(app_address) ? CLInvestApplication.Applicant.PersonAddress[0].StreetNo : (app_address + "-" + CLInvestApplication.Applicant.PersonAddress[0].StreetNo);
//            }
//            if (CLInvestApplication.Applicant.PersonAddress[0].StreetName != null)
//            {
//                app_address = string.IsNullOrEmpty(app_address) ? CLInvestApplication.Applicant.PersonAddress[0].StreetName : (app_address + " " + CLInvestApplication.Applicant.PersonAddress[0].StreetName.ToUpper());
//            }
//            pdfFormFields.SetField("client.address[1].streetAddressWithoutSuite", app_address);
//            pdfFormFields.SetField("client.address[1].city", CLInvestApplication.Applicant.PersonAddress[0].City.ToUpper());
//            pdfFormFields.SetField("client.address[1].province.desc", CLInvestApplication.Applicant.PersonAddress[0].Province.ToUpper());
//            pdfFormFields.SetField("client.address[1].postalCode", CLInvestApplication.Applicant.PersonAddress[0].Postcode.ToUpper());

//            pdfFormFields.SetField("client.phone[20].number", CLInvestApplication.Applicant.Cellphone /*(xlRange.Range["F16"] != null && xlRange.Range["F16"].Value2 != null) ? Int64.Parse(xlRange.Range["F16"].Value2.ToString()).ToString("###-#######") : ""*/);

//            //2. Policy type
//            pdfFormFields.SetField("account[1].type.cd", "1");


//            //3. Spousal contribution information            
//            if (CLInvestApplication.CoApplicationFlag != false)
//            {
//                pdfFormFields.SetField("client.kyc.spouse.name.fullName1", CLInvestApplication.CoApplicant.FirstName.ToUpper());
//                pdfFormFields.SetField("client.kyc.spouse.name.fullName3", CLInvestApplication.CoApplicant.LastName.ToUpper());

//                idfilled = false;
//                foreach (ID id in CLInvestApplication.CoApplicant.PersonIDs)
//                {
//                    if (id.IdType.ToUpper() == "SIN")
//                    {
//                        pdfFormFields.SetField("account[1].spouse.sin", id.IdNumber);
//                    }
//                    if (idfilled)
//                    {
//                        break;
//                    }
//                }

//                string co_bd = "";
//                if (CLInvestApplication.CoApplicant.DobDay != null)
//                {
//                    co_bd = CLInvestApplication.CoApplicant.DobDay;
//                }
//                if (CLInvestApplication.CoApplicant.DobMonth != null)
//                {
//                    co_bd += "/" + CLInvestApplication.CoApplicant.DobMonth;
//                }
//                if (CLInvestApplication.CoApplicant.DobYear != null)
//                {
//                    co_bd += "/" + CLInvestApplication.CoApplicant.DobYear;
//                }
//                pdfFormFields.SetField("account[1].spouse.birthDate[ddMMyyyy]", co_bd);
//            }


//            //4. What guarantee level do you want?
//            pdfFormFields.SetField("_segfund_guarantee_level", "1");


//            //9. How are you paying for this policy
//            if (CLInvestApplication.SourceOF != null)
//            {
//                //Math.Round(xyz * 0.35, 2).ToString("#,##0.#0")
//                pdfFormFields.SetField("_deposit_lumpsum_amount", Math.Round(double.Parse(CLInvestApplication.SourceOF.ApplyAmount), 2).ToString("#,##0.#0"));
//                pdfFormFields.SetField("_deposit_lumpsum_method_seg", "1");
//            }
//            else if (CLInvestApplication.SourceTF != null)
//            {
//                pdfFormFields.SetField("_deposit_lumpsum_amount", Math.Round(double.Parse(CLInvestApplication.SourceTF.TransferAmount), 2).ToString("#,##0.#0"));
//                pdfFormFields.SetField("_deposit_lumpsum_method_seg", "4");
//                pdfFormFields.SetField("_.From_2", CLInvestApplication.SourceTF.TransferFromAccountInfo.InstitutionName);
//                pdfFormFields.SetField("_payment_for_premium_method_transfer_from_address",
//                                        CLInvestApplication.SourceTF.TransferFromAccountInfo.InstitutionAddressApart+", "+
//                                        CLInvestApplication.SourceTF.TransferFromAccountInfo.InstitutionAddressNo+" "+
//                                        CLInvestApplication.SourceTF.TransferFromAccountInfo.InstitutionAddressStreet+", "+
//                                        CLInvestApplication.SourceTF.TransferFromAccountInfo.InstitutionAddressCity+", "+
//                                        CLInvestApplication.SourceTF.TransferFromAccountInfo.InstitutionAddressProvince+", "+
//                                        CLInvestApplication.SourceTF.TransferFromAccountInfo.InstitutionAddressPostcode );
//            }



//            int count = 0;
//            int i = 0;
//            foreach (Beneficiary bnf in CLInvestApplication.InvestmentBeneficiary)
//            {
//                if (bnf.BnfType.ToUpper() == "BENEFICIARY")
//                {
//                    if (bnf.BnfFirstName != null || bnf.BnfLastName != null)
//                    {
//                        count++;
//                    }
//                    if (count <= 2)
//                    {
//                        string bnf0_name = "";
//                        if (CLInvestApplication.InvestmentBeneficiary[0].BnfFirstName != null)
//                        {
//                            bnf0_name = CLInvestApplication.InvestmentBeneficiary[0].BnfFirstName.ToUpper();
//                        }
//                        if (CLInvestApplication.InvestmentBeneficiary[0].BnfLastName != null)
//                        {
//                            bnf0_name = string.IsNullOrEmpty(bnf0_name) ? CLInvestApplication.InvestmentBeneficiary[0].BnfLastName.ToUpper() : (bnf0_name + " " + CLInvestApplication.InvestmentBeneficiary[0].BnfLastName.ToUpper());
//                        }
//                        pdfFormFields.SetField("_beneficiary_full_name1", bnf0_name);
//                        pdfFormFields.SetField("_beneficiary_revocable1", string.IsNullOrEmpty(bnf0_name) ? "" : "1");
//                        pdfFormFields.SetField("_beneficiary_relationship1", CLInvestApplication.InvestmentBeneficiary[0].BnfRelationship);
//                        pdfFormFields.SetField("_beneficiary_percentage1", (double.Parse(CLInvestApplication.InvestmentBeneficiary[0].BnfPercentage) * 100).ToString());


//                        string bnf1_name = "";
//                        if (CLInvestApplication.InvestmentBeneficiary[1].BnfFirstName != null)
//                        {
//                            bnf1_name = CLInvestApplication.InvestmentBeneficiary[1].BnfFirstName.ToUpper();
//                        }
//                        if (CLInvestApplication.InvestmentBeneficiary[1].BnfLastName != null)
//                        {
//                            bnf1_name = string.IsNullOrEmpty(bnf1_name) ? CLInvestApplication.InvestmentBeneficiary[1].BnfLastName.ToUpper() : (bnf1_name + " " + CLInvestApplication.InvestmentBeneficiary[1].BnfLastName.ToUpper());
//                        }
//                        pdfFormFields.SetField("_beneficiary_full_name2", bnf1_name);
//                        pdfFormFields.SetField("_beneficiary_revocable2", string.IsNullOrEmpty(bnf1_name) ? "" : "1");
//                        pdfFormFields.SetField("_beneficiary_relationship2", CLInvestApplication.InvestmentBeneficiary[1].BnfRelationship);
//                        pdfFormFields.SetField("_beneficiary_percentage2", (double.Parse(CLInvestApplication.InvestmentBeneficiary[1].BnfPercentage) * 100).ToString());
//                    }
//                    else
//                    {
//                        pdfFormFields.SetField("_beneficiary_full_name1", "SEE <11. Special instructions>");

//                        //11. Special instructions
//                        i = 0;
//                        string bnf_info = "";
//                        while (i < count)
//                        {
//                            if (CLInvestApplication.InvestmentBeneficiary[i].BnfFirstName != null)
//                            {
//                                bnf_info += "Name : " + CLInvestApplication.InvestmentBeneficiary[i].BnfFirstName.ToUpper();
//                            }
//                            if (CLInvestApplication.InvestmentBeneficiary[i].BnfLastName != null)
//                            {
//                                bnf_info = string.IsNullOrEmpty(bnf_info) ? CLInvestApplication.InvestmentBeneficiary[i].BnfLastName.ToUpper() : (bnf_info + " " + CLInvestApplication.InvestmentBeneficiary[i].BnfLastName.ToUpper());
//                            }
//                            if (CLInvestApplication.InvestmentBeneficiary[i].BnfRelationship != null)
//                            {
//                                bnf_info = bnf_info + ", Relationship to annuitant : " + CLInvestApplication.InvestmentBeneficiary[i].BnfRelationship;
//                                bnf_info = bnf_info + ", Revocable : YES";
//                            }
//                            if (CLInvestApplication.InvestmentBeneficiary[i].BnfPercentage != null)
//                            {
//                                bnf_info = bnf_info + ", Percent allocated: " + ((double.Parse(CLInvestApplication.InvestmentBeneficiary[i].BnfPercentage) * 100) + "%").ToString();
//                            }
//                            bnf_info += "\n";
//                            i++;
//                        }
//                        pdfFormFields.SetField("_special_instructions", bnf_info);
//                    }
//                }


//                //D. Trustee for beneficiary
//                foreach (Beneficiary tr in CLInvestApplication.InvestmentBeneficiary)
//                {
//                    if (tr.TrusteeFirstName != null || tr.TrusteeLastName != null)
//                    {
//                        pdfFormFields.SetField("_segfund_beneficiary_trustee_full_name1", tr.TrusteeFirstName.ToUpper());
//                        pdfFormFields.SetField("_segfund_beneficiary_trustee_full_name3", tr.TrusteeLastName.ToUpper());
//                        pdfFormFields.SetField("_segfund_beneficiary_trustee_relationship", tr.TrRelationship.ToUpper());
//                    }
//                }
                

//                pdfStamper.FormFlattening = false; //False-PDF可更改   //True-PDF不可更改
//                pdfStamper.Close();
//                pdfReader.Close();
//                MessageBox.Show("Canada life Seg_registered application was Done!");
//                ErrMessage = string.IsNullOrEmpty(ErrMessage) ? "Fihished!" : ErrMessage + " \nFinished!";
//            }
//        }

//        private void AutoFill_iA_RRSP_NonReg_Loan(Investment CLInvestApplication, string SourePDFfolder, string outputdirectory, string NPCode = "")
//        {
//            if (fillln != null && (fillof != null || filltf1 != null || filltf2 != null))
//            {
//                ErrMessage = string.IsNullOrEmpty(ErrMessage) ? "Loan application should be made individually!" : ErrMessage + " \nLoan application should be made individually!";
//                return;
//            }
//            Investment tempApp = new Investment();
//            bool isLoan = false;
//            if (fillln != null)
//            {
//                tempApp = fillln;
//                isLoan = true;
//            }
//            else if (fillof != null)
//            {
//                tempApp = fillof;
//            }
//            else if (filltf1 != null) //tranfer form----F51_147A_Transfer_Authorization_for_Registered_and_Non_registered.pdf
//            {
//                tempApp = filltf1;
//            }
//            else if (filltf2 != null)
//            {
//                tempApp = filltf2;
//            }
//            //tempPDFfile = DefaultOutFolder + "\\" + applicantName + @"\temppdf.pdf";
//            string tempPDFfile= Path.Combine(outputdirectory,
//                                            CLInvestApplication.Applicant.LastName + ", " + CLInvestApplication.Applicant.FirstName +
//                                            (CLInvestApplication.CoApplicationFlag ? " & " + CLInvestApplication.CoApplicant.LastName + ", " + CLInvestApplication.CoApplicant.FirstName : "") +
//                                            "_IA_INVEST_" + DateTime.Now.ToString("yyMMdd_hhmmss") + ".pdf");
//                string pdfTemplate = SourePDFfolder + @"\iA\NEW\F17A - Investment Application - IAG Savings and Retirement Plan.pdf"; //@"C:\Users\Jade\Documents\FillPDF\PDFSolutions\PDFSolutions\Files\iA_IAG.pdf";                                         
//            prepare_iA_NPcodeFile(pdfTemplate, tempPDFfile);

//            PdfReader.unethicalreading = true;
//            PdfReader pdfReader = new PdfReader(tempPDFfile);
//            //string applicationFile = DefaultOutFolder + "\\" + applicantName + "\\" + applicantName + "_IA_APP_" + tempApp.AccountType.ToUpper() + ".pdf";
//string applicationFile = Path.Combine(outputdirectory,
//                                            CLInvestApplication.Applicant.LastName + ", " + CLInvestApplication.Applicant.FirstName +
//                                            (CLInvestApplication.CoApplicationFlag ? " & " + CLInvestApplication.CoApplicant.LastName + ", " + CLInvestApplication.CoApplicant.FirstName : "") +
//                                            "_IA_INVEST_" + DateTime.Now.ToString("yyMMdd_hhmmss") + ".pdf");


//                if (File.Exists(applicationFile))
//            {
//                File.Move(applicationFile, applicationFile.Substring(0, applicationFile.Length - 4) + "_" + DateTime.Now.ToString("yyMMdd_hhmmss") + ".pdf");
//            }
//            pdfStamper = new PdfStamper(pdfReader, new FileStream(applicationFile, FileMode.Create));
//            pdfFormFields = pdfStamper.AcroFields;
//            string NP_Code = pdfFormFields.GetField("H_Proposition");
//            ErrMessage += "\n" + tempApp.AccountType.ToUpper() + " application NP Code: " + pdfFormFields.GetField("H_Proposition");
//            //1-Type of Contract
//            if (isLoan)//(fillln != null)
//            {
//                if (fillln.SourceLoan.LoanFrom.ToUpper() == "B2B BANK")
//                {
//                    pdfFormFields.SetField("B03c", "");
//                    pdfFormFields.SetField("B06c", "On");
//                    pdfFormFields.SetField("B04t", fillln.SourceLoan.LoanFrom.ToUpper());
//                }
//                else
//                {
//                    pdfFormFields.SetField("B03c", "On");
//                    pdfFormFields.SetField("B06c", "");
//                }
//                pdfFormFields.SetField("F07c", "On");
//                pdfFormFields.SetField("F06t", fillln.SourceLoan.ApplyAmount);
//                pdfFormFields.SetField("F36t", "48600");
//                pdfFormFields.SetField("F40t", "47300");
//                pdfFormFields.SetField("F43t", "40080");
//                pdfFormFields.SetField("F47t", "45650");
//            }
//            else
//            {
//                pdfFormFields.SetField("B03c", "On");
//            }

//            //4-Life Insurance Agent
//            //AgentInfo agentI = ViewModelBase.Advisor;//new AgentInfo();
//            pdfFormFields.SetField("B43t", tempApp.Advisor1.Agency.ToUpper());
//            pdfFormFields.SetField("B44t", tempApp.Advisor1.AgencyName.ToUpper());

//            if (string.IsNullOrEmpty(tempApp.Advisor1.SalesRepCode) || string.IsNullOrEmpty(tempApp.Advisor1.DealerCode))
//            {
//                pdfFormFields.SetField("B48t", tempApp.Advisor1.AdvisorCode_iA);
//                pdfFormFields.SetField("B49t", tempApp.Advisor1.AdvisorSU_iA);
//            }
//            else
//            {
//                pdfFormFields.SetField("B45t", tempApp.Advisor1.SalesRepCode);
//                pdfFormFields.SetField("B46t", tempApp.Advisor1.DealerCode);
//            }
//            pdfFormFields.SetField("B47t", tempApp.Advisor1.AdvisorcommissionPercent);
//            pdfFormFields.SetField("B50t", tempApp.Advisor1.AdvisorName.ToUpper());
//            pdfFormFields.SetField("B51t", tempApp.Advisor1.AdvisorEmail.ToUpper());
//            pdfFormFields.SetField("B52t", tempApp.Advisor1.AdvisorTelephone);

//            if (!string.IsNullOrEmpty(tempApp.Advisor2.AdvisorCode_iA /*ViewModelBase.Advisor2nd.AdvisorCode_iA*/))
//            {
//                pdfFormFields.SetField("B47t", "70");
//                pdfFormFields.SetField("B55t", tempApp.Advisor2.AdvisorCode_iA /*ViewModelBase.Advisor2nd.AdvisorCode_iA*/);
//                pdfFormFields.SetField("B54t", "30");
//                if (!string.IsNullOrEmpty(tempApp.Advisor2.AdvisorName.ToUpper() /*ViewModelBase.Advisor2nd.AgentName*/))
//                {
//                    pdfFormFields.SetField("B57t", tempApp.Advisor2.AdvisorName.ToUpper() /*ViewModelBase.Advisor2nd.AgentName*/);
//                }
//            }

//            //3-Holder/Annuitant
//            //Excel._Worksheet xlWorksheet = readWorkbook.Sheets["Personal Info"];
//            //Excel.Range xlRange = xlWorksheet.UsedRange;

//            pdfFormFields.SetField("B21t", tempApp.Applicant.FirstName.ToUpper());
//            pdfFormFields.SetField("B22t", tempApp.Applicant.LastName.ToUpper());
//            foreach (ID id in tempApp.Applicant.PersonIDs)
//            {
//                if (id.IdType.ToUpper() == "SIN")
//                {
//                    pdfFormFields.SetField("B23t", id.IdNumber);
//                }
//            }

//            //string bd = (xlRange.Range["D8"] != null && xlRange.Range["D8"].Value2 != null) ? xlRange.Range["D8"].Value2.ToString() : "yyyy";
//            //bd += (xlRange.Range["F8"] != null && xlRange.Range["F8"].Value2 != null) ? convertMonth(xlRange.Range["F8"].Value2.ToString()) : "00";
//            //string tempday = (xlRange.Range["H8"] != null && xlRange.Range["H8"].Value2 != null) ? ("00" + xlRange.Range["H8"].Value2.ToString()) : "00";
//            //bd += tempday.Substring(tempday.Length - 2);
//            //pdfFormFields.SetField("B24t", bd);
//            string app_bd = (tempApp.Applicant.DobYear);
//            app_bd += tempApp.Applicant.DobMonth;
//            string bd_day = ("00" + tempApp.Applicant.DobDay);
//            app_bd += bd_day.Substring(bd_day.Length - 2);
//            pdfFormFields.SetField("B24t", app_bd);

//            pdfFormFields.SetField("B25c", (tempApp.Applicant.Gender.ToUpper() == "FEMALE") ? "On" : "");
//            pdfFormFields.SetField("B26c", (tempApp.Applicant.Gender.ToUpper() == "MALE") ? "On" : "");
//            pdfFormFields.SetField("B27c", /*English*/ "On");
//            pdfFormFields.SetField("B30t", (tempApp.Applicant.Homephone != null) ? tempApp.Applicant.Homephone : (tempApp.Applicant.Cellphone != null) ? tempApp.Applicant.Cellphone : "");
//            pdfFormFields.SetField("B29t", tempApp.Applicant.Email.ToUpper());

//            string app_add = "";
//            if (tempApp.Applicant.PersonAddress[0].StreetNo != null)
//            {
//                app_add = tempApp.Applicant.PersonAddress[0].StreetNo;
//            }
//            if (tempApp.Applicant.PersonAddress[0].StreetName != null)
//            {
//                app_add = string.IsNullOrEmpty(app_add) ? tempApp.Applicant.PersonAddress[0].StreetName.ToUpper() : (app_add + " " + tempApp.Applicant.PersonAddress[0].StreetName.ToUpper());
//            }
//            pdfFormFields.SetField("B34t", app_add);
//            pdfFormFields.SetField("B34bt", tempApp.Applicant.PersonAddress[0].AptNo);
//            pdfFormFields.SetField("B35t", tempApp.Applicant.PersonAddress[0].City.ToUpper());
//            pdfFormFields.SetField("B36t", tempApp.Applicant.PersonAddress[0].Province.ToUpper());
//            pdfFormFields.SetField("B37t", tempApp.Applicant.PersonAddress[0].Postcode.ToUpper());
//            //Employment
//            pdfFormFields.SetField("B38t", tempApp.Applicant.PersonEmployment[0].Occupation.ToUpper());
//            pdfFormFields.SetField("B38t", tempApp.Applicant.PersonEmployment[0].WorkPhone);



//            //Beneficiary
//            string[,] B_P = new string[5, 9]{{"C02t","C03t","C04t","C05t","D351c","D361c","D371c","D381c","C10t"},
//                                             {"C11t","C12t","C13t","C14t","D352c","D362c","D372c","D382c","C19t"},
//                                             {"C20t","C21t","C22t","C23t","D353c","D363c","D373c","D383c","C28t"},
//                                             {"C29t","C30t","C32t","C31t","D354c","D364c","D374c","D384c","C37t"},
//                                             {"C38t","C39t","C40t","C41t","D355c","D365c","D375c","D385c","C46t"}};
//            int count = 0;
//            int i = 0;
//            int j = 0;
//            foreach (Beneficiary bnf in tempApp.InvestmentBeneficiary)
//            {
//                if (bnf.BnfType.ToUpper() == "BENEFICIARY")
//                {
//                    if (bnf.BnfFirstName != null || bnf.BnfLastName != null)
//                    {
//                        j = 0;
//                        pdfFormFields.SetField(B_P[i, j], bnf.BnfFirstName.ToUpper()); j++;
//                        pdfFormFields.SetField(B_P[i, j], bnf.BnfLastName.ToUpper()); j++;
//                        pdfFormFields.SetField(B_P[i, j], DateTime.FromOADate(Int32.Parse(bnf.BnfBirthday)).ToString("yyyyMMdd")); j++;
//                        pdfFormFields.SetField(B_P[i, j], (double.Parse(bnf.BnfPercentage) * 100).ToString()); j++;
//                        pdfFormFields.SetField(B_P[i, j], bnf.Revokable == "1" ? "On" : ""); j++;
//                        pdfFormFields.SetField(B_P[i, j], bnf.Revokable == "0" ? "On" : ""); j++;
//                        pdfFormFields.SetField(B_P[i, j], bnf.BnfGender.ToUpper() == "FEMALE" ? "On" : ""); j++;
//                        pdfFormFields.SetField(B_P[i, j], bnf.Revokable == "MALE" ? "On" : ""); j++;
//                        pdfFormFields.SetField(B_P[i, j], bnf.BnfRelationship.ToUpper()); j++;
//                        count++;
//                        i++;
//                    }
//                }
//                if (count > 5)
//                {
//                    break;
//                }
//            }

//            //Cont.Beneficiary
//            string[,] CB_P = new string[2, 5] {{"C48t","D376c","D386c","C50t","C51t"},
//                                               {"C53t","D377c","D387c","C55t","C56t"}};

//            count = 0;
//            i = 0;
//            j = 0;
//            string cb_name = "";
//            foreach (Beneficiary bnf in tempApp.InvestmentBeneficiary)
//            {
//                if (bnf.BnfType.ToUpper() == "CONT. BENEFICIARY")
//                {
//                    if (bnf.BnfFirstName != null || bnf.BnfLastName != null)
//                    {
//                        j = 0;
//                        cb_name = string.IsNullOrEmpty(bnf.BnfFirstName) ? bnf.BnfFirstName.ToUpper() : "";
//                        cb_name = string.IsNullOrEmpty(cb_name) ? bnf.BnfLastName.ToUpper() : cb_name += bnf.BnfLastName.ToUpper();
//                        pdfFormFields.SetField(CB_P[i, j], cb_name); j++;
//                        pdfFormFields.SetField(CB_P[i, j], bnf.BnfGender.ToUpper() == "FEMALE" ? "On" : ""); j++;
//                        pdfFormFields.SetField(CB_P[i, j], bnf.Revokable == "MALE" ? "On" : ""); j++;
//                        pdfFormFields.SetField(CB_P[i, j], DateTime.FromOADate(Int32.Parse(bnf.BnfBirthday)).ToString("yyyyMMdd")); j++;
//                        pdfFormFields.SetField(CB_P[i, j], (double.Parse(bnf.BnfPercentage) * 100).ToString()); j++;
//                        count++;
//                        i++;
//                    }
//                }
//                if (count > 2)
//                {
//                    break;
//                }
//            }

//            //Trustee
//            string[,] T_P = new string[2, 4] {{"C57t","C58t","C59t","C60t"},
//                                              {"C61t","C62t","C63t","C64t"}};
//            count = 0;
//            i = 0;
//            j = 0;
//            string mb_name = "";
//            string t_name = "";
//            foreach (Beneficiary bnf in tempApp.InvestmentBeneficiary)
//            {
//                if (bnf.TrusteeFirstName != null || bnf.TrusteeLastName != null)
//                {
//                    j = 0;
//                    mb_name = string.IsNullOrEmpty(bnf.BnfFirstName) ? bnf.BnfFirstName.ToUpper() : "";
//                    mb_name = string.IsNullOrEmpty(mb_name) ? bnf.TrusteeLastName.ToUpper() : mb_name += bnf.TrusteeLastName.ToUpper();
//                    pdfFormFields.SetField(T_P[i, j], mb_name); j++;
//                    pdfFormFields.SetField(T_P[i, j], DateTime.FromOADate(Int32.Parse(bnf.BnfBirthday)).ToString("yyyyMMdd")); j++;
//                    t_name = string.IsNullOrEmpty(bnf.TrusteeFirstName) ? bnf.TrusteeFirstName.ToUpper() : "";
//                    t_name = string.IsNullOrEmpty(t_name) ? bnf.TrusteeLastName.ToUpper() : t_name += bnf.TrusteeLastName.ToUpper();
//                    pdfFormFields.SetField(T_P[i, j], t_name); j++;
//                    pdfFormFields.SetField(T_P[i, j], bnf.TrRelationship.ToUpper()); j++;
//                }
//            }


//            if (fillof != null)
//            {
//                //2-Type of Registration 
//                switch (fillof.AccountType.ToUpper())
//                {
//                    case "LIRA":
//                        pdfFormFields.SetField("B14c", "On");
//                        break;
//                    case "RRSP":
//                        pdfFormFields.SetField("B10c", "On");
//                        break;
//                    case "NON-REG": //8,9,10,11
//                        //pdfFormFields.SetField("B38t", /*Principal occupation or business*/(xlRange.Range["E34"] != null && xlRange.Range["E34"].Value2 != null) ? xlRange.Range["E34"].Value2.ToString() : ""); //Postal code
//                        //if (xlRange.Range["C34"] != null && xlRange.Range["C34"].Value2 != null && xlRange.Range["C34"].Value2.ToString() != "Other - Specify")
//                        //{
//                        //    pdfFormFields.SetField("B38t", /*Principal occupation*/ xlRange.Range["C34"].Value2.ToString());
//                        //}
//                        //else if (xlRange.Range["E34"] != null && xlRange.Range["E34"].Value2 != null)
//                        //{
//                        //    pdfFormFields.SetField("B38t", /*Principal occupation*/ xlRange.Range["E34"].Value2.ToString());
//                        //}
//                        //else if (xlRange.Range["C27"] != null && xlRange.Range["C27"].Value2 != null)
//                        //{
//                        //    pdfFormFields.SetField("B38t", /*Principal occupation*/ xlRange.Range["C27"].Value2.ToString());
//                        //}
//                        pdfFormFields.SetField("B38t", tempApp.Applicant.PersonEmployment[0].Occupation.ToUpper());

//                        pdfFormFields.SetField("B12c", "On");
//                        pdfFormFields.SetField("D01c", "On");
//                        pdfFormFields.SetField("D12c", "On");
//                        pdfFormFields.SetField("D16c", "On");
//                        pdfFormFields.SetField("D18c", "On");

//                        if (Int32.Parse(fillof.PlanAmount) >= 100000)
//                        {
//                            pdfFormFields.SetField("D21c", "On");
//                        }
//                        else
//                        {
//                            pdfFormFields.SetField("D20c", "On");
//                        }
//                        pdfFormFields.SetField("D22c", "On"); // Same Person

//                        string relationship = "";
//                        //if (xlRange.Range["D9"] != null && xlRange.Range["D9"].Value2 != null)
//                        //{
//                        //    if (xlRange.Range["D9"].Value2.ToString() == "Married")
//                        //    {
//                        //        relationship = "Spouse";
//                        //    }
//                        //    else if (xlRange.Range["D9"].Value2.ToString() == "Married")
//                        //    {
//                        //        relationship = "Common Law";
//                        //    }
//                        //}
//                        if (tempApp.Applicant.MaritalStatus.ToUpper() == "MARRIED")
//                        {
//                            relationship = "Spouse";
//                        }
//                        if (tempApp.Applicant.MaritalStatus.ToUpper() == "COMMON LAW")
//                        {
//                            relationship = "Common Law";
//                        }

//                        pdfFormFields.SetField("D24c", "On"); // individual
//                        //fill individual info

//                        //9a - Information about the Applicant
//                        //xlWorksheet = readWorkbook.Sheets["Personal Info"];
//                        //xlRange = xlWorksheet.UsedRange;

//                        pdfFormFields.SetField("D31t", tempApp.Applicant.FirstName.ToUpper());
//                        pdfFormFields.SetField("D32t", tempApp.Applicant.LastName.ToUpper());

//                        //if (xlRange.Range["K8"] != null && xlRange.Range["K8"].Value2 != null && isSINvalid(xlRange.Range["K8"].Value2.ToString().ToUpper()))
//                        //{
//                        //    pdfFormFields.SetField("D34t", /*SIN*/(xlRange.Range["K8"] != null && xlRange.Range["K8"].Value2 != null) ? xlRange.Range["K8"].Value2.ToString().ToUpper() : ""); // SIN
//                        //}

//                        //9b- Confirmation of Identity of Individual Applicant
//                        //pdfFormFields.SetField("D49t", /*ID Type: Driver's License*/(xlRange.Range["A18"] != null && xlRange.Range["A18"].Value2 != null) ? xlRange.Range["A18"].Value2.ToString().ToUpper() : "");
//                        //pdfFormFields.SetField("D50t", /*ID NUMBER*/(xlRange.Range["C18"] != null && xlRange.Range["C18"].Value2 != null) ? xlRange.Range["C18"].Value2.ToString().ToUpper() : "");
//                        //pdfFormFields.SetField("D51t", "CANADA");
//                        //pdfFormFields.SetField("D52t", /*Issue Province*/(xlRange.Range["D19"] != null && xlRange.Range["D19"].Value2 != null) ? xlRange.Range["D19"].Value2.ToString().ToUpper() : "");
//                        //pdfFormFields.SetField("D53t", /*Expiry Date*/(xlRange.Range["H18"] != null && xlRange.Range["H18"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["H18"].Value2.ToString())).ToString("yyyyMMdd") : "");
//                        //pdfFormFields.SetField("D54t", /*Verify Date*/(xlRange.Range["J18"] != null && xlRange.Range["J18"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["J18"].Value2.ToString())).ToString("yyyyMMdd") : "");

//                        bool idfilled = false;
//                        foreach (ID id in tempApp.Applicant.PersonIDs)
//                        {
//                            switch (id.IdType.ToUpper())
//                            {
//                                case "SIN":
//                                    pdfFormFields.SetField("D34t", id.IdNumber);
//                                    break;
//                                case "PROVINCIAL DRIVER'S LICENSE":
//                                case "PROVINCIAL PHOTO ID":
//                                case "PASSPORT":
//                                case "PR CARD":
//                                case "HEALTH CARD":
//                                    pdfFormFields.SetField("D49t", id.IdType.ToUpper());
//                                    pdfFormFields.SetField("D50t", id.IdNumber);
//                                    pdfFormFields.SetField("D51t", "CANADA");
//                                    pdfFormFields.SetField("D52t", id.IssueProvince.ToUpper());
//                                    pdfFormFields.SetField("D53t", DateTime.FromOADate(Int32.Parse(id.ExpiryDate)).ToString("yyyyMMdd"));
//                                    pdfFormFields.SetField("D54t", DateTime.FromOADate(Int32.Parse(id.IssueDate)).ToString("yyyyMMdd");
//                                    break;
//                            }
//                            if (idfilled)
//                            {
//                                break;
//                            }
//                        }

//                        //string bid = (xlRange.Range["D8"] != null && xlRange.Range["D8"].Value2 != null) ? xlRange.Range["D8"].Value2.ToString() : "yyyy";
//                        //bid += (xlRange.Range["F8"] != null && xlRange.Range["F8"].Value2 != null) ? convertMonth(xlRange.Range["F8"].Value2.ToString()) : "00";
//                        //string tempBday = (xlRange.Range["H8"] != null && xlRange.Range["H8"].Value2 != null) ? ("00" + xlRange.Range["H8"].Value2.ToString()) : "00";
//                        //bid += tempBday.Substring(tempBday.Length - 2);
//                        //pdfFormFields.SetField("D33t", /*Birthday*/bid);
//                        string bd = "";
//                        string bdday = (tempApp.Applicant.DobDay != null) ? ("00" + tempApp.Applicant.DobDay) : "00";
//                        if (tempApp.Applicant.DobYear != null)
//                        {
//                            bd = tempApp.Applicant.DobYear;
//                        }
//                        if (tempApp.Applicant.DobMonth != null)
//                        {
//                            bd = string.IsNullOrEmpty(bd) ? tempApp.Applicant.DobMonth : bd += tempApp.Applicant.DobMonth;
//                        }
//                        if (tempApp.Applicant.DobDay != null)
//                        {
//                            bd = string.IsNullOrEmpty(bd) ? bdday.Substring(("00" + tempApp.Applicant.DobDay).Length - 2) : bd += bdday.Substring(("00" + tempApp.Applicant.DobDay).Length - 2);
//                        }

//                        pdfFormFields.SetField("D35c", tempApp.Applicant.Gender.ToUpper() == "FEMALE" ? "On" : "");
//                        pdfFormFields.SetField("D36c", tempApp.Applicant.Gender.ToUpper() == "MALE" ? "On" : "");
//                        pdfFormFields.SetField("D37c", /*English*/ "On"); //pdfFormFields.SetField("B28c", /*French*/ "On");                        
//                        pdfFormFields.SetField("D39t", tempApp.Applicant.Email.ToUpper());
//                        pdfFormFields.SetField("D40t", tempApp.Applicant.Homephone);
//                        pdfFormFields.SetField("D43t", tempApp.Applicant.Cellphone);

//                        string app_address = "";
//                        if (tempApp.Applicant.PersonAddress[0].StreetNo != null)
//                        {
//                            app_address = tempApp.Applicant.PersonAddress[0].StreetNo;
//                        }
//                        if (tempApp.Applicant.PersonAddress[0].StreetName != null)
//                        {
//                            app_address = string.IsNullOrEmpty(app_address) ? tempApp.Applicant.PersonAddress[0].StreetName.ToUpper() : app_address += tempApp.Applicant.PersonAddress[0].StreetName.ToUpper();
//                        }
//                        pdfFormFields.SetField("D44t", app_address);
//                        pdfFormFields.SetField("B37bt", tempApp.Applicant.PersonAddress[0].AptNo);
//                        pdfFormFields.SetField("D45t", tempApp.Applicant.PersonAddress[0].City.ToUpper());
//                        pdfFormFields.SetField("D46t", tempApp.Applicant.PersonAddress[0].Province.ToUpper());
//                        pdfFormFields.SetField("D47t", tempApp.Applicant.PersonAddress[0].Postcode.ToUpper());

//                        //if (xlRange.Range["C34"] != null && xlRange.Range["C34"].Value2 != null && xlRange.Range["C34"].Value2.ToString() != "Other - Specify")
//                        //{
//                        //    pdfFormFields.SetField("D48t", /*Principal occupation*/ xlRange.Range["C34"].Value2.ToString());
//                        //}
//                        //else if (xlRange.Range["E34"] != null && xlRange.Range["E34"].Value2 != null)
//                        //{
//                        //    pdfFormFields.SetField("D48t", /*Principal occupation*/ xlRange.Range["E34"].Value2.ToString());
//                        //}
//                        //else if (xlRange.Range["C27"] != null && xlRange.Range["C27"].Value2 != null)
//                        //{
//                        //    pdfFormFields.SetField("D48t", /*Principal occupation*/ xlRange.Range["C27"].Value2.ToString());
//                        //}
//                        pdfFormFields.SetField("D48t", tempApp.Applicant.PersonEmployment[0].Occupation.ToUpper());

//                        //9c
//                        pdfFormFields.SetField("D55c", "On");
//                        pdfFormFields.SetField("D58c", "On");

//                        //Co-Applicant
//                        pdfFormFields.SetField("C65t", tempApp.CoApplicant.FirstName.ToUpper());
//                        pdfFormFields.SetField("C66t", tempApp.CoApplicant.LastName.ToUpper());

//                        string coapp_bd = "";
//                        string coapp_day = (tempApp.CoApplicant.DobDay != null) ? ("00" + tempApp.CoApplicant.DobDay) : "00";
//                        if (tempApp.CoApplicant.DobYear != null)
//                        {
//                            coapp_bd = tempApp.CoApplicant.DobYear;
//                        }
//                        if (tempApp.CoApplicant.DobMonth != null)
//                        {
//                            coapp_bd = string.IsNullOrEmpty(coapp_bd) ? tempApp.CoApplicant.DobMonth : coapp_bd += tempApp.CoApplicant.DobMonth;
//                        }
//                        if (tempApp.CoApplicant.DobDay != null)
//                        {
//                            coapp_bd = string.IsNullOrEmpty(coapp_bd) ? coapp_day.Substring(("00" + tempApp.CoApplicant.DobDay).Length - 2) : coapp_bd += coapp_day.Substring(("00" + tempApp.CoApplicant.DobDay).Length - 2);
//                        }
//                        pdfFormFields.SetField("C67t", coapp_bd);
//                        pdfFormFields.SetField("C68t", string.IsNullOrEmpty(relationship) ? relationship : "");
//                        //if (xlRange.Range["L6"] != null && xlRange.Range["L6"].Value2 != null && xlRange.Range["L6"].Value2.ToString().ToUpper() == "YES")
//                        //{
//                        //    xlWorksheet = readWorkbook.Sheets["Co Applicant Info"];
//                        //    xlRange = xlWorksheet.UsedRange;

//                        //    //7-Designation of a Successor Annuitant
//                        //    pdfFormFields.SetField("C65t",/*First Name*/(xlRange.Range["E7"] != null && xlRange.Range["E7"].Value2 != null) ? xlRange.Range["E7"].Value2.ToString().ToUpper() : ""); // First name
//                        //    pdfFormFields.SetField("C66t",/*Last Name*/(xlRange.Range["B7"] != null && xlRange.Range["B7"].Value2 != null) ? xlRange.Range["B7"].Value2.ToString().ToUpper() : ""); // Last Name
//                        //    bd = (xlRange.Range["D8"] != null && xlRange.Range["D8"].Value2 != null) ? xlRange.Range["D8"].Value2.ToString() : "yyyy";
//                        //    bd += (xlRange.Range["F8"] != null && xlRange.Range["F8"].Value2 != null) ? convertMonth(xlRange.Range["F8"].Value2.ToString()) : "00";
//                        //    tempday = (xlRange.Range["H8"] != null && xlRange.Range["H8"].Value2 != null) ? ("00" + xlRange.Range["H8"].Value2.ToString()) : "00";
//                        //    bd += tempday.Substring(tempday.Length - 2);
//                        //    pdfFormFields.SetField("C67t", /*Birthday*/bd);
//                        //    if (!string.IsNullOrEmpty(relationship))
//                        //    {
//                        //        pdfFormFields.SetField("C68t", relationship);
//                        //    }

//                        //    xlWorksheet = readWorkbook.Sheets["Personal Info"];
//                        //    xlRange = xlWorksheet.UsedRange;
//                        //}
//                        break;

//                    case "SPOUSAL RRSP":
//                        pdfFormFields.SetField("B11C", "On"); //2-Type of Registration: Spousal RSP
//                        //int i = 37;
//                        //while (i <= 41) //4-Spousal Contribution
//                        //{
//                        //    if (xlRange.Range["F" + i.ToString()] != null && xlRange.Range["F" + i.ToString()].Value2 != null && xlRange.Range["F" + i.ToString()].Value2.ToString() == "Spouse")
//                        //    {
//                        //        pdfFormFields.SetField("B39t", /*Spouse First Name*/(xlRange.Range["D" + i.ToString()] != null && xlRange.Range["D" + i.ToString()].Value2 != null) ? xlRange.Range["D" + i.ToString()].Value2.ToString() : "");
//                        //        pdfFormFields.SetField("B40t", /*Spouse Last Name*/(xlRange.Range["B" + i.ToString()] != null && xlRange.Range["B" + i.ToString()].Value2 != null) ? xlRange.Range["B" + i.ToString()].Value2.ToString() : "");
//                        //        pdfFormFields.SetField("B41t", /*Spouse SIN*/(xlRange.Range["M" + i.ToString()] != null && xlRange.Range["M" + i.ToString()].Value2 != null) ? xlRange.Range["M" + i.ToString()].Value2.ToString() : "");
//                        //        pdfFormFields.SetField("B42t", /*Spouse Birthday*/(xlRange.Range["J" + i.ToString()] != null && xlRange.Range["J" + i.ToString()].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["J" + i.ToString()].Value2.ToString())).ToString("yyyyMMdd") : "");
//                        //        break;
//                        //    }
//                        //    i++;
//                        //}
//                        foreach (Beneficiary bnf in tempApp.InvestmentBeneficiary)
//                        {
//                            if (bnf.BnfRelationship.ToUpper() == "SPOUSE")
//                            {
//                                pdfFormFields.SetField("B39t", bnf.BnfFirstName.ToUpper());
//                                pdfFormFields.SetField("B40t", bnf.BnfLastName.ToUpper());
//                                pdfFormFields.SetField("B41t", bnf.BnfSIN);
//                                pdfFormFields.SetField("B42t", DateTime.FromOADate(Int32.Parse(bnf.BnfBirthday)).ToString("yyyyMMdd"));
//                            }
//                        }
//                        break;
//                }


//                //8- Investment Instructions From Account Info (Own funds or Transfer)
//                if (fillof.PayMethod == "Void Cheque" || fillof.PayMethod == "Direct Deposit")
//                {
//                    pdfFormFields.SetField("F03c", "On");
//                    pdfFormFields.SetField("F02t", fillof.PlanAmount);
//                }
//                else if (fillof.PayMethod == "Personal Cheque")
//                {
//                    pdfFormFields.SetField("F05c", "On");
//                    pdfFormFields.SetField("F04t", fillof.PlanAmount);
//                }
//                //pdfFormFields.SetField("F36t", fillof.FundCode1);
//                //pdfFormFields.SetField("F40t", fillof.FundCode2);
//                //pdfFormFields.SetField("F43t", fillof.FundCode3);
//                //pdfFormFields.SetField("F47t", fillof.FundCode4);
//                //pdfFormFields.SetField("F51t", fillof.FundCode5);

//                //13-Pre-Authorized Debit (PAD)
//                pdfFormFields.SetField("I09c", "On");
//                if (fillof.PaymentType == "One-time PAD")  //13-----------Onetime PAD
//                {
//                    if (!string.IsNullOrEmpty(fillof.OnetimePADdate))
//                    {
//                        if (fillof.ApplyYear + convertMonth(fillof.ApplyMonth) + fillof.ApplyDay == DateTime.FromOADate(Int32.Parse(fillof.OnetimePADdate)).ToString("yyyyMMdd"))
//                        {
//                            pdfFormFields.SetField("I08c", "On");//Immediately
//                        }
//                        else
//                        {
//                            pdfFormFields.SetField("I07c", "On");
//                            pdfFormFields.SetField("I18t", DateTime.FromOADate(Int32.Parse(fillof.OnetimePADdate)).ToString("yyyyMMdd"));
//                        }
//                    }
//                    else
//                    {
//                        pdfFormFields.SetField("I08c", "On");//Immediately
//                    }
//                }

//                //20- Pre-Authorized Debit-PAD
//                if (fillof.PaymentType.ToUpper() == "ONE-TIME PAD")
//                {
//                    if (!string.IsNullOrEmpty(fillof.OnetimePADdate))
//                    {
//                        pdfFormFields.SetField("I07c", "On");
//                        pdfFormFields.SetField("I18t", DateTime.FromOADate(Int32.Parse(fillof.OnetimePADdate)).ToString("yyyyMMdd"));
//                    }
//                    else
//                    {
//                        pdfFormFields.SetField("I08c", "On");//Immediately
//                    }
//                }
//                else if (fillof.PaymentType.ToUpper() == "REGULAR PAD")  //13-----------Regular PAD
//                {
//                    switch (fillof.PADFrequency.ToUpper())
//                    {
//                        case "MONTHLY":
//                            pdfFormFields.SetField("I01c", "On");
//                            break;
//                        case "LAST DAY":
//                            pdfFormFields.SetField("I02c", "On");
//                            break;
//                        case "WEEKLY":
//                            pdfFormFields.SetField("I03c", "On");
//                            break;
//                        case "BI-WEEKLY":
//                            pdfFormFields.SetField("I04c", "On");
//                            break;
//                    }
//                    pdfFormFields.SetField("I05t", fillof.ApplyAmount);
//                    pdfFormFields.SetField("I06t", fillof.RegularPAD1stDate);
//                }
//                //13-----------Banking information
//                pdfFormFields.SetField("I11c", "On");
//                pdfFormFields.SetField("I32t", ("00000" + fillof.TransitNo).Substring(("00000" + fillof.TransitNo).Length - 5));
//                pdfFormFields.SetField("I33t", ("000" + fillof.InstitutionNo).Substring(("000" + fillof.InstitutionNo).Length - 3));
//                pdfFormFields.SetField("I34t", fillof.AccountNo);
//                pdfFormFields.SetField("I35t", fillof.AccountOwnerName);
//            }

//            if (filltf1 != null)//tranfer form----F51_147A_Transfer_Authorization_for_Registered_and_Non_registered.pdf
//            {
//                pdfFormFields.SetField("F11c", "On");
//                pdfFormFields.SetField("F12t", filltf1.RelinquishingInstitutionName);
//                pdfFormFields.SetField("F13t", filltf1.TransferAmount);
//                if (fillof == null)
//                {
//                    //pdfFormFields.SetField("F36t", filltf1.FundCode1);
//                    //pdfFormFields.SetField("F40t", filltf1.FundCode2);
//                    //pdfFormFields.SetField("F43t", filltf1.FundCode3);
//                    //pdfFormFields.SetField("F47t", filltf1.FundCode4);
//                    //pdfFormFields.SetField("F51t", filltf1.FundCode5);

//                    //2-Type of Registration 
//                    switch (filltf1.AccountType.ToUpper())
//                    {
//                        case "LIRA":
//                            pdfFormFields.SetField("B14c", "On");
//                            break;
//                        case "RRSP":
//                            pdfFormFields.SetField("B10c", "On");
//                            break;
//                        case "NON-REG":
//                            pdfFormFields.SetField("B12c", "On");
//                            pdfFormFields.SetField("D01c", "On");
//                            pdfFormFields.SetField("D12c", "On");
//                            pdfFormFields.SetField("D16c", "On");
//                            pdfFormFields.SetField("D18c", "On");
//                            if (Int32.Parse(filltf1.TransferAmount) >= 100000)
//                            {
//                                pdfFormFields.SetField("D21c", "On");
//                            }
//                            else
//                            {
//                                pdfFormFields.SetField("D20c", "On");
//                            }
//                            pdfFormFields.SetField("D22c", "On"); // same person
//                            pdfFormFields.SetField("D24c", "On"); // individual
//                                                                  ////fill individual info
//                                                                 //9a - Information about the Applicant
//                            /////////////////////////////////////////////////////////////////////////////////////////////
//                            //xlWorksheet = readWorkbook.Sheets["Personal Info"];
//                            //xlRange = xlWorksheet.UsedRange;

//                            pdfFormFields.SetField("D31t", /*First name*/ tempApp.Applicant.FirstName.ToUpper());
//                            pdfFormFields.SetField("D32t", /*Last Name*/ tempApp.Applicant.LastName.ToUpper());

//                            //if (xlRange.Range["K8"] != null && xlRange.Range["K8"].Value2 != null && isSINvalid(xlRange.Range["K8"].Value2.ToString().ToUpper()))
//                            //{
//                            //    pdfFormFields.SetField("D34t", /*SIN*/(xlRange.Range["K8"] != null && xlRange.Range["K8"].Value2 != null) ? xlRange.Range["K8"].Value2.ToString().ToUpper() : "");
//                            //}

//                            bool idfilled = false;
//                            foreach (ID id in tempApp.Applicant.PersonIDs)
//                            {
//                                if (id.IdType.ToUpper() == "SIN")
//                                {
//                                    pdfFormFields.SetField("D34t", id.IdNumber);
//                                    idfilled = true;
//                                }                                                               
//                                if (idfilled)
//                                {
//                                    break;
//                                }
//                            }
                            
//                            //string bid = (xlRange.Range["D8"] != null && xlRange.Range["D8"].Value2 != null) ? xlRange.Range["D8"].Value2.ToString() : "yyyy";
//                            //bid += (xlRange.Range["F8"] != null && xlRange.Range["F8"].Value2 != null) ? convertMonth(xlRange.Range["F8"].Value2.ToString()) : "00";
//                            //string tempBday = (xlRange.Range["H8"] != null && xlRange.Range["H8"].Value2 != null) ? ("00" + xlRange.Range["H8"].Value2.ToString()) : "00";
//                            //bid += tempBday.Substring(tempBday.Length - 2);
//                            //pdfFormFields.SetField("D33t", /*Birthday*/bid);
//                            string bd = tempApp.Applicant.DobYear;
//                            bd += tempApp.Applicant.DobMonth;
//                            string bdday = string.IsNullOrEmpty(tempApp.Applicant.DobDay) ? ("00" + tempApp.Applicant.DobDay) : "00";
//                            bd += bdday.Substring(bdday.Length - 2);

//                            pdfFormFields.SetField("D35c", (tempApp.Applicant.Gender.ToUpper() == "FEMALE") ? "On" : "");
//                            pdfFormFields.SetField("D36c", (tempApp.Applicant.Gender.ToUpper() == "MALE") ? "On" : "");
//                            pdfFormFields.SetField("D37c", /*English*/ "On"); //pdfFormFields.SetField("B28c", /*French*/ "On");
//                            //pdfFormFields.SetField("D40t", /*Home Phone*/(xlRange.Range["H16"] != null && xlRange.Range["H16"].Value2 != null) ? xlRange.Range["H16"].Value2.ToString().ToUpper().Replace("-", "") : (/*Cell Phone*/(xlRange.Range["F16"] != null && xlRange.Range["F16"].Value2 != null) ? xlRange.Range["F16"].Value2.ToString().ToUpper().Replace("-", "") : ""));
//                            pdfFormFields.SetField("D40t", string.IsNullOrEmpty(tempApp.Applicant.Homephone) ? tempApp.Applicant.Homephone : string.IsNullOrEmpty(tempApp.Applicant.Cellphone) ? tempApp.Applicant.Cellphone : "");
//                            pdfFormFields.SetField("D43t", string.IsNullOrEmpty(tempApp.Applicant.Cellphone) ? tempApp.Applicant.Cellphone : "");
//                            pdfFormFields.SetField("D39t", tempApp.Applicant.Email.ToUpper()); 
                            
//                            string add = "";
//                            if (tempApp.Applicant.PersonAddress[0].StreetNo != null)
//                            {
//                                add = tempApp.Applicant.PersonAddress[0].StreetNo;
//                            }
//                            if (tempApp.Applicant.PersonAddress[0].StreetName != null)
//                            {
//                                add = string.IsNullOrEmpty(add) ? tempApp.Applicant.PersonAddress[0].StreetName.ToUpper() : (add + " " + tempApp.Applicant.PersonAddress[0].StreetName.ToUpper());
//                            }
//                            pdfFormFields.SetField("D44t", add);
//                            pdfFormFields.SetField("B37bt", /*Apt. PO BOX*/ tempApp.Applicant.PersonAddress[0].AptNo);
//                            pdfFormFields.SetField("D45t", /*City*/ tempApp.Applicant.PersonAddress[0].City.ToUpper());
//                            pdfFormFields.SetField("D46t", /*Province*/  tempApp.Applicant.PersonAddress[0].Province.ToUpper());
//                            pdfFormFields.SetField("D47t", /*PostalCode*/ tempApp.Applicant.PersonAddress[0].Postcode);

//                            //if (xlRange.Range["C34"] != null && xlRange.Range["C34"].Value2 != null && xlRange.Range["C34"].Value2.ToString() != "Other - Specify")
//                            //{
//                            //    pdfFormFields.SetField("D48t", /*Principal occupation*/ xlRange.Range["C34"].Value2.ToString());
//                            //}
//                            //else if (xlRange.Range["E34"] != null && xlRange.Range["E34"].Value2 != null)
//                            //{
//                            //    pdfFormFields.SetField("D48t", /*Principal occupation*/ xlRange.Range["E34"].Value2.ToString());
//                            //}
//                            //else if (xlRange.Range["C27"] != null && xlRange.Range["C27"].Value2 != null)
//                            //{
//                            //    pdfFormFields.SetField("D48t", /*Principal occupation*/ xlRange.Range["C27"].Value2.ToString());
//                            //}
//                            pdfFormFields.SetField("D48t", tempApp.Applicant.PersonEmployment[0].Occupation.ToUpper());

//                            //9b- Confirmation of Identity of Individual Applicant
//                            //pdfFormFields.SetField("D49t", /*ID Type: Driver's License*/(xlRange.Range["A18"] != null && xlRange.Range["A18"].Value2 != null) ? xlRange.Range["A18"].Value2.ToString().ToUpper() : "");
//                            //pdfFormFields.SetField("D50t", /*ID NUMBER*/(xlRange.Range["C18"] != null && xlRange.Range["C18"].Value2 != null) ? xlRange.Range["C18"].Value2.ToString().ToUpper() : "");
//                            //pdfFormFields.SetField("D51t", "CANADA");
//                            //pdfFormFields.SetField("D52t", /*Issue pROVINCE*/(xlRange.Range["D19"] != null && xlRange.Range["D19"].Value2 != null) ? xlRange.Range["D19"].Value2.ToString().ToUpper() : "");
//                            //pdfFormFields.SetField("D53t", /*Expiry Date*/(xlRange.Range["H18"] != null && xlRange.Range["H18"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["H18"].Value2.ToString())).ToString("yyyyMMdd") : "");
//                            //pdfFormFields.SetField("D54t", /*Verify Date*/(xlRange.Range["J18"] != null && xlRange.Range["J18"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["J18"].Value2.ToString())).ToString("yyyyMMdd") : "");
//                            idfilled = false;
//                            foreach (ID id in tempApp.Applicant.PersonIDs)
//                            {
//                                if (id.IdType.ToUpper() == "PROVINCIAL DRIVER'S LICENSE" || id.IdType.ToUpper() == "PROVINCIAL PHOTO ID" || id.IdType.ToUpper() == "PR CARD" || id.IdType.ToUpper() == "HEALTH CARD")
//                                {
//                                    pdfFormFields.SetField("D49t", id.IdType.ToUpper());
//                                    pdfFormFields.SetField("D50t", id.IdNumber);
//                                    pdfFormFields.SetField("D51t", "CANADA");
//                                    pdfFormFields.SetField("D52t", id.IssueProvince.ToUpper());
//                                    pdfFormFields.SetField("D52t", id.ExpiryDate);
//                                    pdfFormFields.SetField("D54t", id.IssueDate);
//                                }
//                                else if (id.IdType.ToUpper() == "PASSPORT")
//                                {
//                                    pdfFormFields.SetField("D49t", id.IdType.ToUpper());
//                                    pdfFormFields.SetField("D50t", id.IdNumber);
//                                    pdfFormFields.SetField("D51t", "");
//                                    pdfFormFields.SetField("D52t", id.IssueProvince.ToUpper());
//                                    pdfFormFields.SetField("D52t", id.ExpiryDate);
//                                    pdfFormFields.SetField("D54t", id.IssueDate);
//                                }
//                                if (idfilled)
//                                {
//                                    break;
//                                }
//                            }

//                            //9c
//                            pdfFormFields.SetField("D55c", "On");
//                            pdfFormFields.SetField("D58c", "On");
//                            break;
//                        case "SPOUSAL RRSP":
//                            pdfFormFields.SetField("B11c", "On");
//                            //int i = 37;
//                            //while (i <= 41)
//                            //{
//                            //    if (xlRange.Range["F" + i.ToString()] != null && xlRange.Range["F" + i.ToString()].Value2 != null && xlRange.Range["F" + i.ToString()].Value2.ToString() == "Spouse")
//                            //    {
//                            //        pdfFormFields.SetField("B39t", /*Spouse First Name*/(xlRange.Range["D" + i.ToString()] != null && xlRange.Range["D" + i.ToString()].Value2 != null) ? xlRange.Range["D" + i.ToString()].Value2.ToString() : "");
//                            //        pdfFormFields.SetField("B40t", /*Spouse Last Name*/(xlRange.Range["B" + i.ToString()] != null && xlRange.Range["B" + i.ToString()].Value2 != null) ? xlRange.Range["B" + i.ToString()].Value2.ToString() : "");
//                            //        pdfFormFields.SetField("B41t", /*Spouse SIN*/(xlRange.Range["M" + i.ToString()] != null && xlRange.Range["M" + i.ToString()].Value2 != null) ? xlRange.Range["M" + i.ToString()].Value2.ToString() : "");
//                            //        pdfFormFields.SetField("B42t", /*Spouse Birthday*/(xlRange.Range["J" + i.ToString()] != null && xlRange.Range["J" + i.ToString()].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["J" + i.ToString()].Value2.ToString())).ToString("yyyyMMdd") : "");
//                            //        break;
//                            //    }
//                            //    i++;
//                            //}
//                            foreach (Beneficiary bnf in tempApp.InvestmentBeneficiary)
//                            {
//                                if (bnf.BnfRelationship.ToUpper() == "SPOUSE")
//                                {
//                                    pdfFormFields.SetField("B39t", bnf.BnfFirstName.ToUpper());
//                                    pdfFormFields.SetField("B40t", bnf.BnfLastName.ToUpper());
//                                    pdfFormFields.SetField("B41t", bnf.BnfSIN);
//                                    pdfFormFields.SetField("B42t", DateTime.FromOADate(Int32.Parse(bnf.BnfBirthday)).ToString("yyyyMMdd"));
//                                }
//                            }
//                            break;
//                    }
//                }
//            }
//            if (filltf2 != null)
//            {
//                pdfFormFields.SetField("F14t", filltf2.RelinquishingInstitutionName);
//                pdfFormFields.SetField("F15t", filltf2.TransferAmount);
//            }
//            if (fillln != null)
//            {
//                pdfFormFields.SetField("B12c", "On");  //Non-Reg
//                pdfFormFields.SetField("D01c", "On"); //Retirement Savings
//                pdfFormFields.SetField("D11c", "On"); //Loan
//                pdfFormFields.SetField("D16c", "On");
//                pdfFormFields.SetField("D18c", "On");
//                if (Int32.Parse(fillln.ApplyAmount) >= 100000)
//                {
//                    pdfFormFields.SetField("D21c", "On");
//                }
//                else
//                {
//                    pdfFormFields.SetField("D20c", "On");
//                }
//                pdfFormFields.SetField("D22c", "On"); // same person
//                pdfFormFields.SetField("D24c", "On"); // individual
//                                                      //fill individual info

//                //9a
//                //xlWorksheet = readWorkbook.Sheets["Personal Info"];
//                //xlRange = xlWorksheet.UsedRange;

//                ////pdfFormFields.SetField("B38t", /*Principal occupation or business*/(xlRange.Range["E34"] != null && xlRange.Range["E34"].Value2 != null) ? xlRange.Range["E34"].Value2.ToString() : "");
//                //if (xlRange.Range["C34"] != null && xlRange.Range["C34"].Value2 != null && xlRange.Range["C34"].Value2.ToString() != "Other - Specify")
//                //{
//                //    pdfFormFields.SetField("B38t", /*Principal occupation*/ xlRange.Range["C34"].Value2.ToString());
//                //}
//                //else if (xlRange.Range["E34"] != null && xlRange.Range["E34"].Value2 != null)
//                //{
//                //    pdfFormFields.SetField("B38t", /*Principal occupation*/ xlRange.Range["E34"].Value2.ToString());
//                //}
//                //else if (xlRange.Range["C27"] != null && xlRange.Range["C27"].Value2 != null)
//                //{
//                //    pdfFormFields.SetField("B38t", /*Principal occupation*/ xlRange.Range["C27"].Value2.ToString());
//                //               
//                //pdfFormFields.SetField("D31t", /*First name*/ tempApp.Applicant.FirstName.ToUpper());
//                //pdfFormFields.SetField("D32t", /*Last Name*/tempApp.Applicant.LastName.ToUpper());
//                //if (xlRange.Range["K8"] != null && xlRange.Range["K8"].Value2 != null && isSINvalid(xlRange.Range["K8"].Value2.ToString().ToUpper()))
//                //{
//                //    pdfFormFields.SetField("D34t", /*SIN*/(xlRange.Range["K8"] != null && xlRange.Range["K8"].Value2 != null) ? xlRange.Range["K8"].Value2.ToString().ToUpper() : "");
//                //}
//                //string bid = (xlRange.Range["D8"] != null && xlRange.Range["D8"].Value2 != null) ? xlRange.Range["D8"].Value2.ToString() : "yyyy";
//                //bid += (xlRange.Range["F8"] != null && xlRange.Range["F8"].Value2 != null) ? convertMonth(xlRange.Range["F8"].Value2.ToString()) : "00";
//                //string tempBday = (xlRange.Range["H8"] != null && xlRange.Range["H8"].Value2 != null) ? ("00" + xlRange.Range["H8"].Value2.ToString()) : "00";
//                //bid += tempBday.Substring(tempBday.Length - 2);
//                //pdfFormFields.SetField("D33t", /*Birthday*/bid); //Birthday
//                //pdfFormFields.SetField("D35c", /*Gender Female*/(xlRange.Range["B8"] != null && xlRange.Range["B8"].Value2 != null && xlRange.Range["B8"].Value2.ToString().ToUpper() == "FEMALE") ? "On" : ""); // Gender Female
//                //pdfFormFields.SetField("D36c", /*Gender Male*/(xlRange.Range["B8"] != null && xlRange.Range["B8"].Value2 != null && xlRange.Range["B8"].Value2.ToString().ToUpper() == "MALE") ? "On" : ""); // Gender Male
//                //pdfFormFields.SetField("D37c", "On"); // English
//                //                                      //pdfFormFields.SetField("B28c", "On"); //French
//                //pdfFormFields.SetField("D40t", /*home phone*/(xlRange.Range["H16"] != null && xlRange.Range["H16"].Value2 != null) ? xlRange.Range["H16"].Value2.ToString().ToUpper().Replace("-", "") : ((xlRange.Range["F16"] != null && xlRange.Range["F16"].Value2 != null) ? xlRange.Range["F16"].Value2.ToString().ToUpper().Replace("-", "") : "")); // home phone
//                //pdfFormFields.SetField("D39t", /*email*/(xlRange.Range["A16"] != null && xlRange.Range["A16"].Value2 != null) ? xlRange.Range["A16"].Value2.ToString().ToUpper() : ""); // email
//                //                                                                                                                                                                        //pdfFormFields.SetField("D43t", /*Cell phone*/(xlRange.Range["F16"] != null && xlRange.Range["F16"].Value2 != null) ? xlRange.Range["F16"].Value2.ToString().ToUpper() : ""); //Cell phone
//                //address = "";
//                //if (xlRange.Range["A12"] != null && xlRange.Range["A12"].Value2 != null)
//                //{
//                //    address = xlRange.Range["A12"].Value2.ToString().ToUpper();
//                //}
//                //if (xlRange.Range["B12"] != null && xlRange.Range["B12"].Value2 != null)
//                //{
//                //    address = string.IsNullOrEmpty(address) ? xlRange.Range["B12"].Value2.ToString().ToUpper() : (address + " " + xlRange.Range["B12"].Value2.ToString().ToUpper());
//                //}
//                //pdfFormFields.SetField("D44t", address); // address
//                //pdfFormFields.SetField("B37bt", /*Apt. PO BOX*/(xlRange.Range["E12"] != null && xlRange.Range["E12"].Value2 != null) ? xlRange.Range["E12"].Value2.ToString() : ""); //Apt. PO BOX
//                //pdfFormFields.SetField("D45t", /*City*/(xlRange.Range["F12"] != null && xlRange.Range["F12"].Value2 != null) ? xlRange.Range["F12"].Value2.ToString() : ""); //City
//                //pdfFormFields.SetField("D46t", /*Province*/(xlRange.Range["H12"] != null && xlRange.Range["H12"].Value2 != null) ? xlRange.Range["H12"].Value2.ToString() : ""); //Province
//                //pdfFormFields.SetField("D47t", /*Postal code*/(xlRange.Range["I12"] != null && xlRange.Range["I12"].Value2 != null) ? xlRange.Range["I12"].Value2.ToString().Replace(" ", "") : ""); //Postal code
//                //if (xlRange.Range["C34"] != null && xlRange.Range["C34"].Value2 != null && xlRange.Range["C34"].Value2.ToString() != "Other - Specify")
//                //{
//                //    pdfFormFields.SetField("D48t", /*Principal occupation*/ xlRange.Range["C34"].Value2.ToString());
//                //}
//                //else if (xlRange.Range["E34"] != null && xlRange.Range["E34"].Value2 != null)
//                //{
//                //    pdfFormFields.SetField("D48t", /*Principal occupation*/ xlRange.Range["E34"].Value2.ToString());
//                //}
//                //else if (xlRange.Range["C27"] != null && xlRange.Range["C27"].Value2 != null)
//                //{
//                //    pdfFormFields.SetField("D48t", /*Principal occupation*/ xlRange.Range["C27"].Value2.ToString());
//                //}
//                ////9b- Confirmation of Identity of Individual Applicant
//                //pdfFormFields.SetField("D49t", /*ID Type: Driver's License*/(xlRange.Range["A18"] != null && xlRange.Range["A18"].Value2 != null) ? xlRange.Range["A18"].Value2.ToString().ToUpper() : "");
//                //pdfFormFields.SetField("D50t", /*ID NUMBER*/(xlRange.Range["C18"] != null && xlRange.Range["C18"].Value2 != null) ? xlRange.Range["C18"].Value2.ToString().ToUpper() : "");
//                //pdfFormFields.SetField("D51t", "CANADA");
//                //pdfFormFields.SetField("D52t", /*Issue pROVINCE*/(xlRange.Range["D19"] != null && xlRange.Range["D19"].Value2 != null) ? xlRange.Range["D19"].Value2.ToString().ToUpper() : "");
//                //pdfFormFields.SetField("D53t", /*Expiry Date*/(xlRange.Range["H18"] != null && xlRange.Range["H18"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["H18"].Value2.ToString())).ToString("yyyyMMdd") : "");
//                //pdfFormFields.SetField("D54t", /*Verify Date*/(xlRange.Range["J18"] != null && xlRange.Range["J18"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["J18"].Value2.ToString())).ToString("yyyyMMdd") : "");

//                //9c
//                pdfFormFields.SetField("D55c", "On");
//                pdfFormFields.SetField("D58c", "On");
//                string relationship = "";
//                if (tempApp.Applicant.MaritalStatus != null)
//                {
//                    switch (tempApp.Applicant.MaritalStatus.ToUpper())
//                    {
//                        case "MARRIED":
//                            relationship= "Spouse";
//                            break;
//                        case "COMMON LAW":
//                            relationship = "Common Law";
//                            break;
//                    }
//                }
//                pdfFormFields.SetField("C65t", tempApp.CoApplicant.FirstName.ToUpper());
//                pdfFormFields.SetField("C66t", tempApp.CoApplicant.LastName.ToUpper());

//                string co_bd = tempApp.CoApplicant.DobYear;
//                co_bd += tempApp.CoApplicant.DobMonth;
//                string co_bdday = string.IsNullOrEmpty(tempApp.CoApplicant.DobDay) ? "00" + tempApp.CoApplicant.DobDay : "00";
//                co_bd += co_bdday.Substring(co_bdday.Length - 2);
//                pdfFormFields.SetField("C67t", co_bd);
//                pdfFormFields.SetField("E03t", co_bd);
//                if (!string.IsNullOrEmpty(relationship))
//                {
//                    pdfFormFields.SetField("C68t", relationship);
//                }
//                // 10a,b,c  Co-Borrower
//                pdfFormFields.SetField("E01t", tempApp.CoApplicant.FirstName.ToUpper());
//                pdfFormFields.SetField("E02t", tempApp.CoApplicant.LastName.ToUpper());

//                bool idfilled = false;
//                foreach (ID id in tempApp.CoApplicant.PersonIDs)
//                {
//                    if (id.IdType.ToUpper() == "SIN")
//                    {
//                        pdfFormFields.SetField("E04t", id.IdNumber);
//                    }
//                    if (idfilled)
//                    {
//                        break;
//                    }
//                }
//                pdfFormFields.SetField("E05c", (tempApp.CoApplicant.Gender.ToUpper() == "FEMALE") ? "On" : "");
//                pdfFormFields.SetField("E06c", (tempApp.CoApplicant.Gender.ToUpper() == "MALE") ? "On" : "");
//                pdfFormFields.SetField("E07c", /*English*/ "On"); //pdfFormFields.SetField("E08c", /*French*/);
//                pdfFormFields.SetField("E10t", tempApp.CoApplicant.Homephone != null ? tempApp.CoApplicant.Homephone : tempApp.CoApplicant.Cellphone !=null ? tempApp.CoApplicant.Cellphone : "");
//                pdfFormFields.SetField("E09t", tempApp.CoApplicant.Email.ToUpper());               
//                string co_add = tempApp.CoApplicant.PersonAddress[0].StreetNo;
//                co_add = string.IsNullOrEmpty(co_add) ? tempApp.CoApplicant.PersonAddress[0].StreetName.ToUpper() : (co_add + " " + tempApp.CoApplicant.PersonAddress[0].StreetName.ToUpper());
//                pdfFormFields.SetField("E14t", co_add);
//                pdfFormFields.SetField("E14bt", tempApp.CoApplicant.PersonAddress[0].AptNo);
//                pdfFormFields.SetField("E15t", tempApp.CoApplicant.PersonAddress[0].City.ToUpper());
//                pdfFormFields.SetField("E16t", tempApp.CoApplicant.PersonAddress[0].Province.ToUpper());
//                pdfFormFields.SetField("E17t", tempApp.CoApplicant.PersonAddress[0].Postcode.ToUpper());
//                pdfFormFields.SetField("E18t", tempApp.CoApplicant.PersonEmployment[0].Occupation.ToUpper());                
//                foreach (ID id in tempApp.CoApplicant.PersonIDs)
//                {
//                    switch (id.IdType.ToUpper())
//                    {
//                        case "PROVINCIAL DRIVER'S LICENSE":
//                        case "PROVINCIAL PHOTO ID":                       
//                        case "PR CARD":
//                        case "HEALTH CARD":
//                            pdfFormFields.SetField("E19t", id.IdType.ToUpper());
//                            pdfFormFields.SetField("E20t", id.IdNumber.ToUpper());
//                            pdfFormFields.SetField("E21t",/*Country of issue:*/ "CANADA");
//                            pdfFormFields.SetField("E22t", id.IssueProvince.ToUpper());
//                            pdfFormFields.SetField("E23t", id.ExpiryDate);
//                            pdfFormFields.SetField("E24t", id.IssueDate);
//                            pdfFormFields.SetField("E25c", "On");
//                            break;
//                        case "PASSPORT":
//                            pdfFormFields.SetField("E19t", id.IdType.ToUpper());
//                            pdfFormFields.SetField("E20t", id.IdNumber.ToUpper());
//                            pdfFormFields.SetField("E21t", /*Issue Country*/ "");
//                            pdfFormFields.SetField("E22t", id.IssueProvince.ToUpper());
//                            pdfFormFields.SetField("E23t", id.ExpiryDate);
//                            pdfFormFields.SetField("E24t", id.IssueDate);
//                            pdfFormFields.SetField("E25c", "On");
//                            break;
//                    }
//                }
//                pdfFormFields.SetField("E25c", "On" /*No//Is the Co-Applicant a tax resident or a citizen of the United States?*/);
//                pdfFormFields.SetField("E28c", "On" /*No//Is the Co-Applicant a tax resident in a jurisdiction other than Canada or the United States?*/);

              
//                //if (xlRange.Range["L6"] != null && xlRange.Range["L6"].Value2 != null && xlRange.Range["L6"].Value2.ToString() == "Yes")
//                //{
//                //    string relationship = "";
//                //    if (xlRange.Range["D9"] != null && xlRange.Range["D9"].Value2 != null)
//                //    {
//                //        if (xlRange.Range["D9"].Value2.ToString() == "Married")
//                //        {
//                //            relationship = "Spouse";
//                //        }
//                //        else if (xlRange.Range["D9"].Value2.ToString() == "Married")
//                //        {
//                //            relationship = "Common Law";
//                //        }
//                //    }
//                //    xlWorksheet = readWorkbook.Sheets["Co Applicant Info"];
//                //    xlRange = xlWorksheet.UsedRange;

//                //    //7Designation of a Successor Annuitant
//                //    pdfFormFields.SetField("C65t",/*First Name*/(xlRange.Range["E7"] != null && xlRange.Range["E7"].Value2 != null) ? xlRange.Range["E7"].Value2.ToString().ToUpper() : "");
//                //    pdfFormFields.SetField("C66t",/*Last Name*/(xlRange.Range["B7"] != null && xlRange.Range["B7"].Value2 != null) ? xlRange.Range["B7"].Value2.ToString().ToUpper() : "");
//                //    bd = (xlRange.Range["D8"] != null && xlRange.Range["D8"].Value2 != null) ? xlRange.Range["D8"].Value2.ToString() : "yyyy";
//                //    bd += (xlRange.Range["F8"] != null && xlRange.Range["F8"].Value2 != null) ? convertMonth(xlRange.Range["F8"].Value2.ToString()) : "00";
//                //    tempday = (xlRange.Range["H8"] != null && xlRange.Range["H8"].Value2 != null) ? ("00" + xlRange.Range["H8"].Value2.ToString()) : "00";
//                //    bd += tempday.Substring(tempday.Length - 2);
//                //    pdfFormFields.SetField("C67t", /*Birthday*/bd);

//                //    if (!string.IsNullOrEmpty(relationship))
//                //    {
//                //        pdfFormFields.SetField("C68t", relationship);
//                //    }

//                //    // 10a,b,c  Co-Borrower
//                //    pdfFormFields.SetField("E01t",/*First Name*/(xlRange.Range["E7"] != null && xlRange.Range["E7"].Value2 != null) ? xlRange.Range["E7"].Value2.ToString().ToUpper() : "");
//                //    pdfFormFields.SetField("E02t",/*Last Name*/(xlRange.Range["B7"] != null && xlRange.Range["B7"].Value2 != null) ? xlRange.Range["B7"].Value2.ToString().ToUpper() : "");
//                //    if (xlRange.Range["K8"] != null && xlRange.Range["K8"].Value2 != null && isSINvalid(xlRange.Range["K8"].Value2.ToString().ToUpper()))
//                //    {
//                //        pdfFormFields.SetField("E04t", /*SIN*/(xlRange.Range["K8"] != null && xlRange.Range["K8"].Value2 != null) ? xlRange.Range["K8"].Value2.ToString().ToUpper() : "");
//                //    }
//                //    pdfFormFields.SetField("E03t", /*Birthday*/bd);
//                //    pdfFormFields.SetField("E05c",/*Femle*/(xlRange.Range["B8"] != null && xlRange.Range["B8"].Value2 != null && xlRange.Range["B8"].Value2.ToString().ToUpper() == "FEMALE") ? "On" : "");
//                //    pdfFormFields.SetField("E06c",/*Male*/(xlRange.Range["B8"] != null && xlRange.Range["B8"].Value2 != null && xlRange.Range["B8"].Value2.ToString().ToUpper() == "MALE") ? "On" : "");
//                //    pdfFormFields.SetField("E07c",/*English*/"On");
//                //    //pdfFormFields.SetField("E08c",/*French*/);
//                //    pdfFormFields.SetField("E10t",/*CellPhone*/(xlRange.Range["H16"] != null && xlRange.Range["H16"].Value2 != null) ? xlRange.Range["H16"].Value2.ToString().ToUpper().Replace("-", "") : ((xlRange.Range["F16"] != null && xlRange.Range["F16"].Value2 != null) ? xlRange.Range["F16"].Value2.ToString().ToUpper().Replace("-", "") : ""));
//                //    pdfFormFields.SetField("E09t",/*Email*/(xlRange.Range["A16"] != null && xlRange.Range["A16"].Value2 != null) ? xlRange.Range["A16"].Value2.ToString().ToUpper() : "");
//                //    //pdfFormFields.SetField("E11t",/**/);
//                //    //pdfFormFields.SetField("E12t",/**/);
//                //    //pdfFormFields.SetField("E13t",/**/);
//                //    address = "";
//                //    if (xlRange.Range["A12"] != null && xlRange.Range["A12"].Value2 != null)
//                //    {
//                //        address = xlRange.Range["A12"].Value2.ToString().ToUpper();
//                //    }
//                //    if (xlRange.Range["B12"] != null && xlRange.Range["B12"].Value2 != null)
//                //    {
//                //        address = string.IsNullOrEmpty(address) ? xlRange.Range["B12"].Value2.ToString().ToUpper() : (address + " " + xlRange.Range["B12"].Value2.ToString().ToUpper());
//                //    }
//                //    pdfFormFields.SetField("E14t",/*AddressNumberStreet*/address); // address
//                //    pdfFormFields.SetField("E14bt",/*Apt.PO Box*/(xlRange.Range["E12"] != null && xlRange.Range["E12"].Value2 != null) ? xlRange.Range["E12"].Value2.ToString() : ""); //Apt. PO BOX
//                //    pdfFormFields.SetField("E15t",/*City*/(xlRange.Range["F12"] != null && xlRange.Range["F12"].Value2 != null) ? xlRange.Range["F12"].Value2.ToString() : ""); //City
//                //    pdfFormFields.SetField("E16t",/*Province*/(xlRange.Range["H12"] != null && xlRange.Range["H12"].Value2 != null) ? xlRange.Range["H12"].Value2.ToString() : ""); //Province
//                //    pdfFormFields.SetField("E17t",/*PostCode*/(xlRange.Range["I12"] != null && xlRange.Range["I12"].Value2 != null) ? xlRange.Range["I12"].Value2.ToString() : ""); //Postal code
                    
//                //    //pdfFormFields.SetField("E18t",/*PrincipalOccupation*/);
//                //    if (xlRange.Range["C34"] != null && xlRange.Range["C34"].Value2 != null && xlRange.Range["C34"].Value2.ToString() != "Other - Specify")
//                //    {
//                //        pdfFormFields.SetField("E18t", /*Principal occupation*/ xlRange.Range["C34"].Value2.ToString());
//                //    }
//                //    else if (xlRange.Range["E34"] != null && xlRange.Range["E34"].Value2 != null)
//                //    {
//                //        pdfFormFields.SetField("E18t", /*Principal occupation*/ xlRange.Range["E34"].Value2.ToString());
//                //    }
//                //    else if (xlRange.Range["C27"] != null && xlRange.Range["C27"].Value2 != null)
//                //    {
//                //        pdfFormFields.SetField("E18t", /*Principal occupation*/ xlRange.Range["C27"].Value2.ToString());
//                //    }

//                //    pdfFormFields.SetField("E19t",/*ID type*/(xlRange.Range["A18"] != null && xlRange.Range["A18"].Value2 != null) ? xlRange.Range["A18"].Value2.ToString().ToUpper() : "");
//                //    pdfFormFields.SetField("E20t",/*Document number:*/(xlRange.Range["C18"] != null && xlRange.Range["C18"].Value2 != null) ? xlRange.Range["C18"].Value2.ToString().ToUpper() : "");
//                //    pdfFormFields.SetField("E21t",/*Country of issue:*/"CANADA");
//                //    pdfFormFields.SetField("E22t",/*Province/state of issue*/(xlRange.Range["D19"] != null && xlRange.Range["D19"].Value2 != null) ? xlRange.Range["D19"].Value2.ToString().ToUpper() : "");
//                //    pdfFormFields.SetField("E23t",/*Expiry date (if applicable):*/(xlRange.Range["H18"] != null && xlRange.Range["H18"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["H18"].Value2.ToString())).ToString("yyyyMMdd") : "");
//                //    pdfFormFields.SetField("E24t",/*Date identity confirmed*/(xlRange.Range["J18"] != null && xlRange.Range["J18"].Value2 != null) ? DateTime.FromOADate(Int32.Parse(xlRange.Range["J18"].Value2.ToString())).ToString("yyyyMMdd") : "");
//                //    pdfFormFields.SetField("E25c", "On"/*No//Is the Co-Applicant a tax resident or a citizen of the United States?*/);
                    
//                //    //pdfFormFields.SetField("E26c",/*Yes//Is the Co-Applicant a tax resident or a citizen of the United States?*/);
//                //    //pdfFormFields.SetField("E27t",/*If “YES”, indicate the U.S. Taxpayer Identification Number (TIN) or Social Security Number (SSN)*/);
//                //    pdfFormFields.SetField("E28c", "On"/*No//Is the Co-Applicant a tax resident in a jurisdiction other than Canada or the United States?*/);
//                //    //pdfFormFields.SetField("E29c",/*Yes//Is the Co-Applicant a tax resident in a jurisdiction other than Canada or the United States?*/);
//                //    //pdfFormFields.SetField("E30t",/*Jurisdiction of tax residence:*/);
//                //    //pdfFormFields.SetField("E31t",/*Tax Identification Number*/);
//                //    //pdfFormFields.SetField("E32t",/*Jurisdiction of tax residence:*/);
//                //    //pdfFormFields.SetField("E33t",/*Tax Identification Number*/);
//                //    //pdfFormFields.SetField("E34t",/*Line1//Reason for no TIN (if applicable):*/);
//                //    //pdfFormFields.SetField("E35t",/*Line2//Reason for no TIN (if applicable):*/);
//                //    //pdfFormFields.SetField("E36t",/*Line3//Reason for no TIN (if applicable):*/);

//                //}
//            }
//            pdfFormFields.SetField("F27c", "On");
//            pdfFormFields.SetField("F26t", "100");
//            pdfFormFields.SetField("F28c", "On");
//            pdfFormFields.SetField("F34c", "On");

//            pdfStamper.FormFlattening = false;
//            pdfStamper.Close();
//            pdfReader.Close();
//        }

//        private void prepare_iA_NPcodeFile(string applicationFilePath, string tempNPCodefile)
//        {
//            //pdfTemplate = SourePDFfolder + @"\iA\NEW\TFSA Application.pdf";//  @"C:\Users\Jade\Documents\FillPDF\PDFSolutions\PDFSolutions\Files\iA_IAG.pdf";
//            //newFile = DefaultOutFolder + "\\" + applicantName + @"\temppdf.pdf";
//            if (File.Exists(tempNPCodefile))
//            {
//                File.Delete(tempNPCodefile);
//            }
//            if (!File.Exists(applicationFilePath))
//            {
//                ErrMessage += "\nSource iA application file doesn't exist.";
//                return;
//            }
//            pdfReader = new PdfReader(applicationFilePath);
//            PdfReader.unethicalreading = true;
//            pdfStamper = new PdfStamper(pdfReader, new FileStream(tempNPCodefile, FileMode.Create));
//            pdfFormFields = pdfStamper.AcroFields;
//            pdfFormFields.SetField("B03c", "On"); //TFSA, RRSP, Non-Reg......
//            pdfFormFields.SetField("Prénom", "1"); //For RESP
//            pdfStamper.Close();
//            pdfReader.Close();
//            System.Diagnostics.Process.Start(tempNPCodefile);
//            FileInfo fi = new FileInfo(tempNPCodefile);
//            System.Threading.Thread.Sleep(3000);
//            while (IsFileinUse(fi))
//            {
//                System.Threading.Thread.Sleep(2000);
//            }
//            return;
//        }


        protected virtual bool IsFileinUse(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }
        #endregion command

        #region public interface

        
        #endregion public interface

    }
}