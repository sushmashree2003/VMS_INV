using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("HW_HealthCheckUp_Feedback")]
    public class HW_HealthCheckUp_FeedbackDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HWHCFB_Id { get; set; }
        public long HWHCHUP_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HWHCFB_Feedback { get; set; }
        public bool HWHCFB_SatisfiedFlg { get; set; }
        public bool HWHCFB_ActiveFlag { get; set; }
        public long? HWHCFB_CreatedBy { get; set; }
        public long? HWHCFB_UpdatedBy { get; set; }
        public DateTime HWHCFB_CreatedDate { get; set; }
        public DateTime HWHCFB_UpdatedDate { get; set; }
    }
}
