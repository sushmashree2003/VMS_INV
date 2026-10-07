using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_Master_ClientProject_PaymentDTO
    {
        public long ISMCLTPRP_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public long MI_Id { get; set; }
        public string institutioncode { get; set; }
        public long FMI_Id { get; set; }
        public string FMI_Name { get; set; }
        public long? FTI_Id { get; set; }
        public string FTI_Name { get; set; }
        public long UserId { get; set; }
        public long ISMCLTPRP_Year { get; set; }
        public string ISMCLTPRP_InstallmentName { get; set; }
        public decimal? ISMCLTPRP_BalanceAmt { get; set; }
        public decimal? ISMCLTPRP_ReceivedAmt { get; set; }
        public string paymentstatusflag { get; set; }
        public decimal? ISMCLTPRP_InstallmentAmt { get; set; }
        public DateTime? ISMCLTPRP_PaymentDate { get; set; }
        public bool ISMCLTPRP_ActiveFlag { get; set; }
        public long ISMCLTPRP_CreatedBy { get; set; }
        public long ISMCLTPRP_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMMPR_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long asmaY_Id { get; set; }
        public string ISMMPR_ProjectName { get; set; }
        public string ASMAY_Year { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string msg { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array yearlist { get; set; }
        public Array projectlist { get; set; }
        public Array alldata { get; set; }
        public Array editlist { get; set; }
        public Array clientlist { get; set; }
        public long cltprid { get; set; }
        public Array getinstallment { get; set; }
        public Array master_bankdetails { get; set; }
        public Array geteditclient { get; set; }
        public Array geteditclientprojectid { get; set; }
        public Array paymentmodedetails { get; set; }
        public Array getpaymentdetails { get; set; }
        public Array getpaymentnotificationdetails { get; set; }

        //public long prjprid { get; set; }

        // PAYMENT DETAILS DTO
        public long ISMCPPD_Id { get; set; }
        public long flag { get; set; }
        public decimal? ISMCPPD_ReceivedAmount { get; set; }
        public DateTime? ISMCPPD_ReceivedDate { get; set; }
        public long IVRMMOD_Id { get; set; }
        public string ISMCPPD_PaymentRefNo { get; set; }
        public string ISMCPPD_Remarks { get; set; }
        public DateTime? ISMCPPD_ChequeDate { get; set; }
        public ISM_Client_Project_Payment_Details[] ISM_Client_Project_Payment_Details { get; set; }

        // CONFIGURATION DTO
        public long ISMCPC_Id { get; set; }
        public long ISMCPC_RemainderDays { get; set; }
        public string ISMCPC_FullORPartialPayment { get; set; }
        public Array getconfigloaddata { get; set; }

        // PAYMENT NOTIFICATION DTO
        public long? IVRM_MI_Id { get; set; }
        public string ISMMCLT_ClientCode { get; set; }
        public Array getreportdetails { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public long? HRMBD_Id { get; set; }       
        public decimal? previousamount { get; set; }
    }

    public class ISM_Client_Project_Payment_Details
    {
        public long ISMCPPD_Id { get; set; }
        public long flag { get; set; }
        public long? HRMBD_Id { get; set; }
        public decimal? ISMCPPD_ReceivedAmount { get; set; }
        public DateTime? ISMCPPD_ReceivedDate { get; set; }
        public decimal? ISMCPPD_TDSAmount { get; set; }
        public long IVRMMOD_Id { get; set; }
        public string ISMCPPD_PaymentRefNo { get; set; }
        public string ISMCPPD_Remarks { get; set; }
        public DateTime? ISMCPPD_ChequeDate { get; set; }
    }
}
