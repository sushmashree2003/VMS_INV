using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{
    [Table("Tenant_NoticeBoard")]
    public class TenantNoticeBoardDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENENTNTB_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMB_Id { get; set; }
        public string TENENTNTB_Title { get; set; }
        public string TENENTNTB_Description { get; set; }
        public DateTime? TENENTNTB_DisplayDate { get; set; }
        public DateTime TENENTNTB_StartDate { get; set; }
        public DateTime TENENTNTB_EndDate { get; set; }
        public string TENENTNTB_AllOrIndFlg { get; set; }
        public bool TENENTNTB_ActiveFlag { get; set; }
        public DateTime? TENENTNTB_CreatedDate { get; set; }
        public DateTime? TENENTNTB_UpdatedDate { get; set; }

        public List<TenantNoticeBoardFilesDMO> TenantNoticeBoardFilesDMO { get; set; }
        public List<TenantNoticeBoardTenantDMO> TenantNoticeBoardTenantDMO { get; set; }

    }
}
