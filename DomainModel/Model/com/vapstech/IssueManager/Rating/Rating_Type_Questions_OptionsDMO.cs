using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Rating_Type_Questions_Options")]
    public class Rating_Type_Questions_OptionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RATMTQO_Id { get; set; }
        public long RATMTQ_Id { get; set; }
        public long RATMOP_Id { get; set; }
        public int RATMTQO_TOOOrder { get; set; }
        public bool RATMTQO_ActiveFlag { get; set; }
        public long? RATMTQO_CreatedBy { get; set; }
        public DateTime? RATMTQO_CreatedDate { get; set; }
        public long? RATMTQO_UpdatedBy { get; set; }
        public DateTime? RATMTQO_UpdatedDate { get; set; }
    }
}
