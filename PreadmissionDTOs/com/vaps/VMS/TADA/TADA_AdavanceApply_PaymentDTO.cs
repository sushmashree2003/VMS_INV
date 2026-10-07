using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
   public class TADA_AdavanceApply_PaymentDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array GetReport { get; set; }
        public bool returnvalue { get; set; }
        public long VTADAAA_Id { get; set; }
        public string returnval { get; set; }
        public string VTADAAAP_PaymentMode { get; set; }
        public decimal VTADAAAP_AmountPaid { get; set; }
        public string VTADAAAP_PaymentRefNo { get; set; }
        public string VTADAAAP_Remarks { get; set; }
        public DateTime? VTADAAAP_Date { get; set; }
        public Array Adavance { get; set; }
        public long VTADAAAP_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string IVRMMCT_Name { get; set; }
        public Array get_emp { get; set; }
        public long HRME_Id { get; set; }
        public Array Veiwdetails { get; set; }
        public Array EditArray { get; set; }
        public Array Edit_Two { get; set; }
        public bool VTADAAAP_ActiveFlg { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string MI_Name { get; set; }
        public DateTime? VTADAAAP_CreatedDate { get; set; }
        //VTADASLEAP_Id
        public long VTADASLEAP_Id { get; set; }

    }
}
