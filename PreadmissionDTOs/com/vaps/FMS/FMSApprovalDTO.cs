using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class FMSApprovalDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }


        public long? MI_Id { get; set; }
        public long? UserId { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public Array viewlist { get; set; }
        
        public long? HRME_Id { get; set; }
        public long? FMSFLREQ_Id { get; set; }
        public bool? ApprovedFlag { get; set; }
        public long? ApprovedDays { get; set; }
        public string Remark { get; set; }
        public grid_array[] grid_arrays { get; set; }
        public bool approved_flag { get; set; }
        
        public string Email { get; set; }
        public string HRLP_EmailTo { get; set; }

        public Array emaildetails { get; set; }


    }
    public class grid_array
    {
        public long? HRME_Id { get; set; }
        public long? FMSFLREQ_Id { get; set; }
        public long? Approveddays { get; set; }
        public string remarks { get; set; }
        public int approved_flag { get; set; }
        public long? UserId { get; set; }

    }
}
