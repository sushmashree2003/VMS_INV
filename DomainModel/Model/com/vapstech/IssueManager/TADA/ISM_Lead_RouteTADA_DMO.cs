using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("ISM_Lead_RouteTADA")]
    public class ISM_Lead_RouteTADA_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ILRT_ID { get; set; }
        public long IER_ID { get; set; }
        public string ILRT_FileName { get; set; }
        public string ILRT_Filedesc { get; set; }
        public string ILRT_FilePath { get; set; }
        public bool? ILRT_ApprovedFlg { get; set; }
        public string ILRT_Remarks { get; set; }
        public string ILRT_StatusFlg { get; set; }
        public bool ILRT_ActiveFlag { get; set; }
        public long ILRT_Createdby { get; set; }
        public DateTime ILRT_CreatedDate { get; set; }
        public long ILRT_Updateby { get; set; }
        public DateTime ILRT_UpdateDate { get; set; }
        public string ILRT_VerifieRemarks { get; set; }
        public decimal ILRT_Cost { get; set; }
        public long IERC_ID { get; set; }
       
    }
}