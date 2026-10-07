using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Apply_Authorisation_Heads")]
    public class VMS_TADA_Apply_Authorisation_HeadsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long VTADAAAH_Id { get; set; }
        public long VTADAAA_Id { get; set; }
        public long VTADAAD_Id { get; set; }
        public decimal  VTADAAAH_SactionedAmount { get; set; }
        public string  VTADAAAH_Remarks { get; set; }
        public string VTADAAAH_StatusFlg { get; set; }
        public DateTime  VTADAAAH_Date { get; set; }
        public bool  VTADAAAH_ActiveFlg { get; set; }
        public long  VTADAAAH_CreatedBy { get; set; }
        public DateTime VTADAAAH_CreatedDate { get; set; }
        public long  VTADAAAH_UpdatedBy { get; set; }
        public DateTime VTADAAAH_UpdatedDate { get; set; }
        public string VTADAAAH_FileName { get; set; }
        public string VTADAAAH_FilePath { get; set; }
       


    }
}
