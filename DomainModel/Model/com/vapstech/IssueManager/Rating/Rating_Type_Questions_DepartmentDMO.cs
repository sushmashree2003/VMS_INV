using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Rating_Type_Questions_Department")]
    public class Rating_Type_Questions_DepartmentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RATMTQD_Id { get; set; }
        public long RATMTQ_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public bool RATMTQD_ActiveFlag { get; set; }
        public long? RATMTQD_CreatedBy { get; set; }
        public DateTime? RATMTQD_CreatedDate { get; set; }
        public long? RATMTQD_UpdatedBy { get; set; }
        public DateTime? RATMTQD_UpdatedDate { get; set; } 
    }
}
