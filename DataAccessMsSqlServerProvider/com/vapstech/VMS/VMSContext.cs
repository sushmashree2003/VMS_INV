using Microsoft.EntityFrameworkCore;
using System.Linq;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.VMS.Training;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.VMS.Exit;
using DomainModel.Model.com.vapstech.VMS.Sales;
using DomainModel.Model.com.vapstech.SpaceBooking;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.IssueManager.Rating;
using DomainModel.Model.com.vaps.Fee;

namespace DataAccessMsSqlServerProvider
{
    public class VMSContext : DbContext
    {

        public VMSContext(DbContextOptions<VMSContext> options) : base(options)
        {
        }

        public DbSet<ISM_Postsales_enhancement_Elearning_DMO> ISM_Postsales_enhancement_Elearning_DMO { get; set; }
        public DbSet<ISM_Accounts_Team_mapping_DMO> ISM_Accounts_Team_mapping_DMO { get; set; }

        public DbSet<ISM_Module_Pending_DMO> ISM_Module_Pending_DMO { get; set; }

        public DbSet<ISM_HealthCheckup_Assign_DMO> ISM_HealthCheckup_Assign_DMO { get; set; }
        public DbSet<OT_Candidates_Exam_AnswersheetDMO> OT_Candidates_Exam_AnswersheetDMO { get; set; }
        public DbSet<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public DbSet<HRGroupDeptDessgDMO> HRGroupDeptDessgDMO { get; set; }
        public DbSet<HR_Training_Assignment_Questions_DMO> HR_Training_Assignment_Questions_DMO { get; set; }
        public DbSet<HR_Training_Assignment_Option_DMO> HR_Training_Assignment_Option_DMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<ISM_Sales_Lead_Comments_DMO> ISM_Sales_Lead_Comments_DMO_con { get; set; }
        public DbSet<OT_Candidates_Exam_SubjectiveAnswerDMO> OT_Candidates_Exam_SubjectiveAnswerDMO { get; set; }
        public DbSet<OT_Candidates_Exam_AnswerDMO> OT_Candidates_Exam_AnswerDMO { get; set; }
        public DbSet<OT_Candidates_ExamDMO> OT_Candidates_ExamDMO { get; set; }
        public DbSet<OT_QuestionPaperTypeDMO> OT_QuestionPaperTypeDMO { get; set; }
        public DbSet<OT_Master_OE_QNS_OptionsDMO> OT_Master_OE_QNS_OptionsDMO { get; set; }
        public DbSet<OT_Master_OE_QuestionsDMO> OT_Master_OE_QuestionsDMO { get; set; }
        public DbSet<ISM_Client_Project_Docs_DMO> ISM_Client_Project_Docs_DMO_con { get; set; }
        public DbSet<ISM_Client_Project_Master_Docs_DMO> ISM_Client_Project_Master_Docs_DMO_con { get; set; }
        public DbSet<ISM_Client_Project_ManPower_DMO> ISM_Client_Project_ManPower_DMO_con { get; set; }
        public DbSet<ISM_Client_Master_Components_DMO> ISM_Client_Master_Components_DMO_con { get; set; }
        public DbSet<ISM_Client_Project_BOM_DMO> ISM_Client_Project_BOM_DMO_con { get; set; }
        public DbSet<HR_Candidate_DetailsDMO> HR_Candidate_DetailsDMO { get; set; }
        public DbSet<HR_Candidate_Documents_VerifiedByDMO> HR_Candidate_Documents_VerifiedByDMO { get; set; }
        public DbSet<HR_Candidate_DocumentsDMO> HR_Candidate_DocumentsDMO { get; set; }

