using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_Planner_Approved")]
    public class ISM_Task_Planner_ApprovedDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTPLAP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public long HRME_Id { get; set; }
        public string ISMTPLAP_Remarks { get; set; }
        public bool ISMTPLAP_ActiveFlg { get; set; }      
        public long ISMTPLAP_CreatedBy { get; set; }
        public long ISMTPLAP_UpdatedBy { get; set; }

        public decimal? ISMTPLAP_TotalHrs  { get; set; }

        public List<ISM_Task_Planner_Approved_TasksDMO> ISM_Task_Planner_Approved_TasksDMO { get; set; }


    }
}
