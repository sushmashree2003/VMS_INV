using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class MasterStatus_DTO:CommonParamDTO
    {
        public long ISMMISTS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string ISMMISTS_StatusName { get; set; }
        public string ISMMISTS_StatusFlag { get; set; }
        public int ISMMISTS_Order { get; set; }
        public string ISMMISTS_Color { get; set; }
        public bool ISMMISTS_ActiveFlag { get; set; }
        public long ISMMISTS_CreatedBy { get; set; }
        public long ISMMISTS_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public Array deptlist { get; set; }
        public Array getStatusdetails { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public string ISM_AS_Remarks { get; set; }

    }
}
