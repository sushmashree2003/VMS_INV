using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals
{
    public class ClgStudentDashboardDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long? AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public int COEME_Id { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }     
        public string ASMAY_Year { get; set; }
        public int month { get; set; }
        public Array yearlist { get; set; }
        public Array currentyear { get; set; }
        public Array studentdetails { get; set; }      
        public Array attendancedetails { get; set; }
        public Array noticeboard { get; set; }
        public Array feedetails { get; set; }
        public Array coereportlist { get; set; }
        public Array calenderlist { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string amst_mobile { get; set; }
        public string amst_email_id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long user_id { get; set; }
        public Array hodlist { get; set; }
        public long IHOD_Id { get; set; }
        public bool IHOD_ActiveFlag { get; set; }
        public string IHOD_Flg { get; set; }
        public bool IHODS_ActiveFlag { get; set; }
        public Array saved_hods_stf { get; set; }
        public bool returnval { get; set; }
        public string returnsavestatus { get; set; }

        public ClgStudentDashboardDTO[] employee { get; set; }
        public Array saved_hods { get; set; }
        public Array branchlist { get; set; }
        public Array query01 { get; set; }
        public ClgStudentDashboardDTO[] class_lst { get; set; }
        public string branchname { get; set; }
        public bool IHODB_ActiveFlag { get; set; }
        public Array saved_hods_cls { get; set; }
        public Array hodbranch { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public Array Fillstudentstrenth { get; set; }
    }
}
