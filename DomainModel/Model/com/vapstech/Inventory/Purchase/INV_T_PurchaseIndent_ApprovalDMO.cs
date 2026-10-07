using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_T_PurchaseIndent_Approval", Schema = "INV")]
    public class INV_T_PurchaseIndent_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTPIAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMPIAPP_Id { get; set; }
        public long? INVMPR_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVTPI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public decimal INVTPIAPP_PRQty { get; set; }
        public decimal INVTPIAPP_PIQty { get; set; }
        public decimal INVTPIAPP_PIUnitRate { get; set; }
        public decimal INVTPIAPP_ApproxAmount { get; set; }
        public bool INVTPIAPP_RejectFlg { get; set; }
        public decimal INVTPIAPP_ApprovedQty { get; set; }
        public string INVTPIAPP_Remarks { get; set; }
        public bool INVTPIAPP_ActiveFlg { get; set; }
        public DateTime INVTPIAPP_CreatedDate { get; set; }
        public DateTime INVTPIAPP_UpdatedDate { get; set; }
        public long INVTPIAPP_UpdatedBy { get; set; }
        public long INVTPIAPP_CreatedBy { get; set; }
     
    }
}
