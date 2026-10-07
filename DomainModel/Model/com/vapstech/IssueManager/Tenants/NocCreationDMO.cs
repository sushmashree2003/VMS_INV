using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity
{

    [Table("Tenant_Noc")]
    public class NocCreationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long TENANTNOC_Id { get; set; }
        public long TENANTAGR_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime TENANTNOC_NOCDate { get; set; }
        public string TENANTNOC_Remarks { get; set; }
        public bool TENANTNOC_ActiveFlag { get; set; }
        public DateTime TENANTNOC_CreatedDate { get; set; }
        public DateTime TENANTNOC_UpdatedDate { get; set; }
        public long TENANTNOC_CreatedBy { get; set; }
        public long TENANTNOC_UpdatedBy { get; set; }

    }

}

