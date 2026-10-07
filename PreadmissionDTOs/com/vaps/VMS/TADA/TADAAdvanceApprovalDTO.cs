using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
    public class TADAAdvanceApprovalDTO
    {
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long VTADAA_Id { get; set; }
        public int HRPAON_SanctionLevelNo { get; set; }
        public long ISMMCLT_Id { get; set; }
        public Array TimeArray { get; set; }
        public long level { get; set; }
        public long VTADAAA_Id { get; set; }
        public long HRME_Id { get; set; }
        public string VTADAAA_Remarks { get; set; }
        public string VTADAA_StatusFlg { get; set; }
        public DateTime? VTADAA_FromDate { get; set; }
        public DateTime? VTADAA_ToDate { get; set; }
        public long VTADAA_ClientId { get; set; }
        public string VTADAA_ToAddress { get; set; }
        public string duplicate { get; set; }
        public string VTADAA_Remarks { get; set; }
        public decimal VTADAA_TotalAppliedAmount { get; set; }
        public decimal VTADAAA_TotalSactionedAmount { get; set; }
        public string EmplYoeeName { get; set; }
        public string VTADAAAA_StatusFlg { get; set; }
        public long IVRMMCT_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public int approvecnt { get; set; }
        public bool returnvalue { get; set; }
        public string returnval { get; set; }
        public string IVRMMCT_Name { get; set; }
        public Array City_Type { get; set; }
        public Array Client_Master { get; set; }
        public Array GetReport { get; set; }
        public Array Allowense { get; set; }
        public Array EditArray { get; set; }
        public Array multipleclientlist { get; set; }
        public Array EditArrayTwo { get; set; }
        public Array GetArray { get; set; }
        public Array getadvancetada { get; set; }
        public selecteddetails[] AllowenceArray { get; set; }
        public headarraydto[] headarray { get; set; }
        public string client_name { get; set; }
        public string client_remrks { get; set; }
        public Array TourPlanDetails { get; set; }
        public string IER_ID { get; set; }
        public Array editdataOthelist { get; set; }
        public Array GetPlanerdata { get; set; }
        public Array clientlist { get; set; }
        public Array GetCategoryArray { get; set; }
        public Array LeadList { get; set; }
        public long IMRC_CategoryId { get; set; }
        public string message { get; set; }
        public listalesApproval[] listEmployeeSales { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public string PlanName { get; set; }
        public string TourRemark { get; set; }
        public Array Priority { get; set; }
        public Array TourTravel_Mapiing { get; set; }
        public Array Categorywise_MinMax { get; set; }
        public Array ConcalLopdata { get; set; }
        public Array GpsLocationTada { get; set; }
        public bool? AdminFlag { get; set; }
        public class selecteddetails
        {

            public string Type { get; set; }
            public decimal Amount { get; set; }
            public string Remarks { get; set; }
            public long VTADAAD_Id { get; set; }
        }
        public RoutePlan_DepartureDetails[] Plan_DepartureDetails { get; set; }
    }

    public class listalesApproval
    {
        public long ClientId { get; set; }
        public long CategoryId { get; set; }
        public long userId { get; set; }
        public long ISMSMPR_Id { get; set; }
    }
    public class headarraydto
    {

        public long VTADAAAD_Id { get; set; }
        public long VTADAAA_Id { get; set; }
        public string VTADAAAD_ExpenditureHead { get; set; }
        public decimal VTADAAAD_Amount { get; set; }
        public string VTADAAAD_Remarks { get; set; }
        public string flag { get; set; }
        public decimal VTADAAAAH_SactionedAmount { get; set; }

    }
    public class RoutePlan_DepartureDetails
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
}
