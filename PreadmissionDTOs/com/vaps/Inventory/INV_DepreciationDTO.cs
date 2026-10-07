using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_DepreciationDTO
    {
        public long MI_Id { get; set; }
        public Array get_Store { get; set; }
        public Array get_item { get; set; }
        public Array IMFY_YEAR { get; set; }
        public Array Institution { get; set; }
        public compnylist[] companyNames { get; set; }
        public string MI_Name { get; set; }
        public long INVMST_Id { get; set; }
        public string INVMS_StoreName { get; set; }
        public long IMFY_Id { get; set; }
        public Array getdata { get; set; }
        public long INVMSTSS_Id { get; set; }
        public long User_Id { get; set; }
        public INV_Depreciation[] INV_Depreciation { get; set; }
        public long INVDEP_Id { get; set; }
        public string Returnstatus { get; set; }
        public Array GetReport { get; set; }
        public string INVMI_ItemName { get; set; }
        public decimal INVOB_PurchaseRate { get; set; }
        public decimal? INVDEP_DepreciationPer { get; set; }
        public decimal? INVDEP_OpeningValue { get; set; }
        public DateTime? INVDEP_OpeningValueAsOnDate { get; set; }
        public decimal? INVDEP_ClosingValue { get; set; }
        public DateTime? INVDEP_ClosingValueAsOnDate { get; set; }
        public decimal? INVDEP_ReValuationValue { get; set; }
        public bool INVDEP_ActiveFlg { get; set; }
        public long INVMI_Id { get; set; }
        public string message { get; set; }
        public decimal? INVMI_ItemMoreThan180 { get; set; }
        public decimal? INVMI_ItemLessThan180 { get; set; }
        public decimal? INVMI_DeletedItemAmount { get; set; }

    }
    public class compnylist
    {
        public long MI_Id { get; set; }
    }
    public class INV_Depreciation
    {
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long? IMFY_Id { get; set; }
        public decimal? INVDEP_OpeningValue { get; set; }
        public DateTime? INVDEP_OpeningValueAsOnDate { get; set; }
        public decimal? INVDEP_DepreciationPer { get; set; }
        public decimal? INVDEP_ClosingValue { get; set; }
        public DateTime? INVDEP_ClosingValueAsOnDate { get; set; }
        public decimal? INVDEP_ReValuationValue { get; set; }
         public string INVMI_ItemName { get; set; }
        public decimal? INVMI_ItemMoreThan180 { get; set; }
        public decimal? INVMI_ItemLessThan180 { get; set; }
        public decimal? INVMI_DeletedItemAmount { get; set; }
    }
}