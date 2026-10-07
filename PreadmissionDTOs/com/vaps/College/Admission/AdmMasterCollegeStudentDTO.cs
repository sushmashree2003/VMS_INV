using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class AdmMasterCollegeStudentDTO:CommonParamDTO
    {
        public long AMCST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public DateTime? AMCST_Date { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long? AMCOC_Id { get; set; }
        public long? AMCO_Id { get; set; }
        public string AMCST_Sex { get; set; }
        public DateTime AMCST_DOB { get; set; }
        public string AMCST_DOBin_words { get; set; }
        public int AMCST_Age { get; set; }
        public string AMCST_BloodGroup { get; set; }
        public string AMCST_MotherTongue { get; set; }
        public string AMCST_BirthCertNo { get; set; }
        public long? IVRMMR_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public long? IMC_Id { get; set; }
        public string AMCST_StudentSubCaste { get; set; }
        public string AMCST_PerStreet { get; set; }
        public string AMCST_PerArea { get; set; }
        public string AMCST_PerCity { get; set; }
        public string AMCST_PerAdd3 { get; set; }
        public long? AMCST_PerState { get; set; }
        public long? IVRMMC_Id { get; set; }
        public long? AMCST_PerPincode { get; set; }
        public string AMCST_ConStreet { get; set; }
        public string AMCST_ConArea { get; set; }
        public string AMCST_ConAdd3 { get; set; }
        public string AMCST_ConCity { get; set; }
        public string AMCST_Village { get; set; }
        public string AMCST_Taluk { get; set; }
        public string AMCST_District { get; set; }
        public long? AMCST_ConState { get; set; }
        public string AMCST_Urban_Rural { get; set; }
        public long? AMCST_ConCountryId { get; set; }
        public long? AMCST_ConPincode { get; set; }
        public long? AMCST_AadharNo { get; set; }
        public string AMCST_StuBankAccNo { get; set; }
        public string AMCST_StuBankIFSCCode { get; set; }
        public string AMCST_StuCasteCertiNo { get; set; }
        public long? AMCST_MobileNo { get; set; }
        public string AMCST_emailId { get; set; }
        public bool? AMCST_FatherAliveFlag { get; set; }
        public string AMCST_FatherName { get; set; }
        public long? AMCST_FatherAadharNo { get; set; }
        public string AMCST_FatherSurname { get; set; }
        public string AMCST_FatherEducation { get; set; }
        public string AMCST_FatherOccupation { get; set; }
        public string AMCST_FatherOfficeAdd { get; set; }
        public string AMCST_FatherDesignation { get; set; }
        public decimal? AMCST_FatherMonIncome { get; set; }
        public decimal? AMCST_FatherAnnIncome { get; set; }
        public string AMCST_FatherNationality { get; set; }
        public string AMCST_FatherReligion { get; set; }
        public string AMCST_FatherCaste { get; set; }
        public string AMCST_FatherSubCaste { get; set; }
        public long? AMCST_FatherMobleNo { get; set; }
        public string AMCST_FatheremailId { get; set; }
        public long? AMCST_FatherBankAccNo { get; set; }
        public string AMCST_FatherBankIFSCCode { get; set; }
        public string AMCST_FatherCasteCertiNo { get; set; }
        public string AMCST_FatherPhoto { get; set; }
        public string AMCST_FatherSign { get; set; }
        public string AMCST_FatherFingerprint { get; set; }
        public bool? AMCST_MotherAliveFlag { get; set; }
        public string AMCST_MotherName { get; set; }
        public long? AMCST_MotherAadharNo { get; set; }
        public string AMCST_MotherSurname { get; set; }
        public string AMCST_MotherEducation { get; set; }
        public string AMCST_MotherOccupation { get; set; }
        public string AMCST_MotherOfficeAdd { get; set; }
        public string AMCST_MotherDesignation { get; set; }
        public decimal? AMCST_MotherMonIncome { get; set; }
        public decimal? AMCST_MotherAnnIncome { get; set; }
        public string AMCST_MotherNationality { get; set; }
        public string AMCST_MotherReligion { get; set; }
        public string AMCST_MotherCaste { get; set; }
        public string AMCST_MotherSubCaste { get; set; }
        public long? AMCST_MotherMobleNo { get; set; }
        public string AMCST_MotheremailId { get; set; }
        public long? AMCST_MotherBankAccNo { get; set; }
        public string AMCST_MotherBankIFSCCode { get; set; }
        public string AMCST_MotherCasteCertiNo { get; set; }
        public decimal? AMCST_TotalIncome { get; set; }
        public string AMCST_MotherSign { get; set; }
        public string AMCST_MotherPhoto { get; set; }
        public string AMCST_MotherFingerprint { get; set; }
        public string AMCST_BirthPlace { get; set; }
        public long AMCST_Nationality { get; set; }
        public bool? AMCST_BPLCardFlag { get; set; }
        public string AMCST_BPLCardNo { get; set; }
        public bool? AMCST_HostelReqdFlag { get; set; }
        public bool? AMCST_TransportReqdFlag { get; set; }
        public bool? AMCST_GymReqdFlag { get; set; }
        public bool? AMCST_ECSFlag { get; set; }
        public bool? AMCST_PaymentFlag { get; set; }
        public decimal? AMCST_AmountPaid { get; set; }
        public string AMCST_PaymentType { get; set; }
        public DateTime? AMCST_PaymentDate { get; set; }
        public string AMCST_ReceiptNo { get; set; }
        public string AMCST_EMSINo { get; set; }
        public string AMCST_ApplStatus { get; set; }
        public bool? AMCST_FinalpaymentFlag { get; set; }
        public string AMCST_StudentPhoto { get; set; }
        public string AMCST_StudentSign { get; set; }
        public string AMCST_StudentFingerprint { get; set; }
        public int AMCST_NoofSiblingsSchool { get; set; }
        public int AMCST_NoofSiblings { get; set; }
        public int AMCST_NoOfBrothers { get; set; }
        public int AMCST_NoOfSisters { get; set; }
        public int AMCST_NoofDependencies { get; set; }
        public long? AMCST_TPINNO { get; set; }
        public bool AMCST_ActiveFlag { get; set; }

        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string AMCST_SOL { get; set; }
        public long ACMB_Id { get; set; }
        public long ACQ_Id { get; set; }
        public long ACQC_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }

        //Arrays.
        public Array academicYearOnLoad { get; set; }
        public Array AllAcademicYear { get; set; }
        public Array branches { get; set; }
        public Array AllSources { get; set; }
        public Array AllRefrence { get; set; }
        public Array AllcasteCategory { get; set; }
        public Array AllCaste { get; set; }
        public Array AllReligion { get; set; }
        public Array AllCountry { get; set; }
        public Array AllState { get; set; }
        public Array studentCategory { get; set; }
        public Array courses { get; set; }
        public Array semesters { get; set; }
        public Array batches { get; set; }
        public Array quotas { get; set; }
        public Array quotaCategory { get; set; }
        public Array subjectScheme { get; set; }
        public Array schemeType { get; set; }
        public Array admTransNumSetting { get; set; }
        public Array StudentList { get; set; }
        public Array boardList { get; set; }
        public Array Schooltypelist { get; set; }
        public Array prevStateList { get; set; }
        public Array referenceIds { get; set; }
        public Array sourceIds { get; set; }
        public Array PrevSchoolDetails { get; set; }

        public Array Adm_College_Student_SubjectMarksDTO { get; set; }
        public Array StudentGuardianDetails { get; set; }
        public Array StudentSiblingDetails { get; set; }
        public Array StudentReferenceDetails { get; set; }
        public Array StudentSourceDetails { get; set; }
        public Array DocumentList { get; set; }
        public Array combinationlist { get; set; }
        public Adm_College_Student_SMSNoDTO[] Adm_College_Student_SMSNoDTO { get; set; }
        public Adm_College_Student_EmailIdDTO[] Adm_College_Student_EmailIdDTO { get; set; }
        public Adm_M_Student_TempMobileNo[] stdmobile { get; set; }
        public Adm_M_Student_TempEmailId[] stdemail { get; set; }

        public FatherMultipleEmailIdDTO[] FatherMultipleEmailIdDTO { get; set; }
        public FatherMultipleMobileNoDTO[] FatherMultipleMobileNoDTO { get; set; }

        public MotherMultipleEmailIdDTO[] MotherMultipleEmailIdDTO { get; set; }
        public MotherMultipleMobileNoDTO[] MotherMultipleMobileNoDTO { get; set; }

        public AdmCollegeStudentReferenceDTO[] SelectedRefrenceDetails { get; set; }
        public AdmCollegeStudentSourceDTO[] SelectedSourceDetails { get; set; }
        public AdmCollegeStudentPrevSchoolDTO[] SelectedPrevSchoolDetails { get; set; }

        public AdmCollegeStudentGuardianDTO[] SelectedGuardianDetails { get; set; }
        public AdmCollegeStudentSiblingsDetailsDTO[] SelectedSiblingDetails { get; set; }
        public CollegeMasterDocumentDTO[] Uploaded_documentList { get; set; }

        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public Master_NumberingDTO admissionNumbering { get; set; }
        public Adm_College_Student_SubjectMarksDTO[] Adm_College_Student_SubjectMarksTempDTO { get; set; }
        public string courseName { get; set; }
        public string branchName { get; set; }
        public string semesterName { get; set; }
        public string batchName { get; set; }
        public string message { get; set; }
        public long ACAYC_Id { get; set; }
        public long ACAYCB_Id { get; set; }
        public long ACAYCBS_Id { get; set; }
        public string IMC_CasteName { get; set; }
        public string AMCOC_Name { get; set; }
        public string stdmobilenumber { get; set; }

        public string admRegManualFlag { get; set; }
        public string admAdmManualFlag { get; set; }
        public string preventdupRegNo { get; set; }
        public string preventdupAdmNo { get; set; }
        public int duplicateRegNo { get; set; }
        public int duplicateAdmNo { get; set; }
        public int duplicateEmailId { get; set; }
        public int duplicateAdharNo { get; set; }
        public string countryName { get; set; }
        public string SearchColumn { get; set; }
        public string EnteredData { get; set; }
        public int count { get; set; }     
        public string AMCST_PassportNo { get; set; }
        public string AMCST_PassportIssuedAt { get; set; }
        public DateTime? AMCST_PassportIssueDate { get; set; }
        public long? AMCST_PassportIssuedCounrty { get; set; }
        public string AMCST_PassportIssuedPlace { get; set; }
        public DateTime? AMCST_PassportExpiryDate { get; set; }
        public string AMCST_VisaIssuedBy { get; set; }
        public DateTime? AMCST_VISAValidFrom { get; set; }
        public DateTime? AMCST_VISAValidTo { get; set; }
        public bool AMCST_Divyangjan { get; set; }

    }

    public class Adm_College_Student_SMSNoDTO
    {
        public long ACSTSMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_MobileNo { get; set; }
       
       
    }

    public class Adm_College_Student_EmailIdDTO
    {
        public long ACSTE_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_emailId { get; set; }
    }
    public class Adm_M_Student_TempMobileNo
    {
        public long UserName { get; set; }
        public string Role { get; set; }
    }
    public class Adm_M_Student_TempEmailId
    {
        public long UserNameemail { get; set; }
        public string Roleemail { get; set; }

        
    }
    public class FatherMultipleMobileNoDTO
    {
        public long ACSTPMN_Id { get; set; }
        public long? AMCST_FatherMobleNo { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTPMN_Flag { get; set; }
    }
    public class MotherMultipleMobileNoDTO
    {
        public long ACSTPMN_Id { get; set; }
        public long? AMCST_MotherMobleNo { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTPMN_Flag { get; set; }
    }
    public class FatherMultipleEmailIdDTO
    {
        public long ACSTPE_Id { get; set; }
        public string AMCST_FatheremailId { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTPE_Flag { get; set; }
    }
    public class MotherMultipleEmailIdDTO
    {
        public long ACSTPE_Id { get; set; }
        public string AMCST_MotheremailId { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTPE_Flag { get; set; }
    }
    public class Adm_College_Student_SubjectMarksDTO
    {
        public long ACSTSUM_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTSUM_SubjectName { get; set; }
        public decimal ACSTSUM_MaxMarks { get; set; }
        public decimal ACSTSUM_SubjectMarks { get; set; }
        public decimal ACSTSUM_Percentage { get; set; }
        public string ACSTSUM_LangFlg { get; set; }
    }

}
