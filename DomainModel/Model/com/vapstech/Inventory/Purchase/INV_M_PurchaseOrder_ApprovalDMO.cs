using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_M_PurchaseOrder_Approval", Schema = "INV")]
    public class INV_M_PurchaseOrder_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMPOAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long? INVMS_Id { get; set; }
        public long INVMPO_Id { get; set; }

        public long? INVMSQ_Id { get; set; }

        public string INVMPOAPP_PONo { get; set; }
        public DateTime INVMPOAPP_PODate { get; set; }

        public string INVMPOAPP_Remarks { get; set; }
        public string INVMPOAPP_ReferenceNo { get; set; }
        public decimal INVMPOAPP_TotRate { get; set; }
        public decimal INVMPOAPP_TotTax { get; set; }
        public decimal INVMPOAPP_TotAmount { get; set; }
        public bool INVMPOAPP_RejectFlg { get; set; }

        public long INVMPOAPP_ApprovedBy { get; set; }

        public string INVMPOAPP_POTemplate { get; set; }

        public bool INVMPOAPP_ActiveFlg { get; set; }

        public long INVMPOAPP_CreatedBy { get; set; }

        public long INVMPOAPP_UpdatedBy { get; set; }

        public DateTime INVMPOAPP_CreatedDate { get; set; }

        public DateTime INVMPOAPP_UpdatedDate { get; set; }
        public List<INV_T_PurchaseOrder_ApprovalDMO> INV_T_PurchaseOrder_ApprovalDMO { get; set; }

    }
}
