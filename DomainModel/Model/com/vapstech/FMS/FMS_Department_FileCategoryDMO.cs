using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Department_FileCategory")]
    public class FMS_Department_FileCategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FMSDFC_Id { get; set; }
        public long FMSMFC_Id { get; set; }
        public long HRMD_Id { get; set; }
        public bool FMSDFC_ActiveFlg { get; set; }
        public DateTime FMSDFC_CreatedDate { get; set; }
        public DateTime FMSDFC_UpdatedDate { get; set; }
        public long FMSDFC_CreatedBy { get; set; }
        public long FMSDFC_UpdatedBy { get; set; }
    }
}
