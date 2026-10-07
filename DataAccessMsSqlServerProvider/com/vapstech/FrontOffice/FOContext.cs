using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VMS.Training;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.VMS.Sales;

namespace DataAccessMsSqlServerProvider.com.vapstech.FrontOffice
{
    public class FOContext : DbContext
    {
        public FOContext(DbContextOptions<FOContext> options) :base(options)
        {
            Database.SetCommandTimeout(300000);
        }
        //public DbSet<TTMasterCategoryDMO> TTMasterCategoryDMO { get; set; }
        public DbSet<MasterShiftsTimingsDMO> masterShiftsTimings { get; set; }
     
        public DbSet<MasterTimeSettingDMO> MasterTimeSetting { get; set; }

        public DbSet<MasterShiftsDMO> masterShifts { get; set; }
        public DbSet<HolidayWorkingDayTypeDMO> holidayWorkingDayType { get; set; }
        public DbSet<HR_Emp_Leave_StatusDMO> HR_Emp_Leave_StatusDMO { get; set; }
        public DbSet<SMSEmailSetting> smsEmailSetting { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }
        public DbSet<FO_Emp_Punch_Alter_Request> fO_Emp_Punch_Alter_Requests { get; set; }
        public DbSet<FO_Emp_Punch_Alter_Request_DetailsDMO> FO_Emp_Punch_Alter_Request_DetailsDMO { get; set; }
        
        public DbSet<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public DbSet<Institution_Module> Institution_Module { get; set; }
        public DbSet<MasterModule> masterModule { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<HR_Master_GroupType> HR_Master_GroupType_DMO { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department_DMO { get; set; }
        public DbSet<HR_Master_Designation> HR_Master_Designation_DMO { get; set; }
        public DbSet<HR_Leave_Auth_OrderNo_DMO> HR_Leave_Auth_OrderNo_DMO { get; set; }
        public DbSet<IVRM_Storage_path_Details> IVRM_Storage_path_Details { get; set; }
        public DbSet<Month> Month { get; set; }
        public DbSet<ApplicationUserRoleDMO> ApplicationUserRoleDMO { get; set; }
        public DbSet<ApplRole> ApplRole { get; set; }
        public DbSet<HR_Master_Employee_DMO> HR_Master_Employee_DMO { get; set; }
        public DbSet<EmployeeShiftMappingDMO> EmployeeShiftMapping { get; set; }
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<MasterHolidayDMO> Master_holiday { get; set; }
        public DbSet<FO_Master_Employee_HolidaysDMO> FO_Master_Employee_HolidaysDMO { get; set; }
        public DbSet<FO_Master_HolidayWorkingDay_DatesDMO> holidaydate { get; set; }
        public DbSet<TT_Master_DayDMO> TT_Master_DayDMO { get; set; }
        public DbSet<FODayNameDMO> FODayNameDMO { get; set; }
        public DbSet<FO_Emp_Punch_DetailsDMO> FO_Emp_Punch_Details { get; set; }
       
        public DbSet<FO_Emp_PunchDMO> FO_Emp_Punch { get; set; }
        public DbSet<HR_Master_LeaveYearDMO> HR_Master_LeaveYearDMO { get; set; }
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
        public DbSet<FO_FileDownloadedLogsDMO> FO_FileDownloadedLogsDMO { get; set; }
        public DbSet<FO_Biometric_VAPS_IEMapping_DMO> FO_Biometric_VAPS_IEMapping_DMO { get; set; }
        public DbSet<HR_Master_DepartmentCode_HeadDMO> HR_Master_DepartmentCode_Head { get; set; }
        public DbSet<ApplicationUserDMO> ApplicationUserDMO { get; set; }
        public DbSet<ISM_DailyReportGenerationDMO> ISM_DailyReportGenerationDMO { get; set; }
        public DbSet<VMSConsolidateReportTempDMO> VMSConsolidateReportTempDMO { get; set; }
        public DbSet<HR_Emp_Attendance_ApprovalDMO> HR_Emp_Attendance_ApprovalDMO { get; set; }
        public DbSet<DriverOtDetailsDMO> DriverOtDetailsDMO { get; set; }
        public DbSet<VMSDriverOTApprovalDMO> VMSDriverOTApprovalDMO { get; set; }
        public DbSet<ISM_Checklist_ApprovalDMO> ISM_Checklist_ApprovalDMO { get; set; }
        public DbSet<ISM_Checklist_DetailsDMO> ISM_Checklist_DetailsDMO { get; set; }
        public DbSet<ISM_Checklist_TypeDMO> ISM_Checklist_TypeDMO { get; set; }
        public DbSet<ISM_Inbound_Outbound_Call_DetailsDMO> ISM_Inbound_Outbound_Call_DetailsDMO { get; set; }
        public DbSet<HR_Master_Floor> HR_Master_Floor { get; set; }
        public DbSet<HR_Emp_Leave_Trans_DMO> HR_Emp_Leave_Trans_DMO { get; set; }
        public DbSet<HR_Emp_Leave_Trans_Details_DMO> HR_Emp_Leave_Trans_Details_DMO { get; set; }
        public DbSet<HR_Master_Leave_DMO> HR_Master_Leave_DMO { get; set; }
        public DbSet<ISM_Master_Client_DMO> ISM_Master_Client_DMO { get; set; }
        public DbSet<ISM_Master_Client_IEMapping_DMO> ISM_Master_Client_IEMapping_DMO { get; set; }
        public DbSet<ISM_Sales_Lead_DMO> ISM_Sales_Lead_DMO { get; set; }
        public DbSet<VMS_Attendance_GPSLocationDMO> VMS_Attendance_GPSLocationDMO { get; set; }
        public DbSet<HR_Leave_Authorisation_DMO> HR_Leave_Authorisation_DMO { get; set; }
        public DbSet<ISM_Client_MERDMO> ISM_Client_MERDMO { get; set; }
        public DbSet<ISM_Client_MER_FilesDMO> ISM_Client_MER_FilesDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MasterShiftsTimingsDMO>().ToTable("FO_Master_Shifts_Timings", "FO");

            builder.Entity<FODayNameDMO>().ToTable("FO_Master_Day", "FO");

            builder.Entity<FO_Emp_PunchDMO>().ToTable("FO_Emp_Punch", "FO");
            builder.Entity<FO_Emp_Punch_DetailsDMO>().ToTable("FO_Emp_Punch_Details", "FO");
            builder.Entity<MasterShiftsDMO>().ToTable("FO_Master_Shifts", "FO");

            builder.Entity<HolidayWorkingDayTypeDMO>().ToTable("FO_HolidayWorkingDay_Type", "FO");
            builder.Entity<MasterTimeSettingDMO>()
           .ToTable("FO_Master_TimeSettings", "FO");
            builder.Entity<EmployeeShiftMappingDMO>()
              .ToTable("FO_Emp_Shifts_Timings", "FO");
            builder.Entity<MasterHolidayDMO>().ToTable("FO_Master_HolidayWorkingDay", "FO");
            builder.Entity<FO_Master_HolidayWorkingDay_DatesDMO>().ToTable("FO_Master_HolidayWorkingDay_Dates", "FO");
            base.OnModelCreating(builder);
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
