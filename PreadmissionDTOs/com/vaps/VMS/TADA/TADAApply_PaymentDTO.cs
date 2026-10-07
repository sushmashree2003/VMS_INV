using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
   public class TADAApply_PaymentDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long? VTADAAA_Id { get; set; }
        public Array GetReport { get; set; }
        public bool returnvalue { get; set; }
        public string returnval { get; set; }
        public long VTADAA_Id { get; set; }
        public string VTADAAP_PaymentMode { get; set; }
        public decimal VTADAAP_AmountPaid { get; set; }
        public string VTADAAP_PaymentRefNo { get; set; }
        public string VTADAAP_Remarks { get; set; }
        public DateTime? VTADAAP_Date { get; set; }
        public long VTADAAP_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string IVRMMCT_Name { get; set; }
        public Array Adavance { get; set; }
        public bool VTADAAP_ActiveFlg { get; set; }
        public Array EditArray { get; set; }
        public Array Edit_Two { get; set; }
        public Array Veiwdetails { get; set; }
        public Array Veiwdetailstt { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string MI_Name { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? VTADAAP_CreatedDate { get; set; }
        public string VTADAA_BalanceHRMS { get; set; }
       
    }
}
