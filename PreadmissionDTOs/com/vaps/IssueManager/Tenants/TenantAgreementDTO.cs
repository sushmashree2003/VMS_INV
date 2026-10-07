using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Tenants
{
    public class TenantAgreementDTO
    {
        public long TENANTAGR_Id { get; set; }
        public long MI_Id { get; set; }
        public string TENANTAGR_TenantName { get; set; }
        public long TENANTAGR_NoOfOccupants { get; set; }
        public string TENANTAGR_Address { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMR_Id { get; set; }
        public string PAMS_Id { get; set; }
        public Decimal? TENANTAGR_RenewalPercentage { get; set; }
        public string TENANTAGR_PANCardNo { get; set; }
        public string TENANTAGR_EmailId { get; set; }
        public string TENANTAGR_ContactNo { get; set; }
        public decimal TENANTAGR_AgreementDuration { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public DateTime TENANTAGR_AgrFromDate { get; set; }
        public DateTime TENANTAGR_AgrToDate { get; set; }
        public string type { get; set; }
        public string maintype { get; set; }
        
        public Decimal TENANTAGR_AdvanceAmount { get; set; }
        public Decimal TENANTAGR_MonthlyRent { get; set; }
        public string TENANTAGR_RentDueDay { get; set; }
        public string TENANTAGR_RentDueMonth { get; set; }
        public bool TENANTAGR_RenewedFlag { get; set; }
        public string TENANTAGR_Remarks { get; set; }
        public long TENANTAGR_UserId { get; set; }
        public DateTime? TENANTAGR_CreatedDate { get; set; }
        public DateTime? TENANTAGR_UpdatedDate { get; set; }
        public long? TENANTAGR_CreatedBy { get; set; }
        public long? TENANTAGR_UpdatedBy { get; set; }
        public bool? TENANTAGR_ApprovalFlg { get; set; }
        public long UserId { get; set; }
        public decimal? TENANTAGR_RefundedAmount { get; set; }
        public string TENANTAGR_RefundModeOfPayment { get; set; }

        public Array building { get; set; }
        public Array rooms { get; set; }
        public string HRMR_RoomName { get; set; }

        public string HRMB_BuildingName { get; set; }

        public string message { get; set; }
        public Array getyear { get; set; }
        public Array TenantDetails { get; set; }
        public tempoptionsfilesot[] tempfilesDTO { get; set; }

        public long TENANTAGRFL_Id { get; set; }
        public bool returnval { get; set; }
        public Array getloaddata { get; set; }
        public Array getviewfilesdetails { get; set; }
        public Array geteditdetails { get; set; }
        public Array geteditfilesdetails { get; set; }
        public Array tenant_array { get; set; }
        public Array monthnames { get; set; }
        public Array years { get; set; }
        public Array reference { get; set; }

        public long count { get; set; }
       public string TENANTAGRFL_Remarks { get; set; }
        public string TENANTAGRFL_FileName { get; set; }
        public string TENANTAGRFL_FileLocation { get; set; }

        public Array tenants { get; set; }
        public Array tenantchange { get; set; }
        public Array getinstitution { get; set; }
        public long TENANTAGRREN_Id { get; set; }

        public DateTime TENANTAGRREN_PreAgrFromDate { get; set; }
        public DateTime TENANTAGRREN_PreAgrToDate { get; set; }
        public string TENANTAGRREN_Remarks { get; set; }

        public int? TENANTAGRREN_AggreedEnhancement { get; set; }
        public string TENANTAGRREN_AmtOrPercentFlg { get; set; }
        public long TENANTAGRREN_Rent { get; set; }
        public Array tenantrenewal { get; set; }


        public Array edittenantrenewal { get; set; }
        public bool TENANTAGR_ActiveFlag { get; set; }
        public bool TENANTAGRREN_ActiveFlg { get; set; }


        public bool TENANTAGR_LeftFlg { get; set; }

        public string TENANTAGR_LeftRemarks { get; set; }



        public long HRMAM_Id { get; set; }

        public string HRMAM_AmenitiesName { get; set; }
        public decimal HRMRAM_RentPerDay { get; set; }
        public long HRMRAM_NoOfHrs { get; set; }
        public decimal HRMRAM_RentPerHour { get; set; }
        public Array getamenitiesdetails { get; set; }
        public Array getfloordetails { get; set; }
        public Array getownerdetails { get; set; }
        public long HRMF_Id { get; set; }
        public string HRMF_FloorName { get; set; }

        public long[] buldid { get; set; }
        public long[] roomid { get; set; }

        public long[] floorid { get; set; }
        public elewaterbill[] TENANTEBWB_Ids { get; set; }

        public long TENENTNTB_Id { get; set; }

        public long TENENTNTBFL_Id { get; set; }
        public string TENENTNTBFL_FileName { get; set; }
        public string TENENTNTBFL_FilePath { get; set; }

        public long TENANTEBWB_Id { get; set; }
    

        public string TENANTEBWB_Flg { get; set; }

        public long TENANTEBWB_Year { get; set; }

        public long? TENANTEBWB_Month { get; set; }

        public decimal TENANTEBWB_Amount { get; set; }
        public string TENANTEBWB_Remarks { get; set; }
        public bool TENANTEBWB_ActiveFlag { get; set; }
        public decimal TENANTNOCCL_DeductingAmount { get; set; }
        public string returnmsg { get; set; }
        public Array get_printdetails { get; set; }
        public Array getmonth { get; set; }
        public Array amenitiescharges { get; set; }
        public decimal deductingamount { get; set; }
        public decimal advanceamount { get; set; }
        public long[] company_array { get; set; }
        public long[] monthid { get; set; }

        public long HRMRAM_Id { get; set; }
        public long? TENANTAGR_Aadhar { get; set; }
        public string TENANTAGR_Imagepath { get; set; }
        public bool TENANTAGRFL_PhotoFlg { get; set; }
        public string IVRM_Month_Name { get; set; }

        public string flag { get; set; }

        public tenantallbilltypearr[] tenantbilltypearr { get; set; }
        public tenantfineentryarr[] tenantfineentryarr { get; set; }

    }
    
    public class tenantallbilltypearr
    {
        public long TENANTAGR_Id { get; set; }
        public long TENANTELECTRICAL_Amount { get; set; }
        public long TENANTWATER_Amount { get; set; }
        public long TENANTOTHER_Amount { get; set; }
        public string TENANTEBWB_Remarks { get; set; }
    }

    public  class tenantfineentryarr
    {
        public long TENANTAGRPAY_Id { get; set; }
        public long TENANTAGR_Id { get; set; }
        public long TENANTAGRPAY_PaymentMonth { get; set; }
        public long TENANTAGRPAY_PaymentYear { get; set; }
        public decimal TENANTAGRPAY_ActualFineAmount { get; set; }
    }

    public class tempoptionsfilesot
    {

        public long TENANTAGRFL_Id { get; set; }
        public string TENANTAGRFL_FileName { get; set; }
        public string TENANTAGRFL_FileLocation { get; set; }
        public string TENANTAGRFL_Remarks { get; set; }
        public bool TENANTAGRFL_PhotoFlg { get; set; }
    }

    public class elewaterbill
    {
        public long TENANTEBWB_Id { get; set; }

    }
}
