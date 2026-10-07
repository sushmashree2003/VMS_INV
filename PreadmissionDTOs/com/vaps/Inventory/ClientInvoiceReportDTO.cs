using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class ClientInvoiceReportDTO
    {
  
        public string trans_id { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool? returnval { get; set; }
        public bool? already_cnt { get; set; }
        public string message { get; set; }
        public long MI_Id { get; set; }
        public long MI_Id_new { get; set; }
        
        public long ASMAY_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long ISMINC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long UserId { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_Order { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public int ASMC_Order { get; set; }
        public long AMST_Id { get; set; }
        public string membername { get; set; }
        public string AMST_AdmNo { get; set; }
        public string type { get; set; }
        public string saletype { get; set; }
        public string employeename { get; set; }
        public long HRME_Id { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public string INVMUOM_UOMAliasName { get; set; }
        public long INVSTO_Id { get; set; }
        public DateTime? INVSTO_PurchaseDate { get; set; }
        public string INVSTO_BatchNo { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public string INVMI_ItemName { get; set; }
        public string INVMI_ItemCode { get; set; }
        public long INVMT_Id { get; set; }
        public decimal? INVMIT_TaxValue { get; set; }
        public string INVMT_TaxName { get; set; }
        public string INVMT_TaxAliasName { get; set; }
        public string INVTSL_Naration { get; set; }
        public decimal? INVSTO_AvaiableStock { get; set; }

        //=======================INV_M_Sales
        public long INVMSL_Id { get; set; }
        public long INVMST_Id { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMSL_StuOtherFlg { get; set; }
        public string INVMSL_SalesNo { get; set; }
        public DateTime? INVMSL_SalesDate { get; set; }
        public decimal? INVMSL_SalesValue { get; set; }
        public decimal? INVMSL_TotDiscount { get; set; }
        public decimal? INVMSL_TotTaxAmt { get; set; }
        public decimal? INVMSL_TotalAmount { get; set; }
        public string INVMSL_Remarks { get; set; }
        public string INVMSL_ReturnFlg { get; set; }
        public string INVMSL_PaidFlg { get; set; }
        public bool? INVMSL_CreditFlg { get; set; }
        public bool? INVMSL_ActiveFlg { get; set; }
        public long INVMP_Id { get; set; }
        public string Student_flag { get; set; }
        public string INVC_LIFOFIFOFlg { get; set; }
        public string staffcheck { get; set; }


        //=======================INV_Sales
        public long INVMSLS_Id { get; set; }
        public long INVMSLST_Id { get; set; }
        public long INVMSLC_Id { get; set; }
        public long INVMC_Id { get; set; }
        public string INVMC_CustomerName { get; set; }
        public long INVTSL_Id { get; set; }
        public long INVTSLT_Id { get; set; }

        //=======================================
        //=======================================REPORT
        public Array editlistdetails { get; set; }
        public Array editlisttax { get; set; }
        public Array clientlist { get; set; }
        public Array get_salesdetails { get; set; }
        public Array get_class { get; set; }
        public Array get_Section { get; set; }
        public Array get_staff_customer { get; set; }
        public Array get_SalesReport { get; set; }
        public Array get_editdata { get; set; }
        public Array get_editmember { get; set; }
        public Array get_mi_list { get; set; }
        public Array instlist { get; set; }
        public Array editlist { get; set; }
    
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string typeflag { get; set; }
        public string individualflag { get; set; }

        public long? invmsl_id { get; set; }
        public long? invmi_id { get; set; }
        public long? amst_id { get; set; }
        public string studentid { get; set; }
        public long? hrme_id { get; set; }
        public long? invmc_id { get; set; }
        public string selectionflag { get; set; }

       
        public get_mi_arraylistDTO[] get_mi_arraylist { get; set; }

        //=======================================
        public string INVTSL_BatchNo { get; set; }
        public decimal? INVTSL_SalesQty { get; set; }
        public decimal? INVTSL_SalesPrice { get; set; }
        public decimal? INVTSL_DiscountAmt { get; set; }
        public decimal? INVTSL_TaxAmt { get; set; }
        public decimal? INVTSL_Amount { get; set; }
        public bool? INVTSL_ActiveFlg { get; set; }
        public decimal? INVTSLT_TaxPer { get; set; }
        public decimal INVTSLT_TaxAmt { get; set; }
        public bool? INVTSLT_ActiveFlg { get; set; }
        public Array get_Student_Cls_Sec { get; set; }
        public Array get_Student_Cls_Sec_Item_SN { get; set; }
        public Array get_sectionlist { get; set; }
        public Array get_Studentlist { get; set; }
        public Array get_employee { get; set; }
        public Array get_customer { get; set; }
        public Array get_Product { get; set; }
        public Array get_itemTax { get; set; }
        public Array get_item { get; set; }
        public Array get_Store { get; set; }
        public Array get_itemDetail { get; set; }
        public Array availablestock { get; set; }
        public Array get_Sale { get; set; }
        public Array get_Saletypes { get; set; }
        public Array get_SaleItemDetails { get; set; }
        public Array get_SaleItemTax { get; set; }

      public Array banklist { get; set; }



    }

  
}
