using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Master_PhysicalFileName")]
    public class FMS_Master_PhysicalFileNameDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSMPFN_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMSMPFN_PhysicalFileName { get; set; }
        public string FMSMPFN_FileContent { get; set; }
        public string FMSMPFN_CreatedYear { get; set; }
        public string FMSMPFN_FileNo { get; set; }
        public string FMSMPFN_FileType { get; set; }
        public bool FMSMPFN_ActiveFlg { get; set; }
        public DateTime? FMSMPFN_CreatedDate { get; set; }
        public DateTime? FMSMPFN_UpdatedDate { get; set; }
        public long? FMSMPFN_CreatedBy { get; set; }
        public long? FMSMPFN_UpdatedBy { get; set; }
        public List<FMS_Department_PhysicalFileNameDMO> FMS_Department_PhysicalFileNameDMO { get; set; }
    }
}
