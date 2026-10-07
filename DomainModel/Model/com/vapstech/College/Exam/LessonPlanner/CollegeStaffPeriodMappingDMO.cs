using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Exam.LessonPlanner
{
    [Table("Exm_Lesson_Planner_Staff_Work_Allocation", Schema = "CLG")]
    public class CollegeStaffPeriodMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPCSWA_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long LPMT_Id { get; set; }
        public DateTime LPCSWA_AllocatedDate { get; set; }
        public DateTime LPCSWA_TakenDate { get; set; }
        public bool LPCSWA_Flag { get; set; }
        public bool LPCSWA_ActiveFlag { get; set; }
        public long LPCSWA_CreatedBy { get; set; }
        public long LPCSWA_UpdatedBy { get; set; }
    }
}
