using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{
    [Table("Tenant_NoticeBoard_Tenant_Viewed")]
    public class TenantNoticeBoardTenantViewedDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENANTNTBTNTV_Id { get; set; }
        public long TENENTNTB_Id { get; set; }
        public long TENANTAGR_Id { get; set; }
        public DateTime TENANTNTBTNTV_ViewedDateTime { get; set; }
        public bool TENANTNTBTNTV_ActiveFlag { get; set; }
        public DateTime? TENANTNTBTNTV_CreatedDate { get; set; }
        public DateTime? TENANTNTBTNTV_UpdatedDate { get; set; }
        public long? TENANTNTBTNTV_CreatedBy { get; set; }
        public long? TENANTNTBTNTV_UpdatedBy { get; set; }
    }
}
