using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Rating_Master_Options")]
    public class Rating_Master_OptionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long RATMOP_Id { get; set; }
        public long MI_Id { get; set; }
        public string RATMOP_RatingOptions { get; set; }
        public long RATMOP_OptionsValue { get; set; }
        public string RATMOP_RatingORemarks { get; set; }
        public int RATMOP_ROOrder { get; set; }
        public bool RATMOP_ActiveFlag { get; set; }
        public long? RATMOP_CreatedBy { get; set; }
        public DateTime? RATMOP_CreatedDate { get; set; }
        public long? RATMOP_UpdatedBy { get; set; }
        public DateTime? RATMOP_UpdatedDate { get; set; }
    }
}
