using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Rating_Type_Questions")]
    public class Rating_Type_QuestionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long RATMTQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long RATMT_Id { get; set; }
        public long RATMQE_Id { get; set; }
        public int RATMTQ_TQOrder { get; set; }
        public bool? RATMTQ_ActiveFlag { get; set; }
        public long? RATMTQ_CreatedBy { get; set; }
        public DateTime? RATMTQ_CreatedDate { get; set; }
        public long? RATMTQ_UpdatedBy { get; set; }
        public DateTime? RATMTQ_UpdatedDate { get; set; }
        public List<Rating_Type_Questions_DepartmentDMO> Rating_Type_Questions_DepartmentDMO { get; set; }
        public List<Rating_Type_Questions_OptionsDMO> Rating_Type_Questions_OptionsDMO { get; set; }
    }
}
