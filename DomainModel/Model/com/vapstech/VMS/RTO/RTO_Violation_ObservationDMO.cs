using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.RTO
{
    [Table("RTO_Violation_Observation")]
    public class RTO_Violation_ObservationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RTOVOB_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TRMV_Id { get; set; }
        public DateTime? RTOVOB_Date { get; set; }
        public bool? RTOVOB_AnyViolationFlg { get; set; }
        public string RTOVOB_DateTimeOfViolation { get; set; }
        public long RTOMFP_Id { get; set; }
        public string RTOVOB_ViolationDescription { get; set; }
        public decimal RTOVOB_FineAmount { get; set; }
        public bool RTOVOB_ActiveFlag { get; set; }
        public bool RTOVOB_FineAmountPaidFlg { get; set; }
        public DateTime? RTOVOB_CreatedDate { get; set; }
        public DateTime? RTOVOB_UpdatedDate { get; set; }
        public long RTOVOB_CreatedBy { get; set; }
        public long RTOVOB_UpdatedBy { get; set; }
      
        public List<RTO_Violation_Observation_FileDMO> RTO_Violation_Observation_FileDMO { get; set; }
    }
}
