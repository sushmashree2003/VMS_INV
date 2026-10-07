using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class ISM_Sales_Lead_Demo_DTO
    {

        public long? ISMSLEDM_Id { get; set; }
        public long HRMR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public long HRME_Id { get; set; }
        public string ISMSLEDM_DemoType { get; set; }
        public DateTime ISMSLEDM_DemoDate { get; set; }
        public string ISMSLEDM_ContactPerson { get; set; }
        public string ISMSLE_ContactPerson { get; set; }
        public long ISMSLE_ContactNo { get; set; }
        public long? ISMSLEDM_DemoScheduledBy { get; set; }
        public string ISMSLEDM_DemoAddress { get; set; }
        public string ISMSLEDM_Remarks { get; set; }
        public string Online { get; set; }
        public string OffLine { get; set; }
        public long? ISMSBOM_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public bool ISMSLEDM_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSLEDM_CreatedBy { get; set; }
        public long ISMSLEDM_UpdatedBy { get; set; }
        public long user_id { get; set; }
        public string employeename { get; set; }
        public string ISMSLE_LeadName { get; set; }
        public string ISMSLE_LeadCode { get; set; }
        public string ISMSMST_StatusName { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public long? ISMSMST_Id { get; set; }
        public string ISMSMPR_ProductName { get; set; }
        public long ISMSLEDMPR_Id { get; set; }
        public long ISMSMPR_Id { get; set; }
        public string ISMSLEDMPR_DiscussionPoints { get; set; }
        public bool ISMSLEDMPR_ActiveFlag { get; set; }
        public string return_status { get; set; }
        public long? ISMSLEDM_Status_Flg { get; set; }
        public long viewall { get; set; }
        public string ISMSLEDMPR_Remarks { get; set; }
        public Array lead_list_dd { get; set; }
        public Array hrme_list_ddd { get; set; }
        public Array hrme_list_dd { get; set; }
        public Array product_list_dd { get; set; }
        public Array hrmr_list_dd { get; set; }
        public Array demo_list { get; set; }
        public Array Edit_details_lead_demo { get; set; }
        public Array Lead_Demo_Products_list { get; set; }
        public Array product_dd_s { get; set; }
        public Array view_lead_demo { get; set; }
        public Array demo_response_list { get; set; }
        public Array demo_response_details { get; set; }
        public Array status_demo_master { get; set; }
        public Array leadlist { get; set; }
        public Array incentiveleads { get; set; }
        public Array incentiveemployee { get; set; }
        public Array roomlist { get; set; }
        public Array demo_listnew { get; set; }
        public long ISMSLE_NOOFINSTITUTIONS { get; set; }
        public string ISMSLE_REMARKS { get; set; }
       public string ISMSLE_REFERENCE { get; set; }
        public long ISMSLE_STUDENTSTRENGTH { get; set; }
        public Array leaddemoreport { get; set; }
        public long ISMSLE_STAFFSTRENGTH { get; set; }
        public Array Mompendinglist {get;set;}
        public string HRMDES_DesignationName { get; set; }
        public Array leaddeResponseData { get; set; }
        public product_demo_master_tempA[] product_demo_master_temp1 { get; set; }
        public product_demo_master_temp[] product_demo_master_temp2 { get; set; }
        public leadlist1[] leaddatalist { get; set; }
        public roomlist1[] roomdatalist { get; set; }
        public string ISMSLE_EmailId { get; set; }
        public bool? ISMSLEDM_DemoExecuted { get; set; }
        public bool? ISMSLEDMPR_SendQuotationFlg { get; set; }
        public string ISMSLE_LEADADDRESS1 { get; set; }
        public LeadlstStatuss[] LeadlstStatus { get; set; }
        public string msg { get; set; }

        public Array Sales_swappingEmpData { get; set; }
        public Array DemoCancelReportNew { get; set; }
        public string EmpType { get; set; }
        public string Report_Type { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }

        public string ISMSBOM_BOMName { get; set; }
        public class LeadlstStatuss
        {
            public long ISMSLE_Id { get; set; }
            public long ISMSLEDM_Id { get; set; }
            public bool? ISMSLEDM_DemoExecuted { get; set; }
            public long ISMSMST_Id { get; set; }
        }

        public class leadlist1
        {
            public long ISMSLE_Id { get; set; }
        }
        public class roomlist1
        {
            public long HRMR_Id { get; set; }
        }
        public class product_demo_master_temp
        {
            public long ISMSLEDMPR_Id { get; set; }
            public long MI_Id { get; set; }
            public long ISMSLE_Id { get; set; }
            public long ISMSLEDM_Id { get; set; }
            public long ISMSMPR_Id { get; set; }
            public long ISMSLEDMPR_DiscussionPoints { get; set; }
            public long ISMSMST_Id { get; set; }
            public string ISMSLEDMPR_Remarks { get; set; }
            public bool ISMSLEDMPR_ActiveFlag { get; set; }
            public bool ISMSLECOM_SendQuotationFlg { get; set; }
            public bool ISMSLEDM_RescheduleFlg { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public long ISMSLEDMPR_CreatedBy { get; set; }
            public long ISMSLEDMPR_UpdatedBy { get; set; }
            public string ISMSMST_StatusName { get; set; }
            public long? ISMSBOM_Id { get; set; }
            public bool? ISMSLEDM_DemoCancelledFlg { get; set; }

            public string ISMSMPR_ProductName { get; set; }
        }
        public class product_demo_master_tempA
        {
            public long ISMSMPR_Id { get; set; }
            public long MI_Id { get; set; }
            public string ISMSLEDMPR_DiscussionPoints { get; set; }
            public long? ISMSBOM_Id { get; set; }
        }

        //Incentive Report
        public string reporttype { get; set; }
        public Array get_employeereport { get; set; }
        public Array get_leadreport { get; set; }
        public Array getleaddetails { get; set; }      
    }
}
