using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Depreciation", Schema = "INV")]
    public class INV_DepreciationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVDEP_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long IMFY_Id { get; set; }
        public decimal? INVDEP_OpeningValue { get; set; }
        public DateTime? INVDEP_OpeningValueAsOnDate { get; set; }
        public decimal? INVDEP_DepreciationPer { get; set; }
        public decimal? INVDEP_ClosingValue { get; set; }
        public DateTime? INVDEP_ClosingValueAsOnDate { get; set; }
        public decimal? INVDEP_ReValuationValue { get; set; }
        public bool INVDEP_ActiveFlg { get; set; }
        public DateTime? INVDEP_CreatedDate { get; set; }
        public DateTime? INVDEP_UpdatedDate { get; set; }
        public long INVDEP_CreatedBy { get; set; }
        public long INVDEP_UpdatedBy { get; set; }
        public decimal? INVMI_ItemMoreThan180 { get; set; }
        public decimal? INVMI_ItemLessThan180 { get; set; }
        public decimal? INVMI_DeletedItemAmount { get; set; }
    }
}
