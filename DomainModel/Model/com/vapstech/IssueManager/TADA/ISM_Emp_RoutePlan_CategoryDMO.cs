using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("ISM_Emp_RoutePlan_Category")]
    public class ISM_Emp_RoutePlan_CategoryDMO
    {
        //ISM_RoutePlan_DepartureDetails
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IERPC_id { get; set; }
        public long IER_ID { get; set; }
        public long IMRC_CategoryId { get; set; }
        public int IERPC_Percentage { get; set; }
        public long IERPC_Createdby { get; set; }
        public DateTime? IERPC_CreatedDate { get; set; }
        public long IERPC_Updateby { get; set; }
        public DateTime? IERPC_UpdateDate { get; set; }

    }
}
