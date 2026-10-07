using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Correspondence_Files")]
    public class FMS_Correspondence_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSCORFL_Id { get; set; }
        public long FMSCOR_Id { get; set; }
        public long FMSMPFN_Id { get; set; }
        public bool FMSCORFL_ActiveFlg { get; set; }
        public DateTime? FMSCORFL_CreatedDate { get; set; }
        public DateTime? FMSCORFL_UpdatedDate { get; set; }
        public long? FMSCORFL_CreatedBy { get; set; }
        public long? FMSCORFL_UpdatedBy { get; set; }
    }
}
