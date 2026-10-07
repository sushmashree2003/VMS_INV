using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_T_PurchaseOrder_Approval", Schema = "INV")]
    public class INV_T_PurchaseOrder_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTPOAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMPOAPP_Id { get; set; }
        public long? INVMPI_Id { get; set; }
        public long INVTPO_Id { get; set; }

        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }

      
        public decimal INVTPOAPP_POQty { get; set; }
        public decimal INVTPOAPP_ApprovedQty { get; set; }
        public decimal INVTPOAPP_RatePerUnit { get; set; }
        public decimal INVTPOAPP_TaxAmount { get; set; }
        public decimal INVTPOAPP_Amount { get; set; }
        public bool INVTPOAPP_RejectFlg { get; set; }
        public string INVTPOAPP_Remarks { get; set; }
        public bool INVTPOAPP_ActiveFlg { get; set; }
        public long INVTPOAPP_CreatedBy { get; set; }
        public long INVTPOAPP_UpdatedBy { get; set; }
        public DateTime INVTPOAPP_CreatedDate { get; set; }

        public DateTime INVTPOAPP_UpdatedDate { get; set; }

    }
}
