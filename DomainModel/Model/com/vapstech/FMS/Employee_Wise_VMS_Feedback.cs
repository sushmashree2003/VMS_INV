using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("VMS_Emp_FeedBack")]
    public class Employee_Wise_VMS_Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VEF_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime ? VEF_FeedBackDate { get; set; }
        public  string VEF_Remarks { get; set; }
        public bool VEF_NegativeFlag { get; set; }
        public bool VEF_ActiveFlag { get; set; }
        public DateTime ? VEF_CreatedDate { get; set; }
         public DateTime ? VEF_UpdatedDate { get; set; }
        public long VEF_CreatedBy { get; set; }
        public long VEF_UpdatedBy { get; set; }
       
    }
}
