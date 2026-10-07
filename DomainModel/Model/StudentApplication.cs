using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration")]
    public class StudentApplication : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long pasr_id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
        public DateTime? PASR_Date { get; set; }
        public string PASR_RegistrationNo { get; set; }
        public long AMC_Id { get; set; }
        public string PASR_Sex { get; set; }
        public DateTime PASR_DOB { get; set; }
        public int PASR_Age { get; set; }
        public long ASMCL_Id { get; set; }
        public string PASR_BloodGroup { get; set; }
        public string PASR_MotherTongue { get; set; }
        public long Religion_Id { get; set; }
        public long? CasteCategory_Id { get; set; }
        public long? Caste_Id { get; set; }
        public string PASR_PerStreet { get; set; }
        public string PASR_PerArea { get; set; }
        public string PASR_PerCity { get; set; }
        public string PASR_PerState { get; set; }
        public string PASR_PerCountry { get; set; }
        public int PASR_PerPincode { get; set; }
        public string PASR_ConStreet { get; set; }
        public string PASR_ConArea { get; set; }
        public string PASR_ConCity { get; set; }
        public long PASR_ConState { get; set; }
        public string PASR_ConCountry { get; set; }
        public int PASR_ConPincode { get; set; }
        public long PASR_AadharNo { get; set; }
        public long PASR_MobileNo { get; set; }
        public string PASR_emailId { get; set; }
        public string PASR_MaritalStatus { get; set; }
        public int PASR_FatherAliveFlag { get; set; }
        public string PASR_FatherName { get; set; }
        public long PASR_FatherAadharNo { get; set; }
        public string PASR_FatherSurname { get; set; }
        public string PASR_FatherEducation { get; set; }
        public string PASR_FatherOccupation { get; set; }
        public string PASR_FatherDesignation { get; set; }
        public decimal PASR_FatherIncome { get; set; }
        public long? PASR_FatherMobleNo { get; set; }
        public string PASR_FatheremailId { get; set; }
        public int PASR_MotherAliveFlag { get; set; }
        public string PASR_MotherName { get; set; }
        public long PASR_MotherAadharNo { get; set; }
        public string PASR_MotherSurname { get; set; }
        public string PASR_MotherEducation { get; set; }
        public string PASR_MotherOccupation { get; set; }
        public string PASR_MotherDesignation { get; set; }
        public decimal PASR_MotherIncome { get; set; }
        public long? PASR_MotherMobleNo { get; set; }
        public string PASR_MotheremailId { get; set; }
        public decimal PASR_TotalIncome { get; set; }
        public string PASR_BirthPlace { get; set; }
        public string PASR_Nationality { get; set; }
        public int? PASR_HostelReqdFlag { get; set; }
        public int? PASR_TransportReqdFlag { get; set; }
        public int? PASR_GymReqdFlag { get; set; }
        public int? PASR_ECSFlag { get; set; }
        public int PASR_PaymentFlag { get; set; }
        public int PASR_AmountPaid { get; set; }
        public string PASR_PaymentType { get; set; }
        public DateTime? PASR_PaymentDate { get; set; }
        public string PASR_ReceiptNo { get; set; }
        public int PASR_ActiveFlag { get; set; }
        public string PASR_ApplStatus { get; set; }
        public int PASR_FinalpaymentFlag { get; set; }
        // New Fields
        public int? PASR_UndertakingFlag { get; set; }
        public string PASR_MotherOfficeAddr { get; set; }
        public long? PASR_MotherNationality { get; set; }
        public string PASR_FatherOfficeAddr { get; set; }
        public long? PASR_FatherNationality { get; set; }
        public string PASR_OtherPermanentAddr { get; set; }
        public string PASR_OtherResidential_Addr { get; set; }
        public string PASR_ExtraActivity { get; set; }
        public string PASR_LastPlayGrndAttnd { get; set; }

        public long PAMS_Id { get; set; }
        public long Id { get; set; }


        public long? PASR_AltContactNo { get; set; }
        public string PASR_AltContactEmail { get; set; }

        public string PASR_Student_Pic_Path { get; set; }
        public bool? PASR_Adm_Confirm_Flag { get; set; }


        public string PASR_BirthCertificateNo { get; set; }
        public long? PASL_ID { get; set; }

        public string Remark { get; set; }
        public long Repeat_Class_Id { get; set; }

        public long? FMCC_ID { get; set; }

        public long? PASR_Noofbrothers { get; set; }
        public long? PASR_Noofsisters { get; set; }

        public string PASR_lastclassperc { get; set; }

        public bool? PASR_SibblingConcessionFlag { get; set; }
        public bool? PASR_ParentConcessionFlag { get; set; }
        public long PASRAPS_ID { get; set; }
        public string PASR_DOBWords { get; set; }

      

        public string PASR_ChurchName { get; set; }
        public string PASR_ChurchAddress { get; set; }
        public int? PASR_MotherPassingYear { get; set; }
        public int? PASR_FatherPassingYear { get; set; }

        public string PASR_FatherOfficePhNo { get; set; }

        public string PASR_MotherOfficePhNo { get; set; }
        public string PASR_FatherHomePhNo { get; set; }

        public string PASR_MotherHomePhNo  { get; set; }


        public int? PASR_FatherReligion { get; set; }

        public int? PASR_FatherCaste  { get; set; }

        public int? PASR_MotherReligion  { get; set; }

        public int? PASR_MotherCaste  { get; set; }

        public string PASR_Tribe { get; set; }

        public string PASR_FatherTribe  { get; set; }

        public string PASR_MotherTribe  { get; set; }

        public string PASR_FirstLanguage  { get; set; }

        public string PASR_SecondLanguage  { get; set; }

        public string PASR_Thirdlanguage  { get; set; }

        public string PASR_FatherPhoto  { get; set; }

        public string PASR_MotherPhoto  { get; set; }

        public string PASR_Emisno { get; set; }
        public string PASR_Boarding { get; set; }

        public string PASR_Fathersubcaste { get; set; }

        public string PASR_Mothersubcatse { get; set; }

        public string PASR_Subcaste { get; set; }

        public string PASR_Applicationno { get; set; }

        public string PASR_Taluk { get; set; }

        public string PASR_Distirct { get; set; }

        public string PASR_Village { get; set; }

        public string PASR_Stayingwith  { get; set; }

        public string PASR_Medium { get; set; }

        public string PASR_Languagespeaking { get; set; }

        public string PASR_FatherPanno { get; set; }

        public string PASR_MotherPanno  { get; set; }

        public bool PASA_TransferrableJobFlg { get; set; }
        
        public int PASR_UnderAge { get; set; }

        public int PASR_OverAge   { get; set; }
        public string PASR_AccountNo { get; set; }

        public string PASR_BankName { get; set; }

        public string PASR_BranchName { get; set; }

        public string PASR_IFSCCode { get; set; }

        public string PASR_Domicile { get; set; }
        public string PASR_SchoolDISECode { get; set; }
        public string PASR_MedicalComplaints { get; set; }

        public string PASR_OtherInformations { get; set; }
        public bool PASR_NewlyAdmisstedFlg { get; set; }
        public bool PASR_VaccinatedFlg { get; set; }

        public bool PASR_Tcflag { get; set; }

        public int? PASR_NoOfSiblings { get; set; }
        public int? PASR_NoOfSiblingsSchool { get; set; }
        public int? PASR_NoOfElderBrothers { get; set; }
        public int? PASR_NoOfYoungerBrothers { get; set; }
        public int? PASR_NoOfElderSisters { get; set; }
        public int? PASR_NoOfYoungerSisters { get; set; }
        public int? PASR_NoOfDependencies { get; set; }
    }
}
