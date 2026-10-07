using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.IssueManager.HWHealthCheckUp
{
    [Table("HW_HealthCheckUp_Approval")]
    public class HW_HealthCheckUp_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HWHCHUPA_Id { get; set; }
        public long HWHCHUP_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long HWHCHUPA_LevelNo { get; set; }
        public string HWHCHUPA_Remarks { get; set; }
        public string HWHCHUPA_Status { get; set; }
        public DateTime HWHCHUPA_Date { get; set; }
        public bool HWHCHUPA_FinalFlag { get; set; }
        public long HWHCHUPA_CreatedBy { get; set; }
        public DateTime HWHCHUPA_CreatedDate { get; set; }
        public long HWHCHUPA_UpdatedBy { get; set; }
        public DateTime HWHCHUPA_UpdatedDate { get; set; }
        
    }
}
