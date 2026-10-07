using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{
    [Table("Tenant_ElectricityBill_WaterBill")]
    public class TenantElectricalWaterBillDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENANTEBWB_Id { get; set; }
        public long TENANTAGR_Id { get; set; }

        public string TENANTEBWB_Flg { get; set; }

        public long TENANTEBWB_Year { get; set; }

        public long? TENANTEBWB_Month { get; set; }

        public decimal TENANTEBWB_Amount { get; set; }
        public bool TENANTEBWB_ActiveFlag { get; set; }
        public DateTime TENANTEBWB_CreatedDate { get; set; }

        public DateTime TENANTEBWB_UpdatedDate { get; set; }
        public long TENANTEBWB_CreatedBy { get; set; }
        public long TENANTEBWB_UpdatedBy { get; set; }
        public string TENANTEBWB_Remarks { get; set; }
    }
}
