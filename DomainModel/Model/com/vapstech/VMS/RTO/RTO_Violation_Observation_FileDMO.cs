using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.RTO
{
    [Table("RTO_Violation_Observation_File")]
    public class RTO_Violation_Observation_FileDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RTOVOBFL_Id { get; set; }
        public long RTOVOB_Id { get; set; }
        public string RTOVOBFL_FileName { get; set; }
        public string RTOVOBFL_FilePath { get; set; }
        public string RTOVOBFL_Remarks { get; set; }
        public bool RTOVOBFL_ActiveFlag { get; set; }
        public DateTime? RTOVOBFL_CreatedDate { get; set; }
        public long RTOVOBFL_CreatedBy { get; set; }
        public long RTOVOBFL_UpdatedBy { get; set; }
        public DateTime? RTOVOBFL_UpdatedDate { get; set; }


    }
}
