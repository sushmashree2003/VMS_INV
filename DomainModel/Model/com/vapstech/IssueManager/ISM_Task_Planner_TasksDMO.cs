using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_Planner_Tasks")]
    public class ISM_Task_Planner_TasksDMO: CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTPLTA_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTPLTA_Remarks { get; set; }
        public DateTime? ISMTPLTA_StartDate { get; set; }
        public DateTime? ISMTPLTA_EndDate { get; set; }
        public decimal ISMTPLTA_EffortInHrs { get; set; }
        public DateTime? ISMTPLTA_FinishedDate { get; set; }
        public string ISMTPLTA_Status { get; set; }
        public bool ISMTPLTA_DeviationFlg { get; set; }
        public bool? ISMTPLTA_ApprovalFlg { get; set; }
        public bool ISMTPLTA_ExtraFlg { get; set; }
        public decimal ISMTPLTA_TimeTakenInHrs { get; set; }  
        public bool ISMTPLTA_ActiveFlg { get; set; }   
        public long ISMTPLTA_CreatedBy { get; set; }
        public long ISMTPLTA_UpdatedBy { get; set; }
        public string ISMTPLTA_DailyReportRemark { get; set; }
        public bool? ISMTPLTA_PreviousTask { get; set; }
        public ISM_Task_PlannerDMO ISM_Task_PlannerDMO { get; set; }
        public List<ISM_DailyReportGenerationDMO> ISM_DailyReportGenerationDMO { get; set; }
        public List<ISM_Task_Planner_Approved_TasksDMO> ISM_Task_Planner_Approved_TasksDMO { get; set; }
    }
}