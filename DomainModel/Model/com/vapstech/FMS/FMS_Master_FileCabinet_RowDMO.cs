using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Master_FileCabinet_Row")]
    public class FMS_Master_FileCabinet_RowDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSMFCR_Id { get; set; }
        public long FMSMFCBT_Id { get; set; }
        public string FMSMFCR_CabinetRowName { get; set; }
        public bool FMSMFCR_ActiveFlg { get; set; }
        public DateTime? FMSMFCR_CreatedDate { get; set; }
        public DateTime? FMSMFCR_UpdatedDate { get; set; }
        public long? FMSMFCR_CreatedBy { get; set; }
        public long? FMSMFCR_UpdatedBy { get; set; }
    }
}
