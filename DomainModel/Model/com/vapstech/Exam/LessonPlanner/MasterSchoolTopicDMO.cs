using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Lesson_Planner_Master_Topic", Schema = "Exm")]
    public class MasterSchoolTopicDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMT_Id { get; set; }
        public long MI_Id { get; set; }
        public string LPMT_TopicName { get; set; }
        public string LPMT_TopicDescription { get; set; }
        public int LPMT_TopicPeriods { get; set; }
        public int LPMT_Topicorder { get; set; }
        public bool LPMT_ActiveFlag { get; set; }
        public long LPMT_CreatedBy { get; set; }
        public long LPMT_UpdatedBy { get; set; }
       
    }

   
}
