using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{
    [Table("Tenant_Monthly_WaterBills")]
    public class Tenant_Monthly_WaterBillsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENANTMWBILL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMB_ID { get; set; }
        public long TENANTMWBILL_Year { get; set; }
        public long TENANTMWBILL_Month { get; set; }
        public decimal TENANTMWBILL_TotalAmount { get; set; }
        public bool TENANTMWBILL_ActiveFlg { get; set; }
        public DateTime TENANTMWBILL_CreatedDate{ get; set; }
        public DateTime TENANTMWBILL_UpdatedDate { get; set; }
        public long  TENANTMWBILL_CreatedBy{ get; set; }
        public long TENANTMWBILL_UpdatedBy{ get; set; }
    }
}
