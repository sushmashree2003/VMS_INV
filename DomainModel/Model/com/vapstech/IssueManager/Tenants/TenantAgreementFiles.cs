using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{
    [Table("Tenant_Agreement_Files")]
    public class TenantAgreementFiles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENANTAGRFL_Id { get; set; }

        public long TENANTAGR_Id { get; set; }
        public string TENANTAGRFL_FileName { get; set; }
        public string TENANTAGRFL_FileLocation { get; set; }
        public string TENANTAGRFL_Remarks { get; set; }
        public DateTime? TENANTAGRFL_CreatedDate { get; set; }
        public DateTime? TENANTAGRFL_UpdatedDate { get; set; }
        public long TENANTAGRFL_CreatedBy { get; set; }
        public long TENANTAGRFL_UpdatedBy { get; set; }
        public bool? TENANTAGRFL_PhotoFlg { get; set; }
    }
}
