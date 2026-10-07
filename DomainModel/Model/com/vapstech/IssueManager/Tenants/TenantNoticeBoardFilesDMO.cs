using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{
    [Table("Tenant_NoticeBoard_Files")]
    public class TenantNoticeBoardFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENENTNTBFL_Id { get; set; }
        public long TENENTNTB_Id { get; set; }
        public string TENENTNTBFL_FileName { get; set; }
        public string TENENTNTBFL_FilePath { get; set; }
        public bool TENENTNTBFL_ActiveFlag { get; set; }
        public DateTime? TENENTNTBFL_CreatedDate { get; set; }
        public DateTime? TENENTNTBFL_UpdatedDate { get; set; }
        public long? TENENTNTBFL_CreatedBy { get; set; }
        public long? TENENTNTBFL_UpdatedBy { get; set; }
    }
}
