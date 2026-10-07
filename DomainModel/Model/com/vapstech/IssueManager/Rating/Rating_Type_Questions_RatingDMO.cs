using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Rating_Type_Questions_Rating")]
    public class Rating_Type_Questions_RatingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long RATMTQR_Id { get; set; }
        public long RATMTQ_Id { get; set; }
        public long RATMT_Id { get; set; }
        public long RATMQE_Id { get; set; }
        public decimal RATMTQR_FromValue { get; set; }
        public decimal RATMTQR_ToValue { get; set; }
        public string RATMTQR_Rating { get; set; }
        public bool RATMTQR_ActiveFlag { get; set; }
        public long RATMTQR_CreatedBy { get; set; }
        public DateTime RATMTQR_CreatedDate { get; set; }
        public long RATMTQR_UpdatedBy { get; set; }
        public DateTime RATMTQR_UpdatedDate { get; set; }
    }
}
