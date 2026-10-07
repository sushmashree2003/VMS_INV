using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Payment_Voucher_Authorisation")]
    public class VMS_Payment_Voucher_AuthorisationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VPAYVOUAUT_Id { get; set; }
        public long VPAYVOU_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long VPAYVOUAUT_SanctioningLevel { get; set; }
        public decimal VPAYVOUAUT_SanctionedAmount { get; set; }
        public string VPAYVOUAUT_Remarks { get; set; }
        public string VPAYVOUAUT_StatusFlg { get; set; }
        public DateTime VPAYVOUAUT_Date { get; set; }
        public bool VPAYVOUAUT_ActiveFlg { get; set; }
        public long VPAYVOUAUT_CreatedBy { get; set; }
        public DateTime VPAYVOUAUT_CreatedDate { get; set; }
        public long VPAYVOUAUT_UpdatedBy { get; set; }
        public DateTime VPAYVOUAUT_UpdatedDate { get; set; }
        public long MI_Id { get; set; }
        public long VPAYVOUAUT_OTP { get; set; }

    }
}
