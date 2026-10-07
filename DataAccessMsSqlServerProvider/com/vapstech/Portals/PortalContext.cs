using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Portals.Employee;
using DomainModel.Model.com.vapstech.Transport;

using DomainModel.Model.com.vapstech.Portals.HOD;
using DomainModel.Model.com.vapstech.Portals.Principal;
using DomainModel.Model.com.vapstech.Portals.Chairman;
using DomainModel.Model.com.vapstech.Portals.Student;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.Portals.IVRS;
using DomainModel.Model.com.vapstech.MobileApp;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.Portals.IVRM;
using DomainModel.Model.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using DomainModel.Model.com.vapstech.Portals.Tenants;
using DomainModel.Model.com.vapstech.VMS.Training;
using DomainModel.Model.com.vapstech.Fee;

namespace DataAccessMsSqlServerProvider.com.vapstech.Portals
{
    public class PortalContext : DbContext
    {
        public PortalContext(DbContextOptions<PortalContext> options) : base(options)
        {
            Database.SetCommandTimeout(300000);
        }
        public PortalContext()
        {
        }


        //  public DbSet<Fee_Student_StatusDMO> Fee_Student_StatusDMO { get; set; }
        public DbSet<Adm_M_Student> AdmissionStudentDMO { get; set; }
        public DbSet<FeePaymentDetailsDMO> FeeYPaymentDMO { get; set; }
        public DbSet<AcademicYear> AcademicYearDMO { get; set; }
        public DbSet<FeeAmountEntryDMO> FeeAmountEntryDMO { get; set; }
        public DbSet<FeeSpecialFeeGroupDMO> feespecialHead { get; set; }
        public DbSet<FeeSpecialFeeGroupsGroupingDMO> feeSGGG { get; set; }
        public DbSet<FeeMasterTermHeadsDMO> feeMTH { get; set; }
        public DbSet<FeeTermDMO> feeTr { get; set; }
        public DbSet<FeeYearGroupDMO> Yearlygroups { get; set; }
        //  public DbSet<SchoolYearWiseStudent> SchoolYStudentDMO { get; set; }
        public DbSet<Fee_Y_Payment_School_StudentDMO> Fee_Y_Payment_School_StudentDMO { get; set; }
        public DbSet<FeePaymentDetailsDMO> FeePaymentDetailsDMO { get; set; }
        public DbSet<FeeTransactionPaymentDMO> FeeTransactionPaymentDMO { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<StudentMappingDMO> StudentMappingDMO { get; set; }
        public DbSet<IVRM_Master_SubjectsDMO> IVRM_Master_SubjectsDMO { get; set; }
        public DbSet<ExmStudentMarksProcessSubjectwiseDMO> ExmStudentMarksProcessSubjectwiseDMO { get; set; }
        public DbSet<exammasterDMO> exammasterDMO { get; set; }
        public DbSet<IVRM_ClassWorkDMO> IVRM_ClassWorkDMO { get; set; }
        public DbSet<COE_Master_EventsDMO> COE_Master_EventsDMO { get; set; }
        public DbSet<COE_EventsDMO> COE_EventsDMO { get; set; }
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<ApplRole> ApplicationRole_con { get; set; }
        public DbSet<Staff_CovidVaccinationDMO> Staff_CovidVaccinationDMO { get; set; }
        public DbSet<Master_CovidVaccineTypeDMO> Master_CovidVaccineTypeDMO { get; set; }
        public DbSet<Student_CovidVaccinationDMO> Student_CovidVaccinationDMO { get; set; }
        public DbSet<Staff_CovidTestDMO> Staff_CovidTestDMO { get; set; }
        public DbSet<Student_CovidTestDMO> Student_CovidTestDMO { get; set; }
        public DbSet<StudentUserLoginDMO> StudentUserLoginDMO { get; set; }
        public DbSet<Staff_User_Login> IVRM_Staff_User_Login { get; set; }

        public DbSet<Adm_Students_Leave_Apply_DMO> Adm_Students_Leave_Apply_DMO { get; set; }
        public DbSet<Adm_Students_Approval_Process_DMO> Adm_Students_Approval_Process_DMO { get; set; }
        

        public DbSet<ClassTeacherMappingDMO> ClassTeacherMappingDMO { get; set; }

        public DbSet<IVRM_MobileApp_Page> IVRM_MobileApp_Page { get; set; }

        public DbSet<IVRM_User_MobileApp_Login_Privileges> IVRM_User_MobileApp_Login_Privileges { get; set; }

        public DbSet<IVRM_Role_MobileApp_Privileges> IVRM_Role_MobileApp_Privileges { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<Exm_Login_Privilege_SubjectsDMO> Exm_Login_Privilege_SubjectsDMO { get; set; }
        public DbSet<Exm_Login_PrivilegeDMO> Exm_Login_PrivilegeDMO { get; set; }
        public DbSet<IVRM_MobileApp_Download_DMO> IVRM_MobileApp_Download_DMO { get; set; }
        public DbSet<EmployeeStudentExamResultDMO> EmployeeStudentExamResultDMO { get; set; }
        public DbSet<IVRM_Month_DMO> IVRM_Month_DMO { get; set; }
        public DbSet<Fee_Master_BankDMO> Fee_Master_BankDMO { get; set; }
        
        public DbSet<FeeBankDetailsDMO> FeeBankDetailsDMO { get; set; }
        
        public DbSet<StudentGuardianDMO> StudentGuardianDMO { get; set; }
        public DbSet<IVRM_DocsUploadDMO> IVRM_DocsUploadDMO { get; set; }

        public DbSet<HR_Master_BankDeatils> HR_Master_BankDeatils { get; set; }
        public DbSet<RoomBankDeatilsDMO> RoomBankDeatilsDMO { get; set; }

        


        //HOD //aman
        public DbSet<Adm_Students_Certificate_Approve_DMO> Adm_Students_Certificate_Approve_DMO { get; set; }
        public DbSet<HOD_DMO> HOD_DMO { get; set; }
        public DbSet<IVRM_HOD_Class_DMO> IVRM_HOD_Class_DMO { get; set; }
        public DbSet<IVRM_HOD_Staff_DMO> IVRM_HOD_Staff_DMO { get; set; }

        public DbSet<Adm_Students_Certificate_Apply_DMO> Adm_Students_Certificate_Apply_DMO { get; set; }

        public DbSet<Adm_Students_Leave_Approval_DMO> Adm_Students_Leave_Approval_DMO { get; set; }

        public DbSet<Institution> Institution_master { get; set; }

        //Tenants
        public DbSet<TenantAgreement> TenantAgreement { get; set; }
        public DbSet<TenantPaymentDetailsDMO> TenantPaymentDetailsDMO { get; set; }
        //Akash
        public DbSet<TenantElectricalWaterBillDMO> TenantElectricalWaterBillDMO { get; set; }
        //
        public DbSet<IVRM_ModeOfPaymentDMO> IVRM_ModeOfPaymentDMO { get; set; }
        public DbSet<HR_Master_Building> HR_Master_Building { get; set; }
        public DbSet<HR_Master_Floor> HR_Master_Floor { get; set; }
        public DbSet<HR_Master_Room> HR_Master_Room { get; set; }

        public DbSet<PAYUDETAILS> PAYUDETAILS { get; set; }

        public DbSet<Fee_PaymentGateway_DetailsDMO> Fee_PaymentGateway_Details { get; set; }

        //chairman
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<HR_Master_LeaveYearDMO> HR_MasterLeaveYear { get; set; }
        public DbSet<HR_Master_Leave_DMO> HR_Master_Leave { get; set; }
        public DbSet<IVRM_Master_Marital_Status> IVRM_Master_Marital_Status { get; set; }
        public DbSet<HR_Emp_Leave_StatusDMO> HR_Emp_Leave_StatusDMO { get; set; }
        public DbSet<HR_Emp_Leave_Trans_Details_DMO> HR_Emp_Leave_Trans_Details { get; set; }
        public DbSet<HR_Master_EarningsDeductionsPer> HR_Master_EarningsDeductionsPer { get; set; }
        public DbSet<HR_Employee_Salary> HR_Employee_Salary { get; set; }
        public DbSet<HR_Employee_Salary_Details> HR_Employee_Salary_Details { get; set; }
        public DbSet<HR_Employee_EarningsDeductions> HR_Employee_EarningsDeductions { get; set; }
        public DbSet<Exm_Master_CategoryDMO> Exm_Master_CategoryDMO { get; set; }
        public DbSet<Exm_Yearly_CategoryDMO> Exm_Yearly_CategoryDMO { get; set; }
        public DbSet<Exm_Category_ClassDMO> Exm_Category_ClassDMO { get; set; }
        public DbSet<HR_Master_GroupType_DMO> HR_Master_GroupType { get; set; }
        public DbSet<Exm_Yearly_Category_ExamsDMO> Exm_Yearly_Category_ExamsDMO { get; set; }
        //public DbSet<AdmissionClass> AdmissionClass { get; set; }
        //  public DbSet<IVRM_School_Master_SubjectsDMO> Ch_IVRM_School_Master_SubjectsDMO { get; set; }
        public DbSet<Exm_Yrly_Cat_Exams_SubwiseDMO> Ch_Exm_Yrly_Cat_Exams_SubwiseDMO { get; set; }
       // public DbSet<School_M_Section> ch_Section { get; set; }
        public DbSet<IVRM_Master_Gender> IVRM_Master_Gender { get; set; }
        public DbSet<ExmStudentMarksProcessSubjectwiseDMO> ExmStudentMarksProcessSubjectwise { get; set; }

        public DbSet<FeeMasterConfigurationDMO> FeeMasterConfigurationDMO { get; set; }

        //Principal //aman
         public DbSet<IVRM_PrincipalDMO> IVRM_PrincipalDMO { get; set; }
         public DbSet<IVRM_Principal_ClassDMO> IVRM_Principal_ClassDMO { get; set; }
         public DbSet<IVRM_Principal_StaffDMO> IVRM_Principal_StaffDMO { get; set; }


        //public DbSet<COE_Master_EventsDMO> COE_Master_EventsDMO { get; set; }
        // public DbSet<MasterAcademic> AcademicYear { get; set; } by suresh
        // public DbSet<COE_EventsDMO> COE_EventsDMO { get; set; }
        //public DbSet<AdmissionClass> AdmissionClass { get; set; }
        // public DbSet<School_M_Section> School_M_Section { get; set; }
        //public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        //public DbSet<HR_Master_Designation> HR_Master_Designation { get; set; }
        //  public DbSet<MasterEmployee> MasterEmployee { get; set; }by suresh
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }


        // public DbSet<HR_Employee_Salary> HR_Employee_Salary { get; set; }

        // public DbSet<HR_Master_LeaveYearDMO> HR_Master_LeaveYearDMO { get; set; }
        // public DbSet<HR_Employee_Salary> HR_Employee_Salary { get; set; }

        public DbSet<HR_Master_GroupType_DMO> HR_Master_GroupType_DMO { get; set; }
        // public DbSet<HR_Master_LeaveYearDMO> HR_Master_LeaveYear { get; set; }
        public DbSet<HR_Configuration> HR_Configuration { get; set; }
        // public DbSet<HR_Emp_Leave_Trans_Details_DMO> HR_Emp_Leave_Trans_Details { get; set; }
        // public DbSet<HR_Master_Leave_DMO> HR_Master_Leave { get; set; }
        //public DbSet<HR_Master_LeaveYearDMO> HR_MasterLeaveYear { get; set; }
        // public DbSet<HR_Emp_Leave_StatusDMO> HR_Emp_Leave_StatusDMO { get; set; }
        public DbSet<FO_Emp_Punch_DetailsDMO> FO_Emp_Punch_Details { get; set; }

        public DbSet<StudentHelthcertificateDMO> StudentHelthcertificate { get; set; }
        public DbSet<StudentApplication> Enq { get; set; }

        public DbSet<ApplicationUser> ApplicationUser  { get; set; }

        public DbSet<ApplicationUserRole> ApplicationUserRole  { get; set; }
        public DbSet<MasterAreaDMO> MasterAreaDMO { get; set; }
        public DbSet<MasterRouteDMO> MasterRouteDMO { get; set; }
        public DbSet<Route_Location> Route_Location { get; set; }
        public DbSet<MasterLocationDMO> MasterLocationDMO { get; set; }
        public DbSet<PA_Student_Transport_ApplicationDMO> PA_Student_Transport_ApplicationDMO { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<State> state { get; set; }
        public DbSet<Adm_Student_Transport_ApplicationDMO> Adm_Student_Transport_ApplicationDMO { get; set; }

        public DbSet<Adm_Student_Transport_Application_UpdateDMO> Adm_Student_Transport_Application_UpdateDMO { get; set; }
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }


        public DbSet<FeeGroupDMO> FeeGroupDMO { get; set; }
        public DbSet<FeeHeadDMO> FeeHeadDMO { get; set; }
        public DbSet<FeeYearlygroupHeadMappingDMO> FeeYearlygroupHeadMappingDMO { get; set; }

        public DbSet<FeeStudentTransactionDMO> FeeStudentTransactionDMO { get; set; }

        public DbSet<Adm_feedbackDMO> Adm_feedbackDMO { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
        public DbSet<TRMasterconfigurationDMO> TRMasterconfigurationDMO { get; set; }

        public DbSet<Exm_Yearly_Category_Group_SubjectsDMO> Exm_Yearly_Category_Group_SubjectsDMO { get; set; }
        public DbSet<Exm_Yearly_Category_GroupDMO> Exm_Yearly_Category_GroupDMO { get; set; }


        public DbSet<StudentDetailsupdateDMO> StudentDetailsupdateDMO { get; set; }


        public DbSet<Adm_M_Student_FatherMobileNo> Adm_M_Student_FatherMobileNo { get; set; }
        public DbSet<Adm_Master_Father_Email> Adm_Master_Father_Email { get; set; }
        public DbSet<Adm_M_Mother_MobileNo> Adm_M_Mother_MobileNo { get; set; }
        public DbSet<Adm_M_Mother_Emailid> Adm_M_Mother_Emailid { get; set; }
        public DbSet<Adm_M_Student_MobileNo> Adm_M_Student_MobileNo { get; set; }
        public DbSet<Adm_M_Student_Email_Id> Adm_M_Student_Email_Id { get; set; }

        //aman
        public DbSet<IVRM_ImageUploadDMO> IVRM_ImageUploadDMO { get; set; }
        public DbSet<IVRM_NoticeBoardDMO> IVRM_NoticeBoardDMO { get; set; }
        public DbSet<IVRM_NoticeBoard_Class_DMO> IVRM_NoticeBoard_Class_DMO { get; set; }
        public DbSet<IVRM_Homework_DMO> IVRM_Homework_DMO { get; set; }
        public DbSet<IVRM_PushNotificationDMO> IVRM_PushNotificationDMO { get; set; }
        public DbSet<IVRM_PushNotification_Staff_DMO> IVRM_PushNotification_Staff_DMO { get; set; }
        public DbSet<IVRM_PushNotification_Student_DMO> IVRM_PushNotification_Student_DMO { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }

        //==============Interaction
        public DbSet<IVRM_Interactions_StudentDMO> IVRM_Interactions_StudentDMO { get; set; }        
        public DbSet<IVRM_Interactions_Student_StaffDMO> IVRM_Interactions_Student_StaffDMO { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUserClass> Adm_SchoolAttendanceLoginUserClass { get; set; }
        public DbSet<Adm_SchoolAttendanceLoginUser> Adm_SchoolAttendanceLoginUser { get; set; }
        public DbSet<GeneralConfigDMO> GeneralConfigDMO { get; set; }
        public DbSet<IVRM_School_Master_InteractionsDMO> IVRM_School_Master_InteractionsDMO { get; set; }
        public DbSet<IVRM_School_Transaction_InteractionsDMO> IVRM_School_Transaction_InteractionsDMO { get; set; }


        //============IVRS
        public DbSet<IVRM_IVRS_ConfigurationDMO> IVRM_IVRS_ConfigurationDMO { get; set; }
        public DbSet<IVRS_Acc_RechargeDMO> IVRS_Acc_RechargeDMO { get; set; }
        public DbSet<IVRS_Call_DetailsDMO> IVRS_Call_DetailsDMO { get; set; }
        public DbSet<IVRS_Call_StatusDMO> IVRS_Call_StatusDMO { get; set; }
        public DbSet<IVRS_Master_LanguagesDMO> IVRS_Master_LanguagesDMO { get; set; }   
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }
        public DbSet<Adm_School_Student_GFeedbackDMO> Adm_School_Student_GFeedbackDMO { get; set; }

        public DbSet<FeeStudentGroupMappingDMO> FeeStudentGroupMappingDMO { get; set; }
        public DbSet<TR_Student_RouteDMO> TR_Student_RouteDMO { get; set; }
        public DbSet<TR_Student_Route_FeeGroupDMO> TR_Student_Route_FeeGroupDMO { get; set; }

        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        public DbSet<Institution> Institute { get; set; }
        public DbSet<ExamPromotionRemarksDMO> ExamPromotionRemarksDMO { get; set; }
        public DbSet<IVRM_GalleryDMO> IVRM_GalleryDMO { get; set; }
        public DbSet<IVRM_Gallery_PhotosDMO> IVRM_Gallery_PhotosDMO { get; set; }
        public DbSet<IVRM_Gallery_ClassDMO> IVRM_Gallery_ClassDMO { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; }
        public DbSet<Tenant_Agreement_Online_PaymentDetailsDMO> Tenant_Agreement_Online_PaymentDetailsDMO { get; set; }
        public DbSet<Tenant_Monthly_WaterBills_RoomWiseDMO> Tenant_Monthly_WaterBills_RoomWiseDMO { get; set; }
        public DbSet<Tenant_Monthly_WaterBillsDMO> Tenant_Monthly_WaterBillsDMO { get; set; }

        public DbSet<TenantAgreementFiles> TenantAgreementFilesDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //base.OnModelCreating(builder);
            //builder.Entity<HR_Master_GroupType_DMO>().HasKey(m => m.HRMGT_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            //updateUpdatedProperty<TTMasterCategoryDMO>();
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


