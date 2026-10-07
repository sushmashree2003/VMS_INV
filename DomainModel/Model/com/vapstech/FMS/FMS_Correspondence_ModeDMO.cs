using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Correspondence_Mode")]
    public class FMS_Correspondence_ModeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSCORMD_Id { get; set; }
        public long FMSCOR_Id { get; set; }
        public long FMSMDMD_Id { get; set; }
        public DateTime? FMSCORMD_SentDate { get; set; }
        public string FMSCORMD_ReferenceNo { get; set; }
        public string FMSCORMD_FileName { get; set; }
        public string FMSCORMD_FilePath { get; set; }
        public bool FMSCORMD_ActiveFlg { get; set; }
        public DateTime? FMSCORMD_CreatedDate { get; set; }
        public DateTime? FMSCORMD_UpdatedDate { get; set; }
        public long? FMSCORMD_CreatedBy { get; set; }
        public long? FMSCORMD_UpdatedBy { get; set; }
    }
}
