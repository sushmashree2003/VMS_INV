using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Purchase.Inventory
{
    [Table("INV_M_PurchaseOrder_ToSupplier", Schema = "INV")]
    public class INV_M_PurchaseOrder_ToSupplierDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTPOSUP_Id { get; set; }
        public DateTime INVTPOSUP_DateTime { get; set; }
        public long MI_Id { get; set; }
        public long INVMPO_Id { get; set; }
        public string INVTPOSUP_SupplierName { get; set; }
        public string INVTPOSUP_MailAttatchment { get; set; }
        public long? INVTPOSUP_ContactNo { get; set; }
        public string INVTPOSUP_EmailId { get; set; }
        public DateTime? INVTPOSUP_SMSSentDate { get; set; }
        public DateTime? INVTPOSUP_MailSentDate { get; set; }
        public bool INVTPOSUP_ActiveFlg { get; set; }
        public long INVTPOSUP_CreatedBy { get; set; }
        public long INVTPOSUP_UpdatedBy { get; set; }      

    }
}
