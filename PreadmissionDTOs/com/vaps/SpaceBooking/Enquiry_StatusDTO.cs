using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.SpaceBooking
{
   public class Enquiry_StatusDTO
    {
        public long OSENQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long? OSUREG_Id { get; set; }
        public string OSENQ_Name { get; set; }
        public string OSENQ_EmailId { get; set; }
        public long OSENQ_MobileNo { get; set; }
        public string OSENQ_Type { get; set; }
        public string OSENQ_Enquiry { get; set; }
        public string OSENQ_Status { get; set; }
        public DateTime? OSENQ_VisitDate { get; set; }
        public string OSENQ_VisitTime { get; set; }
        public bool OSENQ_ActiveFlag { get; set; }
        public DateTime OSENQ_CreatedDate { get; set; }
        public DateTime OSENQ_UpdatedDate { get; set; }
        public long OSENQ_CreatedBy { get; set; }
        public long OSENQ_UpdatedBy { get; set; }
        public Array enquirydetailList { get; set; }
        public string Type { get; set; }
        public Array datalist { get; set; }
        public Array edit_list { get; set; }

        //============
        public string Email { get; set; }
        public string SMS { get; set; }
        public string R_Email { get; set; }
        public string R_SMS { get; set; }
        public string bodyid { get; set; }
        public string footer { get; set; }
         
    }
}