        public DbSet<HR_Candidate_ExperienceDMO> HR_Candidate_ExperienceDMO { get; set; }
        public DbSet<HR_Candidate_FamilyDMO> HR_Candidate_FamilyDMO { get; set; }
        public DbSet<HR_Candidate_LanguagesDMO> HR_Candidate_LanguagesDMO { get; set; }
        public DbSet<HR_Candidate_QualificationsDMO> HR_Candidate_QualificationsDMO { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }   
        public DbSet<ISM_Sales_Product_ServicesDMO> ISM_Sales_Product_ServicesDMO { get; set; }
        public DbSet<Ism_Sales_Product_Services_ItemsDMO> Ism_Sales_Product_Services_ItemsDMO { get; set; }
        public DbSet<ISM_Sales_BOMDMO> ISM_Sales_BOMDMO { get; set; }
        public DbSet<ISM_Sales_BOMCONFIGDMO> ISM_Sales_BOMCONFIGDMO { get; set; }
        public DbSet<HR_CandidateInterviewScheduleDMO> HR_CandidateInterviewScheduleDMO { get; set; }
        public DbSet<HR_Candidate_InterviewStatusDMO> HR_Candidate_InterviewStatusDMO { get; set; }
        public DbSet<HR_InterviewDMO> HR_InterviewDMO { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYearDMO { get; set; }
        public DbSet<VMS_Live_InterviewDMO> VMS_Live_InterviewDMO { get; set; }
        public DbSet<VMS_Live_Meeting_CandidateOthersDMO> VMS_Live_Meeting_CandidateOthersDMO { get; set; }
        public DbSet<HR_InvitationToCandidateDMO> HR_InvitationToCandidateDMO { get; set; }
        public DbSet<HR_JobApplicationDMO> HR_JobApplicationDMO { get; set; }
        public DbSet<HR_JobDetailsDMO> HR_JobDetailsDMO { get; set; }
        public DbSet<HR_Master_CandidateTypeDMO> HR_Master_CandidateTypeDMO { get; set; }
        public DbSet<HR_Master_JobsDMO> HR_Master_JobsDMO { get; set; }
        public DbSet<HR_Master_NEmployeeDMO> HR_Master_NEmployeeDMO { get; set; }
        public DbSet<HR_Master_PositionDMO> HR_Master_PositionDMO { get; set; }
        public DbSet<HR_Master_PostionTypeDMO> HR_Master_PostionTypeDMO { get; set; }
        public DbSet<HR_Master_PriorityDMO> HR_Master_PriorityDMO { get; set; }
        public DbSet<HR_Master_Qual_CategoryDMO> HR_Master_Qual_CategoryDMO { get; set; }
        public DbSet<ISM_Sales_Lead_MOU_Attachments> ISM_Sales_Lead_MOU_Attachments { get; set; }
        public DbSet<HR_Master_Qual_SubCategoryDMO> HR_Master_Qual_SubCategoryDMO { get; set; }
        public DbSet<HR_Master_Qual_SubjectDMO> HR_Master_Qual_SubjectDMO { get; set; }
        public DbSet<HR_Master_ReferenceDMO> HR_Master_ReferenceDMO { get; set; }
        public DbSet<HR_MRF_ListDMO> HR_MRF_ListDMO { get; set; }
        public DbSet<ISM_Sales_Lead_BOMDMO> ISM_Sales_Lead_BOMDMO { get; set; }
        public DbSet<ISM_Sales_Lead_MOU> ISM_Sales_Lead_MOU { get; set; }
        public DbSet<HR_MRFJOB_DetailsDMO> HR_MRFJOB_DetailsDMO { get; set; }
        public DbSet<HR_MRFRequisitionDMO> HR_MRFRequisitionDMO { get; set; }
        public DbSet<HR_Master_CourseDMO> HR_Master_Course { get; set; }
        public DbSet<State> IVRM_Master_State { get; set; }
        public DbSet<DistrictDMO> DistrictDMO { get; set; }
        public DbSet<City> IVRM_Master_CityDMO { get; set; }
        public DbSet<Country> IVRM_Master_Country { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<UserLogin> userLogin { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        public DbSet<ApplicationUserDMO> ApplicationUserDMO { get; set; }
        public DbSet<HR_Master_LocationDMO> HR_Master_LocationDMO { get; set; }
        public DbSet<IVRM_Master_Gender> IVRM_Master_Gender { get; set; }
        public DbSet<ISM_Sales_Lead_InchargesDMO> ISM_Sales_Lead_InchargesDMO { get; set; }
        public DbSet<HR_Master_EarningsDeductions> HR_Master_EarningsDeductions { get; set; }
        public DbSet<HR_Master_EarningsDeductionsPer> HR_Master_EarningsDeductionsPer { get; set; }
        public DbSet<HR_Candidate_EarningsDeductionsDMO> HR_Candidate_EarningsDeductionsDMO { get; set; }
        public DbSet<HR_Employee_EarningsDeductions> HR_Employee_EarningsDeductions { get; set; }
        public DbSet<HR_Master_DepartmentCode_HeadDMO> HR_Master_DepartmentCode_HeadDMO { get; set; }
        public DbSet<ISM_Sales_Master_Source_DMO> ISM_Sales_Master_Source_DMO { get; set; }       
        public DbSet<ISM_CA_Demo_RatingsDMO> ISM_CA_Demo_RatingsDMO { get; set; }
        public DbSet<Month> month { get; set; }
        public DbSet<SecurityDetailsDMO> SecurityDetailsDMO { get; set; }
        public DbSet<SecurityRoasterDMO> SecurityRoasterDMO { get; set; }
        public DbSet<HR_Master_DepartmentCodeDMO> HR_Master_DepartmentCodeDMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }
        public DbSet<MasterEmployee> HR_Master_Employee_DMO { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<ISM_CA_StudentStrength_MappingDMO> ISM_CA_StudentStrength_MappingDMO { get; set; }     
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Mastert_ConsultancyDMO> Mastert_ConsultancyDMO { get; set; }
        //public DbSet<ApplUser> ApplUser { get; set; }  
        public DbSet<ISM_Master_Client_DMO> ISM_Master_Client_DMO { get; set; }
        public DbSet<SMSEmailSetting> SMSEmailSetting { get; set; }
        public DbSet<Staff_User_Login> IVRM_Staff_User_Login { get; set; }
        public DbSet<VMS_CallLetter_sentDMO> VMS_CallLetter_sentDMO { get; set; }
        public DbSet<HR_Candidate_AppointmentDMO> HR_Candidate_AppointmentDMO { get; set; }
        public DbSet<Master_Employee_Qulaification> Master_Employee_Qulaification_con { get; set; }
        public DbSet<MasterEmployee> Hr_Master_Employee_con { get; set; }
        public DbSet<HR_Training_Employee_DMO> HR_Training_Employee_DMO_con { get; set; }
        public DbSet<HR_Training_Status_DMO> HR_Training_Status_DMO_con { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login_con { get; set; }
        public DbSet<HR_Master_EmployeeType> HR_Master_EmployeeType { get; set; }
        public DbSet<HR_Master_GroupTypeDMO> HR_Master_GroupTypeDMO { get; set; }
        public DbSet<HR_Master_Grade> HR_Master_Grade { get; set; }
        public DbSet<IVRM_Master_Marital_Status> IVRM_Master_Marital_Status { get; set; }
        public DbSet<castecategoryDMO> castecategoryDMO { get; set; }
        public DbSet<mastercasteDMO> mastercasteDMO { get; set; }
        public DbSet<MasterReligionDMO> MasterReligionDMO { get; set; }
        public DbSet<HR_PROCESSDMO> HR_PROCESSDMO { get; set; }
        public DbSet<HR_Process_Auth_OrderNoDMO> HR_Process_Auth_OrderNoDMO { get; set; }
        public DbSet<HR_Master_Amenities_DMO> HR_Master_Amenities_DMO { get; set; }
        public DbSet<HR_Master_Room_FilesDMO> HR_Master_Room_FilesDMO { get; set; }
        public DbSet<HR_Master_Room_AmenitiesDMO> HR_Master_Room_AmenitiesDMO { get; set; }
        public DbSet<HR_Master_Room_ContactsDMO> HR_Master_Room_ContactsDMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<Institution_Module_Page> Institution_Module_Page { get; set; }
        public DbSet<Institution_Module> Institution_Module { get; set; }
        public DbSet<MasterPage> MasterPage { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }
        public DbSet<Master_Employee_Documents> Master_Employee_Documents { get; set; }
        public DbSet<HR_Training_Assignment_Question_Option_DMO> HR_Training_Assignment_Question_Option_DMO { get; set; }//added by praveenkumar
        public DbSet<HR_Training_Assignment_Topic_DMO> HR_Training_Assignment_Topic_DMO { get; set; }//added by praveenkumar
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        public DbSet<GeneralConfigDMO> GeneralConfigDMO { get; set; }
        public DbSet<HR_Master_Building> HR_Master_Building_con { get; set; }
        public DbSet<HR_Master_Floor> HR_Master_Floor_con { get; set; }
        public DbSet<HR_Master_Room> HR_Master_Room_con { get; set; }
        public DbSet<HR_Training_Create_DMO> HR_Training_Create_DMO_con { get; set; }
        public DbSet<HR_Master_External_Trainer_Creation_DMO> HR_Master_External_Trainer_Creation_DMO_con { get; set; }
        public DbSet<HR_Training_Create_Participants_DMO> HR_Training_Create_Participants_DMO_con { get; set; }
        public DbSet<HR_Training_Create_IntTrainer_DMO> HR_Training_Create_IntTrainer_DMO_con { get; set; }
        public DbSet<HR_Training_Create_ExtTrainer_DMO> HR_Training_Create_ExtTrainer_DMO_con { get; set; }
        public DbSet<HR_Master_Feedback_Qns_DMO> HR_Master_Feedback_Qns_DMO_con { get; set; }
        public DbSet<HR_Master_Feedback_Option_DMO> HR_Master_Feedback_Option_DMO_con { get; set; }
        public DbSet<HR_Master_Question_Option_DMO> HR_Master_Question_Option_DMO_con { get; set; }
        public DbSet<HR_Training_Question_DMO> HR_Training_Question_DMO_con { get; set; }
        public DbSet<HR_Training_Feedback_DMO> HR_Training_Feedback_DMO_con { get; set; }
        public DbSet<IVRM_Training_TransactionDMO> IVRM_Training_TransactionDMO { get; set; }
        public DbSet<IVRM_Training_MasterTrainerDMO> IVRM_Training_MasterTrainerDMO { get; set; }      
        public DbSet<ISM_Resignation_ChecKLists_DMO> ISM_Resignation_ChecKLists_DMO_con { get; set; }
        public DbSet<ISM_Resignation_DMO> ISM_Resignation_DMO_con { get; set; }
        public DbSet<ISM_Resignation_ApprovedBy_DMO> ISM_Resignation_ApprovedBy_DMO { get; set; }
        public DbSet<ISM_Resignation_Master_CheckLists_DMO> ISM_Resignation_Master_CheckLists_DMO_con { get; set; }
        public DbSet<ISM_Resignation_Master_Reasons_DMO> ISM_Resignation_Master_Reasons_DMO_con { get; set; }
        public DbSet<ISM_Resignation_RelievingLetter_DMO> ISM_Resignation_RelievingLetter_DMO_con { get; set; }
        public DbSet<ISM_Resignation_Master_CheckLists_AppAuthority_DMO> ISM_Resignation_Master_CheckLists_AppAuthority_DMO { get; set; }
        public DbSet<ISM_Resignation_ChecKLists_ApprovedBy_DMO> ISM_Resignation_ChecKLists_ApprovedBy_DMO { get; set; }
        public DbSet<ISM_Certificate_RequestDMO> ISM_Certificate_RequestDMO { get; set; }
        public DbSet<ISM_Certificate_ApprovalDMO> ISM_Certificate_ApprovalDMO { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation_con { get; set; }
        public DbSet<ISM_Sales_Master_ChannelPartnerDMO> ISM_Sales_Master_ChannelPartnerDMO { get; set; }
        public DbSet<ISM_Sales_Master_ChannelPartner_DocumentsDMO> ISM_Sales_Master_ChannelPartner_DocumentsDMO_con { get; set; }
        public DbSet<ISM_Sales_Master_Product_DMO> ISM_Sales_Master_Product_DMO_con { get; set; }
        public DbSet<ISM_Sales_Master_Category_DMO> ISM_Sales_Lead_TargetMapping_DMO { get; set; }
        public DbSet<ISM_Sales_Lead_TargetMapping_DMO> ISM_Sales_Lead_TargetMapping_DMONew { get; set; }       
        public DbSet<ISM_Sales_Master_Category_DMO> ISM_Sales_Master_Category_DMO_con { get; set; }
        public DbSet<ISM_Sales_Master_Source_DMO> ISM_Sales_Master_Source_DMO_con { get; set; }
        public DbSet<ISM_Sales_Master_Status_DMO> ISM_Sales_Master_Status_DMO_con { get; set; }
        public DbSet<ISM_Sales_Lead_DMO> ISM_Sales_Lead_DMO_con { get; set; }
        public DbSet<ISM_Sales_Lead_EmailId_mobileNoDMO> ISM_Sales_Lead_EmailId_mobileNoDMO { get; set; }
        public DbSet<ISM_Client_Project_Payment_LeadDMO> ISM_Client_Project_Payment_LeadDMO { get; set; }       
        public DbSet<ISM_Sales_Lead_Products_DMO> ISM_Sales_Lead_Products_DMO_con { get; set; }
        public DbSet<ISM_Sales_Lead_Demo_DMO> ISM_Sales_Lead_Demo_DMO_con { get; set; }
        public DbSet<ISM_Sales_Lead_Demo_Products_DMO> ISM_Sales_Lead_Demo_Products_DMO_con { get; set; }
        public DbSet<HR_Master_TrainingTopicDMO> HR_Master_TrainingTopicDMO { get; set; }
        public DbSet<HR_Master_External_TrainingType_Topic_MaterialsDMO> HR_Master_External_TrainingType_Topic_MaterialsDMO { get; set; }
        public DbSet<OT_Master_OE_QNS_Options_FilesDMO> OT_Master_OE_QNS_Options_FilesDMO { get; set; }
        public DbSet<OT_Master_OE_Questions_FilesDMO> OT_Master_OE_Questions_FilesDMO { get; set; }
        public DbSet<OT_Master_ComplexitiesDMO> OT_Master_ComplexitiesDMO { get; set; }
        public DbSet<ISM_Sales_Lead_TemplateDMO> ISM_Sales_Lead_TemplateDMO_con { get; set; }
        public DbSet<ISM_Sales_Lead_SciencePark_BootDMO> ISM_Sales_Lead_SciencePark_BootDMO { get; set; }

        public DbSet<ISM_SciencePark_GoldAndSilver_Package_TransactionDMO> ISM_SciencePark_GoldAndSilver_Package_TransactionDMO { get; set; }

        public DbSet<ISM_SciencePark_TransactionDMO> ISM_SciencePark_TransactionDMO { get; set; }
        public DbSet<ISM_Sales_Multiple_TransactionDMO> ISM_Sales_Multiple_TransactionDMO { get; set; }



        public DbSet<ISM_Sales_Lead_CommentsFilesDMO> ISM_Sales_Lead_CommentsFilesDMO { get; set; }
        public DbSet<MastersProject_DMO> MastersProject_DMO { get; set; }
        public DbSet<HR_Master_LeaveYearDMO> HR_Master_LeaveYearDMO { get; set; }
        public DbSet<INV_Master_SupplierDMO> INV_Master_SupplierDMO { get; set; }
        public DbSet<COE_Master_EventsDMO> COE_Master_EventsDMO { get; set; }
        public DbSet<COE_EventsDMO> COE_EventsDMO { get; set; }
        public DbSet<Rating_Master_TypeDMO> Rating_Master_TypeDMO { get; set; }
        public DbSet<Ratings_Master_QuestionsDMO> Ratings_Master_QuestionsDMO { get; set; }
        public DbSet<Rating_Type_QuestionsDMO> Rating_Type_QuestionsDMO { get; set; }
        public DbSet<ISM_Sales_Lead_TargetMapping_DMO> ISM_Sales_Lead_TargetMappingDMO { get; set; } //Added by Prakash       //MOU
        public DbSet<FeeInstallmentDMO> FeeInstallmentDMO { get; set; }
        public DbSet<FeeInstallmentsyearlyDMO> FeeInstallmentsyearlyDMO { get; set; }
        public DbSet<ISM_Sales_Lead_RawdataDMO> ISM_Sales_Lead_RawdataDMO { get; set; }        
         public DbSet<ISM_Sales_Lead_Negotiation_AuthorisationDMO> ISM_Sales_Lead_Negotiation_AuthorisationDMO { get; set; }
        public DbSet<ISM_Sales_Lead_Incentive_DeptDMO> ISM_Sales_Lead_Incentive_DeptDMO { get; set; }
        public DbSet<ISM_Sales_Lead_Negaotion_DesignationDMO> ISM_Sales_Lead_Negaotion_DesignationDMO { get; set; }
        public DbSet<ISM_Master_Module_BOMDMO> ISM_Master_Module_BOMDMO { get; set; }
        public DbSet<Master_DRBlocked_ScenarioWiseDMO> Master_DRBlocked_ScenarioWiseDMO { get; set; }
        public DbSet<ISM_Master_SciencePark_VendorCostDMO> ISM_Master_SciencePark_VendorCostDMO { get; set; }

        public DbSet<ISM_Sales_Lead_Demo_SwappingDMO> ISM_Sales_Lead_Demo_SwappingDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            //  builder.Entity<Staff_User_Login>().HasKey(m => m.IVRMSTAUL_Id);
            // builder.Entity<HR_Master_EmployeeType>().ToTable("HR_Master_EmployeeType");
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<HR_Candidate_DetailsDMO>();


            return base.SaveChanges();
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
