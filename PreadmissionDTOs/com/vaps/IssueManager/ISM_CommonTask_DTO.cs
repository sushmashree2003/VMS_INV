using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_CommonTask_DTO : CommonParamDTO
    {
        public long MI_Id { set; get; }
        public long UserId { set; get; }
        public long FOMD_Id { get; set; }
        public string FOMD_DayName { get; set; }
        public long ISMMVNE_Id { get; set; }
        public string ISMMVNE_VenueName { get; set; }
        public long ISMMMER_Id { get; set; }
        public string ISMMMER_MeetingRoomName { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }


        public Array daylist { get; set; }
        public Array venuelsit { get; set; }
        public Array mettingroomlist { get; set; }
        public Array departmentlist { get; set; }
        public Array designationlist { get; set; }
        public Array employeelist { get; set; }
        public string HRME_EmployeeCode { get; set; }

        public selectedDept[] selectedDept { get; set; }
        public selectedDesg[] selectedDesg { get; set; }
    }
    public class selectedDept
    {
        public long HRMD_Id { get; set; }
    }
    public class selectedDesg
    {
        public long HRMDES_Id { get; set; }
    }
}
