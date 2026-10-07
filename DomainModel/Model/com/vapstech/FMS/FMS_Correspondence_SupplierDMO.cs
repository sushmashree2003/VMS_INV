using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Correspondence_Supplier")]
    public class FMS_Correspondence_SupplierDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSCORSUP_Id { get; set; }
        public long FMSCOR_Id { get; set; }
        public long INVMS_Id { get; set; }
        public long? ISMMPR_Id { get; set; }
        public bool FMSCORSUP_ActiveFlg { get; set; }
        public DateTime? FMSCORSUP_CreatedDate { get; set; }
        public DateTime? FMSCORSUP_UpdatedDate { get; set; }
        public long? FMSCORSUP_CreatedBy { get; set; }
        public long? FMSCORSUP_UpdatedBy { get; set; }
    }
}
