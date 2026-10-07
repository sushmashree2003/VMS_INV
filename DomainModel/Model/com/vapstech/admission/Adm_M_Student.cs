using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_M_Student")]
    public class Adm_M_Student : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public long? IC_Id { get; set; }
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
        public string AMST_MotherOfficeAdd { get; set; }
        public string AMST_MotherOccupation { get; set; }
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
        public List<StudentActitvityDMO> activityDMOList { get; set; }
        public List<StudentReferenceDMO> referenceDMOList { get; set; }
        public List<StudentSourceDMO> sourceDMOList { get; set; }
        public List<MasterStudentBondDMO> masterbondDMOList { get; set; }
        public List<StudentSiblingDMO> siblingDMOList { get; set; }
        public StudentAchivementDMO achievementDMO { get; set; }
        public List<StudentPrevSchoolDMO> prevSchoolDMOList { get; set; }
        public List<StudentGuardianDMO> guardianDMOList { get; set; }
        public List<StudentDocumentDMO> documentDMOList { get; set; }
        public List<Adm_M_Student_FatherMobileNo> studentFatherMobileNo { get; set; }
        public List<Adm_Master_Father_Email> studentFatherEmail { get; set; }
        public List<Adm_M_Mother_MobileNo> studentMotherMobileNo { get; set; }
        public List<Adm_M_Mother_Emailid> studentMotherEmailid { get; set; }
        public List<Adm_M_Student_Email_Id> studentemailid { get; set; }
        public List<Adm_M_Student_MobileNo> studentmobileid { get; set; }
        public Adm_Student_EcsDetailsDMO Adm_Student_EcsDetailsDMO { get; set; }
        public string AMST_SubCasteIMC_Id { get; set; }
        public string AMST_FatherReligion { get; set; }
        public string AMST_FatherCaste { get; set; }
        public string AMST_FatherSubCaste { get; set; }
        public string AMST_MotherReligion { get; set; }
        public string AMST_MotherCaste { get; set; }
        public string AMST_MotherSubCaste { get; set; }
        public string AMST_Tpin { get; set; }
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
        public long? ASMST_Id { get; set; }
        public string AMST_SecretCode { get; set; }
        public string AMST_BiometricId { get; set; }
        public string AMST_MOInstruction { get; set; }
        public long? AMST_CreatedBy { get; set; }
        public long? AMST_UpdatedBy { get; set; }
        


    }
}



