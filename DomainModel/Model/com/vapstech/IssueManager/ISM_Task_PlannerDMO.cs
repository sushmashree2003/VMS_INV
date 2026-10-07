using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_Planner")]
    public class ISM_Task_PlannerDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTPL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMTPL_PlannedBy { get; set; }
        public string ISMTPL_PlannerName { get; set; }
        public string ISMTPL_Remarks { get; set; }
        public DateTime? ISMTPL_StartDate { get; set; }
        public DateTime? ISMTPL_EndDate { get; set; }
        public decimal ISMTPL_TotalHrs { get; set; }
        public bool? ISMTPL_ApprovalFlg { get; set; }
        public long ISMTPL_ApprovedBy { get; set; }
        public bool ISMTPL_ActiveFlg { get; set; }
        public long ISMTPL_CreatedBy { get; set; }
        public long ISMTPL_UpdatedBy { get; set; }
        public long ? ISMTPL_ApprovedTo { get; set; }

        public List<ISM_Task_Planner_TasksDMO> ISM_Task_Planner_TasksDMO { get; set; }
        public List<ISM_Task_Planner_CategoryDMO> ISM_Task_Planner_CategoryDMO { get; set; }


    }
}
