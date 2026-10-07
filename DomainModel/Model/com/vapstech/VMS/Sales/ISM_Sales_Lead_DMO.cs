using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead")]
    public class ISM_Sales_Lead_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSLE_Id { get; set; }
        public bool? ISMSLE_VerifiedFlg { get; set; } 
        public long MI_Id { get; set; }
        public long ISMSMCA_Id { get; set; }
        public long ISMSMSO_Id { get; set; }
        public long? HRME_Id { get; set; }
        public long? ISMSMCP_Id { get; set; }
        public bool? ISMSMCP_ChannelPartnerFlg { get; set; }
        public string ISMSLE_LeadName { get; set; }
        public string ISMSLE_LeadCode { get; set; }
        public string ISMSLE_ContactPerson { get; set; }
        public long ISMSLE_ContactNo { get; set; }
        public string ISMSLE_LeadAddress1 { get; set; }
        public string ISMSLE_LeadAddress2 { get; set; }
        public string ISMSLE_LeadAddress3 { get; set; }
        public DateTime? ISMSLE_VisitedDate { get; set; }
        public string ISMSLE_EmailId { get; set; }
        public string ISMSLE_TypeFlg { get; set; }
        public long ISMSMST_Id { get; set; }
        public string ISMSLE_ContactDesignation { get; set; }
        public string ISMSLE_Reference { get; set; }
        public long IVRMMC_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public long ?IVRMMCT_Id { get; set; }
        public long ISMSLE_StudentStrength { get; set; }
        public long ISMSLE_StaffStrength { get; set; }
        public long ISMSLE_NoOfInstitutions { get; set; }
        public string ISMSLE_Remarks { get; set; }
        public bool ISMSLE_OrderConfirmedFlg { get; set; }
        public string ISMSLE_ClosureRemarks { get; set; }
        public bool ISMSLE_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSLE_CreatedBy { get; set; }
        public long ISMSLE_UpdatedBy { get; set; }
        public long? ISMSLE_Pincode { get; set; }
        public long? ISMSLE_QuotationValue { get; set; }
        public string ISMSLE_HostingServer { get; set; }
        public bool? ISMSLE_DedicatedHostingFlg { get; set; }
        public bool? ISMSLE_DedicatedMobileAppFlg { get; set; }
        public string ISMSLE_Board { get; set; }
        public string ISMSLE_VerifedRemark { get; set; }
        public long? ISMSLE_ConfirmedBy { get; set; }
        public bool? ISMSLE_QuotationSent { get; set; }
        public string ISMSLERaw_Id { get; set; }
        public long? IMFY_Id { get; set; }
        public List<ISM_Sales_Lead_Products_DMO> ISM_Sales_Lead_Products_DTO { get; set; }
        public List<ISM_Sales_Lead_EmailId_mobileNoDMO> ISM_Sales_Lead_EmailId_mobileNoDMO { get; set; }

        
    }
}
