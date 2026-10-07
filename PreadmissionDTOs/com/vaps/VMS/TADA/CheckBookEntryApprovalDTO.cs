using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
   public class CheckBookEntryApprovalDTO
    {
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long VPAYVOU_Id { get; set; }
        public int HRPAON_SanctionLevelNo { get; set; }
        public string EMAILOTP { get; set; }
        public Array GRnDetails { get; set; }
        public string password { get; set; }
        public long level { get; set; }
       
        public long HRME_Id { get; set; }
        public string VTADAAA_Remarks { get; set; }
        public string VTADAA_StatusFlg { get; set; }
        public DateTime? VTADAA_FromDate { get; set; }
        public DateTime? VTADAA_ToDate { get; set; }
        public string message { get;set; }
     
        public long VTADAA_ClientId { get; set; }
        public string VTADAA_ToAddress { get; set; }
        public string duplicate { get; set; }
        public string VPAYVOU_Remarks { get; set; }
        public string EMAIL_Id { get; set; }
        public decimal VTADAA_TotalAppliedAmount { get; set; }
        public decimal VPAYVOUAUT_SanctionedAmount { get; set; }
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
        public Array filetwo { get; set; }
        public Array GetArray { get; set; }
        public Array getadvancetada { get; set; }
       public Array allcompany { get; set; }


        public long VPAYVOUAUT_OTP { get; set; }
      

    }


   
    
}
