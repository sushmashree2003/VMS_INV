using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.AssetTracking
{
    [Table("Master_CheckPoint")]
    public class AT_Master_CheckPointDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MCP_Id { get; set; }
        public long MI_Id { get; set; }
        public string CheckPoint { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public bool? ActiveFlag { get; set; }
    }
}
