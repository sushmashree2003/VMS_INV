using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_TaskCreation_Client")]
    public class ISM_TaskCreation_ClientDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTCRCL_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public bool ISMTCRCL_ActiveFlg { get; set; }
        public long ISMTCRCL_CreatedBy { get; set; }
        public long ISMTCRCL_UpdatedBy { get; set; }

        public ISM_TaskCreationDMO ISM_TaskCreationDMO { get; set; }
    }
}
