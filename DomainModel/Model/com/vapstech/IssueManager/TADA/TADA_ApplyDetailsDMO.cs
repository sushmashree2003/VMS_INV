using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{

    [Table("VMS_TADA_Apply_Details")]
    public class TADA_ApplyDetailsDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VTADAAD_Id { get; set; }
        public long VTADAA_Id { get; set; }
        public string VTADAAD_ExpenditureHead { get; set; }
        public decimal VTADAAD_Amount { get; set; }
        public string VTADAAD_Remarks { get; set; }
        public string VTADAAD_StatusFlg { get; set; }
        public bool VTADAAD_ActiveFlg { get; set; }
        public long VTADAAD_CreatedBy { get; set; }
        public DateTime? VTADAAD_CreatedDate { get; set; }
        public long VTADAAD_UpdatedBy { get; set; }
        public DateTime? VTADAAD_UpdatedDate { get; set; }
        public decimal? VTADAAD_Totalslots { get; set; }
        public decimal? VTADAAD_Slots { get; set; }

    }
}
