using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_Advance_Planner")]
    public class ISM_Task_Advance_PlannerDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTAPL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string ISMTAPL_Periodicity { get; set; }
        public string ISMTAPL_Remarks { get; set; }
        public bool ISMTAPL_ActiveFlg { get; set; }
        public long ISMTAPL_CreatedBy { get; set; }
        public long ISMTAPL_UpdatedBy { get; set; }
        public long ISMTCR_Id { get; set; }
        public long ISMTAPL_Year { get; set; }
        public DateTime ISMTAPL_FromDate { get; set; }
        public DateTime ISMTAPL_ToDate { get; set; }
        public decimal ISMTAPL_TimeRequired { get; set; }
        public DateTime? ISMTAPL_OFFDate { get; set; }
        public long ISMTAPL_Day { get; set; }
        public string ISMTAPL_TimeRequiredFlg { get; set; }
        public long? ISMTAPL_ApprovalFlag { get; set; }
        public List<ISM_Task_Advance_Planner_DatesDMO> ISM_Task_Advance_Planner_DatesDMO { get; set; }
    }
}
