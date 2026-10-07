using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("HR_Planner_Management_Approval")]
    public class HR_Planner_Management_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRPLMGAPP_Id { get; set; }
        public long HRPLMG_Id { get; set; }
        public long UserId { get; set; }
        public long HRPLMGAPP_ApprovalAlevel { get; set; }
        public long HRPLMGAPP_MaxApprovalDays { get; set; }
        public bool HRPLMGAPP_ActiveFlag { get; set; }
        public long HRPLMGAPP_CreatedBy { get; set; }
        public DateTime HRPLMGAPP_CreatedDate { get; set; }
        public long HRPLMGAPP_UpdatedBy { get; set; }
        public DateTime HRPLMGAPP_Updateddate { get; set; }
    }
}
