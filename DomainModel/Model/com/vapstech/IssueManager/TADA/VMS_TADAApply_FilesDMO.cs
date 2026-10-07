using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Apply_Files")]
    public class VMS_TADAApply_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VTADAAF_Id { get; set; }
        public long VTADAA_Id { get; set; }
        public string VTADAAF_FileName { get; set; }
        public string VTADAAF_Filedesc { get; set; }
        public string VTADAAF_FilePath { get; set; }
         public bool? VTADAAF_ApprovedFlg { get; set; }
        public string VTADAAF_Remarks { get; set; }
        public string VTADAAF_StatusFlg { get; set; }
        public string VTADAAF_VerfieRemarks { get; set; }
        public bool VTADAAF_ActiveFlag { get; set; }
        public string VTADAAF_Cost { get; set; }

    }
}
