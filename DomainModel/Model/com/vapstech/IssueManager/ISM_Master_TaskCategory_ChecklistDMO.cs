using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Master_TaskCategory_Checklist")]
    public class ISM_Master_TaskCategory_ChecklistDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMMTCATCL_Id { get; set; }
        public long ISMMTCAT_Id { get; set; }
        public string ISMMTCATCL_CheckListName { get; set; }
        public bool ISMMTCATCL_UploadEnterFlg { get; set; }
        public bool? ISMMTCATCL_CheckListFlg { get; set; }
        public bool ISMMTCATCL_ActiveFlag { get; set; }
        public long ISMMTCATCL_CreatedBy { get; set; }
        public long ISMMTCATCL_UpdatedBy { get; set; }     
        public DateTime? ISMMTCATCL_CreatedDate { get; set; }
        public DateTime? ISMMTCATCL_UpdatedDate { get; set; }
    }
}
