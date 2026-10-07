using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.VersionControl
{
    [Table("IVRM_Version_Transaction")]
    public class IVRM_Version_TransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMVT_Id { get; set; }
        public long IVRMCH_Id { get; set; }
        public string IVRMVT_VersionName { get; set; }
        public DateTime ? IVRMVT_DeployementDate { get; set; }
        public bool IVRMVT_ActiveFlag { get; set; }
        public DateTime? Createddate { get; set; }
        public DateTime? Updateddate { get; set; }
        public long Createdby { get; set; }
        public long? Updatedby { get; set; }
    }
}