//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace DomainModel.Model.com.vaps.admission
//{
//    [Table("Adm_M_Student")]
//    public class Adm_M_Student:CommonParamDMO
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        //public long AMST_Id { get; set; }
//        //public long MI_Id { get; set; }
//        //public long ASMAY_Id { get; set; }
//        //public string AMST_FirstName { get; set; }
//        //public string AMST_MiddleName { get; set; }
//        //public string AMST_LastName { get; set; }
//        //public DateTime? AMST_Date { get; set; }
//        //public string AMST_RegistrationNo { get; set; }
//        //public string AMST_AdmNo { get; set; }
//        //public long AMC_Id { get; set; }
//        //public string AMST_Sex { get; set; }
//        //public DateTime AMST_DOB { get; set; }
//        //public string AMST_DOB_Words { get; set; }
//        //public int PASR_Age { get; set; }
//        //public long ASMCL_Id { get; set; }
//        //public string AMST_BloodGroup { get; set; }
//        //public string AMST_MotherTongue { get; set; }
//        //public string AMST_BirthCertNO { get; set; }
//        //public long IVRMMR_Id { get; set; }
//        //public long IMCC_Id { get; set; }
//        //public long IC_Id { get; set; }
//        //public string AMST_PerStreet { get; set; }
//        //public string AMST_PerArea { get; set; }
//        //public string AMST_PerCity { get; set; }
//        //public string AMST_PerAdd3 { get; set; }
//        //public long AMST_PerState { get; set; }
//        //public long AMST_PerCountry { get; set; }
//        //public int AMST_PerPincode { get; set; }
//        //public string AMST_ConStreet { get; set; }
//        //public string AMST_ConArea { get; set; }
//        //public string AMST_ConCity { get; set; }
//        //public long AMST_ConState { get; set; }
//        //public long AMST_ConCountry { get; set; }
//        //public int AMST_ConPincode { get; set; }
//        //public long AMST_AadharNo { get; set; }
//        //public string AMST_StuBankAccNo { get; set; }
//        //public string AMST_StuBankIFSC_Code { get; set; }
//        //public string AMST_StuCasteCertiNo { get; set; }
//        //public long AMST_MobileNo { get; set; }
//        //public string AMST_emailId { get; set; }
//        //public string AMST_FatherAliveFlag { get; set; }
//        //public string AMST_FatherName { get; set; }
//        //public long AMST_FatherAadharNo { get; set; }
//        //public string AMST_FatherSurname { get; set; }
//        //public string AMST_FatherEducation { get; set; }
//        //public string AMST_FatherOccupation { get; set; }
//        //public string AMST_FatherOfficeAdd { get; set; }
//        //public string AMST_FatherDesignation { get; set; }
//        //public decimal AMST_FatherMonIncome { get; set; }
//        //public decimal AMST_FatherAnnIncome { get; set; }
//        //public long AMST_FatherNationality { get; set; }
//        //public long AMST_FatherMobleNo { get; set; }
//        //public string AMST_FatheremailId { get; set; }
//        //public string AMST_FatherBankAccNo { get; set; }
//        //public string AMST_FatherBankIFSC_Code { get; set; }
//        //public string AMST_FatherCasteCertiNo { get; set; }
//        //public string ANST_FatherPhoto { get; set; }
//        //public string AMST_MotherAliveFlag { get; set; }
//        //public string AMST_MotherName { get; set; }
//        //public long AMST_MotherAadharNo { get; set; }
//        //public string AMST_MotherSurname { get; set; }
//        //public string AMST_MotherEducation { get; set; }
//        //public string AMST_MotherOfficeAdd { get; set; }
//        //public string AMST_MotherOccupation { get; set; }
//        //public string AMST_MotherDesignation { get; set; }
//        //public decimal AMST_MotherMonIncome { get; set; }
//        //public decimal AMST_MotherAnnIncome { get; set; }
//        //public long AMST_MotherNationality { get; set; }
//        //public long AMST_MotherMobileNo { get; set; }
//        //public string AMST_MotherEmailId { get; set; }
//        //public string AMST_MotherBankAccNo { get; set; }
//        //public string AMST_MotherBankIFSC_Code { get; set; }
//        //public string AMST_MotherCasteCertiNo { get; set; }
//        //public decimal AMST_TotalIncome { get; set; }
//        //public string ANST_MotherPhoto { get; set; }
//        //public string AMST_BirthPlace { get; set; }
//        //public long AMST_Nationality { get; set; }
//        //public int AMST_BPLCardFlag { get; set; }
//        //public string AMST_BPLCardNo { get; set; }
//        //public int AMST_HostelReqdFlag { get; set; }
//        //public int AMST_TransportReqdFlag { get; set; }
//        //public int AMST_GymReqdFlag { get; set; }
//        //public int AMST_ECSFlag { get; set; }
//        //public int AMST_PaymentFlag { get; set; }
//        //public int AMST_AmountPaid { get; set; }
//        //public string AMST_PaymentType { get; set; }
//        //public DateTime? AMST_PaymentDate { get; set; }
//        //public string AMST_ReceiptNo { get; set; }
//        //public int AMST_ActiveFlag { get; set; }
//        //public string AMST_ApplStatus { get; set; }
//        //public int AMST_FinalpaymentFlag { get; set; }
//        //public string AMST_Photoname { get; set; }
//        //public string AMST_SOL { get; set; }

//        //  public long AMST_Noofbrothers { get; set; }

//        //  public long AMST_Noofsisters { get; set; }




