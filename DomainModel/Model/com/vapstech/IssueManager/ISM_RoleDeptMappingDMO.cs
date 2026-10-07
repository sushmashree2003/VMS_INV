using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_RoleType_Department_Mapping")]
    public class ISM_RoleDeptMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMRTDEM_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public long HRMD_Id { get; set; }
        public bool ISMRTDEM_ActiveFlag { get; set; }
        public long ISMRTDEM_CreatedBy { get; set; }
        public long ISMRTDEM_UpdatedBy { get; set; }

    }
}
