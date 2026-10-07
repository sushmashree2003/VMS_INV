using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_MultipleClient_Files")]
    public  class VMS_TADA_MultipleClient_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VTADAMCF_Id { get; set; }
        public long VTADAA_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public string VTADAMCF_Remarks { get; set; }
        public string VTADAMCF_StatusFlg { get; set; }
        public bool VTADAMCF_ActiveFlag { get; set; }
        public DateTime VTADAMCF_CreatedDate { get; set; }
        public DateTime VTADAMCF_UpdatedDate { get; set; }
        public long VTADAMCF_CreatedBy { get; set; }
        public long VTADAMCF_UpdatedBy { get; set; }

    }
}
