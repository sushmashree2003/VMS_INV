using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Tenants
{
   public class TenantOfflinePaymentReportDTO
    {
        public long MI_Id { get; set; }
        public Array yeardata { get; set; }
        public Array monthdata { get; set; }
        public Array displayTableOneDetails { get; set; }
        public string month { get; set; }
        public decimal amount { get; set; }
        public string duedate { get; set; }
        public DateTime? paiddate { get; set; }
        public long TENANTAGR_Id { get; set; }
        public long Month_id { get; set; }
        public Array editfiles { get; set; }
        public Array editlist { get; set; }
        public Array tenantlist { get; set; }
        public string name { get; set; }
        public string months { get; set; }
        public decimal amounts { get; set; }
        public decimal advance { get; set; }
        public string building { get; set; }
        public string tenantname { get; set; }
        public DateTime? paiddates { get; set; }
        public DateTime? TENANTAGRPAY_PaymentDate { get; set; }
        public decimal TENANTAGRPAY_Amount { get; set; }
        public string TENANTAGRPAY_ReceiptNo { get; set; }
        
    public long TENANTAGRPAY_Id { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }
        public DateTime agreementfromdate { get; set; }
        public DateTime agreementtodate { get; set; }
        public long IVRM_Month_Id { get; set; }
        public long TENANTAGRPAY_PaymentYear { get; set; }
        public long monthss { get; set; }
        public long HRMB_ID { get; set; }
        public string TENANTAGRPAY_Flag { get; set; }

        public monthlist[] monthlist { get; set; }
        public string type { get; set; }
        public long yrrrrs { get; set; }
        public string flag { get; set; }
        public string MI_Logo { get; set; }
        public string IVRM_Month_Name { get; set; }



    }
    public class monthlist {
        public long IVRM_Month_Id { get; set; }
    }
   // public class type { public string TENANTAGRPAY_Flag { get; set; }  }
}
