using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Tenant_TaskCreation")]
    public class ISM_Tenant_TaskCreationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTTCR_ID { get; set; }
        public long ISMTCR_ID { get; set; }
        public long TENANTAGR_Id { get; set; }
        public bool ISMTTCR_ActiveFlag { get; set; }
        public long ISMTCRCL_CreatedBy { get; set; }
        public long ISMTCRCL_UpdatedBy { get; set; }

        //public ISM_TaskCreationDMO ISM_TaskCreationDMO { get; set; }
        //public ISM_TaskCreation_ResponseDMO ISM_TaskCreation_ResponseDMO { get; set; }
    }
}
