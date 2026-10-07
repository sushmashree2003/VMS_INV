using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Rating_TaskCategory_Transaction")]
    public class Rating_TaskCategory_TransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RATTCT_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long RATMQE_Id { get; set; }
        public long HRME_Id { get; set; }
        public int RATTCT_Value { get; set; }
        public string RATTCT_Flag { get; set; }
        public DateTime? RATTCT_Feedbackdate { get; set; }
        public long RATTCT_Feedback_UserId { get; set; }
        public bool RATTCT_ActiveFlag { get; set; }
        public long? RATTCT_CreatedBy { get; set; }
        public DateTime? RATTCT_CreatedDate { get; set; }
        public long? RATTCT_UpdatedBy { get; set; }
        public DateTime? RATTCT_UpdatedDate { get; set; }
    }
}
