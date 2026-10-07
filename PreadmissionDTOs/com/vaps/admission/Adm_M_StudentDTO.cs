using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PreadmissionDTOs.com.vaps.admission
{
    public class Adm_M_StudentDTO : CommonParamDTO
    {
        public long userid { get; set; }
        public long AMST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public DateTime AMST_Date { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public long? AMC_Id { get; set; }
        public string AMST_Sex { get; set; }
        public DateTime AMST_DOB { get; set; }
        public string AMST_DOB_Words { get; set; }
        public int? PASR_Age { get; set; }
        public long? ASMCL_Id { get; set; }
        public string AMST_BloodGroup { get; set; }
        public string AMST_MotherTongue { get; set; }
        public string AMST_BirthCertNO { get; set; }
        public long? IVRMMR_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public long? IMC_Id { get; set; }
        public string AMST_PerStreet { get; set; }
        public string AMST_PerArea { get; set; }
        public string AMST_PerCity { get; set; }
        public string AMST_PerAdd3 { get; set; }
        public long? AMST_PerState { get; set; }
        public long? AMST_PerCountry { get; set; }
        public int? AMST_PerPincode { get; set; }
        public string AMST_ConStreet { get; set; }
        public string AMST_ConArea { get; set; }
        public string AMST_ConCity { get; set; }
        public long? AMST_ConState { get; set; }
        public long? AMST_ConCountry { get; set; }
        public int? AMST_ConPincode { get; set; }
        public long? AMST_AadharNo { get; set; }
        public string AMST_StuBankAccNo { get; set; }
        public string AMST_StuBankIFSC_Code { get; set; }
        public string AMST_StuCasteCertiNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string AMST_FatherAliveFlag { get; set; }
        public string AMST_FatherName { get; set; }
        public long? AMST_FatherAadharNo { get; set; }
        public string AMST_FatherSurname { get; set; }
        public string AMST_FatherEducation { get; set; }

        public string AMST_FatherOccupation { get; set; }
        public string AMST_FatherOfficeAdd { get; set; }
        public string AMST_FatherDesignation { get; set; }
        public decimal? AMST_FatherMonIncome { get; set; }
        public decimal? AMST_FatherAnnIncome { get; set; }
        public long? AMST_FatherNationality { get; set; }
        public long? AMST_FatherMobleNo { get; set; }
        public string AMST_FatheremailId { get; set; }
        public string AMST_FatherBankAccNo { get; set; }
        public string AMST_FatherBankIFSC_Code { get; set; }
        public string AMST_FatherCasteCertiNo { get; set; }
        public string ANST_FatherPhoto { get; set; }
        public string AMST_MotherAliveFlag { get; set; }
        public string AMST_MotherName { get; set; }
        public long? AMST_MotherAadharNo { get; set; }
        public string AMST_MotherSurname { get; set; }
        public string AMST_MotherEducation { get; set; }
        public string AMST_MotherOccupation { get; set; }
        public string AMST_MotherOfficeAdd { get; set; }
        public string AMST_MotherDesignation { get; set; }
        public decimal? AMST_MotherMonIncome { get; set; }
        public decimal? AMST_MotherAnnIncome { get; set; }
        public long? AMST_MotherNationality { get; set; }
        public long? AMST_MotherMobileNo { get; set; }
        public string AMST_MotherEmailId { get; set; }
        public string AMST_MotherBankAccNo { get; set; }
        public string AMST_MotherBankIFSC_Code { get; set; }
        public string AMST_MotherCasteCertiNo { get; set; }
        public decimal? AMST_TotalIncome { get; set; }
        public string ANST_MotherPhoto { get; set; }
        public string AMST_BirthPlace { get; set; }
        public long? AMST_Nationality { get; set; }
        public int? AMST_BPLCardFlag { get; set; }
        public string AMST_BPLCardNo { get; set; }
        public int? AMST_HostelReqdFlag { get; set; }
        public int? AMST_TransportReqdFlag { get; set; }
        public int? AMST_GymReqdFlag { get; set; }
        public int? AMST_ECSFlag { get; set; }
        public int? AMST_PaymentFlag { get; set; }
        public int? AMST_AmountPaid { get; set; }
        public string AMST_PaymentType { get; set; }
        public DateTime? AMST_PaymentDate { get; set; }
        public string AMST_ReceiptNo { get; set; }
        public int? AMST_ActiveFlag { get; set; }
        public string AMST_ApplStatus { get; set; }
        public int? AMST_FinalpaymentFlag { get; set; }
        public string AMST_Photoname { get; set; }
        public string AMST_SOL { get; set; }

        public long? AMST_Noofbrothers { get; set; }

        public long? AMST_Noofsisters { get; set; }

        public string AMST_Father_Signature { get; set; }
        public string AMST_Father_FingerPrint { get; set; }
        public string AMST_Mother_Signature { get; set; }
        public string AMST_Mother_FingerPrint { get; set; }
        public long? AMST_Concession_Type { get; set; }


        //-----------------------------------------//

        public Array AllAcademicYear { get; set; }
        public Array AllClass { get; set; }

        public Array AllSection { get; set; }

        public Array AllSubject { get; set; }

        public Array AllCountry { get; set; }

        public Array AllState { get; set; }

        public Array AllCity { get; set; }

        public Array AllReligion { get; set; }

        public Array AllCaste { get; set; }

        public Array AllcasteCategory { get; set; }

        public Array AllRefrence { get; set; }
        public Array AllSources { get; set; }

        public Array AllActivity { get; set; }

        public Array DocumentList { get; set; }


        public Array BondList { get; set; }

        public Array PrevSchoolDetails { get; set; }

        public Array StudentGuardianDetails { get; set; }

        public Array StudentSiblingDetails { get; set; }
        public AdmittedStudentSiblingDTO[] SelectedSiblingDetails { get; set; }

        public Array StudentAchivementDetails { get; set; }
        public Array StudentReferenceDetails { get; set; }
        public Array StudentSourceDetails { get; set; }

        public Array StudentActivityDetails { get; set; }
        public Array adm_m_student { get; set; }

        // public List<MasterActivityDTO> SelectedActivityDetails { get; set; }

        public StudentActivityDTO[] SelectedActivityDetails { get; set; }

        public StudentReferenceDTO[] SelectedRefrenceDetails { get; set; }
        public StudentSourceDTO[] SelectedSourceDetails { get; set; }

        public GovernmentBondDTO[] SelectedBondDetails { get; set; }

        public AdmittedStudentPrevSchoolDTO[] SelectedPrevSchoolDetails { get; set; }

        public AdmittedStudentGuardianDTO[] SelectedGuardianDetails { get; set; }


        public List<Adm_M_StudentDTO> studentdetails { get; set; }

        public Array fillacademicyr { get; set; }
        public Array fillclass { get; set; }

        public Array preAdmtoAdmStuList { get; set; }

        public string SelectedAchivementDetails { get; set; }

        //Added for Admission Student Search by Sripad Joshi.
        public string stuStatus { get; set; }
        public List<Object> field = new List<Object>();
        public List<Object> Operator = new List<Object>();

        public List<Object> value = new List<Object>();
        public List<Object> condition = new List<Object>();
        public Array searchResult { get; set; }

        public Array savetmpdata { get; set; }

        public Array AllAchivement { get; set; }


        public long ASMC_Id { get; set; }

        public long AMSTEC_Id { get; set; }

        public Array studentlist { get; set; }

        public string HFC_Flag { get; set; }

        public Array fillstudlist { get; set; }


        // added on 21/02/2017
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AMSTRegistrationNo { get; set; }
        public string AMSTAdmNo { get; set; }  //  public long? AMC_Id { get; set; }
        public string Gender { get; set; } //AMST_Sex
        public DateTime DOB { get; set; }  //  public int? PASR_Age { get; set; }
        public string AMSTDOBWords { get; set; }
        public string Class { get; set; }  //long?

        public string BloodGroup { get; set; }
        public string MotherTongue { get; set; }
        public string BirthCertificateNo { get; set; }
        public string Religion { get; set; }
        public string CasteCategory { get; set; }
        public string Caste { get; set; }
        public string PermanentStreet { get; set; }
        public string PermanentArea { get; set; }
        public string PermanentCity { get; set; }
        public long? PermanentCountry { get; set; }  //changed
        public int? PermanentPincode { get; set; }
        public string PresentStreet { get; set; }
        public string PresentArea { get; set; }
        public string PresentCity { get; set; }
        public long? PresentState { get; set; }
        public long? PresentCountry { get; set; }
        public int? PresentPincode { get; set; }
        public long? AadharNo { get; set; }
        public string BankAccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string CasteCertificateNo { get; set; }
        public long MobileNo { get; set; }
        public string EmailID { get; set; }
        public string FatherAlive { get; set; }
        public string FatherName { get; set; }
        public long? FatherAadharNo { get; set; }
        public string FatherSurname { get; set; }
        public string FatherOccupation { get; set; }
        public string FatherOfficeAddress { get; set; }
        public string FatherDesignation { get; set; }
        public decimal? FatherMonthlyIncome { get; set; }
        public decimal? FatherAnnualIncome { get; set; }
        public long? FatherNationality { get; set; }
        public long? Fathermobileno { get; set; }
        public string FatherEmailId { get; set; }
        public string FatherAccountNo { get; set; }
        public string FatherIFSCcode { get; set; }
        public string FathercastecertificateNo { get; set; }
        public string FatherPhoto { get; set; }
        public string MotherAlive { get; set; }
        public string MotherName { get; set; }
        public long? MotherAadharNo { get; set; }
        public string MotherSurname { get; set; }
        public string MotherEducation { get; set; }
        public string MotherOcupation { get; set; }
        public string MotherDesignation { get; set; }
        public string MotherOfficesAddress { get; set; }
        public decimal? MotherMonthlyIncome { get; set; }
        public decimal? MotherAnnualIncome { get; set; }
        public long? MotherNationality { get; set; }
        public long? MotherMobileNo { get; set; }
        public string MotherEmailId { get; set; }
        public string MotherBankAccountNo { get; set; }
        public string MotherIFSCcode { get; set; }
        public string MotherCasteCertificateNo { get; set; }
        public decimal? TotalIncome { get; set; }
        public string MotherPhoto { get; set; }
        public string StudentBirthPlace { get; set; }
        public long? StudentNationality { get; set; }
        public int? BPLCardFlag { get; set; }
        public string BPLCardNo { get; set; }
        public int? HostelFacility { get; set; }
        public int? TransportFacility { get; set; }
        public int? GymFacility { get; set; }
        public int? ECSFlag { get; set; }
        public int? PaymentFlag { get; set; }
        public string AmountPaid { get; set; }
        public string PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReceiptNo { get; set; }
        public string ActiveFlag { get; set; }
        public string Applicationstatus { get; set; }
        public string FinalPaymentFlag { get; set; }
        public string PhotoName { get; set; }

        public long? Noofsisters { get; set; }
        public long? NoOfBrothers { get; set; }
        public List<Adm_M_StudentDTO> newlstget { set; get; }
        public int count { get; set; }

        public bool returnval { get; set; } = true;

        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public Array admTransNumSetting { get; set; }
        public Master_NumberingDTO admissionNumbering { get; set; }
        public MasterConfigurationDTO configurationsettings { get; set; }

        public int payementcheck { get; set; }
        public string returnMsg { get; set; }


        public long asms_id { get; set; }
        public Array studentlist123 { get; set; }



        public StudentDocumentDTO documents { get; set; }
        public Array adm_m_stud_cat { get; set; }




        public Array stud_catg_edit { get; set; }
        public string flag123 { get; set; }
        public Array achievement { get; set; }

        public long AMSMD_Id { get; set; }

        public MasterDocumentDTO[] Uploaded_documentList { get; set; }

        public AdmittedStudentGuardianDTO[] guardianPhoto { get; set; }
        public AdmittedStudentGuardianDTO[] guardianSign { get; set; }
        public AdmittedStudentGuardianDTO[] guardianFingerprint { get; set; }
        public int duplicateMobNo { get; set; }
        public int duplicateEmailId { get; set; }
        public int duplicateAdharNo { get; set; }
        public Array admissioncongigurationList { get; set; }
        public bool Edit_flag { get; set; }
        public Array activityIds { get; set; }
        public Array referenceIds { get; set; }
        public Array sourceIds { get; set; }
        public string IMC_CasteName { get; set; }
        public Array academicYearOnLoad { get; set; }
        public Array StudentList1 { get; set; }
        public Array academicyearforreadmit { get; set; }
        public string Status_Flag { get; set; }
        public string companyname { get; set; }
        public Array MasterCompany { get; set; }
        public Array academicList1 { get; set; }
        public string SearchColumn { get; set; }
        public string EnteredData { get; set; }
        public string message { get; set; }
        public Array govtBondList { get; set; }
        public Array bondIds { get; set; }
        public Array boardList { get; set; }
        public Array concessionList { get; set; }
        public string admRegManualFlag { get; set; }
        public string admAdmManualFlag { get; set; }
        public string preventdupRegNo { get; set; }
        public string preventdupAdmNo { get; set; }
        public int duplicateRegNo { get; set; }
        public int duplicateAdmNo { get; set; }
        //added by vishnu 
        public string classname { get; set; }
        public string sectionname { get; set; }
        public Array classsection { get; set; }
        public int totalgender { get; set; }
        public string gender1 { get; set; }
        public Adm_M_Student_TempDTO[] multiplemobileno { get; set; }
        public Adm_M_Student_Eamil[] multipleemailid { get; set; }
        public Adm_M_Mother_MobileNo1[] multiplemobilenomother { get; set; }
        public Adm_M_Mother_Emailid1[] multipleemailidmother { get; set; }
        public Adm_M_Student_MobileNoDTO[] Adm_M_Student_MobileNoDTO { get; set; }
        public Adm_M_Student_EmailIdDTO[] Adm_M_Student_EmailIdDTO { get; set; }
        public Adm_M_Student_TempMobileNo[] stdmobile { get; set; }
        public Adm_M_Student_TempEmailId[] stdemail { get; set; }
        public Adm_M_Student_ECS Adm_M_Student_ECS { get; set; }
        public Array Adm_M_Student_ECSDTo { get; set; }
        public string stdmobilenumber { get; set; }
        public Array Schooltypelist { get; set; }
        public string countryName { get; set; }
        public Array prevStateList { get; set; }
        public DateTime dateString { get; set; }
        public Array studentCategory { get; set; }
        public string admflag { get; set; }
        public string admclassCapacity { get; set; }

        public Array academicList { get; set; }
        public Array classlist { get; set; }
        public Array SectionList { get; set; }
        public string allorindiflag { get; set; }
        public Array getcarddetails { get; set; }
        public string stuname { get; set; }
        public string Name { get; set; }
        public string withtc { get; set; }
        public string withdeactive { get; set; }
        public string AMST_SubCasteIMC_Id { get; set; }
        public string AMST_FatherReligion { get; set; }
        public string AMST_FatherCaste { get; set; }
        public string AMST_FatherSubCaste { get; set; }
        public string AMST_MotherReligion { get; set; }
        public string AMST_MotherCaste { get; set; }
        public string AMST_MotherSubCaste { get; set; }
        public string AMST_GovtAdmno { get; set; }
        public string AMST_StudentPANNo { get; set; }
        public string AMST_FatherPANNo { get; set; }
        public string AMST_MotherPANNo { get; set; }
        public string AMST_LanguageSpoken { get; set; }
        public string AMST_GPSTrackingId { get; set; }
        public string AMST_AppDownloadedDeviceId { get; set; }
        public int? AMST_NoOfElderBrothers { get; set; }
        public int? AMST_NoOfYoungerBrothers { get; set; }
        public int? AMST_NoOfElderSisters { get; set; }
        public int? AMST_NoOfYoungerSisters { get; set; }
    }
}