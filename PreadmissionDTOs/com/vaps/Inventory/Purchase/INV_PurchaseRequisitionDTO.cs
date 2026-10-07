using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Purchase.Inventory
{
    public class INV_PurchaseRequisitionDTO
    {
        public Array transnumconfig { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public string accept { get; set; }
        public string reject { get; set; }
        public string staffcheck { get; set; }
        public long MI_Id { get; set; }
        public long MI_IdNew { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
       
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }

        public string trans_id { get; set; }
        public long INVMI_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public string INVMI_ItemCode { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public decimal? INVSTO_AvaiableStock { get; set; }

        public long INVMPR_Id { get; set; }
        public long? HRME_Id { get; set; }
        public string employeename { get; set; }
        public string departmentname { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public string INVMPR_PRNo { get; set; }
        public DateTime INVMPR_PRDate { get; set; }
        public string INVMPR_Remarks { get; set; }
        public decimal INVMPR_ApproxTotAmount { get; set; }
        public bool INVMPR_PICreatedFlg { get; set; }
        public bool INVMPR_ActiveFlg { get; set; }
        public bool INVMPR_RejectFlg { get; set; }
        public bool INVTPR_RejectFlg { get; set; }
        public string INVMPR_CreatedBy { get; set; }
        public string INVMPR_UpdatedBy { get; set; }
        public long INVTPR_Id { get; set; }
        public decimal INVTPR_PRQty { get; set; }
        public decimal INVTPR_ApproxAmount { get; set; }
        public decimal INVTPR_ApprovedQty { get; set; }
        public decimal INVTPR_PRUnitRate { get; set; }
        public string INVTPR_Remarks { get; set; }
        public bool INVTPR_ActiveFlg { get; set; }
    

        public Array get_employee { get; set; }
        public Array get_item { get; set; }
        public Array get_purchaserequisition { get; set; }
        public Array get_prDetail { get; set; }
        public Array editPR { get; set; }
        public Array get_pidata { get; set; }
        public Array get_itemDetail { get; set; }
        public Array get_mi_list { get; set; }
     
        //===================== PR Report
        public Array get_PRdetails { get; set; }
        public Array get_PRreport { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string bwdateflag { get; set; }




        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

        public arrayPRDTO[] arrayPR { get; set; }
        public prArrayDTO[] prArray { get; set; }
        public itemArrayDTO[] itemArray { get; set; }
        public get_mi_arraylistDTO1[] get_mi_arraylist { get; set; }
        public Array Client_Master { get; set; }
        public arrayClientDTO[] clientnames { get; set; }
    }
    public class arrayPRDTO
    {
        public long INVTPR_Id { get; set; }
        public long INVMPR_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public decimal INVTPR_PRQty { get; set; }
        public decimal INVTPR_ApproxAmount { get; set; }
        public decimal INVTPR_ApprovedQty { get; set; }
        public decimal INVTPR_PRUnitRate { get; set; }

        public string INVTPR_Remarks { get; set; }
        public bool INVTPR_ActiveFlg { get; set; }
        public string INVTPR_CreatedBy { get; set; }
        public string INVTPR_UpdatedBy { get; set; }

    }

    public class prArrayDTO
    {
        public long INVMPR_Id { get; set; }
        public long? HRME_Id { get; set; }
        public string employeename { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public string INVMPR_PRNo { get; set; }
        public DateTime INVMPR_PRDate { get; set; }
        public string INVMPR_Remarks { get; set; }
        public decimal INVMPR_ApproxTotAmount { get; set; }
        public long INVTPR_Id { get; set; }
        public decimal INVTPR_PRQty { get; set; }
        public decimal INVTPR_ApproxAmount { get; set; }
        public decimal INVTPR_ApprovedQty { get; set; }
        public decimal INVTPR_PRUnitRate { get; set; }
        public string INVTPR_Remarks { get; set; }

    }

    public class itemArrayDTO
    {
        public long INVMI_Id { get; set; }
    }

    public class get_mi_arraylistDTO1
    {
        public long MI_Id { get; set; }
    }

    public class arrayClientDTO
    {
        public long INVMPRCLT_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public string INVMPRCLT_Remarks { get; set; }
        public string INVMPRCLT_StatusFlg { get; set; }
        public long INVMPRCLT_CreatedBy { get; set; }
        public long INVMPRCLT_UpdatedBy { get; set; }
    }
}
