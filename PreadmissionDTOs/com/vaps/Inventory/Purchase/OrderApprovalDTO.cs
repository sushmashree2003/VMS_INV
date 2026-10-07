using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Purchase.Inventory
{
    public class OrderApprovalDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public long ISES_Id { get; set; }
        public long UserId { get; set; }
        public long approvecnt { get; set; }
        public pocolumns[] columnids { get; set; }
        public string INVMPO_POTemplate { get; set; }
        public string staffcheck { get; set; }
        public string message { get; set; }
        public string mailmsg { get; set; }
        public long MI_Id { get; set; }
        public List<OrderApprovalDTO> aaaa { get; set; }
        public long ASMAY_Id { get; set; }
        public string trans_id { get; set; }
        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public string INVMI_ItemCode { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public decimal? INVSTO_AvaiableStock { get; set; }
        public decimal INVSTO_PurchaseRate { get; set; }
        public decimal INVTPO_ApproveQty { get; set; }
        public decimal INVTPO_TaxAmount { get; set; }
        public decimal INVTPO_RatePerUnit { get; set; }
        public decimal INVTPO_Amount { get; set; }
        public long INVMPO_Id { get; set; }
        public string INVMPI_PINo { get; set; }
        public DateTime INVMPI_PIDate { get; set; }
        public string INVMPI_Remarks { get; set; }
        public string INVMPI_ReferenceNo { get; set; }
        public decimal INVMPI_ApproxTotAmount { get; set; }
        public bool INVMPI_POCreatedFlg { get; set; }
        public bool INVMPI_ActiveFlg { get; set; }
        public string INVMPI_CreatedBy { get; set; }
        public string INVMPI_UpdatedBy { get; set; }
        public long? HRME_Id { get; set; }
        public string employeename { get; set; }
        public string INVMPR_PRNo { get; set; }
        public DateTime INVMPR_PRDate { get; set; }
        public string INVMPR_Remarks { get; set; }
        public bool INVMPR_PICreatedFlg { get; set; }
        public decimal INVMPR_ApproxTotAmount { get; set; }
        public long INVTPI_Id { get; set; }
        public long? INVMPR_Id { get; set; }
        public decimal INVTPI_PRQty { get; set; }
        public decimal INVTPI_PIUnitRate { get; set; }

        public decimal INVTPI_ApproxAmount { get; set; }
        public decimal INVTPI_PIQty { get; set; }
        public decimal INVMPO_TotAmount { get; set; }
        public decimal INVMPO_TotRate { get; set; }
        public decimal INVMPO_TotTax { get; set; }
        public string INVTPO_Remarks { get; set; }
        public string INVTPI_Remarks { get; set; }
        public bool INVTPI_ActiveFlg { get; set; }
        public DateTime INVTPI_CreatedDate { get; set; }
        public long INVTPI_CreatedBy { get; set; }
        public bool prflag { get; set; }
        public Array get_item { get; set; }
        public Array get_prNo { get; set; }
        public Array get_indentDetail { get; set; }
        public Array get_PIdetails { get; set; }
        public Array get_pimodel { get; set; }
        public Array get_PIReceipt { get; set; }
        public Array get_purchaseindent { get; set; }
        public Array get_editPI { get; set; }
        public Array get_mi_list { get; set; }

        //===================== PR Report
        public Array get_PI_details { get; set; }
        public Array get_PIreport { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string bwdateflag { get; set; }

        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

        public arrayPODTO1[] arrayPI { get; set; }
        public arrayPIDTO1[] arrayPIr { get; set; }
        public piArrayDTO1[] piArray { get; set; }
        public checkArrayDTO1[] checkArray { get; set; }
        public itemsArrayDTO1[] itemArray { get; set; }
        public get_mi_arraylistDTOmi[] get_mi_arraylist { get; set; }
        public Array viewcomments { get; set; }
        public Array indiviewcomments { get; set; }
        public string UserName { get; set; }
        public string INVMPOAPP_Remarks { get; set; }
        public DateTime INVMPOAPP_PODate { get; set; }
        public long INVTPO_Id { get; set; }
    }
    public class arrayPODTO1
    {
        public long INVTPO_Id { get; set; }
       
        public long? INVMPI_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
      
        public decimal INVTPO_POQty { get; set; }
        public decimal INVTPO_RatePerUnit { get; set; }
        public decimal INVTPO_Amount { get; set; }
        public decimal INVTPO_TaxAmount { get; set; }
        public decimal INVTPO_ApproveQty { get; set; }
  
        public string INVTPO_Remarks { get; set; }
        public string INVMI_ItemName { get; set; }
        public string flag { get; set; }
        


    }

    //public class piArrayDTO1
    //{
    //    public long INVMPI_Id { get; set; }
    //}
    //public class checkArrayDTO1
    //{
    //    public long INVMPI_Id { get; set; }
    //}
    //public class itemsArrayDTO1
    //{
    //    public long INVMI_Id { get; set; }
    //}
    //public class get_mi_arraylistDTOmi
    //{
    //    public long MI_Id { get; set; }
    //}

}
