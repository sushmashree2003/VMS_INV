using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Payment_Voucher")]
    public class VMS_Payment_VoucherDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VPAYVOU_Id { get; set; }
        public long MI_Id { get; set; }
        public long? INVMS_Id { get; set; }     
        public DateTime VPAYVOU_PaymentDate { get; set; }
        public decimal VPAYVOU_AppliedAmount { get; set; }
        public decimal VPAYVOU_ApprovedAmount { get; set; }
        public string VPAYVOU_Remarks { get; set; }
        public decimal? VPAYVOU_BankBalalnce { get; set; }
        public string VPAYVOU_StatusFlg { get; set; }
        public bool VPAYVOU_ChequePrintedFlg { get; set; }
        public DateTime? VPAYVOU_ChequePrintedDate { get; set; }
        public bool VPAYVOU_ActiveFlg { get; set; }
        public long VPAYVOU_CreatedBy { get; set; }
        public DateTime VPAYVOU_CreatedDate { get; set; }
        public long VPAYVOU_UpdatedBy { get; set; }
        public DateTime VPAYVOU_UpdatedDate { get; set; }
        public string VPAYVOU_BankAccountNo { get; set; }
        public string VPAYVOU_VoucherNo { get; set; }
        public long HRMBD_Id { get; set; }
        public string VPAYVOU_ChequeNo { get; set; }
        public decimal? VPAYVOU_TDS { get; set; }
        public decimal? VPAYVOU_Penalty { get; set; }
        public string VPAYVOU_EmailOTP { get; set; }
        public string VPAYVOU_ToSupplierFlg { get; set; }
        public bool VPAYVOU_SupplierFlg { get; set; }
        public string VPAYVOU_PaymentMode { get; set; }
        public string VPAYVOU_PaymentReference { get; set; }
        public bool VPAYVOU_SelfPayFlg { get; set; }
        public string VPAYVOU_PaymentTo { get; set; }
        public decimal? VPAYVOU_ESICAmount { get; set; }
        public decimal? VPAYVOU_OtherAmount { get; set; }
        public bool? HRMBD_InterbankTransFlg { get; set; }
        public long? VPAYVOU_HRMBD_Id { get; set; }
        //public long? VMBDGHD_Id { get; set; }
        public List<BankAccount_BRS_FilesDMO> BankAccount_BRS_FilesDMO { get; set; }               
        public List<VMS_Payment_Voucher_Supplier_GRNDMO> VMS_Payment_Voucher_Supplier_GRNDMO { get; set; }
        public List<VMS_Payment_Voucher_BudgetDMO> VMS_Payment_Voucher_BudgetDMO { get; set; }

    }
}
