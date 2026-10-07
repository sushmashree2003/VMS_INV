using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.COE;

namespace DataAccessMsSqlServerProvider.com.vapstech.Alumni
{
   public class AlumniContext : DbContext
    {
        public AlumniContext(DbContextOptions<AlumniContext> options) : base(options)
        { Database.SetCommandTimeout(30000); }

        public DbSet<MasterAcademic> AcademicYear { get; set; }

        public DbSet<MasterRoleType> MasterRoleType { get; set; }
        public DbSet<State> state { get; set; }

        public DbSet<Caste> caste { get; set; }

        public DbSet<CLGAlumniUserRegistrationDMO> CLGAlumniUserRegistrationDMO { get; set; }
        public DbSet<Religion> religion { get; set; }
        public DbSet<CasteCategory> castecategory { get; set; }
        public DbSet<MasterConfiguration> masterConfig { get; set; }
      
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<Institution> Inst { get; set; }
        public DbSet<School_M_Section> Section { get; set; }

        public DbSet<Adm_M_Student> AdmissionStudentDMO { get; set; }

        public DbSet<AlumniUserRegistrationDMO> AlumniUserRegistrationDMO { get; set; }

        public DbSet<Alumni_User_LoginDMO> Alumni_User_LoginDMO { get; set; }

        public DbSet<CLGAlumni_User_LoginDMO> CLGAlumni_User_LoginDMO { get; set; }

        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        public DbSet<COE_Master_EventsDMO> COE_Master_EventsDMO { get; set; }

        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        public DbSet<COE_EventsDMO> COE_EventsDMO { get; set; }
        public DbSet<Master_Numbering> Master_Numbering { get; set; }

        public DbSet<MasterConfiguration> mstConfig { get; set; }

        public DbSet<GeneralConfigDMO> GenConfig { get; set; }


        public DbSet<Alumni_M_StudentDMO> Alumni_M_StudentDMO { get; set; }

        public DbSet<Adm_Master_College_StudentDMO> Adm_Master_College_StudentDMO { get; set; }

        public DbSet<CLGAlumni_M_StudentDMO> CLGAlumni_M_StudentDMO  { get; set; }

        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }

        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }

        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }

        public DbSet<School_M_Class> School_M_Class  { get; set; }
         
        public  DbSet<Country> Country { get; set; }

        public DbSet<Alumini_details> Alumini_details { get; set; }

        public DbSet<CLGAlumni_College_Student_Profession> CLGAlumni_College_Student_Profession { get; set; }
        public DbSet<CLGAlumni_College_Student_Qulaification> CLGAlumni_College_Student_Qulaification { get; set; }
        public DbSet<CLGAlumni_College_Student_Achivement> CLGAlumni_College_Student_Achivement { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //base.OnModelCreating(builder);
            //builder.Entity<FEeGroupLoginPreviledgeDMO>().ToTable("Fee_Group_Login_Previledge");
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
