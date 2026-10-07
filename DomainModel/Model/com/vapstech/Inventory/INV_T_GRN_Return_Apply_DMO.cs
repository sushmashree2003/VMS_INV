using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_T_GRN_Return_Apply", Schema = "INV")]
    public class INV_T_GRN_Return_Apply_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTGRNRETAPP_Id { get; set; }
        public long INVMGRNRETAPP_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public decimal INVTGRNRETAPP_ReturnQty { get; set; }
        public decimal INVTGRNRETAPP_ReturnAmount { get; set; }
        public string INVTGRNRETAPP_ReturnNaration { get; set; }
        public bool INVTGRNRETAPP_ActiveFlg { get; set; }
        public DateTime? INVTGRNRETAPP_CreatedDate { get; set; }
        public long INVTGRNRETAPP_CreatedBy { get; set; }
        public DateTime? INVTGRNRETAPP_UpdatedDate { get; set; }
        public long INVTGRNRETAPP_UpdatedBy { get; set; }
        public string INVTGRNRETAPP_ApprovalFlg { get; set; }
    }
}
