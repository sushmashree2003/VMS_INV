using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_Staff_Transaction_Details")]
 public   class Feedback_Staff_Transaction_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSTTRD_Id { get; set; }        
        public long FSTTR_Id { get; set; }
        public long FMQE_Id { get; set; }
        public long FMOP_Id { get; set; }
        public string FSTTRD_FeedBack { get; set; }
        public bool FSTTRD_ActiveFlag { get; set; }
        public DateTime FSTTRD_CreatedDate { get; set; }
        public DateTime FSTTRD_UpdatedDate { get; set; }
        public long FSTTRD_CreatedBy { get; set; }
        public long FSTTRD_UpdatedBy { get; set; }
      
    }
}
