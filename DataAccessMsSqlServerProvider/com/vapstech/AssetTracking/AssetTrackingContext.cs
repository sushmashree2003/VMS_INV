using System.Linq;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking.AssetTag;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.AssetTracking;

namespace DataAccessMsSqlServerProvider.com.vapstech.AssetTracking
{
    public class AssetTrackingContext : DbContext
    {
        public AssetTrackingContext(DbContextOptions<AssetTrackingContext> options) : base(options)
        { }

        //==============================ASSET TRACKING DMO========================
        public DbSet<GeneralConfigDMO> GeneralConfigDMO { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<SMSEmailSetting> SMSEmailSetting { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<INV_Master_SiteDMO> INV_Master_SiteDMO { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole_con { get; set; }
        public DbSet<ApplRole> applicationRole { get; set; }
        public DbSet<SMS_DETAILS_DMO> SMS_DETAILS_DMO { get; set; }
        public DbSet<INV_Master_LocationDMO> INV_Master_LocationDMO { get; set; }
        public DbSet<INV_Master_StoreDMO> INV_Master_StoreDMO { get; set; }
        public DbSet<INV_Master_ItemDMO> INV_Master_ItemDMO { get; set; }
        public DbSet<INV_Asset_CheckOutDMO> INV_Asset_CheckOutDMO { get; set; }
        public DbSet<INV_Asset_CheckInDMO> INV_Asset_CheckInDMO { get; set; }
        public DbSet<INV_Asset_DisposeDMO> INV_Asset_DisposeDMO { get; set; }
        public DbSet<INV_Asset_TransferDMO> INV_Asset_TransferDMO { get; set; }
        public DbSet<INV_ConfigurationDMO> INV_ConfigurationDMO { get; set; }
        public DbSet<INV_StockDMO> INV_StockDMO { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYear { get; set; }


        //==============================ASSET TAG DMO========================
        public DbSet<INV_Asset_AssetTagDMO> INV_Asset_AssetTagDMO { get; set; }
        public DbSet<INV_AssetTag_CheckOutDMO> INV_AssetTag_CheckOutDMO { get; set; }
        public DbSet<INV_AssetTag_CheckInDMO> INV_AssetTag_CheckInDMO { get; set; }
        public DbSet<INV_AssetTag_DisposeDMO> INV_AssetTag_DisposeDMO { get; set; }
        public DbSet<INV_AssetTag_TransferDMO> INV_AssetTag_TransferDMO { get; set; }

        //============================ SMS AND EMAIL===========================
        public DbSet<Multiple_Mobile_DMO> Multiple_Mobile_DMO { get; set; }
        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }


        //sanjai
        public DbSet<ISM_Master_ProjectDMO> ISM_Master_ProjectDMO { get; set; }
        public DbSet<ISM_Master_Client_DMO> ISM_Master_Client_DMO { get; set; }
        public DbSet<MasterSystemDMO> MasterSystemDMO { get; set; }
        public DbSet<AT_Master_CategoryDMO> AT_Master_CategoryDMO { get; set; }
        public DbSet<AT_Master_CheckPointDMO> AT_Master_CheckPointDMO { get; set; }
        public DbSet<AT_System_Category_Checkpoint_MappingDMO> AT_System_Category_Checkpoint_MappingDMO { get; set; }
        public DbSet<SystemCategoryCheckpointDto> SystemCategoryCheckpointDto { get; set; }
        public DbSet<Health_Entry_StatusDTO> Health_Entry_Status { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {

            }
        }
    }
}