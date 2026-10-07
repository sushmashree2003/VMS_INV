using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_M_PurchaseRequisition_Client", Schema = "INV")]
    public class INV_M_PurchaseRequisition_ClientDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMPRCLT_Id { get; set; }
        public long INVMPR_Id { get; set; }     
        public long ISMMCLT_Id { get; set; }
        public string INVMPRCLT_Remarks { get; set; }
        public string INVMPRCLT_StatusFlg { get; set; }
        public bool INVMPRCLT_ActiveFlag { get; set; }       
        public DateTime INVMPRCLT_CreatedDate { get; set; }
        public DateTime INVMPRCLT_UpdatedDate { get; set; }
        public long INVMPRCLT_CreatedBy { get; set; }
        public long INVMPRCLT_UpdatedBy { get; set; }
    }
}
