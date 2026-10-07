using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class VMS_Receipt_VoucherDTO
    {
        public long VRECVOU_Id { get; set; }
        public long MI_Id { get; set; }
        public long asmaY_Id { get; set; }
        public bool VRECVOU_FromClientFlg { get; set; }
        public long? ISMMCLT_Id { get; set; }
        public long HRMBD_Id { get; set; }
        public string VRECVOU_ReceiptMode { get; set; }
        public string VRECVOU_ReceiptReference { get; set; }
        public string VRECVOU_VoucherNo { get; set; }
        public string VRECVOU_ReceivedFrom { get; set; }
        public string VRECVOU_ChequeDDNo { get; set; }
        public string VRECVOU_Remarks { get; set; }
        public decimal VRECVOU_Amount { get; set; }
        public bool VRECVOU_ActiveFlg { get; set; }
        public long UserId { get; set; }
        public Array allcompany { get; set; }
        public Array banklist { get; set; }
        public Array clientlist { get; set; }
        public Array ModeOfPayment { get; set; }
        public string returnval { get; set; }
        public decimal? VBABRS_ActualBankBalance { get; set; }
        public long VBABRS_Id { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string trans_id { get; set; }
        public Array alldata { get; set; }
        public long? VCBSRC_Id { get; set; }
        public long? ISMCPPD_Id { get; set; }
        public Array checkbookSource { get; set; }
        public BankList[] Banklistdetails { get; set; }
        public Array allReport { get; set; }
        public string optionflag { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public string logo { get; set; }
        public DateTime? VBABRS_ClearedDate { get; set; }
        public DateTime? VRECVOU_Date { get; set; }
        public bool VBABRS_ClearedFlg { get; set; }
    }
    public class BankList
    {
        public long HRMD_Id { get; set; }
    }
    //RecieptReport
}
