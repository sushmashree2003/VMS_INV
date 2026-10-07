using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
    public class TADAApprovalDTO
    {
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long VTADAA_Id { get; set; }
        public int HRPAON_SanctionLevelNo { get; set; }
        public long ISMMCLT_Id { get; set; }
        public Array TimeArray { get; set; } //added by adarsh
        public Array multipleclientlist { get; set; }
        public string client_name { get; set; }
        public string client_remrks { get; set; }//end .MS
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
        public decimal VTADAA_TotalSactionedAmount { get; set; }
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
        public Array EditArrayTwo { get; set; }
        public Array GetArray { get; set; }
        public Array getadvancetada { get; set; }
        public Array GetFile { get; set; }
        public selecteddetails[] AllowenceArray { get; set; }
        public headarraydtow[] headarray { get; set; }
        public Array Veiwdetails { get; set; }
        public long VTADAAAAA_Id { get; set; }
        public Array getsaveddetails { get; set; }
        public Array filldata { get; set; }
        public Array empwise_statuslist { get; set; }
        public string DRApprovalTypeFlag { get; set; }
        public class selecteddetails
        {

            public string Type { get; set; }
            public decimal Amount { get; set; }
            public string Remarks { get; set; }
            public long VTADAAD_Id { get; set; }
        }
        public ApprovaldocfiledocDTO[] filistarray { get; set; }
        public bool? VTADAA_ApprovedFlg { get; set; }
        public string MailSubject { get; set; }
        public string Mailmsg { get; set; }
    }



    public class headarraydtow
    {

        public long VTADAAD_Id { get; set; }
        public long VTADAAA_Id { get; set; }
        public string VTADAAAD_ExpenditureHead { get; set; }
        public decimal VTADAAAD_Amount { get; set; }
        public string VTADAAAD_Remarks { get; set; }
        public string flag { get; set; }
        public decimal VTADAAAH_SactionedAmount { get; set; }

    }
    public class ApprovaldocfiledocDTO
    {


        public string VTADAAF_FileName { get; set; }
        public string VTADAAF_Filedesc { get; set; }
        public string VTADAAF_FilePath { get; set; }
        public string VTADAAF_Remarks { get; set; }
        public string VTADAAF_VerfieRemarks { get; set; }
        public bool? VTADAAF_ApprovedFlg { get; set; }
        public string Flag { get; set; }
        public long VTADAAF_Id { get; set; }
    }
}
