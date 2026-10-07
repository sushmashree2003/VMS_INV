using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Portals.Tenants
{
    [Table("Tenant_Monthly_WaterBills_RoomWise")]
    public class Tenant_Monthly_WaterBills_RoomWiseDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENANTMWBILLRW_Id { get; set; }
        public long TENANTMWBILL_Id { get; set; }
        public long HRMR_ID { get; set; }
        
        public decimal TENANTMWBILLRW_Amount { get; set; }
        public string TENANTMWBILLRW_Remarks { get; set; }
        public decimal? TENANTMWBILLRW_AmountPaid { get; set; }
        public bool? TENANTMWBILLRW_PaidFlg { get; set; }
        public bool TENANTMWBILLRW_ActiveFlg { get; set; }
        public DateTime? TENANTMWBILLRW_CreatedDate { get; set; }
        public DateTime? TENANTMWBILLRW_UpdatedDate { get; set; }
        public long? TENANTMWBILLRW_CreatedBy { get; set; }
        public long? TENANTMWBILLRW_UpdatedBy { get; set; }


    }
}
