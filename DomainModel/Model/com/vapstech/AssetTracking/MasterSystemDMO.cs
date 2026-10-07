using DomainModel.Model.com.vapstech.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.AssetTracking

{
    [Table("Master_System")]
    public class MasterSystemDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MS_Id { get; set; }
        public long MI_Id { get; set; }
        public string System { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public bool? ActiveFlag { get; set; }
    }
}
