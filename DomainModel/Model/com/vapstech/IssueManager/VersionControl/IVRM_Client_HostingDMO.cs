using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.VersionControl
{
    [Table("IVRM_Client_Hosting")]
    public class IVRM_Client_HostingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMCH_Id { get; set; }
        public string IVRMCH_HostName { get; set; }
        public string IVRMCH_HostUrl { get; set; }
        public bool IVRMCH_ActiveFlag { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }
        public long Createdby { get; set; }
        public long Updatedby { get; set; }
    }
}
