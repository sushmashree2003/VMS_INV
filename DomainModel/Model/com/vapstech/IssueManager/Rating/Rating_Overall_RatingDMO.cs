using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Rating_Overall_Rating")]
    public class Rating_Overall_RatingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RATOVERR_Id { get; set; }
        public long MI_Id { get; set; }
        public decimal RATOVERR_FromValue { get; set; }
        public decimal RATOVERR_ToValue { get; set; }
        public decimal RATOVERR_Rating { get; set; }
        public bool RATOVERR_ActiveFlag { get; set; }
        public long RATOVERR_CreatedBy { get; set; }
        public DateTime RATOVERR_CreatedDate { get; set; }
        public long RATOVERR_UpdatedBy { get; set; }
        public DateTime RATOVERR_UpdatedDate { get; set; }
        

    }
}
