using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("HR_Master_DepartmentCode_Head")]
    public class HR_Master_DepartmentCode_HeadDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMDCH_ID { get; set; }
        public long HRME_ID { get; set; }
        public long HRMDC_ID { get; set; }       
        public bool? PlannerApproval { get; set; }
        public long HRMDCH_UpdatedBy { get; set; }
        public long HRMDCH_CreatedBy { get; set; }
    }
}
