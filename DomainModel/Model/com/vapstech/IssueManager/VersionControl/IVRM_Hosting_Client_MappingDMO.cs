using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.VersionControl
{
    [Table("IVRM_Hosting_Client_Mapping")]
    public class IVRM_Hosting_Client_MappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IVRMHCM_Id { get; set; }
        public long IVRMCH_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public bool IVRMHCM_ActiveFlag { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }
        public long Createdby { get; set; }
        public long Updatedby { get; set; }

    }
}
