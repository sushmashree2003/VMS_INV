using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class MasterActivities_DTO:CommonParamDTO
    {
        public long ISMMAC_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMMAC_ActivityName { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string ISMMAC_Desc { get; set; }
        public bool ISMMAC_ActiveFlag { get; set; }
        public long? ISMMAC_CreatedBy { get; set; }
        public long? ISMMAC_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }

        public Array alldata { get; set; }
        public Array deptlist { get; set; }
        public Array designationlist { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }

    }
}
