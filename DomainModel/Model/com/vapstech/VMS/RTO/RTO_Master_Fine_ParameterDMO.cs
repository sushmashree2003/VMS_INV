using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.RTO
{
    [Table("RTO_Master_Fine_Parameter")]
    public class RTO_Master_Fine_ParameterDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RTOMFP_Id { get; set; }
        public string RTOMFP_FineParameterName { get; set; }
        public decimal RTOMFP_FineAmount { get; set; }
        public long RTOMFP_MaxViolationPermitted { get; set; }
        public string RTOMFP_ParameterDescription { get; set; }
        public bool RTOMFP_ActiveFlag { get; set; }
        public DateTime? RTOMFP_CreatedDate { get; set; }
        public DateTime? RTOMFP_UpdatedDate { get; set; }
        public long RTOMFP_CreatedBy { get; set; }
        public long RTOMFP_UpdatedBy { get; set; }

    }
}
