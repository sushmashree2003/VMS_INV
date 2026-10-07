using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.SpaceBooking;

using DomainModel.Model.com.vapstech.VMS.Training;

using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vaps.admission;

namespace DataAccessMsSqlServerProvider.com.vapstech.SpaceBooking
{
    public class SpaceBookingContext : DbContext
    {
        public SpaceBookingContext(DbContextOptions<SpaceBookingContext> options) : base(options)
        { Database.SetCommandTimeout(30000); }

        public DbSet<HR_Master_Amenities_DMO> HR_Master_Amenities_DMO_con { get; set; }
        public DbSet<SMSEmailSetting> smsEmailSetting { get; set; }
        public DbSet<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public DbSet<SMS_MAIL_PARAMETER_DMO> SMS_MAIL_PARAMETER_DMO { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<EMAIL_DETAILS_DMO> EMAIL_DETAILS_DMO { get; set; }
        public DbSet<GeneralConfigDMO> GenConfig { get; set; }
        public DbSet<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public DbSet<FeePaymentDetailsDMO> FeePaymentDetailsDMO_con { get; set; }       
        public DbSet<MasterAcademic> AcademicYear { get; set; }
        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<State> state { get; set; }
        public DbSet<Caste> caste { get; set; }        
        public DbSet<OS_EnquiryDMO> OS_EnquiryDMO { get; set; }
        public DbSet<Country> IVRM_Master_Country { get; set; }
        public DbSet<State> IVRM_Master_State { get; set; }
        public DbSet<Office_Space_User_Registration> Office_Space_User_Registration { get; set; }
        public DbSet<OS_Space_Booking> OS_Space_Booking { get; set; }
        public DbSet<HR_Master_Room> HR_Master_Room_con { get; set; }
        public DbSet<HR_Master_Floor> HR_Master_Floor_con { get; set; }
        public DbSet<HR_Master_Building> HR_Master_Building_con { get; set; }
        public DbSet<HR_Master_Account_DetailsDMO> HR_Master_Account_DetailsDMO { get; set; }
        public DbSet<PAYUDETAILS> PAYUDETAILS { get; set; }

        public DbSet<HR_Master_Room> HR_Master_Room  { get; set; }
        public DbSet<HR_Master_Room_ContactsDMO> HR_Master_Room_ContactsDMO { get; set; }

        public DbSet<Fee_PaymentGateway_DetailsDMO> Fee_PaymentGateway_DetailsDMO { get; set; }
        public DbSet<OS_User_Registration_InternalDMO> OS_User_Registration_InternalDMO { get; set; }

        public DbSet<Multiple_Email_DMO> Multiple_Email_DMO { get; set; }
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
