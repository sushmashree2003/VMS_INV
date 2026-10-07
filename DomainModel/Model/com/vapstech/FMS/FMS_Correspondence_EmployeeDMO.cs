using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Correspondence_Employee")]
    public class FMS_Correspondence_EmployeeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSCOREMP_Id { get; set; }
        public long FMSCOR_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool FMSCOREMP_ActiveFlg { get; set; }
        public DateTime? FMSCOREMP_CreatedDate { get; set; }
        public DateTime? FMSCOREMP_UpdatedDate { get; set; }
        public long? FMSCOREMP_CreatedBy { get; set; }
        public long? FMSCOREMP_UpdatedBy { get; set; }
    }
}
