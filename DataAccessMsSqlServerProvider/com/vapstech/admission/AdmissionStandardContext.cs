using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vaps.admission;

namespace DataAccessMsSqlServerProvider
{
    public class AdmissionStandardContext :DbContext
    {
         public AdmissionStandardContext(DbContextOptions<AdmissionStandardContext> options) :base(options)
        { }

        public DbSet<AdmissionStandardDMO> AdmissionStandardDMO { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AdmissionStandardDMO>()
               .ToTable("Adm_School_Configuration");

            // base.OnModelCreating(builder);
            //builder.Entity<Organisation>()
            //    .HasOne(s => s.IVRM_Master_City)
            //    .WithOne()
            //   .HasForeignKey<City>(s => s.IVRMMC_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<AdmissionStandardDMO>();

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
