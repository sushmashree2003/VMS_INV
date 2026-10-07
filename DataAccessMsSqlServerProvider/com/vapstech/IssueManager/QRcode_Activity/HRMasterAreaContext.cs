using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity;
using DomainModel.Model.com.vapstech.IssueManager.Tenants;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.IssueManager.HRMasterArea
{
    public class HRMasterAreaContext : DbContext
    {
       // public IEnumerable<object> institution;

        public HRMasterAreaContext(DbContextOptions<HRMasterAreaContext> options) : base(options)
        { }
        public DbSet<HRMasterAreaDMO> HRMasterAreaDMO { get; set; }
        public DbSet<HRMasterAreaActivityDMO> HRMasterAreaActivityDMO { get; set; }

        public DbSet<HRMasterAreaActivityTimeDMO> HRMasterAreaActivityTimeDMO { get; set; }

        public DbSet<HRMasterAreaStaffMappingDMO> HRMasterAreaStaffMappingDMO { get; set; }

        public DbSet<HR_Master_Building> HR_Master_Building_con { get; set; }

        public DbSet<HR_Master_Floor> HR_Master_Floor_con { get; set; }

        public DbSet<MasterRoleType> masterRoleType { get; set; }

        public DbSet<Institution> Institution { get; set; }

        public DbSet<UserRoleWithInstituteDMO> UserRoleWithInstituteDMO { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }

        public DbSet<StaffAreaActivityRecordDMO> StaffAreaActivityRecordDMO { get; set; }
        public DbSet<NocCreationDMO> NocCreationDMO { get; set; }
        public DbSet<TenantAgreement> TenantAgreement { get; set; }

    }
}
