using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class CheckBankBalanceDTO
    {
        public long ISMCLTPRP_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public long VPAYVOU_Id { get; set; }
        public long VMCHEBK_Id { get; set; }
        public long MI_Id { get; set; }
        public string institutioncode { get; set; }
        public long FMI_Id { get; set; }
        public decimal ISMCLTPRBOM_Qty { get; set; }
        public decimal ISMINC_AdvPer { get; set; }
        public decimal ISMINC_AdvanceAmount { get; set; }
        public long ISMCLTC_Id { get; set; }
        public long VBABRS_Id { get; set; }
        public long? VCBSRC_Id { get; set; }
        public string optionflag { get; set; }
        public Array CheckbookSource { get; set; }
        public string VBABRS_RefrenceId { get; set; }
        public DateTime VBABRS_Date { get; set; }
        public decimal VBABRS_Amount { get; set; }
        public decimal VBABRS_Amounttwo { get; set; }
        public string VBABRS_CRDRFlg { get; set; }
        public string VBABRS_Remarks { get; set; }
        public bool VBABRS_ClearedFlg { get; set; }
        public decimal? VBABRS_BankBalalnce { get; set; }
        public DateTime? VBABRS_ClearedDate { get; set; }
        public Array state_list { get; set; }
        public long INVMS_Id { get; set; }
        public DateTime VPAYVOU_PaymentDate { get; set; }
        public decimal VPAYVOU_AppliedAmount { get; set; }
        public decimal VPAYVOU_ApprovedAmount { get; set; }
        public string VPAYVOU_Remarks { get; set; }
        public string INVMS_SupplierName { get; set; }
        public decimal VPAYVOU_BankBalalnce { get; set; }
        public string VPAYVOU_StatusFlg { get; set; }
        public bool VPAYVOU_ChequePrintedFlg { get; set; }
        public DateTime? VPAYVOU_ChequePrintedDate { get; set; }


        public string ISMCLTC_Name { get; set; }
        public string trans_id { get; set; }
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
        public long HRMBD_Id { get; set; }
        public long HRMBD_Idtwo { get; set; }
        public DateTime? ISMINC_MOUDate { get; set; }
        public string ISMINC_MOURefNo { get; set; }
        public string ISMINC_ModeOfPayment { get; set; }

        public string ISMINC_WorkOrder { get; set; }
        public string ISMINC_PrInviceNo { get; set; }
        public decimal ISMINC_TotalTaxAmount { get; set; }
        public decimal ISMINC_TotalAmount { get; set; }
        public string ISMINC_Remarks { get; set; }
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
        public string returnvaltwo { get; set; }
        public Array instlist { get; set; }
        public Array allcompany { get; set; }
        public Array modeofpaymentlist { get; set; }
        public Array banklist { get; set; }
        public Array banklistTwo { get; set; }
        public Array yearlist { get; set; }
        public Array ModeOfPayment { get; set; }
        public Array alldatatwo { get; set; }
        public Array get_supplier { get; set; }
        public Array bomlist { get; set; }
        public Array projectlist { get; set; }
        public Array projectlistTWO { get; set; }
        public Array alldata { get; set; }
        public Array editlist { get; set; }
        public Array editlistdetails { get; set; }
        public Array checckleafTotal { get; set; }
        public Array checkDetails { get; set; }
        public Array checkBooklist { get; set; }
        public Array Checkbookgrn { get; set; }
        public Array clientlist { get; set; }
        public long cltprid { get; set; }
        public Array getinstallment { get; set; }
        public Array geteditclient { get; set; }
        public Array geteditclientprojectid { get; set; }
        public Array getpaymentnotificationdetails { get; set; }
        public Array sourcelist { get; set; }
        public long VBABRSouce_Id { get; set; }
        public string VBABRSouce { get; set; }
        public long ISMCPPD_Id { get; set; }
        public long flag { get; set; }
        public decimal? ISMCPPD_ReceivedAmount { get; set; }
        public DateTime? ISMCPPD_ReceivedDate { get; set; }
        public long IVRMMOD_Id { get; set; }
        public string ISMCPPD_PaymentRefNo { get; set; }
        public string ISMCPPD_Remarks { get; set; }
        public DateTime? ISMCPPD_ChequeDate { get; set; }
        // CONFIGURATION DTO
        public long ISMCPC_Id { get; set; }
        public long ISMCPC_RemainderDays { get; set; }
        public string ISMCPC_FullORPartialPayment { get; set; }
        public Array getconfigloaddata { get; set; }
        public Array TrasactionReport { get; set; }

        // PAYMENT NOTIFICATION DTO
        public long? IVRM_MI_Id { get; set; }
        public string ISMMCLT_ClientCode { get; set; }
        public Array getreportdetails { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public itemsdto1[] itemsdto { get; set; }
        public taxdto1[] taxdto { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public Array MasterChecks { get; set; }
        public clearFlasOne[] cleararrflag { get; set; }
        //
    }

    public class clearFlasOne
    {
        public long VBABRS_Id { get; set; }
        public DateTime? VBABRS_ClearedDateone { get; set; }
        public long VPAYVOU_Id { get; set; }
    }
}
