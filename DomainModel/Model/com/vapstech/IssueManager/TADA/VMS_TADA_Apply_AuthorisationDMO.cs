using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Apply_Authorisation")]
    public class VMS_TADA_Apply_AuthorisationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
     
        public long VTADAAA_Id { get; set; }
        public long VTADAA_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long VTADAAA_SanctioningLevel { get; set; }
        public decimal VTADAAA_SactionedAmount { get; set; }
        public string VTADAAA_Remarks { get; set; }
        public string VTADAAA_StatusFlg { get; set; }
        public DateTime VTADAAA_Date { get; set; }
        public bool VTADAAA_ActiveFlg { get; set; }
        public long VTADAAA_CreatedBy { get; set; }
        public DateTime VTADAAA_CreatedDate { get; set; }
        public long VTADAAA_UpdatedBy { get; set; }
        public DateTime VTADAAA_UpdatedDate { get; set; }

        public List<VMS_TADA_Apply_Authorisation_HeadsDMO> VMS_TADA_Apply_Authorisation_HeadsDMO { get; set; }
    }
}
