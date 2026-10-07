using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Rating_Master_Type")]
    public class Rating_Master_TypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RATMT_Id { get; set; }
        public long MI_Id { get; set; }
        public string RATMT_RatingTypeName { get; set; }
        public bool? RATMT_DepartmentwiseFlg { get; set; }
        public string RATMT_PeriodicityFlg { get; set; }
        public bool? RATMT_QuestionwiseOptionFlg { get; set; }
        public bool? RATMT_ActiveFlag { get; set; }
        public long? RATMT_CreatedBy { get; set; }
        public DateTime? RATMT_CreatedDate { get; set; }
        public long? RATMT_UpdatedBy { get; set; }
        public DateTime? RATMT_UpdatedDate { get; set; }
        public bool? RATMT_LInkedToRelievingFlg { get; set; }
    }
}
