using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam.LessonPlanner
{
    [Table("Lesson_Planner_Subject_Topic_Mapping", Schema = "Exm")]
    public class SchoolSubjectWithMasterTopicMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPSMTM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long LPMT_Id { get; set; }  
        public int LPSMTM_Order { get; set; }
        public bool LPSMTM_ActiveFlag { get; set; }
        public long LPSMTM_CreatedBy { get; set; }
        public long LPSMTM_UpdatedBy { get; set; }
    }
}
