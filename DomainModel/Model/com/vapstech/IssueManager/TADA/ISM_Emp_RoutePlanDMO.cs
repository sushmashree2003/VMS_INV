using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("ISM_Emp_RoutePlan")]
    public class ISM_Emp_RoutePlanDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IER_ID { get; set; }
        public long IER_EmpId { get; set; }
        public DateTime? IER_Plan_StartDate { get; set; }
        public DateTime? IER_Plan_EndDate { get; set; }
        public long IER_Createdby { get; set; }
        public DateTime? IER_CreatedDate { get; set; }
        public long IER_Updateby { get; set; }
        public DateTime? IER_UpdateDate { get; set; }
        public bool ISM_Active { get; set; }
        public string IER_PlanName { get; set; }
        public string IER_Remarks { get; set; }
    }
}
