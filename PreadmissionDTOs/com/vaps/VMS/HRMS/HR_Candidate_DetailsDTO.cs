using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Candidate_DetailsDTO : CommonParamDTO
    {
        public long HRCD_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMPT_Id { get; set; }
        public long HRMC_Id { get; set; }
       // public long HRME_Id { get; set; }
        public long HRCD_MRFNO { get; set; }
        public string HRCD_FullName { get; set; }
        public string HRCD_FirstName { get; set; }
        public string HRCD_MiddleName { get; set; }
        public string HRCD_LastName { get; set; }
        public long HRMJ_Id { get; set; }
        public string HRCD_Skills { get; set; }
        public DateTime HRCD_DOB { get; set; }
        public long IVRMMG_Id { get; set; }
        public long HRCD_MobileNo { get; set; }
        public string HRCD_EmailId { get; set; }
        public decimal HRCD_ExpFrom { get; set; }
        public decimal HRCD_ExpTo { get; set; }
        public string HRCD_CurrentCompany { get; set; }
        public string HRCD_ResumeSource { get; set; }
        public string HRCD_JobPortalName { get; set; }
        public string HRCD_RefCode { get; set; }
        //public long HRCD_RefCode { get; set; }
        public string HRCDDV_RejectedFlg { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public decimal HRCD_LastCTC { get; set; }
        public decimal HRCD_ExpectedCTC { get; set; }
        public DateTime HRCD_AppDate { get; set; }
        public DateTime? HRCD_InterviewDate { get; set; }
        public DateTime HRCD_ExitDate { get; set; }
        public long HRCD_NoticePeriod { get; set; }
        public string HRCD_Remarks { get; set; }
        public string HRCD_Resume { get; set; }
        public string HRCD_RecruitmentStatus { get; set; }
        public bool HRCD_ActiveFlg { get; set; }
        public long HRCD_CreatedBy { get; set; }
        public long HRCD_UpdatedBy { get; set; }
        public long? HRMCON_Id { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public string applydate { get; set; }
        public string HRCD_Department { get; set; }
        public long HRME_ID { get; set; }
        public bool candidateflage { get; set; }

        public string HRME_RFCardId { get; set; }
        public string retrunMsg { get; set; }
        public string HRMPT_Name { get; set; }
        public string HRMC_QulaificationName { get; set; }
        public string IVRMMG_GenderName { get; set; }
        public string HRMJ_JobCode { get; set; }
        public string HRMJ_JobTiTle { get; set; }

        public Array pagePrivilege { get; set; }
        public Array AllMDHREmpdataVerify { get; set; }
        public Array AllEmp15dayDR { get; set; }
        public Array VMSCandidateList { get; set; }
        public Array MasterPosTypeList { get; set; }
        public Array MasterPositionList { get; set; }
        public Array MasterQualification { get; set; }
        public Array MasterGender { get; set; }
        public Array VMSEditValue { get; set; }
        public Array AllReportTraningEMP { get; set; }
        public Array VMSMRFList { get; set; }
        public Array masterjob { get; set; }
        public Array earningList { get; set; }
        public Array detectionList { get; set; }
        public Array arrearList { get; set; }
        public Array PromotionallEmpList { get; set; }
        public Array grossList { get; set; }
        public Array institutionlist { get; set; }
        public Array candidatedetailsgrid { get; set; }
        public Array employeeEarningsDeductionsDetails { get; set; }
        public HR_Candidate_EarningsDeductionsDTO[] EarningDTO { get; set; }
        public HR_Candidate_EarningsDeductionsDTO[] DeductionDTO { get; set; }
        public HR_Candidate_EarningsDeductionsDTO[] ArrearDTO { get; set; }
        public HR_Candidate_DetailsDTO Employeedto { get; set; }
        public Array CandidateDetails { get; set; }
        public Array companydetails { get; set; }
        public Array MasterEmployeetype { get; set; }
        public Array GroupTypeList { get; set; }
        public Array GradeList { get; set; }
        public Array maritalstatuslist { get; set; }
        public Array castecategorylist { get; set; }
        public Array castelist { get; set; }
        public Array masterreligionlist { get; set; }
        public Array employeedepartmentlist { get; set; }
        public Array employeedesignationlist { get; set; }
        public Array mastercountry { get; set; }
        public Array masterReligion { get; set; }
        public Array masterCaste { get; set; }
        public Array mastermaritalstatus { get; set; }
        public Array companylist { get; set; }
        public Array letterdetails { get; set; }
        public Array cousultencydetail { get; set; }
        public Array saveFOData { get; set; }
        public string cfilepath { get; set; }
        public string HRETF_Remarks { get; set; }
        public string HRETF_Status { get; set; }
        public string HRETF_FileType { get; set; }
        public string cfilename { get; set; }
        public string HRETF_FilePath { get; set; }
        public string HRETF_FileName { get; set; }
        public string HRRemarks { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public Int32? HRMD_Order { get; set; }
        public Array departmenlist { get; set; }
        public Array candidatelist { get; set; }
        public Array AllDataTraningHR { get; set; }

        public Array desgnationlist { get; set; }
        public Array departmentlist { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long HRMDES_Id { get; set; }
        public Int32? HRMDES_Order { get; set; }
        public Array earingdeductionlist { get; set; }
        public Array saveHRData { get; set; }
        public string HRMED_Name { get; set; }
        public string HRMED_AmountPercentFlag { get; set; }
        public long HRMED_Id { get; set; }
        public string percentOff { get; set; }
        public long HRETF_Id { get; set; }

        //SALARY PART
        public long HRMEDT_Id {  get; set; }
        public string HRMED_EDTypeFlag { get; set; }
        public string HRMED_AmountPercent { get; set; }
        public bool HRMED_ActiveFlag { get; set; }
        public string HRMED_RoundOffFlag { get; set; }
        public string HRMED_EarnDedFlag { get; set; }
        public long HRMED_Order { get; set; }
        public long HRCED_Id { get; set; }
        public decimal HRCED_Amount { get; set; }
        public string HRCED_Percentage { get; set; }
        public bool HRCED_ActiveFlag { get; set; }
        //SALARY PART


        public string HRCD_Photo { get; set; }
        public string HRCD_FatherName { get; set; }
        public long? HRCD_AadharNo { get; set; }
        public string HRCD_PAN { get; set; }
        public DateTime? HRCD_JoiningDate { get; set; }
        public string HRCD_Designation { get; set; }
        public long? HRCD_BondDuration { get; set; }
        public string HRCD_AddressLocal { get; set; }
        public string HRCD_AddressPermanent { get; set; }
        public string HRCD_NatureOfWork { get; set; }
        public string HRCD_ScopeOfService { get; set; }
        public string HRCD_SHName { get; set; }
        public string HRCD_SHAddress { get; set; }
        public long? HRCD_SHGender { get; set; }
        public string HRCD_SHGenderName { get; set; }
        public long? HRCD_SHContactNo { get; set; }
        public string HRCD_Place { get; set; }
        public long? HRCD_PINCode { get; set; }
        public long? HRMET_Id { get; set; }
        public long? HRMGT_Id { get; set; }
        public long? HRMG_Id { get; set; }
        public long? IVRMMMS_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public long? IMC_Id { get; set; }
        public long? IVRMMR_Id { get; set; }
        public string HRMET_EmployeeType { get; set; }
        public string HRMGT_EmployeeGroupType { get; set; }
        public string HRMG_GradeName { get; set; }
        public string IVRMMMS_MaritalStatus { get; set; }
        public string IMCC_CategoryName { get; set; }
        public string IMC_CasteName { get; set; }
        public string IVRMMR_Name { get; set; }
        public int? HRMGT_Order { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public DateTime HRCISC_InterviewDateTime { get; set; }
        public string HRCISC_InterviewRounds { get; set; }
        public string HRCISC_InterviewVenue { get; set; }
        public string HRME_employeename { get; set; }
        public string HRCIS_InterviewFeedBack { get; set; }
        public string HRCIS_Status { get; set; }
        public Array interviewlist { get; set; }
        public long casteCategoryId { get; set; }
        public long casteId { get; set; }
        public long religionId { get; set; }
        public long UserId { get; set; }

        public HR_Candidate_QualificationsDTO[] QualificationsDTO { get; set; }
        public HR_Candidate_ExperienceDTO[] ExperienceDTO { get; set; }
        public HR_Candidate_LanguagesDTO[] LanguagesDTO { get; set; }
        public HR_Candidate_FamilyDTO[] FamilyDTO { get; set; }
        public HR_Candidate_DocumentsDTO[] HR_Candidate_DocumentsDTO { get; set; }
        public HR_Candidate_Documents_VerifiedByDTO[] HR_Candidate_Documents_VerifiedByDTO { get; set; }
        public Array CandidateDetailsverfy { get; set; }
        public Array qualificationlist { get; set; }
        public Array experiencelist { get; set; }
        public Array languagelist { get; set; }
        public Array familylist { get; set; }
        public Array AllDataTraningHO { get; set; }

        public long HRCD_NationalityId { get; set; }
        public long HRCD_Religion { get; set; }
        public long HRCD_MaritalStatus { get; set; }
        public string HRCD_BloodGroup { get; set; }
        public long HRCD_CasteId { get; set; }
        public string Template { get; set; }
        public bool welcomenotice { get; set; }
        public bool thanksnotice { get; set; }
        public string HRCIS_CandidateStatus { get; set; }

        public string HRCD_AddressLocal2 { get; set; }
        public string HRCD_AddLocalPlace { get; set; }
        public long HRCD_AddLocalPIN { get; set; }
        public string HRCD_AddressPermanent2 { get; set; }
        public string HRCD_AddPermanentPlace { get; set; }
        public long HRCD_AddPermanentPIN { get; set; }
        public string HRCD_SHAddress2 { get; set; }
        public bool HRCD_DocSubmitFlag { get; set; }
        public string HRCA_FirstDocName { get; set; }
        public string HRCA_SecDocName { get; set; }
        public DateTime? HRCA_FirstDocDate { get; set; }
        public DateTime? HRCA_SecDocDate { get; set; }
        public decimal HRCA_AnnualCTC { get; set; }
        public decimal HRCA_MonthlyCTC { get; set; }
        public string HRCA_AppointmentRefNo { get; set; }
        public string HRCA_AcknowledgementRefNo { get; set; }
        public long HRCA_Id { get; set; }
        public long HRMP_Id { get; set; }
        public string HRMP_Position { get; set; }
        public string candidatetype { get; set; }
        public long? HRCED_CreatedBy { get; set; }
        public long? HRCED_UpdatedBy { get; set; }
        public long? HRCA_CreatedBy { get; set; }
        public long? HRCA_UpdatedBy { get; set; }
        public string HRCD_Status { get; set; }
        public string userMessage { get; set; }
        public string FlagOnLoadOrOnChangeInst { get; set; }      
        public long HRMNS_Id { get; set; }
        public string HRMNS_NatureWork { get; set; }
        public string HRMNS_ScopeService { get; set; }
        public string HRMNS_ApprovedStatus { get; set; }
        public bool HRMNS_Activeflag { get; set; }
        public bool HRMNS_Approvedflag { get; set; }
        public Array get_scopenature { get; set; }
        public bool Flag { get; set; }
        public string HRMNS_Remark { get; set; }

        //Assets mapping
        public Array floordetails { get; set; }
        public Array employeedetails { get; set; }
        public Array employeelist { get; set; }
        public Array locationdetails { get; set; }
        public Array checkoutemployeelist { get; set; }
        public Array Voucher_Details { get; set; }
        public Array Supplier_Details { get; set; }
        public long? INVMLO_Id { get; set; }
        public long? HRMF_Id { get; set; }
        public Array saveAssetsmap { get; set; }

        //Appointments Head
        public string appointmentType { get; set; }
        public string AppoHead { get; set; }
        public string AppoDetails { get; set; }
        public string AL_Id { get; set; }
        public string AL_Remarks { get; set; }
        public string AL_Flg { get; set; }
        public long DFlag { get; set; }
        public string empremark { get; set; }
        public Array SavedAppointHeadMD { get; set; }
        public Array appointmentDetails { get; set; }
        public Array SavedAppointHead { get; set; }
        public Array AllAppointmentLetters { get; set; }
        public Array AllFOHRDailyReport { get; set; }
        public Array SavePromotionEMP { get; set; }
        public long HRTC_ID { get; set; }
        public string Status { get; set; }
        public string HRTAC_Remarks { get; set; }
        public Array SaveApprovalEMP { get; set; }


        public ApprovalList[] approvalList { get; set; }
        public ApprovalHRTranning[] approvalHRTranning { get; set; }
        public HR_Emp_Promotion_Verified[] hr_Emp_Promotion_Verified { get; set; }

        public Array AppointmentCandidateList{ get; set; }
        public class ApprovalList
        {
            public long HRMNS_Id { get; set; }
            public string HRMNS_ApprovedStatus { get; set; }
            public string HRMNS_Remark { get; set; }
        }
        public class ApprovalHRTranning
        {
            public long HRETF_Id { get; set; }
            public long HRME_ID { get; set; }
            public long HRMD_Id { get; set; }
            public string HRETF_Remarks { get; set; }
            public string HRETF_Status { get; set; }
        }
        public class HR_Emp_Promotion_Verified
        {
            public long HRTC_ID { get; set; }
            public string Status { get; set; }
            public string HRTAC_Remarks { get; set; }
            public long HRME_ID { get; set; }
     
        }
    }  
}