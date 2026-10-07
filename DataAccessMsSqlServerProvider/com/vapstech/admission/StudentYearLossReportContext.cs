using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessMsSqlServerProvider.com.vapstech.admission
{
    public class StudentYearLossReportContext: DbContext
    {
        public StudentYearLossReportContext(DbContextOptions<StudentYearLossReportContext> options) :base(options)
        { }
        public DbSet<MasterAcademic> AcademicYear { get; set; }

        public DbSet<School_M_Class> admissioncls { get; set; }

        public DbSet<School_M_Section> school_M_Section { get; set; }

        public DbSet<Adm_M_Student> AdmissionStudent { get; set; }

        public DbSet<StudentTC> studenttc { get; set; }

        public DbSet<School_Adm_Y_StudentDMO> SchoolAdmYStudent { get; set; }
        public DbSet<AdmissionStandardDMO> standarad { get; set; }
        public DbSet<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
        public DbSet<Masterclasscategory> Masterclasscategory { get; set; }
    }
}
