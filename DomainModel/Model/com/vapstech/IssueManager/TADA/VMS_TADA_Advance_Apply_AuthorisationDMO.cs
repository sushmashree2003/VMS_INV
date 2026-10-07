using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Advance_Apply_Authorisation")]
    public class VMS_TADA_Advance_Apply_AuthorisationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long VTADAAAA_Id { get; set; }
        public long VTADAAA_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long VTADAAAA_SanctioningLevel { get; set; }
        public decimal VTADAAAA_SactionedAmount { get; set; }
        public string VTADAAAA_Remarks { get; set; }
        public string VTADAAAA_StatusFlg { get; set; }
        public DateTime VTADAAAA_Date { get; set; }
        public bool VTADAAAA_ActiveFlg { get; set; }
        public long VTADAAAA_CreatedBy { get; set; }
        public DateTime VTADAAAA_CreatedDate { get; set; }
        public long VTADAAAA_UpdatedBy { get; set; }
        public DateTime VTADAAAA_UpdatedDate { get; set; }

        public List<VMS_TADA_Advance_Apply_Authorisation_HeadsDMO> VMS_TADA_Advance_Apply_Authorisation_HeadsDMO { get; set; }
    }
}
