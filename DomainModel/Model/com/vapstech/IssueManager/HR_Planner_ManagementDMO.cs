using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("HR_Planner_Management")]
    public class HR_Planner_ManagementDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRPLMG_Id { get; set; }
        public long MI_Id { get; set; }
        public long? HRME_Id { get; set; }
        public string HRPLMG_PlannerName { get; set; }
        public string HRPLMG_PlannerDetails { get; set; }
        public string HRPLMG_MeetingPersonName { get; set; }
        public string HRPLMG_ContactNo { get; set; }
        public string HRPLMG_MeetingAddress { get; set; }
        public DateTime? HRPLMG_MeetingDateTime { get; set; }
        public string HRPLMG_MeetingPurpose { get; set; }
        public string HRPLMG_PersonsAccompanying { get; set; }
        public DateTime? HRPLMG_EntryDateTime { get; set; }
        public string HRPLMG_Remarks { get; set; }
        public string HRPLMG_ReminderSchedule { get; set; }
        public string HRPLMG_ReminderFlag { get; set; }
        public string HRPLMG_RepeatFlag { get; set; }
        public bool HRPLMG_ActiveFlag { get; set; }
        public long HRPLMG_CreatedBy { get; set; }
        public DateTime? HRPLMG_CreatedDate { get; set; }
        public long HRPLMG_UpdatedBy { get; set; }
        public bool HRPLMG_RemainderNotificationFlag { get; set; }
        public DateTime? HRPLMG_Updateddate { get; set; }
        public string HRPLMG_MeetingStartTime { get; set; }
        public string HRPLMG_MeetingEndTime { get; set; }
        public string HRPLMG_ReminderContent { get; set; }
        public string HRPLMG_ReminderToMobileNo { get; set; }
        public string HRPLMG_ReminderToUserId { get; set; }
        public string HRPLMG_ReminderToEmailid { get; set; }
        public DateTime? HRPLMG_PaidDate { get; set; }
        public long? ISMMCLT_Id { get; set; }
        public long? ISMPRINC_Id { get; set; }
        
        public List<HR_Planner_Management_FileDMO> HR_Planner_Management_FileDMO { get; set; }
    }
}
