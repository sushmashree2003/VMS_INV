using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.SpaceBooking
{
    [Table("OS_Enquiry")]
    public class OS_EnquiryDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     
       public long OSENQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long? OSUREG_Id { get; set; }
        public string OSENQ_Name { get; set; }
        public string OSENQ_EmailId { get; set; }
        public long OSENQ_MobileNo { get; set; }
        public string OSENQ_Type { get; set; }
        public string OSENQ_Enquiry { get; set; }
        public string OSENQ_Status { get; set; }
        public DateTime? OSENQ_VisitDate { get; set; }
        public string OSENQ_VisitTime { get; set; }
        public bool OSENQ_ActiveFlag { get; set; }
        public DateTime OSENQ_CreatedDate { get; set; }
        public DateTime OSENQ_UpdatedDate { get; set; }
        public long OSENQ_CreatedBy { get; set; }
        public long OSENQ_UpdatedBy { get; set; }

    }
}
