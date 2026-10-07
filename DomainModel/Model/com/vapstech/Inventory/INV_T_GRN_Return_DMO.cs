using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_T_GRN_Return", Schema = "INV")]
    public class INV_T_GRN_Return_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVTGRNRET_Id { get; set; }
        public long INVMGRNRET_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public decimal INVTGRNRET_ReturnQty { get; set; }
        public decimal INVTGRNRET_ReturnAmount { get; set; }
        public string INVTGRNRET_ReturnNaration { get; set; }
        public bool INVTGRNRET_ActiveFlg { get; set; }
        public DateTime INVTGRNRET_CreatedDate { get; set; }
        public long INVTGRNRET_CreatedBy { get; set; }
        public DateTime INVTGRNRET_UpdatedDate { get; set; }
        public long INVTGRNRET_UpdatedBy { get; set; }
        public List<INV_T_GRN_Tax_Return_DMO> INV_T_GRN_Tax_Return_DMO { get; set; }
    }
}
