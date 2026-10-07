using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_Planner_Approved_Tasks")]
    public class ISM_Task_Planner_Approved_TasksDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTPLAPTA_Id { get; set; }
        public long ISMTPLAP_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTPLAPTA_Remarks { get; set; }
        public DateTime? ISMTPLAPTA_StartDate { get; set; }
        public DateTime? ISMTPLAPTA_EndDate { get; set; }

        public decimal? ISMTPLAPTA_EffortInHrs { get; set; }

        public DateTime? ISMTPLAPTA_FinishedDate { get; set; }
        public string ISMTPLAPTA_Status { get; set; }
        public bool ISMTPLAPTA_DeviationFlg { get; set; }
        public bool ISMTPLAPTA_ExtraTaskFlg { get; set; }

        public decimal? ISMTPLAPTA_TimeTakenInHrs { get; set; }

        public bool? ISMTPLAPTA_RejectedFlg { get; set; }
        public bool? ISMTPLAPTA_ApprovalFlg { get; set; }
        //public bool ISMTPLAPTA_UnPlannedFlg { get; set; }
        public bool ISMTPLAPTA_ActiveFlg { get; set; }       
        public long ISMTPLAPTA_CreatedBy { get; set; }
        public long ISMTPLAPTA_UpdatedBy { get; set; }
        public long ISMTPLTA_Id { get; set; }
        public ISM_Task_Planner_ApprovedDMO ISM_Task_Planner_ApprovedDMO { get; set; }
    }
}
