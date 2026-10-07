using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
    public class TADA_Advance_ApplyDTO
    {
        public long VTADAAA_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long IVRMMS_Id { get; set; }
        public Array City_Type { get; set; }
        public long IVRMMCT_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string IVRMMCT_Name { get; set; }
        public string EmplYoeeName { get; set; }
        public Array Client_Master { get; set; }
        public Array Allowense { get; set; }
        public string returnval { get; set; }
        public DateTime? VTADAAA_FromDate { get; set; }
        public DateTime? VTADAAA_ToDate { get; set; }
        public long VTADAAA_ClientId { get; set; }
        public string VTADAAA_StatusFlg { get; set; }
        public decimal VTADAAA_TotalAppliedAmount { get; set; }
        public decimal VTADAAA_TotalSactionedAmount { get; set; }
        public decimal VTADAAA_TotalPaidAmount { get; set; }
        public DateTime? VTADAAA_CreatedDate { get; set; }
        public string VTADAAA_ToAddress { get; set; }
        public Array ViewDetails { get; set; }
        public string IVRMMS_Name { get; set; }
        public string VTADAAA_Remarks { get; set; }
        public string cc_Mail { get; set; }
        public long HRME_Id { get; set; }
        public bool returnvalue { get; set; }
        public Array GetReport { get; set; }
        public selecteddetails[] AllowenceArray { get; set; }
        public multiple_clientDTO[] multiple_client { get; set; }
        public Array EditArray { get; set; }
        public Array editmuticlientarray { get; set; }
        public Array EditArrayTwo { get; set; }
        public Array state { get; set; }
        public string duplicate { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string MI_Name { get; set; }
        public int HRPAON_SanctionLevelNo { get; set; }
        public long IVRMUL_Id { get; set; }
        public string VTADAAA_DepartureTime { get; set; }
        public string VTADAAA_ArrivalTime { get; set; }
        public decimal? VTADACM_TravelHour { get; set; }
        public bool VTADAAA_ActiveFlg { get; set; }
        public Array masterCountry { get; set; }
        public string VTADAAA_ClientMultiple { get; set; }
        public bool CountBalance { get; set; }
        public class selecteddetails
        {

            public string Type { get; set; }
            public decimal Amount { get; set; }
            public string Remarks { get; set; }
            public long VTADAAAD_Id { get; set; }
            public decimal? VTADAAAD_Slots { get; set; }

            public decimal? VTADAAAD_TotalSlots { get; set; }

        }
        public class multiple_clientDTO
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
    }
}
