using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Master_PhysicalFileName_FileCabinet")]
    public class FMS_Master_PhysicalFileName_FileCabinetDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSMPFNFC_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMSMPFN_Id { get; set; }
        public long FMSMFCBT_Id { get; set; }
        public long FMSMFCC_Id { get; set; }
        public long FMSMFCR_Id { get; set; }
        public bool FMSMPFN_ActiveFlg { get; set; }
        public DateTime? FMSMPFN_CreatedDate { get; set; }
        public DateTime? FMSMPFN_UpdatedDate { get; set; }
        public long FMSMPFN_CreatedBy { get; set; }
        public long FMSMPFN_UpdatedBy { get; set; }
    }
}