//        public long AMST_Id { get; set; }
//        public long MI_Id { get; set; }
//        public long ASMAY_Id { get; set; }
//        public string AMST_FirstName { get; set; }
//        public string AMST_MiddleName { get; set; }
//        public string AMST_LastName { get; set; }
//        public DateTime AMST_Date { get; set; }
//        public string AMST_RegistrationNo { get; set; }
//        public string AMST_AdmNo { get; set; }
//        public long? AMC_Id { get; set; }
//        public string AMST_Sex { get; set; }
//        public DateTime AMST_DOB { get; set; }
//        public string AMST_DOB_Words { get; set; }
//        public int? PASR_Age { get; set; }
//        public long? ASMCL_Id { get; set; }
//        public string AMST_BloodGroup { get; set; }
//        public string AMST_MotherTongue { get; set; }
//        public string AMST_BirthCertNO { get; set; }
//        public long? IVRMMR_Id { get; set; }
//        public long? IMCC_Id { get; set; }
//        public long? IC_Id { get; set; }
//        public string AMST_PerStreet { get; set; }
//        public string AMST_PerArea { get; set; }
//        public string AMST_PerCity { get; set; }
//        public string AMST_PerAdd3 { get; set; }
//        public long? AMST_PerState { get; set; }
//        public long? AMST_PerCountry { get; set; }
//        public int? AMST_PerPincode { get; set; }
//        public string AMST_ConStreet { get; set; }
//        public string AMST_ConArea { get; set; }
//        public string AMST_ConCity { get; set; }
//        public long? AMST_ConState { get; set; }
//        public long? AMST_ConCountry { get; set; }
//        public int? AMST_ConPincode { get; set; }
//        public long? AMST_AadharNo { get; set; }
//        public string AMST_StuBankAccNo { get; set; }
//        public string AMST_StuBankIFSC_Code { get; set; }
//        public string AMST_StuCasteCertiNo { get; set; }
//        public long AMST_MobileNo { get; set; }
//        public string AMST_emailId { get; set; }
//        public string AMST_FatherAliveFlag { get; set; }
//        public string AMST_FatherName { get; set; }
//        public long? AMST_FatherAadharNo { get; set; }
//        public string AMST_FatherSurname { get; set; }
//        public string AMST_FatherEducation { get; set; }
//        public string AMST_FatherOccupation { get; set; }
//        public string AMST_FatherOfficeAdd { get; set; }
//        public string AMST_FatherDesignation { get; set; }
//        public decimal? AMST_FatherMonIncome { get; set; }
//        public decimal? AMST_FatherAnnIncome { get; set; }
//        public long? AMST_FatherNationality { get; set; }
//        public long? AMST_FatherMobleNo { get; set; }
//        public string AMST_FatheremailId { get; set; }
//        public string AMST_FatherBankAccNo { get; set; }
//        public string AMST_FatherBankIFSC_Code { get; set; }
//        public string AMST_FatherCasteCertiNo { get; set; }
//        public string ANST_FatherPhoto { get; set; }
//        public string AMST_MotherAliveFlag { get; set; }
//        public string AMST_MotherName { get; set; }
//        public long? AMST_MotherAadharNo { get; set; }
//        public string AMST_MotherSurname { get; set; }
//        public string AMST_MotherEducation { get; set; }
//        public string AMST_MotherOfficeAdd { get; set; }
//        public string AMST_MotherOccupation { get; set; }
//        public string AMST_MotherDesignation { get; set; }
//        public decimal? AMST_MotherMonIncome { get; set; }
//        public decimal? AMST_MotherAnnIncome { get; set; }
//        public long? AMST_MotherNationality { get; set; }
//        public long? AMST_MotherMobileNo { get; set; }
//        public string AMST_MotherEmailId { get; set; }
//        public string AMST_MotherBankAccNo { get; set; }
//        public string AMST_MotherBankIFSC_Code { get; set; }
//        public string AMST_MotherCasteCertiNo { get; set; }
//        public decimal? AMST_TotalIncome { get; set; }
//        public string ANST_MotherPhoto { get; set; }
//        public string AMST_BirthPlace { get; set; }
//        public long? AMST_Nationality { get; set; }
//        public int? AMST_BPLCardFlag { get; set; }
//        public string AMST_BPLCardNo { get; set; }
//        public int? AMST_HostelReqdFlag { get; set; }
//        public int? AMST_TransportReqdFlag { get; set; }
//        public int? AMST_GymReqdFlag { get; set; }
//        public int? AMST_ECSFlag { get; set; }
//        public int AMST_PaymentFlag { get; set; }
//        public int AMST_AmountPaid { get; set; }
//        public string AMST_PaymentType { get; set; }
//        public DateTime? AMST_PaymentDate { get; set; }
//        public string AMST_ReceiptNo { get; set; }
//        public int AMST_ActiveFlag { get; set; }
//        public string AMST_ApplStatus { get; set; }
//        public int AMST_FinalpaymentFlag { get; set; }
//        public string AMST_Photoname { get; set; }
//        public string AMST_SOL { get; set; }

//        public long? AMST_Noofbrothers { get; set; }

//        public long? AMST_Noofsisters { get; set; }

//    }
//}
