using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{
    [Table("Tenant_NoticeBoard_Tenant")]
    public class TenantNoticeBoardTenantDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENANTNTBTNT_Id { get; set; }
        public long TENENTNTB_Id { get; set; }
        public long TENANTAGR_Id { get; set; }
        public bool TENANTNTBTNT_ActiveFlag { get; set; }
        public DateTime? TENANTNTBTNT_CreatedDate { get; set; }
        public DateTime? TENANTNTBTNT_UpdatedDate { get; set; }
        public long? TENANTNTBTNT_CreatedBy { get; set; }
        public long? TENANTNTBTNT_UpdatedBy { get; set; }
    }
}
