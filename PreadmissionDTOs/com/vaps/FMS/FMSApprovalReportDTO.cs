using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class FMSApprovalReportDTO
    {
        public long MI_Id { get; set; }
        public long id { get; set; }
        public long FMSFLREQ_Id { get; set; }
        public Array filename { get; set; }
        public Array institution { get; set; }
        public Array griddata { get; set; }
        public long UserId { get; set; }
        public string physicalname { get; set; }
        public long FMSMPFN_Id { get; set; }
        public long requireddayss { get; set; }
        public DateTime requesteddate { get; set; }
        public DateTime approveddate { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public long approveddayss { get; set; }
        public long fmsmpfn_id { get; set; }
        public long createdbyid { get; set; }
        public long approvedbyid { get; set; }
        public string createdby { get; set; }
        public string approvedby { get; set; }
        public bool FMSFLREQ_ApprovedFlg { get; set; }
        public bool approvedflag { get; set; }
        
       
        public grid_arraydata[] grid_arraydatas { get; set; }
        public class grid_arraydata
        {
            public long fnid { get; set; }

        }


        public grid_mids[] grid_mid { get; set; }
        public class grid_mids
        {
            public long Id { get; set; }

        }
    }
}
