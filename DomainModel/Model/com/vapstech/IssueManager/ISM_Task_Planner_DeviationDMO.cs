using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_Planner_Deviation")]
    public class ISM_Task_Planner_DeviationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTPLDE_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public long ISMTPLAP_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime ISMTPLDE_StartDate { get; set; }
        public DateTime ISMTPLDE_EndDate { get; set; }
        public DateTime ISMTPLDE_FinishedDate { get; set; }
        public string ISMTPLDE_DeviationReason { get; set; }
        public bool ISMTPLDE_ActiveFlg { get; set; }
        public long ISMTPLDE_CreatedBy { get; set; }
        public long ISMTPLDE_UpdatedBy { get; set; }
        public long ISMDR_Id { get; set; }
        public string ISMTPLDE_Status { get; set; }
    }
}
