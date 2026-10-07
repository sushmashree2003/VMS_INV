using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_User_Employees_Mapping")]
    public class ISM_User_Employees_MappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMUSEMM_Id { get; set; }
        public long User_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool ISMUSEMM_ActiveFlag { get; set; }
        public long ISMUSEMM_CreatedBy { get; set; }
        public long ISMUSEMM_UpdatedBy { get; set; }
        public int? ISMUSEMM_Order  { get; set; }
        public bool ISMUSEMM_Proxyflag { get; set; }

    }
}
