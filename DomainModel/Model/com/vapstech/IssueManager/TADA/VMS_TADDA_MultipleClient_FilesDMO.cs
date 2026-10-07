using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADDA_MultipleClient_Files")]
    public class VMS_TADDA_MultipleClient_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VTADDAMCF_Id { get; set; }
        public long VTADAAA_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public string VTADDAMCF_Remarks { get; set; }
        public string VTADDAMCF_StatusFlg { get; set; }
        public bool VTADDAMCF_ActiveFlag { get; set; }
        public DateTime VTADDAMCF_CreatedDate { get; set; }
        public DateTime VTADDAMCF_UpdatedDate { get; set; }
        public long VTADDAMCF_CreatedBy { get; set; }
        public long VTADDAMCF_UpdatedBy { get; set; }
    }
}
