using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Advance_Apply_Authorisation_Heads")]
    public class VMS_TADA_Advance_Apply_Authorisation_HeadsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long  VTADAAAAH_Id { get; set; }
        public long  VTADAAAA_Id { get; set; }
        public long  VTADAAAD_Id { get; set; }
        public decimal  VTADAAAAH_SactionedAmount { get; set; }
        public string  VTADAAAAH_Remarks { get; set; }
        public string VTADAAAAH_StatusFlg { get; set; }
        public DateTime  VTADAAAAH_Date { get; set; }
        public bool  VTADAAAAH_ActiveFlg { get; set; }
        public long VTADAAAAH_CreatedBy { get; set; }
        public DateTime VTADAAAAH_CreatedDate { get; set; }
        public long  VTADAAAAH_UpdatedBy { get; set; }
        public DateTime VTADAAAAH_UpdatedDate { get; set; }
        public string VTADAAAAH_FileName { get; set; }
        public string VTADAAAAH_FilePath { get; set; }
       

    }
}
