using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Correspondence_Files_SoftCopy")]
    public class FMS_Correspondence_Files_SoftCopyDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSCORFLSC_Id { get; set; }
        public long FMSCOR_Id { get; set; }
        public string FMSCORFL_SoftCopyFileName { get; set; }
        public string FMSCORFL_SoftCopyFilePath { get; set; }
        public bool FMSCORFLSC_ActiveFlg { get; set; }
        public DateTime? FMSCORFLSC_CreatedDate { get; set; }
        public DateTime? FMSCORFLSC_UpdatedDate { get; set; }
        public long? FMSCORFLSC_CreatedBy { get; set; }
        public long? FMSCORFLSC_UpdatedBy { get; set; }
    }
}
