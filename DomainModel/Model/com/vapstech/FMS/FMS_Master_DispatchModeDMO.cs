using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Master_DispatchMode")]
    public class FMS_Master_DispatchModeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSMDMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMSMTMD_DispatchMode { get; set; }
        public string FMSMTMD_DispatchModeDetails { get; set; }
        public bool FMSMDMD_ActiveFlg { get; set; }
        public DateTime? FMSMDMD_CreatedDate { get; set; }
        public DateTime? FMSMDMD_UpdatedDate { get; set; }
        public long? FMSMDMD_CreatedBy { get; set; }
        public long? FMSMDMD_UpdatedBy { get; set; }
    }
}
