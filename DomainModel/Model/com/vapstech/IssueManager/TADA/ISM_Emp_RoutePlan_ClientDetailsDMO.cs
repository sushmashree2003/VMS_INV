using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("ISM_Emp_RoutePlan_ClientDetails")]
    public class ISM_Emp_RoutePlan_ClientDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IERC_ID { get; set; }
        public long IER_ID { get; set; }
        public long IERCLE_Id { get; set; }
        public long IERC_Createdby { get; set; }
        public DateTime? IERC_CreatedDate { get; set; }
        public long IERC_Updateby { get; set; }
        public DateTime? IERC_UpdateDate { get; set; }
        public long? IMRC_CategoryId { get; set; }
        public long? IMRC_Percentage { get; set; }
        public long IERPC_id { get; set; }
        public long? IVRMMCT_Id { get; set; }
        public long? IVRMMS_Id { get; set; }
        public long? IVRMMC_Id { get; set; }
    }   
}
