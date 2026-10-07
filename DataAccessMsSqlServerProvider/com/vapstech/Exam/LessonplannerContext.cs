using DomainModel.Model.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Exam.LessonPlanner;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessMsSqlServerProvider.com.vapstech.Exam
{
    public class LessonplannerContext : DbContext
    {
        public LessonplannerContext(DbContextOptions<LessonplannerContext> options) : base(options)
        { }

        public DbSet<MasterSchoolTopicDMO> MasterSchoolTopicDMO { get; set; }
        public DbSet<SchoolSubjectWithMasterTopicMappingDMO> SchoolSubjectWithMasterTopicMapping { get; set; }
        public DbSet<IVRM_School_Master_SubjectsDMO> IVRM_School_Master_SubjectsDMO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<MasterSchoolTopicDMO>().HasKey(m => m.LPMT_Id);
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<MasterSchoolTopicDMO>();

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
