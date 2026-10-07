using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.FMS;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.HWHealthCheckUp;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vapstech.VMS.Sales;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.EntityFrameworkCore;


namespace DataAccessMsSqlServerProvider.com.vapstech.FMS
{
    public class FMSContext : DbContext
    {
        public FMSContext(DbContextOptions<FMSContext> options) : base(options)
        { Database.SetCommandTimeout(300000000); }
       
        public DbSet<FMS_Master_FileCategoryDMO> FMS_Master_FileCategoryDMO { get; set; }
        public DbSet<FMS_CorrespondenceDMO> FMS_CorrespondenceDMO { get; set; }
        public DbSet<FMS_Correspondence_ClientDMO> FMS_Correspondence_ClientDMO { get; set; }
        public DbSet<FMS_Correspondence_SupplierDMO> FMS_Correspondence_SupplierDMO { get; set; }
        public DbSet<FMS_Correspondence_SalesLeadDMO> FMS_Correspondence_SalesLeadDMO { get; set; }
        public DbSet<FMS_Correspondence_Files_SoftCopyDMO> FMS_Correspondence_Files_SoftCopyDMO { get; set; }
        public DbSet<FMS_Correspondence_FilesDMO> FMS_Correspondence_FilesDMO { get; set; }
        public DbSet<FMS_Correspondence_ModeDMO> FMS_Correspondence_ModeDMO { get; set; }
        public DbSet<ISM_Master_Client_DMO> ISM_Master_Client_DMO { get; set; }
        public DbSet<ISM_Master_Client_ProjectDMO> ISM_Master_Client_ProjectDMO { get; set; }
        public DbSet<MastersProject_DMO> MastersProject_DMO { get; set; }
        public DbSet<INV_Master_SupplierDMO> INV_Master_SupplierDMO { get; set; }
        public DbSet<ISM_Sales_Lead_DMO> ISM_Sales_Lead_DMO { get; set; }

        //added by akash(FMSApprovalRequestPageDMO)
        public DbSet<FMSApprovalRequestPageDMO> FMSApprovalRequestPageDMO { get; set; }
        public DbSet<FMSApprovalDMO> FMSApprovalDMO { get; set; }
        public DbSet<SMSEmailSetting> smsEmailSetting { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Emp_Email_Id { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYear { get; set; }
        public DbSet<FMS_Master_PhysicalFileNameDMO> FMS_Master_PhysicalFileNameDMO { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }
        public DbSet<Institution_Module> Institution_Module { get; set; }
        //public DbSet<MasterModule> masterModule { get; set; }
        public DbSet<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public DbSet<SMS_DETAILS_DMO> SMS_DETAILS_DMO { get; set; }
        public DbSet<HR_Process_Auth_OrderNoDMO> HR_Process_Auth_OrderNoDMO { get; set; }
      
        public DbSet<FMS_Master_FileCabinetDMO> FMS_Master_FileCabinetDMO { get; set; }
        public DbSet<FMS_Master_FileCabinet_ColumnDMO> FMS_Master_FileCabinet_ColumnDMO { get; set; }
        public DbSet<FMS_Master_FileCabinet_RowDMO> FMS_Master_FileCabinet_RowDMO { get; set; }
        public DbSet<FMS_Master_DispatchModeDMO> FMS_Master_DispatchModeDMO { get; set; }
        //public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<HR_Master_Room> HR_Master_Room_con { get; set; }
        public DbSet<HR_Master_Floor> HR_Master_Floor_con { get; set; }
        public DbSet<HR_Master_Building> HR_Master_Building_con { get; set; }
        public DbSet<GeneralConfigDMO> GeneralConfigDMO { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<FMS_ReferenceNo_ConfigurationDMO> FMS_ReferenceNo_ConfigurationDMO { get; set; }
        public DbSet<FMS_Department_FileCategoryDMO> FMS_Department_FileCategoryDMO { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<ISM_PDpnt_SDpnt_MappingDMO> ISM_PDpnt_SDpnt_MappingDMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<FMS_Correspondence_EmployeeDMO> FMS_Correspondence_EmployeeDMO { get; set; }
        public DbSet<FMS_Dept_Letter_HeadMappingDMO> FMS_Dept_Letter_HeadMappingDMO { get; set; }
        public DbSet<FMS_Master_PhysicalFileName_FileCabinetDMO> FMS_Master_PhysicalFileName_FileCabinetDMO { get; set; }
        public DbSet<FMS_Department_LetterHeads_TrackingDMO> FMS_Department_LetterHeads_TrackingDMO { get; set; }
        public DbSet<FMS_Department_PhysicalFileNameDMO> FMS_Department_PhysicalFileNameDMO { get; set; }
        public DbSet<HR_PROCESSDMO> HR_PROCESSDMO { get; set; }
        public DbSet<MasterModules> MasterModules { get; set; }
        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        //added by adarsh.ms for VMS FEEDBACK
        public DbSet<ModuleFeedbackDMO> ModuleFeedback { get; set; }
        public DbSet<Employee_Wise_VMS_Feedback> Employee_Wise_VMS_Feedback { get; set; }
       
        public DbSet<HR_Master_LeaveYearDMO> HR_Master_LeaveYearDMO { get; set; }
        public DbSet<IVRM_Month_DMO> IVRM_Month_DMO { get; set; }
        public DbSet<HW_HealthCheckUp_FeedbackDMO> HW_HealthCheckUp_FeedbackDMO { get; set; }
        public DbSet<HW_HealthCheckUpDMO> HW_HealthCheckUpDMO { get; set; }
        public DbSet<HW_HealthCheckUp_FilesDMO> HW_HealthCheckUp_FilesDMO { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<FMS_Master_FileCategoryDMO>().ToTable("FMS_Master_FileCategory");           
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();

        }
        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =ChangeTracker.Entries<T>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                //entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;;;;;
            }
        }

    }
}
