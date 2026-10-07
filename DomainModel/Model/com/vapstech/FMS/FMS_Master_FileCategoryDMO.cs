using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Master_FileCategory")]
    public class FMS_Master_FileCategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSMFC_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMSMFC_FileCategoryName { get; set; }
        public string FMSMFC_FileCategoryCode { get; set; }
        public bool FMSMFC_ActiveFlg { get; set; }
        public DateTime? FMSMFC_CreatedDate { get; set; }
        public DateTime? FMSMFC_UpdatedDate { get; set; }
        public long? FMSMFC_CreatedBy { get; set; }
        public long? FMSMFC_UpdatedBy { get; set; }
        public List<FMS_Department_FileCategoryDMO> FMS_Department_FileCategoryDMO { get; set; }
    }
}
