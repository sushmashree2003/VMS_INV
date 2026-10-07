using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class AddVisitorsDTO : CommonParamDTO
    {



        public string HRMDES_DesignationName { get; set; }
        public long? HRMDES_Id { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public string HRME_LocCity { get; set; }
        public bool? VMMV_VIPFlg { get; set; }
        public long VMMV_Id { get; set; }
        public long VMAP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TENANTAGR_Id { get; set; }
        public long VMVTMT_ToMeet_HRME_Id { get; set; }
        public string VMMV_VisitorName { get; set; }
        public string VMAP_VisitorName { get; set; }
        public string VMAP_VisitorEmailid { get; set; }
        public string VMAP_MeetingPurpose { get; set; }
        public string VMAP_Status { get; set; }
        public string VMAP_Remarks { get; set; }
        public string VMAP_FromPlace { get; set; }
        public string VMAP_ToMeet { get; set; }
        public long VMAP_VisitorContactNo { get; set; }
        public DateTime? VMAP_MeetingDateTime { get; set; }
        public string MI_Name { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long VMMV_VisitorContactNo { get; set; }
        public string VMMV_VisitorEmailid { get; set; }
        public string VMMV_IdentityCardType { get; set; }
        public string VMMV_CardNo { get; set; }
        public string VMMV_CardImage { get; set; }
        public string VMMV_FromPlace { get; set; }
        public string VMMV_FromAddress { get; set; }
        public string VMMV_MeetingDuration { get; set; }
        public string VMMV_MeetingLocation { get; set; }
        public DateTime VMMV_MeetingDateTime { get; set; }
        public DateTime? VMMV_ExitDate { get; set; }
        public string VMMV_MeetingPurpose { get; set; }
        public string VMMV_PersonsAccompanying { get; set; }
        public string VMMV_AuthorisationBy { get; set; }
        public long VMMV_ToMeet { get; set; }
        public long UserId { get; set; }
        
      public string VMMV_QRCode { get; set; }
        public string ASCISS_QRCode { get; set; }
        public string VMMV_EntryDateTime { get; set; }
        public string VMMV_ExitDateTime { get; set; }
        public string VMMV_VisitTypeFlg { get; set; }
        public string VMMV_CkeckedInOutStatus { get; set; }
        public string meetpersion { get; set; }
        public string VMMV_Remarks { get; set; }
        public string VMMV_VisitorPhoto { get; set; }    
        public string VMMV_VisitorFingerPrint { get; set; }
        public bool VMMV_ActiveFlag { get; set; }
        public long? VMMV_UpdatedBy { get; set; }
        public long? VMMV_CreatedBy { get; set; }
        public string returnVal { get; set; }
        public string empname { get; set; }
        public string searchtype { get; set; }
        public bool value { get; set; }
        
        public Array editDetails { get; set; }
        public Array viewdetails { get; set; }
        public Array getmulti_visitors { get; set; }
        public Array getmultivisitordetails { get; set; }
        public string msg { get; set; }
        public Array vis_list { get; set; }
        public object visitorDetails { get; set; }
        public Array institution { get; set; }
        public Array editemplist { get; set; }
        public Array editemplistauthby { get; set; }
        public Array gridoptions { get; set; }
        public Array buildinglist { get; set; }
        public Array roomlist { get; set; }
        public int count { get; set; }
        public long count_subvisitors { get; set; }
        public bool VMMV_DeptFlag { get; set; }
        public string VMMV_PersonToMeet { get; set; }
        public long VMVAP_Id { get; set; }
        public Array emplist { get; set; }
        public Array emplistautjorizedby { get; set; }
        public Array visitorlist { get; set; }
        public Array assigned_visitorlist { get; set; }
        public Array visitornameschange { get; set; }
        public Array employeenameschange { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        public long VMVTMT_Id { get; set; }
        public string GPHS_OTP { get; set; }
        public string message { get; set; }
        public string genotp { get; set; }
        public string HRMEM_EmailId { get; set; }
        public string type { get; set; }
        public string VMVTMT_Remarks { get; set; }
        public string VMVTMT_Location { get; set; }
        public bool dept_flag { get; set; }
        public bool VMVTMT_MetFlg { get; set; }
        public DateTime VMVTMT_DateTime { get; set; }
        public Array institutionlist { get; set; }
        public Array getappvistiordetails { get; set; }
        public Array visitorList { get; set; }
        public string staffdeptflag { get; set; }
        public string searchfilter { get; set; }
        public Array VisiteSerchList { get; set; }
        public tomeet_tempDTO[] tomeet_tempDTO { get; set; }
        public authorizedby_tempDTO[] authorizedby_tempDTO { get; set; }
        public int fhrors { get; set; }
        public int fminutes { get; set; }
        public multivisitor[] multivisitor { get; set; }
        public Array extravisitor { get; set; }
        public Array visitorassigndetails { get; set; }
        public Array getAppointmentdetails { get; set; }
        public Array getAppointment_visitordetails { get; set; }
        public string QrCodeImage { get; set; }

      //  public string VMMV_QRCode { get; set; }


    }
    public class multivisitor
    {
        public long VMMVVI_Id { get; set; }
        public long VMMV_Id { get; set; }
        public string VMMVVI_VisitorName { get; set; }
        public string VMMVVI_VisitorAddress { get; set; }
        public string VMMVVI_VisitorEmailId { get; set; }
        public string VMMVVI_VisitorContactNo { get; set; }
        public string VMMVVI_Remarks { get; set; }
        public long VMMVVI_CreatedBy { get; set; }
        public DateTime VMMVVI_CreatedDate { get; set; }
        public long VMMVVI_UpdatedBy { get; set; }
        public DateTime VMMVVI_Updateddate { get; set; }
        public string VMMVVI_VisitorCardNo { get; set; }

    }
    public class tomeet_tempDTO
    {
        public long VMVTMT_Id { get; set; }
        public long MI_Id { get; set; }
        public long VMMV_Id { get; set; }
        public long VMVTMT_ToMeet_HRME_Id { get; set; }
        public long HRME_Id { get; set; }
    }
    public class authorizedby_tempDTO
    {
        public long VMVAUTH_Id { get; set; }
        public long MI_Id { get; set; }
        public long VMMV_Id { get; set; }
        public long HRME_Id { get; set; }
        public string VMVAUTH_Remarks { get; set; }
        public DateTime? VMVAUTH_DateTime { get; set; }
    }
}
