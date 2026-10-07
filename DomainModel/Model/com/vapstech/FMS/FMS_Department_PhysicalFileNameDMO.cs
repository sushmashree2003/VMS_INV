using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Department_PhysicalFileName")]
    public class FMS_Department_PhysicalFileNameDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSDPFN_Id { get; set; }
        public long FMSMPFN_Id { get; set; }
        public long HRMD_Id { get; set; }
        public bool FMSDPFN_ActiveFlg { get; set; }
        public DateTime? FMSDPFN_CreatedDate { get; set; }
        public DateTime? FMSDPFN_UpdatedDate { get; set; }
        public long? FMSDPFN_CreatedBy { get; set; }
        public long? FMSDPFN_UpdatedBy { get; set; }
    }
}
