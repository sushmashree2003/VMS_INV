using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Purchase.Inventory
{
    public class INV_InwardDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }

        public string trans_id { get; set; }
        
        public string INV_SupplierINVNo { get; set; }
        public string INV_HSNNo { get; set; }
        public string INVMLO_LocationRoomName { get; set; }

        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public long INVMIT_Id { get; set; }
        public long INVMT_Id { get; set; }
        public string INVMT_TaxName { get; set; }
        public string INVMT_TaxAliasName { get; set; }
        public decimal INVMIT_TaxValue { get; set; }
        public string INVMI_ItemCode { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public string INVMUOM_UOMAliasName { get; set; }
        public long INVMPO_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public long INVMSQ_Id { get; set; }
        public long INVMPI_Id { get; set; }
        public long INVTPI_Id { get; set; }
        public long INVTSQ_Id { get; set; }
        public long? INVMS_Id { get; set; }
        public string INVMPO_PONo { get; set; }

        

              public string INVSPT_PaymentReference { get; set; }


        public DateTime INVMPO_PODate { get; set; }
        public string INVMPO_Remarks { get; set; }
        public string INVMPO_ReferenceNo { get; set; }
        public decimal INVMSQ_NegotiatedRate { get; set; }
        public decimal INVMPO_TotRate { get; set; }
        public decimal INVMPO_TotTax { get; set; }


        public string INVMGRN_GRNNo { get; set; }
        public string INVMGRN_InvoiceNo { get; set; }
        public DateTime? INVMGRN_PurchaseDate { get; set; }
        public decimal INVMGRN_PurchaseValue { get; set; }
        public decimal? INVMGRN_TotDiscount { get; set; }
        public decimal? INVMGRN_TotTaxAmt { get; set; }
        public decimal INVMGRN_TotalAmount { get; set; }
        public string INVMGRN_Remarks { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMGRN_ReturnFlg { get; set; }
        public string INVMGRN_PaidFlg { get; set; }
        public bool INVMGRN_CreditFlg { get; set; }
        public bool INVMGRN_ActiveFlg { get; set; }

        public long INVTGRN_Id { get; set; }
        public string INVTGRN_BatchNo { get; set; }
        public decimal INVTGRN_PurchaseRate { get; set; }
        public decimal INVTGRN_MRP { get; set; }
        public decimal INVTGRN_SalesPrice { get; set; }
        public decimal INVTGRN_DiscountAmt { get; set; }
        public decimal INVTGRN_TaxAmt { get; set; }
        public decimal INVTGRN_Amount { get; set; }
        public decimal INVTGRN_Qty { get; set; }
        public string INVTGRN_Naration { get; set; }
        public DateTime? INVTGRN_MfgDate { get; set; }
        public DateTime? INVTGRN_ExpDate { get; set; }
        public bool INVTGRN_ReturnFlg { get; set; }
        public decimal INVTGRN_ReturnQty { get; set; }
        public DateTime? INVTGRN_ReturnDate { get; set; }
        public string INVTGRN_ReturnNo { get; set; }
        public string INVTGRN_ReturnNaration { get; set; }
        public bool INVTGRN_ActiveFlg { get; set; }
        public string INVMS_SupplierName { get; set; }

        public long INVTGRNT_Id { get; set; }
        public decimal INVTGRNT_TaxPer { get; set; }
        public decimal INVTGRNT_TaxAmt { get; set; }
        public bool INVTGRNT_ActiveFlg { get; set; }

        public long INVMGRNPO_Id { get; set; }


        public decimal INV_ItemQty { get; set; }
     
        public string staffcheck { get; set; }
        public get_mi_arraylist1DTO[] get_mi_arraylist { get; set; }
        public class get_mi_arraylist1DTO
        {
            public long MI_Id { get; set; }
        }

        public grnoArrayDTO[] grnoArray { get; set; }
        public class grnoArrayDTO
        {
            public long INVMGRN_Id { get; set; }
            public string INVMGRN_GRNNo { get; set; }

        }
        public Array get_mi_list { get; set; }

        public Array get_Inwarddetails { get; set; }
        public Array get_Inwardreport { get; set; }


        public decimal INV_BP { get; set; }
        public decimal INV_Tax { get; set; }
        public decimal INV_otherChrgs { get; set; }
        public decimal INV_totalvalue { get; set; }
        public decimal INV_PORefere { get; set; }
        
        public decimal INV_EwayBill { get; set; }

        public decimal INVMPO_TotAmount { get; set; }
        public bool INVMPO_ActiveFlg { get; set; }
        public long INVMPO_CreatedBy { get; set; }
        public long INVMPO_UpdatedBy { get; set; }
        public decimal INVTPI_PIQty { get; set; }
        public decimal INVTPI_PIUnitRate { get; set; }
        public decimal INVTPI_ApproxAmount { get; set; }
        public long INVTPO_Id { get; set; }
        public decimal INVTPO_POQty { get; set; }
        public decimal INVTPO_RatePerUnit { get; set; }
        public decimal INVTPO_TaxAmount { get; set; }
        public decimal INVTPO_Amount { get; set; }
        public string INVTPO_Remarks { get; set; }
        public bool INVTPO_ActiveFlg { get; set; }
        public long INVTPO_CreatedBy { get; set; }
        public long INVTPO_UpdatedBy { get; set; }

        public long INVTPOT_Id { get; set; }
        public decimal INVTPOT_TaxPercent { get; set; }
        public decimal INVTPOT_TaxAmount { get; set; }
        public bool INVTPOT_ActiveFlg { get; set; }
        public long INVTPOT_CreatedBy { get; set; }
        public long INVTPOT_UpdatedBy { get; set; }


        public decimal INVTSQ_QuotedRate { get; set; }
        public decimal INVTSQ_NegotiatedRate { get; set; }

        public string quotationflag { get; set; }
        public string Selectionflag { get; set; }
        

        public Array get_quotationno { get; set; }
        public Array get_comparequotationno { get; set; }
        public Array get_purchaseorder { get; set; }
        public Array get_supplier { get; set; }
        public Array get_PI { get; set; }
        
        public Array get_qtdetails { get; set; }
        public Array pidetails { get; set; }
        
        public Array get_pisupplier { get; set; }
        public Array get_itemTax { get; set; }
   
        public Array get_supdata { get; set; }
        
        public Array get_potax { get; set; }
        public Array get_editDetail { get; set; }

        public List<INV_InwardDTO> itemtax { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public arrayPODTO[] arrayPO { get; set; }
        //===================== PO Report
      //  public Array get_Inwarddetails { get; set; }
     
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string bwdateflag { get; set; }
        //================report PO    
        //public poArrayDTO[] poArray { get; set; }
        public InwarditemArray[] InwarditemArray { get; set; }
        public InwardsuplierArray[] InwardsuplierArray { get; set; }

    }

   // =======================PO Trans
   // public class arrayPODTO
   // {
   //     public long INVTPO_Id { get; set; }
   //     public long INVMI_Id { get; set; }
   //     public long INVMUOM_Id { get; set; }
   //     public long INVMPI_Id { get; set; }
   //     public decimal INVTPO_POQty { get; set; }
   //     public decimal INVTPO_RatePerUnit { get; set; }
   //     public decimal INVTPO_TaxAmount { get; set; }
   //     public decimal INVTPO_Amount { get; set; }
   //     public string INVTPO_Remarks { get; set; }
   //     public bool INVTPO_ActiveFlg { get; set; }
   //     public long INVTPO_CreatedBy { get; set; }
   //     public long INVTPO_UpdatedBy { get; set; }


   //     public arrayPOtaxDTO[] arrayPOtax { get; set; }

   // }
   //// ======================= PO TAX
   // public class arrayPOtaxDTO
   // {
   //     public long INVTPOT_Id { get; set; }
   //     public long INVMIT_Id { get; set; }
   //     public decimal INVTPOT_TaxPercent { get; set; }
   //     public decimal INVTPOT_TaxAmount { get; set; }
   //     public bool INVTPOT_ActiveFlg { get; set; }
   //     public long INVTPOT_CreatedBy { get; set; }
   //     public long INVTPOT_UpdatedBy { get; set; }

   // }

   // public class poArrayDTO
   // {
   //     public long INVMPO_Id { get; set; }
   // }
    public class InwarditemArray
    {
        public long INVMI_Id { get; set; }
    }
    public class InwardsuplierArray
    {
        public long INVMS_Id { get; set; }
    }

}
