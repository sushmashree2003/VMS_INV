using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Master_Project")]
    public class MastersProject_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMPR_Id { get; set; }
        public long MI_Id { get; set; }        
        public string ISMMPR_ProjectName { get; set; }
        public string ISMMPR_Desc { get; set; }
        public bool ISMMPR_InternalProjectFlg { get; set; }
        public bool ISMMPR_ActiveFlg { get; set; }
        public long? ISMMPR_CreatedBy { get; set; }
        public long? ISMMPR_UpdatedBy { get; set; }

    }
}
