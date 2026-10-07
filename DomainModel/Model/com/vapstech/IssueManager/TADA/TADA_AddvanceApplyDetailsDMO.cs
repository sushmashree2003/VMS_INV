using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Advance_Apply_Details")]
    public class TADA_AddvanceApplyDetailsDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VTADAAAD_Id { get; set; }
        public long VTADAAA_Id { get; set; }
        public string VTADAAAD_ExpenditureHead { get; set; }
        public decimal VTADAAAD_Amount { get; set; }
        public string VTADAAAD_Remarks { get; set; }
        public string VTADAAAD_StatusFlg { get; set; }
        public bool VTADAAAD_ActiveFlg {get; set;}

       // public string VTADAAAD_StatusFlg { get; set; }

        public long VTADAAAD_CreatedBy { get; set; }
        public DateTime? VTADAAAD_CreatedDate { get; set; }
        public long VTADAAAD_UpdatedBy { get; set; }
        public DateTime? VTADAAAD_UpdatedDate { get; set; }
        public decimal? VTADAAAD_Slots { get; set; }

        public decimal? VTADAAAD_TotalSlots { get; set; }



    }
}
