using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("HR_Planner_Daily_SchedulerDetails")]
    public class HR_Planner_Daily_SchedulerDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRPDSD_Id { get; set; }
        public long HRPLMG_Id { get; set; }
        public DateTime? HRPDSD_Date { get; set; }
        public string HRPDSD_PlannerName { get; set; }
        public string HRPDSD_PlannerDetails { get; set; }
        public string HRPDSD_PlannerContent { get; set; }
        public string HRPDSD_RepeatFlag { get; set; }
        public string HRPLMG_ReminderFlag { get; set; }
        public string HRPDSD_Email { get; set; }
        public DateTime? HRPDSD_DueDate { get; set; }
        public DateTime? HRPDSD_CreatedDate { get; set; }
        public DateTime? HRPDSD_UpdatedDate { get; set; }
    }
}
