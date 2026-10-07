using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Ratings_Master_Authority")]
    public class Rating_Master_AuthorityDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long RATMAUT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public bool? RATMAUT_ActiveFlag { get; set; }
        public long? RATMAUT_CreatedBy { get; set; }
        public DateTime? RATMAUT_CreatedDate { get; set; }
        public long? RATMAUT_UpdatedBy { get; set; }
        public DateTime? RATMAUT_UpdatedDate { get; set; }

    }
}
