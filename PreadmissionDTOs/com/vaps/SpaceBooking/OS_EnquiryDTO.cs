using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.SpaceBooking
{
    public class OS_EnquiryDTO 
    {
        public long OSENQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMMC_Id { get; set; }
        public long User_Id { get; set; }
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
        public bool retrval { get; set; }
        public Array enqlist { get; set; }
        public Array statelist { get; set; }
        public Array countrylist { get; set; }

    }
}
