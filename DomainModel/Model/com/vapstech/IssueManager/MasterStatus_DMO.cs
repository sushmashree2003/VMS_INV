using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Master_IssueStatus")]
    public class MasterStatus_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMISTS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string ISMMISTS_StatusName { get; set; }
        public string ISMMISTS_StatusFlag { get; set; }
        public int ISMMISTS_Order { get; set; }
        public string ISMMISTS_Color { get; set; }
        public bool ISMMISTS_ActiveFlag { get; set; }
        public long ISMMISTS_CreatedBy { get; set; }
        public long ISMMISTS_UpdatedBy { get; set; }

    }
}
