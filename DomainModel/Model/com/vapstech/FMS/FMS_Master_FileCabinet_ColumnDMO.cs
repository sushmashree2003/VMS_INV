using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Master_FileCabinet_Column")]
    public class FMS_Master_FileCabinet_ColumnDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSMFCC_Id { get; set; }
        public long FMSMFCBT_Id { get; set; }
        public string FMSMFCC_CabinetColumnName { get; set; }
        public bool FMSMFCC_ActiveFlg { get; set; }
        public DateTime? FMSMFCC_CreatedDate { get; set; }
        public DateTime? FMSMFCC_UpdatedDate { get; set; }
        public long? FMSMFCC_CreatedBy { get; set; }
        public long? FMSMFCC_UpdatedBy { get; set; }
    }
}
