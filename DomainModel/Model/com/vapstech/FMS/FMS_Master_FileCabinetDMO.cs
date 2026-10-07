using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Master_FileCabinet")]
    public class FMS_Master_FileCabinetDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSMFCBT_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMSMFCBT_FileCabinetName { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }
        public long HRMR_Id { get; set; }
        public long FMSMFCBT_NoOfColumns { get; set; }
        public long FMSMFCBT_NoOfRows { get; set; }
        public bool FMSMFCBT_ActiveFlg { get; set; }
        public DateTime? FMSMFCBT_CreatedDate { get; set; }
        public DateTime? FMSMFCBT_UpdatedDate { get; set; }
        public long? FMSMFCBT_CreatedBy { get; set; }
        public long? FMSMFCBT_UpdatedBy { get; set; }
        public List<FMS_Master_FileCabinet_RowDMO> FMS_Master_FileCabinet_RowDMO { get; set; }
        public List<FMS_Master_FileCabinet_ColumnDMO> FMS_Master_FileCabinet_ColumnDMO { get; set; }
    }
}
