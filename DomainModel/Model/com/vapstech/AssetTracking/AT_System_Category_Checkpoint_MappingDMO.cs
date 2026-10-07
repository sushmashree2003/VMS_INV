using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.AssetTracking
{
    [Table("System_Category_Checkpoint_Mapping")]
    public class AT_System_Category_Checkpoint_MappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SCCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public long MS_Id { get; set; }
        public long MC_Id { get; set; }
        public string MCP_Id { get; set; }
        public int Total_Item { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public bool? ActiveFlag { get; set; }
    }
}
