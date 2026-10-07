using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Implementation_Planner")]
    public class ISM_Implementation_PlannerDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long  ISMIMPPL_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMIMPPL_ActivityName { get; set; }
        public long? IVRMM_Id { get; set; }
        public string ISMIMPPL_Periodicity { get; set; }
        public string ISMIMPPL_Remarks { get; set; }
        public long ISMIMPPL_TimeRequired { get; set; }
        public bool ISMIMPPL_ActiveFlg { get; set; }
        public DateTime ISMIMPPL_CreatedDate { get; set; }
        public DateTime ISMIMPPL_UpdatedDate { get; set; }
        public long ISMIMPPL_CreatedBy { get; set; }
        public long ISMIMPPL_UpdatedBy { get; set; }
        public string ISMIMPPL_TimeRequiredFlg { get; set; }
        public long ISMMIMPPL_Id { get; set; }


    }
}
