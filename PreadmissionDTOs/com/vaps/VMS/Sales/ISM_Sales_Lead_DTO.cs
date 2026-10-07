using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class ISM_Sales_Lead_DTO
    {
       
        public Array leadlists { get; set; }
        public Array Leaddetails { get; set; }
        public Array lead_list2 { get; set; }
        public channellsttwo[] Channellsttwo { get; set; }
        public leadlsttwo[] Leadlsttwo { get; set; }
        public Array finalleadlist { get; set; }
        public bool? ISMSLERaw_flag  { get; set; }
       public leadids[] Leadids { get; set; }
        public Array leaddropdown { get; set; }
        public long?[] channelprtnrselected { get; set; }
        public long?[] leadselected { get; set; }
        public long HRME_Id { get; set; }
        public long? ISMSMCP_Id { get; set; }
        public bool? ISMSMCP_ChannelPartnerFlg { get; set; }
        public bool returnVal { get; set; }
        public string msg { get; set; }
        public string ISMSMCP_ChannelPartnerName { get; set; }
        public string ISMSMCP_CPAddress { get; set; }
        public long ISMSMCP_CPContactNo { get; set; }

        public  ISMSLEDM_Ids[]  ISMSLEDM_Id { get; set; }
        public string ISMSLERaw_Id { get; set; }


        public string ISMSMCP_CPEmailId { get; set; }
        public Array channel_patner_edit { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Array Telecaller { get; set; }
        public Array ChannalPatner_List { get; set; }
        public Array ChannalPatnerList { get; set; }
        public Array attachementlist { get; set; }
        public long ISMSLE_Id { get; set; }
        public long ISMSLEPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMSMCA_Id { get; set; }
        public long ISMSMSO_Id { get; set; }
        public string ISMSLE_LeadName { get; set; }
        public string ISMSLE_LeadCode { get; set; }
        public string ISMSLE_ContactPerson { get; set; }
        public long ISMSLE_ContactNo { get; set; }
        public string ISMSLE_LeadAddress1 { get; set; }
        public string ISMSLE_LeadAddress2 { get; set; }
        public string ISMSLE_LeadAddress3 { get; set; }
        public DateTime? ISMSLE_VisitedDate  { get; set; }
        public string ISMSLE_EmailId { get; set; }
        public string ISMSLE_TypeFlg { get; set; }
        public long ISMSMST_Id { get; set; }
        public string ISMSLE_ContactDesignation { get; set; }
        public string ISMSLE_Reference { get; set; }
        public bool sendemail { get; set; }
        public long? IVRMMC_Id { get; set; }
        public long IVRMMS_Id { get; set; }

        public long? IVRMMCT_Id { get; set; }
        public long ISMSLE_StudentStrength { get; set; }
        public long ISMSLE_StaffStrength { get; set; }
        public long ISMSLE_NoOfInstitutions { get; set; }
        public string ISMSLE_Remarks { get; set; }
        public bool ISMSLE_OrderConfirmedFlg { get; set; }
        public string ISMSLE_ClosureRemarks { get; set; }
        public bool ISMSLE_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? FROMDATE { get; set; }
        public DateTime? TODATE { get; set; }
        public long ISMSLE_CreatedBy { get; set; }
        public long ISMSLE_UpdatedBy { get; set; }
        public string return_value { get; set; }
        public bool returnvalue { get; set; }
        public long user_id { get; set; }
        public string ISMSMCA_CategoryName { get; set; }
        public string ISMSMSO_SourceName { get; set; }
        public string ISMSMST_StatusName { get; set; }
        public long ISMSMPR_Id { get; set; }
        public bool ISMSLEPR_ActiveFlag { get; set; }
        public string IVRMMS_Name { get; set; }
        public string ISMSMPR_ProductName { get; set; }
        public string ISMSMPR_Remarks { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public string IVRMMCT_Name { get; set; }
        public bool retbool { get; set; }
        public string employeename { get; set; }
        public string R_flag { get; set; }
        public long ISMSLE_Pincode { get; set; }
        public long ISMSMCP_AgreementDuration { get; set; }
        public DateTime? ISMSMCP_AgreementStartDate { get; set; }
        public DateTime? ISMSMCP_ActiveFromDate { get; set; }
        public DateTime? ISMSMCP_ActiveToDate { get; set; }
        public bool ISMSMCP_RenewedFlg { get; set; }
        public Array district_dd { get; set; }
        public Array lead_list1 { get; set; }
        public Array lead_list { get; set; }
        public Array category_dd { get; set; }
        public Array source_dd { get; set; }
        public Array product_dd { get; set; }
        public Array companylist { get; set; }
        public Array state_dd { get; set; }
        public Array country_dd { get; set; }
        public Array status_dd { get; set; }
        public Array sales_lead_edit { get; set; }
        public Array sales_lead_edit_product_dd { get; set; }
        public Array sales_lead_edit_product_dd_product { get; set; }
        public Array lead_create_report_list { get; set; }
        public product_list_save1[] product_list_save { get; set; }
        public ISMSMCPD_DocumentPath_Array1[] ISMSMCPD_DocumentPath_Array { get; set; }
        public long ISMSMCPD_Id { get; set; }
        public string ISMSMCPD_DocumentName { get; set; }
        public string ISMSMCPD_DocumentPath { get; set; }
        public bool ISMSMCPD_ActiveFlag { get; set; }
        public DateTime ISMSMCPD_CreatedDate { get; set; }
        public DateTime ISMSMCPD_UpdatedDate { get; set; }
        public long? ISMSLE_QuotationValue { get; set; }
        public string ISMSLE_HostingServer { get; set; }
        public bool? ISMSLE_DedicatedHostingFlg { get; set; }
        public bool? ISMSLE_DedicatedMobileAppFlg { get; set; }
        public string ISMSLE_Board { get; set; }
        public string Template { get; set; }
        public string returnval { get; set; }
        public string retrunMsg { get; set; }
        public string ManagerMailid { get; set; }
        public string TeamLeadMailid { get; set; }
        public Array yearlist { get; set; }
        public Array viewdetails { get; set; }
        public long? IMFY_Id { get; set; }
        public string TemplateName { get; set; }
        public Array leadlist { get; set; }
        public Array fillyear { get; set; }
        public long? ISMSLE_ConfirmedBy { get; set; }
        public Array personal_list_dd { get; set; }
        public Array get_lead_details { get; set; }
        public Array LeadVerfiey { get; set; }
        public Array get_lead_detailsfinal { get; set; }
        public   ISM_DemoRatings[] ISM_DemoRatingsNew { get; set; }
        public ISM_CA_StudentStrength_Mapping[] StudentStrength_Mapping { get; set; }
        
        public long?  IVRM_Month_Id { get; set; }
        public string IMFY_FinancialYear { get; set; }


        public string flag1 { get; set; }

        public string flag2 { get; set; }

        public Array Sales_Raw_Report_OnclickGridData { get; set; }

        public long ISMSLEDM_Id_Cancel { get; set; }
        public string CancelRemarks { get; set; }
        public long ISMSLEDM_IdNew { get; set; }
        public Array leadTempList { get; set; }

        public SaveEmailMob[] SaveEmailMobNum { get; set; }

        public Array SwappingEmpList { get; set; }
        public decimal? Rating { get; set; }
        public string EnteredData { get; set; }

        public string SearchColumnFlag { get; set; }

        public string Role_flag { get; set; }
        public long? IVRMRT_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public string sound { get; set; }

        public string roletype { get; set; }
        public class channellsttwo
        {
            public long ISMSMCP_Id { get; set; }
        }
        public class leadlsttwo
        {
            public long ISMSLE_Id { get; set; }
            public string ISMSLE_VerifedRemark { get; set; }
            public string flag { get; set; }
            public long? ISMSLE_ConfirmedBy { get; set; }
            public string ISMSLEEM_EmailId { get; set; }
            public string ISMSLEEM_MobileNo { get; set; }
            public string ISMSLEEM_DefaultFlag { get; set; }
        }

        public class leadids
        {
            public long ismsmcP_Id { get; set; }
        }
        public class ISMSMCPD_DocumentPath_Array1
        {
            public string ISMSMCPD_DocumentPath { get; set; }
            public string FileName { get; set; }
        }
        public class product_list_save1
        {
            public long ISMSMPR_Id { get; set; }
            public long MI_Id { get; set; }
        }

        public class ISMSLEDM_Ids
        {
            public long ISMSLEDM_Id { get; set; }
        }

        public class ISM_DemoRatings
        {
            public long ICDR_ID { get; set; }
            public long MI_id { get; set; }
            public long ICDR_GivenBy_EmpID { get; set; }
            public long ICDR_ProductID { get; set; }
            public long ICDR_GivenTo_EmpId { get; set; }
            public DateTime? ICDR_DemoDate { get; set; }
            public string ICDR_Remarks { get; set; }
            public decimal? ICDR_Ratings { get; set; }
            public bool ICDR_Active { get; set; }
            public long ICDR_CreatedBy { get; set; }
            public DateTime? ICDR_CreatedDate { get; set; }
            public long ICDR_Updateby { get; set; }
            public DateTime? ICDR_UpdateDate { get; set; }
            public long? IVRM_Month_Id { get; set; }
            public string IMFY_FinancialYear { get; set; }

        }

        public class ISM_CA_StudentStrength_Mapping
        {
            public long ICSSM_ID { get; set; }
            public int ICSSM_Strenth { get; set; }
            public decimal ICSSM_PrecentageFrom { get; set; }
            public decimal ICSSM_PrecentageTo { get; set; }
            public bool ICSSM_Active { get; set; }
            public long MI_ID { get; set; }
            public long ICSSM_Createdby { get; set; }
            public long ICSSM_Updatedby { get; set; }
            public DateTime? ICSSM_CreatedOn { get; set; }
            public DateTime? ICSSM_UpdateOn { get; set; }
        }

        public class SaveEmailMob
        {
            public string email { get; set; }
            public string mobnum { get; set; }
        }
    }
}