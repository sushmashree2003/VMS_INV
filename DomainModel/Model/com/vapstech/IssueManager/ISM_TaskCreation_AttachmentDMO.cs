using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_TaskCreation_Attachment")]
    public class ISM_TaskCreation_AttachmentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTCRAT_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTCRAT_Attatchment { get; set; }
        public string ISMTCRAT_File { get; set; }
        public bool ISMTCRAT_ActiveFlg { get; set; }
        public long ISMTCRAT_CreatedBy { get; set; }
        public long ISMTCRAT_UpdatedBy { get; set; }

        public ISM_TaskCreationDMO ISM_TaskCreationDMO { get; set; }
    }
}
