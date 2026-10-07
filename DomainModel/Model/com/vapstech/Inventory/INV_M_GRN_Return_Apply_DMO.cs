using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_M_GRN_Return_Apply", Schema = "INV")]
    public class INV_M_GRN_Return_Apply_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVMGRNRETAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public long INVMS_Id { get; set; }
        public string INVMGRNRETAPP_GRNRETNo { get; set; }
        public DateTime INVMGRNRETAPP_ReturnDate { get; set; }
        public decimal INVMGRNRETAPP_TotalAmount { get; set; }
        public string INVMGRNRETAPP_Remarks { get; set; }
        public string INVMGRNRETAPP_StatusFlg { get; set; }
        public bool INVMGRNRETAPP_ActiveFlg { get; set; }
        public long INVMGRNRETAPP_CreatedBy { get; set; }
        public long INVMGRNRETAPP_UpdatedBy { get; set; }
        public DateTime? INVMGRNRETAPP_CreatedDate { get; set; }
        public DateTime? INVMGRNRETAPP_UpdatedDate { get; set; }
        public List<INV_T_GRN_Return_Apply_DMO> INV_T_GRN_Return_Apply_DMO { get; set; }
        public List<INV_M_GRN_Return_DMO> INV_M_GRN_Return_DMO { get; set; }
    }
}
