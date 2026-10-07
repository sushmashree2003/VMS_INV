using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_Planner_Category")]
    public class ISM_Task_Planner_CategoryDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
    
        public long ISMTPLC_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public long ISMMTCAT_Id { get; set; }
        public decimal ISMTPLC_TaskPercentage { get; set; }
        public string ISMTPLC_EachTaskMaxDuration { get; set; }
        public string ISMTPLC_DurationFlg { get; set; }
        public bool ISMTPLC_CompulsoryFlg { get; set; }

    }
}
