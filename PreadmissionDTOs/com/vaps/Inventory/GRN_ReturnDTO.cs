using System;
using System.Collections.Generic;
using System.Text;


namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class GRN_ReturnDTO
    {
        public long INVMGRNRET_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public long INVMGRN_Id { get; set; }
        public long INVMS_Id { get; set; }
       
        public string INVMUOM_UOMName { get; set; }
        public string INVMUOM_UOMAliasName { get; set; }
        public decimal INVTGRN_PurchaseRate { get; set; }
        public string INVMS_SupplierName { get; set; }
        public DateTime? INVMGRN_PurchaseDate { get; set; }
        public string INVMI_ItemName { get; set; }
        public string INVMGRNRET_GRNRETNo { get; set; }
        public string INVMGRNRET_DebitNoteNo { get; set; }
        public string INVMGRNRET_EWayBillNo { get; set; }
        public string INVMGRNRET_DCNo { get; set; }
        public DateTime INVMGRNRET_ReturnDate { get; set; }
        public DateTime? INVMGRNRET_DebitNoteDate { get; set; }
        public DateTime? INVMGRNRET_DCDate { get; set; }
        public decimal INVMGRNRET_TotalAmount { get; set; }
        public decimal INVMGRN_PurchaseValue { get; set; }
        public string INVMGRNRET_Remarks { get; set; }
              public string trans_id { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool INVMGRNRET_ActiveFlg { get; set; }
        public long INVMGRNRET_CreatedBy { get; set; }
        public long INVMGRNRET_UpdatedBy { get; set; }
        public DateTime INVMGRNRET_CreatedDate { get; set; }
        public DateTime INVMGRNRET_UpdatedDate { get; set; }
        public bool returnval { get; set; }
        public Array get_GRN { get; set; }
        public Array get_item { get; set; }
        public Array getsupplier { get; set; }
        public Array get_itemDetail { get; set; }
        public Array get_itemTax { get; set; }
        public Array get_GRNReturnList { get; set; }
        public Array get_GRNitemdetails { get; set; }
        public GRNItemDTO[] GRNItem { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

        public class GRNItemDTO
        {
            public long INVMGRN_Id { get; set; }
            public long INVMI_Id { get; set; }
            public long INVMUOM_Id { get; set; }
            public string INVTGRN_BatchNo { get; set; }
            public decimal INVTGRN_PurchaseRate { get; set; }
            public decimal INVTGRN_MRP { get; set; }
            public decimal INVTGRN_SalesPrice { get; set; }
            public decimal INVTGRN_DiscountAmt { get; set; }
            public decimal INVTGRN_TaxAmt { get; set; }
            public decimal INVTGRN_Amount { get; set; }
            public decimal INVTGRN_Qty { get; set; }
            public decimal INVTGRNRET_ReturnQty { get; set; }
            public decimal INVTGRNRET_ReturnAmount { get; set; }
            public string INVTGRN_Naration { get; set; }
            public DateTime? INVTGRN_MfgDate { get; set; }
            public DateTime? INVTGRN_ExpDate { get; set; }
            public bool INVTGRN_ReturnFlg { get; set; }
            public decimal INVTGRN_ReturnQty { get; set; }
            public DateTime? INVTGRN_ReturnDate { get; set; }
            public string INVTGRN_ReturnNo { get; set; }
            public string INVTGRN_ReturnNaration { get; set; }
            public bool INVTGRN_ActiveFlg { get; set; }
            public Array get_itemTax { get; set; }

            public GRNItemTaxDTO[] GRNItemTax { get; set; }
        }
        public class GRNItemTaxDTO
        {
            public long INVMT_Id { get; set; }
            public long INVMI_Id { get; set; }
            public decimal INVTGRNT_TaxPer { get; set; }
            public decimal INVTGRNT_TaxAmt { get; set; }
            public bool INVTGRNT_ActiveFlg { get; set; }

        }
    }
}
