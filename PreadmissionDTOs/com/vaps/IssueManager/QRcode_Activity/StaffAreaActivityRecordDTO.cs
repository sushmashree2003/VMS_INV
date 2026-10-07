using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity
{
    public class StaffAreaActivityRecordDTO
    {

        public long HRMARACT_Id { get; set; }
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public long HRMAR_Id { get; set; }

        public long HRSTARACTRCD_Id { get; set; }
      
        public DateTime? HRSTARACTRCD_Date { get; set; }
        public string HRSTARACTRCD_Time { get; set; }
        public bool HRSTARACTRCD_ActiveFlag { get; set; }
        public DateTime? HRSTARACTRCD_CreatedDate { get; set; }
        public DateTime? HRSTARACTRCD_UpdatedDate { get; set; }
        public long HRSTARACTRCD_CreatedBy { get; set; }
        public long HRSTARACTRCD_UpdatedBy { get; set; }
        public Array get_building { get; set; }

        public string returnval { get; set; }
        public string HRMA_QRCODEPath { get; set; }

        public Array StaffAreaActivityRecord { get; set; }
        public Array QRcodeActivityDetails { get; set; }

        public string HRMARACT_ActivityName { get; set; }

        

        public string UserName { get; set; }
        public TAstListArrayDTO[] tasklistarray { get; set; }

    }

    public class TAstListArrayDTO
    {
        public long HRMARACT_Id { get; set; }
        public long HRMARACTT_Id { get; set; }

        public long HRMAR_Id { get; set; }
        public string HRSTARACTRCD_Remarks { get; set; }
        public string HRSTARACTRCD_Status { get; set; }
    }
}
