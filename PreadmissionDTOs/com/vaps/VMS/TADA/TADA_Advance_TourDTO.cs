using System;
using System.Collections.Generic;
using System.Text;
using static PreadmissionDTOs.com.vaps.VMS.TADA.TADA_Advance_ApplyDTO;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
   public  class TADA_Advance_TourDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public long VRMMDT_Id { get; set; }
        public Array GetCategoryArray { get; set; }
        public long IMRC_CategoryId { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public Array LeadList { get; set; }
        public listEmployeeSales [] listEmployeeSales { get; set; }
        public string message { get; set; }
        public Array getloaddata { get; set; }
        public string PlanName { get; set; }
        public string TourRemark { get; set; }
        public Array GetPlanerdata { get; set; }
        public long IER_ID { get; set; }
        public Array editdata { get; set; }
        public Array editdataOthelist { get; set; }
        public string IERA_StatusFlg { get; set; }
        //added by adarsh 
        public Array getloadarray { get; set; }
        public Array leadroutestatusremark { get; set; }
        public string IER_Remarks { get; set; }
        public string VMSATGPSL_LATLONGValue { get; set; }
        public string ISMSLEDM_DemoExecuted { get; set; }
        public string ILRR_Lat { get; set; }
        public string ILRR_Long { get; set; }
        public Array GetAdavancedata { get; set; }
        public ISM_RoutePlan_DepartureDetails[] Plan_DepartureDetails { get; set; }
        public UploadRouteplanDTO[] filelist { get; set; }
        public Array clientlist { get; set; }
        public bool returnvalue { get; set; }
        public long VTADAAA_Id { get; set; }
        public string returnval { get; set; }
        public string duplicate { get; set; }
        public decimal VTADAAA_TotalAppliedAmount { get; set; }
        public string cc_Mail { get; set; }
        public Tada_tourdetails[] AllowenceArray { get; set; }
        public Tadamultiple_clientDTO[] multiple_client { get; set; }
        public Array Tour_Details { get; set; }
        public Array designationArray { get; set; }
        public Array StateList { get; set; }
        public Array Travel_data { get; set; }
        public Array Travel_Details { get; set; }
        public long VTADAMT_Id { get; set; }
        public long Fromstate { get; set; }
        public long Tostate { get; set; }
        public long VTADAMD_Id { get; set; }
        public Array TravelAmount { get; set; }
        public VMSTADAAmountDTO[] VMSTADAAmountDTO { get; set; }
        public long IERC_ID { get; set; }
        public Array EditArray { get; set; }
        public Array TimeArray { get; set; }
        public Array multipleclientlist { get; set; }
        public Array EditArrayTwo { get; set; }
        public Array GetArray { get; set; }
        public Array TourPlanDetails { get; set; }
        public string duplicatetwo { get; set; }
        public long VTADAA_Id { get; set; }
        public string CC_Mail { get; set; }
        public string Mail_CC { get; set; }
        public TourplannerfiledocDTO[] filelistNew { get; set; }
        public string VTADAA_Remarks { get; set; }
        public decimal VTADAA_TotalAppliedAmount { get; set; }
        public string VTADAA_BalanceHRMS { get; set; }
        public Array LeadLists { get; set; }
        public TADA_demo_master_temp[] product_demo_master_temp2 { get; set; }
        public string return_status { get; set; }
        public Array status_demo_master { get; set; }
        public Array view_lead_demo { get; set; }
        public long ISMSLEDM_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public Array product_dd_s { get; set; }
        public long viewall { get; set; }
        public Array demo_response_details { get; set; }
        public Array FinalSumbmission { get; set; }
        public Array  ApprovalSubmission { get; set; }
        public string VTADAA_BalanceConcal { get; set; }
        public long ISMSLE_STUDENTSTRENGTH { get; set; }
        public Array ConcalLopdata { get; set; }
        public Array GpsLocationTada { get; set; }
        public Array Getdesignation { get; set; }
        public long? HRMDES_Id { get; set; }

        public lead[] leaddetails { get; set; }

    }
    public class TADA_demo_master_temp
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
        //--
        public bool ISMSLECOM_MoMFlg { get; set; }
        public bool ISMSLECOM_NonFlg { get; set; }
        public string NON_Amount { get; set; }
        public bool ISMSLECOM_CancelFlg { get; set; }
        public long SMSLEDM_CategaroyId { get; set; }
        //--
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSLEDMPR_CreatedBy { get; set; }
        public long ISMSLEDMPR_UpdatedBy { get; set; }
        public string ISMSMST_StatusName { get; set; }
        public long IER_ID { get; set; }
        public string ISMSMPR_ProductName { get; set; }
        public long? ISMSBOM_Id { get; set; }
    }
    public class listEmployeeSales
    {
        public long ClientId { get; set; }
        public long CategoryId { get; set; }
        public long userId { get; set; }
        public long ISMSMPR_Id { get; set; }
    }
    public class TourplannerfiledocDTO
    {

        public string VTADAAF_FileName { get; set; }
        public string VTADAAF_Filedesc { get; set; }
        public string VTADAAF_FilePath { get; set; }
        public string VTADAAF_Remarks { get; set; }
        public string VTADAAF_VerfieRemarks { get; set; }
        public bool? VTADAAF_ApprovedFlg { get; set; }
        public long VTADAAF_Id { get; set; }
        public string VTADAAF_Cost { get; set; }
    }

    public class Tada_tourdetails
    {

        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public long VTADAAAD_Id { get; set; }
        public decimal? VTADAAAD_Slots { get; set; }

        public decimal? VTADAAAD_TotalSlots { get; set; }
        
    }
    public class ISM_RoutePlan_DepartureDetails
    {
       public long IRPD_ID { get; set; }
        public long IER_ID { get; set; }
        public long IVRMMS_Id { get; set; }
        public DateTime? IRPD_DepartureDate { get; set; }
        public DateTime? IRPD_ArrivalDate { get; set; }
        public string IRPD_DepartureTime { get; set; }
        public string IRPD_ArrivalTime { get; set; }
        public long? IRPD_Createdby { get; set; }
        public DateTime? IRPD_CreatedDate { get; set; }
        public long? IRPD_Updateby { get; set; }
        public DateTime? IRPD_UpdateDate { get; set; }
        public int? IRPD_Order { get; set; }
    }
    public class Tadamultiple_clientDTO
    {
        public long VTADDAMCF_Id { get; set; }
        public long VTADAAA_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public string VTADDAMCF_Remarks { get; set; }
        public string VTADDAMCF_StatusFlg { get; set; }
        public bool VTADDAMCF_ActiveFlag { get; set; }
        public DateTime VTADDAMCF_CreatedDate { get; set; }
        public DateTime VTADDAMCF_UpdatedDate { get; set; }
        public long VTADDAMCF_CreatedBy { get; set; }
        public long VTADDAMCF_UpdatedBy { get; set; }
    }
    public class UploadRouteplanDTO
    {
        public string ILRT_FilePath { get; set; }
        public string ILRT_FileName { get; set; }
        public string ILRT_Remarks { get; set; }
        public decimal ILRT_Cost { get; set; }
        public long IERC_ID { get; set; }
        public long HRME_Id { get; set; }
    }
    public class VMSTADAAmountDTO
    {

        public long VTADAAAC_Id { get; set; }
        public long VTADAAA_Id { get; set; }
        public string VTADAAAC_FromState { get; set; }
        public string VTADAAAC_ToState { get; set; }
        public decimal VTADAAAC_Amount { get; set; }
        public long VTADAAAC_CreatedBy { get; set; }
        public long VTADAAAC_UpdatedBy { get; set; }
        public DateTime VTADAAAC_CreatedDate { get; set; }
        public DateTime VTADAAAC_UpdatedDate { get; set; }
        public long HRMDES_Id { get; set; }
        public long VTADAMT_Id { get; set; }
        public long VTADAMD_Id { get; set; }
        public long IVRMMCT_Id { get; set; }
    }


    public class lead {

        public long? ISMSLE_Id { get; set; }

        public long? MI_Id { get; set; }

        public string ISMSLE_LeadName { get; set; }

        public string ISMSLE_ContactPerson { get; set; }
        public long ISMSLE_ContactNo { get; set; }
        public string ISMSLE_LeadAddress1 { get; set; }
        public string ISMSLE_LeadAddress2 { get; set; }
        public string ISMSLE_LeadAddress3 { get; set; }

        public string ISMSLE_EmailId { get; set; }

        public string ISMSLE_ContactDesignation { get; set; }


        public long ISMSLE_StudentStrength { get; set; }



    }

}
