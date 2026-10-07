using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Department_LetterHeads")]
    public class FMS_Dept_Letter_HeadMappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSDLH_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string FMSDLH_ReceivedBy { get; set; }
        public string FMSDLH_LetterheadsFromNo { get; set; }
        public string FMSDLH_LetterheadsToNo { get; set; }
        public DateTime? FMSDLH_ReceivedDate { get; set; }
        public bool? FMSDFC_ActiveFlg { get; set; }
        public DateTime? FMSDFC_CreatedDate { get; set; }
        public DateTime? FMSDFC_UpdatedDate { get; set; }
        public long? FMSDFC_CreatedBy { get; set; }
        public long? FMSDFC_UpdatedBy { get; set; }
    }
}
