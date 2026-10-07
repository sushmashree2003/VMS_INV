using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_T_GRN_Tax_Return", Schema = "INV")]
    public class INV_T_GRN_Tax_Return_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTGRNTRET_Id { get; set; }
        public long INVTGRNRET_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal INVTGRNTRET_TaxPer { get; set; }
        public decimal INVTGRNTRET_TaxAmt { get; set; }
        public bool INVTGRNTRET_ActiveFlg { get; set; }
        public DateTime INVTGRNTRET_CreatedDate { get; set; }
        public DateTime INVTGRNTRET_UpdatedDate { get; set; }
        public long INVTGRNTRET_CreatedBy { get; set; }
        public long INVTGRNTRET_UpdatedBy { get; set; }
    }
}
