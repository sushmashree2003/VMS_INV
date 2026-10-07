using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager.HWHealthCheckUp
{
    [Table("HW_HealthCheckUp")]
    public class HW_HealthCheckUpDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HWHCHUP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? HWHCHUP_Date { get; set; }
        public string HWHCHUP_Remarks { get; set; }
        public string HWHCHUP_Stauts { get; set; }
        public bool HWHCHUP_ActiveFlag { get; set; }
        public long HWHCHUP_CreatedBy { get; set; }
        public DateTime? HWHCHUP_CreatedDate { get; set; }
        public long HWHCHUP_UpdatedBy { get; set; }
        public DateTime? HWHCHUP_UpdatedDate { get; set; }
        public List<HW_HealthCheckUp_FilesDMO> HW_HealthCheckUp_FilesDMO { get; set; }
    }
}
