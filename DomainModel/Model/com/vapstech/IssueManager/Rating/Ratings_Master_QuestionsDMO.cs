using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Ratings_Master_Questions")]
    public class Ratings_Master_QuestionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long RATMQE_Id { get; set; }
        public long MI_Id { get; set; }
        public string RATMQE_RatingQuestions { get; set; }
        public bool? RATMQE_ManualEntryFlg { get; set; }
        public string RATMQE_RatingsQRemarks { get; set; }
        public int RATMQE_RQOrder { get; set; }
        public bool? RATMQE_ActiveFlag { get; set; }
        public long? RATMQE_CreatedBy { get; set; }
        public DateTime? RATMQE_CreatedDate { get; set; }
        public long? RATMQE_UpdatedBy { get; set; }
        public DateTime? RATMQE_UpdatedDate { get; set; }
        public bool? RATMQE_AutomaticFlg { get; set; }
        public string RATMQE_QuestionFlg { get; set; }
        public long? RATMQECLSUB_Id { get; set; }
    }
}
