using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Tenants
{
    public class TenantPaidandPendinglistDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long roleId { get; set; }
        public long LogInUserId { get; set; }
        public Array monthdropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public long? monthid { get; set; }
        public DateTime TENANTAGR_AgrFromDate { get; set; }
        public DateTime TENANTAGR_AgrToDate { get; set; }
        public string type { get; set; }
        public string flag { get; set; }
        public Array getloaddata { get; set; }
        public long[] tenantid { get; set; }
        public long HRMLY_LeaveYear { get; set; }
        public string start_Date { get; set; }
        public string end_Date { get; set; }
        public Boolean tenantagrpaY_Flag { get; set; }
        public long? month { get; set; }
        public long? year { get; set; }
        public Array tenantpendingreport { get; set; }
        public string Tenant_Name { get; set; }
        public string TENANTAGR_EmailId { get; set; }
        public string TENANTAGR_ContactNo { get; set; }
        public string TENANTAGR_Address { get; set; }
        public string Buildingname { get; set; }
        public string FlatNo { get; set; }
        public tenantpendingpaymentlistarrs[] tenantpendingpaymentlistarr { get; set; }

        public class tenantpendingpaymentlistarrs
        {
            public long TENANTAGR_Id { get; set; }
            public string IVRM_Month_Name { get; set; }
            public long TENANTEBWB_Month { get; set; }
            public long TENANTEBWB_Year { get; set; }
            public decimal Rentbill_Amount { get; set; }
            public decimal Waterbill_Amount { get; set; }
            public decimal Electricalbill_Amount { get; set; }
        }


    }
}
