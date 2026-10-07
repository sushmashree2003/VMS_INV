using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Tenants
{
    public class TenantWaterBillReportDTO
    {
        public Array get_userEmplist;
        public long TENANTAGR_Id { get; set; }
        public long MI_Id { get; set; }
        public string TENANTAGR_TenantName { get; set; }
        public long TENANTAGR_NoOfOccupants { get; set; }
        public string TENANTAGR_Address { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMR_Id { get; set; }
        public string TENANTAGR_EmailId { get; set; }
        public string TENANTAGR_ContactNo { get; set; }
        public string tenantmwbiL_TotalAmount { get; set; }
        public decimal TENANTAGR_AgreementDuration { get; set; }
        public DateTime TENANTAGR_AgrFromDate { get; set; }
        public DateTime TENANTAGR_AgrToDate { get; set; }
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
        public long UserId { get; set; }
        public Array building { get; set; }
        public Array rooms { get; set; }
        public string HRMR_RoomName { get; set; }
        public Array tenantname { get; set; }
        public Array year { get; set; }
       
        public string yearname { get; set; }
        public string HRMB_BuildingName { get; set; }
        public string message { get; set; }
        public tempoptionsfilesot[] tempfilesDTO { get; set; }
        public long TENANTAGRFL_Id { get; set; }
        public bool returnval { get; set; }
        public Array getloaddata { get; set; }
        public Array getviewfilesdetails { get; set; }
        public Array geteditdetails { get; set; }
        public Array geteditfilesdetails { get; set; }
        public long count { get; set; }
        public string TENANTAGRFL_Remarks { get; set; }
        public string TENANTAGRFL_FileName { get; set; }
        public string TENANTAGRFL_FileLocation { get; set; }
        public Array tenants { get; set; }
        public Array tenantchange { get; set; }
        public long TENANTAGRREN_Id { get; set; }
        public DateTime TENANTAGRREN_PreAgrFromDate { get; set; }
        public DateTime TENANTAGRREN_PreAgrToDate { get; set; }
        public string TENANTAGRREN_Remarks { get; set; }
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
        public long HRMF_Id { get; set; }
        public string HRMF_FloorName { get; set; }
        public long[] buldid { get; set; }
        public long[] roomid { get; set; }
        public long[] floorid { get; set; }
        public long[] tenant { get; set; }
        public long[] monthname { get; set; }
        public long TENENTNTB_Id { get; set; }
        public long TENENTNTBFL_Id { get; set; }
        public string TENENTNTBFL_FileName { get; set; }
        public string TENENTNTBFL_FilePath { get; set; }
        public string Month_Name { get; set; }
        public string month { get; set; }
        public string years { get; set; }
        public string sucess { get; set; }
        public long HRMLY_LeaveYear { get; set; }
        public object TENANTMWBILL_Month { get; set; }
        public object TENANTMWBILL_TotalAmount { get; set; }
       public waterbill1[] TENANTEBWB_Id { get; set; }
        public elewaterbill1news[] TENANTEBWB_Idsnew { get; set; }
        public string returnmsg { get; set; }
    }

    public class waterbill1
    {
        public long TENANTAGR_Id { get; set; }
        public long TENANTMWBILLRW_Id { get; set; }
  
    }
    public class elewaterbill1news
    {
        public long TENANTAGR_Id { get; set; }
        public long TENANTMWBILLRW_Id { get; set; }
    }

}
