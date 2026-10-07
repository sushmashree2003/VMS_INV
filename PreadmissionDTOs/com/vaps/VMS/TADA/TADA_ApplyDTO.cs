using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
    public class TADA_ApplyDTO
    {
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long VTADAA_Id { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string MI_Name { get; set; }
        public long? VTADAAA_Id { get; set; }
        public long HRME_Id { get; set; }
        public string VTADAA_StatusFlg { get; set; }
        public DateTime? VTADAA_FromDate { get; set; }
        public DateTime? VTADAA_ToDate { get; set; }
        public DateTime? VTADAAA_CreatedDate { get; set; }
        public DateTime? VTADAA_CreatedDate { get; set; }
        public long VTADAA_ClientId { get; set; }
        public string VTADAA_ToAddress { get; set; }
        public string duplicate { get; set; }
        public string duplicatetwo { get; set; }
        public string VTADAA_Remarks { get; set; }
        public string IVRMMS_Name { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public decimal VTADAA_TotalAppliedAmount { get; set; }
        public decimal VTADAA_TotalSactionedAmount { get; set; }
        public decimal VTADAA_TotalPaidAmount { get; set; }
        public string EmplYoeeName { get; set; }
        public long IVRMMCT_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public bool returnvalue { get; set; }
        public string returnval { get; set; }
        public string IVRMMCT_Name { get; set; }
        public Array City_Type { get; set; }
        public Array Client_Master { get; set; }
        public Array Travelcity { get; set; }
        
        public Array GetReport { get; set; }
        public Array Allowense { get; set; }
        public Array EditArray { get; set; }
        public Array EditArrayTwo { get; set; }
        public Array EditArryThree { get; set; }
        public Array tada_multipleclient { get; set; }
        public Array EditFour { get; set; }
        public Array EditFive { get; set; }
        public Array Travel_data { get; set; }
        public long VTADAMT_Id { get; set; }
        public Array Travel_Details { get; set; }
        public long? Id { get; set; }
        public Array GetArray { get; set; }
        public Array Viewdetails { get; set; }
        public Array state { get; set; }
        public long VTADAMCF_Id { get; set; }
       
        public selecteddetails[] AllowenceArray { get; set; }
        public MasterplannerfiledocDTO[] filelist { get; set; }
        public multiple_clientDTO[] multiple_client { get; set; }
        public Array masterCountry { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string CC_Mail { get; set; }
        public long Fromstate { get; set; }
        public long Tostate { get; set; }
        public Array TravelAmount { get; set; }
        
        public long VTADAMD_Id { get; set; }
        
            
        
            
        public string VTADAA_DepartureTime { get; set; }
        public string VTADAA_ArrivalTime { get; set; }
        public bool VTADAA_ActiveFlg { get; set; }
        public string VTADAAA_ClientMultiple { get; set; }
        public string VTADAA_BalanceHRMS { get; set; }
        public bool? VTADAA_Finaldocument { get; set; }
        public Array UplodaArray { get; set; }
        public bool? VTADAA_ApprovedFlg { get; set; }
        public string MailSubject { get; set; }
        public string Mailmsg { get; set; }
        public VMSTADADTO_New[] VMSTADAAmountDTO { get; set; }
        public class MasterplannerfiledocDTO
        {

            public string VTADAAF_FileName { get; set; }
            public string VTADAAF_Filedesc { get; set; }
            public string VTADAAF_FilePath { get; set; }
            public string VTADAAF_Remarks { get; set; }
            public string VTADAAF_VerfieRemarks { get; set; }
            public bool? VTADAAF_ApprovedFlg { get; set; }
            public long VTADAAF_Id { get; set; }
        }
        public class selecteddetails
        {

            public string Type { get; set; }
            public decimal Amount { get; set; }
            public string Remarks { get; set; }
            public long VTADAAD_Id { get; set; }

            public decimal? VTADAAD_Totalslots { get; set; }
            public decimal? VTADAAD_Slots { get; set; }

        }
        public class multiple_clientDTO
        {
            public long VTADAMCF_Id { get; set; }
            public long VTADAA_Id { get; set; }
            public long ISMMCLT_Id { get; set; }
            public string VTADAMCF_Remarks { get; set; }
            public string VTADAMCF_StatusFlg { get; set; }
            public bool VTADAMCF_ActiveFlag { get; set; }
            public DateTime VTADAMCF_CreatedDate { get; set; }
            public DateTime VTADAMCF_UpdatedDate { get; set; }
            public long VTADAMCF_CreatedBy { get; set; }
            public long VTADAMCF_UpdatedBy { get; set; }


        }
        public class VMSTADADTO_New
        {

            public long VTADAAAC_Id { get; set; }
            public long VTADAA_Id { get; set; }
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
    }

}

