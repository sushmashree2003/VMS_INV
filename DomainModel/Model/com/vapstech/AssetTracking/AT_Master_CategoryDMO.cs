using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.AssetTracking
{
    [Table("Master_Category")]
    public class AT_Master_CategoryDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MC_Id { get; set; }
        public long MI_Id { get; set; }
        public string Category { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public bool? ActiveFlag { get; set; }
    }

   
}
