using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class MastersPriorities_DTO:CommonParamDTO
    {
       
        public long HRMPR_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMP_Name { get; set; }
        public int HRMP_Order { get; set; }
        public bool HRMP_ActiveFlag { get; set; }
        public long? HRMP_CreatedBy { get; set; }
        public long? HRMP_UpdatedBy { get; set; }
        public long UserId { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array alldata { get; set; }
    }
}
