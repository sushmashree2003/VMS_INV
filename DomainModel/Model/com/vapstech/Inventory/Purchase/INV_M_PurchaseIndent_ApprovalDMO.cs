using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_M_PurchaseIndent_Approval", Schema = "INV")]
    public class INV_M_PurchaseIndent_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMPIAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMPI_Id { get; set; }
        public string INVMPIAPP_PINo { get; set; }
        public DateTime INVMPIAPP_PIDate { get; set; }
        public string INVMPIAPP_Remarks { get; set; }
        public string INVMPIAPP_ReferenceNo { get; set; }
        public decimal INVMPIAPP_ApproxTotAmount { get; set; }
        public bool INVMPIAPP_RejectFlg { get; set; }
        public long INVMPIAPP_ApprovedBy { get; set; }
        public bool INVMPIAPP_ActiveFlg { get; set; }
        public long INVMPIAPP_CreatedBy { get; set; }
        public long INVMPIAPP_UpdatedBy { get; set; }
        public DateTime INVMPIAPP_CreatedDate { get; set; }
        public DateTime INVMPIAPP_UpdatedDate { get; set; }

        public  List<INV_T_PurchaseIndent_ApprovalDMO> INV_T_PurchaseIndent_ApprovalDMO { get; set; }
    }
}
