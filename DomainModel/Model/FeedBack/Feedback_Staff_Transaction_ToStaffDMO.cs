using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_Staff_Transaction_ToStaff")]
   public class Feedback_Staff_Transaction_ToStaffDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSTTRTST_Id { get; set; }
        public long FSTTR_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool FSTTRTST_ActiveFlag { get; set; }
        public DateTime FSTTRTST_CreatedDate { get; set; }
        public DateTime FSTTRTST_UpdatedDate { get; set; }
        public long FSTTRTST_CreatedBy { get; set; }
        public long FSTTRTST_UpdatedBy { get; set; }    

    }
}
