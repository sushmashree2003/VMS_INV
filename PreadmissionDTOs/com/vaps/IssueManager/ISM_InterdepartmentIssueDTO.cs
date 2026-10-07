using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public  class ISM_InterdepartmentIssueDTO
    {
        public string departmentlist { get; set; }
        public long roleId { get; set; }

        public long MI_Id { get; set; }

        public long UserId { get; set; }
        public long User_Id { get; set; }
         public long HRMD_Id { get; set; }
         public long Rcount { get; set; }
         public long AssignedToEmpId { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string ISMTCR_Status { get; set; }
        public long IVRMRT_Id { get; set; }
        public long OpenCount { get; set; }
        public long Intermediate { get; set; }
        public long CloseCount { get; set; }
        public long PendingCount { get; set; }
        public string IVRMRT_Role { get; set; }
        public string ISMTCR_TaskNo { get; set; } 
        public string Task { get; set; }

        public string Description { get; set; }

        public DateTime ISMTCR_CreationDate { get; set; }

        public string Priority { get; set; }

        public string Category { get; set; }

        public string Crestedby { get; set; }

        public string AssignedBy { get; set; }

        public string AssignedTo { get; set; }

        public DateTime AssignedDate { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? Todate { get; set; }

        public string CreatedOnDepartment { get; set; }

        public string CreatedBYDepartment { get; set; }
        public Array getdetails{ get; set; }
        public Array get_empwise { get; set; }

        public Array get_report { get; set; }
        public Array get_ondept { get; set; }
                   
        public DateTime? startdate { get; set; }
        public string reporttype { get; set; }

        public DateTime? enddate { get; set; }
        public Array departmentList { get; set; }

        public Array department_list { get; set; }
        public Array get_bydeptreport { get; set; }
        // public Array selecteddepartment_list { get; set; }
        public Listdep [] selecteddepartment_list { get; set; }
        public Array get_reporttwo { get; set; }

    }

    public class Listdep
    {

        public long hrmD_Id { get; set; }
    }
}
