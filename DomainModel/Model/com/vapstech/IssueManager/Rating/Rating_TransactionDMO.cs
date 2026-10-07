using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Rating_Transaction")]
    public class Rating_TransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RATTR_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? RATTR_FeedbackDate { get; set; }
        public long RATMT_Id { get; set; }
        public long RATMQE_Id { get; set; }
        public long RATMOP_Id { get; set; }
        public string RATTR_FeedBack { get; set; }
        public long RATTR_Feedback_UserId { get; set; }
        public bool RATTR_ActiveFlag { get; set; }
        public long? RATTR_CreatedBy { get; set; }
        public DateTime? RATTR_CreatedDate { get; set; }
        public long? RATTR_UpdatedBy { get; set; }
        public DateTime? RATTR_UpdatedDate { get; set; }
    }
}
