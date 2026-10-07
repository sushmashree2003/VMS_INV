using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Master_Implementation_Planner")]
    public class ISM_Master_Implementation_PlannerDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long ISMMIMPPL_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMMIMPPL_PlannerName { get; set; }
        public string ISMMIMPPL_PlannerDesc { get; set; }
        public bool ISMMIMPPL_ActiveFlag { get; set; }
        public long ISMMIMPPL_CreatedBy { get; set; }
        public long ISMMIMPPL_UpdatedBy { get; set; }
        public DateTime ISMMIMPPL_CreatedDate { get; set; }
        public DateTime ISMMIMPPL_UpdatedDate { get; set; }

    }
}
