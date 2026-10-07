using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class ISM_EMPLOYEE_MEMONOTICEDTO : CommonParamDTO
    {
        public long ISMEMN_ID { get; set; }
        public long MI_ID { get; set; }
        public long HRME_ID { get; set; }
        public long ASMAY_ID { get; set; }
        public string ISMEMN_No { get; set; }
        public DateTime ISMEMN_Date { get; set; }
        public string ISMEMN_Type { get; set; }
        public string ISMEMN_Description { get; set; }
        public bool ISMEMN_EmailSentFlag { get; set; }
        public long ISMEMN_CreatedBy { get; set; }
        public long ISMEMN_UpdatedBy { get; set; }
        public DateTime? ISMEMN_CompleByDate { get; set; }
        public DateTime? ISMEMN_Startdate { get; set; }
        public DateTime? ISMEMN_Enddate { get; set; }
        public long ISMTPLTA_Id { get; set; }
        public long UserId { get; set; }
        public long IVRMRT_Id { get; set; }
        public string Role_flag { get; set; }
        public bool roleflag { get; set; }
        public string empname { get; set; }
        public string empdepartment { get; set; }
        public Array empdRawReports { get; set; }
        public Array memonoticereport { get; set; }
        public Array viewdetails { get; set; }
        public Array get_userEmplist { get; set; }
        public Array DepartmentList { get; set; }
        public Array Designation { get; set; }
        public Array arrayuserEmp { get; set; }

        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }

        public long[] employeelist { get; set; }
        public int?[] departmentlist { get; set; }
        public long?[] departmentlistNew { get; set; }
        public int?[] designationlist { get; set; }

        public long ISMTCR_Id { get; set; }
        public string ISMTCR_Title { get; set; }
        public string ISMTCR_Desc { get; set; }
        public string ISMTCR_Status { get; set; }
        public string ISMTCR_TaskNo { get; set; }
        public string Type { get; set; }
        public class dept
        {
            public int? HRMDC_ID { get; set; }
        }
    }
}
