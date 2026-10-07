using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Tenants
{
    public class TenantWaterBillDTO
    {
       
        public long MI_Id { get; set; }
        public Array years { get; set; }
        public Array tenant_array { get; set; }
        public Array monthnames { get; set; }
        public Array building { get; set; }
        public long TENANTMWBILL_Id { get; set; }
        public long HRMB_ID { get; set; }
        public long TENANTMWBILL_Year { get; set; }
        public long TENANTMWBILL_Month { get; set; }
        public decimal TENANTMWBILL_TotalAmount { get; set; }
        public bool TENANTMWBILL_ActiveFlg { get; set; }
        public DateTime TENANTMWBILL_CreatedDate { get; set; }
        public DateTime TENANTMWBILL_UpdatedDate { get; set; }
        public long TENANTMWBILL_CreatedBy { get; set; }
        public long TENANTMWBILL_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public string returnmsg { get; set; }
        public string HRMB_BuildingName { get; set; }
        public string IVRM_Month_Name { get; set; }
        public string returnval { get; set; }

        public Array tenantdata { get; set; }


    }
}
