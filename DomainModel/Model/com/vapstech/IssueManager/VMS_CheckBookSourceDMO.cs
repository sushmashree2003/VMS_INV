using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_CheckBookSource")]
    public class VMS_CheckBookSourceDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VCBSRC_Id { get; set; }
        public string VCBSRC_SourceName { get; set; }
        public string VCBSRC_SourceMobileNo { get; set; }
        public string VCBSRC_EmailId { get; set; }
        public string VCBSRC_Address { get; set; }
        public bool VCBSRC_ActiveFlg { get; set; }
        public long VCBSRC_CreatedBy { get; set; }
        public DateTime? VCBSRC_CreatedDate { get; set; }
        public long VCBSRC_UpdatedBy { get; set; }
        public DateTime? VCBSRC_UpdatedDate { get; set; }


    }
}
