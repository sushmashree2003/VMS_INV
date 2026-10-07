using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_GRN_Return", Schema = "INV")]
    public class INV_M_GRN_Return_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMGRNRET_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public long INVMS_Id { get; set; }
        public string INVMGRNRET_GRNRETNo { get; set; }
        public string INVMGRNRET_DebitNoteNo { get; set; }
        public string INVMGRNRET_EWayBillNo { get; set; }
        public string INVMGRNRET_DCNo { get; set; }
        public DateTime INVMGRNRET_ReturnDate { get; set; }
        public DateTime? INVMGRNRET_DebitNoteDate { get; set; }
        public DateTime? INVMGRNRET_DCDate { get; set; }
        public decimal INVMGRNRET_TotalAmount { get; set; }
        public string INVMGRNRET_Remarks { get; set; }
        public bool INVMGRNRET_ActiveFlg { get; set; }
        public long INVMGRNRET_CreatedBy { get; set; }
        public long INVMGRNRET_UpdatedBy { get; set; }
        public DateTime INVMGRNRET_CreatedDate { get; set; }
        public DateTime INVMGRNRET_UpdatedDate { get; set; }
        public long? INVMGRNRETAPP_Id { get; set; }
        public List<INV_T_GRN_Return_DMO> INV_T_GRN_Return_DMO { get; set; }
    }
}
