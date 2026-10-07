using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_Advance_Planner_Dates")]
    public class ISM_Task_Advance_Planner_DatesDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTAPLD_Id { get; set; }
        public long ISMTAPL_Id { get; set; }
        public DateTime ISMTAPLD_StartDate { get; set; }
        public DateTime ISMTAPLD_EndDate { get; set; }
        public bool ISMTAPLD_ActiveFlg { get; set; }
        public DateTime ISMTAPLD_CreatedDate { get; set; }
        public DateTime ISMTAPLD_UpdatedDate { get; set; }
        public long ISMTAPLD_CreatedBy { get; set; }
        public long ISMTAPLD_UpdatedBy { get; set; }

    
        

    }
}
