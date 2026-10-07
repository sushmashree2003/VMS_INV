using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class HR_Planner_ManagementDTO
    {
        public long HRPLMG_Id {get;set;}
        public long HRPLMGFL_Id { get;set;}
        public bool reminderflag { get;set;}
        public long MI_Id { get;set;}
        public long HRME_Id { get;set;}
        public long UserId { get;set;}
        public long remiderday { get;set;}
        public string HRPLMG_PlannerName { get; set; }
        public string HRPLMG_ReminderToMobileNo { get; set; }
        public string HRPLMG_ReminderToUserId { get; set; }
        public string HRPLMG_ReminderToEmailid { get; set; }
        public string HRPLMG_PlannerDetails { get; set; }
        public string HRPLMG_MeetingPersonName { get; set; }
        public string HRPLMG_ContactNo { get; set; }
        public string HRPLMG_ReminderContent { get; set; }
        public string HRPLMG_MeetingAddress { get; set; }
        public DateTime? HRPLMG_MeetingDateTime { get; set; }
        public string HRPLMG_MeetingPurpose { get; set; }
        public string HRPLMG_PersonsAccompanying { get; set; }
        public DateTime? HRPLMG_EntryDateTime { get; set; }
        public string HRPLMG_Remarks { get; set; }
        public string HRPLMG_ReminderSchedule { get; set; }
        public string HRPLMG_ReminderFlag { get; set; }
        public string HRPLMG_RepeatFlag { get; set; }
        public string HRMEM_EmailId { get; set; }
        public bool HRPLMG_ActiveFlag { get; set; }
        public bool resrnval { get; set; }
        public long HRPLMG_CreatedBy { get;set;}
        public DateTime? HRPLMG_CreatedDate { get;set;}
        public long HRPLMG_UpdatedBy { get;set;}
        public DateTime? HRPLMG_Updateddate { get; set; }
        public DateTime? HRPLMG_PaidDate { get; set; }
        public string HRPLMG_MeetingStartTime { get; set; }
        public string HRPLMG_MeetingEndTime { get; set; }
        public long roleId { get; set; }
        public long User_Id { get; set; }
        public Array employeelist { get; set; }
        public Array mobilenolist { get; set; }
        public Array griddata { get; set; }
        public Array emailist { get; set; }
        public Array editfiles { get; set; }
        public Array editedlevelcheck { get; set; }
        public Array getempmailid { get; set; }
        public Array editdata { get; set; }
        public Array employeelistdata { get; set; }
        public Array ClientNames { get; set; }
        public long? ISMMCLT_Id { get; set; }
        public long? ISMPRINC_Id { get; set; }
        public long VMSMOUT_Id { get; set; }
        public Array Performadetails { get; set; }
        public Array Templatedetails { get; set; }
        public string returnval { get; set; }
        public long[] emp_array { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Mobile_Number_DTO[] mobile_list_dto { get; set; }

        public Email_Id_DTO[] email_list_dto { get; set; }
        public MasterplannerfiledocDTO[] filelist { get; set; }
        public Multipleuserlevel[] Multipleuserlvl { get; set; }
    }
    public class MasterplannerfiledocDTO
    {
        public long gridid { get; set; }
        public long cfileid { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string name { get; set; }
    }

    public class Multipleuserlevel
    {
        public long userId { get; set; }
        public long HRPLMGAPP_MaxApprovalDays { get; set; }
        public long HRPLMGAPP_ApprovalAlevel { get; set; }
    }
}
