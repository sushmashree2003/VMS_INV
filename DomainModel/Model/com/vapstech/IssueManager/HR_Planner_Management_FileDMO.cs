using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("HR_Planner_Management_File")]
    public class HR_Planner_Management_FileDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     
        public long HRPLMGFL_Id { get; set; }
        public long HRPLMG_Id { get; set; }
        public string HRPLMGFL_FileName { get; set; }
        public string HRPLMGFL_FileLocation { get; set; }
        public bool HRPLMGFL_ActiveFlag { get; set; }
        public long HRPLMGFL_CreatedBy { get; set; }
        public DateTime HRPLMGFL_CreatedDate { get; set; }
        public long HRPLMGFL_UpdatedBy { get; set; }
        public DateTime HRPLMGFL_Updateddate { get; set; }
        
    }
}
