using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking.AssetTag;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.HWHealthCheckUp;
using DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity;
using DomainModel.Model.com.vapstech.IssueManager.Rating;
using DomainModel.Model.com.vapstech.IssueManager.TADA;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using DomainModel.Model.com.vapstech.IssueManager.VersionControl;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.Portals.Tenants;
using DomainModel.Model.com.vapstech.SpaceBooking;
using DomainModel.Model.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using DomainModel.Model.com.vapstech.VMS.Legal;
using DomainModel.Model.com.vapstech.VMS.Sales;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.IssueManager
{
    public class IssueManagerContext : DbContext
    {
        public IssueManagerContext(DbContextOptions<IssueManagerContext> options) : base(options)
        {
            { Database.SetCommandTimeout(900000000); }
        }

        //============================================= Master   

        public DbSet<SMS_DETAILS_DMO> SMS_DETAILS_DMO { get; set; }
        public DbSet<HR_Planner_Management_FileDMO> HR_Planner_Management_FileDMO { get; set; }
        public DbSet<HR_Planner_Management_ApprovalDMO> HR_Planner_Management_ApprovalDMO { get; set; }
        public DbSet<HR_Master_Floor> HR_Master_Floor { get; set; }
        public DbSet<ivrm_email_sentbox> ivrm_email_sentbox { get; set; }
        public DbSet<IVRM_sms_sentBoxDMO> IVRM_sms_sentBoxDMO { get; set; }
        public DbSet<Legal_CaseDetailsDMO> Legal_CaseDetailsDMO { get; set; }
        public DbSet<Legal_CaseStatusDMO> Legal_CaseStatusDMO { get; set; }
        public DbSet<Legal_AdvocateDMO> Legal_AdvocateDMO { get; set; }
        public DbSet<Legal_ActsDMO> Legal_ActsDMO { get; set; }
        public DbSet<Legal_CaseHistoryDMO> Legal_CaseHistoryDMO { get; set; }
        public DbSet<Legal_FinalOrderDMO> Legal_FinalOrderDMO { get; set; }
        public DbSet<Legal_DocumentsDMO> Legal_Documents { get; set; }
        public DbSet<ISM_DailyReportReapprovalDMO> ISM_DailyReportReapprovalDMO { get; set; }
        public DbSet<VMS_TADA_Master_TierDMO> VMS_TADA_Master_TierDMO { get; set; }
        
        public DbSet<VMS_Live_Meeting_StaffOthersDMO> VMS_Live_Meeting_StaffOthersDMO_con { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<VMS_Live_MeetingDMO> VMS_Live_MeetingDMO_con { get; set; }
        public DbSet<ApplRole> ApplicationRole_con { get; set; }
        public DbSet<ISM_Master_TaskCategory_DMO> ISM_Master_TaskCategory_DMO_con { get; set; }
        public DbSet<PN_Sent_Details_DMO> PN_Sent_Details_DMO_con { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole_con { get; set; }
        public DbSet<HR_Planner_ManagementDMO> HR_Planner_ManagementDMO { get; set; }
        public DbSet<TenantElectricalWaterBillDMO> TenantElectricalWaterBillDMO { get; set; }
        public DbSet<Tenant_Monthly_WaterBillsDMO> Tenant_Monthly_WaterBillsDMO { get; set; }
        public DbSet<ISM_DailyReport_Feedback_Send_DMO> ISM_DailyReport_Feedback_Send_DMO_con { get; set; }
        public DbSet<ISM_Master_TaskCategory_Employee_DMO> ISM_Master_TaskCategory_Employee_DMO { get; set; }
        public DbSet<ISM_DailyReport_Feedback_Receive_DMO> ISM_DailyReport_Feedback_Receive_DMO_con { get; set; }
        public DbSet<ISM_DailyReport_FeedBack_Transaction_DMO> ISM_DailyReport_FeedBack_Transaction_DMO_con { get; set; }
        public DbSet<ISM_DailyReport_FeedBack_DMO> ISM_DailyReport_FeedBack_DMO_con { get; set; }
        public DbSet<ISM_Weekly_Deviation_DMO> ISM_Weekly_Deviation_DMO_con { get; set; }
        public DbSet<VMS_TADA_Advance_Apply_CityDMO> VMS_TADA_Advance_Apply_CityDMO { get; set; }
        public DbSet<VMS_TADA_Apply_CityDMO> VMS_TADA_Apply_CityDMO { get; set; }
        public DbSet<ISM_CallCenter_CLientFeedbackDMO> ISM_CallCenter_CLientFeedbackDMO { get; set; }
        
        public DbSet<MastersProject_DMO> MastersProject_DMO { get; set; }
        public DbSet<MasterVenues_DMO> MasterVenues_DMO { get; set; }
        public DbSet<ISM_Task_Advance_Planner_DatesDMO> ISM_Task_Advance_Planner_DatesDMO { get; set; }
        public DbSet<ISM_Task_Advance_PlannerDMO> ISM_Task_Advance_PlannerDMO { get; set; }
        public DbSet<MasterActivities_DMO> MasterActivities_DMO { get; set; }
        public DbSet<ISM_Master_MeetingRoom_DMO> ISM_Master_MeetingRoom_DMO { get; set; }
        public DbSet<MasterStatus_DMO> MasterStatus_DMO { get; set; }
        public DbSet<ISM_PUSH_NOTIFICATION_USERDMO> ISM_PUSH_NOTIFICATION_USERDMO { get; set; }
        public DbSet<ISM_Master_Client_DMO> clientTable { get; set; }
        public DbSet<LNC_Master_Case_TypeDMO> LNC_Master_Case_TypeDMO { get; set; }
        public DbSet<LNC_Court_Casetype_MappingDMO> LNC_Court_Casetype_MappingDMO { get; set; }
        public DbSet<ISM_Master_TaskCategory_DMO> ISM_Master_TaskCategory_DMO { get; set; }
        public DbSet<ISM_Master_Client_IEMapping_DMO> clientMappingTable { get; set; }
        public DbSet<ISM_Master_Module_DevelopersDMO> ISM_Master_Module_DevelopersDMO { get; set; }
        public DbSet<ISM_Master_Client_UserMappingDMO> ISM_Master_Client_UserMappingDMO { get; set; }
        public DbSet<ISM_User_Employees_MappingDMO> ISM_User_Employees_MappingDMO { get; set; }
        public DbSet<ISM_NotificationsDMO> ISM_NotificationsDMO { get; set; }
        public DbSet<ISM_Notification_Department_MappingDMO> ISM_Notification_Department_MappingDMO { get; set; }
        public DbSet<ISM_Master_Client_ProjectDMO> ISM_Master_Client_ProjectDMO { get; set; }
        public DbSet<ISM_RoleDeptMappingDMO> ISM_RoleDeptMappingDMO { get; set; }
        public DbSet<VMS_Complaint_Register_DMO> VMS_Complaint_Register_DMO { get; set; }
        public DbSet<ISM_EMPLOYEE_MEMO_NOTICEDMO> ISM_EMPLOYEE_MEMO_NOTICE { get; set; }
        public DbSet<ISM_EMPLOYEE_MEMO_NOTICE_TASKS> ISM_EMPLOYEE_MEMO_NOTICE_TASKS { get; set; }
        public DbSet<ISM_Master_TaskGroupDMO> ISM_Master_TaskGroupDMO { get; set; }
        public DbSet<ISM_Master_TaskGroup_DeptDMO> ISM_Master_TaskGroup_DeptDMO { get; set; }
        public DbSet<ISM_Master_ClientProject_PaymentDMO> ISM_Master_ClientProject_PaymentDMO { get; set; }
        public DbSet<ISM_Master_Implementation_PlannerDMO> ISM_Master_Implementation_PlannerDMO { get; set; }
        public DbSet<ISM_Implementation_PlannerDMO> ISM_Implementation_PlannerDMO { get; set; }
        public DbSet<ISM_Driver_IndentDMO> ISM_Driver_IndentDMO { get; set; }
        public DbSet<ISM_Driver_Indent_ApprovalDMO> ISM_Driver_Indent_ApprovalDMO { get; set; }
        public DbSet<Master_VehicleDMO> Master_VehicleDMO { get; set; }
        public DbSet<VMS_Laptop_Application_DMO> VMS_Laptop_Application_DMO { get; set; }
        public DbSet<VMS_Laptop_Application_Authorisation_DMO> VMS_Laptop_Application_Authorisation_DMO { get; set; }
        public DbSet<ISM_Invoice_FilesDMO> ISM_Invoice_FilesDMO { get; set; }
        
        //====================================== Transcation
        //added
        public DbSet<ISM_PrinterStatisticsDMO> ISM_PrinterStatisticsDMO { get; set; }
        //
        public DbSet<HR_Training_Create_IntTrainer_DMO> HR_Training_Create_IntTrainer_DMO_con { get; set; }
        public DbSet<HR_Training_Create_Participants_DMO> HR_Training_Create_Participants_DMO_con { get; set; }
        public DbSet<ISM_TaskCreationDMO> ISM_TaskCreationDMO { get; set; }
        public DbSet<ISM_TaskCreation_AttachmentDMO> ISM_TaskCreation_AttachmentDMO { get; set; }
        public DbSet<ISM_TaskCreation_ResponseDMO> ISM_TaskCreation_ResponseDMO { get; set; }
        public DbSet<ISM_TaskCreation_AssignedToDMO> ISM_TaskCreation_AssignedToDMO { get; set; }
        public DbSet<ISM_TaskCreation_ReOpenDetailsDMO> ISM_TaskCreation_ReOpenDetailsDMO { get; set; }
        public DbSet<ISM_TaskCreation_TransferredToDMO> ISM_TaskCreation_TransferredToDMO { get; set; }
        public DbSet<ISM_TaskCreation_ClientDMO> ISM_TaskCreation_ClientDMO { get; set; }
        public DbSet<ISM_Task_PlannerDMO> ISM_Task_PlannerDMO { get; set; }
        public DbSet<ISM_Task_Planner_TasksDMO> ISM_Task_Planner_TasksDMO { get; set; }
        public DbSet<ISM_Task_Planner_ApprovedDMO> ISM_Task_Planner_ApprovedDMO { get; set; }
        public DbSet<ISM_Task_Regenarate_RemarksDMO> ISM_Task_Regenarate_RemarksDMO { get; set; }
        public DbSet<ISM_Task_Planner_Approved_TasksDMO> ISM_Task_Planner_Approved_TasksDMO { get; set; }
        public DbSet<ISM_InteractionsDMO> ISM_InteractionsDMO { get; set; }
        public DbSet<ISM_Interactions_DetailsDMO> ISM_Interactions_DetailsDMO { get; set; }
        public DbSet<ISM_DailyReportExtensionDMO> ISM_DailyReportExtensionDMO { get; set; }
        public DbSet<ISM_PlannerExtensionDMO> ISM_PlannerExtensionDMO { get; set; }
        public DbSet<ISM_EmployeeWise_MonthEnd_DetailsDMO> ISM_EmployeeWise_MonthEnd_DetailsDMO { get; set; }
        public DbSet<VMS_Master_MonthEndDepartmentDMO> VMS_Master_MonthEndDepartmentDMO { get; set; }
        public DbSet<VMS_EmpWiseMonthEndRemarksDMO> VMS_EmpWiseMonthEndRemarksDMO { get; set; }

        //=============================================== Common DMO
        public DbSet<HR_Master_DepartmentCode_HeadDMO> HR_Master_DepartmentCode_Head { get; set; }
        public DbSet<AcademicYear> AcademicYear { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }

        public DbSet<HR_Master_DepartmentCodeDMO> HR_Master_DepartmentCodeDMO { get; set; }
        public DbSet<MastersPriorities_DMO> MastersPriorities_DMO { get; set; }
        public DbSet<MastersModule_DMO> MastersModule_DMO { get; set; }
        public DbSet<workingDays_DMO> workingDays_DMO { get; set; }
        public DbSet<FODayNameDMO> masterday { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<HRGroupDeptDessgDMO> HRGroupDeptDessgDMO { get; set; }
        public DbSet<MasterModules> MasterModules { get; set; }
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<FO_Master_HolidayWorkingDay_DatesDMO> FO_Master_HolidayWorkingDay_DatesDMO { get; set; }
        public DbSet<SMSEmailSetting> smsEmailSetting { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<HR_Master_Grade> HR_Master_Grade { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }

        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public DbSet<Institution_Module> Institution_Module { get; set; }
        public DbSet<Institution_Module_Page> Institution_Module_Page { get; set; }
        public DbSet<ISM_DailyReportGenerationDMO> ISM_DailyReportGenerationDMO { get; set; }
        public DbSet<Institution_EmailId> Institution_EmailId { get; set; }
        public DbSet<ISM_Email_Department_MappingDMO> ISM_Email_Department_MappingDMO { get; set; }
        public DbSet<HR_Master_LeaveYearDMO> HR_Master_LeaveYearDMO { get; set; }
        public DbSet<UserRoleWithInstituteDMO> IVRM_User_Login_InstitutionwiseDMO { get; set; }
        public DbSet<ISM_Departmentwise_RemarksDMO> ISM_Departmentwise_RemarksDMO { get; set; }
        public DbSet<ISM_Task_Planner_DeviationDMO> ISM_Task_Planner_DeviationDMO { get; set; }
        public DbSet<ISM_Block_EmployeeDMO> ISM_Block_EmployeeDMO { get; set; }
        public DbSet<HR_Emp_Leave_StatusDMO> HR_Emp_Leave_StatusDMO { get; set; }
        public DbSet<HR_Master_Leave_DMO> HR_Master_Leave_DMO { get; set; }
        public DbSet<HolidayWorkingDayTypeDMO> HolidayWorkingDayTypeDMO { get; set; }
        public DbSet<FO_Emp_PunchDMO> FO_Emp_PunchDMO { get; set; }
        public DbSet<FO_Emp_Punch_DetailsDMO> FO_Emp_Punch_Details { get; set; }
        public DbSet<IVRM_Month_DMO> IVRM_Month_DMO { get; set; }
        public DbSet<ApplUser> ApplicationUser { get; set; }
        public DbSet<COE_Master_EventsDMO> COE_Master_EventsDMO { get; set; }
        public DbSet<COE_EventsDMO> COE_EventsDMO { get; set; }
        public DbSet<HR_Emp_Leave_Trans_DMO> HR_Emp_Leave_Trans_DMO { get; set; }
        public DbSet<MobileApplAuthenticationDMO> MobileApplAuthenticationDMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<FeeInstallmentDMO> FeeInstallmentDMO { get; set; }
        public DbSet<FeeInstallmentsyearlyDMO> FeeInstallmentsyearlyDMO { get; set; }
        public DbSet<IVRM_ModeOfPayment> IVRM_ModeOfPayment { get; set; }
        public DbSet<ISM_Client_Project_Payment_DetailsDMO> ISM_Client_Project_Payment_DetailsDMO { get; set; }
        public DbSet<ISM_Client_Payment_ConfigurationDMO> ISM_Client_Payment_ConfigurationDMO { get; set; }
        public DbSet<ISM_DailyReportNotSent_DateWiseRemarksDMO> ISM_DailyReportNotSent_DateWiseRemarksDMO { get; set; }
        public DbSet<ISM_DailyReport_ApprovalDMO> ISM_DailyReport_ApprovalDMO { get; set; }
        public DbSet<ISM_Master_TaskCategory_ChecklistDMO> ISM_Master_TaskCategory_ChecklistDMO_con { get; set; }
        public DbSet<ISM_DailyReport_FilesDMO> ISM_DailyReport_FilesDMO { get; set; }
        public DbSet<ISM_DailyReport_CommentsDMO> ISM_DailyReport_CommentsDMO { get; set; }
        public DbSet<ISM_PDpnt_SDpnt_MappingDMO> ISM_PDpnt_SDpnt_Mapping_DMO { get; set; }
        public DbSet<ISM_DailyReport_DaywiseDMO> ISM_DailyReport_DaywiseDMO { get; set; }
        public DbSet<ISM_Proforma_Invoice_TaxDMO> ISM_Proforma_Invoice_TaxDMO { get; set; }
        public DbSet<ISM_Proforma_Invoice_DetailsDMO> ISM_Proforma_Invoice_DetailsDMO { get; set; }
        public DbSet<ISM_Proforma_InvoiceDMO> ISM_Proforma_InvoiceDMO { get; set; }
        public DbSet<ISM_Client_Master_Components_DMO> ISM_Client_Master_Components_DMO { get; set; }
        public DbSet<ISM_DuplicateTask_Save_RemarksDMO> ISM_DuplicateTask_Save_RemarksDMO { get; set; }        

        public DbSet<ISM_Client_Project_BOM_DMO> ISM_Client_Project_BOM_DMO { get; set; }
        public DbSet<INV_Master_TaxDMO> INV_Master_TaxDMO { get; set; }
        public DbSet<ISM_Invoice_TaxDMO> ISM_Invoice_TaxDMO { get; set; }
        public DbSet<ISM_Invoice_DetailsDMO> ISM_Invoice_DetailsDMO { get; set; }
        public DbSet<ISM_InvoiceDMO> ISM_InvoiceDMO { get; set; }
        public DbSet<Institution_Phone_No> Institution_Phone_No { get; set; }
        //==============================================TADA

        public DbSet<TADA_AdavanceApply_PaymentDMO> TADA_AdavanceApply_PaymentDMO { get; set; }
        public DbSet<VMS_TADA_Advance_Apply_AuthorisationDMO> TADA_Advance_Apply_AuthorisationDMO { get; set; }
        public DbSet<TADA_CityMappingDMO> TADA_CityMappingDMO { get; set; }
        public DbSet<TADA_AddvanceApplyDMO> TADA_AdavanceApplyDMO { get; set; }
        public DbSet<City> IVRM_Master_CityDMO { get; set; }
        public DbSet<State> IVRM_Master_StateDMO { get; set; }
        public DbSet<TADA_AddvanceApplyDetailsDMO> TADA_AdvanceApplyDetailsDMO { get; set; }

        public DbSet<TADAApply_PaymentDMO> TADAApply_PaymentDMO { get; set; }
        public DbSet<TADA_ApplyDMO> TADA_ApplyDMO { get; set; }
        public DbSet<TADA_ApplyDetailsDMO> TADA_ApplyDetailsDMO { get; set; }
        public DbSet<VMS_TADAApply_FilesDMO> VMS_TADAApply_FilesDMO { get; set; }

        public DbSet<VMS_TADA_Advance_Apply_Authorisation_HeadsDMO> VMS_TADA_Advance_Apply_Authorisation_HeadsDMO { get; set; }
        public DbSet<VMS_TADA_Advance_Apply_AuthorisationDMO> VMS_TADA_Advance_Apply_AuthorisationDMO { get; set; }
        public DbSet<VMS_TADA_Apply_Authorisation_HeadsDMO> VMS_TADA_Apply_Authorisation_HeadsDMO { get; set; }
        public DbSet<VMS_TADA_Apply_AuthorisationDMO> VMS_TADA_Apply_AuthorisationDMO { get; set; }
        public DbSet<HR_PROCESSDMO> HR_PROCESSDMO { get; set; }
        public DbSet<Country> Country { get; set; }
        //public DbSet<VMS_TADA_Advance_Apply_AuthorisationDMO> TADA_Advance_Apply_AuthorisationDMO { get; set; }
        //public DbSet<VMS_TADA_Advance_Apply_AuthorisationDMO> TADA_Advance_Apply_AuthorisationDMO { get; set; }

        public DbSet<HR_Process_Auth_OrderNoDMO> HR_Process_Auth_OrderNoDMO { get; set; }
        public DbSet<VMS_TADA_Apply_Authorisation_HeadsDMO> TADA_Apply_Authorisation_HeadsDMO { get; set; }
        public DbSet<VMS_TADA_Apply_AuthorisationDMO> TADA_Apply_AuthorisationDMO { get; set; }
        public DbSet<VMS_Payment_Voucher_AuthorisationDMO> VMS_Payment_Voucher_AuthorisationDMO { get; set; }
        public DbSet<VMS_Payment_VoucherDMO> VMS_Payment_VoucherDMO { get; set; }
        public DbSet<INV_Master_SupplierDMO> INV_Master_SupplierDMO { get; set; }
        public DbSet<VMS_BankAccount_BRSDMO> VMS_BankAccount_BRSDMO { get; set; }

        public DbSet<HR_Master_BankDeatils> HR_Master_BankDeatils { get; set; }
        public DbSet<VMS_IVRM_Master_TemplateDMO> VMS_IVRM_Master_TemplateDMO { get; set; }

        //Rating 
        public DbSet<Rating_Master_TypeDMO> Rating_Master_TypeDMO { get; set; }
        public DbSet<RoomBankDeatilsDMO> RoomBankDeatilsDMO { get; set; }
        public DbSet<Rating_Overall_RatingDMO> Rating_Overall_RatingDMO { get; set; }
        public DbSet<Ratings_Master_Questions_AuthorityDMO> Ratings_Master_Questions_AuthorityDMO { get; set; }
        public DbSet<Rating_Type_Questions_RatingDMO> Rating_Type_Questions_RatingDMO { get; set; }
        public DbSet<Ratings_Master_QuestionsDMO> Ratings_Master_QuestionsDMO { get; set; }
        public DbSet<Rating_Master_OptionsDMO> Rating_Master_OptionsDMO { get; set; }
        public DbSet<Rating_Type_QuestionsDMO> Rating_Type_QuestionsDMO { get; set; }
        public DbSet<Rating_Type_Questions_DepartmentDMO> Rating_Type_Questions_DepartmentDMO { get; set; }
        public DbSet<Rating_Type_Questions_OptionsDMO> Rating_Type_Questions_OptionsDMO { get; set; }
        public DbSet<Rating_TransactionDMO> Rating_TransactionDMO { get; set; }
        public DbSet<Rating_Master_Question_ClassificationDMO> Rating_Master_Question_ClassificationDMO { get; set; }
        public DbSet<Rating_Master_Question_SubClassificationDMO> Rating_Master_Question_SubClassificationDMO { get; set; }
        public DbSet<Rating_Master_AuthorityDMO> Rating_Master_AuthorityDMO { get; set; }
        public DbSet<Ratings_Master_Authority_QuestionsDMO> Ratings_Master_Authority_QuestionsDMO { get; set; }
        public DbSet<Rating_TaskCategory_TransactionDMO> Rating_TaskCategory_TransactionDMO { get; set; }
        public DbSet<VMS_SendNoticeDMO> VMS_SendNoticeDMO { get; set; }
         public DbSet<ISM_Proposal_DMO> ISM_ProposalDMO { get; set; }
        public DbSet<ISM_ProposalDMO_Files> ISM_Proposal_FilesDMO { get; set; }
        public DbSet<HW_HealthCheckUpDMO> HW_HealthCheckUpDMO { get; set; }
        public DbSet<HW_HealthCheckUp_FilesDMO> HW_HealthCheckUp_FilesDMO { get; set; }
        public DbSet<HW_HealthCheckUp_ApprovalDMO> HW_HealthCheckUp_ApprovalDMO { get; set; }

        public DbSet<VMS_CheckBookSourceDMO> VMS_CheckBookSourceDMO { get; set; }
        public DbSet<BankAccount_BRS_FilesDMO> BankAccount_BRS_FilesDMO { get; set; }
        public DbSet<VMS_Receipt_VoucherDMO> VMS_Receipt_VoucherDMO { get; set; }
        // 
        public DbSet<ISM_DailyReport_Multiple_ApprovalDMO> ISM_DailyReport_Multiple_ApprovalDMO { get; set; }
        public DbSet<ISM_DailyReport_Daywise_Multiple_ApprovalDMO> ISM_DailyReport_Daywise_Multiple_ApprovalDMO { get; set; }
        public DbSet<HR_Planner_Daily_SchedulerDetailsDMO> HR_Planner_Daily_SchedulerDetailsDMO { get; set; }
        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<ViewOfDeploymentDMO> ViewOfDeploymentDMO { get; set; }
        public DbSet<ViewOfDeploymentTasksDMO> ViewOfDeploymentTasksDMO { get; set; }
        public DbSet<VMSMaster_ChequeBookDMO> VMSMaster_ChequeBookDMO { get; set; }
        public DbSet<VMSMaster_ChequeBook_SLNoDMO> VMSMaster_ChequeBook_SLNoDMO { get; set; }
        public DbSet<VMS_Payment_Voucher_Supplier_GRNDMO> VMS_Payment_Voucher_Supplier_GRNDMO { get; set; }
        public DbSet<PrinterDMO> PrinterDMO { get; set; }

        public DbSet<PrinterDeptMapping> PrinterDeptMapping { get; set; }

        //public DbSet<ISM_Client_Project_Docs> ISM_Client_Project_Docs_DMO_con { get; set; }

        public DbSet<ISM_Client_Project_Docs_DMO> ISM_Client_Project_Docs_DMO_con { get; set; }



        //======Tenants
        public DbSet<HR_Master_Building> HR_Master_Building { get; set; }
        public DbSet<HR_Master_Room> HR_Master_Room { get; set; }
        public DbSet<Tenant_Master_Owner> Tenant_Master_Owner { get; set; }
        public DbSet<TenantAgreement> TenantAgreement { get; set; }
        public DbSet<TenantAgreementFiles> TenantAgreementFilesDMO { get; set; }
        public DbSet<TenantAgreementRenewal> TenantAgreementRenewalDMO { get; set; }

        public DbSet<TenantNoticeBoardDMO> TenantNoticeBoardDMO { get; set; }
        public DbSet<TenantNoticeBoardFilesDMO> TenantNoticeBoardFilesDMO { get; set; }
        public DbSet<TenantNoticeBoardTenantDMO> TenantNoticeBoardTenantDMO { get; set; }
        public DbSet<TenantNoticeBoardTenantViewedDMO> TenantNoticeBoardTenantViewedDMO { get; set; }
        public DbSet<TenantPaymentDetailsDMO> TenantPaymentDetailsDMO { get; set; }
        public DbSet<GeneralConfigDMO> GenConfiguration { get; set; }
        public DbSet<HR_Master_Amenities_DMO> HR_Master_Amenities_DMO { get; set; }
        public DbSet<HR_Master_Room_AmenitiesDMO> HR_Master_Room_AmenitiesDMO { get; set; }
        public DbSet<VMS_BANK_ACCOUNT_BALANCEDMO> VMS_BANK_ACCOUNT_BALANCEDMO { get; set; }
        public DbSet<Visitor_Management_MasterVisitor_DMO> Visitor_Management_MasterVisitor_DMO { get; set; }
        public DbSet<VMS_Master_BudgetHeadsDMO> VMS_Master_BudgetHeadsDMO { get; set; }
        public DbSet<VMS_Master_BudgetHeads_AmountDMO> VMS_Master_BudgetHeads_AmountDMO { get; set; }
        public DbSet<MasterSource> MasterSource { get; set; }
        public DbSet<TenantMasterNocChecklistDMO> TenantMasterNocChecklistDMO { get; set; }
        public DbSet<Tenant_Monthly_WaterBills_RoomWiseDMO> Tenant_Monthly_WaterBills_RoomWiseDMO { get; set; }

        public DbSet<VMS_ChequePrinting_SettingDMO> VMS_ChequePrinting_SettingDMO { get; set; }
        public DbSet<EmployeeHeadFeedBackDMO> EmployeeHeadFeedBackDMO { get; set; }

        //public DbSet<ISM_Tenant_TaskCreationDMO> ISM_Tenant_TaskCreationDMO { get; set; }
        //public DbSet<NocCreationDMO> NocCreationDMO { get; set; }

        public DbSet<ISM_Tenant_TaskCreationDMO> ISM_Tenant_TaskCreationDMO { get; set; }
        public DbSet<NocCreationDMO> NocCreationDMO { get; set; }
        public DbSet<VMS_Payment_Voucher_BudgetDMO> VMS_Payment_Voucher_BudgetDMO { get; set; }

        //============================================= Master

        //===================Version Control
        public DbSet<IVRM_Client_HostingDMO> IVRM_Client_HostingDMO { get; set; }
        public DbSet<IVRM_Hosting_Client_MappingDMO> IVRM_Hosting_Client_MappingDMO { get; set; }
        public DbSet<IVRM_Version_TransactionDMO> IVRM_Version_TransactionDMO { get; set; }
        public DbSet<IVRM_Version_FeaturesDetailsDMO> IVRM_Version_FeaturesDetailsDMO { get; set; }
        public DbSet<INV_AssetTag_CheckOutDMO> INV_AssetTag_CheckOutDMO { get; set; }
        public DbSet<INV_Asset_AssetTagDMO> INV_Asset_AssetTagDMO { get; set; }
        public DbSet<INV_Master_LocationDMO> INV_Master_LocationDMO { get; set; }
        public DbSet<ISM_TaskEnhancement_DeptwiseDMO> ISM_TaskEnhancement_DeptwiseDMO { get; set; }
        public DbSet<ISM_TaskCreation_PriorityHistoryDMO> ISM_TaskCreation_PriorityHistoryDMO { get; set; }

        public DbSet<Legal_Notice_RequestDMO> Legal_Notice_RequestDMO { get; set; }
        public DbSet<Legal_Notice_Request_DocumentDMO> Legal_Notice_Request_DocumentDMO { get; set; }
        public DbSet<Legal_Notice_Request_HistoryDMO> Legal_Notice_Request_HistoryDMO { get; set; }
        public DbSet<Legal_Notice_Master_CategoryDMO> Legal_Notice_Master_CategoryDMO { get; set; }
        public DbSet<LNC_Master_Court_TypeDMO> LNC_Master_Court_TypeDMO { get; set; }
        public DbSet<LNC_Master_CourtDMO> LNC_Master_CourtDMO { get; set; }


        public DbSet<ISM_Emp_RoutePlanDMO> ISM_Emp_RoutePlanDMO { get; set; }
        public DbSet<ISM_Emp_RoutePlan_ClientDetailsDMO> ISM_Emp_RoutePlan_ClientDetailsDMO { get; set; }
        public DbSet<ISM_Emp_RoutePlan_CategoryDMO> ISM_Emp_RoutePlan_CategoryDMO { get; set; }
        public DbSet<ISM_RoutePlan_DepartureDetailsDMO> ISM_RoutePlan_DepartureDetailsDMO { get; set; }

        //ISM_RoutePlan_DepartureDetailsDMO
        
        public DbSet<ISM_Route_PrimaryAccountDMO> ISM_Route_PrimaryAccountDMO { get; set; }
        public DbSet<ISM_Mst_EmpCategory_MappingDMO> ISM_Mst_EmpCategory_MappingDMO { get; set; }
        public DbSet<ISM_Mst_EmpRegion_MappingDMO> ISM_Mst_EmpRegion_MappingDMO { get; set; }
        public DbSet<ISM_Mst_Route_CategoryDMO> ISM_Mst_Route_CategoryDMO { get; set; }
        public DbSet<VMS_TADA_MultipleClient_FilesDMO> VMS_TADA_MultipleClient_FilesDMO { get; set; }
        public DbSet<VMS_TADDA_MultipleClient_FilesDMO> VMS_TADDA_MultipleClient_FilesDMO { get; set; }

        public DbSet<ISM_Lead_RouteTADA_DMO> ISM_Lead_RouteTADA_DMO { get; set; }
        public DbSet<Route_TADA_WalletDMO> Route_TADA_WalletDMO { get; set; }
        public DbSet<Route_TADA_TransactionDMO> Route_TADA_TransactionDMO { get; set; }

        public DbSet<ISM_Sales_Lead_Demo_Products_DMO> ISM_Sales_Lead_Demo_Products_DMO_con { get; set; }
        public DbSet<ISM_Sales_Master_Status_DMO> ISM_Sales_Master_Status_DMO_con { get; set; }
        public DbSet<ISM_Sales_Lead_DMO> ISM_Sales_Lead_DMO_con { get; set; }
        public DbSet<SALES_Advance_Apply_PaymentDMO> SALES_Advance_Apply_PaymentDMO { get; set; }
        public DbSet<ISM_Task_CallCenter_RemarkDMO> ISM_Task_CallCenter_RemarkDMO { get; set; }
        
        public DbSet<Legal_UpdateDMO> Legal_UpdateDMO { get; set; }
        public DbSet<Legal_HistoryDMO> Legal_HistoryDMO { get; set; }
        public DbSet<VMS_TADA_Master_Travel_DetailsDMO> VMS_TADA_Master_Travel_DetailsDMO { get; set; }
        public DbSet<VMS_TADA_Master_TravelDMO> VMS_TADA_Master_TravelDMO { get; set; }
        public DbSet<ISM_Master_Client_FeedbackDMO> ISM_Master_Client_FeedbackDMO { get; set; }
        public DbSet<VMS_TADA_CityMappingBDM> VMS_TADA_CityMappingBDM { get; set; }
        public DbSet<Lop_DetailsDMO> Lop_DetailsDMO { get; set; }
        public DbSet<VMSTADAMasterCityDMO> VMSTADAMasterCityDMO { get; set; }

        //budget
        public DbSet<VMS_Master_BudgetHeads_Amount_newDMO>  VMS_Master_BudgetHeads_Amount_newDMO { get; set; }

        public DbSet<VMS_Master_BudgetHeads_newDMO> VMS_Master_BudgetHeads_newDMO { get; set; }

        public DbSet<VMS_Master_BudgetHeads_Amount_MonthDMO> VMS_Master_BudgetHeads_Amount_MonthDMO { get; set; }
        public DbSet<ISM_Task_Deviation_DateExtend> ISM_Task_Deviation_DateExtend { get; set; }
        public DbSet<ISM_Task_Deviation_DateExtend_Approval> ISM_Task_Deviation_DateExtend_Approval { get; set; }


        public DbSet<VMS_AfterSeven_Employee_WorkDMO> VMS_AfterSeven_Employee_WorkDMO { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;;;;;
            }
        }

    }
}
