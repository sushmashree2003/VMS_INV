using DomainModel.Model.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.VMS.RTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccessMsSqlServerProvider.com.vapstech.IssueManager
{
    public class RTOContext : DbContext
    {
        public RTOContext(DbContextOptions<RTOContext> options) : base(options)
        {
            { Database.SetCommandTimeout(900000000); }
        }
        public DbSet<RTO_Master_Fine_ParameterDMO> RTO_Master_Fine_ParameterDMO { get; set; }
        //RTO_Violation_ObservationDMO
        public DbSet<RTO_Violation_ObservationDMO> RTO_Violation_ObservationDMO { get; set; }
        public DbSet<RTO_Violation_Observation_FileDMO> RTO_Violation_Observation_FileDMO { get; set; }
        public DbSet<VMS_Vehicle_KeysDMO> VMS_Vehicle_KeysDMO { get; set; }
        public DbSet<Master_VehicleDMO> Master_VehicleDMO { get; set; }
        public DbSet<VMS_FO_Vehicle_ObservationDMO> VMS_FO_Vehicle_ObservationDMO { get; set; }
        public DbSet<VMS_FO_Vehicle_Observation_FilesDMO> VMS_FO_Vehicle_Observation_FilesDMO { get; set; }
        //
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

            
        }
    }
}
