using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.IssueManager.HWHealthCheckUp
{
    [Table("HW_HealthCheckUp_Files")]
    public class HW_HealthCheckUp_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HWHCHUPF_Id { get; set; }
        public long HWHCHUP_Id { get; set; }
        public string HWHCHUPF_FileName { get; set; }
        public string HWHCHUPF_FilePath { get; set; }
        public long HWHCHUPF_CreatedBy { get; set; }
        public DateTime? HWHCHUPF_CreatedDate { get; set; }
        public long HWHCHUPF_UpdatedBy { get; set; }
        public DateTime? HWHCHUPF_UpdatedDate { get; set; }
    }
}
