using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner
{
    public class CollegeStaffPeriodMappingDTO
    {
        public long LPCSWA_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long LPMT_Id { get; set; }
        public DateTime LPCSWA_AllocatedDate { get; set; }
        public DateTime LPCSWA_TakenDate { get; set; }
        public bool LPCSWA_Flag { get; set; }
        public bool LPCSWA_ActiveFlag { get; set; }
        public long LPCSWA_CreatedBy { get; set; }
        public long LPCSWA_UpdatedBy { get; set; }
        public long Userid { get; set; }
        public Array masteryear { get; set; }
        public Array mastercourse { get; set; }
        public Array masterbranch { get; set; }
        public Array mastersemester { get; set; }
        public Array mastersection { get; set; }
        public Array mastersubjects { get; set; }
        public Array employeedetails { get; set; }
        public string employeename { get; set; }
        public Array getalldates { get; set; }
        public Array gettopicdetails { get; set; }
        public string LPMT_TopicName { get; set; }
        public int LPMT_Topicorder { get; set; }
        public int LPSMTM_Order { get; set; }
        public bool returnval { get; set; }
        public CollegeStaffPeriodMappingTempDTO[] CollegeStaffPeriodMappingTempDTO { get; set; }
        public Array getsavedetails { get; set; }
        public string rolename { get; set; }
        public string username { get; set; }
        public long roleId { get; set; }
        public long Emp_Code { get; set; }
        public CollegeStaffPeriodMappingsavingTempDTO[] CollegeStaffPeriodMappingsavingTempDTO { get; set; }

    }
    public class CollegeStaffPeriodMappingTempDTO
    {
        public string LPCSWA_AllocatedDate { get; set; }
        public string LPMT_Id { get; set; }
        public long LPCSWA_Id { get; set; }

    }
    public class CollegeStaffPeriodMappingsavingTempDTO
    {
        public string LPCSWA_AllocatedDate { get; set; }
        public string LPMT_Id { get; set; }
        public long LPCSWA_Id { get; set; }
        public string LPCSWA_TakenDate { get; set; }

    }
}
